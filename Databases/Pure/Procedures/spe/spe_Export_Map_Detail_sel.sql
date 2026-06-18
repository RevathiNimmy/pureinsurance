SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Detail_sel'
GO

CREATE PROCEDURE spe_Export_Map_Detail_sel
    @export_map_model_id int,
    @export_map_detail_id int
AS
SELECT
    export_map_model_id,
    export_map_detail_id,
    target_field_name,
    sequence
 FROM Export_Map_Detail
WHERE export_map_model_id = @export_map_model_id AND export_map_detail_id = @export_map_detail_id

GO

