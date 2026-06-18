SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_AccountSubBranch'
GO

CREATE PROCEDURE spu_Get_AccountSubBranch
    @account_id Int
AS
SELECT sub_branch_id,company_id
FROM account 
WHERE account_id=@account_id
GO
