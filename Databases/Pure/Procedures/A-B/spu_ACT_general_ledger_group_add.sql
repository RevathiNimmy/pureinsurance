SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_general_ledger_group_add'
GO

CREATE PROCEDURE spu_ACT_general_ledger_group_add
    @parent_structure_tree_id INT,
    @description varchar(60),
    @child_structure_tree_id INT
AS

   INSERT INTO general_ledger_group
   (parent_structure_tree_id, description, child_structure_tree_id)
   VALUES
   (@parent_structure_tree_id, @description, @child_structure_tree_id)
GO

