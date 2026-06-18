--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.5)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Risk_Type_Linked_Clauses_add'
GO

CREATE PROCEDURE spu_Risk_Type_Linked_Clauses_add      
@document_Template_Id int ,     
@Risk_Type_Id Int,        
@branch_Id INT,      
@default  tinyint,
@UserId INT = NULL,
@UniqueId VARCHAR(50) = NULL,
@ScreenHierarchy VARCHAR(500) = NULL    
      
AS      
Insert into             
Wording_Risk_Type_Link       
(document_template_id,      
 Risk_Type_id,      
branch_id,      
[default],      
UserId,
UniqueId,
ScreenHierarchy)      
values      
      
(      
@document_Template_Id,      
@Risk_Type_Id,      
@branch_Id,      
@default,
@UserId,
@UniqueId,
@ScreenHierarchy
)      
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.5)        
      