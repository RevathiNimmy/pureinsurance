SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Current_version_id'
GO
CREATE  PROCEDURE spu_SAM_Get_Claim_Current_version_id

@baseClaimPeril_id int,
@version_id as int OUTPUT


AS

	SELECT
		@version_id=max(version_id) 

	FROM Claim_Peril 
	WHERE base_claim_peril_id = @baseClaimPeril_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
