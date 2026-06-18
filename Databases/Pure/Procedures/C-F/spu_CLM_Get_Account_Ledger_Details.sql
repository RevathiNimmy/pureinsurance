SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Account_Ledger_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Account_Ledger_Details

@account_id int

AS

BEGIN
	SELECT a.ledger_id, l.ledger_short_name
	FROM Account a, Ledger l
	WHERE a.account_id = @account_id
	AND l.ledger_id = a.ledger_id
END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
