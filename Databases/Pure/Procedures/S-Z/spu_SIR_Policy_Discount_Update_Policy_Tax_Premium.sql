SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Update_Policy_Tax_Premium'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Update_Policy_Tax_Premium  
  
@insurance_file_cnt int  
  
AS  
  
BEGIN  
  
 -- only update the premiums if any of the taxes have been manually changed  
 -- otherwise all the taxes are automatically recalculated anyway  
 IF EXISTS (SELECT * FROM tax_calculation   
     WHERE insurance_file_cnt = @insurance_file_cnt    
     AND is_manually_changed = 1  
     AND risk_cnt IS NULL)    
  
 BEGIN  
   
  -- update the tax calculations premium to match the discounted / loaded premiums  
  UPDATE Tax_Calculation SET premium = prem.this_premium  
  FROM tax_calculation tc  
    
  -- get all premium values by tax band and class of business    
  -- Get all premium values by tax band and class of business    
  JOIN   (SELECT  tgtb.tax_band_id,    
    tgtb.tax_group_id,    
    tgtb.sequence,    
    p.class_of_business_id,    
    SUM(p.this_premium) [this_premium]    
   FROM    insurance_file_risk_link ifrl    
   JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt    
   JOIN    peril p ON p.risk_cnt = r.risk_cnt    
   JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group    
   WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt    
   AND     ifrl.status_flag <> 'U'    
   AND     r.is_risk_selected = 1    
   AND    (p.is_premium = 1 or p.is_levy_tax = 1)    
   GROUP BY    
    tgtb.tax_band_id,    
    tgtb.tax_group_id,    
   tgtb.sequence,    
   p.class_of_business_id) prem    
    
   ON  prem.tax_band_id = tc.tax_band_id    
   AND prem.tax_group_id = tc.tax_group_id    
   AND prem.sequence = tc.sequence    
   AND prem.class_of_business_id = tc.class_of_business_id        
    
  WHERE insurance_file_cnt = @insurance_file_cnt  
  AND risk_cnt IS NULL  
  AND transtype = 'TTIF'  
   
 END  
  
END  
        
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
