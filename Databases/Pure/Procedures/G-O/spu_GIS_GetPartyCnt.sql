SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_GetPartyCnt'
GO

CREATE PROCEDURE spu_GIS_GetPartyCnt
@gis_policy_link_id int

AS

SELECT party_cnt FROM gis_policy_link 
WHERE gis_policy_link_id=@gis_policy_link_id

GO
