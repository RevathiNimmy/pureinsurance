SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_document_template_description_upd
GO
CREATE PROCEDURE spu_document_template_description_upd
@document_template_id INT,
@description VARCHAR(255)
AS
BEGIN
    UPDATE document_template SET description=@description WHERE document_template_id=@document_template_id
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
