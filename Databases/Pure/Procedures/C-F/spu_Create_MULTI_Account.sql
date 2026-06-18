SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Create_MULTI_Account'
GO

/*For multi ledger systems add MULTI account for the source specified if it doesn't already exist*/
/*For single ledger systems add MULTI account for branch 1 if it doesn't already exist*/
CREATE PROCEDURE spu_Create_MULTI_Account
    @source_id INT
AS
BEGIN

DECLARE @address_cnt INT
DECLARE @party_cnt INT
DECLARE @account_id INT
DECLARE @ledger_id INT
DECLARE @element_id INT
DECLARE @node_id INT
DECLARE @parent_node_id INT
DECLARE @current_date DATETIME
DECLARE @MULTIname VARCHAR(10)

/*Set MULTI name*/
IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 16 AND value = 1)
BEGIN
	SELECT @MULTIname = 'MULTI' + CAST(@source_id AS VARCHAR(2))
END
ELSE
BEGIN
	SELECT @MULTIname = 'MULTI'
	SELECT @source_id = 1
	
	/*IF MULTI already exists but not as an insurer then call it MULTI1*/
	IF EXISTS(SELECT NULL FROM party WHERE shortname = @MULTIname AND party_type_id <> 7)
	BEGIN
		SELECT @MULTIname = 'MULTI1'
	END
END

SELECT @current_date = getdate()

