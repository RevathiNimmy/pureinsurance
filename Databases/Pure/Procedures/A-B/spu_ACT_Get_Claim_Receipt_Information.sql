SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Claim_Receipt_Information'
GO

CREATE PROCEDURE spu_ACT_Get_Claim_Receipt_Information  
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
  
IF ISNULL(@hidden_option, '0') <> '1'  
 SELECT @account_company_id=NULL  
ELSE  
 SELECT @account_company_id=@source_id  
  
SELECT @account_id=A.account_id
FROM Account A
INNER JOIN claim_payment wcp ON 
	wcp.party_cnt = A.account_key
INNER JOIN Stats_Folder sf  
	ON sf.document_ref = @document_ref  
	AND sf.source_id = A.company_id
    AND sf.payment_id = wcp.claim_payment_id  

IF @account_id IS NULL
	SELECT @account_id=A.account_id
	FROM Account A
	WHERE A.short_code='CLMRECEIVABLE'

SELECT  
 @currency_id = MIN(wcri.currency_id),  
 @currency_base_xrate = MIN(wcri.currency_base_xrate),  
 @currency_base_date = MIN(wcri.currency_base_date),  
 @account_base_xrate = MIN(wcri.account_base_xrate),  
 @account_base_date = MIN(wcri.account_base_date),  
 @system_base_xrate = MIN(wcri.system_base_xrate),  
 @system_base_date = MIN(wcri.system_base_date),  
 @exchange_rate_override_reason_id = MIN(wcri.exchange_rate_override_reason_id)  
  
FROM claim_receipt_item wcri  
  
 INNER JOIN claim_receipt wcr ON  
  wcri.claim_receipt_id = wcr.claim_receipt_id  
   INNER JOIN Stats_Folder sf  
    ON sf.document_ref = @document_ref  
    AND sf.receipt_id = wcr.claim_receipt_id  
  
GROUP BY wcri.claim_receipt_Id



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
