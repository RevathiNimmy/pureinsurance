SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_copy_tax_calculation_to_event'
GO

CREATE PROCEDURE spu_copy_tax_calculation_to_event
	@event_cnt int,
    	@insurance_file_cnt int
AS

BEGIN
 
INSERT INTO event_tax_calculation(
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
        is_commission_tax)
SELECT @event_cnt,
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
FROM    tax_calculation
WHERE   insurance_file_cnt = @insurance_file_cnt

UPDATE ET
	SET ET.Insurance_section_id = 
	(SELECT EICS.Insurance_Section_ID 
		FROM Event_insurance_COB_Section EICS
		JOIN insurance_COB_section ICS on ICS.COB_Rating_Section_Id = EICS.COB_Rating_Section_id	
		WHERE EICS.insurance_file_cnt = @event_cnt
		AND ICS.insurance_file_cnt = @insurance_file_cnt
		AND ET.insurance_section_id = ICS.insurance_section_id
	)
	FROM Event_Tax_Calculation ET 
	WHERE ET.insurance_file_cnt = @event_cnt

--currently the event_tax_calculation table holds the policy_agents_id of the policy_agents table
--needs updating to match the event_policy_agents table
DECLARE @policyagentsid INT, @agentcnt INT, @eventpolicyagentsid INT
DECLARE c_cursor CURSOR FAST_FORWARD FOR
SELECT EPA.policy_agents_id, PA.policy_agents_id, PA.agent_cnt
FROM event_policy_agents EPA
JOIN policy_agents PA ON PA.agent_cnt = EPA.agent_cnt
AND PA.insurance_file_cnt = @insurance_file_cnt
WHERE EPA.insurance_file_cnt = @event_cnt
OPEN c_cursor
FETCH NEXT FROM c_cursor INTO @eventpolicyagentsid, @policyagentsid, @agentcnt
WHILE @@FETCH_STATUS = 0 
BEGIN
	UPDATE event_tax_calculation
	SET policy_agents_id = @eventpolicyagentsid
	WHERE insurance_file_cnt = @event_cnt
	AND policy_agents_id = @policyagentsid

	FETCH NEXT FROM c_cursor INTO @eventpolicyagentsid, @policyagentsid, @agentcnt
END
CLOSE c_cursor
DEALLOCATE c_cursor

--currently the event_tax_calculation table holds the policy_fee_id of the policy_fee table
--needs updating to match the event_policy_fee table
DECLARE @policyfeeid INT, @partycnt INT, @eventpolicyfeeid INT
DECLARE c_cursor CURSOR FAST_FORWARD FOR
SELECT EPF.policy_fee_id, PF.policy_fee_id, PF.party_cnt
FROM event_policy_fee EPF
JOIN policy_fee PF ON PF.party_cnt = EPF.party_cnt
AND PF.insurance_file_cnt = @insurance_file_cnt
WHERE EPF.insurance_file_cnt = @event_cnt
OPEN c_cursor
FETCH NEXT FROM c_cursor INTO @eventpolicyfeeid, @policyfeeid, @partycnt
WHILE @@FETCH_STATUS = 0 
BEGIN
	UPDATE event_tax_calculation
	SET policy_fee_id = @eventpolicyfeeid
	WHERE insurance_file_cnt = @event_cnt
	AND policy_fee_id = @policyfeeid

	FETCH NEXT FROM c_cursor INTO @eventpolicyfeeid, @policyfeeid, @partycnt
END
CLOSE c_cursor
DEALLOCATE c_cursor

END
GO