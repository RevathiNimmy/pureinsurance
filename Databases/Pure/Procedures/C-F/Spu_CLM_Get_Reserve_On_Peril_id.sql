SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_CLM_Get_Reserve_On_Peril_id'
GO

CREATE Procedure Spu_CLM_Get_Reserve_On_Peril_id
	@claim_peril_id int
AS
Select		R.reserve_id,
			RT.Reserve_type_id,
			RT.name
From Reserve R
	Join Reserve_Type  RT On  RT.reserve_type_id = R.reserve_type_id
	Join Claim_Peril   CP On  CP.Claim_peril_id =  R.Claim_peril_id
Where CP.claim_peril_id = @claim_peril_id

GO