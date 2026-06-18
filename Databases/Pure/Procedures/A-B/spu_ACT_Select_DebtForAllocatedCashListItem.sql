EXEC DDLDropProcedure 'spu_ACT_Select_DebtForAllocatedCashListItem'
GO

CREATE PROCEDURE spu_ACT_Select_DebtForAllocatedCashListItem  
 @account_id INT,  
 @insurance_file_cnt INT,  
 @document_ref VARCHAR(50),  
 @transdetail_id INT,  
  @cashlistitem_id  INT = NULL  
AS  
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')  
BEGIN   

   IF NOT ISNULL(@cashlistitem_id,0)=0  
  SELECT @TransDetail_id = transdetail_id  
   FROM cashlistitem where cashlistitem_id = @cashlistitem_id  
  
   IF @document_ref = '' SET @document_ref = NULL  

   DECLARE @is_gross_agent TINYINT = 0

   SELECT  TOP 1 @is_gross_agent = pa.is_gross_agent FROM account ac INNER JOIN party_agent pa ON ac.account_key = pa.party_cnt
   WHERE pa.party_agent_type_id = 1  AND ac.account_id = @account_id
  
    -- Underwriting code  
    SELECT  
        T.transdetail_id,  
        T.outstanding_amount,  
        T.amount_currency_id,  
        T.outstanding_account_amount,  
        T.account_currency_id,  
        C.iso_code,  
        T.account_id,  
        T.company_id,  
        T.spare,  
       (SELECT cashlistitem_id  
        FROM CashListItem  
        WHERE transdetail_id = T.transdetail_id) AS cashlistitem_id,  
        T.document_id,  
        T.insurance_ref  
    FROM  
 TransDetail T  
    INNER JOIN  
 Document D ON D.document_id=T.document_id  
    INNER JOIN  
 Currency C ON C.currency_id=T.amount_currency_id  
    WHERE  
 D.insurance_file_cnt=@insurance_file_cnt  
    AND (D.document_ref = @document_ref OR @document_ref IS NULL)  
    AND T.account_id=@account_id  
    AND T.outstanding_amount<>0  
    AND ((@is_gross_agent = 1 AND T.spare NOT IN ('COMM')) OR @is_gross_agent = 0)

    UNION  
  
    SELECT  
        T.transdetail_id,  
        T.outstanding_amount,  
     T.amount_currency_id,  
   T.outstanding_account_amount,  
   T.account_currency_id,  
   C.iso_code,  
   T.account_id,  
   T.company_id,  
   T.spare,  
       (SELECT cashlistitem_id  
        FROM CashListItem  
        WHERE transdetail_id = T.transdetail_id) AS cashlistitem_id,  
        T.document_id,  
        T.insurance_ref  
    FROM  
 TransDetail T  
    INNER JOIN  
 Document D ON D.document_id=T.document_id  
    INNER JOIN  
 Currency C ON C.currency_id=T.amount_currency_id  
    WHERE  
 T.transdetail_id=@transdetail_id  
    AND T.outstanding_amount<>0  
  
    ORDER BY T.outstanding_amount,T.spare DESC  
  
END  
ELSE  
BEGIN  
  
    -- Broking code  
    SELECT  
        T.transdetail_id,  
        T.outstanding_amount,  
        T.amount_currency_id,  
        T.outstanding_account_amount,  
        T.account_currency_id,  
        C.iso_code,  
        T.account_id,  
        T.company_id,  
        T.spare  
    FROM  
 TransDetail T  
    INNER JOIN  
 Document D ON D.document_id=T.document_id  
    INNER JOIN  
 Currency C ON C.currency_id=T.amount_currency_id  
    WHERE  
 D.insurance_file_cnt=@insurance_file_cnt  
    AND D.document_ref=@document_ref  
    AND T.account_id=@account_id  
    AND T.outstanding_amount<>0  
  
    UNION  
  
    SELECT  
        T.transdetail_id,  
        T.outstanding_amount,  
     T.amount_currency_id,  
   T.outstanding_account_amount,  
   T.account_currency_id,  
   C.iso_code,  
   T.account_id,  
   T.company_id,  
   T.spare  
    FROM  
 TransDetail T  
    INNER JOIN  
 Document D ON D.document_id=T.document_id  
    INNER JOIN  
 Currency C ON C.currency_id=T.amount_currency_id  
    WHERE  
 T.transdetail_id=@transdetail_id  
    AND T.outstanding_amount<>0  
  
    ORDER BY T.spare DESC  
  
END      

GO
