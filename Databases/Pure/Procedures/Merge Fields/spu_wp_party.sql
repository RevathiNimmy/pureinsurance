SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_party  
GO

--PN6556 Expand trading name to 255 characters  
CREATE PROCEDURE spu_wp_party  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @party_type_code VARCHAR(10) OUTPUT,  
    @party_type VARCHAR(255) OUTPUT,  
    @shortname VARCHAR(20) OUTPUT,  
    @name VARCHAR(60) OUTPUT,  
    @resolved_name VARCHAR(387) OUTPUT,  
    @currency_code VARCHAR(255) OUTPUT,  
    @collect_type VARCHAR(255) OUTPUT,  
    @accum_treatment_type VARCHAR(255) OUTPUT,  
    @stats_treatment_type VARCHAR(255) OUTPUT,  
    @party_category VARCHAR(255) OUTPUT,  
    @agent_name VARCHAR(100) OUTPUT,  
    @consultant_name VARCHAR(100) OUTPUT,  
    @consultant_logic VARCHAR(50) OUTPUT,  
    @created_by VARCHAR(100) OUTPUT,  
    @date_created DATETIME OUTPUT,  
    @last_modified DATETIME OUTPUT,  
    @modified_by VARCHAR(100) OUTPUT,  
    @payment_method_code VARCHAR(255) OUTPUT,  
    @payment_term_code VARCHAR(255) OUTPUT,  
    @credit_card_code VARCHAR(70) OUTPUT,  
    @file_code VARCHAR(50) OUTPUT,  
    @abc_count INT OUTPUT,  
    @statements TINYINT OUTPUT,  
    @reminder_type VARCHAR(255) OUTPUT,  
    @renewals TINYINT OUTPUT,  
    @status VARCHAR(6) OUTPUT,  
    @last_action_type VARCHAR(20) OUTPUT,  
    @is_travel_agent TINYINT OUTPUT,  
    @is_prospect TINYINT OUTPUT,  
    @is_deleted TINYINT OUTPUT,  
    @abi_code_on_406 VARCHAR(4) OUTPUT,  
    @abi_code_on_81 VARCHAR(3) OUTPUT,  
    @abi_codelist VARCHAR(6) OUTPUT,  
    @area VARCHAR(255) OUTPUT,  
    @invariant_key INT OUTPUT,  
    @record_status VARCHAR(5) OUTPUT,  
    @CCJs INT OUTPUT,  
    @service_level_logic VARCHAR(50) OUTPUT,  
    @loyalty_number VARCHAR(20) OUTPUT,  
    @trading_name VARCHAR(255) OUTPUT,  
    @source_id VARCHAR(255) OUTPUT,  
    @sub_branch_id VARCHAR(255) OUTPUT,  
    @payment_terms VARCHAR(7) OUTPUT,  
    @service_level_description VARCHAR(255) OUTPUT,  
    @alternative_identifier VARCHAR(15) OUTPUT,   -- PN7732  
    @tob_letter DATETIME OUTPUT,  
    @tax_number VARCHAR(50) OUTPUT,  
    @domiciled_for_tax TINYINT OUTPUT,  
    @tax_exempt TINYINT OUTPUT,  
    @tax_percentage FLOAT OUTPUT,  
    @seasonal_gift VARCHAR(255) OUTPUT,  
    @branch_address1 VARCHAR(40) OUTPUT,  
    @branch_address2 VARCHAR(40) OUTPUT,  
    @branch_address3 VARCHAR(40)OUTPUT,  
    @branch_address4 VARCHAR(40)OUTPUT,  
    @branch_postal_code VARCHAR(20)OUTPUT  
  
AS  
BEGIN  
  
--DC210301 added consultant logic and service level logic  
    SELECT  
        @party_type_code = pt.code,  
        @party_type = pt.description,  
        @shortname = p.shortname,  
        @name = p.name,  
        @resolved_name = p.resolved_name,  
        @currency_code = c.description,  
        @collect_type = ct.description,  
        @accum_treatment_type = att.description,  
        @stats_treatment_type = stt.description,  
        @party_category = pc.description,  
        @agent_name = p1.resolved_name,  
        @consultant_name = p2.resolved_name,  
        @consultant_logic = p2.shortname,  
        @created_by = u1.username,  
        @date_created = p.date_created,  
        @last_modified = p.last_modified,  
        @modified_by = u2.username,  
        @payment_method_code = p.payment_method_code,  
        @payment_term_code = pff.description,  
        @credit_card_code = p.credit_card_code,  
        @file_code = p.file_code,  
        @abc_count = p.abc_count,  
        @statements = p.statements,  
        @reminder_type = rt.description,  
        @renewals = p.renewals,  
        @status = p.status,  
        @last_action_type = p.last_action_type,  
        @is_travel_agent = p.is_travel_agent,  
        @is_prospect = p.is_prospect,  
        @is_deleted = p.is_deleted,  
        @abi_code_on_406 = p.abi_code_on_406,  
        @abi_code_on_81 = p.abi_code_on_81,  
        @abi_codelist = p.abi_codelist,  
        @area = a.description,  
        @invariant_key = p.invariant_key,  
        @record_status = p.record_status,  
        @CCJs = p.CCJs,  
        @service_level_logic = sl.code,  
        @loyalty_number = p.loyalty_number,  
        @trading_name = p.name,  
        @source_id = s.description,  
        @sub_branch_id = sb.description,  
        @payment_terms = p.payment_term_code,  
        @service_level_description = sl.description,  
        @alternative_identifier = p.alternative_identifier, -- PN7732  
        @tob_letter = p.tob_letter,  
        @tax_number = p.tax_number,  
        @domiciled_for_tax = p.domiciled_for_tax,  
        @tax_exempt  = p.tax_exempt,  
        @tax_percentage = p.tax_percentage,  
        @seasonal_gift = sg.description,  
     @branch_address1 = s.address1 ,  
     @branch_address2 = s.address2 ,  
     @branch_address3 = s.address3 ,  
     @branch_address4 = s.address4 ,  
     @branch_postal_code = s.postal_code  
    FROM  
        party p  
        LEFT OUTER JOIN party_type pt  
            ON p.party_type_id = pt.party_type_id  
        LEFT OUTER JOIN currency c  
            ON p.currency_id = c.currency_id  
        LEFT OUTER JOIN collect_type ct  
            ON p.collect_type_id = ct.collect_type_id  
        LEFT OUTER JOIN accum_treatment_type att  
            ON p.accum_treatment_type_id = att.accum_treatment_type_id  
        LEFT OUTER JOIN stats_treatment_type stt  
            ON p.stats_treatment_type_id = stt.stats_treatment_type_id  
        LEFT OUTER JOIN party_category pc  
            ON p.party_category_id = pc.party_category_id  
        LEFT OUTER JOIN party p1  
            ON p.agent_cnt = p1.party_cnt  
        LEFT OUTER JOIN party p2  
            ON p.consultant_cnt = p2.party_cnt  
        LEFT OUTER JOIN PMUser u1  
            ON p.created_by_id = u1.user_id  
        LEFT OUTER JOIN PMUser u2  
            ON p.modified_by_id = u2.user_id  
        LEFT OUTER JOIN reminder_type rt  
            ON p.reminder_type_id = rt.reminder_type_id  
        LEFT OUTER JOIN area a  
            ON p.area_id = a.area_id  
        LEFT OUTER JOIN service_level sl  
            ON p.service_level_id = sl.service_level_id  
        LEFT OUTER JOIN source s  
            ON p.source_id = s.source_id  
        LEFT OUTER JOIN sub_branch sb  
            ON p.sub_branch_id = sb.sub_branch_id  
        LEFT OUTER JOIN seasonal_gift sg  
            ON p.seasonal_gift_id = sg.seasonal_gift_id  
  LEFT OUTER JOIN pffrequency pff ON  
   p.payment_term_code = pff.pffrequency_id  
   --Start (Girija Chokkalingam)- PN 54018  
   where p.party_cnt = @PartyCnt  
   --End (Girija Chokkalingam)- PN 54018  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

