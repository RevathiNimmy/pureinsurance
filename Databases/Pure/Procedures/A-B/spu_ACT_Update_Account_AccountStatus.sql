SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_ACT_Update_Account_AccountStatus'
GO

CREATE PROCEDURE spu_ACT_Update_Account_AccountStatus
	@account_id INT,
	@accountstatus_id SMALLINT

AS

UPDATE	Account
SET		accountstatus_id = @accountstatus_id
WHERE	account_id = @account_id


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

