SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Start'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 2012-09-15
-- Description:	Gets the renewal parameters
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Start
	@batch_id			  int,
	@insurance_folder_cnt int,
	@insurance_file_cnt	  int
AS
BEGIN
	
	
	UPDATE	Batch_Renewal_Job_Run_Insurance_Folder 
	SET	batch_renewal_job_run_status_id = 2,
		old_insurance_file_cnt = @insurance_file_cnt
	WHERE	batch_id = @batch_id
	AND	insurance_folder_cnt = @insurance_folder_cnt
	AND 	batch_renewal_job_run_status_id = 1
	
	-- Record was found and updated
	IF @@ROWCOUNT = 1  
		SELECT	Batch_Renewal_Job_Run_Insurance_Folder.batch_id,
				Batch_Renewal_Job_Run_Insurance_Folder.insurance_folder_cnt,
				Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_id,
				Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_run_status_id,
				Batch_Renewal_Job_Run_Insurance_Folder.recalculate_commission,
				Batch_Renewal_Job_Run_Insurance_Folder.recalculate_fees,
				Batch_Renewal_Job_Run_Insurance_Folder.recalculate_taxes,
				Batch_Renewal_Job_Run_Insurance_Folder.old_insurance_file_cnt,
				Batch_Renewal_Job_Run_Insurance_Folder.new_insurance_file_cnt,
				Batch.comment	
		FROM	Batch_Renewal_Job_Run_Insurance_Folder
		INNER JOIN Batch	
			ON Batch_Renewal_Job_Run_Insurance_Folder.batch_id = Batch.batch_id
		WHERE	Batch_Renewal_Job_Run_Insurance_Folder.batch_id = @batch_id
		AND		Batch_Renewal_Job_Run_Insurance_Folder.insurance_folder_cnt = @insurance_folder_cnt
 
	
END
