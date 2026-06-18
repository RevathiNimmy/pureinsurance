SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Detail_upd'
GO

CREATE PROCEDURE spe_Export_Map_Detail_upd
    @export_map_model_id int,
    @export_map_detail_id int,
    @target_field_name varchar(40),
    @sequence smallint
AS
BEGIN
UPDATE Export_Map_Detail
    SET
    target_field_name=@target_field_name,
    sequence=@sequence
WHERE export_map_model_id = @export_map_model_id AND export_map_detail_id = @export_map_detail_id
END

GO

