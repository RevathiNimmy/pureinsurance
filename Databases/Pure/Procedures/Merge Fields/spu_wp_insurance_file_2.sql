SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_insurance_file_2
GO

CREATE PROCEDURE spu_wp_insurance_file_2
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @risk_code VARCHAR(255) OUTPUT,  
    @risk_code_logic VARCHAR(50) OUTPUT,  
    @analysis_code VARCHAR(255) OUTPUT,  
    @analysis_code_logic VARCHAR(50) OUTPUT,  
    @proposal_date DATETIME OUTPUT,  
    @diary_date DATETIME OUTPUT,  
    @review_date DATETIME OUTPUT,  
    @renewal_day_number INT OUTPUT,  
    @policy_type VARCHAR(255) OUTPUT,  
    @indicator VARCHAR(1) OUTPUT,  
    @clause VARCHAR(4) OUTPUT,  
    @cover VARCHAR(1) OUTPUT,  
    @area VARCHAR(1) OUTPUT,  
    @long_term_undertaking_date DATETIME OUTPUT,  
    @renewal_stop_code VARCHAR(255) OUTPUT,  
    @renewal_stop_code_logic VARCHAR(50) OUTPUT,  
    @vbs_type VARCHAR(2) OUTPUT,  
    @vbs_status VARCHAR(1) OUTPUT,  
    @is_insurer_rate_table TINYINT OUTPUT,  
    @is_related_policies TINYINT OUTPUT,  
    @is_retained_documents TINYINT OUTPUT,  
    @schemes_postcode VARCHAR(8) OUTPUT,  
    @paid_direct VARCHAR(1) OUTPUT,  
    @scheme VARCHAR(100) OUTPUT,  
    @brokerage_amount MONEY OUTPUT,  
    @is_minimum_brokerage_flag TINYINT OUTPUT,  
    @annual_premium MONEY OUTPUT,  
    @this_premium MONEY OUTPUT,  
    @net_premium MONEY OUTPUT,  
    @commission_amount MONEY OUTPUT,  
    @iptable_amount MONEY OUTPUT,  
    @ipt_percentage FLOAT OUTPUT,  
    @is_ipt_overridden TINYINT OUTPUT,  
    @tax_amount MONEY OUTPUT,  
    @vatable_amount MONEY OUTPUT,  
    @vat_percentage FLOAT OUTPUT,  
    @payment_method VARCHAR(60) OUTPUT,  
    @commission_percentage FLOAT OUTPUT,  
    @invariant_key INT OUTPUT,  
    @insured_name VARCHAR(255) OUTPUT,  
    @alternate_reference VARCHAR(80) OUTPUT,  
    @is_client_invoiced TINYINT OUTPUT,  
    @old_policy_number VARCHAR(30) OUTPUT,  
    @quote_expiry_date DATETIME OUTPUT,  
    @alternate_account VARCHAR(255) OUTPUT,  
    @lead_agent_address_1 VARCHAR(60) OUTPUT,  
    @lead_agent_address_2 VARCHAR(60) OUTPUT,  
    @lead_agent_address_3 VARCHAR(60) OUTPUT,  
    @lead_agent_address_4 VARCHAR(60) OUTPUT,  
    @lead_agent_address_5 VARCHAR(20) OUTPUT,  
    @lead_agent_country VARCHAR(20) OUTPUT,  
    @vat_amount NUMERIC(19,4) OUTPUT, --KN (CMG)  
    @cc_number VARCHAR(30) OUTPUT,  
    @cc_exipry_date VARCHAR(10) OUTPUT,  
    @cc_valid tinyint OUTPUT  
