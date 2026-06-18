SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Product_saa'
GO
CREATE PROCEDURE spe_Product_saa
AS  
  
SELECT  
    product_id,  
    caption_id,  
    code,  
    description,  
    effective_date,  
    is_deleted,  
    scheme_agency_ref,  
    block_no,  
    is_tax_suppressed,  
    quote_auto_numbering_id,  
    is_short_period_rated,  
    is_midnight_renewal,  
    is_auto_renewable,  
    renewal_period,  
    policy_auto_numbering_id,  
    prov_claim_auto_numbering_id,  
    full_claim_auto_numbering_id,  
    is_accumulation,  
    ri_pointer,  
    report_pointer,  
    claim_year_to_check,  
    max_single_claim_value,  
    max_number_of_claim,  
    max_total_claim_value,  
    nb_prorata,  
    mta_prorata,  
    round_prem_to_nearest_unit,  
    rounding_section_id,  
    is_policy_number_at_quote,  
    change_policy_number_at_renewal,  
    hide_summary_at_renewal_acceptance, 
	use_nb_payment_term_at_renselection,
	Quote_all_risk_NB,
	Quote_all_risk_MTC,
	Quote_all_risk_MTA,
	Auto_Renew_BDMPolicy,
	is_retain_policy_number_on_copy,
	Anniversary_Date_Editable,
	disable_cover_start_date_on_REN

FROM Product  
  
ORDER BY product_id ASC  

GO

