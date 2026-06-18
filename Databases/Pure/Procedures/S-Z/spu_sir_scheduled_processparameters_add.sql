
EXECUTE DDLDropProcedure 'spu_sir_scheduled_processparameters_add'
GO
CREATE  PROCEDURE spu_sir_scheduled_processparameters_add
@batchprocesses_list_id	INT,
 @batch_scheduler_id INT,  
@tblProcessParameters BatchProcessParameterDetailType READONLY
 
AS  
  
  INSERT INTO Batch_Scheduler_ScheduledProcessParameters  
 ( batch_scheduler_id, control_name, default_value, data_type, currentid_value)  
 SELECT @batch_scheduler_id,ParameterName,DefaultValue,DataType,CurrentValue FROM @tblProcessParameters 
 

