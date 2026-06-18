
SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Insurance_File_sel'
GO

CREATE PROCEDURE spu_SAM_Insurance_File_sel  
    @insurance_file_cnt int  
AS  
  
DECLARE  
     @tax_value money,  
     @tax_value1 money,  
     @tax_value2 money,  
     @fee_value money,  
     @policyoptions bit,  
     @LevyTax money  
  
 SELECT @policyoptions = value FROM system_options WHERE branch_id = 1 AND option_number = 5021  
  
 if @policyoptions=1  
 BEGIN  
  
   -- Get tax  
 SELECT  @tax_value1 = SUM(value)  
 FROM    Tax_Calculation  
 WHERE   insurance_file_cnt = @insurance_file_cnt  
 AND  risk_cnt IS NULL  
 AND  transtype in ('TTR','TTF','TTIF')  
  
  SELECT  @tax_value2 = SUM(value)  
  FROM    Tax_Calculation rt  
  JOIN    insurance_file_risk_link ifrl      ON ifrl.risk_cnt = rt.risk_cnt  
  JOIN    risk r                             ON r.risk_cnt = rt.risk_cnt  
  WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
  AND     ifrl.status_flag <> 'U'  
  AND     r.is_risk_selected = 1  
  AND  rt.risk_cnt IS NOT NULL  
  AND  transtype in ('TTR','TTF','TTIF')  
  
  SELECT  @tax_value = ISNULL(@tax_value1, 0) + ISNULL(@tax_value2, 0)  
  
  -- Get fee  
  SELECT  @fee_value = SUM(currency_amount)  
  FROM    policy_fee_u  
  WHERE   insurance_file_cnt = @insurance_file_cnt  
  
    -- Get Levy Tax if any  
  
    SELECT @LevyTax = SUM(this_premium) FROM peril  
    INNER JOIN Peril_type pt ON peril.Peril_type_id = pt.Peril_type_id  
    INNER JOIN Insurance_file_risk_link ifrl ON ifrl.risk_cnt = peril.risk_cnt  
    WHERE ifrl.insurance_file_cnt = @insurance_file_cnt  
    AND  pt.is_levy_tax = 1  
  
 End  
 -- Return data  
  
 SELECT  
     ifile.insurance_file_cnt,  
     insurance_file_structure_id,  
     ifile.insurance_file_type_id,  
     ifile.insurance_file_status_id,  
     insurance_file_id,  
     ifile.source_id,  
     ifile.insurance_folder_cnt,  
     insurance_ref,  
     ifile.product_id,  
     lead_insurer_cnt,  
     lead_agent_cnt,  
     lead_agent_percent,  
     account_handler_cnt,  
     insured_cnt,  
     ifile.business_type_id,  
     ifile.collect_type_id,  
     collection_from_cnt,  
     branch_id,  
     ifile.currency_id,  
     ifile.language_id,  
     date_issued,  
     cover_start_date,  
     expiry_date,  
     renewal_date,  
     ifile.renewal_method_id,  
     ifile.renewal_frequency_id,  
     is_referred_at_renewal,  
     ifile.lapsed_reason_id,  
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
     ifile.Analysis_code_id,  
     proposal_date,  
     diary_date,  
     review_date,  
     renewal_day_number,  
     ifile.Policy_type_id,  
     indicator,  
     clause,  
     cover,  
     area,  
     long_term_undertaking_date,  
     ifile.renewal_stop_code_id,  
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
     ifile.payment_method,  
     ifile.user_defined_data_id,  
     commission_percentage,  
     ifile.invariant_key,  
     insured_name,  
     alternate_reference,  
     is_client_invoiced,  
     old_policy_number,  
     quote_expiry_date,  
     alternate_account_cnt,  
     account_executive_cnt,  
     anniversary_date,  
     ifile.policy_style_id,  
     ifile.underwriting_year_id,  
     ifile.policy_status_id,  
     inception_date_tpi,  
     fsa_customer_category_id,  
     fsa_contract_location_id,  
     fsa_underwriter_cnt,  
     fsa_type_of_sale_id,  
     fsa_renewal_consent,  
     ifile.base_currency_id,  
     ifile.country_id,  
     discount_reason_id,  
     discounted_premium,  
     discount_percentage,  
     match_discounted_premium_flag,  
     put_on_next_instalment_renewal,  
     anniversary_copy,  
     discount_recurring_type_id,  
     ifile.lead_allow_consolidated_commission,  
     ifile.sub_allow_consolidated_commission,  
     balance_type,  
     intermediary_agent_account_id,  
     ISNULL(@tax_value,0) + ISNULL(@fee_value,0) + ISNULL(@LevyTax,0)  fees_taxes,  
     terms_agreed,  
     terms_agreed_date,  
     ifile.inception_date,  
     policy_documents_issued_date,  
     policy_documents_correct,  
     error_notification_date,  
     ifile.policy_deductibles_id,  
     ifile.policy_limits_id,  
     risk_transfer_agreement,  
     renewal_premium,  
     renewal_product_id,  
     original_product_id,  
     risk_transfer_editable,  
     currency_base_xrate,  
     RTRIM(party_agent_type.code) as Party_agent_Type_code,  
     RTRIM(business_type.code) business_type_code,  
     manual_discount_percentage,  
     base_insurance_folder_cnt,  
     quote_version,  
     quote_status_id,  
     Contact_user_id,  
     MTA_reason_id,
     RTRIM(p.code) product_code,
     RTRIM(source.code) source_code,
     RTRIM(Sub_Branch.code) sub_Branch_code,	 
     insurance_file_type.code insurance_file_type_code,
     insurance_file_status.code insurance_file_status_code,
     ifile.payment_method payment_method_code,
     currency.code currency_code,
     ag.shortname agent_code,
     ag.name agent_name,
     p.description product_name,
     RTRIM(policy_status.code) policy_status_code,
     policy_type.code policy_type_code,
     ifs.last_trans_description,
     insured.name insured_name,
     ifo.renewal_count,
     RTRIM(Renewal_Frequency.code) Renewal_Frequency_code ,
     RTRIM(Renewal_stop_code.code) Renewal_stop_code_code,
     RTRIM(Renewal_Method.code) Renewal_Method_code,
     RTRIM(Lapsed_Reason.code) Lapsed_Reason_code,
     RTRIM(Policy_Style.code) Policy_Style_code,
     ah.resolved_name AccountHandlerName,
     ah.shortname AccountHandlerCode,
     policy_Deductibles.code policy_Deductibles_code,
     Policy_Limits.code Policy_Limits_code,
     underwriting_year.code underwriting_year_code,
     marked_for_collection,
     ifo.description InsuranceFolderDescription,
     RTRIM(Analysis_Code.code) Analysis_Code_code,
	 ifile.is_marketplace_policy,
     ISNULL(ifile.coins_placement,0) as coins_placement,
     ISNULL(CollectionFrequency_id,0) as CollectionFrequency_id ,   
     ISNULL(DOPaymentTerms_id,0) as DOPaymentTerms_id,
	 RTRIM(source.description) source_desc,
     ifile.Correspondence_Type,
	 ifile.Default_Preferred_Correspondence,
	 ifile.Is_Agent_Correspondence,
	 ifile.Sender_Email,
	 ifile.Receiver_Email  
 FROM
 Insurance_File  ifile  WITH (NOLOCK)

