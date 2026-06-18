/* Get all the uncommitted records from accounts_party_queue table */
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_accounts_party_queue_sel'
GO

CREATE PROCEDURE spu_accounts_party_queue_sel
AS

SELECT 	party_cnt, create_date, commit_ind, commit_date
FROM 	accounts_party_queue
WHERE  	commit_ind = 0

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO