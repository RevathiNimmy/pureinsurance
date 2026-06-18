
--Start( Saurabh Agrawal )Tech Spec - PGR005 Automated Email(5.1.2.7)

SET QUOTED_IDENTIFIER OFF 
GO



EXECUTE DDLDropProcedure 'spu_get_codeandDescdocument_template_saa'
GO

CREATE PROCEDURE spu_get_codeandDescdocument_template_saa    
    @docid int,    
    @document_template_code varchar(10) OUTPUT,    
    @document_template_desc varchar(100) OUTPUT   
       
AS    
     
SELECT @document_template_code = code,    
       @document_template_desc = description     
FROM  document_template    
WHERE   document_template_id=@docid     
AND is_deleted=0    


GO
SET QUOTED_IDENTIFIER OFF 
GO



--End( Saurabh Agrawal )Tech Spec - PGR005 Automated Email(5.1.2.7)