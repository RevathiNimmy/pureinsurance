SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_claimrisk_desc'
GO

CREATE procedure spu_Get_ClaimRisk_Desc
	@ClaimId integer,
	@RiskTypeId integer
as

select description
from claim_risk
where claim_id = @claimid 
and risk_type_id = @risktypeid



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

