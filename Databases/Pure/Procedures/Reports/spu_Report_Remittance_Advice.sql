/*
This stored procedure is used by the following reports:

RemittanceAdvice.rpt
ManualRemittanceAdvice.rpt (called by spu_Report_Manual_Remittance_Advice)
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Remittance_Advice'
GO

CREATE PROCEDURE spu_Report_Remittance_Advice 
    @cashlist_id INT
AS

SET NOCOUNT ON

DECLARE @NZConfig bit

DECLARE @PartyTypeCode VARCHAR(10)
DECLARE @ReportIndicator VARCHAR(2)
DECLARE @CompanyPremiumSection INT
DECLARE @CompanyEarthquakeSection INT
DECLARE @EarthquakeLevySection INT
DECLARE @FireLevySection INT

IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number=86 AND value='1')
	SELECT @NZConfig=1
	
SELECT @PartyTypeCode = RTRIM(PT.code)
FROM Party_Type PT
JOIN Party P 
    ON P.party_type_id = PT.party_type_id
JOIN Account A
    ON A.account_key = P.party_cnt
JOIN CashListItem CLI
    ON CLI.account_id = A.account_id
WHERE CLI.cashlist_id = @cashlist_id

IF @PartyTypeCode = 'AG'
BEGIN /*Agent*/
    SELECT @ReportIndicator = 'A' + CAST(PA.report_indicator AS CHAR(1))
    FROM Party_Agent PA
    JOIN Party P 
        ON P.party_cnt = PA.party_cnt
    JOIN Account A
        ON A.account_key = P.party_cnt
    JOIN CashListItem CLI
        ON CLI.account_id = A.account_id
    WHERE CLI.cashlist_id = @cashlist_id
END
ELSE 
BEGIN /*Insurer*/
    SELECT @ReportIndicator = 'I' + CAST(PI.report_indicator AS CHAR(1))
    FROM Party_Insurer PI
    JOIN Party P 
        ON P.party_cnt = PI.party_cnt
    JOIN Account A
        ON A.account_key = P.party_cnt
    JOIN CashListItem CLI
        ON CLI.account_id = A.account_id
    WHERE CLI.cashlist_id = @cashlist_id
END

SELECT @CompanyPremiumSection=COB_rating_section_id
FROM COB_rating_section
WHERE code='COPREM'

SELECT @CompanyEarthquakeSection=COB_rating_section_id
FROM COB_rating_section
WHERE code='CEPREM'

SELECT @EarthquakeLevySection=COB_rating_section_id
FROM COB_rating_section
WHERE code='EQLEVY'

SELECT @FireLevySection=COB_rating_section_id
FROM COB_rating_section
WHERE code='FSLEVY'

IF @CompanyPremiumSection IS NULL
	SELECT @CompanyPremiumSection=-1
	
IF @CompanyEarthquakeSection IS NULL
	SELECT @CompanyEarthquakeSection=-1
	
IF @EarthquakeLevySection IS NULL
	SELECT @EarthquakeLevySection=-1

IF @FireLevySection IS NULL
	SELECT @FireLevySection=-1
	
