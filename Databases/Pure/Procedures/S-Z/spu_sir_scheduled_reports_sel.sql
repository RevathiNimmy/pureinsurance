SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_reports_sel'
GO

CREATE PROCEDURE spu_sir_scheduled_reports_sel  
 @report_scheduler_id INT = NULL,  
 @frequency   VARCHAR(50) = NULL  
AS  
  
IF @frequency IS NOT NULL  
BEGIN  
 SELECT  
  rpts.report_scheduler_id,  
  rpts.report_id,  
  rpt.description,  
  rpts.frequency,  
  rpts.export_pdf,  
  rpts.archieve_pdf,  
  rpts.export_csv,  
  rpts.reportpath,
  rpts.seprateby  
 FROM report_scheduler rpts  
 INNER JOIN report rpt  
 ON rpts.report_id = rpt.report_id  
 WHERE UPPER(rpts.frequency) = UPPER(@frequency)  
END  
ELSE  
BEGIN  
 IF @report_scheduler_id IS NULL  
 BEGIN  
  SELECT  
   rpts.report_scheduler_id,  
   rpts.report_id,  
   rpt.description,  
   rpts.frequency,  
   rpts.export_pdf,  
   rpts.archieve_pdf,  
   rpts.export_csv,
   rpts.reportpath,
   rpts.seprateby    
  FROM report_scheduler rpts  
  INNER JOIN report rpt  
  ON rpts.report_id = rpt.report_id  
 END  
 ELSE  
 BEGIN  
  SELECT  
   rpts.report_scheduler_id,  
   rpts.report_id,  
   rpt.description,  
   rpts.frequency,  
   rpts.export_pdf,  
   rpts.archieve_pdf,  
   rpts.export_csv,
   rpts.reportpath,
   rpts.seprateby     
  FROM report_scheduler rpts  
  INNER JOIN report rpt  
  ON rpts.report_id = rpt.report_id  
  WHERE rpts.report_scheduler_id = @report_scheduler_id  
 END  
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO