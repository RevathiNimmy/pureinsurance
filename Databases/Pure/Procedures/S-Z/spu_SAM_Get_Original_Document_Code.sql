SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Original_Document_Code'
GO
/*********************************************************************************************************/    
/* spu_SAM_Get_Original_Document_Code - Gets the Original Document Code*/    
/*                                                                                                       */    
/* RDT 7/6/2007                                                                                          */    
/*********************************************************************************************************/    
CREATE PROCEDURE spu_SAM_Get_Original_Document_Code    
    @Code varchar(20)
     
AS    
BEGIN    
    
	WITH cteGetOriginalDocumentCode
As
( 
    SELECT
        document_template_id, code, original_document_template_id
    FROM
        Document_Template WHERE code = @Code
    UNION All
    SELECT
        ic.document_template_id, ic.code, ic.original_document_template_id
    FROM Document_Template ic
    INNER JOIN cteGetOriginalDocumentCode CTEOD ON ic.document_template_id = CTEOD.original_document_template_id  
)
SELECT  top 1 code as Code  FROM cteGetOriginalDocumentCode  
where document_template_id>0    order by document_template_id desc

END 
