SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_batchfrequencyparam_sel'
GO

CREATE PROCEDURE spu_sir_scheduled_batchfrequencyparam_sel --55-- 5    
 @batch_scheduler_id INT = NULL    
AS      
    Begin  
  
select a.batch_scheduler_id,a.frequency, b.parameter_name,b.currentid_value,frequency_type  FROM batch_scheduler a   
join Batch_Scheduler_FrequencyParameters b on a.batch_scheduler_id=b.batch_scheduler_id  
where a.batch_scheduler_id=@batch_Scheduler_id  
    
END 