SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_sir_scheduled_reports_delete'
GO
CREATE  PROCEDURE spu_sir_scheduled_reports_delete	
	@report_scheduler_id INT	
AS

DELETE FROM Report_Scheduler_Parameters WHERE report_scheduler_id = @report_scheduler_id
DELETE FROM Report_Scheduler WHERE report_scheduler_id = @report_scheduler_id
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

