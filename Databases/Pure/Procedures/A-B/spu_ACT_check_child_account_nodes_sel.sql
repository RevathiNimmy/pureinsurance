SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_check_child_account_nodes_sel'
GO

CREATE PROCEDURE spu_ACT_check_child_account_nodes_sel
    @parent_node_id INT
AS
SELECT account_id FROM structuretree 
WHERE parent_node_id = @parent_node_id
AND account_id IS NOT NULL

GO

