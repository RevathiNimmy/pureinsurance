SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountBranch'
GO

CREATE PROCEDURE spu_ACT_Select_AccountBranch
    @account_id int
AS
SELECT
    s.description as company_desc,
    sb.description as sub_branch
FROM source s, sub_branch sb, account a
WHERE a.account_id = @account_id
AND s.source_id = a.company_id
AND sb.sub_branch_id = a.sub_branch_id

GO
