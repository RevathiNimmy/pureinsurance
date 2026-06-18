SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_SAM_Party_Policy_list_Without_Versioning'
GO

CREATE PROCEDURE spu_SAM_Party_Policy_list_Without_Versioning 
        @party_cnt INT,
        @Agent_Key int = null,
        @user_id int =null,
        @RetrieveAssociates As TINYINT=0
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
  
        Regarding              VARCHAR(255),
        PolicyStatus           VARCHAR(255),
        RiskTypeDescription    VARCHAR(255),
        EventDescription       VARCHAR(Max),
        MarkedForCollection    TINYINT,
        IsCurrent              TINYINT,
BaseInsuranceFolderKey INT,
        QuoteVersion           INT,
        QuoteStatusKey         INT,
        QuoteExpiryDate        DATETIME,
        IsMarketPlacePolicy    TINYINT,
        RiskStatus	        VARCHAR(255),
        IsReadonly             BIT,
        CurrentInsuranceFile   INT,
        AssociatedClients VARCHAR(MAX)
      )
	   

 IF @Agent_Key IS NULL AND @user_id IS NOT NULL
 BEGIN
    SELECT @Agent_Key=p.party_cnt from PMUser u join party p on p.party_cnt=u.party_cnt where user_id=@user_id and party_type_id=3
 END

        ;With AssociatedPolicy(InsuranceFile_Cnt)
          AS(
              SELECT DISTINCT IFA.Insurance_file_cnt 
              From insurance_file_associates  IFA
              INNER join insurance_file IFI ON IFA.insurance_file_cnt  = IFI.insurance_file_cnt 
              Where IFA.Party_cnt=@party_cnt And 
              (CASE  
				WHEN ISNUll(IFA.Is_Deleted,0) = 1  
					And ISNull(IFA.date_removed,Dateadd(year,-99,Getdate())) <= GETDATE() 
				THEN 0 ELSE 1 END =1) 
             And  @RetrieveAssociates=1
            )

INSERT INTO #TempPolicies
    SELECT Distinct ifi.Insurance_File_Id ,
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
           ISNULL(insurance_file_status.code, 'LIVE'),
           party_insurer.shortname,
           party_agent.name,
           policy_type.code,
           policy_type.description,
           currency.code,
           ifi.policy_version,
           rc.description,
           ifi.alternate_reference,
