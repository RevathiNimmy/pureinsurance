SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_StructMapID'
GO


CREATE PROCEDURE spu_ACT_Update_StructMapID
    @node_id int,
    @mapping_id int = NULL
AS


BEGIN
UPDATE StructureTree
    SET mapping_id = @mapping_id WHERE node_id = @node_id
END
GO


