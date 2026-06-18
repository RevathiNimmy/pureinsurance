SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO
		  
EXECUTE DDLDropProcedure 'spu_sir_update_batchprocess_parameters_details'
GO				  
CREATE  PROCEDURE spu_sir_update_batchprocess_parameters_details
@batch_scheduler_id INT,  
@tblProcessParameters BatchProcessParameterDetailType READONLY,
@tblBatchFrequency BatchFrequencyDetailType READONLY
AS
BEGIN

 -----------Process Parameters DML Operations
 DELETE FROM Batch_Scheduler_ScheduledProcessParameters 
 WHERE batch_scheduler_id=@batch_scheduler_id AND control_name
 NOT IN (
 SELECT ParameterName
 FROM @tblProcessParameters
 )


UPDATE Batch_Scheduler_ScheduledProcessParameters 
  SET control_name=tbl.ParameterName,
  default_value=tbl.DefaultValue,
  data_type=tbl.DataType,
  currentid_value=tbl.CurrentValue
FROM
  @tblProcessParameters tbl INNER JOIN Batch_Scheduler_ScheduledProcessParameters bsp
  ON bsp.control_name=tbl.ParameterName 
  WHERE batch_scheduler_id=@batch_scheduler_id

 INSERT INTO Batch_Scheduler_ScheduledProcessParameters    
 ( batch_scheduler_id, control_name, default_value, data_type, currentid_value)    
 SELECT @batch_scheduler_id,ParameterName,DefaultValue,DataType,CurrentValue FROM @tblProcessParameters
 WHERE ParameterName 
 NOT IN (
 SELECT control_name FROM
 Batch_Scheduler_ScheduledProcessParameters 
 WHERE batch_scheduler_id=@batch_scheduler_id
 )
 
 
 -----------Frequency DML Operations
 DELETE FROM Batch_Scheduler_FrequencyParameters 
 WHERE batch_scheduler_id=@batch_scheduler_id 

 INSERT INTO Batch_Scheduler_FrequencyParameters    
 ( batch_scheduler_id, frequency_type, parameter_name, default_value, data_type, currentid_value)    
 SELECT @batch_scheduler_id, FrequencyType, ParameterName,DefaultValue,DataType,CurrentValue FROM @tblBatchFrequency
 --WHERE ParameterName NOT IN (SELECT ParameterName FROM
 --Batch_Scheduler_ScheduledProcessParameters WHERE batch_scheduler_id=@batch_scheduler_id)
 
 
END


