EXECUTE DDLDROPPROCEDURE 'spu_SAM_Insurance_File_Upd'

GO

CREATE PROCEDURE spu_SAM_Insurance_File_Upd @source_id                          SMALLINT,
                                            @cover_start_date                   DATETIME,
                                            @expiry_date                        DATETIME,
                                            @product_id                         INT,
                                            @description                        VARCHAR(255),
                                            @insurance_ref                      VARCHAR(30),
                                            @insured_name                       VARCHAR(255),
                                            @currency_id                        SMALLINT,
                                            @lead_agent_cnt                     INT,
                                            @analysis_code_id                   INT,
                                            @alternate_reference                VARCHAR(80),
                                            @insurance_folder_cnt               INT,
                                            @insurance_file_cnt                 INT,
                                            @insured_cnt                        INT,
                                            @branch_id                          SMALLINT,
                                            @lead_allow_consolidated_commission TINYINT,
                                            @sub_allow_consolidated_commission  TINYINT,
                                            @business_type_id                   SMALLINT,
                                            @quote_expiry_date                  DATETIME,
                                            @account_handler_cnt                INT,
                                            @regarding                          VARCHAR(255),
                                            @policy_status_id                   INT,
                                            @inception_date                     DATETIME,
                                            @renewal_date                       DATETIME,
                                            @inception_date_tpi                 DATETIME,
                                            @date_issued                        DATETIME,
                                            @proposal_date                      DATETIME,
                                            @renewal_frequency_id               SMALLINT,
                                            @renewal_method_id                  INT,
                                            @lapsed_reason_id                   INT,
                                            @long_term_undertaking_date         DATETIME,
                                            @renewal_stop_code_id               INT,
                                            @lapsed_date                        DATETIME,
                                            @is_referred_at_renewal             TINYINT,
                                            @is_referred_on_mta                 TINYINT,
                                            @modified_by_id                     INT,
                                            @insurance_file_structure_id        INT,
                                            @payment_method                     VARCHAR(60),
                                            @insurance_file_type_id             INT,
                                            @underwriting_year_id               INT,
                                            @marked_for_collection              TINYINT,
                                            @marked_date                        DATETIME,
                                            @base_insurance_folder_cnt          INT = NULL,
                                            @quote_version                      INT = NULL,
                                            @quote_status_id                    INT = NULL,
                                            @Contact_user_id                    INT =NULL,
                                            @put_on_next_instalment_renewal     TINYINT = 0,
                                            @Anniversary_Date                   DATETIME = '1/1/1900',
                                            @renewal_day_number                 INT=0,
                                            @OldPolicyNumber                    VARCHAR(30)='',
                                            @bIs_Marketplace_Policy             TINYINT = 0,
                                            @coins_placement Varchar(10)= NULL  , 
                                           	@nCollectionFrequency               INT =NULL,
											@nPaymentTerms                      INT =NULL,
											@Correspondence_Type				INT = NULL,	
											@Default_Preferred_Correspondence	INT = NULL,
											@Is_Agent_Correspondence			TINYINT = 0,
											@Sender_Email						VARCHAR(255) = NULL,
											@Receiver_Email						VARCHAR(255) = NULL

AS  
BEGIN  
      IF @Anniversary_Date = '1/1/1900'
        SET @Anniversary_Date=DATEADD(year, 1, @inception_date)
  
      IF @renewal_day_number = 0
   BEGIN  
            SELECT @renewal_day_number = renewal_day_number
            FROM   insurance_file
            WHERE  insurance_file_cnt = @insurance_file_cnt
   END  
      DECLARE @old_insurance_ref VARCHAR(30)
  
SELECT @old_insurance_ref = i.insurance_ref  
      FROM   insurance_file i
      WHERE  i.insurance_file_cnt = @insurance_file_cnt
  
      IF ( @underwriting_year_id IS NULL
           AND @insurance_file_type_id = 3 )
BEGIN  
            SELECT @underwriting_year_id = underwriting_year_id
            FROM   Underwriting_Year
            WHERE  @renewal_date BETWEEN start_date AND end_date
END  
  
      IF ( @marked_for_collection IS NULL )
BEGIN  
 SELECT @marked_for_collection = marked_for_collection  
 FROM   insurance_file i  
 WHERE  i.insurance_file_cnt = @insurance_file_cnt  
END  
  
      IF ( @marked_date IS NULL )
BEGIN  
 SELECT @marked_date = marked_date  
 FROM   insurance_file i  
 WHERE  i.insurance_file_cnt = @insurance_file_cnt  
END  
  
