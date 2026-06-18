SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Cobol_Linkage_sel'
GO

CREATE PROCEDURE spe_GIS_Cobol_Linkage_sel
    @linkage_map_id int,
    @item_sequence int
AS
SELECT
    linkage_map_id,
    item_sequence,
    gis_property_id,
    gis_object_id,
    item_name,
    item_type,
    item_offset,
    item_occurs_offset,
    item_byte_length,
    item_pic_length,
    item_decimal_places,
    conversion_type,
    conversion_list_id,
    occurs_level,
    occurs_times,
    inst_one_level,
    inst_two_level,
    inst_three_level,
    inst_four_level,
    inst_five_level,
    inst_six_level,
    default_value
 FROM GIS_Cobol_Linkage
WHERE linkage_map_id = @linkage_map_id AND item_sequence = @item_sequence

GO

