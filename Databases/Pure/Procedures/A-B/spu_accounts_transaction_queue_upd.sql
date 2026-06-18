SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_accounts_transaction_queue_upd'
GO

CREATE PROCEDURE spu_accounts_transaction_queue_upd
	@transaction_export_folder_cnt int,
	@create_date datetime,
	@commit_ind tinyint,
	@commit_date datetime
 AS

UPDATE accounts_transaction_queue SET
	create_date = @create_date,
	commit_ind = @commit_ind,
	commit_date = @commit_date
WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO