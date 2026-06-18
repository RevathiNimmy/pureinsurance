SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_children'
GO
/* Select Instalments using the parent plan information for merging instalments together */
CREATE PROCEDURE spe_PFInstalments_children
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT
AS

SELECT
    I.DueDate,
    I.Fee,
    I.Amount
FROM
    PFInstalments I
    INNER JOIN
        PFPremiumFinance P
    ON
        P.pfprem_finance_cnt = I.pfprem_finance_cnt
    AND P.pfprem_finance_version = I.pfprem_finance_version
WHERE
    P.parent_finance_cnt = @pfprem_finance_cnt
AND P.parent_finance_version = @pfprem_finance_version
AND I.Status IN (1,2,5,7) AND I.TransactionCode > 2
ORDER BY
    I.DueDate
GO

