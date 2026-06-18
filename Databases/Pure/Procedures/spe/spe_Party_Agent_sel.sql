SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Agent_sel'
GO
CREATE PROCEDURE spe_Party_Agent_sel  
    @party_cnt int  
AS  
Declare @agentpreferredcorrespondencetype varchar(50)

Select @agentpreferredcorrespondencetype = code from party p left join contact_type c on  p.correspondence_type_id = c.contact_type_id
where party_cnt = @party_cnt

SELECT  
    party_cnt,  
    party_agent_type_id,  
    party_agent_origin_id,  
    agency_agreement_date,  
    agency_next_review_date,  
    agency_account_number,  
    is_branch,  
    is_head_office,  
    default_commission_percent,  
    trading_name,  
    binder_indicator,  
    report_indicator,  
    linked_account_executive_id,  
    linked_account_group,  
    payment_method,  
    payment_frequency,  
    address_on_notice,  
    type_of_statement,  
    source,  
    title,  
    multipac,  
    contact_person,  
    first_name,  
    --bank_account,  
    date_cancelled,
    agent_status_id,
    fsa_registration_number,
    broker_abi_id,
    expense_account_id,
    is_in_transfer_mode,
    transfer_to_business_type_id,
    transfer_to_party_cnt,
    use_override_commission_rate,
    domiciled_for_tax,
    allow_consolidated_commission,
    can_make_live_invoice,
    can_make_live_instalments,
    can_make_live_paynow,
    is_standard_account,
    is_float_balance_account,
    is_overdraft_account,
    is_prepayment_account,
    expected_daily_premium,
    days_allowed,
    float_balance_limit,
    overdraft_limit,
    overdraft_expiry,
    alternate_reference_mandatory,
    alternate_reference_for_each_transaction,  --(RC) QBENZ014
    commission_posting_type_id,  --(RC) PLICO 9-10
    is_viewable_only,
    is_single_instalment_plan,
    common_renewal_date,
    produce_agent_renewal_list,
    can_make_live_bankguarantee,
    can_make_live_cashdeposit,
	commission_level_id ,  
    use_override_commission_renewal,   
    is_gross_agent,
    ISNULL(bankaccount_id,0) AS bankaccount_id,
	@agentpreferredcorrespondencetype  correspondence_type ,
	receives_client_correspondence
FROM Party_Agent
WHERE party_cnt = @party_cnt  
