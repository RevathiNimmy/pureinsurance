EXECUTE DDLDropProcedure 'spu_ACT_Get_Unallocated_Claim_Payments_By_PaymentDate'
GO
CREATE PROCEDURE spu_ACT_Get_Unallocated_Claim_Payments_By_PaymentDate 
    @date_of_payment datetime,  
    @date_of_payment_To datetime = NULL,
	@AgentKey INT=0
AS  
  
BEGIN  
  
-- get the claim payable account id  
DECLARE @Claim_Payable_Account_Id int  
SELECT @claim_payable_account_id = account_id FROM account WHERE short_code = 'CLMPAYABLE'  
  
-- this query will return data via a union  
-- the union consists of  
-- any document linked to a standard claim payment party account (AGENTS, CLIENT, OTHER PARTIES)  
-- NB: Parties of Type "Insurer" are explicitly excluded (as these might  
-- be present on a payment as part of the reinsurance)  
-- UNION with  
-- any documents linked to a CLMPAYABLE account  
  
 SELECT  
  td.document_id,  
  doc.document_ref,  
  SUM(td.Currency_amount) as currency_amount,  
  td.currency_id as currency_id,  
  SUM(td.Amount) as amount,  
--Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.0)  
  td.amount_currency_id as amount_currency_id,  
--End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.0)  
  SUM(td.Account_amount) as account_amount,  
  td.account_currency_id as account_currency_id,  
  MAX(c.claim_number) as claim_number,  
  doc.comment,  
  MIN(currency.description) currency_description,  
  MIN(currency.format_string) currency_format_string,  
  cp.date_of_payment,  
--Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.0)  
  cp.payeemediatype payee_media_type,  
--End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.0)  
  MIN(base.description) base_currency_description,  
  MIN(base.format_string) base_format_string,  
--Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.0)  
  MAX(cp.claim_payment_id) as claim_payment_id,  
--End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.0)  
  doc.document_date,  
  account.account_id,  
  cp.base_claim_payment_id,  
  MIN(account.account_name) account_name,  
  
    Case MIN(CLIPS.Code) When  'PENDING'THEN 'Awaiting Settlement Authorisation'  
        ELSE 'Awaiting Settlement'  
    END Status,  
    MIN(MT.Description) MediaTypeDesc,  
    MIN(S.Description) Branch,  
    MIN(BA.Bank_Account_Name)BankAccount,  
    MIN(cp.PayeeMediaType)MediaType,  
    MIN(P.BankAccount_Id) BankAccountID,  
 MIN(doc.document_date) document_date,  
 NULL account_id,  
 cp.base_claim_payment_id,  
 min(Account.account_name) account_name  
 ,MIN(CLICL.CashListItem_id) CashListItem_id,  
    cp.our_ref,  
    cp.PayeeName,  
    MIN(mt.code) MediaTypeCode,  
    MIN(currency.code) CurrencyCode,  
    MIN(BA.code) BankAccountCode,  
    MIN(account.short_code) AccountCode,  
    MIN(s.code) BranchCode,  
    cp.ThirdPartyReference,  
    cp.PayeeSortCode,  
    cp.PayeeAccountNo,  
    cp.party_bank_id PartyBankId,  
    MIN(is_reserved) is_reversed,  
 td.currency_base_xrate,
 MIN(cp.Media_ref) Media_ref
  
   FROM transdetail td  
  
  INNER JOIN (SELECT * FROM document WHERE document_ref like 'CLP%')  doc ON  
   td.document_id = doc.document_id  
  
    LEFT JOIN Claim_Payment CP ON  
    cp.document_id  = td.document_id  
  
    LEFT JOIN claim c ON  
     c.claim_id = cp.claim_id  
  
LEFT JOIN Insurance_File IFILE ON  
    C.Policy_ID = IFILE.Insurance_file_Cnt  
  
    JOIN Product P  
    ON IFILE.Product_ID = P.Product_ID  
  
    LEFT Join BankAccount BA ON  
    P.BankAccount_ID= BA.BankAccount_ID  
  
    JOIN Source S  
    ON IFILE.Source_ID = S.Source_ID  
    LEFT JOIN MediaType MT ON  
    MT.MediaType_id = cp.PayeeMediaType  
  
  INNER JOIN (Select * from Account where account_key <> 0) Account ON  
   td.account_id = account.account_id  
  
   INNER JOIN Party ON  
    party.party_cnt = account.account_key  
  
    INNER JOIN (select party_type_id from Party_Type where party_type_id <> 7) party_type ON  
     party_type.party_type_id = party.party_type_id  
  
  INNER JOIN Currency ON  
   td.currency_id = currency.currency_id  
  
  INNER JOIN Currency Base ON  
   td.amount_currency_id = base.currency_id  
  
  LEFT OUTER JOIN allocationdetail ON  
   allocationdetail.transdetail_id = td.transdetail_id AND ISNULL(allocationdetail.is_reversed,0)=0  
  
