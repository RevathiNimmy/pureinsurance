SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountBranchId'
GO
CREATE PROCEDURE spu_ACT_Select_AccountBranchId
    @account_id int
AS
SELECT
    s.source_id as company_id
   
FROM source s,  account a
WHERE a.account_id = @account_id
AND s.source_id = a.company_id
GO
