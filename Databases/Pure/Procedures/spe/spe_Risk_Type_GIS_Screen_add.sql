SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_Type_GIS_Screen_add'
GO

CREATE PROCEDURE spe_Risk_Type_GIS_Screen_add
    @risk_type_id int,
    @gis_screen_id int

AS

BEGIN
INSERT INTO Risk_Type_GIS_Screen (
    risk_type_id ,
    gis_screen_id )
VALUES (
    @risk_type_id,
    @gis_screen_id)
END

GO

