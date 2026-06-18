SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_MarkPlanAsSaved'
GO

CREATE PROCEDURE spu_MarkPlanAsSaved
	@insurance_file_cnt int
AS

BEGIN

	UPDATE PFPremiumFinance
	SET StatusInd = '010'
	WHERE insurance_file_cnt = @insurance_file_cnt
		And (StatusInd ='000' OR StatusInd = '140')

END
GO