CREATE TABLE #Report_Temp
(
    Document_Date DATETIME,
    Media_Ref VARCHAR(100),
    CashList_Amount MONEY,
    CashList_Currency VARCHAR(4),
    Insurer VARCHAR(255),
    Address1 VARCHAR(60),
    Address2 VARCHAR(60),
    Address3 VARCHAR(60),
    Address4 VARCHAR(60),
    Postal_Code VARCHAR(20),
    Document_Ref VARCHAR(20),
    Document_ID INT,
    Policy_Doc_Date DATETIME,
    Policy_Eff_Date DATETIME,
    Document_Type VARCHAR(255),
    Client VARCHAR(255),
    Client_Code CHAR(20),
    Ledger_ID SMALLINT,
    Policy_Ref VARCHAR(30),
    CoInsurer_Policy_Ref VARCHAR(30),   
    Premium MONEY,
    Commission MONEY,
    Fee MONEY,
    Comm MONEY,
    IPT MONEY,
    This_Payment MONEY,
    Total_Payment MONEY,
    Sort_Column VARCHAR(255),
    Agent_Code VARCHAR(30),
    Agent_Name VARCHAR(255),
    Agent_Sort INT,
    currency_id INT,
    currency_code VARCHAR(10),
    currency_desc VARCHAR(255),
    Branch VARCHAR(255),
    BranchAddress1 VARCHAR(60),
    BranchAddress2 VARCHAR(60),
    BranchAddress3 VARCHAR(60),
    BranchAddress4 VARCHAR(60),
    BranchPostalCode VARCHAR(20),
    show_third_party BIT,
    Company_Premium MONEY,
	Company_Earthquake MONEY,
	Earthquake_Levy MONEY,
	Fire_Levy MONEY,
	Client_Tax MONEY,
	Client_Total_Amount MONEY,
	Broker_Fee MONEY,
	Broker_Tax MONEY,
	Broker_Total_Amount MONEY,
	Payment_Ref VARCHAR(20),
	Insurer_Tax_Number VARCHAR(50),
	Insurer_Code CHAR(20),
	Branch_Tax_Number VARCHAR(20),
	Insurer_Agency VARCHAR(255),
	Underwriter CHAR(20),
	Part_Paid INT,
	Balance_OS MONEY
)

