SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_drivers_add'
GO

CREATE PROCEDURE spe_drivers_add
    @insurance_file_cnt int,
    @driver_number int,
    @name varchar(60),
    @date_of_birth datetime,
    @gender varchar(70),
    @type_of_licence varchar(70),
    @date_passed_test datetime
AS
BEGIN
INSERT INTO drivers (
    insurance_file_cnt ,
    driver_number ,
    name ,
    date_of_birth ,
    gender ,
    type_of_licence ,
    date_passed_test )
VALUES (
    @insurance_file_cnt,
    @driver_number,
    @name,
    @date_of_birth,
    @gender,
    @type_of_licence,
    @date_passed_test)
END

GO

