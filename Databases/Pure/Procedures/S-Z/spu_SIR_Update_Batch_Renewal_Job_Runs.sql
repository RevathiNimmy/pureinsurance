--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 28/07/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
EXECUTE DDLDropProcedure 'spu_SIR_Update_Batch_Renewal_Job_Runs'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_SIR_Update_Batch_Renewal_Job_Runs (  
     @batch_renewal_job_runs_id INT,  
     @failure_reason VARCHAR(500),  
     @is_failed TINYINT,
     @insurance_file_cnt INT)
    
AS  
BEGIN  
  
 UPDATE Batch_Renewal_Job_Runs SET  
     failure_reason = @failure_reason,is_failed = @is_failed  
 WHERE batch_renewal_job_runs_id = @batch_renewal_job_runs_id AND insurance_file_cnt = @insurance_file_cnt
END  
GO