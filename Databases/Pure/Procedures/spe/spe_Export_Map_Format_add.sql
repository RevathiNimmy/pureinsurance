SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Format_add'
GO

CREATE PROCEDURE spe_Export_Map_Format_add
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
INSERT INTO Export_Map_Format (
    export_map_model_id ,
    export_map_detail_id ,
    export_map_format_id ,
    source_field_name ,
    sequence ,
    leading_characters ,
    trailing_characters ,
    start_position ,
    number_of_chars ,
    valid_value ,
    field_separator ,
    is_upper_case )

VALUES (
    @export_map_model_id,
    @export_map_detail_id,
    @export_map_format_id,
    @source_field_name,
    @sequence,
    @leading_characters,
    @trailing_characters,
    @start_position,
    @number_of_chars,
    @valid_value,
    @field_separator,
    @is_upper_case)
END

GO

