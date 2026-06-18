SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_PolicyStandardWording_Select'  
GO




CREATE  PROCEDURE spu_PolicyStandardWording_Select  
    @insurance_file_cnt int  
AS  
SELECT  
  
    psw.document_template_id,
    dt.description  
   
 FROM  policy_standard_wording psw JOIN document_template dt  
 ON psw.document_template_id = dt.document_template_id  
WHERE insurance_file_cnt = @insurance_file_cnt  

GO


