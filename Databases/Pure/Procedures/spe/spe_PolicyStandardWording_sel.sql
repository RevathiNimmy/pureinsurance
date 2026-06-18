SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spe_PolicyStandardWording_sel'  
GO




CREATE  PROCEDURE spe_PolicyStandardWording_sel  
    @insurance_file_cnt int  
AS  
SELECT  
  
    psw.document_template_id,
    dt.description  
   
 FROM  policy_standard_wording psw JOIN document_template dt  
 ON psw.document_template_id = dt.document_template_id  
WHERE insurance_file_cnt = @insurance_file_cnt  

GO


