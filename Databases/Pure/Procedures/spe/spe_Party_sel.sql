SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_sel'
GO
/*  
   sj 13/06/2002 - Add loyalty_number, alternative_identifier, marketing_segment_ind,  
                   trading_name and sub_branch_id  
  
   sd 17/07/2002 - Added left outer join to Sub Branch Table to access description  
*/  
    
CREATE PROCEDURE spe_Party_sel    
    @party_cnt INT    =null, 
	@sShortCode AS VARCHAR(20) = null
AS    
SELECT    
    
    p.party_cnt,    
    p.party_type_id,    
    p.is_also_agent,    
    p.party_structure_id,    
    p.source_id,    
    p.party_id,    
    p.shortname,    
    p.name,    
    p.resolved_name,    
    p.currency_id,    
    p.language_id,    
    p.collect_type_id,    
    p.accum_treatment_type_id,    
    p.stats_treatment_type_id,    
    p.party_category_id,    
    p.agent_cnt,    
    p.consultant_cnt,    
    p.created_by_id,    
    p.date_created,    
    p.last_modified,    
    p.modified_by_id,    
    p.payment_method_code,    
    p.payment_term_code,    
    p.credit_card_code,    
    p.file_code,    
    p.abc_count,    
    p.statements,    
    p.reminder_type_id,    
    p.renewals,    
    p.status,    
    p.last_action_type,    
    p.is_travel_agent,    
    p.is_prospect,    
    p.is_deleted,    
    p.abi_code_on_406,    
    p.abi_code_on_81,    
    p.abi_codelist,    
    p.area_id,    
    p.service_level_id,    
    p.invariant_key,    
    p.record_status,    
    p.CCJs,    
    p.user_defined_data_id,    
    p.seasonal_gift_id,    
    p.correspondence_type_id,    
    p.renewal_stop_code_id,
	p.blacklist_reason_id,
    p.swift_party_id,    
    p.loyalty_number,    
    p.alternative_identifier,    
    p.marketing_segment_ind,    
    p.trading_name,    
    s.sub_branch_id,    
    s.description as sub_branch_name,    
    p.tob_letter,    
    (SELECT use_override_commission_rate FROM party_agent WHERE party_agent.party_cnt=p.party_cnt) AS use_override_commission_rate,   
    (SELECT use_override_commission_renewal FROM party_agent WHERE party_agent.party_cnt=p.party_cnt) AS use_override_commission_renewal,
 p1.shortname as 'LeadAgent_shortname',    
 p1.resolved_name as 'LeadAgent_resolved_name'
 FROM Party p LEFT OUTER JOIN Sub_branch s    
 ON p.Sub_Branch_id = s.Sub_Branch_id    
  
LEFT JOIN party p1 ON p.agent_cnt=p1.party_cnt    
  
WHERE p.party_cnt = @party_cnt OR p.shortname = @sShortCode 
 
GO