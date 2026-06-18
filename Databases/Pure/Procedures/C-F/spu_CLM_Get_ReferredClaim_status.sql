SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_ReferredClaim_status'
GO

Create Procedure spu_CLM_Get_ReferredClaim_status
	@Claim_payment_ID bigint
AS
	Select is_referred,is_referred_for_recommendation,recommended_by,created_by   From Claim_Payment
	WHERE  Claim_payment_ID = @Claim_payment_ID

GO
