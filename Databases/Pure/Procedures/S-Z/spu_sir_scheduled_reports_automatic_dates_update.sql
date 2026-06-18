SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_reports_automatic_dates_update'
GO

CREATE PROCEDURE spu_sir_scheduled_reports_automatic_dates_update
    @report_scheduler_id INT,
    @start_date VARCHAR(20),
    @end_date VARCHAR(20)
AS

BEGIN

	UPDATE Report_Scheduler_Parameters
	SET default_value = @start_date
	WHERE report_scheduler_id = @report_scheduler_id AND
	parameter_name="start_date"

	UPDATE Report_Scheduler_Parameters
	SET default_value = @end_date
	WHERE report_scheduler_id = @report_scheduler_id AND
	parameter_name="end_date"

END

GO