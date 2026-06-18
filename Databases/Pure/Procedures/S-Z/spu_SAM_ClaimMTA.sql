SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_ClaimMTA'
GO

CREATE PROCEDURE spu_SAM_ClaimMTA
	@OldInsurance_File_Cnt INT,
	@OldRisk_Cnt INT,
	@MTADate DATETIME,
	@NewInsurance_File_Cnt INT OUTPUT,
	@NewRisk_Cnt INT OUTPUT,
	@NewGIS_Policy_Link_ID INT OUTPUT
AS

-- ******************************************************************************************************
-- Stored Procedure spu_SAM_ClaimMTA
-- ******************************************************************************************************
-- Revision             Description of Modification             Date        Who 
-- --------             ---------------------------             ----        ---
-- 1.0                  Created                                	26/07/2006  Ramakant Singh
-- ******************************************************************************************************

BEGIN

SET NOCOUNT ON


DECLARE @NewInsurance_File_ID INT,
	@NewInsurance_File_Type_ID INT,
	@NewPolicy_Version INT,
	@MTA_Transaction_Type_ID INT

SELECT @MTA_Transaction_Type_ID=transaction_type_id FROM Transaction_Type
	WHERE code='MTA'

SELECT @NewInsurance_File_Type_ID=insurance_file_type_id FROM Insurance_File_Type 
	WHERE Insurance_File_Type.code='MTA PERM'

SELECT @NewInsurance_File_Cnt=MAX(insurance_file_cnt)+1 FROM Insurance_File

SELECT @NewInsurance_File_ID=0

SELECT	@NewPolicy_Version=MAX(policy_version) 
FROM Insurance_File 
WHERE insurance_file_cnt=@OldInsurance_File_Cnt


SET IDENTITY_INSERT Insurance_File ON

--Copy Policy Details
INSERT INTO Insurance_File(
	insurance_file_cnt,
	insurance_file_structure_id,
	insurance_file_type_id,
	insurance_file_status_id,
	insurance_file_id,	
	source_id,
	insurance_folder_cnt,
	insurance_ref,
	product_id,
	lead_insurer_cnt,
	lead_agent_cnt,
	lead_agent_percent,
	account_handler_cnt,
	insured_cnt,
	business_type_id,
	collect_type_id,
	collection_from_cnt,
	branch_id,
	currency_id,
	language_id,
	date_issued,
	cover_start_date,
	expiry_date,
	renewal_date,
	renewal_method_id,
	renewal_frequency_id,
	is_referred_at_renewal,
	lapsed_reason_id,
	lapsed_date,
	lapsed_description,
	is_referred_on_mta,
	policy_version,
	gemini_policy_status,
	gemini_business_type,
	deferred_ind,
	policy_ignore,
	broker_cnt,
	risk_code_id,
	Analysis_code_id,
	proposal_date,
	diary_date,
	review_date,
	renewal_day_number,
	Policy_type_id,
	indicator,
	clause,
	cover,
	area,
	long_term_undertaking_date,
	renewal_stop_code_id,
	vbs_type,
	vbs_status,
	is_insurer_rate_table,
	is_related_policies,
	is_retained_documents,
	schemes_postcode,
	paid_direct,
	scheme,
	brokerage_amount,
	is_minimum_brokerage_flag,
	annual_premium,
	this_premium,
	net_premium,
	commission_amount,
	iptable_amount,
	ipt_percentage,
	is_ipt_overridden,
	tax_amount,
	vatable_amount,
	vat_percentage,
	vat_amount,
	payment_method,
	user_defined_data_id,
	commission_percentage,
	invariant_key,
	insured_name,
	alternate_reference,
	is_client_invoiced,
	old_policy_number,
	quote_expiry_date,
	alternate_account_cnt,
	loyalty_scheme_flag,
	account_executive_cnt,
	anniversary_date,
	policy_style_id,
	underwriting_year_id,
	policy_status_id,
	edi_message_sent,
	return_premium_currency_id,
	exchange_rate_override_reason_id,
	base_currency_id,
	currency_base_xrate,
	agent_account_currency_id,
	agent_account_base_xrate,
	system_base_xrate,
	currency_base_date,
	account_base_date,
	system_base_date,
	inception_date_tpi,
	fsa_customer_category_id,
	fsa_contract_location_id,
	fsa_underwriter_cnt,
	fsa_type_of_sale_id,
	fsa_renewal_consent,
	cashlistitem_id,
	cashlistitem_valid,
	discount_reason_id,
	discounted_premium,
	discount_percentage,
	match_discounted_premium_flag,
	Country_id,
	put_on_next_instalment_renewal,
	anniversary_copy,
	discount_recurring_type_id,
	lead_allow_consolidated_commission,
	sub_allow_consolidated_commission)

	SELECT @Newinsurance_file_cnt,
	insurance_file_structure_id,
	@NewInsurance_File_Type_ID,
	insurance_file_status_id,
	@NewInsurance_File_ID,	
	source_id,
	insurance_folder_cnt,
	insurance_ref,
	product_id,
	lead_insurer_cnt,
	lead_agent_cnt,
	lead_agent_percent,
	account_handler_cnt,
	insured_cnt,
	business_type_id,
	collect_type_id,
	collection_from_cnt,
	branch_id,
	currency_id,
	language_id,
	date_issued,
	@MTADate,
	expiry_date,
	renewal_date,
	renewal_method_id,
	renewal_frequency_id,
	is_referred_at_renewal,
	lapsed_reason_id,
	lapsed_date,
	lapsed_description,
	is_referred_on_mta,
	@NewPolicy_Version,
	gemini_policy_status,
	gemini_business_type,
	deferred_ind,
	policy_ignore,
	broker_cnt,
	risk_code_id,
	Analysis_code_id,
	proposal_date,
	diary_date,
	review_date,
	renewal_day_number,
	Policy_type_id,
	indicator,
	clause,
	cover,
	area,
	long_term_undertaking_date,
	renewal_stop_code_id,
	vbs_type,
	vbs_status,
	is_insurer_rate_table,
	is_related_policies,
	is_retained_documents,
	schemes_postcode,
	paid_direct,
	scheme,
	brokerage_amount,
	is_minimum_brokerage_flag,
	annual_premium+this_premium,
	0,
	0,
	commission_amount,
	iptable_amount,
	ipt_percentage,
	is_ipt_overridden,
	0,
	vatable_amount,
	vat_percentage,
	vat_amount,
	payment_method,
	user_defined_data_id,
	commission_percentage,
	invariant_key,
	insured_name,
	alternate_reference,
	is_client_invoiced,
	old_policy_number,
	quote_expiry_date,
	alternate_account_cnt,
	loyalty_scheme_flag,
	account_executive_cnt,
	anniversary_date,
	policy_style_id,
	underwriting_year_id,
	policy_status_id,
	edi_message_sent,
	return_premium_currency_id,
	exchange_rate_override_reason_id,
	base_currency_id,
	currency_base_xrate,
	agent_account_currency_id,
	agent_account_base_xrate,
	system_base_xrate,
	currency_base_date,
	account_base_date,
	system_base_date,
	inception_date_tpi,
	fsa_customer_category_id,
	fsa_contract_location_id,
	fsa_underwriter_cnt,
	fsa_type_of_sale_id,
	fsa_renewal_consent,
	cashlistitem_id,
	cashlistitem_valid,
	discount_reason_id,
	discounted_premium,
	discount_percentage,
	match_discounted_premium_flag,
	Country_id,
	put_on_next_instalment_renewal,
	anniversary_copy,
	discount_recurring_type_id,
	lead_allow_consolidated_commission,
	sub_allow_consolidated_commission FROM Insurance_File 
	WHERE insurance_file_cnt=@OldInsurance_File_Cnt


