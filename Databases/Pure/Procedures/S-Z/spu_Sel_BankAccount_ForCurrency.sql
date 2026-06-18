SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_Sel_BankAccount_ForCurrency
GO

CREATE PROCEDURE spu_Sel_BankAccount_ForCurrency
@currency_id INT
AS
SELECT bank_account_name,bankaccount_id FROM bankaccount 
WHERE currency_id=@currency_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO