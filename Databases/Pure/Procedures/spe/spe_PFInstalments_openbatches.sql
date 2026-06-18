SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_openbatches'
GO
CREATE PROCEDURE spe_PFInstalments_openbatches
    @PaymentMethod INT = NULL,
    @ForRecall TINYINT = 0
AS

SELECT
    I.BatchNumber
FROM
    PFInstalments I
INNER JOIN
        PFPremiumFinance P
    ON
        P.pfprem_finance_cnt = I.pfprem_finance_cnt
    AND P.pfprem_finance_version = I.pfprem_finance_version
INNER JOIN
        PFScheme PS
    ON
        P.CompanyNo = PS.CompanyNo
    AND P.SchemeNo = PS.SchemeNo
    AND P.SchemeVersion = PS.SchemeVersion
WHERE
    I.BatchNumber IS NOT NULL
AND ((I.PostedDate IS NULL AND I.Status = 2) OR (I.Status = 3 AND @ForRecall = 1))
AND (PS.PaymentMethod_cnt = @PaymentMethod OR @PaymentMethod IS NULL)
GROUP BY
    I.BatchNumber

GO

