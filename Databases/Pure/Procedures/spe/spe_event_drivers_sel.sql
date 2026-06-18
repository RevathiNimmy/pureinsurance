SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_drivers_sel'
GO

CREATE PROCEDURE spe_event_drivers_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    driver_number,
    name,
    date_of_birth,
    gender,
    type_of_licence,
    date_passed_test
FROM event_drivers
WHERE insurance_file_cnt = @insurance_file_cnt

GO

