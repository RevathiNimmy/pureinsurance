SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_Outstanding_sel'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 2014-09-16
--Description: Select insurance_folder_cnt from Batch_Renewal_Job_Run_Insurance_Folder table
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_Outstanding_sel
	@batch_id INT = NULL
As

BEGIN

	SELECT insurance_folder_cnt
	FROM   Batch_Renewal_Job_Run_Insurance_Folder
	WHERE  batch_id = @batch_id
	AND    batch_renewal_job_run_status_id = 1 -- Select all that are "Ready for processing" for the given batch.

END
