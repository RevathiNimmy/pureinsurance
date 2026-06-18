SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_copy_policy'
GO


CREATE  PROCEDURE spu_pmb_copy_policy
    @old_insurance_file_cnt int,
    @new_insurance_file_cnt int OUTPUT,
    @target_party_cnt int,
    @target_source_id int,
    @CopyTextFile int = 1
AS


BEGIN

/*
    Copy a policy, and its related text_file entries
    CTAF 090701
*/

DECLARE @old_ins_folder_cnt int
DECLARE @new_ins_folder_id int
DECLARE @new_ins_folder_cnt int
DECLARE @new_ins_file_id int
DECLARE @table varchar(70)
DECLARE @error int
DECLARE @code varchar(30)
DECLARE @old_source_id INT
DECLARE @old_branch_id INT /*Sub Branch*/
DECLARE @new_source_id INT
DECLARE @new_branch_id INT /*Sub Branch*/

DECLARE @fees_party_cnt             	int,
    	@fees_fee_percentage         	numeric(7,4),
    	@fees_fee_amount         	numeric(19,4),
    	@fees_commission_percentage	numeric(7,4),
    	@fees_commission_amount         numeric(19,4),
    	@fees_isIPTable                 int,
    	@fees_reference         	varchar(255),
    	@fees_extra_scheme_id           int,
    	@fees_base_currency_id          smallint,
    	@fees_base_fee_amount           money,
    	@fees_base_fee_commission_value money,
    	@fees_insurer_fee_type  	char(1),
    	@fees_tax_amount  		numeric(19,4),
    	@fees_total_fee   		numeric(19,4),
    	@fees_commission_tax_amount     numeric(19,4),
    	@fees_total_commission          numeric(19,4),
    	@fees_FSA_Type_Of_Sale_Id       int,
    	@fees_policy_fee_id             int,
    	@new_policy_fee_id              int


DECLARE @agents_agent_cnt 		     int,
    	@agents_agent_count  		     int,
    	@agents_agent_commission_percentage  numeric(19,4),
    	@agents_agent_commission_amount      numeric(19,4),
    	@agents_agent_commission_value       numeric(19,4),
    	@agents_is_minimum_brokerage         tinyint,
    	@agents_override_rate_table          tinyint,
    	@agents_apply_perc_to_prem_or_comm   tinyint,
    	@agents_base_currency_id             smallint,
    	@agents_base_agent_commission_amount money,
    	@agents_base_agent_commission_value  money,
    	@agents_tax_amount 	             numeric(19,4),
    	@agents_policy_agents_id 	     int,
    	@new_policy_agents_id 		     int


DECLARE @sections_insurance_section_id 	      int,
    	@sections_COB_rating_section_id       int,
    	@sections_premium_excluding_tax       numeric(19,4),
    	@sections_tax_applied 		      numeric(19,4),
    	@sections_premium_including_tax       numeric(19,4),
    	@sections_tax_group_id 		      int,
    	@sections_commission_cnt 	      int,
    	@sections_commission_percentage       numeric(19,4),
    	@sections_commission_charge 	      numeric(19,4),
    	@sections_commission_net 	      numeric(19,4),
    	@sections_commission_tax_applied      numeric(19,4),
    	@sections_commission_payable 	      numeric(19,4),
    	@sections_commission_tax_group_id     int,
    	@sections_is_minimum_brokerage 	      tinyint,
    	@sections_override_rate_table 	      tinyint,
    	@sections_base_premium_excluding_tax  numeric(19,4),
    	@sections_base_tax_applied 	      numeric(19,4),
    	@sections_base_premium_including_tax  numeric(19,4),
    	@sections_base_commission_charge      numeric(19,4),
    	@sections_base_commission_net 	      numeric(19,4),
    	@sections_base_commission_tax_applied numeric(19,4),
    	@sections_base_commission_payable     numeric(19,4),
    	@new_insurance_section_id 	      int

BEGIN TRANSACTION

-- Reset error counter
SELECT @error = 0

