SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_add'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 16/09/2014
-- Description:	Gets the renewal parameters for a risk
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_add
	@batch_id INT,
	@insurance_folder_cnt INT,
	@batch_renewal_job_id INT,
	@recalculate_commission	BIT = 0,
	@recalculate_fees BIT = 0,
	@recalculate_taxes BIT = 0,
	@insurance_file_cnt INT = NULL
AS
BEGIN

	INSERT INTO Batch_Renewal_Job_Run_Insurance_Folder(
		batch_id,
		insurance_folder_cnt,
		batch_renewal_job_id,
		recalculate_commission,
		recalculate_fees,
		recalculate_taxes,
		batch_renewal_job_run_status_id,
		new_insurance_file_cnt
		)
	VALUES (
		@batch_id,
		@insurance_folder_cnt,
		@batch_renewal_job_id,
		@recalculate_commission,
		@recalculate_fees,
		@recalculate_taxes,
		1, -- Ready For processing,
		@insurance_file_cnt
		)

END