--Start (Prakash C Varghese) - (Tech Spec-UIICWR50-MTC-List Policies.doc) - (7.2)
           ifsys.last_trans_description,
           ISNULL(insurance_file_status.description, CASE WHEN LTRIM(RTRIM(insurance_file_type.code)) = 'QUOTE' THEN 'QUOTE'
														WHEN LTRIM(RTRIM(insurance_file_type.code)) = 'RENEWAL' THEN 'IN RENEWAL'
														WHEN LTRIM(RTRIM(insurance_file_type.code)) = 'WRITTEN' THEN 'WRITTEN'
														ELSE 'LIVE' END ),
    insurance_file_type.description,
           NULL,
           ifi.marked_for_collection,
     --End (Prakash C Varghese) - (Tech Spec-UIICWR50-MTC-List Policies.doc) - (7.2)
      0 IsCurrent,
    ifi.base_insurance_folder_cnt,
    ifi.quote_version,
    ifi.quote_status_id,
           CASE WHEN ifi.insurance_file_type_id IN (2,5,8,9) THEN DATEADD(Day,10,GETDATE()) ELSE ifi.quote_expiry_date END,
           ifi.is_marketplace_policy,
        	NULL,
		   (CASE
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
							WHERE  [user_id] = @user_id)
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
							END) As IsReadOnly,
                NULL,
                Cast((    
                 CASE @RetrieveAssociates    
                 WHEN 1 THEN (
					 SELECT P.resolved_name +' ('+ AT.description + ')'  as Name   
                     FROM insurance_file_associates Associate
                     INNER JOIN party P ON P.party_cnt = Associate.party_cnt
                     INNER JOIN Association_Type AT ON Associate.Association_Type_id=AT.Association_Type_id   
					 Where Associate.Insurance_file_cnt=IFI.Insurance_file_cnt   And ISNUll(Associate.Is_Deleted,0) <> 1   
					 FOR XML AUTO, TYPE )    
                 ELSE ''    
                END) As Varchar(Max)) As AssociatedClients 
    FROM   Insurance_File ifi
           INNER JOIN insurance_file_system ifsys
                   ON ifi.insurance_file_cnt = ifsys.insurance_file_cnt
           LEFT JOIN source
                  ON source.source_id = ifi.source_id
           LEFT JOIN party party_insured
                  ON party_insured.party_cnt = ifi.insured_cnt
           LEFT JOIN product
                  ON product.product_id = ifi.product_id
           LEFT JOIN pmcaption product_caption
                  ON product_caption.caption_id = product.caption_id
                     AND product_caption.language_id = 1
           LEFT JOIN insurance_file_type
                  ON insurance_file_type.insurance_file_type_id = ifi.insurance_file_type_id
           LEFT JOIN insurance_file_status
                  ON insurance_file_status.insurance_file_status_id = ifi.insurance_file_status_id
           LEFT JOIN party party_insurer
                  ON party_insurer.party_cnt = ifi.lead_insurer_cnt
           LEFT JOIN party party_agent
                  ON party_agent.party_cnt = ifi.lead_agent_cnt
           LEFT JOIN policy_type
                  ON policy_type.policy_type_id = ifi.policy_type_id
           LEFT JOIN risk_code RC
                  ON rc.risk_code_id = ifi.risk_code_id
           INNER JOIN Currency
                   ON currency.currency_id = ifi.currency_id
		   LEFT JOIN Insurance_File base_ifi WITH ( NOLOCK) ON base_ifi.insurance_file_cnt = ifi.Base_Insurance_File_Cnt
			INNER JOIN (SELECT MAX(ifi.insurance_file_cnt) AS insurance_file_cnt
                         FROM   Insurance_File AS ifi
                        Left Join AssociatedPolicy APol ON APol.InsuranceFile_Cnt=ifi.insurance_file_cnt  
                        INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                        INNER JOIN insurance_file_type IFT ON ifi.insurance_file_type_id=IFT.insurance_file_type_id
                       WHERE  (ifo.insurance_holder_cnt = @party_cnt OR  ifi.insurance_file_cnt in (Apol.InsuranceFile_Cnt))
                              AND IFT.code IN ('QUOTE','POLICY','RENEWAL','MTA PERM','MTA TEMP','MTACAN','MTAREINS','WRITTEN')
                              AND ifi.policy_ignore IS NULL
							  AND ISNull(ifi.out_of_sequence_replaced, 0) <> 1
                              --AND ( ifi.source_id = @source_id )
                       GROUP  BY ifi.insurance_ref) AS PL
                   ON ifi.insurance_file_cnt = PL.insurance_file_cnt
        WHERE  ((ifi.insured_cnt = @party_cnt) OR   ifi.insurance_file_cnt in 
			(Select AssociatedPolicy.InsuranceFile_Cnt from AssociatedPolicy)) 
           AND ifi.policy_ignore IS NULL
     AND (ifi.lead_agent_cnt = @Agent_Key OR @Agent_Key IS NULL OR @Agent_Key = 0)
     AND (ifi.Source_Id IN (SELECT s.source_id FROM   source s WHERE  source_id NOT IN (SELECT source_id FROM pmuser_source WHERE [user_id] =@user_id AND is_deleted = 0 )) or @user_id is null)
	 AND (ISNULL(base_ifi.insurance_file_type_id, 0) NOT IN (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code IN ('MTAQUOTE','MTAQREINS','MTAQCAN')) OR ifi.insurance_file_cnt = ifi.Base_Insurance_File_Cnt)
      
    ORDER  BY IFI.Insurance_Folder_cnt DESC,
              ifi.Insurance_File_Cnt DESC
      
      DECLARE @insurance_folder_key int  
      DECLARE @CurrrentInsuranceFileKey int
	  DECLARE cur_UpdateIsCurrent cursor fast_forward FOR  
			SELECT DISTINCT (InsuranceFolderKey) FROM #TEMPPOLICIES WHERE PolicyTypeId IN (2,5,8,9) AND PolicyStatusCode NOT IN ('CAN')  
			OPEN cur_UpdateIsCurrent  
				FETCH NEXT FROM cur_UpdateIsCurrent INTO @insurance_folder_key  
					WHILE (@@FETCH_STATUS = 0) BEGIN 
    
					SELECT TOP 1 @CurrrentInsuranceFileKey= isnull(InsuranceFileKey,0) FROM #TEMPPOLICIES WHERE PolicyTypeId IN (2,5,8,9) AND PolicyStatusCode NOT IN ('CAN')  
						AND InsuranceFolderKey = @insurance_folder_key AND IsReadOnly = 0 ORDER BY CoverStartDate desc, InsuranceFileKey desc
    
					UPDATE #TempPolicies SET CurrentInsuranceFile=@CurrrentInsuranceFileKey where InsuranceFolderKey=@insurance_folder_key
    
    
    FETCH NEXT FROM cur_UpdateIsCurrent  
            INTO @insurance_folder_key  
    END  
    CLOSE cur_UpdateIsCurrent  
    DEALLOCATE cur_UpdateIsCurrent  

     UPDATE #TempPolicies SET IsCurrent = 1 WHERE InsuranceFileKey = CurrentInsuranceFile
     
     UPDATE t SET RiskStatus= CASE WHEN ISNULL(risk_status_id,4)=4 THEN 'Unquoted'
				WHEN risk_status_id =2 THEN 'Declined'
				WHEN risk_status_id =1 THEN 'Referred'
				ELSE 'Quoted' END,
				EventDescription=(SELECT event_log.description
					 FROM   event_log
						WHERE  event_cnt = (SELECT MAX(event_cnt)
                                FROM   event_log el
                                WHERE  el.insurance_file_cnt = t.InsuranceFileKey))	
				FROM #TempPolicies t INNER JOIN insurance_file_risk_link ifrl ON t.InsuranceFileKey=ifrl.insurance_file_cnt INNER JOIN risk r ON r.risk_cnt=ifrl.risk_cnt 

    SELECT *
    FROM   #TempPolicies
    ORDER  BY InsuranceFileKey DESC

DROP TABLE #TempPolicies

