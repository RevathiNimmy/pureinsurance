SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Element'
GO


CREATE PROCEDURE spu_ACT_Delete_Element
    @element_id int
AS


DELETE FROM Element
WHERE element_id = @element_id
GO


