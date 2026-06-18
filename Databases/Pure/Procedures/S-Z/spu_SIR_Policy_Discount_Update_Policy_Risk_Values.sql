SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Update_Policy_Risk_Values'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Update_Policy_Risk_Values    
 @insurance_file_cnt int    
AS    
 BEGIN      
  DECLARE @risk_cnt int    
    
  DECLARE risk_cursor CURSOR FOR     
  SELECT ifrl.risk_cnt     
  FROM insurance_file_risk_link ifrl    
   INNER JOIN risk r ON    
    ifrl.risk_cnt = r.risk_cnt    
   INNER JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES rs ON   
	r.risk_status_id = rs.risk_status_id
  WHERE ifrl.insurance_file_cnt = @insurance_file_cnt    
  AND r.is_risk_selected = 1
     
  OPEN risk_cursor    
     
  FETCH NEXT FROM risk_cursor     
  INTO @risk_cnt    
     
  WHILE @@FETCH_STATUS = 0    
  BEGIN          
   EXEC spu_SIR_Policy_Discount_Update_Risk_Values @risk_cnt    
     
       -- get the next risk on the policy.    
   FETCH NEXT FROM risk_cursor     
   INTO @risk_cnt        
   END    
     
  CLOSE risk_cursor    
  DEALLOCATE risk_cursor    
 END    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
