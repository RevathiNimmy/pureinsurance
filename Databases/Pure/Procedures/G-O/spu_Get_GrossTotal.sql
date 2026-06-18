SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_GrossTotal'
GO
   
CREATE PROCEDURE spu_Get_GrossTotal    
    @insurance_file_cnt int    
AS    
    
DECLARE    
     @tax_value money,    
     @tax_value1 money,    
     @tax_value2 money,    
     @fee_value money,    
     @policyoptions bit    
    
 BEGIN    
    
   -- Get tax    
 SELECT  @tax_value1 = SUM(value)    
 FROM    Tax_Calculation    
 WHERE   insurance_file_cnt = @insurance_file_cnt    
 AND  risk_cnt IS NULL    
 AND  transtype in ('TTR','TTF','TTIF')    
    
  SELECT  @tax_value2 = SUM(value)    
  FROM    Tax_Calculation rt    
  JOIN    insurance_file_risk_link ifrl      ON ifrl.risk_cnt = rt.risk_cnt    
  JOIN    risk r                             ON r.risk_cnt = rt.risk_cnt    
  WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt    
  AND     ifrl.status_flag <> 'U'    
  AND     r.is_risk_selected = 1    
  AND  rt.risk_cnt IS NOT NULL    
  AND  transtype in ('TTR','TTF','TTIF')    
    
  SELECT  @tax_value = ISNULL(@tax_value1, 0) + ISNULL(@tax_value2, 0)    
    
  -- Get fee    
  SELECT  @fee_value = SUM(currency_amount)    
  FROM    policy_fee_u    
  WHERE   insurance_file_cnt = @insurance_file_cnt    
 End    
 -- Return data    
    
 SELECT    
     insurance_file_cnt,    
     this_premium,    
     insurance_ref,    
     ISNULL(@tax_value,0) + ISNULL(@fee_value,0) fees_taxes ,  
     ISNULL(this_premium,0)+ISNULL(@tax_value,0) + ISNULL(@fee_value,0) GrossTotal  
    
 FROM    
     Insurance_File    
 WHERE    
     insurance_file_cnt = @insurance_file_cnt    
  
