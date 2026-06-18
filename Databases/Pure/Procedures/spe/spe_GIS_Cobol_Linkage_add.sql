SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Cobol_Linkage_add'
GO

CREATE PROCEDURE spe_GIS_Cobol_Linkage_add
    @linkage_map_id int,
    @item_sequence int,
    @gis_property_id int,
    @gis_object_id int,
    @item_name varchar(50),
    @item_type char,
    @item_offset int,
    @item_occurs_offset int,
    @item_byte_length smallint,
    @item_pic_length smallint,
    @item_decimal_places tinyint,
    @conversion_type char,
    @conversion_list_id smallint,
    @occurs_level tinyint,
    @occurs_times smallint,
    @inst_one_level tinyint,
    @inst_two_level tinyint,
    @inst_three_level tinyint,
    @inst_four_level tinyint,
    @inst_five_level tinyint,
    @inst_six_level tinyint,
    @default_value varchar(255)
AS
BEGIN
INSERT INTO GIS_Cobol_Linkage (
    linkage_map_id ,
    item_sequence ,
    gis_property_id ,
    gis_object_id ,
    item_name ,
    item_type ,
    item_offset ,
    item_occurs_offset ,
    item_byte_length ,
    item_pic_length ,
    item_decimal_places ,
    conversion_type ,
    conversion_list_id ,
    occurs_level ,
    occurs_times ,
    inst_one_level ,
    inst_two_level ,
    inst_three_level ,
    inst_four_level ,
    inst_five_level ,
    inst_six_level ,
    default_value )
VALUES (
    @linkage_map_id,
    @item_sequence,
    @gis_property_id,
    @gis_object_id,
    @item_name,
    @item_type,
    @item_offset,
    @item_occurs_offset,
    @item_byte_length,
    @item_pic_length,
    @item_decimal_places,
    @conversion_type,
    @conversion_list_id,
    @occurs_level,
    @occurs_times,
    @inst_one_level,
    @inst_two_level,
    @inst_three_level,
    @inst_four_level,
    @inst_five_level,
    @inst_six_level,
    @default_value)
END

GO

