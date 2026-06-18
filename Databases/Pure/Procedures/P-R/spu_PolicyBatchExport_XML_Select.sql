
SET QUOTED_IDENTIFIER OFF
GO
EXECUTE DDLDropProcedure 'spu_PolicyBatchExport_XML_Select'
GO

CREATE PROCEDURE [dbo].[spu_PolicyBatchExport_XML_Select]
	@batch_id INT = NULL,
	@new_batch BIT = 0,
	@start_date DATETIME = NULL,
	@end_date DATETIME = NULL
AS

DECLARE @parameters VARCHAR(300)
SET @parameters = NULL

IF (@new_batch = 1)
BEGIN
	UPDATE
		ifi
	SET
		ifi.batch_id = @batch_id
	FROM Insurance_File ifi
	JOIN Insurance_File_Type ift ON ifi.insurance_file_type_id =ift.insurance_file_type_id  
	WHERE ift.code IN ('POLICY','MTA PERM','MTA TEMP','MTACAN','MTAREINS','WRITTEN')
	AND
		ifi.batch_id IS NULL
	AND
		(@start_date IS NULL OR ifi.cover_start_date >= @start_date)
	AND
		(@end_date IS NULL OR ifi.cover_start_date <= @end_date)
END

IF (@start_date IS NOT NULL)
BEGIN
	IF (@parameters IS NULL)
		SET @parameters = 'start_date=' + Cast(@start_date AS VARCHAR(12))
	ELSE
		SET @parameters = @parameters + 'start_date=' + Cast(@start_date AS VARCHAR(12))
END

IF (@end_date IS NOT NULL)
BEGIN
	IF (@parameters IS NULL)
		SET @parameters = 'end_date=' + CAST(@end_date  AS VARCHAR(12))
	ELSE
		SET @parameters = @parameters + ' end_date=' + Cast(@end_date  AS VARCHAR(12))
END

IF (@batch_id IS NOT NULL)
BEGIN
	IF (@parameters IS NULL)
		SET @parameters = 'batch_id=' + Cast(@batch_id  AS VARCHAR(10))
	ELSE
		SET @parameters = @parameters + ' batch_id=' + Cast(@batch_id  AS VARCHAR(10))
END

