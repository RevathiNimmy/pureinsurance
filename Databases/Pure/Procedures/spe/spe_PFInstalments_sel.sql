SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_sel'
GO
CREATE PROCEDURE spe_PFInstalments_sel
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @InstalmentNumber INT = NULL
AS

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
    PFTransaction_id
FROM
    PFInstalments
WHERE
    pfprem_finance_cnt = @pfprem_finance_cnt
AND pfprem_finance_version = @pfprem_finance_version
AND	(InstalmentNumber=@InstalmentNumber OR (@InstalmentNumber is NULL and InstalmentNumber > 0))

GO

