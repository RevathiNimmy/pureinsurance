SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_batch_add'
GO
CREATE  PROCEDURE spu_sir_scheduled_batch_add  --2,'Renewal Invitation','RI1 Renewal Invitation','Weekly','Weekly starting on 8/5/2021 12:00:00 AM on every 1(Sunday, Wednesday, Saturday)',0
@batchprocesses_list_id INT,
@process VARCHAR(100) = NULL, 
@description VARCHAR(200), 
@frequency VARCHAR(50) = NULL,  
@frequencyDescription VARCHAR(max),
@batch_file_name VARCHAR(200),
@batch_scheduler_id INT OUTPUT  
AS  
  
  
INSERT INTO Batch_Scheduler (batchprocesses_list_id,process,description, frequency,frequencyDescription,is_deleted,batch_file_name)  
 VALUES(@batchprocesses_list_id,@process,@description, @frequency,@frequencyDescription,0,@batch_file_name)  
 SELECT @batch_scheduler_id = @@IDENTITY  
