SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_saa_old'
GO
CREATE PROCEDURE spe_PFInstalments_saa_old
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @batchnumber INT = NULL,
    @duedate DATETIME = NULL,
    @filter INT = 0
AS

SELECT
    I.pfprem_finance_cnt,
    I.pfprem_finance_version,
    I.InstalmentNumber,
    I.DueDate,
    I.Fee,
    I.Amount,
    I.TransactionCode,
    I.Status,
    I.BatchNumber,
    I.BatchExportDate,
    I.PostedDate,
    P.NoOfInstallments,
    (SELECT
        Count(pfprem_finance_cnt)
    FROM
        PFInstalments PFI
    WHERE
        PFI.pfprem_finance_cnt = I.pfprem_finance_cnt
    AND PFI.pfprem_finance_cnt = I.pfprem_finance_version
    AND PFI.Status <> 1
    AND PFI.TransactionCode > 2) AS InstalmentsProcessed,
    I.PFTransaction_id
FROM
    PFInstalments I
INNER JOIN
    PFPremiumFinance P
    ON
        P.pfprem_finance_cnt = I.pfprem_finance_cnt
    AND P.pfprem_finance_version = I.pfprem_finance_version
WHERE
    I.pfprem_finance_cnt = @pfprem_finance_cnt
AND I.pfprem_finance_version = @pfprem_finance_version
AND ((@filter = 0) OR -- All records
    (I.TransactionCode > 2 AND @filter = 1) OR -- Filter out DDM create/delete
    ((I.Status = 1 OR I.Status = 5) AND @filter = 2 AND I.TransactionCode > 2) OR -- Only Unpaid (New or Retry)
    (I.Status <> 1 AND @filter = 3 AND I.TransactionCode > 2)) -- Only Actioned
AND (I.BatchNumber = @batchnumber OR @batchnumber IS NULL) -- Just those in a batch
AND (I.DueDate <= @DueDate OR @DueDate IS NULL) -- Just those on or before a due date
ORDER BY
    I.InstalmentNumber

GO

