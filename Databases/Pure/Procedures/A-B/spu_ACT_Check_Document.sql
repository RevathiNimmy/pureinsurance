SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Document'
GO


CREATE PROCEDURE spu_ACT_Check_Document
    @document_id int OUTPUT
AS


BEGIN
    SELECT @document_id = document_id
    FROM Document
    WHERE document_id = @document_id
END
BEGIN
IF @document_id = NULL
    SELECT @document_id = -1
END
GO


