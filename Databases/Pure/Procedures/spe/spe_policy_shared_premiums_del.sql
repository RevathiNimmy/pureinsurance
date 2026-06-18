SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_policy_shared_premiums_del'
GO
 
CREATE PROCEDURE spe_policy_shared_premiums_del
		@insurance_file_cnt 	int

AS
BEGIN
	DELETE
	FROM
	policy_shared_premiums 
	WHERE 
	insurance_file_cnt=@insurance_file_cnt
END
GO
