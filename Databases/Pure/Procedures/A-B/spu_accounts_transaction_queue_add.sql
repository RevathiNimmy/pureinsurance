SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_accounts_transaction_queue_add'
GO

CREATE PROCEDURE spu_accounts_transaction_queue_add
	@transaction_export_folder_cnt int,
	@create_date datetime,
	@commit_ind tinyint,
	@commit_date datetime
 AS

INSERT INTO accounts_transaction_queue (
	transaction_export_folder_cnt,
	create_date,
	commit_ind,
	commit_date )
VALUES (
	@transaction_export_folder_cnt,
	@create_date,
	@commit_ind,
	@commit_date )

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO