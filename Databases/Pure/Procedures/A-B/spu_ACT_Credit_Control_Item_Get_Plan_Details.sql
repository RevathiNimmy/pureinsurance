SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Item_Get_Plan_Details'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Item_Get_Plan_Details        
 @pfinstalments_id INT,       
 @business_type varchar(20)      
      
AS       
        
BEGIN       
  
      
 DECLARE @can_auto_cancel TINYINT,        
  @credit_control_step_id INT,        
  @due_days SMALLINT        
      
 DECLARE @plan_source_id INT      
 DECLARE @pfprem_finance_cnt INT      
 DECLARE @pfprem_finance_version INT       
 DECLARE @pffrequency_id INT      
 DECLARE @pfinstalments_result_id INT      
 DECLARE @pfinstalments_due_date datetime  
      
 -- get required plan details      
 SELECT  @plan_source_id = pfpremiumfinance.source_id,      
  @pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt,       
  @pfprem_finance_version = pfpremiumfinance.pfprem_finance_version,       
  @pffrequency_id = pffrequency_id,       
  @pfinstalments_result_id = pfinstalments_result_id,   
  @pfinstalments_due_date = pfinstalments.duedate      
      
 FROM pfinstalments       
      
  INNER JOIN pfpremiumfinance ON      
   pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt      
  AND pfinstalments.pfprem_finance_version = pfpremiumfinance.pfprem_finance_version      
      
   INNER JOIN pfrf ON       
    pfpremiumfinance.pfrf_id = pfrf.pfrf_id      
      
 WHERE pfinstalments_id = @pfinstalments_id      
          
 -- find out if any of the perils have the auto cancel flag set        
 SELECT @can_auto_cancel = MAX(pt.is_auto_cancel)        
 FROM PFPremiumFinance pff        
  INNER JOIN Insurance_File_Risk_Link ifrl        
   ON pff.insurance_file_cnt = ifrl.insurance_file_cnt        
        
   INNER JOIN Peril p        
    ON ifrl.risk_cnt = p.risk_cnt        
        
    INNER JOIN Peril_Type pt        
     ON p.peril_type_id = pt.peril_type_id        
      
 WHERE pff.pfprem_finance_cnt = @pfprem_finance_cnt        
 AND pff.pfprem_finance_version = @pfprem_finance_version        
       
 -- Find out what the first credit control step is      
 SELECT @credit_control_step_id = ISNULL(MIN(credit_control_step_id),0)       
 FROM credit_control_step      
  INNER JOIN credit_control_rule ON       
   credit_control_rule.credit_control_rule_id = credit_control_step.credit_control_rule_id      
 WHERE business_type = @business_type      
 AND pffrequency_id =@pffrequency_id      
 AND pfinstalments_result_id =  @pfinstalments_result_id      
 AND source_id = @plan_source_id      
      
 IF @credit_control_step_id = 0       
  SELECT @credit_control_step_id = ISNULL(MIN(@credit_control_step_id),0)      
  FROM credit_control_step      
   INNER JOIN credit_control_rule ON       
    credit_control_rule.credit_control_rule_id = credit_control_step.credit_control_rule_id      
  WHERE business_type = @business_type      
  And pffrequency_id = @pffrequency_id      
  AND pfinstalments_result_id IS NULL      
  AND source_id = @plan_source_id       
         
 -- get the business type code from the associated insurance file      
 DECLARE @business_type_code varchar(20)      
 SELECT @business_type_code = bt.code       
 FROM PFPremiumFinance pff        
  INNER JOIN Insurance_File iff ON       
   pff.insurance_file_cnt = iff.insurance_file_cnt        
       
   INNER JOIN Business_Type bt ON       
    iff.business_type_id = bt.business_type_id        
 WHERE pff.pfprem_finance_cnt = @pfprem_finance_cnt        
 AND pff.pfprem_finance_version = @pfprem_finance_version      
      
        IF LTRIM(RTRIM((@business_type_code))) = 'DIRECT'        
      
  SELECT @due_days = ccs.number_of_days        
  FROM Credit_Control_Step ccs       
  WHERE ccs.credit_control_step_id = @credit_control_step_id      
      
        ELSE        
      
  SELECT @due_days = ccs.broker_days        
  FROM Credit_Control_Step ccs       
  WHERE ccs.credit_control_step_id = @credit_control_step_id      
      
 -- Get the other required details        
 SELECT acc.account_id,        
  pff.plantransaction_id,        
  pff.insurance_file_cnt,        
  @can_auto_cancel,        
  @credit_control_step_id,        
  @due_days,   
  @pfprem_finance_cnt,    
  @pfprem_finance_version,   
  @pfinstalments_due_date  
      
 FROM PFPremiumFinance pff        
      
  LEFT JOIN Insurance_File iff        
   ON pff.insurance_file_cnt = iff.insurance_file_cnt        
        
   LEFT JOIN Account acc        
    ON iff.insured_cnt = acc.account_key        
      
 WHERE pff.pfprem_finance_cnt = @pfprem_finance_cnt        
 AND pff.pfprem_finance_version = @pfprem_finance_version        
        
END      
    



GO