-- Get the old insurance_folder_cnt
SELECT
    @old_ins_folder_cnt = insurance_folder_cnt,
    @old_source_id = source_id,
    @old_branch_id = branch_id
FROM Insurance_file
WHERE insurance_file_cnt = @old_insurance_file_cnt

IF ISNULL(@target_source_id,0) = 0
BEGIN
    SELECT @new_source_id = @old_source_id
    SELECT @new_branch_id = @old_branch_id
END
ELSE
BEGIN
    SELECT @new_source_id = @target_source_id
    EXEC spu_sub_branch_default @new_source_id, @new_branch_id OUTPUT
END

-- Error trapping
SELECT @error = @error + @@error

SET @new_ins_folder_id=0
-- Generate a new code
-- MSS280801 - Generated sensible InsuranceRef
--SELECT @code = left(NEWID(), 30)
SELECT @code = 'T' + Replace(str(Datepart(YYYY,GetDate()),4)
    + str(Datepart(MM,GetDate()),2)
    + str(Datepart(DD,GetDate()),2)
    + str(Datepart(HH,GetDate()),2)
    + str(Datepart(MI,GetDate()),2)
    + str(Datepart(SS,GetDate()),2)
    ,' ',0)

-- Error trapping
SELECT @error = @error + @@error

-- Create new insurance_folder
PRINT 'Insurance_Folder'
INSERT INTO Insurance_folder
( insurance_folder_id, source_id, insurance_holder_cnt, code, description, inception_date, arc_archive_folder_id, quote_insurance_ref, next_insurance_ref, last_insurance_ref, renewal_count )
SELECT @new_ins_folder_id, @new_source_id, @target_party_cnt, @code, description, inception_date, arc_archive_folder_id, quote_insurance_ref, next_insurance_ref, last_insurance_ref, renewal_count
FROM Insurance_folder
WHERE insurance_folder_cnt = @old_ins_folder_cnt
-- Error trapping
SELECT @error = @error + @@error

-- Get the insurance_file_cnt
SELECT @new_ins_folder_cnt = MAX(insurance_folder_cnt) FROM Insurance_Folder

-- Error trapping
SELECT @error = @error + @@error

