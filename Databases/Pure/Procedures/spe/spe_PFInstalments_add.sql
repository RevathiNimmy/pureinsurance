SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_add'
GO
CREATE PROCEDURE spe_PFInstalments_add
    @pfprem_finance_cnt int,
    @pfprem_finance_version int,
    @InstalmentNumber int,
    @DueDate datetime,
    @Fee numeric(19,4),
    @Amount numeric(19,4),
    @TransactionCode int,
    @Status int,
    @BatchNumber int = NULL,
    @BatchExportDate datetime = NULL,
    @PostedDate datetime = NULL,
    @PFTransaction_id int = NULL
AS

INSERT INTO
    PFInstalments(
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
    PFTransaction_id)
VALUES (
    @pfprem_finance_cnt,
    @pfprem_finance_version,
    @InstalmentNumber,
    @DueDate,
    @Fee,
    @Amount,
    @TransactionCode,
    @Status,
    @BatchNumber,
    @BatchExportDate,
    @PostedDate,
    @PFTransaction_id)
GO

