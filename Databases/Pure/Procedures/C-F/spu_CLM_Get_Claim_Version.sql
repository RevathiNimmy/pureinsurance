SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Version'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Version  
  
@claim_id int = NULL,  
@claim_peril_id int = NULL,  
@claim_payment_id int = NULL,  
@claim_payment_item_id int = NULL,  
@claim_receipt_id int = NULL,  
@claim_receipt_item_id int = NULL,  
@version_id int OUTPUT, 
@base_id int = NULL OUTPUT 
  
AS  
  
IF ISNULL(@claim_id,0) <> 0  
BEGIN  
 SELECT @version_id = version_id, 
	@base_id = base_claim_id FROM claim WITH (NOLOCK) WHERE claim_id = @claim_id  
 RETURN  
END  
  
IF ISNULL(@claim_peril_id,0) <> 0  
BEGIN  
 SELECT @version_id = version_id, 
	@base_id = base_claim_peril_id FROM claim_peril WITH (NOLOCK) WHERE claim_peril_id = @claim_peril_id  
 RETURN  
END  
  
IF ISNULL(@claim_payment_id,0) <> 0  
BEGIN  
 SELECT @version_id = version_id , 
	@base_id = base_claim_payment_id FROM claim_payment WITH (NOLOCK) WHERE claim_payment_id = @claim_payment_id  
END  
  
IF ISNULL(@claim_receipt_id,0) <> 0  
BEGIN  
 SELECT @version_id = version_id, 
	@base_id = base_claim_receipt_id FROM claim_receipt WITH (NOLOCK) WHERE claim_receipt_id = @claim_receipt_id  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
