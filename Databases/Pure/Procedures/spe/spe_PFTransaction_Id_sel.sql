SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFTransaction_Id_sel'
GO
CREATE PROCEDURE spe_PFTransaction_Id_sel
    @financeplancnt int,
    @financeplanversion int
AS
BEGIN
SELECT
    pfTransaction_Id
FROM PFTransaction_ID
WHERE @financeplancnt = pfprem_finance_cnt
AND @financeplanversion = pfprem_finance_version

END
GO

