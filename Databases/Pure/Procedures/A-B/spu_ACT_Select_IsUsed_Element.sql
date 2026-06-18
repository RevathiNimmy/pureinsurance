SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_IsUsed_Element'
GO


CREATE PROCEDURE spu_ACT_Select_IsUsed_Element
    @element_id int
AS


SELECT
    element_id,
    element_name,
    parent_id
FROM Element
    WHERE (parent_id = @element_id OR element_id = @element_id)
GO


