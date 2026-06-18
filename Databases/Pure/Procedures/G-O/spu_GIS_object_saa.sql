SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_object_saa'
GO


CREATE PROCEDURE spu_GIS_object_saa
    @gis_data_model_id INT
AS

SELECT
    gis_object_id,
    gis_data_model_id,
    object_name,
    table_name,
    max_instances,
    is_quote_object,
    parent_object_id,
    polaris_object_id,
    is_selectable_for_screen,
    is_non_gis,
    Edit_Flags
 FROM GIS_object
WHERE   gis_data_model_id = @gis_data_model_id

ORDER BY gis_object_id ASC
GO