--LEFT JOIN CashListItem_claim_Link CLICL ON  
--CLICL.Claim_payment_id=CP.claim_Payment_ID  
  
    LEFT JOIN (SELECT MAX(ccl.CashListItem_id) CashListItem_id,  
                   MIN(ccl.claim_payment_id) claim_payment_id,  
   MIN(ad.is_reversed) is_reserved  
                FROM CashListItem_claim_Link ccl  
    LEFT JOIN AllocationDetail AD on CCL.cashlistitem_id =ad.cashlistitem_id  
                 LEFT JOIN payment_approval pa  
                 ON pa.payment_cnt=ccl.cashlistitem_id AND pa.payment_type=2  
                 WHERE ISNULL(pa.approved, 1) = 1 and ccl.is_deleted <> 1 Group By claim_payment_id)CLICL  
   ON CLICL.Claim_payment_id=CP.claim_Payment_ID  
  
    LEFT JOIN CashListItem CLI  
    ON CLI.CashListItem_id = CLICL.CashListItem_id  
  
    LEFT JOIN CashListItem_Payment_Status CLIPS  
    ON CLI.CashListItem_Payment_status_Id = CLIPS.CashListItem_Payment_status_ID  
  
  
  WHERE td.amount = td.outstanding_amount  
  AND CP.base_claim_payment_id = CP.claim_payment_id  
  AND  
  (  
    (cp.date_of_payment >= @date_of_payment AND cp.date_of_payment < DATEADD(day,1,@date_of_payment_To))  
    OR  
    (  
     (cp.date_of_payment IS NULL)  
     AND  
     (  
      (doc.document_date >=@date_of_payment)  
            AND  
      (doc.document_date <DATEADD(day,1,@date_of_payment_To))  
     )  
    )  
  )  
  AND (IFILE.lead_agent_cnt=@AgentKey OR @AgentKey =0)
  GROUP BY  
   td.document_id,  
   td.currency_id,  
   td.amount_currency_id,  
   td.account_currency_id,  
   doc.document_ref,  
   doc.document_date,  
   doc.comment,  
   account.account_id,  
   cp.date_of_payment,  
   cp.base_claim_payment_id,  
   cp.payeemediatype,  
   cp.PayeeName,  
    cp.our_ref,  
    cp.ThirdPartyReference,  
    cp.PayeeSortCode,  
    cp.PayeeAccountNo,  
    cp.party_bank_id,  
 td.currency_base_xrate,
 cp.Media_ref
  having SUM(td.Amount) <> 0  
