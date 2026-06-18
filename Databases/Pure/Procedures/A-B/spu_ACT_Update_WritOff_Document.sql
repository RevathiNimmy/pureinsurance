SET QUOTED_IDENTIfIER OFF SET ANSI_NullS On
GO


Execute DDLDropProcedure 'spu_ACT_Update_WritOff_Document'
GO

CREATE PROCEDURE spu_ACT_Update_WritOff_Document
	 (@iOldDocumentId as INT, 
	  @iNewDocumentID as INT 
         ) 
as 


Update transdetail 
	
	SET document_id = @iNewDocumentID 
	
	Where 
	document_id = @iOldDocumentID and
	spare = 'WRITEOFF'   
GO  

