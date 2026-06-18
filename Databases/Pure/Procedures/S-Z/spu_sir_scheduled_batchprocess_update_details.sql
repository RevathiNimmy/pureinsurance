EXECUTE DDLDropProcedure 'spu_sir_scheduled_batchprocess_update_details'
GO

CREATE PROCEDURE spu_sir_scheduled_batchprocess_update_details  
 @process VARCHAR(100) = NULL,   
@description VARCHAR(200),   
 @frequency VARCHAR(50) = NULL,    
 @frequencyDescription VARCHAR(max),  
 @batch_scheduler_id INT,
 @batch_file_name varchar(200)

AS  
  
  UPDATE Batch_Scheduler SET process=@process
  ,description=@description
  ,frequency=@frequency
  ,frequencyDescription=@frequencyDescription
  ,batch_file_name=@batch_file_name
  WHERE batch_scheduler_id=@batch_scheduler_id

  