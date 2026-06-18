EXECUTE DDLDropProcedure 'spu_SAM_is_account_deleted'
GO
CREATE PROCEDURE spu_SAM_is_account_deleted
@Account_Code VARCHAR(255),
@Is_Deleted INT OUTPUT,
@Account_Id INT OUTPUT
AS
 SELECT @Is_Deleted = ISNULL(p.is_deleted,0), @Account_Id = a.account_id FROM account a
LEFT  JOIN party p
ON p.party_cnt = a.Account_Id
WHERE a.short_code = @Account_Code
SELECT @Is_Deleted

GO
