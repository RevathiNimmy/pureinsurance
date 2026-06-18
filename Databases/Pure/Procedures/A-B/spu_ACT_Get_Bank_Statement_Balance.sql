EXECUTE DDLDropProcedure 'spu_ACT_Get_Bank_Statement_Balance'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_Get_Bank_Statement_Balance
	@account_id INT,
	@balance NUMERIC(19,4) OUTPUT
AS

SELECT
	@balance=bank_statement_balance
FROM
	BankAccount
WHERE
	account_id=@account_id

GO