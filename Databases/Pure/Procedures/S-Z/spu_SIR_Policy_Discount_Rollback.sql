SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Rollback'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Rollback      
  
 @insurance_file_cnt int        
        
AS        
        
 BEGIN        
         
  UPDATE rs SET rs.this_premium = rs.discount_original_this_premium         
    
 FROM rating_section rs        
        
  INNER JOIN risk r On         
       rs.risk_cnt = r.risk_cnt        
        
   INNER JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES pdvrs ON    
    r.risk_status_id = pdvrs.risk_status_id    
    
     INNER JOIN insurance_file_risk_link ifrl ON         
   ifrl.risk_cnt = r.risk_cnt        
         
  INNER JOIN insurance_file ifile ON         
   ifile.insurance_file_cnt = ifrl.insurance_file_cnt        
    
 WHERE r.is_risk_selected = 1  
 -- dont update original rating sections   
 AND rs.original_flag = 0     
 AND ifile.insurance_file_cnt = @insurance_file_cnt
      
 END        


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
