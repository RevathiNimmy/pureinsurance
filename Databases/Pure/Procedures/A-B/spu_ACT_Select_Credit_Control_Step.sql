SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Credit_Control_Step'
GO


CREATE PROCEDURE spu_ACT_Select_Credit_Control_Step  
	@credit_control_step_id INT  
AS  
  
BEGIN  
  
 SELECT  
	credit_control_step_id,  
	credit_control_rule_id,  
	step_number,  
	number_of_days,  
	broker_days,  
	client_document_template_id,  
	oip_document_template_id,  
	broker_report_id,  
	policy_tolerance_amount,  
	account_tolerance_amount,  
	pmwrk_task_id,  
	pmuser_group_id,  
	check_auto_cancel,  
	auto_cancel_policy,  
	next_step,  
	previous_step,  
	off_hold_step,  
	recurring_days,  
	recurring_letters,  
	jump_to_next_step,  
	second_letter_id,  
	second_oip_letter_id,  
	second_pmwrk_task_id,  
	second_pmuser_group_id,  
	action_type_id,  
	second_action_type_id,  
	percentage_step_one,  
	percentage_step_two,  
	step_description,  
	pmwrk_task_group_id,  
	broker_letter_id,  
	stop_account,  
	auto_lapse_renewal,  
	instalment_failure_count,  
	auto_cancel_document_1_trigger_amount,  
	auto_cancel_document_2_trigger_amount,  
	auto_cancel_document_1_template_id,  
	auto_cancel_document_2_template_id, 
	write_off_tolerance, 
	write_off_reason_id,
	jump_to_next_step_broker,
	single_instalment_jump_to_next_step_broker,
	single_instalment_account_number_of_days,
	single_instalment_account_tollerance_amount,
	single_instalment_broker_letter_id
  
 FROM Credit_Control_Step  
 WHERE credit_control_step_id = @credit_control_step_id  
  
END  





GO
