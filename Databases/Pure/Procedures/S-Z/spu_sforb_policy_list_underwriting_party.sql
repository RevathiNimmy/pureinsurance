EXECUTE DDLDropProcedure spu_sforb_policy_list_underwriting_party
GO
create PROCEDURE [dbo].[spu_sforb_policy_list_underwriting_party]


              @ins_file_type char(10),  
    @insurance_ref varchar(255) = NULL,  
    @party_cnt int,  
    @source_id smallint = NULL,  
    @transaction_type varchar(50) = NULL , 
	@RetrieveAssociates TINYINT=0 
 
AS
    DECLARE @multi_company INT

    
	 IF ISNULL(@transaction_type,'') IN ('NB','MTA')  
    BEGIN  
        SELECT @multi_company = SUM(CAST(value AS INT))  
        FROM Hidden_Options  
        WHERE (option_number = 16 OR option_number = 37)  
        AND value = '1'  
        IF @multi_company IS NULL  
            BEGIN  
                SELECT @multi_company = 0  
            END  
    END  
 ELSE  
    BEGIN  
        SELECT @multi_company=0  
    END
    CREATE TABLE #Policies
    (
	insurance_file_id INT,
	ins_file_source_id INT,
	insurance_file_cnt INT,
	insurance_ref VARCHAR(100),
	insurance_folder_code VARCHAR(255),
	type_code VARCHAR(10),
	Insured_name VARCHAR(255),
	insured_shortname VARCHAR(20),
	Party_id INT,
	party_source_id INT,
       renewal_date DATETIME,
	   insurance_holder_cnt  INT,
        insurance_folder_cnt INT,
		product_id  INT,
		code VARCHAR(10),
		caption VARCHAR(255),
		lead_agent_cnt INT,
		date_created DATETIME,
		status_code VARCHAR(10),
		agent_name VARCHAR(20),
		this_premium NUMERIC(19,4),
		policy_type_id INT,
		policy_type  VARCHAR(255),


		type_desc  VARCHAR(255),
		
        policy_type_code  VARCHAR(10),
        
        OpenPolicyClaims INT,
        ClosePolicyClaims INT,
        cover_start_date DATETIME,
        expiry_date DATETIME,
        Marked_For_Collection INT,
        quote_status_id INT,
        quote_version INT,
        base_insurance_folder_cnt INT,
        RenewedVersion INT,
        is_marketplace_policy INT,
        Correspondence_Type INT,
        Default_Preferred_Correspondence INT,
        Is_Agent_Correspondence TINYINT,
        AssociatedClients XML,
        MasterPartyResolve_name VARCHAR(387),
        TypeOfPolicy	VARCHAR(100),
        Event_Description VARCHAR(8000)
    )

    INSERT INTO #Policies
    (
        Insured_name,
        insured_shortname,
        Party_id,
        party_source_id,
        agent_name,
        insurance_file_id,
        ins_file_source_id,
        insurance_file_cnt,
        insurance_ref ,
        insurance_folder_code,
        type_code,
        renewal_date,
        insurance_holder_cnt,
        insurance_folder_cnt,
        product_id,
        code,
        caption,
        lead_agent_cnt,
        date_created,
        status_code,
        this_premium,
        policy_type_id,
        policy_type,
        policy_type_code,
        type_desc,
        OpenPolicyClaims,
        ClosePolicyClaims,
        cover_start_date,
        expiry_date,
        Marked_For_Collection,
        quote_status_id,
        quote_version,
        base_insurance_folder_cnt,
        RenewedVersion,
        is_marketplace_policy,
        Correspondence_Type,
        Default_Preferred_Correspondence,
        Is_Agent_Correspondence,
        AssociatedClients,
        MasterPartyResolve_name,
        TypeOfPolicy,
        Event_Description 
    )

    SELECT Party.name                                   AS insured_name,
           Party.shortname                              AS insured_shortname,
           Party.party_id,
           Party.source_id                              AS party_source_id,
           Party2.shortname                             AS agent_name,
           Insurance_File.insurance_file_id,
           Insurance_File.source_id                     AS ins_file_source_id,
           Insurance_File.insurance_file_cnt,
           Insurance_File.insurance_ref,
           Insurance_File_System.last_trans_description AS insurance_folder_code,
           Insurance_File_type.code                     AS type_code,
           Insurance_File.renewal_date,
           Insurance_Folder.insurance_holder_cnt,
           Insurance_Folder.insurance_folder_cnt,
           Insurance_File.product_id,
           Product.code,
           PMCaption.caption,
           Insurance_File.lead_agent_cnt,
           Insurance_File_System.date_created,
	   CASE   
           WHEN Product.change_policy_number_at_renewal = 1 THEN  ISNULL(ift.code,Insurance_File_status.code)  
           ELSE Insurance_File_status.code   
           END AS status_code,
           
           Insurance_File.this_premium,
           Insurance_File.policy_type_id,
           Policy_Type.description                      AS policy_type,
           Policy_Type.code                             AS policy_type_code,
           Insurance_File_type.description              AS type_desc,
           NULL  AS OpenPolicyClaims,
           NULL  AS ClosePolicyClaims,
           Insurance_File.cover_start_date,
           Insurance_File.expiry_date,
           Insurance_File.Marked_For_Collection,
           Insurance_File.quote_status_id,
           Insurance_File.quote_version,
           Insurance_File.base_insurance_folder_cnt,
           ( CASE
               WHEN ( Insurance_File.insurance_file_status_id IS NULL
                      AND Insurance_File.policy_version > 1 ) THEN 1
               ELSE 0
             END )  AS RenewedVersion,
           is_marketplace_policy,
		   Insurance_File.Correspondence_Type,
		   Insurance_File.Default_Preferred_Correspondence,
		   Insurance_File.Is_Agent_Correspondence,
           NULL,
           Party.resolved_name As MasterPartyResolve_name,
           'Main' AS  TypeOfPolicy,
            (SELECT  event_log.description    
                    FROM     event_log    
                    WHERE    event_cnt = (SELECT MAX(event_cnt)    
                    FROM     event_log el    
                    WHERE    el.insurance_file_cnt = Insurance_File.Insurance_File_cnt))

    FROM   Insurance_File
           INNER JOIN Insurance_Folder
                   ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
           INNER JOIN Insurance_File_System
                   ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
           INNER JOIN Insurance_File_Type
                   ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
           INNER JOIN Product
                   ON Product.product_id = Insurance_File.product_id
           INNER JOIN PMCaption
                   ON PMCaption.caption_id = Product.caption_id
           INNER JOIN Policy_Type
                   ON Insurance_file.policy_type_id = Policy_Type.policy_type_id
           INNER JOIN (SELECT MAX(ifi.insurance_file_cnt) AS insurance_file_cnt
                        FROM   Insurance_File AS ifi
                        INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                        INNER JOIN insurance_file_type IFT ON ifi.insurance_file_type_id=IFT.insurance_file_type_id
                       WHERE  (ifo.insurance_holder_cnt = @party_cnt ) --OR ifi.insurance_file_cnt in (Select Insurance_file_cnt From insurance_file_associates where party_cnt = @party_cnt And Is_Deleted<>1)) -- OR  ifi.insurance_file_cnt in (Apol.InsuranceFile_Cnt))
                              AND IFT.code IN ('QUOTE','POLICY','RENEWAL','MTA PERM','MTA TEMP','MTACAN','MTAREINS','WRITTEN')
                              AND ifi.policy_ignore IS NULL
							  AND ISNull(ifi.out_of_sequence_replaced, 0) <> 1
                              AND ( ifi.source_id = @source_id
                                     OR @multi_company <> 2 )
                       GROUP  BY ifi.insurance_ref) AS PL
                   ON Insurance_File.insurance_file_cnt = PL.insurance_file_cnt
				    LEFT JOIN  (select insurance_file_cnt,insurance_ref ,ifs.code code from insurance_file i
		              INNER JOIN Insurance_Folder AS ifo ON i.insurance_folder_cnt = ifo.insurance_folder_cnt
		              JOIN   Insurance_File_Status  ifs on i.insurance_file_status_id = ifs.insurance_file_status_id 
		              WHERE ifo.insurance_holder_cnt=@party_cnt	AND i.insurance_file_status_id IN (3,4) and insurance_file_type_id <>8  ) ift ON ift.insurance_ref=Insurance_File.insurance_ref
           INNER JOIN Party
                   ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
           LEFT OUTER JOIN Party AS Party2
                        ON Insurance_File.lead_agent_cnt = Party2.party_cnt
           LEFT OUTER JOIN Insurance_File_Status
                        ON Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id
    WHERE  Insurance_file.policy_ignore IS NULL
           AND ( party.party_cnt = @party_cnt ) --OR Insurance_file.insurance_file_cnt in (Select Insurance_file_cnt From insurance_file_associates where party_cnt = @party_cnt And Is_Deleted<>1))
    --ORDER  BY Insurance_File_System.date_created DESC
	UNION
	SELECT Party.name                                   AS insured_name,
           Party.shortname                              AS insured_shortname,
           Party.party_id,
           Party.source_id                              AS party_source_id,
           Party2.shortname                             AS agent_name,
           Insurance_File.insurance_file_id,
           Insurance_File.source_id                     AS ins_file_source_id,
           Insurance_File.insurance_file_cnt,
           Insurance_File.insurance_ref,
           Insurance_File_System.last_trans_description AS insurance_folder_code,
           Insurance_File_type.code                     AS type_code,
           Insurance_File.renewal_date,
           Insurance_Folder.insurance_holder_cnt,
           Insurance_Folder.insurance_folder_cnt,
           Insurance_File.product_id,
           Product.code,
           PMCaption.caption,
           Insurance_File.lead_agent_cnt,
           Insurance_File_System.date_created,
           CASE 
			WHEN Product.change_policy_number_at_renewal = 1 THEN  ISNULL(ift.code,Insurance_File_status.code)
			ELSE Insurance_File_status.code 
           END AS status_code,
           Insurance_File.this_premium,
           Insurance_File.policy_type_id,
           Policy_Type.description                      AS policy_type,
           Policy_Type.code                             AS policy_type_code,
           Insurance_File_type.description              AS type_desc,
           NULL  AS OpenPolicyClaims,
           NULL  AS ClosePolicyClaims,
           Insurance_File.cover_start_date,
           Insurance_File.expiry_date,
           Insurance_File.Marked_For_Collection,
           Insurance_File.quote_status_id,
           Insurance_File.quote_version,
           Insurance_File.base_insurance_folder_cnt,
           ( CASE
               WHEN ( Insurance_File.insurance_file_status_id IS NULL
                      AND Insurance_File.policy_version > 1 ) THEN 1
               ELSE 0
             END )  AS RenewedVersion,
           is_marketplace_policy,
		   Insurance_File.Correspondence_Type,
		   Insurance_File.Default_Preferred_Correspondence,
		   Insurance_File.Is_Agent_Correspondence,
           NULL,
           Party.resolved_name As MasterPartyResolve_name,
           'Main' AS  TypeOfPolicy,
            (SELECT  event_log.description    
                    FROM     event_log    
                    WHERE    event_cnt = (SELECT MAX(event_cnt)    
                    FROM     event_log el    
                    WHERE    el.insurance_file_cnt = Insurance_File.Insurance_File_cnt))

    FROM   Insurance_File
           INNER JOIN Insurance_Folder
                   ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
           INNER JOIN Insurance_File_System
                   ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
           INNER JOIN Insurance_File_Type
                   ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
           INNER JOIN Product
                   ON Product.product_id = Insurance_File.product_id
           INNER JOIN PMCaption
                   ON PMCaption.caption_id = Product.caption_id
           INNER JOIN Policy_Type
                   ON Insurance_file.policy_type_id = Policy_Type.policy_type_id
           INNER JOIN (SELECT MAX(ifi.insurance_file_cnt) AS insurance_file_cnt
                        FROM   Insurance_File AS ifi
                        INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                        INNER JOIN insurance_file_type IFT ON ifi.insurance_file_type_id=IFT.insurance_file_type_id
                       WHERE  (ifi.insurance_file_cnt in (Select Insurance_file_cnt From insurance_file_associates where party_cnt = @party_cnt And Is_Deleted<>1)) -- OR  ifi.insurance_file_cnt in (Apol.InsuranceFile_Cnt))
                              AND IFT.code IN ('QUOTE','POLICY','RENEWAL','MTA PERM','MTA TEMP','MTACAN','MTAREINS','WRITTEN')
                              AND ifi.policy_ignore IS NULL
							  AND ISNull(ifi.out_of_sequence_replaced, 0) <> 1
                              AND ( ifi.source_id = @source_id
                                     OR @multi_company <> 2 )
                       GROUP  BY ifi.insurance_ref) AS PL
                   ON Insurance_File.insurance_file_cnt = PL.insurance_file_cnt
				    LEFT JOIN  (select insurance_file_cnt,insurance_ref ,ifs.code code from insurance_file i
		              INNER JOIN Insurance_Folder AS ifo ON i.insurance_folder_cnt = ifo.insurance_folder_cnt
		              JOIN   Insurance_File_Status  ifs on i.insurance_file_status_id = ifs.insurance_file_status_id 
		              WHERE ifo.insurance_holder_cnt=@party_cnt	AND i.insurance_file_status_id IN (3,4) and insurance_file_type_id <>8  ) ift ON ift.insurance_ref=Insurance_File.insurance_ref
           INNER JOIN Party
                   ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
           LEFT OUTER JOIN Party AS Party2
                        ON Insurance_File.lead_agent_cnt = Party2.party_cnt
           LEFT OUTER JOIN Insurance_File_Status
                        ON Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id
    WHERE  Insurance_file.policy_ignore IS NULL
           AND (Insurance_file.insurance_file_cnt in (Select Insurance_file_cnt From insurance_file_associates where party_cnt = @party_cnt And Is_Deleted<>1))
    ORDER  BY Insurance_File_System.date_created DESC

    UPDATE P
    SET AssociatedClients = (
                        SELECT
                        Associate.resolved_name Name
                        FROM insurance_file_associates IFA
                        INNER JOIN Party Associate ON IFA.party_cnt = Associate.party_cnt
                        Where Ifa.Insurance_file_cnt=P.insurance_file_cnt AND
                        (CASE  WHEN ISNUll(IFA.Is_Deleted,0) = 1  And ISNull(IFA.date_removed,Dateadd(year,-99,Getdate())) <= GETDATE() THEN 0   ELSE 1 END =1)
                        FOR XML AUTO, TYPE
        ) FROM #Policies P

    Create Table #Claims
    (
        Claim_id INT,
        Claim_number VARCHAR(30),
        policy_Number VARCHAR(30),
        claim_status_id INT,
        is_dirty INT
    )
    INSERT INTO #Claims (Claim_id)
    SELECT MAX(Claim_id) FROM Claim C
    JOIN #Policies P ON C.Policy_Number = P.Insurance_ref
    GROUP  BY POLICY_NUMBER, base_claim_id

    UPDATE CT
    SET CT.Claim_Number = C.Claim_Number, CT.policy_Number = C.Policy_Number,
        CT.claim_status_id = C.Claim_Status_id, CT.is_dirty = C.is_dirty
    FROM #Claims CT
    JOIN Claim C ON CT.Claim_id = C.Claim_id

    UPDATE P
    SET OpenPolicyClaims = (SELECT Count(*) FROM #Claims CT WHERE CT.policy_Number= P.Insurance_ref  AND claim_status_id <> 3 AND is_dirty =0 )
    FROM #Policies P

    UPDATE P
    SET ClosePolicyClaims = (SELECT Count(*) FROM #Claims CT WHERE CT.policy_Number= P.Insurance_ref  AND claim_status_id = 3 )
    FROM #Policies P

    SELECT * FROM #Policies ORDER  BY insurance_folder_cnt DESC, date_created DESC
GO
