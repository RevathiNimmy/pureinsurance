SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_drivers_del'
GO

CREATE PROCEDURE spe_drivers_del
    @insurance_file_cnt int,
    @driver_number int
AS
DELETE FROM drivers
WHERE insurance_file_cnt = @insurance_file_cnt AND driver_number = @driver_number

GO

