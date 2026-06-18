SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitheader'
GO


CREATE PROCEDURE spu_wp_debitheader  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
DECLARE @SharedIndicator INT  
DECLARE @Share FLOAT  
DECLARE @in_tef INT  
  
IF (ISNULL(@DocumentRef, ' ') = ' ')  
BEGIN  
    -- Only do this for underwriting  
    IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')  
    BEGIN  
        SELECT TOP 1 @DocumentRef = document_ref  
        FROM    document  
        WHERE   insurance_file_cnt = @InsuranceFileCnt  
        AND     document_ref  NOT LIKE 'C%' AND document_ref NOT LIKE 'I%C%' 
        ORDER BY document_id DESC  /* By design not expected to work for Instalment business */  
    END  
END  

/*Get the share percentage if there is one and remove it from the document ref*/  
SELECT @SharedIndicator = CHARINDEX('|', @DocumentRef)  
  
If @SharedIndicator = 0  
BEGIN  
    SELECT @Share = 100  
END  
ELSE  
BEGIN  
    SELECT @Share = CONVERT(NUMERIC(15,11), RTRIM(SUBSTRING(@DocumentRef, @SharedIndicator + 1, 25 - @SharedIndicator)))  
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)  
END  
  
--Does document exist in tef  
SELECT  @in_tef = tef.transaction_export_folder_cnt  
FROM    transaction_export_folder tef, insurance_file i  
WHERE   tef.document_ref = @DocumentRef  
AND     i.insurance_file_cnt = @InsuranceFileCnt  
AND     tef.source_id = i.source_id  
  
IF @in_tef IS NOT NULL  
    SELECT  
        tef.transaction_export_folder_cnt,  
        tef.product_id,  
        tef.transaction_export_folder_id,  
        tef.source_id,  
        tef.insurance_file_cnt,  
        tef.debit_credit,  
        tef.document_ref,  
        tef.document_comment,  
        tef.document_date,  
        tef.is_payable_by_instalments,  
        tef.accounting_date,  
        tef.posting_period_year,  
        tef.posting_period_number,  
        ((tef.premium_total * @Share) / 100) 'premium_total',  
        tef.transaction_type_id,  
        tef.transaction_type_code,  
        CASE (SELECT count(source_id)  
            FROM source  
            WHERE source_id = i.source_id  
            AND source_id = i.source_id  
            AND underwriting_branch_ind =1  
            AND i.alternate_reference IS NOT NULL)  
        WHEN 0 THEN  
            i.insurance_ref  
        ELSE  
            i.alternate_reference  
        END 'insurance_ref',  
        tef.effective_date,  
        tef.cover_start_date,  
        tef.expiry_date,  
        tef.insurance_holder_cnt,  
        tef.insurance_holder_id,  
        tef.insurance_holder_shortname,  
        tef.insurance_holder_account_key,  
        tef.product_code,  
        tef.business_type_id,  
        tef.business_type_code,  
        tef.account_handler_cnt,  
        tef.account_handler_id,  
        tef.account_handler_shortname,  
        tef.account_handler_account_key,  
        tef.agent_cnt,  
        tef.agent_id,  
        tef.agent_shortname,  
        tef.agent_account_key,  
        tef.branch_id,  
        tef.branch_code,  
        tef.currency_code,  
        tef.loss_id,  
        tef.loss_code,  
        tef.loss_date,  
        tef.created_by_user_id,  
        tef.created_by_username,  
        tef.accounts_export_status,  
        tef.reason,  
        ISNULL (tt.description, '') [description],  
        ISNULL (eis.payment_method, '') [payment_method]  
    FROM        transaction_export_folder tef  
    JOIN        insurance_file i ON tef.insurance_file_cnt = i.insurance_file_cnt  
    LEFT JOIN transaction_type tt on tt.transaction_type_id = tef.transaction_type_id  
    LEFT JOIN   event_log el ON  el.event_cnt = tef.event_log_id  
    LEFT JOIN   event_insurance_file eis ON el.event_cnt = eis.insurance_file_cnt  
  
    WHERE   tef.document_ref = @DocumentRef  
    AND     tef.insurance_file_cnt = @InsuranceFileCnt  
    AND     tef.accounts_export_status = 'c'  
ELSE  
    SELECT  d.document_ref,  
            d.comment as document_comment,  
            d.document_date,  
            d.company_id as source_id,  
   tef.debit_credit as debit_credit,  
   t.due_date  
    FROM    party p  
    JOIN    account a ON a.account_key =  p.party_cnt  
    -- PN17565 Removed cashlistitem join (Parallel fix to 11895)  
    JOIN    transdetail t ON t.account_id = a.account_id  
    JOIN    document d ON d.document_id = t.document_id  
    LEFT JOIN    transaction_export_folder tef ON d.document_ref = tef.document_ref  
    WHERE   d.insurance_file_cnt = @InsuranceFileCnt
    AND     d.document_ref = LTRIM(@documentRef)  

GO