SET @new_ins_file_id=0
-- insurance_file
PRINT 'Insurance_File'
INSERT INTO Insurance_file
(insurance_file_structure_id, insurance_file_type_id, insurance_file_status_id, insurance_file_id, source_id, insurance_folder_cnt, insurance_ref, product_id, lead_insurer_cnt, lead_agent_cnt, lead_agent_percent, account_handler_cnt, insured_cnt, business_type_id, collect_type_id, collection_from_cnt, branch_id, currency_id, language_id, date_issued, cover_start_date, expiry_date, renewal_date, renewal_method_id, renewal_frequency_id, is_referred_at_renewal, lapsed_reason_id, lapsed_date, lapsed_description, is_referred_on_mta, policy_version, gemini_policy_status, gemini_business_type, deferred_ind, policy_ignore, broker_cnt, risk_code_id, Analysis_code_id, proposal_date, diary_date, review_date, renewal_day_number, Policy_type_id, indicator, clause, cover, area, long_term_undertaking_date, renewal_stop_code_id, vbs_type, vbs_status, is_insurer_rate_table, is_related_policies, is_retained_documents, schemes_postcode, paid_direct, scheme, brokerage_amount, is_minimum_brokerage_flag, annual_premium, this_premium, net_premium, commission_amount, iptable_amount, ipt_percentage, is_ipt_overridden, tax_amount, vatable_amount, vat_percentage, vat_amount, payment_method, user_defined_data_id, commission_percentage, invariant_key, insured_name, alternate_reference, is_client_invoiced, old_policy_number, quote_expiry_date, alternate_account_cnt, account_executive_cnt, anniversary_date, fsa_customer_category_id, fsa_contract_location_id, fsa_underwriter_cnt, fsa_type_of_sale_id, fsa_renewal_consent, policy_style_id, underwriting_year_id, policy_status_id, edi_message_sent, return_premium_currency_id, exchange_rate_override_reason_id, base_currency_id, currency_base_xrate, agent_account_currency_id, agent_account_base_xrate, system_base_xrate, currency_base_date, account_base_date, system_base_date, inception_date_tpi, country_id)
SELECT insurance_file_structure_id, insurance_file_type_id, insurance_file_status_id, @new_ins_file_id, @new_source_id, @new_ins_folder_cnt, @code, product_id, lead_insurer_cnt, lead_agent_cnt, lead_agent_percent, account_handler_cnt, @target_party_cnt, business_type_id, collect_type_id, collection_from_cnt, @new_branch_id, currency_id, language_id, date_issued, cover_start_date, expiry_date, renewal_date, renewal_method_id, renewal_frequency_id, is_referred_at_renewal, lapsed_reason_id, lapsed_date, lapsed_description, is_referred_on_mta, policy_version, gemini_policy_status, gemini_business_type, deferred_ind, policy_ignore, broker_cnt, risk_code_id, Analysis_code_id, proposal_date, diary_date, review_date, renewal_day_number, Policy_type_id, indicator, clause, cover, area, long_term_undertaking_date, renewal_stop_code_id, vbs_type, vbs_status, is_insurer_rate_table, is_related_policies, is_retained_documents, schemes_postcode, paid_direct, scheme, brokerage_amount, is_minimum_brokerage_flag, annual_premium, this_premium, net_premium, commission_amount, iptable_amount, ipt_percentage, is_ipt_overridden, tax_amount, vatable_amount, vat_percentage, vat_amount, payment_method, user_defined_data_id, commission_percentage, invariant_key, insured_name, alternate_reference, is_client_invoiced, old_policy_number, quote_expiry_date, alternate_account_cnt, account_executive_cnt, anniversary_date, fsa_customer_category_id, fsa_contract_location_id, fsa_underwriter_cnt, fsa_type_of_sale_id, fsa_renewal_consent, policy_style_id, underwriting_year_id, policy_status_id, edi_message_sent, return_premium_currency_id, exchange_rate_override_reason_id, base_currency_id, currency_base_xrate, agent_account_currency_id, agent_account_base_xrate, system_base_xrate, currency_base_date, account_base_date, system_base_date, inception_date_tpi, country_id
FROM Insurance_file
WHERE insurance_file_cnt = @old_insurance_file_cnt
-- Error trapping
SELECT @error = @error + @@error

-- Get the insurance_file
SELECT @new_insurance_file_cnt = MAX(Insurance_File_Cnt) FROM Insurance_File

PRINT 'Insurance_File_System'
INSERT INTO Insurance_file_system
( insurance_file_cnt, endorsement_count, created_by_id, date_created, modified_by_id, last_modified, last_trans_date, last_trans_type_id, last_trans_description, last_trans_debit_credit, last_trans_document_ref, last_trans_cover_start_date, last_trans_expiry_date )
SELECT @new_insurance_file_cnt, endorsement_count, created_by_id, date_created, modified_by_id, getdate(), last_trans_date, last_trans_type_id, last_trans_description, last_trans_debit_credit, last_trans_document_ref, last_trans_cover_start_date, last_trans_expiry_date
FROM Insurance_file_system
WHERE insurance_file_cnt = @old_insurance_file_cnt
-- Error trapping
SELECT @error = @error + @@error

-- Extras, Fees & Discounts
DECLARE c_fees CURSOR FORWARD_ONLY FOR
SELECT
    	party_cnt,
    	fee_percentage,
    	fee_amount,
    	commission_percentage,
    	commission_amount,
    	isIPTable,
    	reference,
    	extra_scheme_id,
    	base_currency_id,
    	base_fee_amount,
    	base_fee_commission_value,
    	insurer_fee_type,
    	tax_amount,
    	total_fee,
    	commission_tax_amount,
    	total_commission,
    	FSA_Type_Of_Sale_Id,
    	policy_fee_id
