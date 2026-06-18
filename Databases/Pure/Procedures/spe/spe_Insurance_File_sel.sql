
EXECUTE DDLDROPPROCEDURE 'spe_Insurance_File_Sel'

GO
CREATE PROCEDURE spe_Insurance_File_sel    
    @insurance_file_cnt int    
AS    
    DECLARE @tax_value     MONEY,
            @tax_value1    MONEY,
            @tax_value2    MONEY,
            @fee_value     MONEY,
            @policyoptions BIT,
            @LevyTax       MONEY,
			@mediaType     VARCHAR(25)='' 
    
    SELECT @policyoptions = value
    FROM   system_options
    WHERE  branch_id = 1
           AND option_number = 5021
    
    IF @policyoptions = 1
 BEGIN    
   -- Get tax    
          SELECT @tax_value1 = SUM(value)
          FROM   Tax_Calculation WITH(NOLOCK)
          WHERE  insurance_file_cnt = @insurance_file_cnt
                 AND risk_cnt IS NULL
                 AND transtype IN ( 'TTR', 'TTF', 'TTIF' )
    
          SELECT @tax_value2 = SUM(value)
          FROM   Tax_Calculation rt WITH(NOLOCK)
                 JOIN insurance_file_risk_link ifrl WITH(NOLOCK)
                   ON ifrl.risk_cnt = rt.risk_cnt
                 JOIN risk r WITH(NOLOCK)
                   ON r.risk_cnt = rt.risk_cnt
          WHERE  ifrl.insurance_file_cnt = @insurance_file_cnt
                 AND ifrl.status_flag <> 'U'
                 AND r.is_risk_selected = 1
                 AND rt.risk_cnt IS NOT NULL
                 AND transtype IN ( 'TTR', 'TTF', 'TTIF' )
    
          SELECT @tax_value = ISNULL(@tax_value1, 0)
                              + ISNULL(@tax_value2, 0)
    
  -- Get fee    
          SELECT @fee_value = SUM(currency_amount)
          FROM   policy_fee_u WITH(NOLOCK)
          WHERE  insurance_file_cnt = @insurance_file_cnt
    
    -- Get Levy Tax if any    
          SELECT @LevyTax = SUM(this_premium)
          FROM   peril WITH(NOLOCK)
                 INNER JOIN Peril_type pt WITH(NOLOCK)
                         ON peril.Peril_type_id = pt.Peril_type_id
                 INNER JOIN Insurance_file_risk_link ifrl WITH(NOLOCK)
                         ON ifrl.risk_cnt = peril.risk_cnt
          WHERE  ifrl.insurance_file_cnt = @insurance_file_cnt
                 AND pt.is_levy_tax = 1
      END
    
	SELECT @mediaType=ISNULL(m.description,'') FROM PFPremiumFinance pf WITH(NOLOCK) JOIN
      PFScheme ps WITH(NOLOCK) ON pf.SchemeNo =ps.SchemeNo AND pf.SchemeVersion=ps.SchemeVersion
      join MediaType m WITH(NOLOCK) ON ps.mediatype_id=m.mediatype_id
	  WHERE Insurance_File_Cnt= @insurance_file_cnt
    
 -- Return data    
    SELECT insurance_file_cnt,
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
     ifile.business_type_id,    
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
     ifile.payment_method,    
     user_defined_data_id,    
     commission_percentage,    
     invariant_key,    
     insured_name,    
     alternate_reference,    
     is_client_invoiced,    
     old_policy_number,    
     quote_expiry_date,    
     alternate_account_cnt,    
     account_executive_cnt,    
     anniversary_date,    
     policy_style_id,    
     underwriting_year_id,    
     policy_status_id,    
     inception_date_tpi,    
     fsa_customer_category_id,    
     fsa_contract_location_id,    
     fsa_underwriter_cnt,    
     fsa_type_of_sale_id,    
     fsa_renewal_consent,    
     base_currency_id,    
     country_id,    
     discount_reason_id,    
     discounted_premium,    
     discount_percentage,    
     match_discounted_premium_flag,    
     put_on_next_instalment_renewal,    
     anniversary_copy,    
     discount_recurring_type_id,    
     lead_allow_consolidated_commission,    
     sub_allow_consolidated_commission,    
     balance_type,    
     intermediary_agent_account_id,    
	ISNULL(@tax_value, 0) + ISNULL(@fee_value, 0)          + ISNULL(@LevyTax, 0) fees_taxes,
     terms_agreed,    
     terms_agreed_date,    
     inception_date,    
     policy_documents_issued_date,    
     policy_documents_correct,    
     error_notification_date,    
     policy_deductibles_id,    
     policy_limits_id,    
     risk_transfer_agreement,    
     renewal_premium,    
     renewal_product_id,    
     original_product_id,    
     risk_transfer_editable,    
     currency_base_xrate,    
	party_agent_type.code AS Party_agent_Type_code,
	business_type.code    business_type_code,
     manual_discount_percentage,    
     base_insurance_folder_cnt,    
     quote_version,    
     quote_status_id,    
     Contact_user_id,
	coins_placement,
	MTA_reason_id,
	is_marketplace_policy,
	CollectionFrequency_id,
	DOPaymentTerms_id,
	Correspondence_Type,
	Default_Preferred_Correspondence,
	Is_Agent_Correspondence,
	 CASE WHEN @mediaType='' THEN ifile.payment_method
     ELSE @mediaType  END Media_type,
	 sender_email,
	 receiver_email,
     original_insurance_file_type_id	 
    
    
    FROM   Insurance_File ifile WITH (NOLOCK)
           LEFT JOIN party_agent
                  ON ifile.lead_agent_cnt = party_agent.party_cnt
           LEFT JOIN party_agent_type
                  ON party_agent.party_agent_type_id = party_agent_type.party_agent_type_id
LEFT JOIN business_type ON    
 ifile.business_type_id = business_type.business_type_id    
 WHERE    
     insurance_file_cnt = @insurance_file_cnt 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
