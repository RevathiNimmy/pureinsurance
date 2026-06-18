SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_StructAccountId'
GO


CREATE PROCEDURE spu_ACT_Update_StructAccountId
    @node_id int,
    @account_id int = NULL
AS

-- PWF 31/07/2002 - compare company_id for node and 
--   parent_node to ensure compatibility
DECLARE
    @company_id int,
    @account_company_id int

-- Check for Null and set automatically
IF @account_id IS NULL
    UPDATE StructureTree
    SET    account_id = @account_id
    WHERE  node_id = @node_id
ELSE
BEGIN
    -- Validate the company id's
    SELECT @company_id = company_id
    FROM   StructureTree
    WHERE  node_id = @node_id
    
    SELECT @account_company_id = company_id
    FROM   Account
    WHERE  account_id = @account_id
    
    -- If id's match update, else raise an error
    IF @company_id = @account_company_id
        UPDATE StructureTree
        SET    account_id = @account_id
        WHERE  node_id = @node_id
    ELSE
        RAISERROR ('Company mismatch for structure tree account', 16, 1)
END

GO


