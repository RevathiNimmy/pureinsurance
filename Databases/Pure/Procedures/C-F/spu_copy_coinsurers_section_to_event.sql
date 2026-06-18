SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_copy_coinsurers_section_to_event'
GO


CREATE PROCEDURE spu_copy_coinsurers_section_to_event
    @event_cnt int,
    @insurance_file_cnt int
AS

BEGIN

DECLARE coinsurer_cursor CURSOR FAST_FORWARD FOR SELECT policy_coinsurers_section_id FROM policy_coinsurers_section WHERE insurance_file_cnt=@insurance_file_cnt
DECLARE @policy_coinsurers_section_id int
DECLARE @event_policy_coinsurers_section_id int

OPEN coinsurer_cursor

FETCH NEXT FROM coinsurer_cursor INTO @policy_coinsurers_section_id

WHILE @@FETCH_STATUS=0
BEGIN

	SELECT @event_policy_coinsurers_section_id=ISNULL(MAX(policy_coinsurers_section_id),0)+1 FROM event_policy_coinsurers_section
	
    INSERT INTO event_policy_coinsurers_section
    (
	policy_coinsurers_section_id,
	insurance_file_cnt,
	party_cnt,
	COB_rating_section_id,	
	sequence,
	share_percent,
	premium_exc_tax,
	premium_inc_tax,
	tax_group_id,
	commission_percent,
	commission_charge,
	commission_exc_tax,
	commission_inc_tax,
	commission_tax_group_id,
	base_premium_exc_tax,
	base_premium_inc_tax,
	base_commission_charge,
	base_commission_exc_tax,
	base_commission_inc_tax,
	override_rate_table,
	is_applied
    )
    SELECT
	@event_policy_coinsurers_section_id,
	@event_cnt,
	party_cnt,
	COB_rating_section_id,	
	sequence,
	share_percent,
	premium_exc_tax,
	premium_inc_tax,
	tax_group_id,
	commission_percent,
	commission_charge,
	commission_exc_tax,
	commission_inc_tax,
	commission_tax_group_id,
	base_premium_exc_tax,
	base_premium_inc_tax,
	base_commission_charge,
	base_commission_exc_tax,
	base_commission_inc_tax,
	override_rate_table,
	is_applied
    FROM
    policy_coinsurers_section
    WHERE
    policy_coinsurers_section_id = @policy_coinsurers_section_id

	IF EXISTS(SELECT NULL FROM tax_calculation WHERE insurance_file_cnt=@insurance_file_cnt AND policy_coinsurers_section_id=@policy_coinsurers_section_id)
		BEGIN
			INSERT INTO event_tax_calculation
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
			@event_cnt,
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
			@event_policy_coinsurers_section_id,
			is_commission_tax
			FROM
			tax_calculation
			WHERE
			insurance_file_cnt = @insurance_file_cnt
			AND
			policy_coinsurers_section_id = @policy_coinsurers_section_id	
		END

	FETCH NEXT FROM coinsurer_cursor INTO @policy_coinsurers_section_id

END

CLOSE coinsurer_cursor
DEALLOCATE coinsurer_cursor

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

