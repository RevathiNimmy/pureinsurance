SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Renewal_List'
GO

CREATE PROCEDURE spu_Report_Renewal_List
    @branch_id INT,
    @AllPolicies VARCHAR(10)
AS

DECLARE @InsuranceFileAccountExec BIT
DECLARE @MultiCompany BIT
DECLARE @IsProspect BIT
DECLARE @RenewalListOrder VARCHAR(2)


/*Initialise variables*/
SELECT @InsuranceFileAccountExec = 0
SELECT @MultiCompany = 0
SELECT @IsProspect = NULL


/*Validate parameters*/
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF LEFT(@AllPolicies, 1) = 'C'
BEGIN
    SELECT @IsProspect = 0
END

/*Do we get the account exec from the client or the policy?*/
SELECT 
    @InsuranceFileAccountExec = 1
WHERE EXISTS
(
    SELECT
        NULL
    FROM hidden_options
    WHERE option_number = 40
)

/*Is this a multi ledger system?*/
SELECT 
    @MultiCompany = 1
WHERE EXISTS
(
    SELECT
        NULL
    FROM hidden_options
    WHERE option_number = 16
)

IF @MultiCompany = 0
BEGIN
    /*Not multi-ledger, always use branch 1 for retrieving system options*/
    SELECT 
        @RenewalListOrder = value 
    FROM system_options 
    WHERE option_number = 2
    AND branch_id = 1
END
ELSE
BEGIN
    /*Multi-ledger, branch always passed in and always used for retrieving system options*/
    SELECT 
        @RenewalListOrder = value 
    FROM system_options 
    WHERE option_number = 2
    AND branch_id = @branch_id
END


CREATE TABLE #Renewals
(
    /*Client*/
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    
    /*Insurer*/
    insurer_code VARCHAR(20),
    insurer_name VARCHAR(255),
    
    /*Account Executive*/
    executive_code VARCHAR(20),
    executive_name VARCHAR(255),
            
    /*Account Handler*/
    handler_code VARCHAR(20),
    handler_name VARCHAR(255),
    
    /*Agent for Client*/
    agent_code VARCHAR(20),
    agent_name VARCHAR(255),
    
    /*Brokerage*/
    brokerage_code VARCHAR(20),
    brokerage_name VARCHAR(255),
    
    /*Policy*/
    policy_number VARCHAR(30),
    cover_start_date DATETIME,
    renewal_date DATETIME,
    renewal_year_month DATETIME,
    payment_method VARCHAR(60),
    this_premium MONEY,
    warning_message VARCHAR(255),
    comm MONEY,
    comm_perc FLOAT,
      
    /*Risk*/
    risk_code VARCHAR(10),
    risk_desc VARCHAR(255),
    risk_group_desc VARCHAR(255),
    
    /*Company_Details*/
    branch_id INT,
    branch_code VARCHAR(10),
    branch_desc VARCHAR(255),
    
    /*Currency Details*/
    currency_id SMALLINT,
    currency_code VARCHAR(10),
    currency_desc VARCHAR(255)
    
)

INSERT INTO #Renewals
SELECT 
    /*Client*/
    P_Cli.Shortname,
    P_Cli.resolved_name,
    
    /*Insurer*/
    P_Ins.shortname,
    P_Ins.resolved_name,
    
    /*Account Executive*/
    CASE @InsuranceFileAccountExec
        WHEN 1 THEN ISNULL(P_ExePol.shortname, '') 
        ELSE ISNULL(P_ExeCli.shortname, '') 
    END,
    CASE @InsuranceFileAccountExec 
        WHEN 1 THEN ISNULL(P_ExePol.resolved_name, '') 
        ELSE ISNULL(P_ExeCli.resolved_name, '') 
    END,
    
    /*Account Handler*/
    ISNULL(P_Han.shortname, ''),
    ISNULL(P_Han.resolved_name, ''),

    /*Agent for Client*/
    ISNULL(P_Age.shortname, ''),
    ISNULL(P_Age.resolved_name, ''),
    
    /*Brokerage*/
    ISNULL(P_Bro.shortname, ''),
    ISNULL(P_Bro.resolved_name, ''),
    
    /*Policy*/
    IFI.insurance_ref,
    IFI.cover_start_date,
    IFI.renewal_date,
    DATEADD(d,-DAY(ifi.renewal_date)+1,IFI.renewal_date),
    IFI.payment_method,
    IFI.this_premium,
    RSC.description,
    CASE 
        WHEN P_Ins.shortname LIKE 'MULTI%' THEN 
            (
                SELECT 
                    SUM(coinsurer_commission_amount) 
                FROM policy_coinsurers 
                WHERE insurance_file_cnt = ifi.insurance_file_cnt
            ) 
        ELSE IFI.commission_amount 
    END,
    CASE 
        WHEN P_Ins.shortname LIKE 'MULTI%' THEN 
            (
                SELECT 
                    ROUND(AVG((coinsurer_commission_amount/coinsurer_value)*100),2) 
                FROM policy_coinsurers 
                WHERE insurance_file_cnt = ifi.insurance_file_cnt 
                AND coinsurer_value > 0
            )
        ELSE IFI.commission_percentage
    END,
    
    /*Risk*/
    RC.code,
    RC.description,
    RG.description,

    /*Company_Details*/
    S.source_id,
    S.code,
    S.description,

    /*Currency Details*/
    C.currency_id,
    C.code,
    C.description
    
    
FROM insurance_file IFI
JOIN insurance_folder IFO
    ON IFO.insurance_folder_cnt = IFI.insurance_folder_cnt
JOIN party P_Cli
    ON P_Cli.party_cnt = IFO.insurance_holder_cnt
JOIN party P_Ins
    ON P_Ins.party_cnt = IFI.lead_insurer_cnt
LEFT JOIN party P_Han
    ON P_Han.party_cnt = IFI.account_handler_cnt 
LEFT JOIN party P_ExeCli
    ON P_ExeCli.party_cnt = P_Cli.consultant_cnt
LEFT JOIN Party P_ExePol
    ON P_ExePol.party_cnt = IFI.account_executive_cnt
LEFT JOIN Party P_Age
    ON P_Age.party_cnt = P_Cli.agent_cnt
LEFT JOIN Party P_Bro
    ON P_Bro.party_cnt = IFI.broker_cnt    
JOIN risk_code RC
    ON RC.risk_code_id = IFI.risk_code_id
JOIN risk_group RG
    ON RG.risk_group_id = RC.risk_group_id
LEFT JOIN renewal_stop_code RSC
    ON RSC.renewal_stop_code_id = IFI.renewal_stop_code_id 
JOIN source S
    ON S.source_id = IFI.source_id
JOIN currency C
    ON C.currency_id = IFI.currency_id
WHERE P_Cli.party_type_id IN (1, 2, 4) /*Personal, Group and Corporate Clients*/
AND IFI.insurance_file_type_id IN (2, 5, 6, 7) /*Live Policy, Permanent MTA, Temporary MTA and Temporary MTA Quotation*/
AND ISNULL(IFI.insurance_file_status_id, 3) = 3 /*Live and Under Renewal*/
AND ISNULL(IFI.policy_ignore, 0) <> 1
AND IFI.policy_version = 
    (
        SELECT 
            MAX(policy_version) 
        FROM insurance_file
        WHERE insurance_folder_cnt = IFO.insurance_folder_cnt
        AND insurance_file_type_id NOT IN (3, 4, 11) /*Policy Under Renewal, MTA Quotation and Renewal What If Quotation*/
    )
AND IFI.source_id = ISNULL(@branch_id, IFI.source_id)
AND P_Cli.is_prospect = ISNULL(@IsProspect, P_Cli.is_prospect)


/*Retrieve the data in the correct order*/
SELECT
    *
FROM #Renewals
ORDER BY
    renewal_year_month,
    CASE @RenewalListOrder 
        WHEN 0 THEN client_code
        WHEN 1 THEN RIGHT('0' + DATENAME(dd, renewal_date), 2)
        WHEN 2 THEN insurer_code
        WHEN 3 THEN brokerage_code
        WHEN 4 THEN executive_code
        WHEN 5 THEN handler_code
        WHEN 6 THEN agent_code
        ELSE client_code
    END,
    CASE @RenewalListOrder
        WHEN 0 THEN ''
        WHEN 1 THEN client_code
        WHEN 2 THEN client_code
        WHEN 3 THEN client_code
        WHEN 4 THEN client_code
        WHEN 5 THEN client_code
        WHEN 6 THEN client_code  
        ELSE ''
    END

DROP TABLE #Renewals


GO
