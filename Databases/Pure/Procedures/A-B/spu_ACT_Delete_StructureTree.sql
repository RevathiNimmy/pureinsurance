SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_StructureTree'
GO


CREATE PROCEDURE spu_ACT_Delete_StructureTree
    @node_id int
AS


DELETE FROM StructureTree
WHERE node_id = @node_id
GO


