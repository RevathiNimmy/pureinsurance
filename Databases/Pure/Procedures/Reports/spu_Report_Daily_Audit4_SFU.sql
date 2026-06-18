SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit4_SFU'
GO
CREATE PROCEDURE spu_Report_Daily_Audit4_SFU
    @branch_id int
AS
BEGIN
    DECLARE @start_date datetime
    DECLARE @end_date datetime

    SELECT @start_date = CONVERT(datetime, CONVERT(char(8), GETDATE(), 112), 112)
    SELECT @end_date = DATEADD(second, -1, (DATEADD(day, 1, @start_date)))

    EXECUTE spu_Report_ReceiptPayment_SFU @branch_id, @start_date, @end_date
END
GO

