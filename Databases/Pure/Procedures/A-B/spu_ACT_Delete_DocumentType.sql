SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_DocumentType'
GO


CREATE PROCEDURE spu_ACT_Delete_DocumentType
    @documenttype_id smallint
AS


DELETE FROM DocumentType
WHERE documenttype_id = @documenttype_id
GO


