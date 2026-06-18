SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_GetPlanPaid'
GO


CREATE PROCEDURE spu_SIR_GetPlanPaid
    @lPremFinanceCnt INT,
    @lPremFinanceVersion INT
AS

DECLARE @lStatusCollected INT
SELECT @lStatusCollected = 3

DECLARE @crTotalCollected currency

--Calculate the sum of the collected instalments
SELECT
    @crTotalCollected = ISNULL(SUM (Amount), 0)
FROM
    PFInstalments
WHERE
    pfprem_finance_cnt = @lPremFinanceCnt
AND
    pfprem_finance_version = @lPremFinanceVersion
AND
    Status = @lStatusCollected

DECLARE @crTotalCost currency

--Get the total cot of the plan
SELECT
    @crTotalCost = ISNULL(TotalCost, 0)
FROM
    PFPremiumFinance
WHERE
    pfprem_finance_cnt = @lPremFinanceCnt
AND
    pfprem_finance_version = @lPremFinanceVersion

--Return the data
SELECT
    @crTotalCollected 'Total Collected',
    @crTotalCost 'Total Cost'

GO
