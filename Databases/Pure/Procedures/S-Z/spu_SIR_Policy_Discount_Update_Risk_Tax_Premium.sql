SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Update_Risk_Tax_Premium'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Update_Risk_Tax_Premium  
      
@risk_cnt int      
      
AS      
      
BEGIN      
      
 -- only update the premiums if any of the taxes have been manually changed      
 -- otherwise all the taxes are automatically recalculated anyway      
 IF EXISTS (SELECT * FROM tax_calculation       
     WHERE risk_cnt = @risk_cnt        
     AND is_manually_changed = 1)        
      
 BEGIN      
       
  -- update the tax calculations premium to match the discounted / loaded premiums      
  UPDATE Tax_Calculation SET premium = prem.this_premium      
  FROM tax_calculation tc      
        
  -- get all premium values by tax band and class of business        
  JOIN   (SELECT        
    tgtb.tax_band_id,        
                  tgtb.tax_group_id,        
                  tgtb.sequence,        
                  p.class_of_business_id,        
                  sum(p.this_premium) [this_premium]        
          FROM    peril p        
          JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group        
          WHERE   p.risk_cnt = @risk_cnt        
          AND     (p.is_premium = 1 or p.is_levy_tax = 1)        
          GROUP BY        
                  tgtb.tax_band_id,        
                  tgtb.tax_group_id,        
                  tgtb.sequence,        
                  p.class_of_business_id) prem      
          
  ON  prem.tax_band_id = tc.tax_band_id        
  AND prem.tax_group_id = tc.tax_group_id        
  AND prem.sequence = tc.sequence        
  AND prem.class_of_business_id = tc.class_of_business_id        
      
  WHERE risk_cnt = @risk_cnt      
      
  AND transtype = 'TTR'      
       
 END      
      
END      
            
    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
