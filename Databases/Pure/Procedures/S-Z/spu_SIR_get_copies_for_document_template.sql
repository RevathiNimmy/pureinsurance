SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_get_copies_for_document_template'
GO

 
CREATE PROCEDURE spu_SIR_get_copies_for_document_template
@document_template_id int  
  
AS  
  
BEGIN  
	SELECT Code,count(*) as NoOfCopiedDocs FROM document_template 
	WHERE original_document_template_id = @document_template_id
	GROUP BY Code    
END  

--EXEC spu_SIR_get_copies_for_document_template 100383