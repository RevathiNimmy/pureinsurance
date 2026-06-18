
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_batchprocess_parameter_sel'
GO

CREATE PROCEDURE spu_sir_batchprocess_parameter_sel  
 @batch_scheduler_id INT
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  if @batch_scheduler_id <> 0
  BEGIN
 
 SELECT a.control_name,a.currentid_value,b.process,b.frequency 
 FROM  Batch_Scheduler_ScheduledProcessParameters a
 join Batch_Scheduler b on a.batch_scheduler_id=b.batch_scheduler_id
 
 WHERE a.batch_scheduler_id=@batch_scheduler_id
 END

END  