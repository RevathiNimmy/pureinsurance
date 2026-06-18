SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitcash'
GO

CREATE PROCEDURE spu_wp_debitcash  
   @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
DECLARE @SharedIndicator int  
DECLARE @InsuranceRef varchar(50)  
  
SELECT @SharedIndicator = CHARINDEX('|',@DocumentRef)  


IF ISNULL(@PartyCnt,0)=0 AND @DocumentRef<>''
SELECT @PartyCnt = account_key FROM account a 
			INNER JOIN TransDetail td ON a.account_id=td.account_id
			INNER JOIN document d ON d.document_id=td.document_id 
			WHERE d.document_ref=@DocumentRef AND ISNULL(a.account_key,0)<>0 AND d.documenttype_id IN (22,23)

  
If @SharedIndicator <> 0  
BEGIN  
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)  
END  
  if @DocumentRef =''  
begin  
 Select @InsuranceRef= insurance_ref from insurance_file where insurance_file_cnt=@InsuranceFileCnt  
 select @DocumentRef =document_ref  FROM  document d  
        JOIN transdetail t  
            ON t.document_id = d.document_id  
        JOIN cashlistitem csh  
            ON csh.transdetail_id = t.transdetail_id  
            where t.insurance_ref=@insuranceref  
end  
IF EXISTS  
    (  
        SELECT NULL  
        FROM  document d  
        JOIN transdetail t  
            ON t.document_id = d.document_id  
        JOIN cashlistitem csh  
            ON csh.transdetail_id = t.transdetail_id  
        WHERE d.document_ref = LTRIM(@DocumentRef)  
    )  
BEGIN  
    SELECT  
        csh.media_ref,  
 (Select Description from Company WHERE company_id=d.company_id) 'branch',  
        abs(csh.amount) 'amount',  
        d.document_ref,  
        d.document_date,  
        -- PN62297:  
        cpt.description 'description',  
        a.short_code,  
        CASE WHEN RTRIM(a.contact_name)<>''THEN RTRIM(a.contact_name) ELSE a.account_name END account_name,  
        c.iso_code,  
        c.description 'CurrencyDesc',  
        c.symbol,  
        csh.our_ref,  
        csh.their_ref,  
        mt.description 'MediaType',  
        MTS.description as 'media_type_status_desc',  
        csh.address1,  
        csh.address2,  
        csh.address3,  
        csh.address4,  
        RTrim(Convert(varchar(255),csh.amount)) + '|' + RTrim(c.description) + '|' + RTrim(c.minor_part) 'CashAmountWord',  
        crt.description'CashListItemReceiptType',  
        csh.contact_name,  
        country.description 'Country',  
        csh.payment_name,  
        csh.postal_code,  
  csh.transaction_date,  
  c.symbol 'currency_major',  
  c.minor_part 'currency_minor',  
  csh.receipt_details 'receipt_details',  
  abs(csh.amount) 'CashAmountWordsMajor',  
  abs(csh.amount) 'CashAmountWordsMinor',  
  pm.username,  
  RIGHT(RTRIM(csh.cc_number),4) 'cc_number_last4digits',  
  clt.description 'transaction_type',  
  csh.cc_auth_code 'cc_auto_authorisation_code',  
  csh.cc_manual_auth_code 'cc_manual_authorisation_code',  
  csh.cc_transaction_code 'cc_transaction_code',  
  mti.description 'cc_card_type',  
  csh.cc_expiry_date 'cc_expiry_date',  
  csh.cc_start_date 'cc_start_date',  
  csh.cc_issue 'cc_issue_number'  ,  
  csh.comments,
  clb.description 'bank_name' ,
  csh.collection_date 'collection_date'  
  
    FROM document d  
    JOIN transdetail t  
        ON t.document_id = d.document_id  
    JOIN cashlistitem csh  
        ON csh.transdetail_id = t.transdetail_id  
    JOIN cashlist cl  
        ON csh.cashlist_id = cl.cashlist_id  
 JOIN currency c  
        ON c.currency_id = cl.currency_id  
    JOIN cashlisttype clt  
        ON cl.cashlisttype_id = clt.cashlisttype_id  
    JOIN mediatype mt  
        ON csh.mediatype_id = mt.mediatype_id  
    JOIN account a  
        ON a.account_id = csh.account_id  
    LEFT JOIN party p  
        ON p.party_cnt = a.account_key  
    LEFT JOIN country  
        ON csh.address_country = country.country_id  
 JOIN PMUser pm  
  ON csh.pmuser_id=pm.user_id  
  Join MediaType_Status MTS on mt.MediaType_Status_Id= Mts.MediaType_Status_Id  
    LEFT JOIN cashlistitem_receipt_type crt  
        ON crt.cashlistitem_receipt_type_id = csh.cashlistitem_receipt_type_id  
    LEFT JOIN cashlistitem_payment_type cpt  
        ON cpt.cashlistitem_payment_type_id = csh.cashlistitem_payment_type_id  
    LEFT JOIN mediatype_issuer mti  
        ON mti.mediatype_issuer_id = csh.mediatype_issuer_id  
		LEFT JOIN CashListItem_Bank clb 
		on clb.cashlistitem_bank_id=csh.cashlistitem_bank_id
    WHERE d.document_ref = LTRIM(@documentRef )  and p.party_cnt =@PartyCnt  
    Order By t.transdetail_id DESC  
END  
ELSE  
BEGIN  
    /*Must be instalments*/  
    SELECT  
   csh.media_ref,  
 (Select Description from Company WHERE company_id=d.company_id) 'branch',  
        abs(csh.amount) 'amount',  
        d.document_ref,  
        d.document_date,  
        -- PN17822: put receipt type into payment type field for receipts  
        CASE ISNULL(csh.cashlistitem_payment_type_id, 0)  
            WHEN 0 THEN crt.description  
            ELSE cpt.description  
  END 'description',  
        a.short_code,  
        CASE WHEN RTRIM(a.contact_name)<>''THEN RTRIM(a.contact_name) ELSE a.account_name END account_name,  
        c.iso_code,  
        c.description 'CurrencyDesc',  
        c.symbol,  
        csh.our_ref,  
        csh.their_ref,  
        mt.description 'MediaType',  
  MTS.description as 'media_type_status_desc',  
        csh.address1,  
        csh.address2,  
        csh.address3,  
        csh.address4,  
        RTrim(Convert(varchar(255),csh.amount)) + '|' + RTrim(c.description) + '|' + RTrim(c.minor_part) 'CashAmountWord',  
        crt.description'CashListItemReceiptType',  
        csh.contact_name,  
        country.description 'Country',  
        csh.payment_name,  
        csh.postal_code,  
        csh.transaction_date,  
  c.symbol 'currency_major',  
  c.minor_part 'currency_minor',  
  csh.receipt_details 'receipt_details',  
  abs(csh.amount) 'CashAmountWordsMajor',  
  abs(csh.amount) 'CashAmountWordsMinor',  
  pm.username,  
  RIGHT(RTRIM(csh.cc_number),4) 'cc_number_last4digits',  
  clt.description 'transaction_type',  
  csh.cc_auth_code 'cc_auto_authorisation_code',  
  csh.cc_manual_auth_code 'cc_manual_authorisation_code',  
  csh.cc_transaction_code 'cc_transaction_code',  
  mti.description 'cc_card_type',  
  csh.cc_expiry_date 'cc_expiry_date',  
  csh.cc_start_date 'cc_start_date',  
  csh.cc_issue 'cc_issue_number',  
  csh.collection_date 'collection_date',  
  csh.comments,
  clb.description 'bank_name'  
    FROM document d  
    JOIN transdetail t  
        ON t.document_id = d.document_id  
    JOIN pfinstalments pfi  
        ON pfi.pftransaction_id = t.transdetail_id  
    JOIN cashlistitem_instalments clii  
        ON clii.pfinstalments_id = pfi.pfinstalments_id  
    JOIN cashlistitem csh  
        ON csh.cashlistitem_id = clii.cashlistitem_id  
    JOIN cashlist cl  
        ON csh.cashlist_id = cl.cashlist_id  
 JOIN currency c  
        ON c.currency_id = cl.currency_id  
    JOIN cashlisttype clt  
        ON cl.cashlisttype_id = clt.cashlisttype_id  
    JOIN mediatype mt  
        ON csh.mediatype_id = mt.mediatype_id  
    JOIN account a  
        ON a.account_id = csh.account_id  
    LEFT JOIN party p  
        ON p.party_cnt = a.account_key  
    JOIN country  
        ON csh.address_country = country.country_id  
 JOIN PMUser pm  
  ON csh.pmuser_id=pm.user_id  
  Join MediaType_Status MTS on mt.MediaType_Status_Id= Mts.MediaType_Status_Id  
    LEFT JOIN cashlistitem_receipt_type crt  
        ON crt.cashlistitem_receipt_type_id = csh.cashlistitem_receipt_type_id  
    LEFT JOIN cashlistitem_payment_type cpt  
        ON cpt.cashlistitem_payment_type_id = csh.cashlistitem_payment_type_id  
    LEFT JOIN mediatype_issuer mti  
        ON mti.mediatype_issuer_id = csh.mediatype_issuer_id  
		LEFT JOIN CashListItem_Bank clb 
		on clb.cashlistitem_bank_id=csh.cashlistitem_bank_id
    WHERE d.document_ref = LTRIM(@documentRef )  and p.party_cnt =@PartyCnt  
    Order By t.transdetail_id DESC  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

