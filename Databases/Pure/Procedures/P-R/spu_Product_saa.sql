SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Product_saa'
GO


CREATE PROCEDURE spu_Product_saa
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
    use_nb_payment_term_at_renselection,
    use_prior_term_scheme_at_ren   
    
 FROM Product    
    
ORDER BY description ASC 
GO