JOIN Insurance_Folder ifo	ON
	ifile.insurance_folder_cnt=ifo.insurance_folder_cnt
	
JOIN Insurance_File_System ifs	ON 
	ifile.insurance_file_cnt=ifs.insurance_file_cnt

 JOIN party insured ON  
    ifile.insured_cnt = insured.party_cnt  
     
 LEFT JOIN party ag ON  
    ifile.lead_agent_cnt = ag.party_cnt  
    
  LEFT JOIN party_agent  ON  
    ifile.lead_agent_cnt = party_agent.party_cnt  
  
 LEFT JOIN party_agent_type ON  
  party_agent.party_agent_type_id = party_agent_type.party_agent_type_id  
   -- End (Arul Stephen) - (Tech Spec - UIICWR6 -Policy Get Bank Guarantee .doc)  
  
LEFT JOIN business_type ON  
 ifile.business_type_id = business_type.business_type_id  

LEFT JOIN Product p ON
  ifile.product_id = p.product_id
LEFT JOIN source ON
  ifile.source_id = source.source_id 
  
LEFT JOIN Sub_Branch ON
  ifile.branch_id = Sub_Branch.Sub_branch_id
  
LEFT JOIN Insurance_File_Type ON
  ifile.insurance_file_type_id = Insurance_File_Type.insurance_file_type_id
  
 LEFT JOIN Insurance_File_Status ON
  ifile.insurance_file_status_id = insurance_file_status.insurance_file_status_id

 LEFT JOIN Currency ON
  ifile.currency_id= Currency.currency_id
  
  LEFT JOIN policy_status ON
  ifile.policy_status_id= policy_status.policy_status_id
  
   LEFT JOIN policy_type ON
  ifile.Policy_type_id= policy_type.policy_type_id
  
  LEFT JOIN Renewal_Frequency ON
  ifile.renewal_frequency_id= Renewal_Frequency.renewal_frequency_id
  
    LEFT JOIN Renewal_stop_code ON
  ifile.renewal_stop_code_id= Renewal_stop_code.renewal_stop_code_id
  
      LEFT JOIN Renewal_Method ON
  ifile.renewal_method_id= Renewal_Method.renewal_method_id
   
         LEFT JOIN Lapsed_Reason ON
  ifile.lapsed_reason_id= Lapsed_Reason.lapsed_reason_id
   
            LEFT JOIN Policy_Style ON
  ifile.policy_style_id= Policy_Style.policy_style_id
  
              LEFT JOIN party ah  ON
  ifile.account_handler_cnt= ah.party_cnt
  
                LEFT JOIN Policy_Deductibles ON
  ifile.Policy_Deductibles_id= Policy_Deductibles.policy_Deductibles_id
                  
                  LEFT JOIN Policy_Limits ON
  ifile.Policy_limits_id= Policy_Limits.policy_Limits_id
  
                    LEFT JOIN Underwriting_Year ON
  ifile.underwriting_year_id= Underwriting_Year.underwriting_year_id
  
                      LEFT JOIN Analysis_Code ON
  ifile.Analysis_code_id= Analysis_Code.analysis_code_id  
 WHERE  
     ifile.insurance_file_cnt = @insurance_file_cnt  
     

GO