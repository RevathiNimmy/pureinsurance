SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_reports_update'
GO

CREATE PROCEDURE spu_sir_scheduled_reports_update  
 @report_scheduler_id INT = NULL,  
 @report_scheduler_parameter_id INT = NULL,  
 @frequency VARCHAR(50) = '',  
 @export_pdf TINYINT = 0,  
 @archieve_pdf TINYINT = 0,  
 @export_csv TINYINT = 0,  
 @is_automatic TINYINT = 0,
 @seprateby VARCHAR(255) = '' 
AS  
  
IF ISNULL(@report_scheduler_id,0) > 0  
BEGIN  
 UPDATE Report_Scheduler  
 SET frequency = @frequency,  
  export_pdf = @export_pdf,  
  archieve_pdf = @archieve_pdf,  
  export_csv = @export_csv,
  seprateby = @seprateby  
 WHERE report_scheduler_id = @report_scheduler_id  
END  
  
ELSE  
BEGIN  
 UPDATE Report_Scheduler_Parameters  
 SET is_automatic = @is_automatic  
 WHERE report_scheduler_parameter_id = @report_scheduler_parameter_id  
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

