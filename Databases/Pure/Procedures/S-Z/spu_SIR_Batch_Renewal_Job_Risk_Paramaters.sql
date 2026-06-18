SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_Risk_Paramaters'
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 12/09/2014
-- Description:	Gets the renewal parameters for a risk
-- =============================================
CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_Risk_Paramaters
	@batch_id	int,
	@insurance_folder_cnt int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  risk_folder_cnt,
            rerate,
            recalculate_reinsurance,
			recalculate_fees,
			recalculate_taxes
	FROM	Batch_Renewal_Job_Run_Risk  
	WHERE	batch_id = @batch_id
	AND		insurance_folder_cnt = @insurance_folder_cnt
 
END
