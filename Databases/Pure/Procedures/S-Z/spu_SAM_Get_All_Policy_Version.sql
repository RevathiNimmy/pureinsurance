SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDROPPROCEDURE 'spu_SAM_Get_All_Policy_Version'
GO

CREATE PROCEDURE spu_SAM_Get_All_Policy_Version 
					@InsuranceFolderCnt INT,
					@nUserId INT = NULL,
					@RetrieveAssociates TINYINT=0
AS

  DECLARE @PreviousVersionInstalmentPlanStatus varchar(10)

  SELECT
		@PreviousVersionInstalmentPlanStatus = prem2.StatusInd
		FROM Insurance_File IFL
		LEFT JOIN insurance_file_risk_link AS ifrl
		ON ifl.insurance_file_cnt = ifrl.insurance_file_cnt
		LEFT OUTER JOIN insurance_file_risk_link AS ifrl2
		ON ifrl.original_risk_cnt = ifrl2.risk_cnt
		LEFT OUTER JOIN pfpremiumfinance AS prem2
		ON prem2.insurance_file_cnt = ifrl2.insurance_file_cnt
		WHERE IFL.insurance_folder_cnt = @InsuranceFolderCnt

  Declare @PolicyVersions Table  
  (  
    InsuranceFolderKey INT,  
    insuranceFileKey INT Primary KEY,  
    InsuranceHolderKey INT,  
    PolicyTypeCode VARCHAR(10),  
    PolicyRef VARCHAR(255),  
    InsuranceFileTypeDesc VARCHAR(255),  
    ProductDesc VARCHAR(255),  
    RenewalDate DateTime,  
    PartyShortName CHAR(20),  
    Premium NUMERIC(19,4),  
    InsuranceFileTypeCode VARCHAR(10),  
    InsuranceFileTypeKey INT,  
    CoverStartDate DateTime,  
    ExpiryDate DateTime,  
    QuoteExpiryDate DateTime,  
    DateIssued DateTime,  
    TaxAmount  NUMERIC(19,4),  
    GracePeriod INT,  
    ProductCode VARCHAR(10),  
    PolicyVersion VARCHAR(10),  
    PaymentMethod VARCHAR(60),  
    Insurer VARCHAR(387),  
    Handler VARCHAR(387),  
    Branch VARCHAR(255),  
    SchemeName VARCHAR(25),  
    Regarding VARCHAR(255),  
    PaymentFrequency VARCHAR(255),  
    LegalExpenseProvider VARCHAR(387),  
    InstalmentPlanStatus VARCHAR(10),  
    PreviousVersionInstalmentPlanStatus VARCHAR(10),  
    AlternativeRef VARCHAR(1024),  
    PolicyStatus VARCHAR(255),  
    LapseCancelDate DateTime,  
    InsuredPersons VARCHAR(387),  
    Intermediary VARCHAR(387),  
    Regarding2 VARCHAR(255),  
    Currency VARCHAR(255),  
    TransactionDate DateTime,  
    party_cnt INT,  
    PolicyStatusCode VARCHAR(10),  
    IsCurrent VARCHAR(10),  
    IsMarketPlacePolicy INT,  
    IsMigratedPolicy TinyINT,  
    IsOutOfSequenceReplaced INT,  
    IsReadOnly INT,  
    EventDesc VARCHAR(MAX),  
    BaseInsuranceFileKey INT,  
    AgentKey INT,  
    AgentName VARCHAR(255),  
    MarkedQuoteForCollection TinyINT,  
    AssociatedClients VARCHAR(1024)  
  )  
  INSERT INTO @PolicyVersions 
  (
    InsuranceFolderKey,
    insuranceFileKey,
    InsuranceHolderKey, 
    PolicyTypeCode,
    PolicyRef,
    InsuranceFileTypeDesc,
    ProductDesc,
    RenewalDate ,
    PartyShortName,
    Premium,
    InsuranceFileTypeCode,
    InsuranceFileTypeKey,
    CoverStartDate,
    ExpiryDate,
    QuoteExpiryDate,
    DateIssued,
    TaxAmount,
    GracePeriod,
    ProductCode,
    PolicyVersion,
    PaymentMethod,
    Insurer,
    Handler,
    Branch,
    SchemeName,
    Regarding,
    PaymentFrequency,
    LegalExpenseProvider,
    InstalmentPlanStatus,
    PreviousVersionInstalmentPlanStatus,
    AlternativeRef,
    PolicyStatus,
    LapseCancelDate,
    InsuredPersons,
    Intermediary,
    Regarding2,
    Currency,
    TransactionDate,
    party_cnt,
    PolicyStatusCode,
    IsCurrent,
    IsMarketPlacePolicy,
    IsMigratedPolicy,
    IsOutOfSequenceReplaced, 
    IsReadOnly,
    BaseInsuranceFileKey,
    AgentKey,
    AgentName,
    MarkedQuoteForCollection,
    AssociatedClients
  )

  SELECT
    ifi.insurance_folder_cnt InsuranceFolderKey,
           ifi.insurance_file_cnt insuranceFileKey,
           ifo.insurance_holder_cnt InsuranceHolderKey,
           ptp.code PolicyTypeCode,
           ifi.insurance_ref PolicyRef,
           ift.description InsuranceFileTypeDesc,
           prd.description ProductDesc,
           ifi.renewal_date RenewalDate,
           pty.shortname PartyShortName,
           ifi.this_premium Premium,
           ift.code InsuranceFileTypeCode,
           ifi.insurance_file_type_id InsuranceFileTypeKey,
           ifi.cover_start_date CoverStartDate,
           ifi.expiry_date ExpiryDate,
           ifi.quote_expiry_date QuoteExpiryDate,
     ifs1.date_created AS DateIssued,
           ifi.tax_amount TaxAmount,
           prd.grace_period GracePeriod,
           prd.code ProductCode,
               CASE 
        WHEN ifi.insurance_file_type_id IN (10,7,12,4,3,1) 
        THEN 'V' + CAST(Policy_Version AS VARCHAR) + '-Q' + CAST(ISNULL(quote_version,0) AS VARCHAR)
        ELSE 'V' + CAST(Policy_Version AS VARCHAR)
    END AS PolicyVersion,
           ifi.payment_method PaymentMethod,
           isr.resolved_name Insurer,
           ah.resolved_name Handler,
    src.description AS Branch,
    '' AS SchemeName,
    ifo.description AS Regarding,
    (select Top 1 pff.description from PFPremiumFinance pfp
							LEFT JOIN PFRF pf ON pf.pfrf_id = pfp.pfrf_id  
							LEFT JOIN PFFrequency pff ON pff.pffrequency_id = pf.pFfrequency_id
							where Insurance_File_Cnt = ifi.insurance_file_cnt 
							order by pfprem_finance_version desc) PaymentFrequency, 
    (SELECT TOP 1
      party.resolved_name
            FROM policy_fee
    JOIN party
      ON policy_fee.party_cnt = party.party_cnt
    WHERE policy_fee.insurance_file_cnt = ifi.insurance_file_cnt
    AND party.party_type_id = 10)
    AS LegalExpenseProvider,

     (select Top 1 StatusInd from PFPremiumFinance where Insurance_File_Cnt = ifi.insurance_file_cnt order by pfprem_finance_version desc) InstalmentPlanStatus,                    
					(select Top 1 StatusInd from (	
													select Top 2 StatusInd,pfprem_finance_version from PFPremiumFinance 
													where Insurance_File_Cnt = ifi.insurance_file_cnt 													
													order by pfprem_finance_version desc
													) As PVIPS 
													where pfprem_finance_version <> (	select max(pfprem_finance_version) 
																						from PFPremiumFinance 
																						where Insurance_File_Cnt = ifi.insurance_file_cnt 
																					)
													order by pfprem_finance_version desc
					) PreviousVersionInstalmentPlanStatus, 
    ifi.alternate_reference AS 'AlternativeRef',
    ISNULL(ifs.description, 'Live') AS PolicyStatus,
      ifi.lapsed_date LapseCancelDate,  
      pty.resolved_name InsuredPersons,  
    ISNULL(ag.resolved_name, NULL) Intermediary,
      ifo.description Regarding,  
      cur.description Currency,  
    ifs1.last_trans_date TransactionDate,
    ag.party_cnt,
    ISNULL(ifs.code, 'Live') AS PolicyStatusCode,
    CASE
      WHEN ifi.insurance_file_cnt = CurrentVersion.CurrentInsuranceFile THEN 1
      ELSE 0
    END AS IsCurrent,
    ifi.is_marketplace_policy IsMarketPlacePolicy,
    CASE
      WHEN EXISTS (SELECT
          1
        FROM Insurance_File ifi
        WHERE ifi.insurance_file_cnt = rs.insurance_file_cnt
        AND ifi.insurance_file_cnt = rs.renewal_insurance_file_cnt) THEN 1
      ELSE 0
    END IsMigratedPolicy,
    ifi.out_of_sequence_replaced AS 'IsOutOfSequenceReplaced',
    CASE

      WHEN EXISTS (SELECT
          NULL
        FROM insurance_file ifirs
        WHERE ifirs.insurance_file_cnt = ifi.insurance_file_cnt
        AND (ISNULL(out_of_sequence_replaced, 0) = 1
        OR (ift.code IN ('MTAQUOTE','MTAQTETEMP','MTAQREINS','MTAQCAN')
        AND ISNULL(ifirs.insurance_file_status_id, 0) = 1)
        OR ((SELECT TOP 1
          insurance_file_cnt
        FROM insurance_file
        WHERE insurance_file_type_id = 8
        AND insurance_folder_cnt = ifi.insurance_folder_cnt
        AND ISNULL(base_insurance_file_cnt, insurance_file_cnt) = insurance_file_cnt
        ORDER BY insurance_file_cnt DESC)
        <> ifi.insurance_file_cnt
        AND ifi.insurance_file_type_id = 8)
        OR ifirs.source_id IN (SELECT
          source_id
        FROM pmuser_source
        WHERE [user_id] = @nUserId)
        OR EXISTS (SELECT
          NULL
        FROM insurance_file ifican
        WHERE ifi.insurance_file_type_id = 8
        AND ifican.insurance_file_cnt >
        ifi.insurance_file_cnt
		AND ift.insurance_file_type_id IN (2, 5, 9)
        AND ifican.insurance_folder_cnt = ifi.insurance_folder_cnt
        AND ISNULL(ifican.base_insurance_file_cnt, ifican.insurance_file_cnt) = ifican.insurance_file_cnt)
        )) THEN 1
      ELSE 0
    END IsReadOnly,
	ISNULL(ifi.Base_Insurance_File_Cnt,0)   BaseInsuranceFileKey,
	ag.party_cnt AgentKey,
	ag.resolved_name AgentName,
	ISNULL(ifi.marked_for_collection,0) MarkedQuoteForCollection,
	Cast(CASE @RetrieveAssociates
    WHEN 1 THEN 
        (SELECT  p.resolved_name +' ('+ AT.description + ')' Name 
    FROM insurance_file_associates Associate
    INNER JOIN party P ON Associate.party_cnt = P.party_cnt
    INNER JOIN Association_Type AT on Associate.Association_Type_id=AT.Association_Type_id 
    Where Associate.Insurance_file_cnt= ifi.insurance_file_cnt And
    (CASE  WHEN ISNUll(Associate.Is_Deleted,0) = 1  And ISNull(Associate.date_removed,Dateadd(year,-99,Getdate())) <= ifi.cover_start_date THEN 0   ELSE 1      END =1)
    FOR XML AUTO, TYPE )
    ELSE ''
    END As varchar(Max)) As AssociatedClients

      FROM Insurance_File ifi
  INNER JOIN Insurance_Folder ifo
    ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
  INNER JOIN Party pty
    ON ifo.insurance_holder_cnt = pty.party_cnt
  INNER JOIN Product prd
    ON ifi.product_id = prd.product_id
  INNER JOIN Insurance_File_Type ift
    ON ifi.insurance_file_type_id = ift.insurance_file_type_id
  INNER JOIN Source src
    ON ifi.source_id = src.source_id
  INNER JOIN Currency cur
    ON ifi.currency_id = cur.currency_id
  INNER JOIN Insurance_File_System ifs1
    ON ifi.insurance_file_cnt = ifs1.insurance_file_cnt
  LEFT JOIN Insurance_File_Status ifs
    ON ifi.insurance_file_status_id = ifs.insurance_file_status_id
  LEFT JOIN Policy_Type ptp
    ON ifi.policy_type_id = ptp.policy_type_id
  LEFT JOIN Party isr
    ON ifi.lead_insurer_cnt = isr.party_cnt
  LEFT JOIN Party ah
    ON ifi.account_handler_cnt = ah.party_cnt
  LEFT JOIN Party ag
    ON ifi.lead_agent_cnt = ag.party_cnt  
  LEFT OUTER JOIN (SELECT
    TOP 1 Insurance_File_cnt CurrentInsuranceFile
  FROM insurance_file
  WHERE insurance_folder_cnt = @InsuranceFolderCnt
  AND insurance_file_type_id IN (2, 5, 8, 9)
  AND ISNULL(out_of_sequence_replaced, 0) <> 1  AND (insurance_file_status_id > 2 OR insurance_file_status_id IS NULL)
  order by cover_start_date desc,insurance_file_cnt desc,renewal_date desc) CurrentVersion
    ON CurrentVersion.CurrentInsuranceFile = ifi.insurance_file_cnt
  LEFT JOIN renewal_status rs
    ON rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt
  
     WHERE ifi.insurance_folder_cnt = @InsuranceFolderCnt
       AND ifi.policy_ignore IS NULL
  
     ORDER BY ifi.insurance_file_cnt

  DECLARE   @EventTable  TABLE 
  (
	Event_cnt INT ,
	Insurance_file_cnt INT,
	Claim_cnt INT,
	is_manual_description INT,
	EventDesc VARCHAR(8000)
  );
  WITH EventCTE (Event_cnt,Insurance_file_cnt, Claim_cnt, is_manual_description, EventDesc)
  AS
  (
	Select Event_cnt, Insurance_file_cnt,Claim_cnt,is_manual_description, description 
	FROM event_log JOIN @PolicyVersions IFI ON IFI.insuranceFileKey = event_log.Insurance_file_cnt AND claim_cnt IS NULL
  )

 
  INSERT INTO @EventTable
  SELECT Event_cnt, Insurance_file_cnt, Claim_cnt, is_manual_description, EventCTE.EventDesc 
  FROM EventCTE  JOIN @PolicyVersions IFI ON IFI.insuranceFileKey = EventCTE.Insurance_file_cnt 

  WHERE (Event_cnt IN
    (SELECT MAX(Event_cnt) FROM EventCTE WHERE
    insurance_file_cnt= ifi.insuranceFileKey AND claim_cnt IS NULL AND is_manual_description =1  AND EventDesc NOT LIKE 'lapse%'
    --AND NOT EXISTS(SELECT 1 FROM EventCTE  WHERE insurance_file_cnt= ifi.insuranceFileKey AND EventDesc LIKE 'lapse%')
    )
    OR Event_cnt IN
     (SELECT TOP 1 max(event_cnt)
     FROM EventCTE
     WHERE claim_cnt IS NULL AND NOT EXISTS(SELECT 1 FROM EventCTE WHERE is_manual_description =1 AND insurance_file_cnt=ifi.insuranceFileKey)
     AND insurance_file_cnt=ifi.insuranceFileKey)
    OR
    Event_cnt IN
     (SELECT TOP 1 Event_cnt FROM EventCTE WHERE
     insurance_file_cnt= ifi.insuranceFileKey AND claim_cnt IS NULL AND EventDesc LIKE 'lapse%' AND is_manual_description =1))


	    
