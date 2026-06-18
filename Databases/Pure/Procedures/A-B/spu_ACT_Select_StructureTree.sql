SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_StructureTree'
GO

CREATE PROCEDURE spu_ACT_Select_StructureTree
    @node_id int = NULL,
    @element_id int = NULL,
    @parent_node_id int = NULL,
    @account_id int = NULL,
    @mapping_id int = NULL
AS
    SELECT  s.node_id,
            s.mapping_id,
            s.account_id,
            s.element_id,
            s.parent_node_id,
            e.element_name,
            s.company_id
    FROM    StructureTree s
    JOIN    Element e ON s.element_id = e.element_id
    WHERE   s.node_id = @node_id

-- SP180100 - The only parameter ever supplied is node_id so tune the SQL
-- EK030200 - Removed because of reduced functionality
-- RFC140302 - Removed all unused parameters as a result of Benchmarking.
/*
    WHERE (s.node_id = @node_id OR @node_id = NULL)
    AND (s.element_id = @element_id OR @element_id = NULL)
    AND (s.parent_node_id = @parent_node_id OR @parent_node_id = NULL)
    AND (s.account_id = @account_id OR @account_id = NULL)
    AND (s.mapping_id = @mapping_id OR @mapping_id = NULL)
*/
GO

