EXEC DDLDropProcedure 'spu_ACT_Get_Claim_Information'
GO
                 
CREATE PROCEDURE spu_ACT_Get_Claim_Information    
 @document_ref VARCHAR(50),    
 @source_id INT,    
 @account_id INT OUTPUT,    
 @currency_id SMALLINT OUTPUT,    
 @currency_base_xrate FLOAT OUTPUT,    
 @currency_base_date DATETIME OUTPUT,    
 @account_base_xrate FLOAT OUTPUT,    
 @account_base_date DATETIME OUTPUT,    
 @system_base_xrate FLOAT OUTPUT,    
 @system_base_date DATETIME OUTPUT,    
 @exchange_rate_override_reason_id INT OUTPUT    
    
AS    
    
DECLARE @hidden_option VARCHAR(20)    
DECLARE @account_company_id INT    
    
SELECT @hidden_option = NULL    
SELECT @hidden_option = ISNULL(value,NULL) FROM hidden_options    
 WHERE branch_id = @Source_id AND option_number =16    
    
IF ISNULL(@hidden_option,0)<>'1'    
 SELECT @account_company_id=NULL    
ELSE    
 SELECT @account_company_id=@source_id    
    
SELECT    
 @account_id = ac.account_id,    
 @currency_id = c.currency_id,    
 @currency_base_xrate = c.currency_base_xrate,    
 @currency_base_date = c.currency_base_date,    
 @account_base_xrate = c.account_base_xrate,    
 @account_base_date = c.account_base_date,    
 @system_base_xrate = c.system_base_xrate,    
 @system_base_date = c.system_base_date,    
 @exchange_rate_override_reason_id = c.exchange_rate_override_reason_id    
FROM Claim c    
JOIN account ac    
 ON ac.account_key = c.client_id    
 AND ac.company_id = ISNULL(@account_company_id,ac.company_id)    
JOIN Stats_Folder sf    
 ON sf.loss_id=c.claim_id    
WHERE sf.document_ref=@document_ref AND ISNULL(c.currency_base_xrate, 0) <> 0 

GO