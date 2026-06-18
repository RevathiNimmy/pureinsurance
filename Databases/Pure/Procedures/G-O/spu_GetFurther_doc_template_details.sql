
--Start( Saurabh Agrawal )Tech Spec - PGR005 Automated Email(5.1.2.7)

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


EXECUTE DDLDropProcedure 'spu_GetFurther_doc_template_details'
GO

CREATE PROCEDURE spu_GetFurther_doc_template_details 
@docTemplateId INT 
As
 
SELECT archive_with_no_print,
       email_as_body,
       spool_document,
       archive_as_text,
	   [description],
	   archive_as_xml,
	   CCMDocumentTemplate 	
FROM document_Template where document_template_id = @docTemplateId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


--End( Saurabh Agrawal )Tech Spec - PGR005 Automated Email(5.1.2.7)