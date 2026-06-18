SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Cobol_Linkage_del'
GO

CREATE PROCEDURE spe_GIS_Cobol_Linkage_del
    @linkage_map_id int,
    @item_sequence int
AS
DELETE FROM GIS_Cobol_Linkage
WHERE linkage_map_id = @linkage_map_id AND item_sequence = @item_sequence

GO