SET IDENTITY_INSERT Insurance_File OFF


--For Debug purpose
--SELECT * FROM insurance_file WHERE insurance_file_cnt=@NewInsurance_File_Cnt



--Copy Policy Details to Insurance_file_system
INSERT INTO Insurance_File_System(insurance_file_cnt,
	endorsement_count,
	created_by_id,
	date_created,
	modified_by_id,
	last_modified,
	last_trans_date,
	last_trans_type_id,
	last_trans_description,
	last_trans_debit_credit,
	last_trans_document_ref,
	last_trans_cover_start_date,
	last_trans_expiry_date)

	SELECT @Newinsurance_file_cnt,
	endorsement_count,
	created_by_id,
	date_created,
	modified_by_id,
	last_modified,
	last_trans_date,
	@MTA_Transaction_Type_ID,
	last_trans_description,
	last_trans_debit_credit,
	last_trans_document_ref,
	last_trans_cover_start_date,
	last_trans_expiry_date FROM Insurance_File_System 
	WHERE insurance_file_cnt=@OldInsurance_File_Cnt



--For Debug purpose
--SELECT * FROM insurance_file_system WHERE insurance_file_cnt=@NewInsurance_File_Cnt







EXEC spu_copy_coinsurance @OldInsuranceFileCnt=@OldInsurance_File_Cnt,  
    	@NewInsuranceFileCnt=@NewInsurance_File_Cnt

--For Debug purpose
--SELECT * FROM coi_arrangement WHERE insurance_file_cnt=@NewInsurance_File_Cnt

EXEC spu_copy_sub_agent @OldInsuranceFileCnt=@OldInsurance_File_Cnt,  
    	@NewInsuranceFileCnt=@NewInsurance_File_Cnt

--For Debug purpose
--SELECT * FROM insurance_file_agent WHERE insurance_file_cnt=@NewInsurance_File_Cnt


EXEC spu_copy_policy_standard_wordings @old_insurance_file_cnt=@OldInsurance_File_Cnt,  
    	    @new_insurance_file_cnt=@NewInsurance_File_Cnt

--For Debug purpose
--SELECT * FROM policy_standard_wording WHERE insurance_file_cnt=@NewInsurance_File_Cnt



DECLARE @UniqueRisk_Cnt INT

DECLARE @TotalCount_Rating_Section INT
DECLARE @TotalCount_Peril INT

