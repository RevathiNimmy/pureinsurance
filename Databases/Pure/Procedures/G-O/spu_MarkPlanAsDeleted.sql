SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_MarkPlanAsDeleted'
GO

CREATE PROCEDURE spu_MarkPlanAsDeleted
	@insurance_file_cnt int
AS

BEGIN

	UPDATE PFPremiumFinance
	SET StatusInd = '000'
	WHERE insurance_file_cnt = @insurance_file_cnt

END
GO
