SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_CLM_Get_Claim_Payment_Total'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payment_Total    
@nClaim_Id int    
AS    
BEGIN  
 --Recovery can be greater than payment but it should not be more than the total reserve  

  SELECT sum(CPI.this_payment)    
	 FROM claim_payment  CP join Claim_Payment_Item CPI on CP.claim_payment_id = CPI.claim_payment_id   
	 WHERE claim_id = @nClaim_Id
	 and CPI.recovery_type_id IS NULL  

END    

GO