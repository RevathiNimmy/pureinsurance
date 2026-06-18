SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_del'
GO

CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_del
	@batch_renewal_job_id INT    
AS  
BEGIN  
	
	DELETE FROM batch_renewal_job_products WHERE batch_renewal_job_id= @batch_renewal_job_id

	DELETE FROM batch_renewal_job_branches WHERE batch_renewal_job_id= @batch_renewal_job_id

	DELETE FROM batch_renewal_job_agents WHERE batch_renewal_job_id= @batch_renewal_job_id

	DELETE FROM Batch_Renewal_Job_Runs WHERE batch_renewal_job_id = @batch_renewal_job_id

	DELETE FROM batch_renewal_job WHERE batch_renewal_job_id= @batch_renewal_job_id

END

GO

