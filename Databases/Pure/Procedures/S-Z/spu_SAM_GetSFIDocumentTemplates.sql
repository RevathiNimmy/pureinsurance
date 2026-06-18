

EXECUTE DDLDropProcedure 'spu_SAM_GetSFIDocumentTemplates'
GO



CREATE PROCEDURE spu_SAM_GetSFIDocumentTemplates
 @functional_area TINYINT,  
 @product_Id INT,  
 @Process_Type_Id INT = 0,  
 @source_id INT= 0  
AS  
BEGIN  
 
 SELECT  
 pdl.Document_Template_Id,  
 dt.Description,
 dt.Code,
 dt.document_template_group_id, 
 dt.document_template_sub_group_id,
 dt.is_portal_internal_only InternalOnly
 FROM PMB_Doc_Link pdl  
 LEFT JOIN document_template dt ON dt.document_template_id = pdl.Document_Template_Id  
 WHERE pdl.product_id = @product_Id
 AND pdl.functional_area = @functional_area 
 AND pdl.Process_Type_Id = @Process_Type_Id	
 AND (pdl.source_id = @source_id or pdl.source_id is null or @source_id =0)
 AND pdl.generate_through_SAM=1
END  

GO