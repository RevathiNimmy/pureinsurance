SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_gis_screen_code'
GO


CREATE PROCEDURE spu_get_gis_screen_code
    @gis_policy_link_id INT
AS


SELECT  gs.code
FROM    gis_screen gs,
    risk r,
    gis_policy_link gpl
WHERE   gpl.gis_policy_link_id = @gis_policy_link_id
AND gpl.risk_id = r.risk_cnt
AND r.gis_screen_id = gs.gis_screen_id
GO


