SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Recalculate_Risk_Fees'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Recalculate_Risk_Fees    
    
 @insurance_file_cnt int,    
 @transaction_type_id int    
    
AS    
    
BEGIN    
    
 DECLARE @risk_cnt int   
 DECLARE @use_existing_fee_details int   
    
 -- policy discount doesnt want the fee details to be reloaded   
 -- so default to using existing fee details   
 SET @use_existing_fee_details = 1  
  
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
    
     EXEC spu_SIR_Recalculate_Risk_Fees    
     @transaction_type_id,    
     @risk_cnt,    
     @insurance_file_cnt,    
     @use_existing_fee_details  
    
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
