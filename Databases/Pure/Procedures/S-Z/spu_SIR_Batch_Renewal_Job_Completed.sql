SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Completed'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 12/09/2014
-- Description:	Sets the job for man insurance folder as completed (Or Failed)
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Completed
	@batch_id int,
	@insurance_folder_cnt int,
	@batch_renewal_job_run_status_id int,
	@new_insurance_file_cnt	int = NULL,
	@message varchar(256) = NULL
	
AS
BEGIN
	
	UPDATE	Batch_Renewal_Job_Run_Insurance_Folder 
	SET	batch_renewal_job_run_status_id = @batch_renewal_job_run_status_id,
		new_insurance_file_cnt = @new_insurance_file_cnt,
		[message] = @message
	WHERE	batch_id = @batch_id
	AND	insurance_folder_cnt = @insurance_folder_cnt
 
END
