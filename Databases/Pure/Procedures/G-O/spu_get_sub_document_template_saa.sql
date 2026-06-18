SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_sub_document_template_saa'
GO

CREATE PROCEDURE spu_get_sub_document_template_saa
    @code char(10),
    @document_template_id int OUTPUT,
    @document_type_id int OUTPUT,
    @description char(255) OUTPUT
AS
BEGIN
    SELECT  @document_template_id = DT.document_template_id,
            @document_type_id = DT.document_type_id,
            @description = DT.description
    FROM document_template AS DT INNER JOIN document_type AS DTY
        ON  dt.document_type_id = DTY.document_type_id
    WHERE DTY.code = 'LETTER'
    AND   DT.code = @code
    AND   DT.is_deleted = 0
END
GO
