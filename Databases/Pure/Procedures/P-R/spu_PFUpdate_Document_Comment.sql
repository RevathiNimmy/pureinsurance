SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_PFUpdate_Document_Comment'
GO

CREATE PROCEDURE spu_PFUpdate_Document_Comment
                @transdetail_id int,
                @comment varchar(255)

AS

BEGIN
    UPDATE Document
    SET         comment = @comment
    WHERE  document_id = 
    (
        SELECT document_id
        FROM Transdetail
        WHERE transdetail_id = @transdetail_id
    )
END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO

