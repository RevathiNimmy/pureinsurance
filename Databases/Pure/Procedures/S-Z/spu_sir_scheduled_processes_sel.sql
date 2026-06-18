EXECUTE DDLDropProcedure 'spu_sir_scheduled_processes_sel'
GO
CREATE PROCEDURE spu_sir_scheduled_processes_sel    
 @batch_scheduler_id INT = NULL      
AS        
        
        
 BEGIN        
   SELECT        
  bs.batch_scheduler_id,        
  bs.Process,       
  bs.description AS ProcessDescription,  
  bs.frequencyDescription AS FrequencyDescription,
  bs.batch_file_name AS BatchFileName  
  --rpts.batchpath,      
 FROM batch_scheduler bs        
 WHERE bs.batch_scheduler_id = @batch_scheduler_id        
      
END 