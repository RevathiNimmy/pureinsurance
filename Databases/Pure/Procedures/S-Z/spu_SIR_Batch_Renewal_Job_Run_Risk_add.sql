SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Run_Risk_add'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date:16/09/2014
--Description: Insert data into table Batch_Renewal_Job_Run_Risk
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Run_Risk_add
	@batch_id INT,
	@insurance_folder_cnt INT,
	@risk_folder_cnt INT,
	@rerate	BIT,
	@recalculate_reinsurance BIT,
	@recalculate_fees BIT,
	@recalculate_taxes BIT
AS
BEGIN

	INSERT INTO Batch_Renewal_Job_Run_Risk(
		batch_id,
		insurance_folder_cnt,
		risk_folder_cnt,
		rerate,
		recalculate_reinsurance,
		recalculate_fees,
		recalculate_taxes
		)
	VALUES (
		@batch_id,
		@insurance_folder_cnt,
		@risk_folder_cnt,
		@rerate,
		@recalculate_reinsurance,
		@recalculate_fees,
		@recalculate_taxes
		)

END

