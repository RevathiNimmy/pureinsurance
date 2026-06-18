SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Model_add'
GO

CREATE PROCEDURE spe_Export_Map_Model_add
    @export_map_model_id int,
    @code char(10),
    @description varchar(255),
    @target_database_name varchar(30),
    @date_created datetime,
    @created_by_username varchar(12)
AS
BEGIN
INSERT INTO Export_Map_Model (
    export_map_model_id ,
    code ,
    description ,
    target_database_name ,
    date_created ,
    created_by_username )
VALUES (
    @export_map_model_id,
    @code,
    @description,
    @target_database_name,
    @date_created,
    @created_by_username)
END

GO

