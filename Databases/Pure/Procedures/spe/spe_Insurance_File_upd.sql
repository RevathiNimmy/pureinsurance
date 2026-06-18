
EXECUTE DDLDROPPROCEDURE 'spe_Insurance_File_Upd'

GO

CREATE PROCEDURE spe_Insurance_File_Upd @insurance_file_cnt                 INT,
                                        @insurance_file_structure_id        INT,
                                        @insurance_file_type_id             INT,
                                        @insurance_file_status_id           INT,
                                        @insurance_file_id                  INT,
                                        @source_id                          SMALLINT,
                                        @insurance_folder_cnt               INT,
                                        @insurance_ref                      VARCHAR(30),
                                        @product_id                         INT,
                                        @lead_insurer_cnt                   INT,
                                        @lead_agent_cnt                     INT,
                                        @lead_agent_percent                 NUMERIC(12, 8),
                                        @account_handler_cnt                INT,
                                        @insured_cnt                        INT,
                                        @business_type_id                   SMALLINT,
                                        @collect_type_id                    SMALLINT,
                                        @collection_from_cnt                INT,
                                        @branch_id                          SMALLINT,
                                        @currency_id                        SMALLINT,
                                        @language_id                        SMALLINT,
                                        @date_issued                        DATETIME,
                                        @cover_start_date                   DATETIME,
                                        @expiry_date                        DATETIME,
                                        @renewal_date                       DATETIME,
                                        @renewal_method_id                  INT,
                                        @renewal_frequency_id               SMALLINT,
                                        @is_referred_at_renewal             TINYINT,
                                        @lapsed_reason_id                   INT,
                                        @lapsed_date                        DATETIME,
                                        @lapsed_description                 VARCHAR(255),
                                        @is_referred_on_mta                 TINYINT,
                                        @policy_version                     INT,
                                        @gemini_policy_status               INT,
                                        @gemini_business_type               INT,
                                        @deferred_ind                       INT,
                                        @policy_ignore                      INT,
                                        @broker_cnt                         INT,
                                        @risk_code_id                       INT,
                                        @Analysis_code_id                   INT,
                                        @proposal_date                      DATETIME,
                                        @diary_date                         DATETIME,
                                        @review_date                        DATETIME,
                                        @renewal_day_number                 INT,
                                        @Policy_type_id                     INT,
                                        @indicator                          CHAR(1),
                                        @clause                             CHAR(4),
                                        @cover                              CHAR(1),
                                        @area                               CHAR(1),
                                        @long_term_undertaking_date         DATETIME,
                                        @renewal_stop_code_id               INT,
                                        @vbs_type                           CHAR(2),
                                        @vbs_status                         CHAR(1),
                                        @is_insurer_rate_table              TINYINT,
                                        @is_related_policies                TINYINT,
                                        @is_retained_documents              TINYINT,
                                        @schemes_postcode                   VARCHAR(8),
                                        @paid_direct                        CHAR(1),
                                        @scheme                             INT,
                                        @brokerage_amount                   NUMERIC(19, 4),
                                        @is_minimum_brokerage_flag          TINYINT,
                                        @annual_premium                     NUMERIC(19, 4),
                                        @this_premium                       NUMERIC(19, 4),
                                        @net_premium                        NUMERIC(19, 4),
                                        @commission_amount                  NUMERIC(19, 4),
                                        @iptable_amount                     NUMERIC(19, 4),
                                        @ipt_percentage                     NUMERIC(7, 4),
                                        @is_ipt_overridden                  TINYINT,
                                        @tax_amount                         NUMERIC(19, 4),
                                        @vatable_amount                     NUMERIC(19, 4),
                                        @vat_percentage                     NUMERIC(7, 4),
                                        @vat_amount                         NUMERIC(19, 4),
                                        @payment_method                     VARCHAR(60),
                                        @user_defined_data_id               INT,
                                        @commission_percentage              NUMERIC(7, 4),
                                        @invariant_key                      INT,
                                        @insured_name                       VARCHAR(255),
                                        @alternate_reference                VARCHAR(80),
                                        @is_client_invoiced                 TINYINT,
                                        @old_policy_number                  VARCHAR(30),
                                        @quote_expiry_date                  DATETIME,
                                        @alternate_account_cnt              INT,
                                        @account_executive_cnt              INT = NULL,
                                        @anniversary_date                   DATETIME,
                                        @policy_style_id                    INT,
                                        @underwriting_year_id               INT,
                                        @policy_status_id                   INT,
                                        @inception_date_tpi                 DATETIME,
                                        @fsa_customer_category_id           SMALLINT,
                                        @fsa_contract_location_id           SMALLINT,
                                        @fsa_underwriter_cnt                INT,
                                        @fsa_type_of_sale_id                SMALLINT,
                                        @fsa_renewal_consent                TINYINT,
                                        @base_currency_id                   SMALLINT,
                                        @country_id                         SMALLINT,
                                        @discount_reason_id                 SMALLINT,
                                        @discounted_premium                 NUMERIC(19, 4),
                                        @discount_percentage                NUMERIC(11, 8),
                                        @match_discounted_premium_flag      TINYINT,
                                        @put_on_next_instalment_renewal     INT,
                                        @anniversary_copy                   INT,
                                        @discount_recurring_type_id         INT = NULL,
                                        @lead_allow_consolidated_commission TINYINT,
                                        @sub_allow_consolidated_commission  TINYINT,
                                        @terms_agreed                       BIT,
                                        @terms_agreed_date                  DATETIME,
                                        @inception_date                     DATETIME,
                                        @policy_documents_issued_date       DATETIME,
                                        @policy_documents_correct           BIT,
                                        @error_notification_date            DATETIME,
                                        @policy_deductibles_id              INT,
                                        @policy_limits_id                   INT,
                                        @risk_transfer_agreement            BIT,
                                        @renewal_premium                    NUMERIC(19, 4),
                                        @renewal_product_id                 INT = NULL,
                                        @original_product_id                INT = NULL,
                                        @risk_transfer_editable             BIT,
                                        @manual_discount_percentage         NUMERIC(11, 8) = NULL,
                                        @base_insurance_folder_cnt          INT = NULL,
                                        @quote_version                      INT = NULL,
                                        @quote_status_id                    INT = NULL,
                                        @Contact_user_id                    INT =NULL,
                                        @coins_placement                    VARCHAR(10)=NULL,
                                        @MTA_reason_id                      SMALLINT =NULL,
                                        @bIs_Marketplace_Policy             TINYINT = 0,
										@nCollectionFrequency_id INT = NULL,
										@nDOPaymentTerms_id INT = NULL 	  ,
										
										@Correspondence_Type INT = NULL,
										@Default_Preferred_Correspondence	INT = NULL,
										@Is_Agent_Correspondence TINYINT = 0,
										@Sender_Email						VARCHAR(255) = NULL,
										@Receiver_Email						VARCHAR(255) = NULL,
										@original_insurance_file_type_id    INT=NULL
AS  
BEGIN  
-- Set @account_handler_cnt = NULL if it is 0. This is to ensure that the foreign key constraint of party table does not create error
IF @account_handler_cnt = 0 
BEGIN
    SELECT @account_handler_cnt = NULL
END
  
-- DD05042004 Choose the underwriting year if not chosen  
      IF ( @underwriting_year_id IS NULL
           AND @insurance_file_type_id = 3 )
BEGIN  
            SELECT @underwriting_year_id = underwriting_year_id
            FROM   Underwriting_Year
            WHERE  @renewal_date BETWEEN start_date AND end_date
END  
ELSE IF @underwriting_year_id IS NULL
BEGIN
            SELECT @underwriting_year_id = underwriting_year_id
            FROM   Underwriting_Year
            WHERE  @cover_start_date BETWEEN start_date AND end_date
END


IF @Contact_user_id = 0
        BEGIN
            SELECT @Contact_user_id = NULL
        END
		
DECLARE @Quote_InsuranceFile_Type_ID INT
DECLARE @Policy_InsuranceFile_Type_ID INT
SELECT @Quote_InsuranceFile_Type_ID = insurance_file_type_id FROM Insurance_File_Type WHERE code = 'QUOTE'
SELECT @Policy_InsuranceFile_Type_ID = insurance_file_type_id FROM Insurance_File_Type WHERE code = 'POLICY'


--If we are making a quote live, date_issue should be present date and should never be changed for any subsequent policy version
IF (@date_issued IS NULL OR @date_issued = '1899-12-29 00:00:00.000') AND 
    ((select Insurance_File_Type_id from Insurance_File where insurance_file_cnt = @insurance_file_cnt) = @Quote_InsuranceFile_Type_ID) AND
    (@insurance_file_type_id = @Policy_InsuranceFile_Type_ID) BEGIN
    SELECT @date_issued = GETDATE()
END
  
IF @payment_method = '' BEGIN
	SELECT @payment_method = NULL
END
UPDATE Insurance_File  
      SET    insurance_file_structure_id = @insurance_file_structure_id,
             insurance_file_type_id = @insurance_file_type_id,
             insurance_file_status_id = @insurance_file_status_id,
             insurance_file_id = @insurance_file_id,
             source_id = @source_id,
             insurance_folder_cnt = @insurance_folder_cnt,
             insurance_ref = @insurance_ref,
             product_id = @product_id,
             lead_insurer_cnt = @lead_insurer_cnt,
             lead_agent_cnt = @lead_agent_cnt,
             lead_agent_percent = @lead_agent_percent,
             account_handler_cnt = @account_handler_cnt,
             insured_cnt = @insured_cnt,
             business_type_id = @business_type_id,
             collect_type_id = @collect_type_id,
             collection_from_cnt = @collection_from_cnt,
             branch_id = @branch_id,
             currency_id = @currency_id,
             language_id = @language_id,
             date_issued = @date_issued,
             cover_start_date = @cover_start_date,
             expiry_date = @expiry_date,
             renewal_date = @renewal_date,
             renewal_method_id = @renewal_method_id,
             renewal_frequency_id = @renewal_frequency_id,
             is_referred_at_renewal = @is_referred_at_renewal,
             lapsed_reason_id = @lapsed_reason_id,
             lapsed_date = @lapsed_date,
             lapsed_description = @lapsed_description,
             is_referred_on_mta = @is_referred_on_mta,
             policy_version = @policy_version,
             gemini_policy_status = @gemini_policy_status,
             gemini_business_type = @gemini_business_type,
             deferred_ind = @deferred_ind,
             policy_ignore = @policy_ignore,
             broker_cnt = @broker_cnt,
             risk_code_id = @risk_code_id,
             Analysis_code_id = @Analysis_code_id,
             proposal_date = @proposal_date,
             diary_date = @diary_date,
             review_date = @review_date,
             renewal_day_number = @renewal_day_number,
             Policy_type_id = @Policy_type_id,
             indicator = @indicator,
             clause = @clause,
             cover = @cover,
             area = @area,
             long_term_undertaking_date = @long_term_undertaking_date,
             renewal_stop_code_id = @renewal_stop_code_id,
             vbs_type = @vbs_type,
             vbs_status = @vbs_status,
             is_insurer_rate_table = @is_insurer_rate_table,
             is_related_policies = @is_related_policies,
             is_retained_documents = @is_retained_documents,
             schemes_postcode = @schemes_postcode,
             paid_direct = @paid_direct,
             scheme = @scheme,
             brokerage_amount = @brokerage_amount,
             is_minimum_brokerage_flag = @is_minimum_brokerage_flag,
             annual_premium = @annual_premium,
             this_premium = @this_premium,
             net_premium = @net_premium,
             commission_amount = @commission_amount,
             iptable_amount = @iptable_amount,
             ipt_percentage = @ipt_percentage,
             is_ipt_overridden = @is_ipt_overridden,
             tax_amount = @tax_amount,
             vatable_amount = @vatable_amount,
             vat_percentage = @vat_percentage,
             vat_amount = @vat_amount,
             payment_method = ISNULL(@payment_method,payment_method),
             user_defined_data_id = @user_defined_data_id,
             commission_percentage = @commission_percentage,
             invariant_key = @invariant_key,
             insured_name = @insured_name,
             alternate_reference = @alternate_reference,
             is_client_invoiced = @is_client_invoiced,
             old_policy_number = @old_policy_number,
             quote_expiry_date = @quote_expiry_date,
             alternate_account_cnt = @alternate_account_cnt,
             account_executive_cnt = @account_executive_cnt,
             anniversary_date = @anniversary_date,
             policy_style_id = @policy_style_id,
             underwriting_year_id = @underwriting_year_id,
             policy_status_id = @policy_status_id,
             inception_date_tpi = @inception_date_tpi,
             fsa_customer_category_id = @fsa_customer_category_id,
             fsa_contract_location_id = @fsa_contract_location_id,
             fsa_underwriter_cnt = @fsa_underwriter_cnt,
             fsa_type_of_sale_id = @fsa_type_of_sale_id,
             fsa_renewal_consent = @fsa_renewal_consent,
             base_currency_id = @base_currency_id,
             country_id = @country_id,
             discount_reason_id = @discount_reason_id,
             discounted_premium = @discounted_premium,
             discount_percentage = @discount_percentage,
             match_discounted_premium_flag = @match_discounted_premium_flag,
             put_on_next_instalment_renewal = @put_on_next_instalment_renewal,
    anniversary_copy = @anniversary_copy,  
    discount_recurring_type_id = @discount_recurring_type_id,  
             lead_allow_consolidated_commission = @lead_allow_consolidated_commission,
             sub_allow_consolidated_commission = @sub_allow_consolidated_commission,
             terms_agreed = @terms_agreed,
             terms_agreed_date = @terms_agreed_date,
             inception_date = @inception_date,
             policy_documents_issued_date = @policy_documents_issued_date,
             policy_documents_correct = @policy_documents_correct,
             error_notification_date = @error_notification_date,
             policy_deductibles_id = @policy_deductibles_id,
             policy_limits_id = @policy_limits_id,
             risk_transfer_agreement = @risk_transfer_agreement,
             renewal_premium = @renewal_premium,
             renewal_product_id = @renewal_product_id,
             original_product_id = @original_product_id,
             risk_transfer_editable = @risk_transfer_editable,
             manual_discount_percentage = @manual_discount_percentage,
             Contact_user_id = @Contact_user_id,
             MTA_reason_id = @MTA_reason_id,
             is_marketplace_policy = @bIs_Marketplace_Policy,
             coins_placement= @coins_placement,             
			 CollectionFrequency_id = IIF(@nCollectionFrequency_id=0,null,@nCollectionFrequency_id),
	         DOPaymentTerms_id =IIF(@nDOPaymentTerms_id=0,null,@nDOPaymentTerms_id),            
			 Correspondence_Type = @Correspondence_Type,
			 Default_Preferred_Correspondence  = @Default_Preferred_Correspondence,
			 Is_Agent_Correspondence = @Is_Agent_Correspondence,
			 Sender_Email = @Sender_Email,
			 Receiver_Email = @Receiver_Email,
			 original_insurance_file_type_id = @original_insurance_file_type_id
      WHERE  insurance_file_cnt = @insurance_file_cnt

IF @lead_agent_cnt IS NULL
   BEGIN
	 DELETE FROM Tax_Calculation
            WHERE  insurance_file_cnt = @insurance_file_cnt
                   AND transtype = 'TTAC'

	 DELETE FROM Agent_Commission  
            WHERE  insurance_file_cnt = @insurance_file_cnt
                   AND is_lead_agent = 1
END  

END  
