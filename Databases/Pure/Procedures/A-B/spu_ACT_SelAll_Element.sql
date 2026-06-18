SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Element'
GO


CREATE PROCEDURE spu_ACT_SelAll_Element
AS


SELECT Element.parent_id,
    Parent.element_name,
    Element.element_id,
    Element.element_name
     FROM Element INNER JOIN Element As Parent ON Element.parent_id = Parent.element_id
         WHERE Element.parent_id > 0
         ORDER BY Element.parent_id, Element.element_id
GO


