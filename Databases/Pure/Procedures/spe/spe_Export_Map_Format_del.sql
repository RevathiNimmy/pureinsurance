SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Format_del'
GO

CREATE PROCEDURE spe_Export_Map_Format_del
    @export_map_model_id int,
    @export_map_detail_id int,
    @export_map_format_id int
AS
DELETE FROM Export_Map_Format
WHERE export_map_model_id = @export_map_model_id AND export_map_detail_id = @export_map_detail_id AND export_map_format_id = @export_map_format_id

GO

