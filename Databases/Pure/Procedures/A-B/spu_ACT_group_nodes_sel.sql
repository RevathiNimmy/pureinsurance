SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_group_nodes_sel'
GO

CREATE PROCEDURE spu_ACT_group_nodes_sel
    @parent_node_id INT
AS
select s.node_id, 
       e.element_name, 
       x.group_for_gl_export_ind
    from structuretree s, 
         element e, 
         elementextras x
    Where s.element_id = e.element_id
    AND e.element_id = x.element_id
    AND s.account_id is null
    AND s.parent_node_id = @parent_node_id

GO

