EXECUTE DDLDropProcedure 'spu_PFScheme_GetDocTemplate'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_PFScheme_GetDocTemplate

AS BEGIN

    SELECT document_template_id, description 
    FROM document_template 
    ORDER BY description

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
