SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_StructParentID'
GO


CREATE PROCEDURE spu_ACT_Update_StructParentID
    @node_id int,
    @parent_node_id int
AS

-- PWF 31/07/2002 - compare company_id for node and 
--   parent_node to ensure compatibility
DECLARE
    @company_id int,
    @parent_company_id int

SELECT @company_id = company_id
FROM   StructureTree
WHERE  node_id = @node_id

SELECT @parent_company_id = company_id
FROM   StructureTree
WHERE  node_id = @parent_node_id

IF @company_id = @parent_company_id
    UPDATE StructureTree
    SET    parent_node_id = @parent_node_id 
    WHERE  node_id = @node_id
ELSE
    RAISERROR ('Company mismatch for structure tree parent', 16, 1)


GO


