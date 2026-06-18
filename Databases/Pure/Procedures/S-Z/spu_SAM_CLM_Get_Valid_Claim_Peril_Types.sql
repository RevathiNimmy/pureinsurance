SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Valid_Claim_Peril_Types'
GO


CREATE PROCEDURE spu_SAM_CLM_Get_Valid_Claim_Peril_Types  
  
@risk_cnt int   
  
AS  
  
SELECT pt.peril_Type_id,   
 pt.code as peril_type_code,   
  pt.description as peril_type_description,   
   r.reserve_type_id,   
    r.name as reserve_type_code,   
     r.description as reserve_type_description  
     
FROM peril p  
  
    JOIN    peril_type pt     ON   
 pt.peril_type_id = p.peril_type_id    
  
    JOIN    rating_section rs ON   
 rs.rating_section_id = p.rating_section_id   
 AND rs.risk_cnt = p.risk_cnt    
  
 LEFT   JOIN peril_type_reserve_type ptrt ON   
 pt.peril_type_id = ptrt.peril_type_id  
  
 LEFT JOIN reserve_type r ON   
  r.reserve_type_id = ptrt.reserve_type_id  

WHERE   p.risk_cnt = @risk_cnt   

AND pt.is_deleted = 0





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