FROM policy_fee
WHERE insurance_file_cnt = @old_insurance_file_cnt
OPEN c_fees
FETCH NEXT FROM c_fees INTO
 @fees_party_cnt,
        @fees_fee_percentage,
        @fees_fee_amount,
        @fees_commission_percentage,
        @fees_commission_amount,
        @fees_isIPTable,
 	@fees_reference,
        @fees_extra_scheme_id,
 	@fees_base_currency_id,
 	@fees_base_fee_amount,
 	@fees_base_fee_commission_value,
 	@fees_insurer_fee_type,
 	@fees_tax_amount,
 	@fees_total_fee,
 	@fees_commission_tax_amount,
 	@fees_total_commission,
 	@fees_FSA_Type_Of_Sale_Id,
 	@fees_policy_fee_id
WHILE @@FETCH_STATUS = 0
BEGIN
 INSERT INTO Policy_fee
        (
        insurance_file_cnt,
        party_cnt,
        fee_percentage,
        fee_amount,
        commission_percentage,
        commission_amount,
        isIPTable,
 	reference,
        extra_scheme_id,
 	base_currency_id,
 	base_fee_amount,
 	base_fee_commission_value,
 	insurer_fee_type,
 	tax_amount,
 	total_fee,
 	commission_tax_amount,
 	total_commission,
 	FSA_Type_Of_Sale_Id
        )
    VALUES
        (
        @new_insurance_file_cnt,
        @fees_party_cnt,
        @fees_fee_percentage,
        @fees_fee_amount,
        @fees_commission_percentage,
        @fees_commission_amount,
        @fees_isIPTable,
 	@fees_reference,
        @fees_extra_scheme_id,
 	@fees_base_currency_id,
 	@fees_base_fee_amount,
 	@fees_base_fee_commission_value,
 	@fees_insurer_fee_type,
 	@fees_tax_amount,
 	@fees_total_fee,
 	@fees_commission_tax_amount,
 	@fees_total_commission,
 	@fees_FSA_Type_Of_Sale_Id
        )

SELECT @new_policy_fee_id = @@IDENTITY

 INSERT INTO tax_calculation
        (
        insurance_file_cnt,
        policy_fee_id,
        tax_group_id,
        percentage,
        value,
	transtype ,
	tax_band_id,
	is_value,
	is_manually_changed,
	allow_tax_credit,
	calc_basis
        )
    SELECT
        @new_insurance_file_cnt,
        @new_policy_fee_id,
        tax_group_id,
        percentage,
	value,
	transtype,
	tax_band_id,
	is_value,
	is_manually_changed,
	allow_tax_credit,
	calc_basis
    FROM tax_calculation
    WHERE insurance_file_cnt =  @old_insurance_file_cnt
    AND policy_fee_id = @fees_policy_fee_id

 SELECT @error = @error + @@error

 FETCH NEXT FROM c_fees INTO
  	@fees_party_cnt,
        @fees_fee_percentage,
        @fees_fee_amount,
        @fees_commission_percentage,
        @fees_commission_amount,
        @fees_isIPTable,
  	@fees_reference,
        @fees_extra_scheme_id,
  	@fees_base_currency_id,
  	@fees_base_fee_amount,
  	@fees_base_fee_commission_value,
  	@fees_insurer_fee_type,
  	@fees_tax_amount,
  	@fees_total_fee,
  	@fees_commission_tax_amount,
  	@fees_total_commission,
  	@fees_FSA_Type_Of_Sale_Id,
  	@fees_policy_fee_id

END
CLOSE c_fees
DEALLOCATE c_fees


-- Agents
DECLARE c_agents CURSOR FORWARD_ONLY FOR
SELECT
 	agent_cnt,
 	agent_count,
 	agent_commission_percentage,
 	agent_commission_amount,
 	agent_commission_value,
 	is_minimum_brokerage ,
 	override_rate_table,
 	apply_perc_to_prem_or_comm,
 	base_currency_id,
 	base_agent_commission_amount,
 	base_agent_commission_value,
 	tax_amount,
 	policy_agents_id
