SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Mapping'
GO


CREATE PROCEDURE spu_ACT_Delete_Mapping
    @mapping_id int
AS


DELETE FROM Mapping
WHERE mapping_id = @mapping_id
GO


