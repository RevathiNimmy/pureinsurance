SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit_currency'
GO
--DC201101 added bank as an extra parameter
CREATE PROCEDURE spu_Report_Daily_Audit_currency
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @bank varchar(60)
AS
    EXECUTE spu_Report_ReceiptPayment_currency @branch_id, @start_date, @end_date, @bank
GO