FROM policy_agents
WHERE insurance_file_cnt = @old_insurance_file_cnt
OPEN c_agents
FETCH NEXT FROM c_agents INTO
 	@agents_agent_cnt,
 	@agents_agent_count,
  	@agents_agent_commission_percentage,
 	@agents_agent_commission_amount,
 	@agents_agent_commission_value,
 	@agents_is_minimum_brokerage,
 	@agents_override_rate_table,
 	@agents_apply_perc_to_prem_or_comm,
 	@agents_base_currency_id,
 	@agents_base_agent_commission_amount,
 	@agents_base_agent_commission_value,
 	@agents_tax_amount,
 	@agents_policy_agents_id
WHILE @@FETCH_STATUS = 0
BEGIN
 INSERT INTO Policy_agents
        (
        insurance_file_cnt,
 	agent_cnt,
 	agent_count,
 	agent_commission_percentage,
 	agent_commission_amount,
 	agent_commission_value,
 	is_minimum_brokerage ,
 	override_rate_table,
 	apply_perc_to_prem_or_comm,
 	base_currency_id,
 	base_agent_commission_amount,
 	base_agent_commission_value,
 	tax_amount
        )
    VALUES
        (
        @new_insurance_file_cnt,
 	@agents_agent_cnt,
 	@agents_agent_count,
 	@agents_agent_commission_percentage,
 	@agents_agent_commission_amount,
 	@agents_agent_commission_value,
 	@agents_is_minimum_brokerage,
 	@agents_override_rate_table,
 	@agents_apply_perc_to_prem_or_comm,
 	@agents_base_currency_id,
 	@agents_base_agent_commission_amount,
 	@agents_base_agent_commission_value,
 	@agents_tax_amount
        )

SELECT @new_policy_agents_id = @@IDENTITY

 INSERT INTO tax_calculation
        (
        insurance_file_cnt,
        policy_agents_id,
        tax_group_id,
	tax_band_id,
        percentage,
        value,
  	is_value,
	is_manually_changed,
	transtype,
	allow_tax_credit,
	calc_basis
        )
    SELECT
        @new_insurance_file_cnt,
        @new_policy_agents_id,
        tax_group_id,
	tax_band_id,
        percentage,
	value,
	is_value,
	is_manually_changed,
	transtype,
	allow_tax_credit,
	calc_basis
    FROM tax_calculation
    WHERE insurance_file_cnt =  @old_insurance_file_cnt
    AND policy_fee_id = @agents_policy_agents_id

 SELECT @error = @error + @@error

 FETCH NEXT FROM c_agents INTO
 	@agents_agent_cnt,
 	@agents_agent_count,
 	@agents_agent_commission_percentage,
 	@agents_agent_commission_amount,
 	@agents_agent_commission_value,
 	@agents_is_minimum_brokerage,
 	@agents_override_rate_table,
 	@agents_apply_perc_to_prem_or_comm,
 	@agents_base_currency_id,
 	@agents_base_agent_commission_amount,
 	@agents_base_agent_commission_value,
 	@agents_tax_amount,
 	@agents_policy_agents_id
END
CLOSE c_agents
DEALLOCATE c_agents

--sections
DECLARE c_COB_sections CURSOR FORWARD_ONLY FOR
SELECT
        insurance_section_id,
        COB_rating_section_id,
        premium_excluding_tax,
        tax_applied,
        premium_including_tax,
 	tax_group_id,
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
 	base_commission_payable

FROM insurance_COB_section
WHERE insurance_file_cnt = @old_insurance_file_cnt
OPEN c_COB_sections
FETCH NEXT FROM c_COB_sections INTO
        @sections_insurance_section_id,
        @sections_COB_rating_section_id,
        @sections_premium_excluding_tax,
        @sections_tax_applied,
        @sections_premium_including_tax,
 	@sections_tax_group_id,
        @sections_commission_cnt,
 	@sections_commission_percentage,
 	@sections_commission_charge,
 	@sections_commission_net,
 	@sections_commission_tax_applied,
 	@sections_commission_payable,
 	@sections_commission_tax_group_id,
 	@sections_is_minimum_brokerage,
 	@sections_override_rate_table,
 	@sections_base_premium_excluding_tax,
 	@sections_base_tax_applied,
 	@sections_base_premium_including_tax,
 	@sections_base_commission_charge,
 	@sections_base_commission_net,
 	@sections_base_commission_tax_applied,
 	@sections_base_commission_payable

