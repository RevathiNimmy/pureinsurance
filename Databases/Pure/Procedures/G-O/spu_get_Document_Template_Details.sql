SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Document_Template_Details'
GO
  
CREATE PROCEDURE spu_get_Document_Template_Details
AS        
        
BEGIN  
	SELECT CCMDocumentTemplate,
	document_template_id
	FROM Document_Template
	WHERE CCMDocumentTemplate IS NOT NULL
END  
GO