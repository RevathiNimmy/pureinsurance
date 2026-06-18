SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_batchprocesses_delete'
GO
CREATE  PROCEDURE spu_sir_scheduled_batchprocesses_delete  
 @batch_scheduler_id INT  
AS  
  
DELETE FROM Batch_Scheduler_FrequencyParameters WHERE batch_scheduler_id = @batch_scheduler_id  
DELETE FROM Batch_Scheduler_ScheduledProcessParameters WHERE batch_scheduler_id = @batch_scheduler_id  
DELETE FROM Batch_Scheduler WHERE batch_scheduler_id = @batch_scheduler_id 