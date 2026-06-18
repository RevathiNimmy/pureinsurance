SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_party_to_event'
GO


CREATE PROCEDURE spu_copy_party_to_event
    @event_cnt int,
    @party_cnt int
AS


BEGIN
INSERT INTO event_party (
    party_cnt,
    party_type_id,
    party_structure_id,
    is_also_agent,
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
    agent_cnt,
    created_by_id,
    party_category_id,
    date_created,
    last_modified,
    consultant_cnt,
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
    user_defined_data_id)
select @event_cnt,
    party_type_id,
    party_structure_id,
    is_also_agent,
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
    agent_cnt,
    created_by_id,
    party_category_id,
    date_created,
    last_modified,
    consultant_cnt,
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
    user_defined_data_id
from    party
where   party_cnt = @party_cnt
END
GO


