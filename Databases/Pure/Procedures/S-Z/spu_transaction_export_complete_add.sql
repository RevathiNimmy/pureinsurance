SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_transaction_export_complete_add'
GO

CREATE PROCEDURE spu_transaction_export_complete_add
	@transaction_export_folder_cnt int
AS

INSERT INTO transaction_export_complete 
    (transaction_export_folder_cnt)
VALUES 
    (@transaction_export_folder_cnt)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO