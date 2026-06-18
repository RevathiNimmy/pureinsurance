SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Model_upd'
GO

CREATE PROCEDURE spe_Export_Map_Model_upd
    @export_map_model_id int,
    @code char(10),
    @description varchar(255),
    @target_database_name varchar(30),
    @date_created datetime,
    @created_by_username varchar(12)
AS
BEGIN
UPDATE Export_Map_Model
    SET
    code=@code,
    description=@description,
    target_database_name=@target_database_name,
    date_created=@date_created,
    created_by_username=@created_by_username
WHERE export_map_model_id = @export_map_model_id
END

GO

