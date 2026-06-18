SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_copy_event_to_policy_fee'
GO

-- AMB 03/10/2003: Accident Management RFC development

CREATE PROCEDURE spu_copy_event_to_policy_fee
    @event_cnt           int,
    @insurance_file_cnt  int
AS

BEGIN

DELETE FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt AND policy_fee_id IS NOT NULL
DELETE FROM policy_fee WHERE insurance_file_cnt = @insurance_file_cnt

DECLARE fee_cursor CURSOR FAST_FORWARD FOR SELECT policy_fee_id FROM event_policy_fee WHERE insurance_file_cnt=@event_cnt
						ORDER BY policy_fee_id

DECLARE @event_policy_fee_id int
DECLARE @policy_fee_id int

OPEN fee_cursor

FETCH NEXT FROM fee_cursor INTO @event_policy_fee_id

WHILE @@FETCH_STATUS=0
BEGIN

    INSERT INTO  
	policy_fee
	(
	insurance_file_cnt,
	party_cnt,
	fee_percentage,
	fee_amount,
	commission_percentage,
	commission_amount,
	isIPTable,
	extra_scheme_id,
	Base_currency_id,
	Base_fee_amount,
	Base_fee_commission_value,
	tax_amount,
	total_fee,
	commission_tax_amount,
	total_commission,
	Base_tax_amount,
	Base_Commission_tax_amount,
	Base_Total_Commission,
	Reference,
        fsa_type_of_sale_id,
        insurer_fee_type
    )
    SELECT	
	@insurance_file_cnt,
	party_cnt,
	fee_percentage,
	fee_amount,
	commission_percentage,
	commission_amount,
	isIPTable,
	extra_scheme_id,
	Base_currency_id,
	Base_fee_amount,
	Base_fee_commission_value,
	tax_amount,
	total_fee,
	commission_tax_amount,
	total_commission,
	Base_tax_amount,
	Base_Commission_tax_amount,
	Base_Total_Commission,
	Reference,
        fsa_type_of_sale_id,
        ISNULL(insurer_fee_type,'')
    FROM    
    event_policy_fee
    WHERE    
    policy_fee_id=@event_policy_fee_id

	SELECT @policy_fee_id=@@IDENTITY
	
	IF EXISTS(SELECT NULL FROM event_tax_calculation WHERE insurance_file_cnt=@event_cnt AND policy_fee_id=@event_policy_fee_id)
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
			@policy_fee_id,
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
			FROM
			event_tax_calculation
			WHERE
			insurance_file_cnt = @event_cnt
			AND
			policy_fee_id = @event_policy_fee_id

		END
	
	FETCH NEXT FROM fee_cursor INTO @event_policy_fee_id
	
END

CLOSE fee_cursor
DEALLOCATE fee_cursor

END
GO


