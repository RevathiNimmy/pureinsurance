SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_CCM_Doc_Template'
GO

  
CREATE PROCEDURE spu_get_CCM_Doc_Template
	@CCMDocumentTemplate As VARCHAR(255)
AS        
        
BEGIN  
	SELECT DISTINCT document_template_id
	FROM Document_Template
	WHERE CCMDocumentTemplate = @CCMDocumentTemplate
END  
GO