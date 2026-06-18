SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_transaction_export_complete_sel'
GO

CREATE PROCEDURE spu_transaction_export_complete_sel
	@transaction_export_folder_cnt int
AS
SELECT transaction_export_folder_cnt
FROM   transaction_export_complete 
WHERE  transaction_export_folder_cnt = @transaction_export_folder_cnt

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO