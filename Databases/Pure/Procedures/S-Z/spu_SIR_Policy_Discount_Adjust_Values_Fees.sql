SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Adjust_Values_Fees'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Adjust_Values_Fees    
    
 @insurance_file_cnt int,    
 @discount_percentage float    
AS    
    
 BEGIN    
  UPDATE policy_fee_u    
  SET base_fee_amount = base_fee_amount + (base_fee_amount * (@discount_percentage/100)),    
   currency_amount = currency_amount + (currency_amount * (@discount_percentage/100))    
  FROM policy_fee_u    
  WHERE insurance_file_cnt = @insurance_file_cnt    
   AND risk_cnt IS NOT NULL  
  AND fee_rate_amount <> 0    
 END    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
