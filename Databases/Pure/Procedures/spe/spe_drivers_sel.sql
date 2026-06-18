SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_drivers_sel'
GO

CREATE PROCEDURE spe_drivers_sel
    @insurance_file_cnt int,
    @driver_number int
AS
SELECT
    insurance_file_cnt,
    driver_number,
    name,
    date_of_birth,
    gender,
    type_of_licence,
    date_passed_test
 FROM drivers
WHERE insurance_file_cnt = @insurance_file_cnt AND driver_number = @driver_number

GO

