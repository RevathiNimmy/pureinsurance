SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_claim_spooled_desc'
GO

CREATE PROCEDURE spu_get_claim_spooled_desc 
    @Claim_id INT  
AS  
BEGIN  
  
    SELECT ds.description FROM document_template dt
     JOIN document_spooler ds ON dt.document_template_id=ds.document_template_id 
     WHERE claim_cnt= @Claim_id  
  
END 
GO

