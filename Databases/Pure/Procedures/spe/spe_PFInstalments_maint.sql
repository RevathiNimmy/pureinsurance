SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_maint'
GO
/*
    Return Premium Finance Instalments

    Return Array:
         0 - pfprem_finance_cnt
         1 - pfprem_finance_version
         2 - BatchNumber
         3 - InstalmentNumber
         4 - Amount
         5 - Fee
         6 - DueDate
         7 - BatchExportDate
         8 - TransactionCode
         9 - Status
        10 - PostedDate

    History
        PF060901 - Created
*/
CREATE PROCEDURE spe_PFInstalments_maint
    @pfprem_finance_cnt int,
    @pfprem_finance_version int
AS
    SELECT
        pfprem_finance_cnt,
        pfprem_finance_version,
        BatchNumber,
        InstalmentNumber,
        Amount,
        Fee,
        DueDate,
        BatchExportDate,
        TransactionCode,
        Status,
        PostedDate
    FROM
        PFInstalments
    WHERE
        pfprem_finance_cnt = @pfprem_finance_cnt
    AND
        pfprem_finance_version = @pfprem_finance_version
    ORDER BY
        InstalmentNumber

GO