--Variable for Risk Details
DECLARE @Insurance_File_Risk_Link__insurance_file_cnt 	INT,
	@Insurance_File_Risk_Link__risk_cnt		INT,
	@Insurance_File_Risk_Link__status_flag		VARCHAR(1),
	@Insurance_File_Risk_Link__original_risk_cnt	INT,
	@Insurance_File_Risk_Link__renewed_risk_cnt	INT,
	@LastActionOnRisk 				VARCHAR(10)

--Variable for Rating_Section Table
DECLARE @Rating_Section__risk_cnt	INT,
	@Rating_Section__rating_section_id	INT,
	@Rating_Section__rating_section_type_id	INT,
	@Rating_Section__policy_section_type_id	INT,
	@Rating_Section__sequence_number	INT,
	@Rating_Section__description		VARCHAR(30),	     	     
	@Rating_Section__rate_type_id		INT,
	@Rating_Section__annual_rate		NUMERIC(19,4),
	@Rating_Section__sum_insured		NUMERIC(19,4),    
	@Rating_Section__annual_premium		NUMERIC(19,4),
	@Rating_Section__this_premium		NUMERIC(19,4),
	@Rating_Section__original_flag		TINYINT,
	@Rating_Section__currency_id		SMALLINT,
	@Rating_Section__country_id		INT,
	@Rating_Section__state_id		INT,
	@Rating_Section__this_discount		NUMERIC(19,4),
	@Rating_Section__applied_discount	NUMERIC(19,4),
	@Rating_Section__adjusted_discount	NUMERIC(4,4),
	@Rating_Section__is_amended		TINYINT,
	@Rating_Section__calculated_premium	MONEY,
	@Rating_Section__override_reason	VARCHAR(255),
	@Rating_Section__discount_original_this_premium	MONEY


--Variable for Peril Table
DECLARE	@Peril__risk_cnt			INT,
	@Peril__rating_section_id		INT,
	@Peril__peril_id			INT,
	@Peril__peril_type_id			INT,
	@Peril__class_of_business_id		INT,
	@Peril__sequence_number			INT,
	@Peril__description			VARCHAR(30),
	@Peril__sum_insured			NUMERIC(21,6),
	@Peril__rating_sum_insured		NUMERIC(21,6),
	@Peril__rate_type_id			INT,
	@Peril__annual_rate			NUMERIC(21,6),
	@Peril__annual_premium			NUMERIC(21,6),
	@Peril__this_premium			NUMERIC(21,6),
	@Peril__coinsured_this_premium		NUMERIC(21,6),
	@Peril__coinsured_sum_insured		NUMERIC(21,6),
	@Peril__coinsured_commission		NUMERIC(21,6),
	@Peril__retained_this_premium		NUMERIC(21,6),
	@Peril__retained_sum_insured		NUMERIC(21,6),
	@Peril__lead_commission_band		INT,
	@Peril__sub_commission_band		INT,
	@Peril__lead_commission_value		NUMERIC(21,6),
	@Peril__sub_commission_value		NUMERIC(21,6),
	@Peril__tax_group			INT,
	@Peril__tax_value			NUMERIC(21,6),
	@Peril__ri_band				INT,
	@Peril__xl_band				TINYINT,
	@Peril__is_premium			TINYINT,
	@Peril__is_sum_insured			TINYINT,
	@Peril__is_levy_tax			TINYINT,
	@Peril__is_taxed			TINYINT



--Variable for ri_arrangement table
DECLARE @RI_Arrangement__ri_arrangement_id	INT,    
	@RI_Arrangement__risk_cnt		INT,
	@RI_Arrangement__ri_band_id		INT,
	@RI_Arrangement__ri_model_id		INT,
	@RI_Arrangement__sum_insured		MONEY,
	@RI_Arrangement__premium		MONEY,
	@RI_Arrangement__original_flag		TINYINT,
	@RI_Arrangement__is_modified		TINYINT

--Variable for GIS_Date_model & GIS_Policy_Link
DECLARE @GIS_Data_Model__code			CHAR(10),
	@GIS_Policy_Link__gis_policy_link_id	INT,    
	@GIS_Policy_Link__gis_data_model_id	INT,
	@GIS_Policy_Link__insurance_file_cnt	INT,
	@GIS_Policy_Link__quote_ref		CHAR(11),
	@GIS_Policy_Link__quote_ref_password	VARCHAR(30),
	@GIS_Policy_Link__guaranteed_quote_date	DATETIME,
	@GIS_Policy_Link__gis_scheme_id		INT,
	@GIS_Policy_Link__transact_date		DATETIME,
	@GIS_Policy_Link__transact_type		VARCHAR(20),
	@GIS_Policy_Link__party_cnt		INT,
	@GIS_Policy_Link__risk_id		INT,
	@GIS_Policy_Link__claim_id		INT,
	@GIS_Policy_Link__gis_data_model_type_id INT,
	@GIS_Policy_Link__work_claim_id		INT

DECLARE @proc_name NVARCHAR(1000)







--Copy Risk Details
DECLARE insurance_file_risk_link_Cursor CURSOR FAST_FORWARD FOR
	SELECT *,CASE WHEN original_risk_cnt IS NULL THEN
			'NONE'
			ELSE 
			'MTA' END FROM insurance_file_risk_link 
	WHERE insurance_file_cnt=@OldInsurance_file_cnt

