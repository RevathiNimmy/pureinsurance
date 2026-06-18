SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_Arrears'
GO

CREATE PROCEDURE spu_PFPremiumFinance_Arrears
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT

AS
SELECT
    SUM(Amount)
FROM
    PFInstalments
WHERE
    pfprem_finance_cnt = @pfprem_finance_cnt
AND pfprem_finance_version = @pfprem_finance_version
AND (Status = 1 OR Status = 5 OR Status=6)
AND DueDate <= getdate()
GO