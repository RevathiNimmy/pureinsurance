SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_ReAdd_Stats_Trans'
GO

CREATE PROCEDURE spu_SIR_ReAdd_Stats_Trans
 @document_id INT,  
 @user_id INT,  
 @sign INT,
 @new_doc_ref varchar(255)
AS  

Declare @username varchar(255),
	@docref	varchar(255),
	@folder_cnt INT,
	@new_folder_cnt INT

BEGIN
    SELECT @username = username From PMUser
	Where user_id = @user_id

    SELECT @docref = d.document_ref, @folder_cnt = sf.stats_folder_cnt
	From document d
	INNER JOIN stats_folder sf ON sf.document_ref = d.document_ref
	Where document_id = @document_id

    INSERT INTO stats_folder 
	([product_id], [source_id],[debit_credit],[document_ref]
    ,[document_comment],[document_date],[accounting_date],[posting_period_year]
    ,[posting_period_number],[premium_total] ,[transaction_type_id] ,[transaction_type_code]
    ,[transaction_date],[insurance_file_cnt] ,[insurance_ref],[effective_date],[cover_start_date]
    ,[expiry_date],[insurance_holder_cnt] ,[insurance_holder_shortname],[insurance_holder_name]
    ,[product_code],[business_type_id],[business_type_code],[account_handler_cnt]
    ,[account_handler_shortname],[branch_id],[branch_code],[currency_code]
    ,[agent_cnt],[agent_shortname],[loss_id],[loss_code],[loss_date]
    ,[created_by_user_id],[created_by_username],[stats_version],[underwriting_year_id]
    ,[payment_id],[Receipt_Id])
	SELECT product_id, source_id, debit_credit, @new_doc_ref, document_comment, 
	document_date, accounting_date, posting_period_year, posting_period_number, premium_total * @sign, 
	transaction_type_id, transaction_type_code, transaction_date, insurance_file_cnt, insurance_ref, 
	effective_date, cover_start_date, expiry_date, insurance_holder_cnt, insurance_holder_shortname, 
	insurance_holder_name, product_code, business_type_id, business_type_code, account_handler_cnt, 
	account_handler_shortname, branch_id, branch_code, currency_code, agent_cnt, agent_shortname, loss_id, 
	loss_code, loss_date, @user_id, @username, stats_version, underwriting_year_id, payment_id, receipt_id 
    FROM stats_folder WHERE stats_folder_cnt = @folder_cnt

    SELECT @new_folder_cnt = @@IDENTITY

    INSERT INTO stats_detail
    ([stats_folder_cnt],[stats_detail_id],[stats_detail_type],[risk_id]
    ,[risk_type_id],[risk_type_code],[peril_id],[peril_description]
    ,[peril_type_id],[peril_type_code],[policy_section_type_id],[policy_section_type_code]
    ,[class_of_business_id],[class_of_business_code],[tax_type_id],[tax_type_code]
    ,[tax_value],[ri_party_cnt],[ri_shortname],[ri_party_type],[ri_share_percent]
    ,[ri_agreement_code],[annual_premium],[currency_code],[currency_rate],[this_premium_original]
    ,[this_premium_home],[commission_percent],[lead_commission_value_home],[sub_commission_value_home]
    ,[sum_insured_home],[sum_insured_currency_code],[sum_insured_change],[transaction_ledger_id]
    ,[transaction_account_id],[account_type_code],[ceded_ref],[cover_share_percent]
    ,[sum_insured_total],[charges_total],[taxes_total],[recoveries_total],[commission_excluded]
    ,[withholding_tax_excluded],[purchase_order_no],[purchase_invoice_no],[stats_version]
    ,[this_premium_system],[lead_commission_value_system],[sub_commission_value_system]
    ,[sum_insured_system],[is_commission_modified],[original_flag],[cover_to_date]
    ,[Claim_RI_Only_Amendment],[Earning_Pattern_id])	
	SELECT @new_folder_cnt, stats_detail_id, 
	stats_detail_type, risk_id, risk_type_id, risk_type_code, peril_id, peril_description, 
	peril_type_id, peril_type_code, policy_section_type_id, policy_section_type_code, class_of_business_id, 
	class_of_business_code, tax_type_id, tax_type_code, tax_value * @sign, ri_party_cnt, ri_shortname, 
	ri_party_type, ri_share_percent, ri_agreement_code, annual_premium * @sign, currency_code, currency_rate, 
	this_premium_original * @sign, this_premium_home * @sign, commission_percent, lead_commission_value_home * @sign, 
	sub_commission_value_home * @sign, sum_insured_home * @sign, sum_insured_currency_code, NULL, transaction_ledger_id, 
	transaction_account_id, account_type_code, ceded_ref, cover_share_percent, sum_insured_total * @sign, 
	NULL, NULL, NULL, NULL, NULL, NULL, NULL, stats_version, this_premium_system * @sign, lead_commission_value_system * @sign, 
	sub_commission_value_system * @sign, sum_insured_system * @sign, is_commission_modified, original_flag, cover_to_date, 
	claim_ri_only_amendment, earning_pattern_id 
    FROM stats_detail WHERE stats_folder_cnt = @folder_cnt
  
    SELECT @folder_cnt = transaction_export_folder_cnt
	From transaction_export_folder
	Where document_ref like @docref

    INSERT INTO transaction_export_folder
	([product_id],[transaction_export_folder_id],[source_id],[insurance_file_cnt]
   ,[debit_credit],[document_ref],[document_comment],[document_date]
   ,[is_payable_by_instalments],[accounting_date],[posting_period_year],[posting_period_number]
   ,[premium_total],[transaction_type_id],[transaction_type_code],[insurance_ref]
   ,[effective_date],[cover_start_date],[expiry_date],[insurance_holder_cnt],[insurance_holder_id]
   ,[insurance_holder_shortname],[insurance_holder_account_key],[product_code]
   ,[business_type_id],[business_type_code],[account_handler_cnt],[account_handler_id]
   ,[account_handler_shortname],[account_handler_account_key],[agent_cnt]
   ,[agent_id],[agent_shortname],[agent_account_key],[branch_id]
   ,[branch_code],[currency_code],[loss_id],[loss_code],[loss_date]
   ,[created_by_user_id],[created_by_username],[accounts_export_status]
   ,[reason],[real_insurance_file_cnt],[underwriting_year_id],[base_currency_id]
   ,[terms_of_payment_id],[payment_due_date],[event_log_id])
	SELECT product_id, transaction_export_folder_id, source_id, insurance_file_cnt, debit_credit,
	@new_doc_ref, document_comment, document_date, is_payable_by_instalments, accounting_date, 
	posting_period_year, posting_period_number, premium_total * @sign, transaction_type_id, transaction_type_code,
	insurance_ref, effective_date, cover_start_date, expiry_date, insurance_holder_cnt, insurance_holder_id,
	insurance_holder_shortname, insurance_holder_account_key, product_code, business_type_id, business_type_code,
	account_handler_cnt, account_handler_id, account_handler_shortname, account_handler_account_key, agent_cnt,
	agent_id, agent_shortname, agent_account_key, branch_id, branch_code, currency_code, loss_id, loss_code,
	loss_date, @user_id, @username, accounts_export_status, reason, real_insurance_file_cnt,
	underwriting_year_id, base_currency_id, terms_of_payment_id, payment_due_date, event_log_id
    FROM transaction_export_folder WHERE transaction_export_folder_cnt = @folder_cnt

    SELECT @new_folder_cnt = @@IDENTITY

    INSERT INTO transaction_export_detail
    ([transaction_export_folder_cnt],[transaction_export_detail_id],[transaction_amount]
    ,[transaction_ledger_code] ,[account_type_code],[transaction_account_key]
    ,[ceded_ref],[cover_share_percent],[sum_insured_total],[charges_total]
    ,[taxes_total],[recoveries_total],[commission_excluded],[withholding_tax_excluded]
    ,[mapping_code],[spare],[purchase_order_no],[purchase_invoice_no]
    ,[base_transaction_amount],[base_taxes_amount],[suspended],[release_to_income]
    ,[release_account_code],[transdetail_type_code],[tax_group_id]
    ,[tax_band_id],[manually_released],[released_on_full_settlement]
    ,[released_for_whole_posting],[released_on_policy_effective])
	SELECT @new_folder_cnt, transaction_export_detail_id, transaction_amount * @sign, transaction_ledger_code,
	account_type_code, transaction_account_key, ceded_ref, cover_share_percent, sum_insured_total * @sign,
	charges_total * @sign, taxes_total * @sign, recoveries_total * @sign, commission_excluded * @sign, 
	withholding_tax_excluded * @sign, mapping_code, spare, purchase_order_no, purchase_invoice_no, 
	base_transaction_amount * @sign, base_taxes_amount * @sign, suspended, release_to_income, 
	release_account_code, transdetail_type_code, tax_group_id, tax_band_id, manually_released, 
	released_on_full_settlement, released_for_whole_posting, released_on_policy_effective 
    FROM transaction_export_detail WHERE transaction_export_folder_cnt = @folder_cnt

END  

GO