SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_template_chaser'
GO

CREATE PROCEDURE spu_get_template_chaser
    @document_template_id int
AS
    SELECT chaser_description from document_template
        WHERE document_template_id = @document_template_id

GO

