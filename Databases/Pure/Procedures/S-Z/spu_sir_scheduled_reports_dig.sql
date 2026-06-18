SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_sir_scheduled_reports_dig'
GO
CREATE  PROCEDURE spu_sir_scheduled_reports_dig	
	@report_name VARCHAR(100)
AS

DECLARE @report_id INT

SELECT @report_id = report_id FROM Report WHERE [description] = @report_name

SELECT report_scheduler_id FROM Report_Scheduler WHERE report_id = @report_id
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

