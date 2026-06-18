SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PFPremiumFinance_settlement'
GO
CREATE PROCEDURE spe_PFPremiumFinance_settlement
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @SettleDate DATETIME,
    @SettleAmount NUMERIC(19,4) OUTPUT,
    @Refund NUMERIC(19,4) OUTPUT
AS

SELECT
    @SettleAmount = SUM(Amount) - SUM(Fee), @Refund = SUM(Fee)
FROM
    PFInstalments
WHERE
    pfprem_finance_cnt = @pfprem_finance_cnt
AND pfprem_finance_version = @pfprem_finance_version
AND DueDate > @SettleDate
AND (Status = 1 OR Status = 5)
GO

