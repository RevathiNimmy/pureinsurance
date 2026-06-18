SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Refresh_CCM_Templates'
GO

  
CREATE PROCEDURE spu_Refresh_CCM_Templates
	@RefreshAll AS BIT,
	@DocumentTemplateID AS INT
AS        
        
BEGIN  
	IF @RefreshAll = 1	
			UPDATE Document_Template
			SET DocumentFieldsetFieldList = NULL,
			CCMRefreshDate = NULL	
	ELSE
			UPDATE Document_Template
			SET DocumentFieldsetFieldList = NULL,
			CCMRefreshDate = NULL
			WHERE document_template_id = @DocumentTemplateID
END  
GO