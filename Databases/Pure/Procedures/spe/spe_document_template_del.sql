SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_document_template_del'
GO

CREATE PROCEDURE spe_document_template_del
    @document_template_id int
AS
DELETE FROM document_template
WHERE document_template_id = @document_template_id

GO

