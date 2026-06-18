SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_scheduled_batchprocesses_sel'
GO

CREATE PROCEDURE spu_sir_scheduled_batchprocesses_sel -- 5
 @batch_scheduler_id INT         = NULL,    
 @frequency          VARCHAR(50) = NULL    
AS    
    
IF @frequency IS NOT NULL    
BEGIN    
 SELECT    
  bs.batch_scheduler_id,    
  bs.batchProcesses_list_id,    
  bs.Process,   
  CASE WHEN CHARINDEX('_',bs.description)>0 THEN 
			left(bs.description,len(bs.description)-CHARINDEX('_', REVERSE(bs.description))) 
			ELSE 
			bs.description
			END AS Description,  
  bs.frequencyDescription AS FrequencyDescription,    
  --rpts.batchpath,  
 (SELECT 
   CASE currentid_value 
   WHEN 'True' THEN 'Enabled' 
   ELSE 'Disabled' END 
   FROM Batch_Scheduler_FrequencyParameters
   WHERE parameter_name='Enabled' AND batch_scheduler_id=bs.batch_scheduler_id
  ) AS Status,
 bpl.description as ProcessSelected,
 bs.description AS ProcessDescription,
 bs.batch_file_name AS BatchFileName
 FROM batch_scheduler bs    
 INNER JOIN BatchProcesses_List bpl    
 ON bs.batchprocesses_list_id = bpl.batchprocesses_list_id    
 WHERE UPPER(bs.frequency) = UPPER(@frequency)    
END    
ELSE    
BEGIN    
 IF @batch_scheduler_id IS NULL    
 BEGIN    
	  SELECT    
	  bs.batch_scheduler_id,    
	  bs.batchProcesses_list_id,    
	  bs.Process,   
	  CASE WHEN CHARINDEX('_',bs.description)>0 THEN 
			left(bs.description,len(bs.description)-CHARINDEX('_', REVERSE(bs.description))) 
			ELSE 
			bs.description
			END AS Description,  
	  bs.frequencyDescription AS FrequencyDescription,    
	  --rpts.batchpath,  
	  (SELECT 
   CASE currentid_value 
   WHEN 'True' THEN 'Enabled' 
   ELSE 'Disabled' END 
   FROM Batch_Scheduler_FrequencyParameters
   WHERE parameter_name='Enabled' AND batch_scheduler_id=bs.batch_scheduler_id
  ) AS Status,
	  bpl.description as ProcessSelected,
		bs.description AS ProcessDescription,
		bs.batch_file_name AS BatchFileName
	 FROM batch_scheduler bs    
	 INNER JOIN BatchProcesses_List bpl    
	 ON bs.batchprocesses_list_id = bpl.batchprocesses_list_id    
 END    
 ELSE    
 BEGIN    
	  SELECT    
	  bs.batch_scheduler_id,    
	  bs.batchProcesses_list_id,    
	  bs.Process,   
	  CASE WHEN CHARINDEX('_',bs.description)>0 THEN 
			left(bs.description,len(bs.description)-CHARINDEX('_', REVERSE(bs.description))) 
			ELSE 
			bs.description
			END AS Description,  
	  bs.frequencyDescription AS FrequencyDescription,    
	  --rpts.batchpath,  
	  (SELECT 
	   CASE currentid_value 
	   WHEN 'True' THEN 'Enabled' 
	   ELSE 'Disabled' END 
	   FROM Batch_Scheduler_FrequencyParameters
	   WHERE parameter_name='Enabled' AND batch_scheduler_id=bs.batch_scheduler_id
      ) AS Status,  
	  bpl.description as ProcessSelected,
	  bs.description AS ProcessDescription,
	  bs.batch_file_name AS BatchFileName
	  FROM batch_scheduler bs    
	  INNER JOIN BatchProcesses_List bpl    
	  ON bs.batchprocesses_list_id = bpl.batchprocesses_list_id    
	  WHERE bs.batch_scheduler_id = @batch_scheduler_id    
 END    
END   