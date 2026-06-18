--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
EXECUTE DDLDropProcedure 'spu_SIR_Add_Batch_Renewal_Job_Runs'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIR_Add_Batch_Renewal_Job_Runs (    
     @batch_renewal_job_id INT,    
     @insurance_file_cnt INT,    
     @run_date DATETIME,    
     @failure_reason VARCHAR(500),    
     @document_printed VARCHAR(10),    
     @is_failed TINYINT,  
     @GUID VARCHAR(100),
     @Batch_Renewal_Job_Runs_ID INT OUTPUT,
     @Record_Count INT OUTPUT)
    
AS    
BEGIN    
    
 INSERT INTO Batch_Renewal_Job_Runs(    
     batch_renewal_job_id,    
     insurance_file_cnt,    
     run_date,    
     failure_reason,    
     document_printed,    
     is_failed,  
     GUID)    
  VALUES(    
     @batch_renewal_job_id,    
     @insurance_file_cnt,    
     @run_date,    
     @failure_reason,    
     @document_printed,    
     @is_failed,  
     @GUID)    
	SET @Batch_Renewal_Job_Runs_ID = @@IDENTITY
    SELECT @Record_Count = COUNT(*) FROM Batch_Renewal_Job_Runs  WHERE GUID = @GUID  
END    
GO
