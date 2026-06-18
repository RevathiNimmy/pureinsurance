SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Format_upd'
GO

CREATE PROCEDURE spe_Export_Map_Format_upd
    @export_map_model_id int,
    @export_map_detail_id int,
    @export_map_format_id int,
    @source_field_name varchar(40),
    @sequence smallint,
    @leading_characters varchar(255),
    @trailing_characters varchar(255),
    @start_position smallint,
    @number_of_chars smallint,
    @valid_value varchar(255),
    @field_separator varchar(5),
    @is_upper_case tinyint
AS
BEGIN
UPDATE Export_Map_Format
    SET
    source_field_name=@source_field_name,
    sequence=@sequence,
    leading_characters=@leading_characters,
    trailing_characters=@trailing_characters,
    start_position=@start_position,
    number_of_chars=@number_of_chars,
    valid_value=@valid_value,
    field_separator=@field_separator,
    is_upper_case=@is_upper_case
WHERE export_map_model_id = @export_map_model_id AND export_map_detail_id = @export_map_detail_id AND export_map_format_id = @export_map_format_id
END

GO

