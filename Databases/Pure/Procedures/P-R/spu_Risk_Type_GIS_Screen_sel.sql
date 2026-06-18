SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_GIS_Screen_sel'
GO


CREATE PROCEDURE spu_Risk_Type_GIS_Screen_sel
    @risk_type_id int
AS


SELECT
    RTGS.gis_screen_id
 FROM Risk_Type_GIS_Screen RTGS, GIS_Screen GS

WHERE RTGS.risk_type_id = @risk_type_id
 AND RTGS.gis_screen_id = GS.gis_screen_id
 AND GS.is_deleted = 0

ORDER BY GS.code ASC
GO