UPDATE Insurance_File  
      SET    source_id = @source_id,
             cover_start_date = @cover_start_date,
             expiry_date = @expiry_date,
             product_id = @product_id,
             insurance_ref = @insurance_ref,
             insured_name = @insured_name,
             currency_id = @currency_id,
             lead_agent_cnt = @lead_agent_cnt,
             Analysis_code_id = @Analysis_code_id,
             alternate_reference = @alternate_reference,
             insured_cnt = @insured_cnt,
             branch_id = @branch_id,
             lead_allow_consolidated_commission = @lead_allow_consolidated_commission,
             sub_allow_consolidated_commission = @sub_allow_consolidated_commission,
             business_type_id = @business_type_id,
             quote_expiry_date = @quote_expiry_date,
             account_handler_cnt = @account_handler_cnt,
             policy_status_id = @policy_status_id,
             inception_date = @inception_date,
             renewal_date = @renewal_date,
             inception_date_tpi = @inception_date_tpi,
             date_issued = @date_issued,
             proposal_date = @proposal_date,
             renewal_frequency_id = @renewal_frequency_id,
             renewal_method_id = @renewal_method_id,
             lapsed_reason_id = @lapsed_reason_id,
             long_term_undertaking_date = @long_term_undertaking_date,
             renewal_stop_code_id = @renewal_stop_code_id,
             lapsed_date = @lapsed_date,
             is_referred_at_renewal = @is_referred_at_renewal,
             is_referred_on_mta = @is_referred_on_mta,
             insurance_file_structure_id = @insurance_file_structure_id,
             payment_method = @payment_method,
             insurance_file_type_id = @insurance_file_type_id,
             underwriting_year_id = @underwriting_year_id,
             marked_for_collection = @marked_for_collection,
             marked_date = @marked_date,
             Contact_user_id = @Contact_user_id,
             put_on_next_instalment_renewal = @put_on_next_instalment_renewal,
             Anniversary_Date = @Anniversary_Date,
             renewal_day_number = @renewal_day_number,
             old_policy_number = @OldPolicyNumber,
             is_marketplace_policy = @bIs_Marketplace_Policy,
			 CollectionFrequency_id = @nCollectionFrequency,
			 DOPaymentTerms_id = @nPaymentTerms,
			 coins_placement = @coins_placement ,
			 Correspondence_Type = @Correspondence_Type,
			 Default_Preferred_Correspondence = @Default_Preferred_Correspondence,
			 Is_Agent_Correspondence = @Is_Agent_Correspondence,
			 Sender_Email = @Sender_Email,
			 Receiver_Email = @Receiver_Email

      WHERE  insurance_file_cnt = @insurance_file_cnt
  
UPDATE Insurance_File_System  
      SET    last_trans_description = @regarding,
             modified_by_id = @modified_by_id,
             last_modified = GETDATE()
      WHERE  insurance_file_cnt = @insurance_file_cnt
  
UPDATE insurance_folder  
      SET    [description] = @description,
             inception_date = @inception_date
      WHERE  insurance_folder_cnt = @insurance_folder_cnt
  
IF @old_insurance_ref <> @insurance_ref  
BEGIN  
            IF EXISTS (SELECT *
                       FROM   Transaction_Export_Folder
                       WHERE  insurance_ref = @old_insurance_ref)
              UPDATE Transaction_Export_Folder
              SET    insurance_ref = @insurance_ref
              WHERE  insurance_ref = @old_insurance_ref
  
            IF EXISTS (
                      SELECT *
                       FROM   Transdetail
                       WHERE  insurance_ref = @old_insurance_ref)
              UPDATE Transdetail
              SET    insurance_ref = @insurance_ref
              WHERE  insurance_ref = @old_insurance_ref
  
            IF EXISTS (SELECT *
                       FROM   Claim
                       WHERE  policy_number = @old_insurance_ref)
              UPDATE Claim
              SET    policy_number = @insurance_ref
              WHERE  policy_number = @old_insurance_ref
END  

  
IF @lead_agent_cnt IS NULL  
        BEGIN
            DELETE FROM Tax_Calculation
            WHERE  agent_commission_cnt IN (SELECT agent_commission_cnt
                                            FROM   Agent_Commission
                                            WHERE  insurance_file_cnt = @insurance_file_cnt
                                                   AND is_lead_agent = 1)

 DELETE FROM Agent_Commission  
            WHERE  insurance_file_cnt = @insurance_file_cnt
                   AND is_lead_agent = 1
        END
  
      IF @lapsed_date IS NOT NULL AND @lapsed_date > '1900-01-01' AND @insurance_file_type_id=1
 UPDATE Insurance_File  
        SET    insurance_file_status_id = 2
        WHERE  insurance_file_cnt = @insurance_file_cnt
  END
  