UNION  
  
 SELECT  
  td.document_id,  
  doc.document_ref,  
  SUM(td.Currency_amount),  
  td.currency_id,  
  SUM(td.Amount),  
  td.amount_currency_id,  
  SUM(td.Account_amount),  
  td.account_currency_id,  
  MIN(c.claim_number),  
  doc.comment,  
  MIN(currency.description),  
  MIN(currency.format_string),  
  cp.date_of_payment,  
  MIN(cp.payeemediatype),  
  MIN(base.description) base_currency_description,  
  MIN(base.format_string) base_format_string,  
  MAX(cp.claim_payment_id) as max_claim_payment_id,  
  doc.document_date,  
  account.account_id,  
  cp.base_claim_payment_id,  
  MIN(account.account_name),  
      Case MIN(CLIPS.Code) When  'PENDING'THEN 'Awaiting Settlement Authorisation'  
        ELSE 'Awaiting Settlement'  
    END Status,  
    MIN(MT.Description) MediaTypeDesc,  
    MIN(S.Description) Branch,  
    MIN(BA.Bank_Account_Name)BankAccount,  
    MIN(cp.PayeeMediaType)MediaType,  
    MIN(P.BankAccount_Id) BankAccountID,  
  
 MIN(doc.document_date) document_date,  
 NULL account_id,  
 cp.base_claim_payment_id,  
 min(Account.account_name) account_name  
 ,MIN(CLICL.CashListItem_id) CashListItem_id,  
    cp.our_ref,  
    cp.PayeeName,  
    MIN(mt.code) MediaTypeCode,  
    MIN(currency.code) CurrencyCode,  
    MIN(BA.code) BankAccountCode,  
    MIN(account.short_code) AccountCode,  
    MIN(s.code) BranchCode,  
    cp.ThirdPartyReference,  
    cp.PayeeSortCode,  
    cp.PayeeAccountNo,  
    cp.party_bank_id PartyBankId,  
    MIN(is_reserved) is_reversed,  
 td.currency_base_xrate,
 MIN(cp.Media_ref) Media_ref
  
 FROM transdetail td  
  
  INNER JOIN (SELECT * FROM document WHERE document_ref like 'CLP%')  doc ON  
   td.document_id = doc.document_id  
  
    LEFT JOIN Claim_Payment CP ON  
    cp.document_id  = td.document_id  
  
    LEFT JOIN claim c ON  
     c.claim_id = cp.claim_id  
  
    LEFT JOIN Insurance_File IFILE ON  
    C.Policy_ID = IFILE.Insurance_file_Cnt  
  
    JOIN Product P  
    ON IFILE.Product_ID = P.Product_ID  
  
    LEFT Join BankAccount BA ON  
    P.BankAccount_ID= BA.BankAccount_ID  
  
    JOIN Source S  
    ON IFILE.Source_ID = S.Source_ID  
  
    LEFT JOIN MediaType MT ON  
    MT.MediaType_ID = CP.PayeeMediaType  
  
  INNER JOIN (Select * from Account where account_id = @claim_payable_account_id  ) Account ON  
   td.account_id = account.account_id  
  
  INNER JOIN Currency ON  
   td.currency_id = currency.currency_id  
  
  INNER JOIN Currency Base ON  
   td.amount_currency_id = base.currency_id  
  
--LEFT JOIN CashListItem_claim_Link CLICL ON  
   -- CLICL.Claim_payment_id=CP.claim_Payment_ID  
  
   LEFT JOIN (SELECT MAX(ccl.CashListItem_id) CashListItem_id ,MIN(ccl.claim_payment_id) claim_payment_id,  
   MIN(ad.is_reversed) is_reserved  
   FROM CashListItem_claim_Link ccl  
   LEFT JOIN AllocationDetail AD on CCL.cashlistitem_id =ad.cashlistitem_id  
   LEFT JOIN payment_approval pa  
   ON pa.payment_cnt=ccl.cashlistitem_id AND pa.payment_type=2  
   WHERE ISNULL(pa.approved, 1) = 1 and ccl.is_deleted <> 1 Group By claim_payment_id)CLICL  
  
   ON CLICL.Claim_payment_id=CP.claim_Payment_ID  
  
    LEFT JOIN CashListItem CLI  
    ON CLI.CashListItem_id = CLICL.CashListItem_id  
  
    LEFT JOIN CashListItem_Payment_Status CLIPS  
    ON CLI.CashListItem_Payment_status_Id = CLIPS.CashListItem_Payment_status_ID  
    WHERE td.amount = td.outstanding_amount  
  AND CP.base_claim_payment_id = CP.claim_payment_id  
  AND  
  (  
    (cp.date_of_payment >= @date_of_payment AND cp.date_of_payment < DATEADD(day,1,@date_of_payment_To))  
    OR  
    (  
     (cp.date_of_payment IS NULL)  
     AND  
     (  
      (doc.document_date >=@date_of_payment)  
            AND  
      (doc.document_date <DATEADD(day,1,@date_of_payment_To))  
        )  
    )  
  )  
  GROUP BY  
   td.document_id,  
   td.currency_id,  
   td.amount_currency_id,  
   td.account_currency_id,  
   doc.document_ref,  
   doc.document_date,  
   doc.comment,  
   account.account_id,  
   cp.date_of_payment,  
   cp.base_claim_payment_id,  
   cp.payeemediatype,  
   cp.PayeeName,  
    cp.our_ref,  
    cp.ThirdPartyReference,  
    cp.PayeeSortCode,  
    cp.PayeeAccountNo,  
    cp.party_bank_id,  
 td.currency_base_xrate,
 cp.Media_ref
  having SUM(td.Amount) <> 0  
  
Order by doc.document_ref  
  
END  
GO
