EXECUTE DDLDropProcedure 'spu_ACT_Get_Unallocated_Claim_Payments'
GO
CREATE PROCEDURE spu_ACT_Get_Unallocated_Claim_Payments  
 @account_id int  ,
 @AgentKey INT=0
AS  
  
BEGIN  
 IF NOT Exists(select NULL from Account acc JOIN party_insurer pty ON acc.account_key =pty.party_cnt  
     where acc.account_id =@account_id  
      and pty.is_reinsurer <>1)  
 BEGIN  
 SELECT  
  td.document_id,  
  doc.document_ref,  
  SUM(td.Currency_amount) currency_amount,  
  td.currency_id,  
  SUM(td.Amount) amount,  
  td.amount_currency_id,  
  SUM (td.Account_amount) account_amount,  
  td.account_currency_id,  
  MIN(c.claim_number) claim_number,  
  doc.comment,  
  MIN(currency.description) currency_description,  
  MIN(currency.format_string) currency_format_string,  
  MIN(date_of_payment) date_of_payment,  
  MIN(cp.payeemediatype) payee_media_type,  
  MIN(base.description) base_currency_description,  
  MIN(base.format_string) base_format_string,  
  cp.claim_payment_id,  
  MIN(doc.document_date),  
  @account_id,  
  cp.base_claim_payment_id,  
  MIN(Account.account_name),  
  
    Case MIN(CLIPS.Code) When  'PENDING' THEN 'Awaiting Settlement Authorisation'  
        ELSE 'Awaiting Settlement'  
    END Status,  
    MIN(MT.Description) MediaTypeDesc,  
    MIN(S.Description) Branch,  
    MIN(BA.Bank_Account_Name)BankAccount,  
    MIN(cp.PayeeMediaType)MediaType,  
    MIN(P.BankAccount_Id) BankAccountID ,  
  MIN(doc.document_date) document_date,  
  @account_id account_id,  
  cp.base_claim_payment_id,  
  MIN(Account.account_name) account_name ,  
  MIN(CLICL.CashListItem_id) CashListItem_id,  
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
  
 FROM transdetail  td  
  
  INNER JOIN (Select account_id, account_name,short_code  from account where account_id = @account_id) account ON  
  td.account_id = account.account_id  
  
  INNER JOIN Currency ON  
   td.currency_id = currency.currency_id  
  
 INNER JOIN Currency Base ON  
  td.amount_currency_id = base.currency_id  
  
        INNER JOIN document doc ON  
        td.document_id = doc.document_id  
  
     LEFT JOIN Stats_Folder sf ON  
        sf.document_ref=doc.document_ref  
  
    LEFT JOIN Claim_Payment CP ON  
  cp.claim_payment_id  = sf.payment_id  
  
    LEFT JOIN claim c ON  
    cp.claim_id = c.claim_id  
  
    LEFT JOIN Insurance_File IFILE ON  
    C.Policy_ID = IFILE.Insurance_file_Cnt  
  
    JOIN Product P  
    ON IFILE.Product_ID = P.Product_ID  
  
    LEFT Join BankAccount BA ON  
    P.BankAccount_ID= BA.BankAccount_ID  
  
    JOIN Source S  
    ON IFILE.Source_ID = S.Source_ID  
  
  -- Not many ways as cashlist is suppose to be in one to one relationship with claim payment due to current interface design  
  LEFT JOIN (Select MIN(CCL.claim_payment_id) claim_payment_id,  
  MAX(CCL.CashListItem_id) CashListItem_id ,  
  MIN(ad.is_reversed) is_reserved  
   From CashListItem_claim_Link CCL  
   LEFT JOIN AllocationDetail AD on CCL.cashlistitem_id =ad.cashlistitem_id  
    Where is_deleted <> 1 Group By claim_payment_id) CLICL ON  
    CLICL.Claim_payment_id=CP.claim_Payment_ID  
  
    LEFT JOIN CashListItem CLI  
    ON CLI.CashListItem_id = CLICL.CashListItem_id  
  
    LEFT JOIN CashListItem_Payment_Status CLIPS  
    ON CLI.CashListItem_Payment_status_Id = CLIPS.CashListItem_Payment_status_ID  
  
    LEFT JOIN MediaType MT ON  
    MT.MediaType_ID = CP.PayeeMediaType  
  
 WHERE td.document_id in (  
    SELECT document_id  
    FROM document  
    WHERE documenttype_id in (  
     SELECT documenttype_id  
     FROM documenttype WHERE code ='CLP'))  
 AND td.account_id = @account_id  
    AND td.amount = td.outstanding_amount  
  
  AND CP.base_claim_payment_id = CP.claim_payment_id  
   AND td.outstanding_amount<>0  
   AND (IFILE.lead_agent_cnt=@AgentKey OR @AgentKey=0)
 group by td.document_id,  
   doc.document_ref,  
   td.currency_id,  
   amount_currency_id,  
   account_currency_id,  
   doc.comment,  
   cp.claim_payment_id ,  
   cp.base_claim_payment_id,  
   cp.PayeeName,  
    cp.our_ref,  
        cp.ThirdPartyReference,  
        cp.PayeeSortCode,  
        cp.PayeeAccountNo,  
        cp.party_bank_id,  
  td.currency_base_xrate,
  cp.Media_ref
  
 END  
END  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