SELECT 
    InsuranceFolderKey,
    insuranceFileKey,
    InsuranceHolderKey, 
    PolicyTypeCode,
    PolicyRef,
    InsuranceFileTypeDesc,
    ProductDesc,
    RenewalDate ,
    PartyShortName,
    Premium,
    InsuranceFileTypeCode,
    InsuranceFileTypeKey,
    CoverStartDate,
    ExpiryDate,
    QuoteExpiryDate,
    DateIssued,
    TaxAmount,
    GracePeriod,
    ProductCode,
    PolicyVersion,
    PaymentMethod,
    Insurer,
    Handler,
    Branch,
    SchemeName,
    Regarding,
    PaymentFrequency,
    LegalExpenseProvider,
    InstalmentPlanStatus,
    PreviousVersionInstalmentPlanStatus,
    AlternativeRef,
    PolicyStatus,
    LapseCancelDate,
    InsuredPersons,
    Intermediary,
    Regarding2 AS Regarding,
    Currency,
    TransactionDate,
    party_cnt,
    PolicyStatusCode,
    IsCurrent,
    IsMarketPlacePolicy,
    IsMigratedPolicy,
    IsOutOfSequenceReplaced, 
    IsReadOnly,
    E.EventDesc,
    BaseInsuranceFileKey,
    AgentKey,
    AgentName,
    MarkedQuoteForCollection,
    AssociatedClients

 FROM @PolicyVersions P LEFT JOIN @EventTable E 
 ON P.insuranceFileKey = E.Insurance_file_cnt





