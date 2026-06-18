SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_GetClaimCnt'
GO

CREATE PROCEDURE spu_GIS_GetClaimCnt
@gis_policy_link_id int

AS

SELECT claim_id FROM gis_policy_link 
WHERE gis_policy_link_id=@gis_policy_link_id

GO
