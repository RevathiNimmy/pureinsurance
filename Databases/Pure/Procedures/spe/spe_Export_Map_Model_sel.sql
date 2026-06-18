SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Model_sel'
GO

CREATE PROCEDURE spe_Export_Map_Model_sel
    @export_map_model_id int
AS
SELECT
    export_map_model_id,
    code,
    description,
    target_database_name,
    date_created,
    created_by_username
 FROM Export_Map_Model
WHERE export_map_model_id = @export_map_model_id

GO

