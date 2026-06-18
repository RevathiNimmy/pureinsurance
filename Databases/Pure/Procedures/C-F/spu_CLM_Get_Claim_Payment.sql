
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_CLM_Get_Claim_Payment
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payment     
    @ClaimPaymentKey  int = NULL,  
    @nClaimKey		  int = NULL 	  
AS    
    
BEGIN    

IF ISNULL(@ClaimPaymentKey,0) <> 0 
	BEGIN  
		SELECT cp.Claim_ID as ClaimKey,   
			   cp.Amount ,  
			   cp.created_By as CreatorUserKey,  
			   c.Claim_Number as ClaimNumber,
			   curr.symbol  as CurrencySymbol    
		FROM claim_payment cp JOIN  claim c ON c.claim_id = cp.claim_id 
			  JOIN currency curr ON cp.currency_id = curr.currency_id  
		WHERE cp.claim_payment_id =@ClaimPaymentKey
	END
ELSE
	BEGIN
		SELECT SUM(cp.Amount)		
		FROM claim_payment cp  
		WHERE cp.claim_id = @nClaimKey and is_referred = 1
	END
END    