WHILE @@FETCH_STATUS = 0
BEGIN
 INSERT INTO insurance_COB_section
        (
 	insurance_file_cnt,
        COB_rating_section_id,
        premium_excluding_tax,
        tax_applied,
        premium_including_tax,
 	tax_group_id,
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
 	base_commission_payable
        )
    VALUES
        (
        @new_insurance_file_cnt,
        @sections_COB_rating_section_id,
        @sections_premium_excluding_tax,
        @sections_tax_applied,
        @sections_premium_including_tax,
 	@sections_tax_group_id,
        @sections_commission_cnt,
 	@sections_commission_percentage,
 	@sections_commission_charge,
 	@sections_commission_net,
 	@sections_commission_tax_applied,
 	@sections_commission_payable,
 	@sections_commission_tax_group_id,
 	@sections_is_minimum_brokerage,
 	@sections_override_rate_table,
 	@sections_base_premium_excluding_tax,
 	@sections_base_tax_applied,
 	@sections_base_premium_including_tax,
 	@sections_base_commission_charge,
 	@sections_base_commission_net,
 	@sections_base_commission_tax_applied,
 	@sections_base_commission_payable
        )

SELECT @new_insurance_section_id = @@IDENTITY

 INSERT INTO tax_calculation
        (
        insurance_file_cnt,
        insurance_section_id,
 	percentage,
 	value,
        tax_group_id,
        transtype,
        is_commission_tax,
	tax_band_id,
	is_value,
	is_manually_changed,
	allow_tax_credit,
	calc_basis
        )
    SELECT
        @new_insurance_file_cnt,
        @new_insurance_section_id,
 	percentage,
 	value,
        tax_group_id,
        transtype,
        is_commission_tax,
	tax_band_id,
	is_value,
	is_manually_changed,
	allow_tax_credit,
	calc_basis
    FROM tax_calculation
    WHERE insurance_file_cnt =  @old_insurance_file_cnt
    AND insurance_section_id = @sections_insurance_section_id

 SELECT @error = @error + @@error

 FETCH NEXT FROM c_COB_sections INTO
        @sections_insurance_section_id,
        @sections_COB_rating_section_id,
        @sections_premium_excluding_tax,
        @sections_tax_applied,
        @sections_premium_including_tax,
  	@sections_tax_group_id,
        @sections_commission_cnt,
  	@sections_commission_percentage,
  	@sections_commission_charge,
  	@sections_commission_net,
  	@sections_commission_tax_applied,
  	@sections_commission_payable,
  	@sections_commission_tax_group_id,
  	@sections_is_minimum_brokerage,
  	@sections_override_rate_table,
  	@sections_base_premium_excluding_tax,
  	@sections_base_tax_applied,
  	@sections_base_premium_including_tax,
  	@sections_base_commission_charge,
  	@sections_base_commission_net,
  	@sections_base_commission_tax_applied,
  	@sections_base_commission_payable
END
CLOSE c_COB_sections
DEALLOCATE c_COB_sections
-- text_file
If @CopyTextFile = 1
Begin
    PRINT 'text_file'
    INSERT INTO text_file
    ( entity_type_id, entity_cnt, slot_number, file_number )
    SELECT entity_type_id, @new_insurance_file_cnt, slot_number, file_number
    FROM text_file
    WHERE entity_cnt = @old_insurance_file_cnt
    AND entity_type_id = (SELECT entity_type_id FROM entity_type WHERE code = 'POLICY')
    -- Error trapping
    SELECT @error = @error + @@error
End

-- Did it all work?
IF (@error = 0)
BEGIN
    COMMIT TRANSACTION
END
ELSE
BEGIN
    ROLLBACK TRANSACTION
    -- Raise an error
    RAISERROR ('Failed', 1, 2) WITH SETERROR
END

-- Finished!

END
GO
