SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_accounts_party_queue_add'
GO

CREATE PROCEDURE spu_accounts_party_queue_add
	@party_cnt int,
	@create_date datetime,
	@commit_ind tinyint,
	@commit_date datetime
 AS

INSERT INTO accounts_party_queue (
	party_cnt,
	create_date,
	commit_ind,
	commit_date )
VALUES (
	@party_cnt,
	@create_date,
	@commit_ind,
	@commit_date )

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO