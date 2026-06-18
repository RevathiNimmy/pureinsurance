
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_GetPlanPK'
GO


CREATE PROCEDURE spu_SIR_GetPlanPK
    @lPFInstalmentId INT
AS

SELECT
    pf.pfprem_finance_cnt,
    pf.pfprem_finance_version,
	pf.StatusInd,
	pf.insurance_file_cnt
FROM
	PFPremiumFinance pf
	INNER JOIN PFInstalments pfi ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt
	AND pf.pfprem_finance_version=pfi.pfprem_finance_version
WHERE
    pfinstalments_id = @lPFInstalmentId


GO
