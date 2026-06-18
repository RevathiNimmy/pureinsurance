  
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Product_Values_From_Insurance_File_cnt'
GO
--Start (Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)    
CREATE PROCEDURE spu_Get_Product_Values_From_Insurance_File_cnt    
 @insurance_file_cnt INT    
AS    
    
--This SP can be extended to include more columns  
 BEGIN    
    
 SELECT  p.product_id,    
   p.caption_id,    
   p.code,    
   p.description,    
   p.effective_date,    
   p.is_deleted,  
   P.ri_manual_premium_adjustment  
    
 FROM Product P    
 LEFT JOIN Insurance_File I on  I.Product_iD=P.Product_id    
 WHERE I.insurance_file_cnt = @insurance_file_cnt    
    
 END    
--End (Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
  
 
