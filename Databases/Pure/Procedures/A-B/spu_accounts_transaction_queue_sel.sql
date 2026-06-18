/* Get all the uncommitted records from accounts_transaction_queue table */
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_accounts_transaction_queue_sel'
GO

CREATE PROCEDURE spu_accounts_transaction_queue_sel
AS

SELECT 	transaction_export_folder_cnt, create_date, commit_ind, commit_date
FROM 	accounts_transaction_queue
WHERE  	commit_ind = 0

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO