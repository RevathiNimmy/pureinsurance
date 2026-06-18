SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Map_Detail_add'
GO

CREATE PROCEDURE spe_Export_Map_Detail_add
    @export_map_model_id int,
    @export_map_detail_id int,
    @target_field_name varchar(40),
    @sequence smallint
AS
BEGIN
INSERT INTO Export_Map_Detail (
    export_map_model_id ,
    export_map_detail_id ,
    target_field_name ,
    sequence )
VALUES (
    @export_map_model_id,
    @export_map_detail_id,
    @target_field_name,
    @sequence)
END

GO

