SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Cobol_Linkage_Sel_Map'
GO


CREATE PROCEDURE spu_GIS_Cobol_Linkage_Sel_Map
    @linkage_map_id int = NULL
AS


SELECT
        CL.linkage_map_id,
        CL.item_sequence,
        CL.gis_property_id,
        CL.gis_object_id,
        CL.item_name,
        CL.item_type,
        CL.item_offset,
        CL.item_occurs_offset,
        CL.item_byte_length,
        CL.item_pic_length,
        CL.item_decimal_places,
        CL.conversion_type,
        CL.conversion_list_id,
        CL.occurs_level,
        CL.occurs_times,
        CL.default_value,
        CL.insurer_code,
        CL.object_name,
        CL.property_name,
        CL.inst_one_level,
        CL.inst_two_level,
        CL.inst_three_level,
        CL.inst_four_level

    FROM     GIS_Cobol_Linkage  AS CL

    WHERE (linkage_map_id = @linkage_map_id OR @linkage_map_id = NULL)

    ORDER BY item_sequence
GO


