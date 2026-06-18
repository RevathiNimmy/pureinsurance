SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_IsDuplicate_Element'
GO


CREATE PROCEDURE spu_ACT_Do_IsDuplicate_Element
    @parent_node_id int,
    @element_name varchar(20)
AS


SELECT Element.element_name
    FROM StructureTree INNER JOIN Element
         ON StructureTree.element_id = Element.element_id
            WHERE StructureTree.parent_node_id = @parent_node_id
                  AND Element.element_name = @element_name
GO


