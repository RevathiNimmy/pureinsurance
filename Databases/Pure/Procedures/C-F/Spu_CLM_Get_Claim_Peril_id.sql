SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_CLM_Get_Claim_Peril_id'
GO

Create Procedure Spu_CLM_Get_Claim_Peril_id
	@claim_id int
AS
Select		policy_id,
		claim_peril_id,
		pt.code
From Claim
	Join Claim_Peril cp On Claim.Claim_id=cp.Claim_id
	Join Peril_Type  pt On  pt.peril_type_id = cp.peril_type_id
Where Claim.claim_id=@claim_id
  
