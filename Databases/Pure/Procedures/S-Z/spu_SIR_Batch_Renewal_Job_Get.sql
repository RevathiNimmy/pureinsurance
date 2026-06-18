SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Get'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 16/09/2014
-- Description:	Gets the renewal parameters
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Get
	@batch_id  int,
	@insurance_folder_cnt int
	
AS
BEGIN
		
		SELECT	Batch_Renewal_Job_Run_Insurance_Folder.batch_id,
				Batch_Renewal_Job_Run_Insurance_Folder.insurance_folder_cnt,
				Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_id,
				Batch_Renewal_Job_Run_Insurance_Folder.new_insurance_file_cnt,
				Batch.comment,
				Batch_Renewal_Job_Type.code 'job_type',
				(SELECT COUNT(insurance_folder_cnt) 
				 FROM   Batch_Renewal_Job_Run_Insurance_Folder
				 WHERE Batch_Renewal_Job_Run_Insurance_Folder.batch_id = @batch_id) 'records_in_batch'
		FROM	Batch_Renewal_Job_Run_Insurance_Folder
		INNER JOIN Batch	
			 ON Batch_Renewal_Job_Run_Insurance_Folder.batch_id = Batch.batch_id
		INNER JOIN Batch_Renewal_Job
			INNER JOIN Batch_Renewal_Job_Type
			 ON Batch_Renewal_Job_Type.batch_renewal_job_type_id =Batch_Renewal_Job.batch_renewal_job_type_id
			 ON Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_id  = Batch_Renewal_Job.batch_renewal_job_id
		WHERE	Batch_Renewal_Job_Run_Insurance_Folder.batch_id = @batch_id
		AND	Batch_Renewal_Job_Run_Insurance_Folder.insurance_folder_cnt = @insurance_folder_cnt
		
END
