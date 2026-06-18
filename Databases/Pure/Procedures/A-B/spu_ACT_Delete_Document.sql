SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Document'
GO


CREATE PROCEDURE spu_ACT_Delete_Document
    @document_id int
AS


DELETE FROM Document
WHERE document_id = @document_id
GO


