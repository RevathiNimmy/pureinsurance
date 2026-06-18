SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_copy_event_to_policy_agents'
GO

CREATE PROCEDURE spu_copy_event_to_policy_agents
    @event_cnt int,
    @insurance_file_cnt int
AS

BEGIN

DELETE FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt AND policy_agents_id IS NOT NULL
DELETE FROM policy_agents WHERE insurance_file_cnt = @insurance_file_cnt

DECLARE agent_cursor CURSOR FAST_FORWARD FOR SELECT policy_agents_id FROM event_policy_agents WHERE insurance_file_cnt=@event_cnt

DECLARE @event_policy_agents_id int
DECLARE @policy_agents_id int

OPEN agent_cursor

FETCH NEXT FROM agent_cursor INTO @event_policy_agents_id

WHILE @@FETCH_STATUS=0
BEGIN

	INSERT INTO policy_agents
	(
	insurance_file_cnt,
	agent_cnt,
	agent_count,
	agent_commission_percentage,
	agent_commission_amount,
	agent_commission_value,
	is_minimum_brokerage,
	override_rate_table,
	apply_perc_to_prem_or_comm,
	base_currency_id,
	base_agent_commission_amount,
	base_agent_commission_value,
	tax_amount,
	Base_tax_amount
	)
	SELECT
	@insurance_file_cnt,
	agent_cnt,
	agent_count,
	agent_commission_percentage,
	agent_commission_amount,
	agent_commission_value,
	is_minimum_brokerage,
	override_rate_table,
	apply_perc_to_prem_or_comm,
	base_currency_id,
	base_agent_commission_amount,
	base_agent_commission_value,
	tax_amount,
	Base_tax_amount
	FROM
	event_policy_agents
	WHERE
	policy_agents_id=@event_policy_agents_id

	SELECT @policy_agents_id=@@IDENTITY
	
	IF EXISTS(SELECT NULL FROM event_tax_calculation WHERE insurance_file_cnt=@event_cnt AND policy_agents_id=@event_policy_agents_id)
		BEGIN
			INSERT INTO tax_calculation
			(
			insurance_file_cnt,
			risk_cnt,
			tax_band_id,
			premium,
			percentage,
			value,
			is_value,
			is_manually_changed,
			Calc_Basis,
			Basis_Value,
			Sum_Insured,
			Sum_Insured_Rounded,
			currency_id,
			allow_tax_credit,
			original_sum_insured,
			country_id,
			state_id,
			class_of_business_id,
			tax_group_id,
			sequence,
			transtype,
			policy_fee_u_id,
			agent_commission_cnt,
			ri_party_cnt,
			insurance_section_id,
			policy_fee_id,
			policy_agents_id,
			insurer_party_cnt,
			claim_peril_id,
			claim_payment_id,
			claim_receipt_id,
			claim_payment_item_id,
			claim_receipt_item_id,
			ri_arrangement_line_id,
			is_not_applied_to_client,
			include_tax_in_instalments,
			spread_tax_across_instalments,
			base_tax_calculation_cnt,
			version_id,
			pfprem_finance_cnt,
			pfprem_finance_version,
			policy_coinsurers_section_id,
			is_commission_tax
			)
			SELECT
			@insurance_file_cnt,
			risk_cnt,
			tax_band_id,
			premium,
			percentage,
			value,
			is_value,
			is_manually_changed,
			Calc_Basis,
			Basis_Value,
			Sum_Insured,
			Sum_Insured_Rounded,
			currency_id,
			allow_tax_credit,
			original_sum_insured,
			country_id,
			state_id,
			class_of_business_id,
			tax_group_id,
			sequence,
			transtype,
			policy_fee_u_id,
			agent_commission_cnt,
			ri_party_cnt,
			insurance_section_id,
			policy_fee_id,
			@policy_agents_id,
			insurer_party_cnt,
			claim_peril_id,
			claim_payment_id,
			claim_receipt_id,
			claim_payment_item_id,
			claim_receipt_item_id,
			ri_arrangement_line_id,
			is_not_applied_to_client,
			include_tax_in_instalments,
			spread_tax_across_instalments,
			base_tax_calculation_cnt,
			version_id,
			pfprem_finance_cnt,
			pfprem_finance_version,
			policy_coinsurers_section_id,
			is_commission_tax
			FROM
			event_tax_calculation
			WHERE
			insurance_file_cnt = @event_cnt
			AND
			policy_agents_id = @event_policy_agents_id
				
		END
	
	FETCH NEXT FROM agent_cursor INTO @event_policy_agents_id
	
END

CLOSE agent_cursor
DEALLOCATE agent_cursor

END
GO


