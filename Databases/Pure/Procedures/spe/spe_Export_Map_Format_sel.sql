SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Format_sel'
GO

CREATE PROCEDURE spe_Export_Map_Format_sel
    @export_map_model_id int,
    @export_map_detail_id int,
    @export_map_format_id int
AS
SELECT
    export_map_model_id,
    export_map_detail_id,
    export_map_format_id,
    source_field_name,

    sequence,
    leading_characters,
    trailing_characters,
    start_position,
    number_of_chars,
    valid_value,
    field_separator,
    is_upper_case
 FROM Export_Map_Format
WHERE export_map_model_id = @export_map_model_id AND export_map_detail_id = @export_map_detail_id AND export_map_format_id = @export_map_format_id

GO

