SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_vehicles_del'
GO

CREATE PROCEDURE spe_vehicles_del
    @insurance_file_cnt int,
    @vehicle_number int
AS
DELETE FROM vehicles
WHERE insurance_file_cnt = @insurance_file_cnt AND vehicle_number = @vehicle_number

GO

