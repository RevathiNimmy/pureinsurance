SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_saa'
GO
/*
    Return Premium Finance Instalments

    History
        DD000801 - Created
        PF060901 - Optimised
*/
CREATE PROCEDURE spe_PFInstalments_saa
    @pfprem_finance_cnt int,
    @pfprem_finance_version int,
    @batchnumber int = Null,
    @duedate datetime = Null,
    @filter int = 0
AS

    DECLARE @InstalmentCount int
    DECLARE @InstalmentsProcessed int

    /* Get count of instalments */
    SELECT @InstalmentCount = NoOfInstallments
    FROM PFPremiumFinance
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt
    AND pfprem_finance_version = @pfprem_finance_version

    /* Get count of processed instalments */
    SELECT @InstalmentsProcessed = Count(*)
    FROM PFInstalments
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt
    AND pfprem_finance_version = @pfprem_finance_version
    AND Status <> 1
    AND TransactionCode > 2

    /* Lightly Optimised */
    SELECT
        pfprem_finance_cnt,
        pfprem_finance_version,
        InstalmentNumber,
        DueDate,
        Fee,
        Amount,
        TransactionCode,
        Status,
        BatchNumber,
        BatchExportDate,
        PostedDate,
        @InstalmentCount As NoOfInstallments,
        @InstalmentsProcessed As InstalmentsProcessed,
        PFTransaction_id
    FROM
        PFInstalments
    WHERE
        pfprem_finance_cnt = @pfprem_finance_cnt
    AND
        pfprem_finance_version = @pfprem_finance_version
    AND (
        (@filter = 0) -- All records
    OR
        (@filter = 1 AND TransactionCode > 2) -- Filter out DDM create/delete
    OR
        (@filter = 2 AND TransactionCode > 2 AND Status IN (1,5)) -- Only Unpaid (New or Retry)
    OR
        (@filter = 3 AND TransactionCode > 2 AND Status <> 1)) -- Only Actioned
    AND
        (BatchNumber = @batchnumber OR @batchnumber IS NULL) -- Just those in a batch
    AND
        (DueDate < = @duedate OR @duedate IS NULL) -- Just those on or before a due date
    ORDER BY
        InstalmentNumber

    /*
        All where clauses that compare variables with constants should be optimised
        out of the query for maximum performance. I'm in a rush at the moment and
        there are a hell of a lot of combinations here so i'm not doing it
    */

GO

