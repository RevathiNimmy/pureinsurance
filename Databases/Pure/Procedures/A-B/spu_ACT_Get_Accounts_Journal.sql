SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Accounts_Journal'
GO

CREATE PROCEDURE spu_ACT_Get_Accounts_Journal
  
AS

SELECT 
    account_id
FROM account
WHERE accounttype_id IN (1, 2) /*Income and Expense*/ 

GO


