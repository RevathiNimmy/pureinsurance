
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_update_Document_FieldsetFieldList'
GO
 
  
CREATE PROCEDURE spu_update_Document_FieldsetFieldList
	@CCMDocumentName VARCHAR(255),
	@DocumentFieldsetFieldList VARCHAR(MAX)
AS        
        
BEGIN  
	UPDATE Document_Template
	SET DocumentFieldsetFieldList = @DocumentFieldsetFieldList,
	CCMRefreshDate = GETDATE()	 
	WHERE CCMDocumentTemplate= @CCMDocumentName
END  
GO