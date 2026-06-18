SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_upd'
GO
CREATE PROCEDURE spe_PFInstalments_upd
    @pfprem_finance_cnt int,
    @pfprem_finance_version int,
    @InstalmentNumber int,
    @DueDate datetime,
    @Fee numeric(19,4),
    @Amount numeric(19,4),
    @TransactionCode int,
    @Status int,
    @BatchNumber int,
    @BatchExportDate datetime,
    @PostedDate datetime,
    @PFTransaction_id INT
AS

UPDATE
    PFInstalments
SET
    DueDate = @DueDate,
    Fee = @Fee,
    Amount = @Amount,
    TransactionCode = @TransactionCode,
    Status = @Status,
    BatchNumber = @BatchNumber,
    BatchExportDate = @BatchExportDate,
    PostedDate = @PostedDate,
    PFTransaction_id = @PFTransaction_id
WHERE
    pfprem_finance_cnt = @pfprem_finance_cnt
AND pfprem_finance_version = @pfprem_finance_version
AND InstalmentNumber = @InstalmentNumber

GO

