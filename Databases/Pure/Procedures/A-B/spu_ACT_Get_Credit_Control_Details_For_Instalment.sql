SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Credit_Control_Details_For_Instalment'
GO
CREATE PROCEDURE spu_ACT_Get_Credit_Control_Details_For_Instalment  

 @pfinstalments_id INT,
 @processMode INT = 0
AS

BEGIN

 --**********
 -- CONSTANT
 DECLARE @InstalmentImportCancellation int
 DECLARE @InstalmentImportRejection int
 SET @InstalmentImportCancellation = 1
 SET @InstalmentImportRejection = 2
  --**********

 DECLARE
  @can_auto_cancel TINYINT ,
  @credit_control_step_id INT ,
  @due_days SMALLINT ,
  @plan_source_id INT ,
  @pfprem_finance_cnt INT ,
  @pfprem_finance_version INT ,
  @pffrequency_id INT ,
  @pfinstalments_result_id INT ,
  @pfinstalments_due_date datetime ,
  @pfinstalment_failure_count int ,
  @pfrf_recollect_on_next tinyint ,
  @pfrf_recollect_days smallint ,
  @pfrf_retry_limit smallint ,
  @credit_control_item_id int ,
  @account_id int ,
  @plantransaction_id int ,
  @insurance_file_cnt int ,
  @pffrequency_description varchar(20),
  @source_description varchar(20),
  @pfinstalments_amount money,
  @pfinstalments_result_description varchar(50),
  @existing_credit_control_rule_instalment_result_id int,
  @existing_credit_control_rule_id int,
  @existing_credit_control_step_id int,
  @potential_credit_control_rule_id int,
  @next_credit_control_step_id int,
  @business_type varchar(20),
  @statusInd varchar (3),
  @policy_is_paid tinyint,
  @insurance_file_status_id int,
  @insurance_file_status_description varchar(10),
  @base_balance money,
  --PN66512
  @credit_control_rule_id_for_lapsed int,
  @step_number_for_lapsed int,
  @original_due_date datetime,
  @previous_step int

 -- get required plan details
 SELECT
  @insurance_file_cnt = pfpremiumfinance.insurance_file_cnt,
  @insurance_file_status_id = insurance_file.insurance_file_status_id,
  @insurance_file_status_description = insurance_file_status.description,
  @statusind = pfpremiumfinance.statusind,
  @plan_source_id = pfpremiumfinance.source_id,
  @pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt,
  @pfprem_finance_version = pfpremiumfinance.pfprem_finance_version,
  @pffrequency_id = pfrf.pffrequency_id,
  @pfinstalments_result_id = pfinstalments.pfinstalments_result_id,
  @pfinstalments_due_date = pfinstalments.duedate,
  @pfinstalment_failure_count = pfinstalments.failure_count,
  @pfrf_recollect_on_next = pfrf.recollect_on_next,
  @pfrf_recollect_days = pfrf.recollect_days,
  @pfrf_retry_limit = pfrf.retry_limit,
  @credit_control_item_id = credit_control_item_id,
  @pffrequency_description =  pffrequency.description,
  @source_description = source.description,
  @pfinstalments_amount = pfinstalments.amount,
  @pfinstalments_result_description = pfinstalments_result.description,
  @existing_credit_control_rule_instalment_result_id = credit_control_rule.pfinstalments_result_id,
  @existing_credit_control_step_id = credit_control_step.credit_control_step_id,
  @existing_credit_control_rule_id = credit_control_rule.credit_control_rule_id,
  @original_due_date = pfinstalments.original_duedate

 FROM pfinstalments with (nolock)

  LEFT JOIN pfinstalments_result with (nolock) ON
   pfinstalments.pfinstalments_result_id = pfinstalments_result.pfinstalments_result_id

  LEFT JOIN credit_control_item with (nolock) ON
   pfinstalments.pfinstalments_id = credit_control_item.pfinstalments_id AND credit_control_item.is_deleted = 0

  LEFT JOIN credit_control_step with (nolock) ON
   credit_control_item.credit_control_step_id = credit_control_step.credit_control_step_id

  LEFT JOIN credit_control_rule with (nolock) ON
   credit_control_step.credit_control_rule_id = credit_control_rule.credit_control_rule_id

  INNER JOIN pfpremiumfinance with (nolock) ON
   pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt
   AND pfinstalments.pfprem_finance_version = pfpremiumfinance.pfprem_finance_version

   INNER JOIN insurance_file with (nolock) ON
    pfpremiumfinance.insurance_file_cnt = insurance_file.insurancE_filE_cnt

   LEFT JOIN insurance_file_status with (nolock) ON
    insurance_file.insurance_file_status_id = insurance_file_status.insurance_file_status_id

  INNER JOIN pfrf with (nolock) ON 
   pfpremiumfinance.pfrf_id = pfrf.pfrf_id

  INNER JOIN pffrequency with (nolock) ON
   pffrequency.pffrequency_id = pfrf.pffrequency_id

  INNER JOIN source with (nolock) ON
   pfpremiumfinance.source_id = source.source_id

 WHERE pfinstalments.pfinstalments_id = @pfinstalments_id

 IF @processMode = @InstalmentImportCancellation
  EXEC spu_ACT_Select_Is_Policy_Paid
   @insurance_file_cnt,
   @policy_is_paid OUTPUT,
   @base_balance OUTPUT

 IF @statusInd = '140' -- on hold
  SET @business_type = 'INSH'
 IF @statusInd = '999' -- cancelled
  SET @business_type = 'INSC'
 IF @statusInd = '040' OR @statusInd = '900'-- live
  SET @business_type = 'INS'

 -- find out if any of the perils have the auto cancel flag set
 SELECT @can_auto_cancel = MIN(ISNULL(pt.is_auto_cancel,0))  
 FROM PFPremiumFinance pff
  INNER JOIN Insurance_File_Risk_Link ifrl ON
   pff.insurance_file_cnt = ifrl.insurance_file_cnt

  INNER JOIN Peril p ON
   ifrl.risk_cnt = p.risk_cnt

  INNER JOIN Peril_Type pt ON
   p.peril_type_id = pt.peril_type_id

 WHERE pff.pfprem_finance_cnt = @pfprem_finance_cnt
 AND pff.pfprem_finance_version = @pfprem_finance_version

 -- THE FAILURE COUNT ISNT ACTUALLY UPDATED UNTIL AFTER
 -- CREDIT CONTROL IS GENERATED
 -- SO INCREMENT THE VALUE BY ONE AS THIS IS THE REAL CORRECT VALUE
 IF @processMode = @InstalmentImportRejection
  SET @pfinstalment_failure_count = ISNULL(@pfinstalment_failure_count,0) + 1

  -- get the credit control details for this process
  EXEC spu_ACT_Get_Credit_Control_Step_To_Use
      @processMode, @business_type,
      @pffrequency_id, @pfinstalments_result_id,
      @plan_source_id, @policy_is_paid, @insurance_file_status_id,
      @pfinstalment_failure_count,
      @credit_control_step_id OUTPUT,
      @potential_credit_control_rule_id OUTPUT,
      @insurance_file_cnt

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
 
 
  DECLARE @processing_days int
 SET @processing_days = 0
 -- Get Credit Control Rule Processing Days
 IF ISNULL(@credit_control_step_id,0) <> 0
 BEGIN
 SELECT @processing_days = processing_days
 FROM credit_control_rule
 WHERE credit_control_rule_id in (
  SELECT credit_control_rule_id
  FROM credit_control_step
  WHERE credit_Control_Step_id = @credit_control_step_id)
 END

 -- if this is direct business
 IF LTRIM(RTRIM((@business_type_code))) = 'DIRECT'
  BEGIN
	--PN66512
	SELECT @credit_control_rule_id_for_lapsed = credit_control_rule_id,
		@step_number_for_lapsed = step_number,
		@previous_step = previous_step
	FROM credit_control_step
	WHERE credit_Control_Step_id = @credit_control_step_id


		SELECT @due_days = ccs.number_of_days
		FROM Credit_Control_Step ccs
		--WHERE ccs.credit_control_step_id = @credit_control_step_id
		WHERE ccs.credit_control_rule_id = @credit_control_rule_id_for_lapsed and step_number = @step_number_for_lapsed-1								
	
		IF (@due_days > 0 )
		SELECT @due_days = @due_days + 1
	
	
  END
 ELSE
  SELECT @due_days = ccs.broker_days
  FROM Credit_Control_Step ccs
  WHERE ccs.credit_control_step_id = @credit_control_step_id

 -- Get the other required details
 SELECT
 @Account_Id = acc.account_id,
 @PlanTransaction_Id = pff.plantransaction_id,
 @Insurance_file_cnt = pff.insurance_file_cnt

 FROM PFPremiumFinance pff

 LEFT JOIN Insurance_File iff
 ON pff.insurance_file_cnt = iff.insurance_file_cnt

 LEFT JOIN Account acc
 ON iff.insured_cnt = acc.account_key

 WHERE pff.pfprem_finance_cnt = @pfprem_finance_cnt
 AND pff.pfprem_finance_version = @pfprem_finance_version



 SELECT
  @account_id account_id,
  @plantransaction_id plan_transaction_id,
  @insurance_file_cnt insurance_file_cnt,
  @can_auto_cancel can_auto_cancel,
  @credit_control_step_id credit_control_step_id,
  @due_days due_days,
  @plan_source_id plan_source_id,
  @pfprem_finance_cnt plan_id,
  @pfprem_finance_version plan_version,
  @pffrequency_id rate_frequency_id,
  @pfinstalments_result_id instalment_result_id,
  @pfinstalments_due_date instalment_due_date,
  @pfinstalment_failure_count instalment_failure_count,
  @pfrf_recollect_on_next rate_recollect_on_next_rate,
  @pfrf_recollect_days rate_recollect_days,
  @pfrf_retry_limit rate_retry_limit,
  @credit_control_item_id credit_control_item_id,
  @pffrequency_description frequency_description,
  @source_description  source_description,
  @pfinstalments_amount instalment_amount,
  @pfinstalments_result_description instalment_result_description,
  @business_type business_type,
  @policy_is_paid policy_is_paid,
  @insurance_file_status_description insurance_file_status_description,
  @base_balance outstanding_policy_amount_for_cancellations_only,
  @processing_days processing_days,
  @original_due_date

END





GO
