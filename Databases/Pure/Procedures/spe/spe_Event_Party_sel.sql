SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Event_Party_sel'
GO

CREATE PROCEDURE spe_Event_Party_sel
    @party_cnt int
AS
SELECT
    party_cnt,
    party_type_id,
    is_also_agent,
    party_structure_id,
    source_id,
    party_id,
    shortname,
    name,
    resolved_name,
    currency_id,
    language_id,
    collect_type_id,
    accum_treatment_type_id,
    stats_treatment_type_id,
    party_category_id,
    agent_cnt,
    consultant_cnt,
    created_by_id,
    date_created,
    last_modified,
    modified_by_id,
    payment_method_code,
    payment_term_code,
    credit_card_code,
    file_code,
    abc_count,
    statements,
    reminder_type_id,
    renewals,
    status,
    last_action_type,
    is_travel_agent,
    is_prospect,
    is_deleted,
    abi_code_on_406,
    abi_code_on_81,
    abi_codelist,
    area_id,
    service_level_id,
    invariant_key,
    record_status,
    CCJs,
    user_defined_data_id,
    seasonal_gift_id,
    correspondence_type_id,
    renewal_stop_code_id,
    swift_party_id,
    loyalty_number,
    alternative_identifier,
    marketing_segment_ind,
    trading_name,
    sub_branch_id,
    tob_letter
 FROM event_party
WHERE party_cnt = @party_cnt

GO

