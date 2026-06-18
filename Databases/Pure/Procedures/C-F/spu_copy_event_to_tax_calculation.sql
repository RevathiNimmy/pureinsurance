SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXEC DDLDropProcedure 'spu_copy_event_to_tax_calculation'
GO

CREATE PROCEDURE spu_copy_event_to_tax_calculation
    @event_cnt int,
    @insurance_file_cnt int
AS

BEGIN
DELETE FROM
    tax_calculation
WHERE
    insurance_file_cnt = @insurance_file_cnt
INSERT INTO tax_calculation (
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

SELECT 	@event_cnt,
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
	(select max(ICS.insurance_section_id) from
		Insurance_COB_Section ICS
		JOIN Event_Insurance_COB_Section EIS on ISNULL(EIS.COB_Rating_Section_Id,0) = ISNULL(ICS.COB_Rating_Section_Id,0)
		JOIN Event_tax_calculation ETC2 On ETC2.insurance_section_id = EIS.insurance_section_id
		WHERE ETC2.insurance_file_cnt = @event_cnt
		AND ETC.insurance_section_id = ETC2.insurance_section_id),
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

from    event_tax_calculation ETC
where   insurance_file_cnt = @event_cnt
END

DECLARE @policyagentsid INT, @agentcnt INT, @eventpolicyagentsid INT
DECLARE c_cursor CURSOR FAST_FORWARD FOR
SELECT PA.policy_agents_id, EPA.policy_agents_id, EPA.agent_cnt
FROM policy_agents PA
JOIN event_policy_agents EPA ON EPA.agent_cnt = PA.agent_cnt
AND EPA.insurance_file_cnt = @insurance_file_cnt
WHERE PA.insurance_file_cnt = @event_cnt
OPEN c_cursor
FETCH NEXT FROM c_cursor INTO @policyagentsid, @eventpolicyagentsid, @agentcnt
WHILE @@FETCH_STATUS = 0 
BEGIN

	UPDATE tax_calculation
	SET policy_agents_id = @policyagentsid
	WHERE insurance_file_cnt = @insurance_file_cnt
	AND policy_agents_id = @eventpolicyagentsid

	FETCH NEXT FROM c_cursor INTO @policyagentsid, @eventpolicyagentsid, @agentcnt
END
CLOSE c_cursor
DEALLOCATE c_cursor

DECLARE @policyfeeid INT, @partycnt INT, @eventpolicyfeeid INT
DECLARE c_cursor CURSOR FAST_FORWARD FOR
SELECT PF.policy_fee_id, EPF.policy_fee_id, EPF.party_cnt
FROM policy_fee PF
JOIN event_policy_fee EPF ON PF.party_cnt = EPF.party_cnt
AND EPF.insurance_file_cnt = @insurance_file_cnt
WHERE PF.insurance_file_cnt = @event_cnt
OPEN c_cursor
FETCH NEXT FROM c_cursor INTO @policyfeeid, @eventpolicyfeeid, @partycnt
WHILE @@FETCH_STATUS = 0 
BEGIN

	UPDATE tax_calculation
	SET policy_fee_id = @policyfeeid
	WHERE insurance_file_cnt = @insurance_file_cnt
	AND policy_fee_id = @eventpolicyfeeid

	FETCH NEXT FROM c_cursor INTO @policyfeeid, @eventpolicyfeeid, @partycnt
END
CLOSE c_cursor
DEALLOCATE c_cursor

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