OPEN insurance_file_risk_link_Cursor
FETCH NEXT FROM insurance_file_risk_link_Cursor 
	INTO @Insurance_File_Risk_Link__insurance_file_cnt,
	@Insurance_File_Risk_Link__risk_cnt,
	@Insurance_File_Risk_Link__status_flag,
	@Insurance_File_Risk_Link__original_risk_cnt,
	@Insurance_File_Risk_Link__renewed_risk_cnt,
	@LastActiononRisk

--Loop Through all the Risks attached to Policy
--Copy Risks and Risks related details
WHILE @@FETCH_STATUS = 0
BEGIN
	
	SELECT @UniqueRisk_Cnt=MAX(risk_cnt)+1 FROM Risk

	SET IDENTITY_INSERT Risk ON

	INSERT INTO Risk(
		risk_cnt,
		risk_status_id,
		risk_folder_cnt,
		accumulation_id,
		risk_type_id,
		description,
		sequence_number,
		sum_insured_requested,
		inception_date,
		expiry_date,
		is_not_index_linked,
		is_accumulated,
		lapsed_reason_id,
		lapsed_date,
		lapsed_description,
		var_data_ref,
		total_sum_insured,
		total_annual_premium,
		total_this_premium,
		is_ri_at_risk_level,
		is_auto_reinsured,
		gis_screen_id,
		eml_percentage,
		risk_number,
		variation_number,
		is_risk_selected,
		coverage,
		insured_item,
		extensions,
		pro_rata_rate,
		premium_this_year,
		original_risk_status_id,
		is_discounted)

	SELECT @UniqueRisk_Cnt,
		risk_status_id,
		risk_folder_cnt,
		accumulation_id,
		risk_type_id,
		description,
		sequence_number,
		sum_insured_requested,
		inception_date,
		expiry_date,
		is_not_index_linked,
		is_accumulated,
		lapsed_reason_id,
		lapsed_date,
		lapsed_description,
		var_data_ref,
		total_sum_insured,
		total_annual_premium+total_this_premium,
		0,
		is_ri_at_risk_level,
		is_auto_reinsured,
		gis_screen_id,
		eml_percentage,
		risk_number,
		variation_number,
		is_risk_selected,
		coverage,
		insured_item,
		extensions,
		pro_rata_rate,
		premium_this_year,
		original_risk_status_id,
		is_discounted
	FROM Risk WHERE risk_cnt=@Insurance_File_Risk_Link__risk_cnt
	
	SET IDENTITY_INSERT Risk OFF
	
	--For Debug purpose
	--SELECT * FROM risk WHERE risk_cnt=@UniqueRisk_Cnt
	


	--Copy Risk related Rating Section and Peril
	SELECT	@TotalCount_Rating_Section=count(*) FROM Rating_Section
	WHERE risk_cnt=@Insurance_File_Risk_Link__risk_cnt


	DECLARE Rating_Section_Cursor CURSOR FAST_FORWARD FOR
	SELECT 	risk_cnt,
		rating_section_id,
		rating_section_type_id,
		policy_section_type_id,
		sequence_number,
		description,	     	     
		rate_type_id,
		annual_rate,
		sum_insured,    
		annual_premium,
		this_premium,
		original_flag,
		currency_id,
		country_id,
		state_id,
		this_discount,
		applied_discount,
		adjusted_discount,
		is_amended,
		calculated_premium,
		override_reason,
		discount_original_this_premium
	FROM Rating_Section 
	WHERE risk_cnt=@Insurance_File_Risk_Link__risk_cnt
	
	OPEN Rating_Section_Cursor
	FETCH NEXT FROM Rating_Section_Cursor 
		INTO 	@Rating_Section__risk_cnt,
			@Rating_Section__rating_section_id,
			@Rating_Section__rating_section_type_id,
			@Rating_Section__policy_section_type_id,
			@Rating_Section__sequence_number,
			@Rating_Section__description,	     	     
			@Rating_Section__rate_type_id,
			@Rating_Section__annual_rate,
			@Rating_Section__sum_insured,    
			@Rating_Section__annual_premium,
			@Rating_Section__this_premium,
			@Rating_Section__original_flag,
			@Rating_Section__currency_id,
			@Rating_Section__country_id,
			@Rating_Section__state_id,
			@Rating_Section__this_discount,
			@Rating_Section__applied_discount,
			@Rating_Section__adjusted_discount,
			@Rating_Section__is_amended,
			@Rating_Section__calculated_premium,
			@Rating_Section__override_reason,
			@Rating_Section__discount_original_this_premium
	
		--Loop Through all the Rating_Sections
		--Copy Rating Section details
		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF(@LastActiononRisk='MTA')
			BEGIN
				INSERT INTO Rating_Section(
					risk_cnt,
					rating_section_id,
					rating_section_type_id,
					policy_section_type_id,
					sequence_number,
					description,	     	     
					rate_type_id,
					annual_rate,
					sum_insured,    
					annual_premium,
					this_premium,
					original_flag,
					currency_id,
					country_id,
					state_id,
					this_discount,
					applied_discount,
					adjusted_discount,
					is_amended,
					calculated_premium,
					override_reason,
					discount_original_this_premium)
				VALUES(@UniqueRisk_Cnt,
					@Rating_Section__rating_section_id,
					@Rating_Section__rating_section_type_id,
					@Rating_Section__policy_section_type_id,
					@Rating_Section__sequence_number,
					@Rating_Section__description,	     	     
					@Rating_Section__rate_type_id,
					@Rating_Section__annual_rate,
					@Rating_Section__sum_insured,    
					@Rating_Section__annual_premium,
					@Rating_Section__this_premium,
					@Rating_Section__original_flag,
					@Rating_Section__currency_id,
					@Rating_Section__country_id,
					@Rating_Section__state_id,
					@Rating_Section__this_discount,
					@Rating_Section__applied_discount,
					@Rating_Section__adjusted_discount,
					@Rating_Section__is_amended,
					@Rating_Section__calculated_premium,
					@Rating_Section__override_reason,
					@Rating_Section__discount_original_this_premium
					)		

			END

			ELSE

			BEGIN
				--Insert Record 1
				INSERT INTO Rating_Section(
					risk_cnt,
					rating_section_id,
					rating_section_type_id,
					policy_section_type_id,
					sequence_number,
					description,	     	     
					rate_type_id,
					annual_rate,
					sum_insured,    
					annual_premium,
					this_premium,
					original_flag,
					currency_id,
					country_id,
					state_id,
					this_discount,
					applied_discount,
					adjusted_discount,
					is_amended,
					calculated_premium,
					override_reason,
					discount_original_this_premium)
				VALUES(@UniqueRisk_Cnt,
					@Rating_Section__rating_section_id,
					@Rating_Section__rating_section_type_id,
					@Rating_Section__policy_section_type_id,
					@Rating_Section__sequence_number,
					@Rating_Section__description,	     	     
					@Rating_Section__rate_type_id,
					@Rating_Section__annual_rate,
					@Rating_Section__sum_insured,    
					@Rating_Section__annual_premium + @Rating_Section__this_premium,
					(-1) * @Rating_Section__this_premium,
					1,
					@Rating_Section__currency_id,
					@Rating_Section__country_id,
					@Rating_Section__state_id,
					@Rating_Section__this_discount,
					@Rating_Section__applied_discount,
					@Rating_Section__adjusted_discount,
					@Rating_Section__is_amended,
					0,
					@Rating_Section__override_reason,
					@Rating_Section__discount_original_this_premium
					)
				


				--Insert Record 2
				INSERT INTO Rating_Section(
					risk_cnt,
					rating_section_id,
					rating_section_type_id,
					policy_section_type_id,
					sequence_number,
					description,	     	     
					rate_type_id,
					annual_rate,
					sum_insured,    
					annual_premium,
					this_premium,
					original_flag,
					currency_id,
					country_id,
					state_id,
					this_discount,
					applied_discount,
					adjusted_discount,
					is_amended,
					calculated_premium,
					override_reason,
					discount_original_this_premium)
				VALUES(@UniqueRisk_Cnt,
					@Rating_Section__rating_section_id + @TotalCount_Rating_Section,
					@Rating_Section__rating_section_type_id,
					@Rating_Section__policy_section_type_id,
					@Rating_Section__sequence_number,
					@Rating_Section__description,	     	     
					@Rating_Section__rate_type_id,
					@Rating_Section__annual_rate,
					@Rating_Section__sum_insured,    
					@Rating_Section__annual_premium + @Rating_Section__this_premium,
					@Rating_Section__this_premium,
					@Rating_Section__original_flag,
					@Rating_Section__currency_id,
					@Rating_Section__country_id,
					@Rating_Section__state_id,
					@Rating_Section__this_discount,
					@Rating_Section__applied_discount,
					@Rating_Section__adjusted_discount,
					@Rating_Section__is_amended,
					0,
					@Rating_Section__override_reason,
					@Rating_Section__discount_original_this_premium
					)


			END
	
	
			FETCH NEXT FROM Rating_Section_Cursor 
				INTO 	@Rating_Section__risk_cnt,
					@Rating_Section__rating_section_id,
					@Rating_Section__rating_section_type_id,
					@Rating_Section__policy_section_type_id,
					@Rating_Section__sequence_number,
					@Rating_Section__description,	     	     
					@Rating_Section__rate_type_id,
					@Rating_Section__annual_rate,
					@Rating_Section__sum_insured,    
					@Rating_Section__annual_premium,
					@Rating_Section__this_premium,
					@Rating_Section__original_flag,
					@Rating_Section__currency_id,
					@Rating_Section__country_id,
					@Rating_Section__state_id,
					@Rating_Section__this_discount,
					@Rating_Section__applied_discount,
					@Rating_Section__adjusted_discount,
					@Rating_Section__is_amended,
					@Rating_Section__calculated_premium,
					@Rating_Section__override_reason,
					@Rating_Section__discount_original_this_premium
	END
	CLOSE Rating_Section_Cursor
	DEALLOCATE Rating_Section_Cursor








	

	--Copy Risk related Perils
	SELECT	@TotalCount_Peril=count(*) FROM Peril
	WHERE risk_cnt=@Insurance_File_Risk_Link__risk_cnt

	DECLARE Peril_Cursor CURSOR FAST_FORWARD FOR
	SELECT 	risk_cnt,
		rating_section_id,
		peril_id,
		peril_type_id,
		class_of_business_id,
		sequence_number,
		description,
		sum_insured,
		rating_sum_insured,
		rate_type_id,
		annual_rate,
		annual_premium,
		this_premium,
		coinsured_this_premium,
		coinsured_sum_insured,
		coinsured_commission,
		retained_this_premium,
		retained_sum_insured,
		lead_commission_band,
		sub_commission_band,
		lead_commission_value,
		sub_commission_value,
		tax_group,
		tax_value,
		ri_band,
		xl_band,
		is_premium,
		is_sum_insured,
		is_levy_tax,
		is_taxed
	FROM Peril 
	WHERE risk_cnt=@Insurance_File_Risk_Link__risk_cnt
	
	OPEN Peril_Cursor
	FETCH NEXT FROM Peril_Cursor 
		INTO 	@Peril__risk_cnt,
			@Peril__rating_section_id,
			@Peril__peril_id,
			@Peril__peril_type_id,
			@Peril__class_of_business_id,
			@Peril__sequence_number,
			@Peril__description,
			@Peril__sum_insured,
			@Peril__rating_sum_insured,
			@Peril__rate_type_id,
			@Peril__annual_rate,
			@Peril__annual_premium,
			@Peril__this_premium,
			@Peril__coinsured_this_premium,
			@Peril__coinsured_sum_insured,
			@Peril__coinsured_commission,
			@Peril__retained_this_premium,
			@Peril__retained_sum_insured,
			@Peril__lead_commission_band,
			@Peril__sub_commission_band,
			@Peril__lead_commission_value,
			@Peril__sub_commission_value,
			@Peril__tax_group,
			@Peril__tax_value,
			@Peril__ri_band,
			@Peril__xl_band,
			@Peril__is_premium,
			@Peril__is_sum_insured,
			@Peril__is_levy_tax,
			@Peril__is_taxed
	
		--Loop Through all the Perils
		--Copy Peril details
		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF(@LastActiononRisk='MTA')
			BEGIN
				INSERT INTO Peril(
					risk_cnt,
					rating_section_id,
					peril_id,
					peril_type_id,
					class_of_business_id,
					sequence_number,
					description,
					sum_insured,
					rating_sum_insured,
					rate_type_id,
					annual_rate,
					annual_premium,
					this_premium,
					coinsured_this_premium,
					coinsured_sum_insured,
					coinsured_commission,
					retained_this_premium,
					retained_sum_insured,
					lead_commission_band,
					sub_commission_band,
					lead_commission_value,
					sub_commission_value,
					tax_group,
					tax_value,
					ri_band,
					xl_band,
					is_premium,
					is_sum_insured,
					is_levy_tax,
					is_taxed)
				VALUES(@UniqueRisk_Cnt,
					@Peril__rating_section_id,
					@Peril__peril_id,
					@Peril__peril_type_id,
					@Peril__class_of_business_id,
					@Peril__sequence_number,
					@Peril__description,
					@Peril__sum_insured,
					@Peril__rating_sum_insured,
					@Peril__rate_type_id,
					@Peril__annual_rate,
					@Peril__annual_premium,
					@Peril__this_premium,
					@Peril__coinsured_this_premium,
					@Peril__coinsured_sum_insured,
					@Peril__coinsured_commission,
					@Peril__retained_this_premium,
					@Peril__retained_sum_insured,
					@Peril__lead_commission_band,
					@Peril__sub_commission_band,
					@Peril__lead_commission_value,
					@Peril__sub_commission_value,
					@Peril__tax_group,
					@Peril__tax_value,
					@Peril__ri_band,
					@Peril__xl_band,
					@Peril__is_premium,
					@Peril__is_sum_insured,
					@Peril__is_levy_tax,
					@Peril__is_taxed
					)		

			END

			ELSE

			BEGIN
				--Insert Record 1
				INSERT INTO Peril(
					risk_cnt,
					rating_section_id,
					peril_id,
					peril_type_id,
					class_of_business_id,
					sequence_number,
					description,
					sum_insured,
					rating_sum_insured,
					rate_type_id,
					annual_rate,
					annual_premium,
					this_premium,
					coinsured_this_premium,
					coinsured_sum_insured,
					coinsured_commission,
					retained_this_premium,
					retained_sum_insured,
					lead_commission_band,
					sub_commission_band,
					lead_commission_value,
					sub_commission_value,
					tax_group,
					tax_value,
					ri_band,
					xl_band,
					is_premium,
					is_sum_insured,
					is_levy_tax,
					is_taxed)
				VALUES(@UniqueRisk_Cnt,
					@Peril__rating_section_id,
					@Peril__peril_id,
					@Peril__peril_type_id,
					@Peril__class_of_business_id,
					@Peril__sequence_number,
					@Peril__description,
					(-1) * @Peril__sum_insured,
					(-1) * @Peril__rating_sum_insured,
					@Peril__rate_type_id,
					@Peril__annual_rate,
					@Peril__annual_premium,
					(-1) * @Peril__this_premium,
					@Peril__coinsured_this_premium,
					@Peril__coinsured_sum_insured,
					@Peril__coinsured_commission,
					@Peril__retained_this_premium,
					@Peril__retained_sum_insured,
					@Peril__lead_commission_band,
					@Peril__sub_commission_band,
					@Peril__lead_commission_value,
					@Peril__sub_commission_value,
					@Peril__tax_group,
					@Peril__tax_value,
					@Peril__ri_band,
					@Peril__xl_band,
					@Peril__is_premium,
					@Peril__is_sum_insured,
					@Peril__is_levy_tax,
					@Peril__is_taxed
					)
				


				--Insert Record 2
				INSERT INTO Peril(
					risk_cnt,
					rating_section_id,
					peril_id,
					peril_type_id,
					class_of_business_id,
					sequence_number,
					description,
					sum_insured,
					rating_sum_insured,
					rate_type_id,
					annual_rate,
					annual_premium,
					this_premium,
					coinsured_this_premium,
					coinsured_sum_insured,
					coinsured_commission,
					retained_this_premium,
					retained_sum_insured,
					lead_commission_band,
					sub_commission_band,
					lead_commission_value,
					sub_commission_value,
					tax_group,
					tax_value,
					ri_band,
					xl_band,
					is_premium,
					is_sum_insured,
					is_levy_tax,
					is_taxed)
				VALUES(@UniqueRisk_Cnt,
					@Peril__rating_section_id+@TotalCount_Peril,
					@Peril__peril_id,
					@Peril__peril_type_id,
					@Peril__class_of_business_id,
					@Peril__sequence_number,
					@Peril__description,
					@Peril__sum_insured,
					@Peril__rating_sum_insured,
					@Peril__rate_type_id,
					@Peril__annual_rate,
					@Peril__this_premium,
					@Peril__this_premium,
					@Peril__coinsured_this_premium,
					@Peril__coinsured_sum_insured,
					@Peril__coinsured_commission,
					@Peril__retained_this_premium,
					@Peril__retained_sum_insured,
					@Peril__lead_commission_band,
					@Peril__sub_commission_band,
					@Peril__lead_commission_value,
					@Peril__sub_commission_value,
					@Peril__tax_group,
					@Peril__tax_value,
					@Peril__ri_band,
					@Peril__xl_band,
					@Peril__is_premium,
					@Peril__is_sum_insured,
					@Peril__is_levy_tax,
					@Peril__is_taxed
					)


			END
	
	
			FETCH NEXT FROM Peril_Cursor 
				INTO 	@Peril__risk_cnt,
					@Peril__rating_section_id,
					@Peril__peril_id,
					@Peril__peril_type_id,
					@Peril__class_of_business_id,
					@Peril__sequence_number,
					@Peril__description,
					@Peril__sum_insured,
					@Peril__rating_sum_insured,
					@Peril__rate_type_id,
					@Peril__annual_rate,
					@Peril__annual_premium,
					@Peril__this_premium,
					@Peril__coinsured_this_premium,
					@Peril__coinsured_sum_insured,
					@Peril__coinsured_commission,
					@Peril__retained_this_premium,
					@Peril__retained_sum_insured,
					@Peril__lead_commission_band,
					@Peril__sub_commission_band,
					@Peril__lead_commission_value,
					@Peril__sub_commission_value,
					@Peril__tax_group,
					@Peril__tax_value,
					@Peril__ri_band,
					@Peril__xl_band,
					@Peril__is_premium,
					@Peril__is_sum_insured,
					@Peril__is_levy_tax,
					@Peril__is_taxed
	END
	CLOSE Peril_Cursor
	DEALLOCATE Peril_Cursor







	--Copy Risk related ri_arrangement
	DECLARE RI_Arrangement_Cursor CURSOR FAST_FORWARD FOR
	SELECT 	ri_arrangement_id,    
		risk_cnt,
		ri_band_id,
		ri_model_id,
		sum_insured,
		premium,
		original_flag,
		is_modified
	FROM RI_Arrangement 
	WHERE risk_cnt=@Insurance_File_Risk_Link__risk_cnt
	
	OPEN RI_Arrangement_Cursor
	FETCH NEXT FROM RI_Arrangement_Cursor 
		INTO 	@RI_Arrangement__ri_arrangement_id,    
			@RI_Arrangement__risk_cnt,
			@RI_Arrangement__ri_band_id,
			@RI_Arrangement__ri_model_id,
			@RI_Arrangement__sum_insured,
			@RI_Arrangement__premium,
			@RI_Arrangement__original_flag,
			@RI_Arrangement__is_modified
	
		--Loop Through all the RI_Arrangement
		--Copy RI_Arrangement details
		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF(@LastActiononRisk='MTA')
			BEGIN
				INSERT INTO RI_Arrangement(
					risk_cnt,
					ri_band_id,
					ri_model_id,
					sum_insured,
					premium,
					original_flag,
					is_modified)
				VALUES(@UniqueRisk_Cnt,
					@RI_Arrangement__ri_band_id,
					@RI_Arrangement__ri_model_id,
					@RI_Arrangement__sum_insured,
					@RI_Arrangement__premium,
					@RI_Arrangement__original_flag,
					@RI_Arrangement__is_modified
					)		

			END

			ELSE

			BEGIN
				--Insert Record 1
				INSERT INTO RI_Arrangement(
					risk_cnt,
					ri_band_id,
					ri_model_id,
					sum_insured,
					premium,
					original_flag,
					is_modified)
				VALUES(@UniqueRisk_Cnt,
					@RI_Arrangement__ri_band_id,
					@RI_Arrangement__ri_model_id,
					(-1) * @RI_Arrangement__sum_insured,
					(-1) * @RI_Arrangement__premium,
					1,
					@RI_Arrangement__is_modified
					)
				


				--Insert Record 2
				INSERT INTO RI_Arrangement(
					risk_cnt,
					ri_band_id,
					ri_model_id,
					sum_insured,
					premium,
					original_flag,
					is_modified)
				VALUES(@UniqueRisk_Cnt,
					@RI_Arrangement__ri_band_id,
					@RI_Arrangement__ri_model_id,
					@RI_Arrangement__sum_insured,
					@RI_Arrangement__premium,
					@RI_Arrangement__original_flag,
					@RI_Arrangement__is_modified
					)


			END
	
	
			FETCH NEXT FROM RI_Arrangement_Cursor 
				INTO 	@RI_Arrangement__ri_arrangement_id,    
					@RI_Arrangement__risk_cnt,
					@RI_Arrangement__ri_band_id,
					@RI_Arrangement__ri_model_id,
					@RI_Arrangement__sum_insured,
					@RI_Arrangement__premium,
					@RI_Arrangement__original_flag,
					@RI_Arrangement__is_modified
	END
	CLOSE RI_Arrangement_Cursor
	DEALLOCATE RI_Arrangement_Cursor



	--Link new added Risk to Insurance_file
	INSERT INTO insurance_file_risk_link(
		insurance_file_cnt,
		risk_cnt,
		status_flag,
		original_risk_cnt,
		renewed_risk_cnt)
	VALUES (@NewInsurance_File_Cnt,
		@UniqueRisk_Cnt,
		@Insurance_File_Risk_Link__status_flag,
		@Insurance_File_Risk_Link__risk_cnt,
		@Insurance_File_Risk_Link__renewed_risk_cnt
		)

	
	--SELECT * FROM Insurance_File_Risk_Link WHERE insurance_file_cnt=@NewInsurance_File_Cnt


	
	--GIS Data
	SELECT 	@GIS_Data_Model__code=GDM.code,
		@GIS_Policy_Link__gis_policy_link_id=GPL.gis_policy_link_id,    
		@GIS_Policy_Link__gis_data_model_type_id=GPL.gis_data_model_type_id,
		@GIS_Policy_Link__insurance_file_cnt=GPL.insurance_file_cnt

	 FROM gis_data_model GDM ,gis_policy_link GPL 
		WHERE GDM.Gis_data_model_id=GPL.Gis_data_model_id
			AND GPL.risk_id=@Insurance_File_Risk_Link__risk_cnt
	
	
	SELECT @proc_name = 'DECLARE @NewGIS_Policy_Link_ID INT EXEC spg_' + LTRIM(RTRIM(@GIS_Data_Model__code)) + '_copy_dataset
		@Old_gis_policy_link_id=' +  CAST(@GIS_Policy_Link__gis_policy_link_id AS VARCHAR(255)) + ',
		@old_insurance_file_cnt=' + CAST(@GIS_Policy_Link__insurance_file_cnt AS VARCHAR(255)) + ',
		@old_risk_id=' + CAST(@Insurance_File_Risk_Link__risk_cnt AS VARCHAR(255)) + ',
		@new_insurance_file_cnt=' + CAST(@GIS_Policy_Link__insurance_file_cnt AS VARCHAR(255)) + ',
		@new_risk_id=' + CAST(@UniqueRisk_Cnt AS VARCHAR(255)) + ','
		
	SELECT @proc_name=@proc_name + '@copy_quotes=NULL, @new_gis_policy_link_id=@NewGIS_Policy_Link_ID OUTPUT'


	EXEC sp_executesql @proc_name
	

	

	IF(@OldRisk_Cnt=@Insurance_File_Risk_Link__risk_cnt)
	BEGIN
		SELECT @NewRisk_Cnt=@UniqueRisk_Cnt
		SELECT @NewGIS_Policy_Link_ID=(SELECT TOP 1 GIS_Policy_Link_ID 
			FROM GIS_Policy_Link WHERE Risk_ID=@NewRisk_Cnt ORDER BY GIS_Policy_Link_ID DESC)
	END
	
		
	
	
	FETCH NEXT FROM insurance_file_risk_link_Cursor 
		INTO @Insurance_File_Risk_Link__insurance_file_cnt,
		@Insurance_File_Risk_Link__risk_cnt,
		@Insurance_File_Risk_Link__status_flag,
		@Insurance_File_Risk_Link__original_risk_cnt,
		@Insurance_File_Risk_Link__renewed_risk_cnt,
		@LastActiononRisk
END
CLOSE insurance_file_risk_link_Cursor
DEALLOCATE insurance_file_risk_link_Cursor




SET NOCOUNT ON



END
GO 
