SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Credit_Control_Step_To_Use'
GO

CREATE PROCEDURE spu_ACT_Get_Credit_Control_Step_To_Use  
@processMode tinyint = 0,  
@business_type varchar(20),  
@pffrequency_id int,  
@pfinstalments_result_id int,  
@plan_source_id int,  
@policy_is_paid tinyint = NULL,  
@insurance_file_status_id int = NULL,  
@instalment_failure_count int = NULL,  
@credit_control_step_id int OUTPUT,  
@potential_credit_control_rule_id int OUTPUT,
@insurance_file_cnt INT  
  
AS  
  
DECLARE @InstalmentImportCancellation int  
SET @InstalmentImportCancellation = 1  
DECLARE @InstalmentImportRejection int  
SET @InstalmentImportRejection = 2  
  
IF @processMode = @InstalmentImportCancellation  
BEGIN  
 -- Find out what the first credit control step is  
 SELECT TOP 1 @credit_control_step_id = ISNULL(credit_control_step_id,0),  
  @potential_credit_control_rule_id = credit_control_rule.credit_control_rule_id  
 FROM credit_control_step  
  
 INNER JOIN credit_control_rule ON  
 credit_control_rule.credit_control_rule_id = credit_control_step.credit_control_rule_id  
  
  LEFT JOIN credit_control_rule_insurance_file_status CCRIFS ON  
   CCRIFS.credit_control_rule_id = credit_control_rule.credit_control_rule_id  
  
  LEFT JOIN (SELECT Count(*) Count, Credit_Control_Rule_id
	     from credit_control_rule_insurance_file_status 
	     GROUP BY credit_control_rule_id) CCRIFS_Exists ON  
   CCRIFS_Exists.credit_control_rule_id = credit_control_rule.credit_control_rule_id  

 WHERE business_type = @business_type  
 AND ((@policy_is_paid IS NULL) OR (credit_control_rule.policy_is_paid = @policy_is_paid))  
 AND pffrequency_id =@pffrequency_id  
 AND source_id = @plan_source_id  
 AND instalment_failure_count IS NULL  
 AND pfinstalments_result_id = @pfinstalments_result_id 
 AND ((@insurance_file_status_id IS NULL AND CCRIFS_Exists.Count IS NULL) OR (CCRIFS.insurance_file_status_id = @insurance_file_status_id))  
 AND credit_control_rule.is_active = 1
 ORDER BY step_number ASC  
  
END  
  
IF @processMode = @InstalmentImportRejection  
BEGIN  
 -- Find out what the first credit control step is  
 SELECT TOP 1 @credit_control_step_id = ISNULL(credit_control_step_id,0),  
  @potential_credit_control_rule_id = credit_control_rule.credit_control_rule_id  
 FROM credit_control_step  
  
 INNER JOIN credit_control_rule ON  
 credit_control_rule.credit_control_rule_id = credit_control_step.credit_control_rule_id  
  
  LEFT JOIN credit_control_rule_insurance_file_status CCRIFS ON  
   CCRIFS.credit_control_rule_id = credit_control_rule.credit_control_rule_id  
  
 WHERE business_type = @business_type  
 AND pffrequency_id =@pffrequency_id  
 AND (pfinstalments_result_id =  @pfinstalments_result_id OR ISNULL(pfinstalments_result_id, 0) = 0) 
 AND instalment_failure_count = @instalment_failure_count  
 AND source_id = @plan_source_id  
 AND Credit_Control_Rule.is_active = 1
 AND (Credit_Control_Rule.Product_id IS NULL OR Credit_Control_Rule.Product_id IN ( SELECT Product_ID FROM Insurance_file WHERE Insurance_file_cnt = @insurance_file_cnt))
 ORDER BY step_number ASC  
END  



GO
