SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_accounts_party_queue_upd'
GO

CREATE PROCEDURE spu_accounts_party_queue_upd
	@party_cnt int,
	@create_date datetime,
	@commit_ind tinyint,
	@commit_date datetime
 AS

UPDATE accounts_party_queue SET
	create_date = isnull(@create_date,create_date),
	commit_ind = isnull(@commit_ind,commit_ind),
	commit_date = isnull(@commit_date,commit_date)
WHERE party_cnt = @party_cnt

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO