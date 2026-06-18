SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_insurance_file_1  
GO

-------------------------------------------------------------------------------
-- Changes
--
-- Added anniversary_date                                   AMB 22-Oct-2003
-- Added File_type											John White 6/Dec/2016
-------------------------------------------------------------------------------

CREATE PROCEDURE spu_wp_insurance_file_1
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @insurance_ref VARCHAR(30) OUTPUT,
    @product VARCHAR(255) OUTPUT,
    @lead_insurer VARCHAR(100) OUTPUT,
    @lead_insurer_logic VARCHAR(50) OUTPUT,
    @lead_agent VARCHAR(100) OUTPUT,
    @lead_agent_logic VARCHAR(50) OUTPUT,
    @lead_agent_percent FLOAT OUTPUT,
    @account_handler VARCHAR(100) OUTPUT,
    @acc_hand_logic VARCHAR(50) OUTPUT,
    @insured VARCHAR(255) OUTPUT,
    @business_type VARCHAR(255) OUTPUT,
    @collect_type VARCHAR(100) OUTPUT,
    @collection_from VARCHAR(255) OUTPUT,
    @branch VARCHAR(255) OUTPUT,
    @currency VARCHAR(255) OUTPUT,
    @language VARCHAR(255) OUTPUT,
    @date_issued DATETIME OUTPUT,
    @cover_start_date DATETIME OUTPUT,
    @expiry_date DATETIME OUTPUT,
    @renewal_date DATETIME OUTPUT,
    @renewal_method VARCHAR(255) OUTPUT,
    @renewal_frequency VARCHAR(255) OUTPUT,
    @is_referred_at_renewal TINYINT OUTPUT,
    @lapsed_reason VARCHAR(255) OUTPUT,
    @lapsed_date DATETIME OUTPUT,
    @lapsed_description VARCHAR(255) OUTPUT,
    @is_referred_on_mta TINYINT OUTPUT,
    @policy_version INT OUTPUT,
    @gemini_policy_status INT OUTPUT,
    @gemini_business_type INT OUTPUT,
    @deferred_ind INT OUTPUT,
    @policy_ignore INT OUTPUT,
    @broker VARCHAR(100) OUTPUT,
    @insured_name VARCHAR(255) OUTPUT,
    @lead_agent_commission MONEY OUTPUT,
    @anniversary_date DATETIME OUTPUT,
    @policy_status VARCHAR(255) OUTPUT,
    @currency_ISO_code VARCHAR(4) OUTPUT,
    @currency_symbol VARCHAR(4) OUTPUT,
    @InceptionDateTPI DATETIME OUTPUT,
    @UWYearCode VARCHAR(10) OUTPUT,
    @UWYearDesc VARCHAR(255) OUTPUT,
    @fsa_customer_category VARCHAR(255) OUTPUT,
    @fsa_contract_location VARCHAR(255) OUTPUT,
    @fsa_underwriter VARCHAR(255) OUTPUT,
    @fsa_type_of_sale VARCHAR(255) OUTPUT,
    @fsa_renewal_consent VARCHAR(255) OUTPUT,
    @fsa_terms_agreed VARCHAR(255) OUTPUT,
    @fsa_terms_agreed_date VARCHAR(255) OUTPUT,
    @fsa_inception_renewal_date VARCHAR(255) OUTPUT,
    @fsa_policy_documents_issued_date VARCHAR(255) OUTPUT,
    @fsa_policy_documents_correct VARCHAR(255) OUTPUT,
    @fsa_date_errors_notified_to_insurer VARCHAR(255) OUTPUT,
    @risk_transfer_agreement VARCHAR(3) OUTPUT,
    @quote_version INT OUTPUT,
	@File_type VARCHAR(255) OUTPUT
AS
BEGIN

        DECLARE @IsUnderwriting VARCHAR(20)

        SELECT @IsUnderwriting = Value
        FROM hidden_options
        WHERE branch_id = 1
            AND option_number = 1

        --PSL 25/09/2003 Issue 3317 commission percent is for commission table, also we just want the lead row
        SELECT
            @insurance_ref =
            CASE (
                  SELECT count(source_id)
                  FROM source
                  WHERE
                      source_id = i.source_id
                      AND source_id = i.source_id
                      AND underwriting_branch_ind =1
                      AND i.alternate_reference IS NOT NULL
                 )
                WHEN 0 THEN
                    i.insurance_ref
                ELSE
                    i.alternate_reference
            END,
            @product = pr.description,
            @lead_insurer = p1.resolved_name,
            @lead_insurer_logic = p1.shortname,
            @lead_agent = p2.resolved_name,
            @lead_agent_logic = p2.shortname,
            @lead_agent_percent = ac.commission_percentage,
            @account_handler = p3.resolved_name,
            @acc_hand_logic = p3.shortname,
            @insured = p4.resolved_name,
            @business_type = bt.description,
            @collect_type = ct.description,
            @collection_from = p5.resolved_name,
            @branch = b.description,
            @currency = c.description,
            @language = l.description,
            @date_issued = i.date_issued,
            @cover_start_date = i.cover_start_date,
            @expiry_date = i.expiry_date,
            @renewal_date = i.renewal_date,
            @renewal_method = rm.description,
            @renewal_frequency = rf.description,
            @is_referred_at_renewal = i.is_referred_at_renewal,
            @lapsed_reason = lr.description,
            @lapsed_date = i.lapsed_date,
            @lapsed_description = i.lapsed_description,
            @is_referred_on_mta = i.is_referred_on_mta,
            @policy_version = i.policy_version,
            @gemini_policy_status = i.gemini_policy_status,
            @gemini_business_type = i.gemini_business_type,
            @deferred_ind = i.deferred_ind,
            @policy_ignore = i.policy_ignore,
            @broker =p6.resolved_name,
            @insured_name = i.insured_name,
            @lead_agent_commission = ac.commission_value,
            @anniversary_date = i.anniversary_date,
            @currency_ISO_code = RTRIM(c.iso_code),
            @currency_symbol = RTRIM(c.symbol),
            @InceptionDateTPI = inception_date_tpi,
            @UWYearCode = UW.code,
            @UWYearDesc = UW.description,
            @fsa_customer_category =(
                                     SELECT
                                         CASE i.fsa_customer_category_id
                                             WHEN 1  THEN 'Retail'
                                             WHEN 0 THEN 'Commercial'
                                             ELSE ''
                                         END
                                    ),
            @fsa_contract_location =(
                                     SELECT
                                     CASE i.fsa_contract_location_id
                                         WHEN 0 THEN 'Non Distance'
                                         WHEN 1 THEN 'Distance with info provided'
                                         WHEN 2 THEN 'Distance - unable to provide info'
                                         ELSE ''
                                     END
                                    ),
            @fsa_underwriter = p7.resolved_name,
            @fsa_type_of_sale =(
                                SELECT
                                CASE i.fsa_type_of_sale_id
                                    WHEN 0 THEN 'Non Advised'
                                    WHEN 1 THEN 'Advised'
                                    ELSE ''
                                END
                               ),
            @fsa_renewal_consent =(
                                   SELECT
                                   CASE i.fsa_renewal_consent
                                       WHEN 1  THEN 'Yes'
                                       WHEN 0 THEN 'No'
                                       ELSE ''
                                   END
                                  ),
            @fsa_terms_agreed = (
                                 SELECT
                                 CASE i.terms_agreed
                                     WHEN 1  THEN 'Yes'
                                     WHEN 0 THEN 'No'
                                     ELSE ''
                                 END
                                ),
            @fsa_terms_agreed_date = i.terms_agreed_date,
            @fsa_inception_renewal_date = i.inception_date,
            @fsa_policy_documents_issued_date = i.policy_documents_issued_date,
            @fsa_policy_documents_correct =(
                                            SELECT
                                   CASE i.policy_documents_correct
                                                WHEN 1  THEN 'Yes'
                                                ELSE 'No'
                                            END
                                           ),
            @fsa_date_errors_notified_to_insurer = i.error_notification_date,
            @risk_transfer_agreement= (
                                       SELECT
                                       CASE i.risk_transfer_agreement
                                           WHEN 1 THEN 'Yes'
                                           ELSE 'No'
                                       END
                                      ) ,
             @quote_version = i.quote_version
        FROM
            insurance_file i
            LEFT OUTER JOIN product pr
                ON i.product_id = pr.product_id
            LEFT OUTER JOIN party p1
                ON i.lead_insurer_cnt = p1.party_cnt
            LEFT OUTER JOIN party p2
                ON i.lead_agent_cnt = p2.party_cnt
            LEFT OUTER JOIN party p3
                ON i.account_handler_cnt = p3.party_cnt
            LEFT OUTER JOIN party p4
                ON i.insured_cnt = p4.party_cnt
            LEFT OUTER JOIN party p5
                ON i.collection_from_cnt = p5.party_cnt
            LEFT OUTER JOIN party p6
                ON i.broker_cnt = p6.party_cnt
            LEFT OUTER JOIN party p7
                ON i.fsa_underwriter_cnt = p7.party_cnt
            LEFT OUTER JOIN business_type bt
                ON i.business_type_id = bt.business_type_id
            LEFT OUTER JOIN collect_type ct
                ON i.collect_type_id = ct.collect_type_id
            LEFT OUTER JOIN source b
                ON i.source_id = b.source_id
            LEFT OUTER JOIN currency c
                ON i.currency_id = c.currency_id
            LEFT OUTER JOIN language l
                ON i.language_id = l.language_id
            LEFT OUTER JOIN renewal_method rm
                ON i.renewal_method_id = rm.renewal_method_id
            LEFT OUTER JOIN renewal_frequency rf
                ON i.renewal_frequency_id = rf.renewal_frequency_id
            LEFT OUTER JOIN lapsed_reason lr
                ON i.lapsed_reason_id = lr.lapsed_reason_id
            LEFT OUTER JOIN agent_commission ac
                ON i.insurance_file_cnt = ac.insurance_file_cnt
                AND ac.is_lead_agent =1
            LEFT OUTER JOIN underwriting_year UW
                ON i.underwriting_year_id = UW.underwriting_year_id
        WHERE
            i.insurance_file_cnt = @InsuranceFileCnt
            and
			 ISNULL(ABS(ac.commission_value),0) > case WHEN
			(select max(is_lead_agent) from agent_commission where insurance_file_cnt=@InsuranceFileCnt and Commission_value>0) > 0 Then 0
			else -1
			END

    IF @IsUnderwriting = 'U'
    BEGIN
        SELECT @policy_status = (
                                 SELECT description
                                 FROM
                                     policy_status
                                     JOIN insurance_file i
                                         ON policy_status.policy_status_id = i.policy_status_id
                                 WHERE i.insurance_file_cnt=@InsuranceFileCnt
                                )

		SELECT @File_type = (
                                 SELECT description
                                 FROM
                                     Insurance_File_Type
                                     JOIN insurance_file i
                                         ON insurance_file_type.insurance_file_type_id = i.insurance_file_type_id
                                 WHERE i.insurance_file_cnt=@InsuranceFileCnt
                                )
    END
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

