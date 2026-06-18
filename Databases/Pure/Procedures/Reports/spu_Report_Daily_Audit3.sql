SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit3'
GO
--DC201101 added bank as an extra parameter
CREATE PROCEDURE spu_Report_Daily_Audit3
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @bank varchar(60)
AS
BEGIN
    EXECUTE spu_Report_ReceiptPayment @branch_id, @start_date, @end_date, @bank
END
GO

