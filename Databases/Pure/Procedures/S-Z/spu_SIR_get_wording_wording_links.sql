SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_get_wording_wording_links'
GO

CREATE PROCEDURE spu_SIR_get_wording_wording_links  
  
@document_template_id int  
  
AS   
  
BEGIN  
 Select Distinct Code from document_template where document_templatE_id in (  
 Select document_template_id from wording_wording_link  
 where calls_template_id = @document_template_id)  
END   
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
