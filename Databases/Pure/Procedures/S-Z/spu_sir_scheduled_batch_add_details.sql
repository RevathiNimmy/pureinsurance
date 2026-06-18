
EXECUTE DDLDropProcedure 'spu_sir_scheduled_batch_add_details'
GO
CREATE  PROCEDURE spu_sir_scheduled_batch_add_details  
 @batch_scheduler_id INT,  
@tblBatchFrequency BatchFrequencyDetailType READONLY
 
AS  
  
 INSERT INTO Batch_Scheduler_FrequencyParameters  
 (batch_scheduler_id, frequency_type, parameter_name, default_value, data_type,  currentid_value)  
 SELECT @batch_scheduler_id,FrequencyType,ParameterName,DefaultValue,DataType,CurrentValue FROM @tblBatchFrequency 
 

