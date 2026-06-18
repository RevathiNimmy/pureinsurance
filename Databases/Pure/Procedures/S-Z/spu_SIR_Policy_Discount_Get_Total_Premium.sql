SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Get_Total_Premium'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Get_Total_Premium    
 @insurance_file_cnt int    
AS    
    
BEGIN    
    
 DECLARE @insurance_file_tax money    
 DECLARE @risk_tax money    
 DECLARE @fee_amount money    
 DECLARE @this_premium money    
 DECLARE @policy_tax money    
 DECLARE @policy_fee_tax money    
 DECLARE @risk_fee_tax money    
 DECLARE @insurance_folder_cnt int    
 DECLARE @product_is_true_monthly_policy int    
 DECLARE @amount_to_be_put_on_next_instalment money    
 DECLARE @policy_fee_amount money    
 DECLARE @risk_fee_amount money    
    
 -- annual premium    
 SELECT @this_premium = ROUND(this_premium,2) FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt  
    
 -- policy level tax    
 -- SELECT @insurance_file_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTIF'    
 -- ignore policy level tax for policy discounts  
 SELECT @insurance_file_tax = 0  
  
 -- risk_tax    
 SELECT @risk_tax = SUM(ROUND(value,2)) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt)  
 and is_risk_selected =1) and transtype ='TTR'    
    
 -- policy fees tax    
 --SELECT @policy_fee_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  and risk_cnt IS NULL    
 -- ignore policy level fees tax for policy discounts  
  SELECT @policy_fee_tax = 0  
  
 -- risk fees tax    
 SELECT @risk_fee_tax = sum(ROUND(value,2)) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  
 AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt) and is_risk_selected =1)    
    
 -- policy_fees_u  - policy fees    
 --SELECT @policy_fee_amount = SUM(currency_amount) FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt   and risk_cnt IS NULL    
 -- ignore policy fees for policy discounts  
  SELECT @policy_fee_amount = 0  
  
 -- policy_fees_u  - risk fees    
 SELECT @risk_fee_amount = SUM(ROUND(currency_amount,2)) FROM policy_fee_u  
 INNER JOIN risk ON    
  policy_fee_u.risk_cnt = risk.risk_cnt    
 WHERE insurance_file_cnt = @insurance_file_cnt    
 AND policy_fee_u.risk_cnt IS NOT NULL    
 AND risk.is_risk_selected = 1    
    
 -- return the premium and the details of the items included in the total amount    
 SELECT  ISNULL(@this_premium,0) + ISNULL(@insurance_file_tax,0) + ISNULL(@risk_tax,0), --+ ISNULL(@policy_fee_amount,0) + ISNULL(@risk_fee_amount,0) + ISNULL(@policy_fee_tax, 0) +  ISNULL(@risk_fee_tax, 0),    
  ISNULL(@this_premium,0) as this_premium,    
  ISNULL(@insurance_file_tax,0) as insurance_file_tax,    
  ISNULL(@risk_tax,0) as risk_tax,    
  ISNULL(@policy_fee_amount,0)  as policy_fee_amount,    
  ISNULL(@risk_fee_amount,0)  as risk_fee_amount,    
  ISNULL(@policy_fee_tax, 0) as fee_tax    
    
END    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
