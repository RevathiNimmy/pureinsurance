SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Claim_Peril_Sel'
GO

CREATE PROCEDURE
spu_CLM_Claim_Peril_Sel
(
@ClaimPerilId int
)
AS

SELECT
CP.claim_peril_id,
CP.claim_id,
CP.peril_type_id,
CP.description,
CP.comments,
CP.sum_insured,
CP.ri_band,
CP.gis_screen_id
FROM Claim_Peril CP
WHERE
CP.claim_peril_id=@ClaimPerilId

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

