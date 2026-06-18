SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Format_Char_sel'
GO

CREATE PROCEDURE spe_Export_Map_Format_Char_sel
    @leading_characters varchar(255)

AS
DECLARE @export_map_detail_id int
SELECT @export_map_detail_id=export_map_detail_id
FROM Export_Map_Format
WHERE rtrim(leading_characters)  = @leading_characters

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
WHERE export_map_detail_id  = @export_map_detail_id

GO

