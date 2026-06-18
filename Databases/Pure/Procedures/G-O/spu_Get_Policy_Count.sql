SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_Count'
GO

CREATE PROCEDURE spu_Get_Policy_Count
   @insurance_file_cnt Int
AS
SELECT 	COUNT(DISTINCT Trans.pfprem_finance_cnt)
FROM 		PFTransaction_id Trans
INNER JOIN 	PFPremiumFinance PFPLan 
	ON PFPlan.pfprem_finance_cnt=Trans.pfprem_finance_cnt
	AND PFPlan.pfprem_finance_version=Trans.pfprem_finance_version
WHERE 	(PFPlan.StatusInd='040' OR PFPlan.StatusInd='140')
AND	Trans.insurance_file_cnt=@insurance_file_cnt
GO