AS 
BEGIN
 
    SELECT  
        @lead_agent_address_1 = a.address1,  
        @lead_agent_address_2 = a.address2,  
        @lead_agent_address_3 = a.address3,  
        @lead_agent_address_4 = a.address4,  
        @lead_agent_address_5 = a.postal_code,  
        @lead_agent_country = c.description  
    FROM    
        insurance_file i
        INNER JOIN party_address_usage pau
            ON i.lead_agent_cnt = pau.party_cnt
        INNER JOIN address_usage_type aut
            ON pau.address_usage_type_id = aut.address_usage_type_id  
        INNER JOIN address a
            ON pau.address_cnt = a.address_cnt  
        INNER JOIN country c  
            ON a.country_id = c.country_id  
    WHERE   
        i.insurance_file_cnt = @InsuranceFileCnt  
        AND aut.code = '3131 XCO'  

    SELECT  
        @risk_code = rc.description,  
        @risk_code_logic = rc.code,  
        @analysis_code = ac.description,  
        @analysis_code_logic = ac.code,  
        @proposal_date = i.proposal_date,  
        @diary_date = i.diary_date,  
        @review_date = i.review_date,  
        @renewal_day_number = i.renewal_day_number,  
        @policy_type = pt.description,  
        @indicator = i.indicator,  
        @clause = i.clause,  
        @cover = i.cover,  
        @area = i.area,  
        @long_term_undertaking_date = i.long_term_undertaking_date,  
        @renewal_stop_code = rsc.description,  
        @renewal_stop_code_logic = rsc.code,  
        @vbs_type = i.vbs_type,  
        @vbs_status = i.vbs_status,  
        @is_insurer_rate_table = i.is_insurer_rate_table,  
        @is_related_policies = i.is_related_policies,  
        @is_retained_documents = i.is_retained_documents,  
        @schemes_postcode = i.schemes_postcode,  
        @paid_direct = i.paid_direct,  
        @scheme = (
                   SELECT scheme_desc 
                   FROM GIS_Scheme 
                   WHERE gis_scheme_id=i.scheme
                  ),  
        @brokerage_amount = i.brokerage_amount,  
        @is_minimum_brokerage_flag = i.is_minimum_brokerage_flag,  
        @annual_premium = i.annual_premium,  
        @this_premium = i.this_premium,  
        @net_premium = i.net_premium,  
        @commission_amount = i.commission_amount,  
        @iptable_amount = i.iptable_amount,  
        @ipt_percentage = i.ipt_percentage,  
        @is_ipt_overridden = i.is_ipt_overridden,  
        @tax_amount = i.tax_amount,  
        @vatable_amount = i.vatable_amount,  
        @vat_percentage = i.vat_percentage,  
        @payment_method = i.payment_method,  
        @commission_percentage = i.commission_percentage,  
        @invariant_key = i.invariant_key,  
        @insured_name = i.insured_name,  
        @alternate_reference = i.alternate_reference,  
        @is_client_invoiced = i.is_client_invoiced,  
        @old_policy_number = i.old_policy_number,  
        @quote_expiry_date = i.quote_expiry_date,  
        @alternate_account = p.resolved_name,  
        @vat_amount = i.vat_amount, --KN(CMG)  
        @cc_number = cli.cc_number,  
        @cc_exipry_date = cli.cc_expiry_date,  
        @cc_valid = i.cashlistitem_valid  
    FROM    
        insurance_file i
        LEFT OUTER JOIN risk_code rc
            ON i.risk_code_id = rc.risk_code_id 
        LEFT OUTER JOIN analysis_code ac
            ON i.analysis_code_id = ac.analysis_code_id    
        LEFT OUTER JOIN policy_type pt
            ON i.policy_type_id = pt.policy_type_id    
        LEFT OUTER JOIN renewal_stop_code rsc
            ON i.renewal_stop_code_id = rsc.renewal_stop_code_id    
        LEFT OUTER JOIN party p
            ON i.alternate_account_cnt = p.party_cnt    
        LEFT OUTER JOIN cashlistitem cli  
            ON i.cashlistitem_id=cli.cashlistitem_id  
    WHERE 
        i.insurance_file_cnt = @InsuranceFileCnt

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
