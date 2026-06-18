
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Document_FieldsetFieldList'
GO
 
  
CREATE PROCEDURE spu_get_Document_FieldsetFieldList
	@CCMDocumentName VARCHAR(255)
AS        
        
BEGIN  
	SELECT DISTINCT DocumentFieldsetFieldList 
	FROM Document_Template
	WHERE CCMDocumentTemplate= @CCMDocumentName
END  
GO