INSERT INTO #Report_Temp
SELECT DISTINCT
    D.document_date,
    ISNULL(I.media_ref, ''),
    (
        SELECT 
            ISNULL(SUM(ROUND(cli.amount,2)),0)
        FROM Cashlistitem cli 
        WHERE cli.cashlist_id = C.cashlist_id
    ) ,
    CU2.code,
    ISNULL(P.name, ''),
    ISNULL(AD.address1, ''),
    ISNULL(AD.address2, ''),
    ISNULL(AD.address3, ''),
    ISNULL(AD.address4, ''),
    ISNULL(AD.postal_code, ''),
    PaidD.document_ref,
    PaidD.document_id,
    PaidD.document_date,
    (
        SELECT
            MIN(ref_date)
        FROM transdetail
        WHERE document_id = PaidD.document_id
        AND document_sequence = 1
    ),
    DT.description,
    (
        SELECT 
            ISNULL(P1.resolved_name, '')
        FROM Transdetail TD1
        JOIN Account A1
            ON A1.account_id = TD1.account_id
        JOIN Party P1
            ON P1.party_cnt = A1.account_key
        WHERE TD1.document_id = PaidD.document_id
        AND TD1.document_sequence = 
            (   
                SELECT 
                    MIN(document_sequence)
                FROM transdetail     
                WHERE document_id = td1.document_id 
                AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            )
    ),
    (
        SELECT 
            ISNULL(A1.short_code, '')
        FROM Transdetail TD1
        JOIN Account A1
            ON A1.account_id = TD1.account_id
        WHERE TD1.document_id = PaidD.document_id
        AND TD1.document_sequence = 
            (   
                SELECT 
                    MIN(document_sequence)
                FROM transdetail     
                WHERE document_id = td1.document_id 
                AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            )
    ),
    (
        CASE l.ledger_short_name
            WHEN 'NO' THEN 1
            WHEN 'SA' THEN 2
            WHEN 'PU' THEN 3
            WHEN 'IN' THEN 4
            WHEN 'AG' THEN 5
            WHEN 'RF' THEN 6
            WHEN 'FE' THEN 7 
            WHEN 'DI' THEN 8
            WHEN 'CO' THEN 9
            WHEN 'UB' THEN 10
            WHEN 'TR' THEN 11
            ELSE 0 
        END
    ),
    (
        SELECT 
            ISNULL(MAX(insurance_ref), '')
        FROM TransDetail
        WHERE transdetail_id = PaidTD.transdetail_id 
    ),
    (
        SELECT
            ISNULL(MAX(PCO.coinsurer_policy_number),'')
        FROM TransDetail TD
        LEFT JOIN Insurance_File INS
            ON INS.insurance_ref = TD.insurance_ref
        LEFT JOIN Policy_Coinsurers PCO
            ON PCO.insurance_file_cnt = INS.insurance_file_cnt
        WHERE TD.transdetail_id = PaidTD.transdetail_id 
        AND PCO.party_cnt = P.party_cnt
        AND INS.policy_version IN 
            (
                SELECT 
                    MAX(policy_version)
                FROM insurance_file
                WHERE insurance_ref = PaidTD.insurance_ref
            ) 
    ),   
    (   
        SELECT
            SUM(-1 * ISNULL(ROUND(currency_amount,2), 0))
        FROM transdetail td2
        WHERE td2.document_id = PaidD.document_id
        AND td2.document_sequence = 
            (
                SELECT 
                    MIN(document_sequence)
                FROM transdetail     
                WHERE document_id = td2.document_id 
                AND account_id = PaidTD.account_id
            )
    ),
    (   
        SELECT
            ISNULL(SUM(ISNULL(ROUND(currency_amount,2), 0)),0)
        FROM transdetail TDComm
        JOIN account AComm
            ON TDComm.account_id = AComm.account_id
        JOIN Document DComm
            ON TDComm.document_id = DComm.document_id
        WHERE TDComm.document_id = PaidD.document_id
        AND (
                (   
                    TDComm.spare LIKE '%AGENT%'
                    OR
                    TDComm.spare like '%COMM %'
                    OR
                    TDComm.spare like 'COMM'                    
                )
                OR
                (   
                    Dcomm.documenttype_id IN (8, 10, 11, 12, 20, 21)
                )
            )
        AND TDComm.account_id = A.account_id
        AND AComm.ledger_id = A.ledger_id
    ),
    (
        SELECT 
            ISNULL(SUM(ROUND(tdf.currency_amount,2)),0) * -1
        FROM transdetail tdf
        JOIN transdetail_type ttf
            ON ttf.transdetail_type_id = tdf.transdetail_type_id
        WHERE tdf.document_id = PaidD.document_id
        AND tdf.account_id = A.account_id
        AND ttf.code = 'IFEE'
    ),
    0,
    (   
        SELECT  
            ISNULL(SUM(ROUND(ISNULL(ROUND(TD1.ref_amount,2) * (ROUND(TD1.amount,2)/ABS(ROUND(TD1.amount,2))), 0),2)),0)
        FROM AllocationDetail AL1
        JOIN TransDetail TD1
            ON AL1.transdetail_id = TD1.transdetail_id
        JOIN transdetail_type ttf
            ON ttf.transdetail_type_id = TD1.transdetail_type_id
        WHERE TD1.document_id = PaidD.document_id
        AND AL1.cashlistitem_id = I.cashlistitem_id
        AND TD1.amount <> 0
        AND TD1.ref_amount <> 0
    AND ttf.code IN ('GROSS','AGENT')
    ),
    (   
        SELECT 
            SUM(ISNULL(ROUND(AL1.alloc_ccy_amount,2), 0))
        FROM AllocationDetail AL1
        JOIN TransDetail TD1
            ON AL1.transdetail_id = TD1.transdetail_id
        WHERE TD1.document_id = PaidD.document_id
        AND AL1.cashlistitem_id = I.cashlistitem_id
    ),
    (   
        SELECT 
            SUM(ISNULL(ROUND(TM1.currency_match_amount,2), 0))
        FROM Transmatch TM1
        JOIN TransDetail TD1
            ON tm1.transdetail_id = TD1.transdetail_id
        JOIN AllocationDetail AL1
            ON AL1.transdetail_id = TD1.transdetail_id
        WHERE TD1.document_id = PaidD.document_id
        AND AL1.cashlistitem_id = I.cashlistitem_id
        AND TM1.allocationdetail_id IS NOT NULL
    ),
    (
        CASE
            WHEN (@ReportIndicator = 'I0' OR @ReportIndicator = 'A0') THEN
                (
                    SELECT 
                        ISNULL(A1.short_code, '')
                    FROM Transdetail TD1
                    JOIN Account A1
                        ON A1.account_id = TD1.account_id
                    WHERE TD1.document_id = PaidD.document_id
                    AND TD1.document_sequence = 
                        (   
                            SELECT 
                                MIN(document_sequence)
                            FROM transdetail     
                            WHERE document_id = td1.document_id 
                            AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                        )
                ) /*Should be payment date but all items have just been paid today.*/
            WHEN (@ReportIndicator = 'I1' OR @ReportIndicator = 'A1') THEN 
                PaidTD.insurance_ref
            WHEN (@ReportIndicator = 'I2' OR @ReportIndicator = 'A2') THEN
                (
                    SELECT 
                        ISNULL(A1.short_code, '')
                    FROM Transdetail TD1
                    JOIN Account     A1
                        ON A1.account_id = TD1.account_id
                    WHERE TD1.document_id = PaidD.document_id
                    AND TD1.document_sequence = 
                        (   
                            SELECT 
                                MIN(document_sequence)
                            FROM transdetail     
                            WHERE document_id = td1.document_id 
                            AND ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                        )
                )
            WHEN (@ReportIndicator = 'I3') THEN 
                CONVERT(VARCHAR(20),TransExp.cover_start_date,120)
            WHEN (@ReportIndicator = 'A3') THEN 
                CONVERT(VARCHAR(20),InsFile.renewal_date,120)
            WHEN (@ReportIndicator = 'A4') THEN 
                RC.description
            ELSE 
                (
                    SELECT 
                        ISNULL(A1.short_code, '')
                    FROM Transdetail TD1
                    JOIN Account A1
                        ON A1.account_id = TD1.account_id
                    WHERE TD1.document_id = PaidD.document_id
                    AND TD1.document_sequence = 
                    (   
                        SELECT
                            MIN(document_sequence)
                        FROM transdetail     
                        WHERE document_id = td1.document_id 
                        AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
                )
        END     
    ) Sort_Column,
    '',
    '',
    0,
    CU.currency_id,
    CU.code,
    CU.description,
    ISNULL(S.description, ''),
    ISNULL(S.address1, ''),
    ISNULL(S.address2, ''),
    ISNULL(S.address3, ''),
    ISNULL(S.address4, ''),
    ISNULL(S.postal_code, ''),
    (
        SELECT
            ISNULL(MAX(1), 0)
        WHERE @PartyTypeCode = 'IN'
        AND EXISTS
                (
                    SELECT
                        NULL
                    FROM system_options
                    WHERE option_number = 5006
                    AND value = '1'
                    
                )
    ),
    CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN
	(
	SELECT SUM(Premium_Excluding_Tax) FROM
	event_insurance_COB_section
	WHERE insurance_file_cnt=EIF.insurance_file_cnt AND COB_rating_section_id NOT IN (@CompanyEarthquakeSection,@EarthquakeLevySection,@FireLevySection)
	)
	ELSE
	(
	SELECT SUM(Premium_Exc_Tax) FROM
	event_policy_coinsurers_section
	JOIN COB_Rating_Section ON event_policy_coinsurers_section.COB_rating_section_id=COB_Rating_Section.COB_rating_section_id
	WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
	AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
	AND event_policy_coinsurers_section.COB_rating_section_id NOT IN (@CompanyEarthquakeSection,@EarthquakeLevySection,@FireLevySection)
	)
	END,
	CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN
	(
	SELECT SUM(Premium_Excluding_Tax) FROM
	event_insurance_COB_section
	WHERE insurance_file_cnt=EIF.insurance_file_cnt AND COB_rating_section_id=@CompanyEarthquakeSection
	)
	ELSE
	(
	SELECT SUM(Premium_Exc_Tax) FROM
	event_policy_coinsurers_section
	JOIN COB_Rating_Section ON event_policy_coinsurers_section.COB_rating_section_id=COB_Rating_Section.COB_rating_section_id
	WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
	AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
	AND event_policy_coinsurers_section.COB_rating_section_id=@CompanyEarthquakeSection
	)
	END,
	CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN
	(
	SELECT SUM(Premium_Excluding_Tax) FROM
	event_insurance_COB_section
	WHERE insurance_file_cnt=EIF.insurance_file_cnt AND COB_rating_section_id=@EarthquakeLevySection
	)
	ELSE
	(
	SELECT SUM(Premium_Exc_Tax) FROM
	event_policy_coinsurers_section
	JOIN COB_Rating_Section ON event_policy_coinsurers_section.COB_rating_section_id=COB_Rating_Section.COB_rating_section_id
	WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
	AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
	AND event_policy_coinsurers_section.COB_rating_section_id=@EarthquakeLevySection
	)
	END,
	CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN
	(
	SELECT SUM(Premium_Excluding_Tax) FROM
	event_insurance_COB_section
	WHERE insurance_file_cnt=EIF.insurance_file_cnt AND COB_rating_section_id=@FireLevySection
	)
	ELSE
	(
	SELECT SUM(Premium_Exc_Tax) FROM
	event_policy_coinsurers_section
	JOIN COB_Rating_Section ON event_policy_coinsurers_section.COB_rating_section_id=COB_Rating_Section.COB_rating_section_id
	WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
	AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
	AND event_policy_coinsurers_section.COB_rating_section_id=@FireLevySection
	)
	END,
    CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN EIF.tax_amount
	ELSE
	(
	SELECT SUM(Premium_Inc_Tax)-SUM(Premium_Exc_Tax) FROM
	event_policy_coinsurers_section
	WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
	AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
	)
	END,
    CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN EIF.this_premium
	ELSE
	(
	SELECT coinsurer_value FROM
	event_policy_coinsurers
	WHERE event_policy_coinsurers.insurance_file_cnt=EIF.insurance_file_cnt
	AND event_policy_coinsurers.party_cnt=CoIns.party_cnt
	)
	END,
    CASE @PartyTypeCode
    WHEN 'IN' THEN
	CASE ISNULL(CoIns.party_cnt,-1)
		WHEN -1 THEN
			(SELECT SUM(commission_net) FROM
			event_insurance_COB_section
			WHERE insurance_file_cnt=EIF.insurance_file_cnt
			AND PaidD.documenttype_id NOT IN (33,34))
		ELSE
			(SELECT SUM(commission_exc_tax) FROM
			event_policy_coinsurers_section
			WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
			AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
			AND PaidD.documenttype_id NOT IN (33,34))
		END
    ELSE EPA.agent_commission_value
    END,
    CASE @PartyTypeCode
    WHEN 'IN' THEN
    (
	CASE ISNULL(CoIns.party_cnt,-1)
		WHEN -1 THEN
			(SELECT SUM(commission_tax_applied) FROM
			event_insurance_COB_section
			WHERE insurance_file_cnt=EIF.insurance_file_cnt
			AND PaidD.documenttype_id NOT IN (33,34))
		ELSE
			(SELECT SUM(commission_inc_tax)-SUM(commission_exc_tax) FROM
			event_policy_coinsurers_section
			WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
			AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
			AND PaidD.documenttype_id NOT IN (33,34))
		END
    )
    ELSE EPA.tax_amount
    END,
    CASE @PartyTypeCode
    WHEN 'IN' THEN
	CASE ISNULL(CoIns.party_cnt,-1)
		WHEN -1 THEN
			(SELECT EIF.commission_amount WHERE PaidD.documenttype_id NOT IN (33,34))
		ELSE
			(SELECT SUM(commission_inc_tax) FROM
			event_policy_coinsurers_section
			WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
			AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
			AND PaidD.documenttype_id NOT IN (33,34))
		END
    ELSE (ISNULL(EPA.agent_commission_value,0)+ISNULL(EPA.tax_amount,0))
    END,
    D.document_ref,
    P.tax_number,
    P.shortname,
    S.vat_no,
    (
    SELECT PI.agency_number FROM party_insurer PI
    WHERE @PartyTypeCode='IN' AND PI.party_cnt=P.party_cnt
    ),
    CASE @PartyTypeCode
    WHEN 'IN' THEN P.shortname
    ELSE P2.shortname
    END,
    0,
    0.0
FROM CashList C
JOIN Currency CU2
    ON C.currency_id = CU2.currency_id
JOIN Source S
    ON S.source_id = C.company_id    
JOIN CashListItem I
    ON C.cashlist_id = I.cashlist_id 
JOIN TransDetail TD
    ON I.transdetail_id = TD.transdetail_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Account A
    ON TD.account_id = A.account_id
JOIN Ledger L
    ON L.Ledger_Id = A.Ledger_Id
JOIN Party P
    ON A.account_key = P.party_cnt
JOIN AllocationDetail AL
    ON AL.cashlistitem_id = I.cashlistitem_id
JOIN TransDetail PaidTD
    ON AL.transdetail_id = PaidTD.transdetail_id
JOIN Currency CU
    ON CU.currency_id = PaidTD.currency_id
JOIN Document PaidD
    ON PaidTD.document_id = PaidD.document_id
JOIN Transmatch PaidTM
    ON PaidTM.transdetail_id = PaidTD.transdetail_id
JOIN DocumentType DT
    ON DT.documenttype_id = PaidD.documenttype_id
LEFT JOIN Party_Address_Usage PAU
        JOIN Address_Usage_Type AUT
        ON AUT.address_usage_type_id = PAU.address_usage_type_id 
        JOIN Address AD 
        ON AD.address_cnt = PAU.address_cnt
    ON PAU.party_cnt = P.party_cnt
    AND AUT.code = '3131 XCO'
LEFT JOIN Transaction_Export_Folder TransExp 
    ON TransExp.Document_Ref = PaidD.Document_Ref 
    AND TransExp.source_id = PaidD.company_id 
LEFT JOIN Insurance_File InsFile 
    ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt 
LEFT JOIN Party P2
    ON InsFile.lead_insurer_cnt=P2.party_cnt
LEFT JOIN Risk_Code RC
    ON RC.risk_code_id = InsFile.risk_code_id
LEFT JOIN event_log EL
	ON EL.event_cnt = TransExp.event_log_id
LEFT JOIN event_insurance_file EIF
	ON EIF.insurance_folder_cnt = EL.event_cnt
LEFT JOIN event_policy_agents EPA
	ON EPA.insurance_file_cnt=EIF.insurance_file_cnt AND EPA.agent_cnt=p2.party_cnt
LEFT JOIN event_policy_coinsurers CoIns
	ON CoIns.insurance_file_cnt=EIF.insurance_file_cnt AND CoIns.party_cnt=P.party_cnt
WHERE c.cashlist_id = @cashlist_id
AND PaidTM.allocationdetail_id IS NOT NULL
ORDER BY    
    Sort_Column
OPTION (MAXDOP 1)

IF @PartyTypeCode = 'IN' 
    AND EXISTS
        (
            SELECT
                NULL
            FROM system_options
            WHERE option_number = 5006
            AND value = '1'
        )
    AND ISNULL(@NZConfig,0)=0
BEGIN

    INSERT INTO #Report_Temp
    SELECT
        rt.Document_Date,
        rt.Media_Ref,
        0,
        rt.CashList_Currency,
        rt.Insurer,
        rt.Address1,
        rt.Address2,
        rt.Address3,
        rt.Address4,
        rt.Postal_Code,
        rt.Document_Ref,
        rt.Document_ID,
        rt.Policy_Doc_Date,
        rt.Policy_Eff_Date,
        rt.Document_Type,
        rt.Client,
        rt.Client_Code,
        rt.Ledger_ID,
        rt.Policy_Ref,
        rt.CoInsurer_Policy_Ref,    
        0,
        0,
        0,
        (
            SELECT
                ISNULL(SUM(ISNULL(currency_amount, 0)), 0)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        ),
        0,
        0,
        0,
        rt.Sort_Column,
        p.shortname,
        p.resolved_name,
        1,
        rt.currency_id,
        rt.currency_code,
        rt.currency_desc,
        rt.Branch,
        rt.BranchAddress1,
        rt.BranchAddress2,
        rt.BranchAddress3,
        rt.BranchAddress4,
        rt.BranchPostalCode,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        '',
        '',
        '',
        '',
        '',
        '',
        0,
        0.0
    FROM #Report_Temp rt
    JOIN transdetail td 
        ON td.document_id = rt.document_id
    JOIN account a 
        ON a.account_id = td.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id
    JOIN party p 
        ON p.party_cnt = a.account_key
    WHERE l.ledger_short_name in ('AG', 'TR')
    AND rt.agent_sort = 0
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
    

    INSERT INTO #Report_Temp
    SELECT
        rt.Document_Date,
        rt.Media_Ref,
        0,
        rt.CashList_Currency,
        rt.Insurer,
        rt.Address1,
        rt.Address2,
        rt.Address3,
        rt.Address4,
        rt.Postal_Code,
        rt.Document_Ref,
        rt.Document_ID,
        rt.Policy_Doc_Date,
        rt.Policy_Eff_Date,
        rt.Document_Type,
        rt.Client,
        rt.Client_Code,
        rt.Ledger_ID,
        rt.Policy_Ref,
        rt.CoInsurer_Policy_Ref,    
        0,
        0,
        0,
        /*Sub Agent Amount (DD) : 0 if no sub agent or isn't a direct debit transaction*/
        (
            SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
            FROM Transdetail TD1
            JOIN Account A2
                ON A2.account_id = TD1.account_id
            WHERE TD1.document_id = td.Document_id
            AND A2.ledger_id in (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
            AND ISNULL(TD1.spare, '') <> 'AGENT ADJ'
            AND TD1.document_sequence = 
                (
                    SELECT MIN(TD2.document_sequence)
                    FROM Transdetail TD2
                    WHERE TD2.document_id = TD1.document_id
                    AND  TD2.account_id = TD1.account_id
                )
            AND 0 <>
                (
                    SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                    FROM Transdetail TD1
                    JOIN Account A2
                        ON A2.account_id = TD1.account_id
                    WHERE TD1.document_id = td.Document_id
                    AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                ) 
        )
        /*Sub Agent Amount (Not DD) : 0 if no sub agent or is a direct debit transaction*/
        -
        (
            (
                SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)       
                FROM Transdetail TD1
                JOIN Account A1
                    ON A1.account_id = TD1.account_id
                WHERE TD1.document_id = td.Document_id
                AND a1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                AND td1.document_sequence = 
                    (   
                        SELECT MIN(document_sequence)
                        FROM Transdetail        
                        WHERE document_id = td1.document_id 
                        AND a1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
                AND EXISTS
                    (
                        SELECT NULL
                        FROM Transdetail TD2
                        JOIN Account A2
                            ON A2.account_id = TD2.account_id
                        WHERE TD2.document_id = td.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                        AND ISNULL(TD2.spare, '') <> 'AGENT ADJ'
                    )
                AND 0 = 
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = td.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
            -   
            (   
                SELECT ISNULL(SUM(ROUND(amount,2)),0)
                FROM Transdetail TD1
                JOIN Account A1
                    ON A1.account_id = TD1.account_id
                WHERE TD1.document_id = td.Document_id
                AND A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                AND ISNULL(TD1.spare, '') <> 'AGENT ADJ'        
                AND 0 =
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = td.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
        ),            
        0,
        0,
        0,
        rt.Sort_Column,
        p.shortname,
        p.resolved_name,
        1,
        rt.currency_id,
        rt.currency_code,
        rt.currency_desc,
        rt.Branch,
        rt.BranchAddress1,
        rt.BranchAddress2,
        rt.BranchAddress3,
        rt.BranchAddress4,
        rt.BranchPostalCode,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        '',
        '',
        '',
        '',
        '',
        '',
        0,
        0.0
    FROM #Report_Temp rt
    JOIN transdetail td 
        ON td.document_id = rt.document_id 
    JOIN account a 
        ON a.account_id = td.account_id 
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id 
    JOIN party p 
        ON p.party_cnt = a.account_key
    WHERE l.ledger_short_name = 'UB'
    AND rt.agent_sort = 0
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )

END

IF @PartyTypeCode='IN'
	UPDATE #Report_Temp SET Balance_OS=(ISNULL(client_total_amount,0)-ISNULL(broker_total_amount,0))-ABS(ISNULL(total_payment,0))
ELSE
	UPDATE #Report_Temp SET Balance_OS=ISNULL(premium,0)-ABS(ISNULL(total_payment,0))
	
UPDATE #Report_Temp
SET
Company_Premium=0.0,
Company_Earthquake=0.0,
Earthquake_Levy=0.0,
Fire_Levy=0.0,
Client_Tax=0.0,
Client_Total_Amount=0.0,
Broker_Fee=0.0,
Broker_Tax=0.0,
Broker_Total_Amount=0.0,
part_paid=1
WHERE
ROUND(this_payment,2)<>ROUND(total_payment,2)

SET NOCOUNT OFF

SELECT  
    rt.Document_Date,
    rt.Media_Ref,
    rt.CashList_Amount,
    rt.CashList_Currency,
    rt.Insurer,
    rt.Address1,
    rt.Address2,
    rt.Address3,
    rt.Address4,
    rt.Postal_Code,
    rt.Document_Ref,
    rt.Document_ID,
    rt.Policy_Doc_Date,
    rt.Policy_Eff_Date,
    rt.Document_Type,
    rt.Client,
    rt.Client_Code,
    rt.Ledger_ID,
    rt.Policy_Ref,
    rt.CoInsurer_Policy_Ref,    
    rt.Premium,
    rt.Commission,
    rt.Fee,
    rt.comm,
    rt.IPT,
    ROUND(rt.This_Payment,2) 'This_Payment',
    rt.Total_Payment,
    rt.Agent_Code,
    rt.Agent_Name,
    rt.Agent_Sort,
    rt.currency_id,
    rt.currency_code,
    rt.currency_desc,
    rt.Branch,
    rt.BranchAddress1,
    rt.BranchAddress2,
    rt.BranchAddress3,
    rt.BranchAddress4,
    rt.BranchPostalCode,
    rt.Sort_Column,
    rt.show_third_party,
    ISNULL(rt.company_premium,0) * SIGN(ISNULL(rt.premium,0)) as company_premium,
    ISNULL(rt.company_earthquake,0) * SIGN(ISNULL(rt.premium,0)) as company_earthquake,
    ISNULL(rt.earthquake_levy,0) * SIGN(ISNULL(rt.premium,0)) as earthquake_levy,
    ISNULL(rt.fire_levy,0) * SIGN(ISNULL(rt.premium,0)) as fire_levy,
    ISNULL(rt.client_tax,0) * SIGN(ISNULL(rt.premium,0)) as client_gst,
    ISNULL(rt.client_total_amount,0)  * SIGN(ISNULL(rt.premium,0)) as client_total,
    ISNULL(rt.broker_fee,0) * SIGN(ISNULL(rt.premium,0)) as broker_fee,
    ISNULL(rt.broker_tax,0) * SIGN(ISNULL(rt.premium,0)) as broker_gst,
    ISNULL(rt.broker_total_amount,0) * SIGN(ISNULL(rt.premium,0)) as broker_total,
    (ISNULL(rt.client_total_amount,0)-ISNULL(rt.broker_total_amount,0)) * SIGN(ISNULL(rt.premium,0)) as net_premium,
    rt.balance_os * SIGN(ISNULL(rt.premium,0)),
    (ISNULL(rt.client_total_amount,0)-ISNULL(rt.broker_fee,0)) * SIGN(ISNULL(rt.premium,0)) as net_to_policy,
    ISNULL(rt.this_payment,0) as payment,
    rt.part_paid,
    rt.payment_ref,
    rt.insurer_tax_number,
    rt.insurer_code,
    rt.branch_tax_number,
    rt.insurer_agency,
    rt.underwriter
FROM #Report_Temp rt
ORDER BY 
    rt.Sort_Column,
    rt.Document_Id, 
    rt.Agent_Sort

DROP TABLE #Report_Temp


GO
