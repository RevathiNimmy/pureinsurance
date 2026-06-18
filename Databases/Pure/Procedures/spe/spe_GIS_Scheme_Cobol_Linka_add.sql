SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Scheme_Cobol_Linka_add'
GO

CREATE PROCEDURE spe_GIS_Scheme_Cobol_Linka_add
    @linkage_map_id int,
    @item_sequence int,
    @gis_scheme_id int
AS
BEGIN
INSERT INTO GIS_Scheme_Cobol_Linkage (
    linkage_map_id ,
    item_sequence ,
    gis_scheme_id )
VALUES (
    @linkage_map_id,
    @item_sequence,
    @gis_scheme_id)
END

GO

