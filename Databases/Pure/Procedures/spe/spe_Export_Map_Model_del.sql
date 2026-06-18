SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Model_del'
GO

CREATE PROCEDURE spe_Export_Map_Model_del
    @export_map_model_id int
AS
DELETE FROM Export_Map_Model
WHERE export_map_model_id = @export_map_model_id

GO

