SET QUOTED_IDENTIFIER OFF 

go 

SET ANSI_NULLS ON 
GO 

EXECUTE DDLDROPPROCEDURE 'spu_SAM_Party_Policy_list' 

GO 

CREATE PROCEDURE spu_SAM_Party_Policy_list 
        @nParty_cnt INT, 
        @nUserId    INT = NULL,
        @Agent_Key int = NULL 
AS 
    CREATE TABLE #TempPolicies
      ( 
         InsuranceFileId        INT,
         BranchKey              INT,
         BranchCode             CHAR(10),
         InsuranceFileKey       INT,
         PolicyRef              VARCHAR(30),
         InsuranceFolderKey     INT,
         PolicyTypeId           INT,
         LeadInsurerKey         INT,
         DateIssued             DATETIME,
         CoverStartDate         DATETIME,
         ExpiryDate             DATETIME,
         RenewalDate            DATETIME,
         InsuredKey             INT,
         ProductKey             INT,
         LeadAgentKey           INT,
         ThisPremium            NUMERIC(19, 4),
         AnnualPremium          NUMERIC(19, 4),
         NetPremium             NUMERIC(19, 4),
         TaxAmount              NUMERIC(19, 4),
         GeminiPolicyStatus     INT,
         PartyShortName         CHAR(20),
         ProductCode            CHAR(10),
         ProductDesc            VARCHAR(255),
         InsuranceFileTypeCode  CHAR(10),
         PolicyStatusCode       CHAR(10),
         InsurerShortName       CHAR(20),
         AgentShortName         CHAR(255),
         PolicyTypeCode         CHAR(10),
         PolicyTypeDesc         VARCHAR(255),
         CurrencyCode           CHAR(10),
         PolicyVersion          INT,
         RiskCodeDescription    VARCHAR(255),
         AlternativeRef         VARCHAR(255),
         --Start (Prakash C Varghese) - (Tech Spec-UIICWR50-MTC-List Policies.doc) - (7.2)  
         Regarding              VARCHAR(255),
         PolicyStatus           VARCHAR(255),
         RiskTypeDescription    VARCHAR(255),
         EventDescription       VARCHAR(MAX),
         MarkedForCollection    TINYINT,
         IsCurrent              TINYINT,
         BaseInsuranceFolderKey INT,
         QuoteVersion           INT,
         QuoteStatusKey         INT,
         QuoteExpiryDate        DATETIME,
         RiskStatus             VARCHAR(255),
         IsMarketPlacePolicy    TINYINT,
	     IsMigratedPolicy BIT,
         IsReadonly             BIT ,
		 RenewedVersion TINYINT
      ) 

    INSERT INTO #TempPolicies
    SELECT DISTINCT ifi.insurance_file_id, 
                    ifi.source_id, 
                    source.code, 
                    ifi.Insurance_File_Cnt,
                    ifi.Insurance_Ref,
                    ifi.Insurance_Folder_cnt,
                    ifi.Insurance_File_Type_id, 
                    ifi.lead_insurer_cnt, 
                    ifi.system_base_date,  
                    ifi.cover_start_date, 
                    ifi.expiry_date, 
                    ifi.renewal_date, 
                    ifi.Insured_Cnt,
                    ifi.Product_Id,
                    ifi.Lead_Agent_Cnt,
                    ifi.This_Premium,
                    ifi.Annual_Premium,
                    ifi.Net_Premium,
                    ifi.Tax_Amount,
                    ifi.Gemini_Policy_Status, 
                    party_insured.shortname, 
                    product.code, 
                    product_caption.caption, 
                    insurance_file_type.code, 
                    CASE Isnull(ifi.out_of_sequence_replaced, 0) 
                      WHEN 1 THEN 'OOS CANCEL' 
                      ELSE Isnull(insurance_file_status.code, 'LIVE') 
                    END, 
                    party_insurer.shortname, 
                    party_agent.shortname, 
                    policy_type.code, 
                    policy_type.description, 
                    currency.code, 
                    ifi.policy_version, 
                    rc.description, 
                    ifi.alternate_reference, 
                    ifsys.last_trans_description, 
                    CASE Isnull(ifi.out_of_sequence_replaced, 0) 
                      WHEN 1 THEN 'OOS REPLACED' 
                      ELSE Isnull(insurance_file_status.code, 'LIVE') 
                    END, 
                    insurance_file_type.description, 
                    (SELECT event_log.description 
                     FROM   event_log 
                     WHERE  event_cnt = (SELECT Max(event_cnt) 
                                         FROM   event_log el 
                                         WHERE  el.insurance_file_cnt = 
                                                ifi.insurance_file_cnt)), 
                    ifi.marked_for_collection, 
                    0   IsCurrent, 
                    ifi.base_insurance_folder_cnt, 
                    ifi.quote_version, 
                    ifi.quote_status_id, 
                    ifi.quote_expiry_date, 
                    CASE 
                      WHEN tmp.insurance_file_cnt IS NOT NULL THEN 'Unquoted' 
                      WHEN tmp1.insurance_file_cnt IS NOT NULL THEN 'Unquoted' 
                      WHEN tmp2.insurance_file_cnt IS NOT NULL THEN 'Declined' 
                      WHEN tmp3.insurance_file_cnt IS NOT NULL THEN 'Referred' 
                      ELSE 'Quoted' 
                    END AS RiskStatus, 
                    ifi.is_marketplace_policy,
                    CASE 
                      WHEN EXISTS (SELECT 1 
                                   FROM   insurance_file ifi WITH ( nolock) 
                                   WHERE  ifi.insurance_file_cnt = 
                                          rs.insurance_file_cnt 
                                          AND ifi.insurance_file_cnt = 
                                              rs.renewal_insurance_file_cnt 
                                  ) THEN 1 
                      ELSE 0 
                    END IsMigratedPolicy, 
                    CASE 
                      WHEN EXISTS (SELECT NULL 
                                   FROM   insurance_file ifirs 
                                   WHERE  ifirs.insurance_file_cnt = 
                                          ifi.insurance_file_cnt 
                                          AND ( 
                                  Isnull(out_of_sequence_replaced, 
                                  0) 
                                  = 1 
                                   OR ( ifirs.insurance_file_type_id 
                                        IN 
                                        ( 4, 7, 10, 11 ) 
                                        AND 
								Isnull(ifirs.insurance_file_status_id, 0) 
								= 1 ) 
									OR ( (SELECT TOP 1 
										insurance_file_cnt 
										FROM   insurance_file 
										WHERE  insurance_file_type_id 
												= 8 
												AND 
								insurance_folder_cnt = ifi.insurance_folder_cnt 
								AND Isnull(base_insurance_file_cnt, 
								insurance_file_cnt) = 
								insurance_file_cnt 
								ORDER  BY insurance_file_cnt DESC) <> 
								ifi.insurance_file_cnt 
								AND ifi.insurance_file_type_id = 8 ) 
								OR ifirs.source_id IN (SELECT source_id 
								FROM   pmuser_source 
								WHERE  [user_id] = @nUserId) 
								OR EXISTS(SELECT NULL 
								FROM   insurance_file ifican 
								WHERE  ifi.insurance_file_type_id = 8 
								AND ifican.insurance_file_cnt > 
								ifi.insurance_file_cnt 
								AND ifican.insurance_file_type_id IN ( 2, 
								5, 9 ) 
								AND ifican.insurance_folder_cnt = 
								ifi.insurance_folder_cnt 
								AND 
								Isnull(ifican.base_insurance_file_cnt, 
								ifican.insurance_file_cnt) = 
								ifican.insurance_file_cnt) )) THEN 1 
								ELSE 0 
								END IsReadOnly,
					(CASE WHEN (ifi.insurance_file_status_id IS NULL AND ifi.policy_version > 1) THEN 1 ELSE 0 END)
    FROM Insurance_File ifi    WITH ( NOLOCK)
	INNER JOIN insurance_file_system ifsys WITH ( NOLOCK) 
    ON ifi.insurance_file_cnt = ifsys.insurance_file_cnt 
    LEFT JOIN source 
    ON source.source_id = ifi.source_id 
    LEFT JOIN party party_insured WITH ( nolock) 
    ON party_insured.party_cnt = ifi.insured_cnt 
    LEFT JOIN product 
    ON product.product_id = ifi.product_id 
    LEFT JOIN pmcaption product_caption 
    ON product_caption.caption_id = product.caption_id 
    AND product_caption.language_id = 1 
    LEFT JOIN insurance_file_type 
    ON insurance_file_type.insurance_file_type_id = 
    ifi.insurance_file_type_id 
    LEFT JOIN insurance_file_status 
    ON insurance_file_status.insurance_file_status_id = 
    ifi.insurance_file_status_id 
    LEFT JOIN party party_insurer WITH ( nolock) 
    ON party_insurer.party_cnt = ifi.lead_insurer_cnt 
    LEFT JOIN party party_agent WITH ( nolock) 
    ON party_agent.party_cnt = ifi.lead_agent_cnt 
    LEFT JOIN policy_type 
    ON policy_type.policy_type_id = ifi.policy_type_id 
    LEFT JOIN risk_code RC 
    ON rc.risk_code_id = ifi.risk_code_id 
    INNER JOIN currency 
    ON currency.currency_id = ifi.currency_id 
    LEFT JOIN (SELECT ifrl.insurance_file_cnt 
    FROM   risk rsk 
    JOIN insurance_file_risk_link ifrl 
    ON rsk.risk_cnt = ifrl.risk_cnt 
    WHERE  risk_status_id IS NULL) tmp 
    ON tmp.insurance_file_cnt = ifi.insurance_file_cnt 
    LEFT JOIN (SELECT ifrl.insurance_file_cnt 
    FROM   risk rsk 
    JOIN insurance_file_risk_link ifrl 
    ON rsk.risk_cnt = ifrl.risk_cnt 
    WHERE  risk_status_id = 4) tmp1 
    ON tmp1.insurance_file_cnt = ifi.insurance_file_cnt 
    LEFT JOIN (SELECT ifrl.insurance_file_cnt 
    FROM   risk rsk 
    JOIN insurance_file_risk_link ifrl 
    ON rsk.risk_cnt = ifrl.risk_cnt 
    WHERE  risk_status_id = 2) tmp2 
    ON tmp2.insurance_file_cnt = ifi.insurance_file_cnt 
    LEFT JOIN (SELECT ifrl.insurance_file_cnt 
    FROM   risk rsk 
    JOIN insurance_file_risk_link ifrl 
    ON rsk.risk_cnt = ifrl.risk_cnt 
    WHERE  risk_status_id = 1) tmp3 
    ON tmp3.insurance_file_cnt = ifi.insurance_file_cnt 
    
    LEFT JOIN (SELECT ifrl.insurance_file_cnt 
    FROM   risk rsk 
    JOIN insurance_file_risk_link ifrl 
    ON rsk.risk_cnt = ifrl.risk_cnt 
    WHERE  risk_status_id = 3) tmp4 
    ON tmp4.insurance_file_cnt = ifi.insurance_file_cnt 
    LEFT JOIN renewal_status rs 
    ON ifi.insurance_file_cnt = rs.insurance_file_cnt 
    LEFT JOIN insurance_file base_ifi WITH ( nolock) 
    ON base_ifi.insurance_file_cnt = ifi.base_insurance_file_cnt 
    WHERE  ifi.insured_cnt = @nParty_cnt 

    AND ifi.policy_ignore IS NULL 
    AND ( Isnull(base_ifi.insurance_file_type_id, 0) NOT IN ( 4, 10, 11 ) 
    OR ifi.insurance_file_cnt = ifi.base_insurance_file_cnt ) 
    ORDER  BY IFI.base_insurance_folder_cnt DESC, 
    ifi.insurance_file_cnt DESC, 
    IFI.quote_version DESC 
   
    -- any change in sort order will affect Nexus [WPR 63][Sagicor]   
    DECLARE @nInsurance_folder_key INT 
    DECLARE cur_updateiscurrent CURSOR fast_forward FOR 
      SELECT DISTINCT ( insurancefolderkey ) 
      FROM   #temppolicies 
      WHERE  policytypeid IN ( 1, 2, 5, 8, 
                               9, 12 ) 
             AND policystatuscode NOT IN ( 'LAP', 'CAN' ) 

    OPEN cur_updateiscurrent 

    FETCH next FROM cur_updateiscurrent INTO @nInsurance_folder_key 

    WHILE ( @@FETCH_STATUS = 0 ) 
      BEGIN 
          UPDATE #temppolicies 
          SET    iscurrent = 1 
          WHERE  policytypeid IN ( 2, 5, 8, 9, 12 ) 
                 AND insurancefilekey IN (SELECT TOP 1 insurancefilekey 
                                          FROM   #temppolicies 
                                          WHERE  policytypeid IN ( 1, 2, 5, 8, 
                                                                   9, 12 ) 
                                                 AND policystatuscode NOT IN ( 
                                                     'LAP' 
                                                     , 
                                                     'CAN' ) 
                                                 AND insurancefolderkey = 
                                                     @nInsurance_folder_key 
                                                 AND isReadonly = 0 
                                          ORDER  BY coverstartdate DESC, 
                                                    insurancefilekey DESC) 

          FETCH next FROM cur_updateiscurrent INTO @nInsurance_folder_key 
      END 

    CLOSE cur_updateiscurrent 

    DEALLOCATE cur_updateiscurrent 

    SELECT * 
    FROM   #temppolicies 

    DROP TABLE #temppolicies 

GO 

SET QUOTED_IDENTIFIER OFF 

GO 

SET ANSI_NULLS ON 

GO 
