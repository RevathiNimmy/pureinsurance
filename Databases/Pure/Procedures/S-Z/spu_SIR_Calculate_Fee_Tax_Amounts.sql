SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Calculate_Fee_Tax_Amounts'
GO


CREATE PROCEDURE spu_SIR_Calculate_Fee_Tax_Amounts    
 @policy_fee_u_id int    
    
AS    
    
BEGIN    
    
 DECLARE @tax_group_id int    
 DECLARE @total_tax_amount money    
 DECLARE @tax_base_amount money    
 DECLARE @fee_currency_id int    
 DECLARE @company_id int    
 DECLARE @fee_amount money    
 DECLARE @insurance_file_cnt int
 DECLARE @risk_cnt int
   
-- Get the required policy fee details    
SELECT    
    @tax_group_id = tax_group_id,    
    @fee_currency_id = currency_id,    
    @company_id = branch_id,    
    @fee_amount = currency_amount,
    @insurance_file_cnt = insurance_file_cnt, 
    @risk_cnt = risk_cnt    
FROM policy_fee_u    
WHERE policy_fee_u_id = @policy_fee_u_Id    
  
 -- delete any existing tax calculations for this item  
 DELETE FROM tax_calculation   
 WHERE policy_fee_u_id = @policy_fee_u_id  
   
-- Calculate the Tax and regenerate the tax_calculations    
EXEC spu_SIR_Calculate_Tax_Amounts    
 @company_id=@company_id,    
 @tax_group_id=@tax_group_id,    
 @transtype='TTF',    
 @currency_id=@fee_currency_id,    
 @amount=@fee_amount,    
 @tax_currency_amount=@total_tax_amount OUTPUT,    
 @tax_base_amount=@tax_base_amount OUTPUT,  
 @associated_key_id =@policy_fee_u_id, 
 @insurance_file_cnt = @insurance_file_cnt, 
 @risk_cnt = @risk_cnt
        
  -- Write it back (this will include zeros)  
Update Policy_Fee_U Set  
  currency_tax_amount = round(@total_tax_amount,4),  
  base_tax_amount = round(@tax_base_amount,4)  
  WHERE policy_fee_u_id= @policy_fee_u_id  
    
END    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
