
EXECUTE DDLDropProcedure 'spu_ACT_Get_Party_Name_From_Account'
GO

CREATE PROCEDURE spu_ACT_Get_Party_Name_From_Account
@account_id INT
AS
SELECT p.shortname FROM account ac
		INNER JOIN party p ON p.party_cnt = ac.account_key
WHERE account_id = @account_id


