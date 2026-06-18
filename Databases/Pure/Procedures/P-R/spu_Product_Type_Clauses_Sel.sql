--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.2)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Product_Clauses_Sel'
GO
CREATE PROCEDURE  spu_Product_Clauses_Sel      
@Product_type_id INT       
     
AS            
            
select  dt.document_template_id,            
   dt.code,            
   dt.description,        
    1,    
    wpl.[default],    
   b.Source_id,    
   b.description         
              
from              
    document_template dt           
              
     left join document_type ty on ty.document_type_id=dt.document_type_id          
     left join wording_product_link wpl on wpl.document_template_id=dt.document_template_id          
     left join Source b on b.Source_id=wpl.branch_id          
    where              
    ty.code = 'CLAUSES'  and dt.is_deleted=0 and dt.effective_date<getdate()          
    and dt.document_template_id in (            
    select  document_template_id            
    from    wording_Product_link            
    where   Product_id = @Product_type_id)  and  wpl.Product_id= @Product_type_id       
    AND   ISNULL(copy_of_original,0)=0 
          
union          
          
            
select  dt.document_template_id,            
    dt.code,            
    dt.description,            
    0,0,    
     '',    
   ''             
from              
    document_template dt            
    left join document_type ty on ty.document_type_id=dt.document_type_id          
    left join wording_product_link wpl on wpl.document_template_id=dt.document_template_id          
    left join Source b on b.Source_id=wpl.branch_id        
    where             
              
    ty.code = 'CLAUSES'  and dt.is_deleted=0 and dt.effective_date<getdate()          
    and dt.document_template_id not in (            
    select  document_template_id            
     from    wording_Product_link            
    where   Product_id = @Product_type_id) 
	And    dt.document_template_id > 0  
	AND   ISNULL(copy_of_original,0)=0 
ORDER BY code             
 GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)-(6.3.2)
          
          
           
        
      
    
  
