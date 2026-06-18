SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Already_ReferredClaim_status'
GO

Create Procedure spu_CLM_Get_Already_ReferredClaim_status
	@Claim_payment_ID bigint
AS
 Select is_referred,ISNULL(recommended_by,0) From Claim_Payment  
 WHERE  Claim_payment_ID = @Claim_payment_ID  

GO
