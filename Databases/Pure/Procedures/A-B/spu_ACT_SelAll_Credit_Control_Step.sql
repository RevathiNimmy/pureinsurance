SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Credit_Control_Step'
GO

CREATE PROCEDURE spu_ACT_SelAll_Credit_Control_Step
    @credit_control_rule_id INT
AS

BEGIN

    SELECT credit_control_step_id,
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
			--jmf 24/7/2003 - claim debt credit control
			second_letter_id,
			second_oip_letter_id,
			second_pmwrk_task_id,
			second_pmuser_group_id
			action_type_id,
			second_action_type_id,
			percentage_step_one,
			percentage_step_two
      FROM Credit_Control_Step
     WHERE credit_control_rule_id = @credit_control_rule_id
  ORDER BY step_number

END
GO