-- *******************************************************************************************
-- ROOT LEVEL - EXPORT_HEADER
-- *******************************************************************************************

  Select  1                         AS Tag,
  NULL AS  Parent,
  'http://www.siriusfs.com/SFI/Export/Policy_Batch_Export/20060419'
                                 AS [EXPORT_HEADER!1!xmlns],
  'http://www.w3.org/2001/XMLSchema-instance'
                                 AS [EXPORT_HEADER!1!xmlns:xsi],
  'http://www.siriusfs.com/SFI/Export/Policy_Batch_Export/20060419 Policy_Batch_Export.xsd'                AS [EXPORT_HEADER!1!xsi:schemaLocation],
  GetDate()                       AS [EXPORT_HEADER!1!date_exported],
  'POLICY_BATCH_EXPORT' As [EXPORT_HEADER!1!interface_name],
  @parameters As [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  NULL AS  [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL AS  [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  NULL AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS  [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  NULL AS  [RISK!7!risk_cnt],
  NULL AS  [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]
  
  UNION

-- *******************************************************************************************
-- FIRST LEVEL - PARTY
-- *******************************************************************************************

 Select  2         AS Tag,
  1         AS Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt   as [PARTY!2!party_cnt],
  rtrim(party_type.code)  as [PARTY!2!party_type_code],
  rtrim(source.code)  as [PARTY!2!source_code],
  rtrim(p.shortname)  as [PARTY!2!shortname],
  rtrim(p.name)   as [PARTY!2!name],
  rtrim(p.resolved_name)  as [PARTY!2!resolved_name],
  rtrim(currency.code)  as [PARTY!2!currency_code],
  rtrim(language.code)  as [PARTY!2!language_code],
  rtrim(party_agent.shortname) as [PARTY!2!agent_code],
  rtrim(pmuserc.username)  as [PARTY!2!created_by_username],
  p.date_created   as [PARTY!2!date_created],
  p.last_modified   as [PARTY!2!last_modified],
  rtrim(pmuserm.username)  as [PARTY!2!modified_by_username],
  rtrim(p.payment_method_code) as [PARTY!2!payment_method_code],
  rtrim(p.file_code)  as [PARTY!2!file_code],
  p.is_prospect   as [PARTY!2!is_prospect],
  rtrim(p.loyalty_number)  as [PARTY!2!loyalty_number],
  rtrim(p.alternative_identifier) as [PARTY!2!alternative_identifier],
  rtrim(p.trading_name)  as [PARTY!2!trading_name],
  rtrim(subbranch.code)  as [PARTY!2!sub_branch_code],
  rtrim(p.tax_number)  as [PARTY!2!tax_number],
  p.domiciled_for_tax  as [PARTY!2!domiciled_for_tax],
  p.tax_exempt   as [PARTY!2!tax_exempt],
  p.tax_percentage  as [PARTY!2!tax_percentage],
  rtrim(blacklist_reason.code) as [PARTY!2!blacklist_reason_code],
  rtrim(pc.party_title_code) as [PARTY!2!party_title_code],
  rtrim(pc.forename)  as [PARTY!2!forename],
  rtrim(pc.initials)  as [PARTY!2!initials],
  rtrim(pc.employment_status_code)as [PARTY!2!employment_status_code],
  rtrim(pc.employer_business) as [PARTY!2!employer_business],
  rtrim(pc.secondary_employer_business) as [PARTY!2!secondary_employer_business],
  rtrim(pc.secondary_employment_status_co)as [PARTY!2!secondary_employment_status_code],
  rtrim(pc.marital_status_code)  as [PARTY!2!marital_status_code],
  pc.number_of_children  as [PARTY!2!number_of_children],
  pc.Nationality_id  as [PARTY!2!Nationality_id],
  rtrim(pc.country_of_origin_code)as [PARTY!2!country_of_origin_code],
  pc.mailshot   as [PARTY!2!mailshot],
  pc.is_pet_owner   as [PARTY!2!is_pet_owner],
  rtrim(pc.accommodation_type_code) as [PARTY!2!accommodation_type_code],

	CASE WHEN pls.date_of_birth < '1900-01-01T00:00:00' THEN
		NULL
	Else
		pls.date_of_birth
	END
	As [PARTY!2!date_of_birth],

  rtrim(pls.gender_code)   As [PARTY!2!gender_code],
  rtrim(pls.occupation_code)  As [PARTY!2!occupation_code],
  rtrim(pls.secondary_occupation_code) As [PARTY!2!secondary_occupation_code],
  pls.is_smoker   As [PARTY!2!is_smoker],
  rtrim(cc.company_reg)   as [PARTY!2!company_reg],
  cc.trading_since_date  as [PARTY!2!trading_since_date],
  cc.party_business_id  as [PARTY!2!party_business_code],
  cc.location   as [PARTY!2!location],
  cc.no_of_offices  as [PARTY!2!no_of_offices],
  cc.no_of_employees  as [PARTY!2!no_of_employees],
  cc.financial_year  as [PARTY!2!financial_year],
  rtrim(cc.trade_code)  as [PARTY!2!trade_code],
  cc.wage_roll   as [PARTY!2!wage_roll],
  cc.trade_id   as [PARTY!2!trade_id],
  rtrim(party_group_type.code) as [PARTY!2!party_group_type_code],
  gc.is_registered_charity as [PARTY!2!is_registered_charity],
  rtrim(gc.charity_number) as [PARTY!2!charity_number],
  gc.number_of_members  as [PARTY!2!number_of_members],
  NULL AS [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  NULL AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  NULL AS [RISK!7!risk_cnt],
  NULL    AS  [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]
 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  INNER JOIN source ON
   p.source_id = source.source_id

  INNER JOIN currency ON
   p.currency_id = currency.currency_id

  INNER JOIN language ON
   p.language_id = language.language_id

  INNER JOIN pmuser AS pmuserc ON
   p.created_by_id = pmuserc.user_id

  INNER JOIN pmuser AS pmuserm ON
   p.modified_by_id = pmuserm.user_id

  LEFT JOIN party_personal_client AS pc ON
   p.party_cnt = pc.party_cnt

  LEFT JOIN party_lifestyle AS pls ON
   p.party_cnt = pls.party_cnt

  LEFT JOIN party_corporate_client AS cc ON
   p.party_cnt = cc.party_cnt

  LEFT JOIN party_group_client AS gc ON
   p.party_cnt = gc.party_cnt

  LEFT JOIN party AS party_agent ON
   p.agent_cnt = party_agent.party_cnt

  LEFT JOIN source AS subbranch ON
   p.source_id = subbranch.source_id

  LEFT JOIN blacklist_reason ON
   p.blacklist_reason_id = blacklist_reason.blacklist_reason_id

  LEFT JOIN party_group_type ON
   gc.party_group_type_id = party_group_type.party_group_type_id

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND insfile.batch_id = @batch_id
UNION

-- *******************************************************************************************
-- THIRD LEVEL - PARTYBUILDER - PARENT - PARTY
-- *******************************************************************************************

 Select  3         As Tag,
  2         As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt   as [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  'NO PARTY BUILDER DATA FOUND'   AS   [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  NULL AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS  [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS  [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS  [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS  [POLICY!6!alternate_reference],
  NULL AS  [POLICY!6!is_client_invoiced],
  NULL AS  [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS  [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS  [POLICY!6!discounted_premium],
  NULL AS  [POLICY!6!discount_percentage],
  NULL AS  [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS  [POLICY!6!policy_documents_issued_date],
  NULL AS  [POLICY!6!policy_documents_correct],
  NULL AS  [POLICY!6!insurance_file_type_code],
  NULL AS  [RISK!7!risk_cnt],
  NULL AS  [RISKDATA!8!!CDATA],
  NULL AS  [RATING_SECTION!9!rating_section_id],
  NULL AS  [RATING_SECTION!9!sequence],
  NULL AS  [RATING_SECTION!9!rating_section_type_code],
  NULL AS  [RATING_SECTION!9!sum_insured],
  NULL AS  [RATING_SECTION!9!annual_premium],
  NULL AS  [RATING_SECTION!9!this_premium],
  NULL AS  [RATING_SECTION!9!original_flag],
  NULL AS  [RATING_SECTION!9!calculated_premium],
  NULL AS  [RATING_SECTION!9!earning_pattern_code]
 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND insfile.batch_id = @batch_id

    UNION

-- *******************************************************************************************
-- FOURTH LEVEL - ADDRESS - PARENT - PARTY
-- *******************************************************************************************

Select DISTINCT 4         As Tag,
  2         As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt As [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL AS  [PARTYBUILDER!3!!CDATA],
  addr.address_cnt As [PARTYADDRESS!4!party_address_cnt],
  address_usage_type.description As [PARTYADDRESS!4!party_address_type],
  rtrim(addr.address1) As [PARTYADDRESS!4!party_address1],
  rtrim(addr.address2) As [PARTYADDRESS!4!party_address2],
  rtrim(addr.address3) As [PARTYADDRESS!4!party_address3],
  rtrim(addr.address4) As [PARTYADDRESS!4!party_address4],
  rtrim(addr.postal_code) As [PARTYADDRESS!4!party_postal_code],
  rtrim(country.code) As [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  NULL AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  NULL AS  [RISK!7!risk_cnt],
  NULL    AS  [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]
 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  INNER JOIN party_address_usage ON
   p.party_cnt = party_address_usage.party_cnt

  INNER JOIN address AS addr ON
   addr.address_cnt = party_address_usage.address_cnt

  INNER JOIN address_usage_type ON
   party_address_usage.address_usage_type_id = address_usage_type.address_usage_type_id

  LEFT JOIN country ON
   addr.country_id = country.country_id

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND insfile.batch_id = @batch_id

UNION

-- *******************************************************************************************
-- FIFTH LEVEL - CONTACTS - PARENT - PARTY
-- *******************************************************************************************

Select DISTINCT 5         As Tag,
  2         As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt             As [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL    AS  [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  contact.contact_cnt As [PARTYCONTACT!5!contact_cnt],
  rtrim(contact_type.code)As [PARTYCONTACT!5!contact_type_code],
  contact.description As [PARTYCONTACT!5!description],
  ltrim(rtrim(contact.area_code) + ' ' + rtrim(contact.number)) As [PARTYCONTACT!5!detail],
  rtrim(contact.extension)As [PARTYCONTACT!5!extension],
  NULL AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  NULL AS  [RISK!7!risk_cnt],
  NULL    AS  [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]
  
 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  INNER JOIN party_contact_usage ON
   p.party_cnt = party_contact_usage.party_cnt

  INNER JOIN contact ON
   contact.contact_cnt = party_contact_usage.contact_cnt

  INNER JOIN contact_type ON
   contact.contact_type_id = contact_type.contact_type_id

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND insfile.batch_id = @batch_id

UNION

-- *******************************************************************************************
-- SIXTH LEVEL - POLICY - PARENT - PARTY
-- *******************************************************************************************

 Select  6         As Tag,
  2         As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt             As [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL    AS  [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  insfile.insurance_file_cnt As [POLICY!6!insurance_file_cnt],
  rtrim(insfilestatus.code) As [POLICY!6!insurance_file_status_code],
  rtrim(insfile.insurance_ref) As [POLICY!6!insurance_ref],
  insfile.insurance_folder_cnt As [POLICY!6!insurance_folder_cnt],
  rtrim(product.code)  As [POLICY!6!product_code],
  rtrim(insurerparty.shortname) As [POLICY!6!lead_insurer_code],
  rtrim(agentparty.shortname) As [POLICY!6!lead_agent_code],
  rtrim(currency.code)  As [POLICY!6!currency_code],
  rtrim(language.code)  As [POLICY!6!language_code],

	CASE WHEN insfile.date_issued < '1900-01-01T00:00:00' THEN
		NULL
	Else
		insfile.date_issued
	END
	As [POLICY!6!date_issued],

  insfile.cover_start_date As [POLICY!6!cover_start_date],
  insfile.expiry_date  As [POLICY!6!expiry_date],
  insfile.renewal_date  As [POLICY!6!renewal_date],
  rtrim(renewal_frequency.code) As [POLICY!6!renewal_frequency_code],
  insfile.is_referred_at_renewal As [POLICY!6!is_referred_at_renewal],
  insfile.policy_version  As [POLICY!6!policy_version],
  rtrim(Analysis_code.code) As [POLICY!6!Analysis_code],
  insfile.proposal_date  As [POLICY!6!proposal_date],
  rtrim(Policy_type.code)  As [POLICY!6!Policy_type_code],
  insfile.annual_premium  As [POLICY!6!annual_premium],
  insfile.this_premium  As [POLICY!6!this_premium],
  insfile.net_premium  As [POLICY!6!net_premium],
  insfile.commission_amount As [POLICY!6!commission_amount],
  insfile.iptable_amount  As [POLICY!6!iptable_amount],
  insfile.ipt_percentage  As [POLICY!6!ipt_percentage],
  insfile.is_ipt_overridden As [POLICY!6!is_ipt_overridden],
  insfile.tax_amount  As [POLICY!6!tax_amount],
  insfile.vatable_amount  As [POLICY!6!vatable_amount],
  insfile.vat_percentage  As [POLICY!6!vat_percentage],
  insfile.vat_amount  As [POLICY!6!vat_amount],
  rtrim(insfile.alternate_reference) As [POLICY!6!alternate_reference],
  insfile.is_client_invoiced As [POLICY!6!is_client_invoiced],
  insfile.quote_expiry_date As [POLICY!6!quote_expiry_date],
  rtrim(policy_status.code) As [POLICY!6!policy_status_code],
  insfile.inception_date_tpi As [POLICY!6!inception_date_tpi],
  rtrim(discount_reason.code) As [POLICY!6!discount_reason_code],
  insfile.discounted_premium As [POLICY!6!discounted_premium],
  insfile.discount_percentage As [POLICY!6!discount_percentage],
  insfile.match_discounted_premium_flag As [POLICY!6!match_discounted_premium_flag],
  rtrim(Country.code)  As [POLICY!6!Country_code],
  insfile.inception_Date  As [POLICY!6!inception_Date],
  insfile.policy_documents_issued_date As [POLICY!6!policy_documents_issued_date],
  insfile.policy_documents_correct As [POLICY!6!policy_documents_correct],
  insfiletype.code As [POLICY!6!insurance_file_type_code],
  NULL As [RISK!7!risk_cnt],
  NULL As [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]

 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  LEFT JOIN insurance_file_status AS insfilestatus ON
   insfilestatus.insurance_file_status_id = insfile.insurance_file_status_id

  INNER JOIN product ON
   product.product_id = insfile.product_id

  LEFT JOIN party AS insurerparty ON
   insurerparty.party_cnt = insfile.lead_insurer_cnt

  LEFT JOIN party AS agentparty ON
   insurerparty.party_cnt = insfile.lead_agent_cnt

  INNER JOIN currency ON
   currency.currency_id = insfile.currency_id

  INNER JOIN language ON
   language.language_id = insfile.language_id

  INNER JOIN renewal_frequency ON
   renewal_frequency.renewal_frequency_id = insfile.renewal_frequency_id

  LEFT JOIN Analysis_code ON
   Analysis_code.Analysis_code_id = insfile.Analysis_code_id

  LEFT JOIN Policy_type ON
   Policy_type.Policy_type_id = insfile.Policy_type_id

  LEFT JOIN policy_status ON
   policy_status.policy_status_id = insfile.policy_status_id

  LEFT JOIN discount_reason ON
   discount_reason.discount_reason_id = insfile.discount_reason_id

  LEFT JOIN Country ON
   Country.Country_id = insfile.Country_id

LEFT JOIN insurance_file_type AS insfiletype ON
   insfiletype.insurance_file_type_id = insfile.insurance_file_type_id

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND insfile.batch_id = @batch_id

UNION

-- *******************************************************************************************
-- SEVENTH LEVEL - RISK - PARENT - POLICY
-- *******************************************************************************************

 Select  7         As Tag,
  6         As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt         As [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL AS  [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  insfile.insurance_file_cnt AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  risk.risk_cnt AS [RISK!7!risk_cnt],
  NULL AS [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]
 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  INNER JOIN insurance_file_risk_link AS ifrl ON
   insfile.insurance_file_cnt = ifrl.insurance_file_cnt

  INNER JOIN risk ON
   ifrl.risk_cnt = risk.risk_cnt

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND Risk.is_risk_selected = 1
	AND insfile.batch_id = @batch_id

UNION

-- *******************************************************************************************
-- EIGHTH LEVEL - PARTYBUILDER - PARENT - PARTY
-- *******************************************************************************************

 Select  8    As Tag,
  7    As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt   as [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL AS [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  insfile.insurance_file_cnt AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  risk.risk_cnt AS [RISK!7!risk_cnt],
  'NO RISK DATA FOUND' AS [RISKDATA!8!!CDATA],
  NULL AS [RATING_SECTION!9!rating_section_id],
  NULL AS [RATING_SECTION!9!sequence],
  NULL AS [RATING_SECTION!9!rating_section_type_code],
  NULL AS [RATING_SECTION!9!sum_insured],
  NULL AS [RATING_SECTION!9!annual_premium],
  NULL AS [RATING_SECTION!9!this_premium],
  NULL AS [RATING_SECTION!9!original_flag],
  NULL AS [RATING_SECTION!9!calculated_premium],
  NULL AS [RATING_SECTION!9!earning_pattern_code]

 FROM party p

  INNER JOIN party_type ON
   p.party_type_id = party_type.party_type_id

  INNER JOIN insurance_file AS insfile ON
   p.party_cnt = insfile.insured_cnt

  INNER JOIN insurance_folder AS insfol ON
   insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

  INNER JOIN insurance_file_risk_link AS ifrl ON
   insfile.insurance_file_cnt = ifrl.insurance_file_cnt

  INNER JOIN risk ON
   ifrl.risk_cnt = risk.risk_cnt

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
	AND insfile.batch_id = @batch_id
   AND Risk.is_risk_selected = 1

UNION

-- *******************************************************************************************
-- NINTH LEVEL - RATING_SECTION - PARENT - RISK
-- *******************************************************************************************

 Select 9 As Tag,
  7 As Parent,
  NULL AS  [EXPORT_HEADER!1!xmlns],
  NULL AS  [EXPORT_HEADER!1!xmlns:xsi],
  NULL AS  [EXPORT_HEADER!1!xsi:schemaLocation],
  NULL AS  [EXPORT_HEADER!1!date_exported],
  NULL AS  [EXPORT_HEADER!1!interface_name],
  NULL AS  [EXPORT_HEADER!1!parameters_used],
  NULL AS  [EXPORT_HEADER!1!total_transactions],
  NULL AS  [EXPORT_HEADER!1!total_amount],
  p.party_cnt   as [PARTY!2!party_cnt],
  NULL AS [PARTY!2!party_type_code],
  NULL AS [PARTY!2!source_code],
  NULL AS [PARTY!2!shortname],
  NULL AS [PARTY!2!name],
  NULL AS [PARTY!2!resolved_name],
  NULL AS [PARTY!2!currency_code],
  NULL AS [PARTY!2!language_code],
  NULL AS [PARTY!2!agent_code],
  NULL AS [PARTY!2!created_by_username],
  NULL AS [PARTY!2!date_created],
  NULL AS [PARTY!2!last_modified],
  NULL AS [PARTY!2!modified_by_username],
  NULL AS [PARTY!2!payment_method_code],
  NULL AS [PARTY!2!file_code],
  NULL AS [PARTY!2!is_prospect],
  NULL AS [PARTY!2!loyalty_number],
  NULL AS [PARTY!2!alternative_identifier],
  NULL AS [PARTY!2!trading_name],
  NULL AS [PARTY!2!sub_branch_code],
  NULL AS [PARTY!2!tax_number],
  NULL AS [PARTY!2!domiciled_for_tax],
  NULL AS [PARTY!2!tax_exempt],
  NULL AS [PARTY!2!tax_percentage],
  NULL AS [PARTY!2!blacklist_reason_code],
  NULL AS [PARTY!2!party_title_code],
  NULL AS [PARTY!2!forename],
  NULL AS [PARTY!2!initials],
  NULL AS [PARTY!2!employment_status_code],
  NULL AS [PARTY!2!employer_business],
  NULL AS [PARTY!2!secondary_employer_business],
  NULL AS [PARTY!2!secondary_employment_status_code],
  NULL AS [PARTY!2!marital_status_code],
  NULL AS [PARTY!2!number_of_children],
  NULL AS [PARTY!2!Nationality_id],
  NULL AS [PARTY!2!country_of_origin_code],
  NULL AS [PARTY!2!mailshot],
  NULL AS [PARTY!2!is_pet_owner],
  NULL AS [PARTY!2!accommodation_type_code],
  NULL AS [PARTY!2!date_of_birth],
  NULL AS [PARTY!2!gender_code],
  NULL AS [PARTY!2!occupation_code],
  NULL AS [PARTY!2!secondary_occupation_code],
  NULL AS [PARTY!2!is_smoker],
  NULL AS [PARTY!2!company_reg],
  NULL AS [PARTY!2!trading_since_date],
  NULL AS [PARTY!2!party_business_code],
  NULL AS [PARTY!2!location],
  NULL AS [PARTY!2!no_of_offices],
  NULL AS [PARTY!2!no_of_employees],
  NULL AS [PARTY!2!financial_year],
  NULL AS [PARTY!2!trade_code],
  NULL AS [PARTY!2!wage_roll],
  NULL AS [PARTY!2!trade_id],
  NULL AS [PARTY!2!party_group_type_code],
  NULL AS [PARTY!2!is_registered_charity],
  NULL AS [PARTY!2!charity_number],
  NULL AS [PARTY!2!number_of_members],
  NULL AS [PARTYBUILDER!3!!CDATA],
  NULL AS  [PARTYADDRESS!4!party_address_cnt],
  NULL AS  [PARTYADDRESS!4!party_address_type],
  NULL AS  [PARTYADDRESS!4!party_address1],
  NULL AS  [PARTYADDRESS!4!party_address2],
  NULL AS  [PARTYADDRESS!4!party_address3],
  NULL AS  [PARTYADDRESS!4!party_address4],
  NULL AS  [PARTYADDRESS!4!party_postal_code],
  NULL AS  [PARTYADDRESS!4!party_country_code],
  NULL AS  [PARTYCONTACT!5!contact_cnt],
  NULL AS  [PARTYCONTACT!5!contact_type_code],
  NULL AS  [PARTYCONTACT!5!description],
  NULL AS  [PARTYCONTACT!5!detail],
  NULL AS  [PARTYCONTACT!5!extension],
  insfile.insurance_file_cnt AS  [POLICY!6!insurance_file_cnt],
  NULL AS  [POLICY!6!insurance_file_status_code],
  NULL AS  [POLICY!6!insurance_ref],
  NULL AS  [POLICY!6!insurance_folder_cnt],
  NULL AS  [POLICY!6!product_code],
  NULL AS  [POLICY!6!lead_insurer_code],
  NULL AS  [POLICY!6!lead_agent_code],
  NULL AS  [POLICY!6!currency_code],
  NULL AS  [POLICY!6!language_code],
  NULL AS  [POLICY!6!date_issued],
  NULL AS  [POLICY!6!cover_start_date],
  NULL AS  [POLICY!6!expiry_date],
  NULL AS  [POLICY!6!renewal_date],
  NULL AS  [POLICY!6!renewal_frequency_code],
  NULL AS [POLICY!6!is_referred_at_renewal],
  NULL AS  [POLICY!6!policy_version],
  NULL AS  [POLICY!6!Analysis_code],
  NULL AS  [POLICY!6!proposal_date],
  NULL AS  [POLICY!6!Policy_type_code],
  NULL AS  [POLICY!6!annual_premium],
  NULL AS  [POLICY!6!this_premium],
  NULL AS  [POLICY!6!net_premium],
  NULL AS [POLICY!6!commission_amount],
  NULL AS  [POLICY!6!iptable_amount],
  NULL AS  [POLICY!6!ipt_percentage],
  NULL AS [POLICY!6!is_ipt_overridden],
  NULL AS  [POLICY!6!tax_amount],
  NULL AS  [POLICY!6!vatable_amount],
  NULL AS  [POLICY!6!vat_percentage],
  NULL AS  [POLICY!6!vat_amount],
  NULL AS [POLICY!6!alternate_reference],
  NULL AS [POLICY!6!is_client_invoiced],
  NULL AS [POLICY!6!quote_expiry_date],
  NULL AS  [POLICY!6!policy_status_code],
  NULL AS [POLICY!6!inception_date_tpi],
  NULL AS  [POLICY!6!discount_reason_code],
  NULL AS [POLICY!6!discounted_premium],
  NULL AS [POLICY!6!discount_percentage],
  NULL AS [POLICY!6!match_discounted_premium_flag],
  NULL AS  [POLICY!6!Country_code],
  NULL AS  [POLICY!6!inception_Date],
  NULL AS [POLICY!6!policy_documents_issued_date],
  NULL AS [POLICY!6!policy_documents_correct],
  NULL AS [POLICY!6!insurance_file_type_code],
  risk.risk_cnt AS [RISK!7!risk_cnt],
  'NO RISK DATA FOUND' AS [RISKDATA!8!!CDATA],
  ratsec.rating_section_id AS [RATING_SECTION!9!rating_section_id],
  ratsec.sequence_number AS [RATING_SECTION!9!sequence],
  rtrim(ratsectype.code) AS [RATING_SECTION!9!rating_section_type_code],
  ratsec.sum_insured AS [RATING_SECTION!9!sum_insured],
  ratsec.annual_premium AS [RATING_SECTION!9!annual_premium],
  ratsec.this_premium AS [RATING_SECTION!9!this_premium],
  ratsec.original_flag AS [RATING_SECTION!9!original_flag],
  ratsec.calculated_premium AS [RATING_SECTION!9!calculated_premium],
  rtrim(earnpatrn.code) AS [RATING_SECTION!9!earning_pattern_code]

 FROM party p

	INNER JOIN party_type ON
	p.party_type_id = party_type.party_type_id

	INNER JOIN insurance_file AS insfile ON
	p.party_cnt = insfile.insured_cnt

	INNER JOIN insurance_folder AS insfol ON
	insfile.insurance_folder_cnt = insfol.insurance_folder_cnt

	INNER JOIN insurance_file_risk_link AS ifrl ON
	insfile.insurance_file_cnt = ifrl.insurance_file_cnt

	INNER JOIN risk ON
	ifrl.risk_cnt = risk.risk_cnt

	INNER JOIN rating_section AS ratsec ON
	risk.risk_cnt = ratsec.risk_cnt

	LEFT JOIN rating_section_type AS ratsectype ON
	ratsectype.rating_section_type_id = ratsec.rating_section_type_id

	LEFT JOIN policy_section_type AS polsectype ON
	polsectype.policy_section_type_id = ratsec.policy_section_type_id

	LEFT JOIN rate_type AS ratetype ON
	ratetype.rate_type_id = ratsec.rate_type_id

	LEFT JOIN earning_pattern AS earnpatrn ON
	earnpatrn.earning_pattern_id = ratsec.earning_pattern_id

	LEFT JOIN insurance_file_type AS insfiletype ON
	insfiletype.insurance_file_type_id = insfile.insurance_file_type_id

  WHERE (party_type.code = 'PC' OR party_type.code = 'GC' OR party_type.code = 'CC')
    AND Risk.is_risk_selected = 1
	AND insfile.batch_id = @batch_id

 ORDER BY
  [PARTY!2!party_cnt],
  [PARTYBUILDER!3!!CDATA],
  [PARTYADDRESS!4!party_address_cnt],
  [PARTYCONTACT!5!contact_cnt],
  [POLICY!6!insurance_file_cnt],
  [RISK!7!risk_cnt],
  [RISKDATA!8!!CDATA],
  [RATING_SECTION!9!rating_section_id]

  FOR XML EXPLICIT
  --Added particulary for PM055377 (Requirement: Once policy is exported in Written Status, 
  --it should be re-exported when made live. Hence the batch_id of written policy is set to NULL after export 
  --so that it can be exported again when made live)
  update insurance_file set batch_id=null where batch_id=@batch_id and insurance_file_type_id=11