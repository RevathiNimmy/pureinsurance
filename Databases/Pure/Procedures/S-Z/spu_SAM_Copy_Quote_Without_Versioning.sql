SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_Copy_Quote_Without_Versioning'
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[spu_SAM_Copy_Quote_Without_Versioning]  		
	@insurance_file_cnt			INT, 
	@insurance_folder_cnt		INT,  	
	 	
	@new_insurance_ref			VARCHAR(255),
	@new_insurance_file_cnt		INT	OUTPUT,
	@new_insurance_folder_cnt	INT	OUTPUT,
	@userID INT  
AS  
DECLARE @new_insurance_file_id INT


	DECLARE @sSQL AS NVARCHAR(MAX)
	
	--Create Insurance Folder
	SET @sSQL = 'INSERT INTO insurance_folder(insurance_folder_id, source_id, insurance_holder_cnt, code, description, inception_date, arc_archive_folder_id, quote_insurance_ref, next_insurance_ref, last_insurance_ref, renewal_count, renewal_NCD_Year, renewal_NCD_contents, last_edi_message_count_received, last_edi_message_count_sent) ' + 
				' SELECT ifd.insurance_folder_id, ifd.source_id, ifd.insurance_holder_cnt, ''' + CAST(@new_insurance_ref AS VARCHAR) + ''', ifd.description, ifd.inception_date, ifd.arc_archive_folder_id,''' + CAST(@new_insurance_ref AS VARCHAR) + ''', ifd.next_insurance_ref, ifd.last_insurance_ref, ifd.renewal_count, ifd.renewal_NCD_Year, ifd.renewal_NCD_contents, ' + 
				' ifd.last_edi_message_count_received, ifd.last_edi_message_count_sent FROM insurance_folder ifd ' + 
				' WHERE ifd.insurance_folder_cnt = ' + CAST(@insurance_folder_cnt AS VARCHAR)
	
	EXEC SP_ExecuteSQL @sSQL
	SELECT @new_insurance_folder_cnt = @@IDENTITY
	
	--Create Insurance ID
	SELECT @new_insurance_file_id = 1

	--Create Insurance File
	 SET @sSQL = 'INSERT INTO insurance_file(insurance_file_structure_id, insurance_file_type_id, insurance_file_status_id, insurance_file_id, source_id, insurance_folder_cnt, insurance_ref, product_id, lead_insurer_cnt, lead_agent_cnt, lead_agent_percent, account_handler_cnt, insured_cnt, business_type_id, collect_type_id, collection_from_cnt, branch_id, currency_id, language_id, date_issued, cover_start_date, expiry_date, renewal_date, renewal_method_id, renewal_frequency_id, is_referred_at_renewal, lapsed_reason_id, lapsed_date, lapsed_description, is_referred_on_mta, policy_version, gemini_policy_status, gemini_business_type, deferred_ind, policy_ignore, broker_cnt, risk_code_id, Analysis_code_id, proposal_date, diary_date, review_date, renewal_day_number, Policy_type_id, indicator, clause, cover, area, long_term_undertaking_date, renewal_stop_code_id, vbs_type, vbs_status, is_insurer_rate_table, is_related_policies, is_retained_documents, schemes_postcode, paid_direct, scheme, brokerage_amount, is_minimum_brokerage_flag, annual_premium, this_premium, net_premium, commission_amount, iptable_amount, ipt_percentage, is_ipt_overridden, tax_amount, vatable_amount, vat_percentage, vat_amount, payment_method, user_defined_data_id, commission_percentage, invariant_key, insured_name, alternate_reference, is_client_invoiced, old_policy_number, quote_expiry_date, alternate_account_cnt, loyalty_scheme_flag, account_executive_cnt, anniversary_date, policy_style_id, underwriting_year_id, policy_status_id, edi_message_sent, return_premium_currency_id, exchange_rate_override_reason_id, base_currency_id, currency_base_xrate, agent_account_currency_id, agent_account_base_xrate, system_base_xrate, currency_base_date, account_base_date, system_base_date, inception_date_tpi, fsa_customer_category_id, fsa_contract_location_id, fsa_underwriter_cnt, fsa_type_of_sale_id, fsa_renewal_consent, cashlistitem_id, cashlistitem_valid, discount_reason_id, discounted_premium, discount_percentage, match_discounted_premium_flag, Country_id, put_on_next_instalment_renewal, anniversary_copy, discount_recurring_type_id, lead_allow_consolidated_commission, sub_allow_consolidated_commission, posting_period_id, addon_created, balance_type, intermediary_agent_account_id, terms_agreed, terms_agreed_date, inception_Date, policy_documents_issued_date, policy_documents_correct, error_notification_date, Base_Insurance_File_Cnt, risk_transfer_agreement, Policy_Deductibles_id, Policy_limits_id, renewal_premium, renewal_product_id, original_product_id, risk_transfer_editable, marked_for_collection, marked_date, number_of_fleet_vehicles,base_insurance_folder_cnt, DOPaymentTerms_id, CollectionFrequency_id, manual_discount_percentage, coins_placement) ' + 
	'SELECT ifi.insurance_file_structure_id, 1, NULL, ' + CAST(@new_insurance_file_id AS VARCHAR) + ', ifi.source_id, ' + CAST(@new_insurance_folder_cnt AS VARCHAR) + ',''' + CAST(@new_insurance_ref AS VARCHAR) + ''', ifi.product_id, ifi.lead_insurer_cnt, ifi.lead_agent_cnt, ifi.lead_agent_percent, ifi.account_handler_cnt, ifi.insured_cnt, ifi.business_type_id, ifi.collect_type_id, ifi.collection_from_cnt, ifi.branch_id, ifi.currency_id, ifi.language_id,'''+ CONVERT (varchar(19), Getdate()) +''', ifi.cover_start_date, ifi.expiry_date, ifi.renewal_date, ifi.renewal_method_id, ifi.renewal_frequency_id, ifi.is_referred_at_renewal, ifi.lapsed_reason_id, ifi.lapsed_date, ifi.lapsed_description, ifi.is_referred_on_mta, 0, ifi.gemini_policy_status, ifi.gemini_business_type, ifi.deferred_ind, ifi.policy_ignore, ifi.broker_cnt, ifi.risk_code_id, ifi.Analysis_code_id, ifi.proposal_date, ifi.diary_date, ifi.review_date, ifi.renewal_day_number, ifi.Policy_type_id, ifi.indicator, ifi.clause, ifi.cover, ifi.area, ifi.long_term_undertaking_date, ifi.renewal_stop_code_id, ifi.vbs_type, ifi.vbs_status, ifi.is_insurer_rate_table, ifi.is_related_policies, ifi.is_retained_documents, ifi.schemes_postcode, ifi.paid_direct, ifi.scheme, ifi.brokerage_amount, ifi.is_minimum_brokerage_flag, ifi.annual_premium, ifi.this_premium, ifi.net_premium, ifi.commission_amount, ifi.iptable_amount, ifi.ipt_percentage, ifi.is_ipt_overridden, ifi.tax_amount, ifi.vatable_amount, ifi.vat_percentage, ifi.vat_amount, ifi.payment_method, ifi.user_defined_data_id, ifi.commission_percentage, ifi.invariant_key, ifi.insured_name, ifi.alternate_reference, ifi.is_client_invoiced, ifi.old_policy_number, ifi.quote_expiry_date, ifi.alternate_account_cnt, ifi.loyalty_scheme_flag, ifi.account_executive_cnt, ifi.anniversary_date, ifi.policy_style_id, ifi.underwriting_year_id, ifi.policy_status_id, ifi.edi_message_sent, ifi.return_premium_currency_id, ifi.exchange_rate_override_reason_id, ifi.base_currency_id, ifi.currency_base_xrate, ifi.agent_account_currency_id, ifi.agent_account_base_xrate, ifi.system_base_xrate, ifi.currency_base_date, ifi.account_base_date, '''+ CONVERT (varchar(19), Getdate()) +''', ifi.inception_date_tpi, ifi.fsa_customer_category_id, ifi.fsa_contract_location_id, ifi.fsa_underwriter_cnt, ifi.fsa_type_of_sale_id, ifi.fsa_renewal_consent, ifi.cashlistitem_id, ifi.cashlistitem_valid, ifi.discount_reason_id, ifi.discounted_premium, ifi.discount_percentage, ifi.match_discounted_premium_flag, ifi.Country_id, ifi.put_on_next_instalment_renewal, ifi.anniversary_copy, ifi.discount_recurring_type_id, ifi.lead_allow_consolidated_commission, ifi.sub_allow_consolidated_commission, ifi.posting_period_id, ifi.addon_created, ifi.balance_type, ifi.intermediary_agent_account_id, ifi.terms_agreed, ifi.terms_agreed_date, ifi.inception_Date, ifi.policy_documents_issued_date, ifi.policy_documents_correct, ifi.error_notification_date, NULL, ifi.risk_transfer_agreement, ifi.Policy_Deductibles_id, ifi.Policy_limits_id, ifi.renewal_premium, ifi.renewal_product_id, ifi.original_product_id, ifi.risk_transfer_editable, ifi.marked_for_collection, ifi.marked_date, ifi.number_of_fleet_vehicles ,' + CAST(@new_insurance_folder_cnt AS VARCHAR) + ', ifi.DOPaymentTerms_id, ifi.CollectionFrequency_id, ifi.manual_discount_percentage, ifi.coins_placement  FROM insurance_file ifi WHERE ifi.insurance_file_cnt = ' + CAST(@insurance_file_cnt AS VARCHAR)
	
	
	EXEC SP_ExecuteSQL @sSQL	
	SELECT @new_insurance_file_cnt = @@IDENTITY

	SET @sSQL = 'INSERT INTO insurance_file_risk_link (insurance_file_cnt, risk_cnt, status_flag, original_risk_cnt) SELECT  ' + CAST(@new_insurance_file_cnt AS VARCHAR) + ', risk_cnt, status_flag = CASE WHEN status_flag=''D'' THEN ''D'' ELSE ''U'' END, NULL FROM insurance_file_risk_link WHERE insurance_file_cnt = ' + CAST(@insurance_file_cnt AS VARCHAR)
	EXEC SP_ExecuteSQL @sSQL

	SET @sSQL = 'INSERT INTO insurance_file_system SELECT ' + CAST(@new_insurance_file_cnt AS VARCHAR) + ', ifs.endorsement_count, ' + CAST(@userID AS VARCHAR) + ',''' + CAST(getdate() AS VARCHAR) + ''',' + CAST(@userID AS VARCHAR) + ', ifs.last_modified, ifs.last_trans_date, ifs.last_trans_type_id, ifs.last_trans_description, ifs.last_trans_debit_credit, ifs.last_trans_document_ref, ifs.last_trans_cover_start_date, ifs.last_trans_expiry_date FROM insurance_file_system ifs WHERE ifs.insurance_file_cnt = ' + CAST(@insurance_file_cnt AS VARCHAR)  
	EXEC SP_ExecuteSQL @sSQL

	--SET @sSQL = 'INSERT INTO Tax_Calculation SELECT risk_cnt,tax_band_id,premium,percentage,value,is_value,is_manually_changed,Calc_Basis,Basis_Value,Sum_Insured,Sum_Insured_Rounded,currency_id,allow_tax_credit,original_sum_insured,country_id,state_id,class_of_business_id,tax_group_id,sequence,' + CAST(@new_insurance_file_cnt AS VARCHAR) + ',transtype,policy_fee_u_id,agent_commission_cnt,ri_party_cnt,ri_arrangement_line_id,insurance_section_id,policy_fee_id,policy_agents_id,insurer_party_cnt,claim_peril_id,claim_payment_id,claim_receipt_id,claim_payment_item_id,claim_receipt_item_id,is_not_applied_to_client,include_tax_in_instalments,spread_tax_across_instalments,base_tax_calculation_cnt,version_id,pfprem_finance_cnt,pfprem_finance_version,policy_coinsurers_section_id,is_commission_tax,apply_tax_by,tax_band_rate_id,is_suspended FROM Tax_Calculation tc WHERE tc.insurance_file_cnt = ' + CAST(@insurance_file_cnt AS VARCHAR)  
	--EXEC SP_ExecuteSQL @sSQL


	EXEC spu_copy_policy_agents @insurance_file_cnt, @new_insurance_file_cnt     
	EXEC spu_copy_coinsurance @insurance_file_cnt, @new_insurance_file_cnt
	EXEC spu_copy_sub_agent @insurance_file_cnt, @new_insurance_file_cnt
	EXEC spu_copy_policy_standard_wordings @insurance_file_cnt, @new_insurance_file_cnt
    EXEC spu_sir_copy_insurance_file_associates @insurance_file_cnt, @new_insurance_file_cnt
	SET @sSQL = 'INSERT INTO Renewal_Status(product_id, renewal_status_type_id, insurance_holder_cnt, insurance_file_cnt, lead_agent_cnt, created_by_id, date_created, critical_date, is_invite_printed, renewal_insurance_file_cnt, broker_xfer_status_type_id, date_invite_printed, renewal_exception_reason_id, renewal_exception_notes, email_sent, email_sent_date) ' +
				'SELECT product_id, renewal_status_type_id, insurance_holder_cnt, insurance_file_cnt, lead_agent_cnt,' + CAST(@userID AS VARCHAR) + ',''' + CAST(getdate() AS VARCHAR) + ''', critical_date, is_invite_printed, ' + CAST(@new_insurance_file_cnt AS VARCHAR) + ', broker_xfer_status_type_id, date_invite_printed, renewal_exception_reason_id, renewal_exception_notes, email_sent, email_sent_date FROM Renewal_Status rs WHERE rs.renewal_insurance_file_cnt = ' + CAST(@insurance_file_cnt AS VARCHAR)
	EXEC SP_ExecuteSQL @sSQL


	INSERT INTO event_log  
	(  
	    party_cnt,  
	    insurance_folder_cnt,  
	    insurance_file_cnt,  
	    claim_cnt,  
	    document_cnt,  
	    old_address_cnt,  
	    new_address_cnt,  
	    campaign_id,  
	    document_type_id,  
	    report_type_id,  
	    event_type_id,  
	    user_id,  
	    event_date,  
	    description,  
	    old_party_type_id,  
	    event_log_subject_id,  
	    account_key,  
	    fsa_complaint_folder_cnt,  
	    short_description,  
	    priority_code,  
	    is_completed,  
	    peril_id,  
	    case_id,  
	    rtf_text,  
	    is_manual_description,  
	    batch_id,  
	    bg_id  
	)
	SELECT ifi.insured_cnt, ifi.insurance_folder_cnt, ifi.insurance_file_cnt, NULL, NULL, NULL, NULL,
		NULL, NULL, NULL, 5,ISNULL(@UserId,ifs.created_by_id) , getdate(), 'Quote Copied From ' + (SELECT Insurance_Ref FROM Insurance_File WHERE insurance_file_cnt = @insurance_file_cnt) + ' To ' + @new_insurance_ref , NULL, NULL, NULL, NULL,
		NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
	FROM insurance_file ifi 
		INNER JOIN insurance_file_system ifs ON ifi.insurance_file_cnt = ifs.insurance_file_cnt
	WHERE ifi.insurance_file_cnt = @new_insurance_file_cnt

GO
