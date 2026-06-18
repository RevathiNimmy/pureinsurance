SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_StructureTree'
GO


CREATE PROCEDURE spu_ACT_Add_StructureTree
    @node_id int OUTPUT,
    @company_id int,
    @mapping_id int = NULL,
    @account_id int = NULL,
    @element_id int = NULL,
    @parent_node_id int = NULL
AS

-- If multi-tree accounting is not enabled we should use company 1.
IF NOT EXISTS (SELECT * FROM hidden_options WITH(NOLOCK) WHERE option_number = 16 AND value = 1)
    SELECT @company_id = 1

INSERT INTO StructureTree (
    company_id,
    mapping_id,
    account_id,
    element_id,
    parent_node_id)
VALUES (
    @company_id,
    @mapping_id,
    @account_id,
    @element_id,
    @parent_node_id)

SELECT @node_id = @@IDENTITY

GO


