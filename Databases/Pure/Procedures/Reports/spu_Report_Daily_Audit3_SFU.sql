EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit3_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO



/****** Object:  Stored Procedure dbo.sp_Report_Daily_Audit3    Script Date: 16/10/00 12:18:29 ******/


--DC201101 added bank as an extra parameter


CREATE PROCEDURE spu_Report_Daily_Audit3_SFU
	@branch_id	int,
	@start_date	datetime, 
	@end_date	datetime,
	@bank		varchar(60)

AS

	EXEC spu_Report_ReceiptPayment_SFU @branch_id, @start_date, @end_date, @bank

















GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

