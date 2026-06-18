
--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.7)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Product_Linked_Clauses_del'
GO
Create PROCEDURE spu_Product_Linked_Clauses_del  
 @Document_Template_Id int,  
 @product_id int,
 @UserId INT = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL    
AS  
  
UPDATE wording_product_link SET
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
	where   document_template_id=@Document_Template_Id AND product_id=@product_id  

delete from wording_product_link where  Document_Template_Id=@Document_Template_Id and
    
product_id=@product_id  
  
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.7)