

--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.4)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Risk_Type_Linked_Clauses_del'
GO

CREATE PROCEDURE spu_Risk_Type_Linked_Clauses_del    
     
 @Document_Template_Id int,
 @Risk_Type_id int,
 @UserId INT = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL
     
    
AS    

UPDATE wording_Risk_Type_link SET
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
	where   document_template_id=@Document_Template_Id AND risk_type_id=@Risk_Type_id  

delete from wording_Risk_Type_link where   document_template_id=@Document_Template_Id
    
     
and risk_type_id=@Risk_Type_id   
    
  GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.4)      
  
