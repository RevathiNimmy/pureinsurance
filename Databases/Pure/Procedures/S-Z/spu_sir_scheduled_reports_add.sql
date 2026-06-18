SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_sir_scheduled_reports_add'
GO
CREATE  PROCEDURE spu_sir_scheduled_reports_add	
	@report_name VARCHAR(100) = NULL,
	@frequency VARCHAR(50) = NULL,
	@reportpath VARCHAR(100) = '',
	@report_scheduler_id INT OUTPUT
AS

DECLARE @report_id INT

SELECT @report_id = report_id FROM Report WHERE [description] = @report_name

INSERT INTO Report_Scheduler (report_id, frequency, reportpath)
	VALUES(@report_id, @frequency, @reportpath)
	SELECT @report_scheduler_id = @@IDENTITY
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