/*Check if MULTI already exists*/
IF NOT EXISTS(SELECT NULL FROM party WHERE shortname = @MULTIname)
BEGIN

	/*Check if there is a ZZMULTI - if there is, don't bother creating another MULTI*/
	IF NOT EXISTS(SELECT NULL FROM party WHERE shortname LIKE 'z%multi' OR shortname LIKE 'multi%')
	BEGIN
	
		/*Add party*/
		EXEC spe_Party_add 
			@party_cnt = @party_cnt OUTPUT,
			@party_type_id = 7,
			@is_also_agent = 0,
			@party_structure_id = 1,
			@source_id = @source_id,
			@party_id = NULL,
			@shortname = @MULTIname,
			@name = @MULTIname,
			@resolved_name = @MULTIname,
			@currency_id = 26,
			@language_id = 1,
			@collect_type_id = NULL,
			@accum_treatment_type_id = NULL,
			@stats_treatment_type_id = NULL,
			@party_category_id = NULL,
			@agent_cnt = NULL,
			@consultant_cnt = NULL,
			@created_by_id = 1,
			@date_created = @current_date,
			@last_modified = @current_date,
			@modified_by_id = 1,
			@payment_method_code = 'Cash',
			@payment_term_code = '',
			@credit_card_code = '',
			@file_code = '',
			@abc_count = NULL,
			@statements = 0,
			@reminder_type_id = NULL,
			@renewals = 0,
			@status = '',
			@last_action_type = '',
			@is_travel_agent = 0,
			@is_prospect = 0,
			@is_deleted = 0,
			@abi_code_on_406 = '',
			@abi_code_on_81 = '999',
			@abi_codelist = '',
			@area_id = NULL,
			@service_level_id = 0,
			@invariant_key = 0,
			@record_status = '',
			@CCJs = 0,
			@user_defined_data_id = NULL,
			@seasonal_gift_id = NULL,
			@correspondence_type_id = NULL,
			@renewal_stop_code_id = NULL,
			@swift_party_id = NULL,
			@loyalty_number = NULL,
			@alternative_identifier = NULL,
			@marketing_segment_ind = NULL,
			@trading_name = NULL,
			@sub_branch_id = NULL,
	                @tob_letter = NULL
	
		/*Add party_insurer*/
		EXEC spe_party_insurer_add 
			@party_cnt = @party_cnt,
			@agency_number = '99999',
			@binder_indicator = 0,
			@report_indicator = 0,
			@is_reinsurer = 0,
			@reinsurance_type = 0,
			@is_reinsurance_debit_credit_n = 0,
			@default_comm_rate = 0,
			@tax_group_id = NULL,
			@tax_registration_number = NULL,
			@tax_code = NULL,
			@payment_method = NULL,
			@payment_frequency = NULL,
			@bank_account = NULL,
			@fsa_insurerstatus_id = 0,
			@fsa_registration_number = NULL,
			@fsa_insurercreditrating_id = 0,
			@is_retained = 0,
			@is_domiciled_for_tax = 0,
			@risk_transfer_agreement = 0
	
		/*Add dummy address*/
		EXEC spe_Address_add
			@address_cnt = @address_cnt OUTPUT,
			@source_id = @source_id,
			@address_id = NULL,
			@address1 = 'Internal Use Only',
			@address2 = '',
			@address3 = '',
			@address4 = '',
			@postal_code = 'X9 9XX',
			@country_id = 1,
			@created_by_id = 1,
			@date_created = @current_date,
			@modified_by_id = 1,
			@last_modified = @current_date
	
		/*Add link between dummy address and party*/
		EXEC spe_Party_Address_Usage_add
			@party_cnt = @party_cnt,
			@address_cnt = @address_cnt,
			@description = NULL,
			@address_usage_type_id = 4
	
		/*Get ledger_id*/
		SELECT @ledger_id = (SELECT MIN(ledger_id) FROM ledger WHERE company_id = @source_id AND ledger_short_name = 'IN')
	
		/*Add account*/
		EXEC spu_ACT_add_Account 
			@account_id = @account_id OUTPUT,
			@company_id = @source_id,
			@purgefrequency_id = 1,
			@accounttype_id = 4,
			@paymenttype_id = 1,
			@currency_id = 26,
			@ledger_id = @ledger_id,
			@account_name = @MULTIname,
			@short_code = @MULTIname,
			@restrict_enquiry = 0,
			@restrict_update = 0,
			@delete_at_purge = 0,
			@contact_name = '',
			@address1 = 'Internal Use Only',
			@address2 = '',
			@address3 = '',
			@address4 = '',
			@postal_code = 'X9 9XX',
			@address_country = 1,
			@phone_area_code = '',
			@phone_number = '',
			@phone_extension = '',
			@fax_area_code = '',
			@fax_number = '',
			@fax_extension = '',
			@payment_name = '',
			@payment_account_code = '',
			@payment_branch_code = '',
			@payment_expiry_date = @current_date,
			@payment_reference1 = '',
			@payment_reference2 = '',
			@prooflist_report_id = NULL,
			@bordereau_report_id = NULL,
			@credit_limit = 0,
			@discount_percentage = 0,
			@settlement_period = 0,
			@bank_name = '',
			@bank_address1 = '',
			@bank_address2 = '',
			@bank_address3 = '',
			@bank_address4 = '',
			@bank_postal_code = '',
			@bank_country = NULL,
			@bank_phone_area_code = '',
			@bank_phone_number = '',
			@bank_phone_extension = '',
			@bank_fax_area_code = '',
			@bank_fax_number = '',
			@bank_fax_extension = '',
			@comments = '',
			@account_key = @party_cnt,
			@nominal_account_id = 0,
			@accountstatus_id = 1,
			@sub_branch_id = NULL,
			@allow_electronic_payment = 0,
			@client_money_calc_account_type = 0,
	    		@client_bank_account_type = NULL
	
		/*Add element*/
		EXEC spu_ACT_add_Element
			@element_id = @element_id OUTPUT,
			@element_name = @MULTIname,
			@parent_id = NULL
	
		/*Add elementextras*/
		EXEC spe_ElementExtras_add
			@element_id = @element_id,
			@totalling_id = NULL,
			@description = NULL,
			@report_map_id = NULL,
			@account_map_id = NULL,
			@is_Deletable = 1
	
		/*Get parent node for structuretree*/
		SELECT @parent_node_id = 
			(
				SELECT node_id
				FROM structuretree
				WHERE mapping_id IN
				(
					SELECT mapping_id
					FROM mapping
					WHERE company_id = @source_id
					AND description = 'Insurer Ledger'
				)
			)
	
		/*Add structuretree*/
		EXEC spu_ACT_add_StructureTree 
			@node_id = @node_id OUTPUT,
			@company_id = 1,
			@mapping_id = NULL,
			@account_id = @account_id,
			@element_id = @element_id,
			@parent_node_id = @parent_node_id
	
	END
END

END

GO