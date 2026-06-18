SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_event_to_policy_section'
GO


CREATE PROCEDURE spu_copy_event_to_policy_section
    @event_cnt int,
    @insurance_file_cnt int
AS
BEGIN

DELETE FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt AND insurance_section_id IS NOT NULL
DELETE FROM insurance_COB_section WHERE insurance_file_cnt = @insurance_file_cnt

DECLARE section_cursor CURSOR FAST_FORWARD FOR SELECT insurance_section_id FROM event_insurance_COB_section WHERE insurance_file_cnt=@event_cnt

DECLARE @event_insurance_section_id int
DECLARE @insurance_section_id int

OPEN section_cursor

FETCH NEXT FROM section_cursor INTO @event_insurance_section_id

WHILE @@FETCH_STATUS=0
BEGIN

    INSERT INTO insurance_COB_section
    (
    Insurance_file_cnt,  
    COB_Rating_section_id ,
    Premium_Excluding_Tax ,
    Tax_applied,
    Premium_Including_Tax ,
    Tax_group_id,
    commission_cnt,
    commission_percentage,
    commission_charge,
    commission_net,
    commission_tax_applied,
    commission_payable,
    commission_tax_group_id,
    is_minimum_brokerage,
    override_rate_table,
    base_premium_excluding_tax,
    base_tax_applied,
    base_premium_including_tax,
    base_commission_charge,
    base_commission_net,
    base_commission_tax_applied,
    base_commission_payable,
    is_applied
    )
    select
    @insurance_file_cnt,
    COB_Rating_section_id ,
    Premium_Excluding_Tax ,
    Tax_applied,
    Premium_Including_Tax ,
    Tax_group_id,
    commission_cnt,
    commission_percentage,
    commission_charge,
    commission_net,
    commission_tax_applied,
    commission_payable,
    commission_tax_group_id,
    is_minimum_brokerage,
    override_rate_table,
    base_premium_excluding_tax,
    base_tax_applied,
    base_premium_including_tax,
    base_commission_charge,
    base_commission_net,
    base_commission_tax_applied,
    base_commission_payable,
    is_applied
    from
    event_insurance_COB_section
    where
    insurance_file_cnt = @event_cnt
    AND
    insurance_section_id=@event_insurance_section_id
    
    

    SELECT @insurance_section_id=@@IDENTITY
    
    IF EXISTS(SELECT NULL FROM event_tax_calculation WHERE insurance_file_cnt=@event_cnt AND insurance_section_id=@event_insurance_section_id)
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
        @insurance_section_id,
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
        FROM
        event_tax_calculation
        WHERE
        insurance_file_cnt = @event_cnt
        AND
        insurance_section_id = @event_insurance_section_id

    END
        
    FETCH NEXT FROM section_cursor INTO @event_insurance_section_id
    
END

CLOSE section_cursor
DEALLOCATE section_cursor

END
GO


