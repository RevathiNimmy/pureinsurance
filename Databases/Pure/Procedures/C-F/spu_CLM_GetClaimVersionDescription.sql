
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_GetClaimVersionDescription'
GO


Create Procedure spu_CLM_GetClaimVersionDescription 
	@Claim_id integer
AS

	Select Claim_Version_Description From 
	Claim Where Claim_id = @Claim_ID
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO