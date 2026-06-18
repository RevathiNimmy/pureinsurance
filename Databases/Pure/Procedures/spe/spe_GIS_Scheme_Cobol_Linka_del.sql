SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Scheme_Cobol_Linka_del'
GO

CREATE PROCEDURE spe_GIS_Scheme_Cobol_Linka_del
    @linkage_map_id int,
    @item_sequence int,
    @gis_scheme_id int
AS
DELETE FROM GIS_Scheme_Cobol_Linkage
WHERE linkage_map_id = @linkage_map_id AND item_sequence = @item_sequence AND gis_scheme_id = @gis_scheme_id

GO

