SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Marked_Items'
GO


CREATE PROCEDURE spu_Report_Marked_Items
    @company_id INT,
    @insurer_code VARCHAR(20),
    @PaymentGroups INT = 0
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
WHERE A.short_code = @insurer_code

IF @PartyTypeCode = 'AG'
BEGIN /*Agent*/
    SELECT @ReportIndicator = 'A' + CAST(PA.report_indicator AS CHAR(1))
    FROM Party_Agent PA
    JOIN Party P
        ON P.party_cnt = PA.party_cnt
    JOIN Account A
        ON A.account_key = P.party_cnt
    WHERE A.short_code = @insurer_code
END
ELSE
BEGIN /*Insurer*/
    SELECT @ReportIndicator = 'I' + CAST(PI.report_indicator AS CHAR(1))
    FROM Party_Insurer PI
    JOIN Party P
        ON P.party_cnt = PI.party_cnt
    JOIN Account A
        ON A.account_key = P.party_cnt
    WHERE A.short_code = @insurer_code
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
    Insurer VARCHAR(255),
    Address1 VARCHAR(60),
    Address2 VARCHAR(60),
    Address3 VARCHAR(60),
    Address4 VARCHAR(60),
    Postal_Code VARCHAR(20),
    Document_Ref VARCHAR(20),
    Document_ID INT,
    Policy_Doc_Date DATETIME ,
    Policy_Eff_Date DATETIME,
    Document_Type VARCHAR(255) ,
    Client VARCHAR(255),
    Client_Code CHAR(20) ,
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
    Sort_Column VARCHAR(255) ,
    Agent_Code VARCHAR(30),
    Agent_Name VARCHAR(255) ,
    Agent_Sort INT ,
    Currency_Iso_Code VARCHAR(4),
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
    Insurer_Tax_Number VARCHAR(50),
    Insurer_Code CHAR(20),
    Branch_Tax_Number VARCHAR(20),
    Branch VARCHAR(255),
    BranchAddress1 VARCHAR(60),
    BranchAddress2 VARCHAR(60),
    BranchAddress3 VARCHAR(60),
    BranchAddress4 VARCHAR(60),
    BranchPostalCode VARCHAR(20),
    Insurer_Agency VARCHAR(255),
    Underwriter CHAR(20),
    Part_Paid INT,
    Balance_OS MONEY
)

INSERT INTO #Report_Temp
SELECT DISTINCT
    ISNULL(P.name, '')      Insurer,
    ISNULL(AD.address1, '')     Address1,
    ISNULL(AD.address2, '')     Address2,
    ISNULL(AD.address3, '')     Address3,
    ISNULL(AD.address4, '')     Address4,
    ISNULL(AD.postal_code, '')  Postal_Code,
    DCli.document_ref       Document_Ref,
    DCli.document_id        Document_ID,
    DCli.document_date      Policy_Doc_Date,
    (
        SELECT
            td.ref_date
        FROM transdetail
        WHERE document_id = DCli.document_id
        AND document_sequence = 1
    ) Policy_Eff_Date,
    DTCli.description       Document_Type,
    ISNULL(PCli.resolved_name, ISNULL(ACli.account_name, '')) Client,
    ISNULL(ACli.short_code, '') Client_Code,
        (CASE l.ledger_short_name
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
         ELSE 0 END) ledger_id,
    ISNULL(TCli.insurance_ref, '')  Policy_Ref,
    ISNULL(CoIns.coinsurer_policy_number,'') Coinsurer_Policy_ref,
    ISNULL
    (
        (
        SELECT  SUM(round(currency_amount,2))
        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND account_id = A.account_id
        AND (
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )

                    AND
                    (
                        (spare = 'GROSS'
                        OR
                        spare = '')
                        OR
                        DCli.documenttype_id IN (33, 34)
                    )
                )
            OR
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'RF' )

                    AND
                    (
                        (spare = 'Premium Finance Debi')
                        AND
                        DCli.documenttype_id = 1
                    )
                )
            OR
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name IN ( 'AG', 'TR' ) )

                    AND
                    (spare = 'AGENT'
                    OR
                    (spare = ''
                    AND
                    DCli.documenttype_id = 1))
                )
            )
        )
    , 0)                Premium,

    ISNULL
    (
        (
        SELECT  SUM(round(currency_amount,2))
        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND account_id = A.account_id
        AND (
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )

                    AND
                    spare like 'COMM%'
                )
            OR
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name IN ( 'AG', 'TR') )

                    AND
                    spare = 'AGENT ADJ'
                )

            )
        )
    , 0)                Commission,
    (
        SELECT ISNULL(SUM(ROUND(tdf.currency_amount,2)),0)
        FROM transdetail tdf
        JOIN transdetail_type ttf
            ON ttf.transdetail_type_id = tdf.transdetail_type_id
        WHERE tdf.document_id = DCli.document_id
        AND tdf.account_id = A.account_id
        AND ttf.code = 'IFEE'
    ) Fee,
    0,
    ISNULL
    (
        (
        SELECT SUM(
            CASE amount
                WHEN 0 THEN 0.00
            ELSE
                ROUND((ref_amount * amount/abs(amount)),2)
            END )

        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND account_id = A.account_id
        AND (
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )

                    AND
                    (
                        (
                        spare = 'GROSS'
                        OR
                        spare = ''
                        )
                        OR
                        DCli.documenttype_id IN (33, 34)
                    )
                )
                OR
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name IN ( 'AG', 'TR' ) )

                    AND
                    (spare = 'AGENT'
                    OR
                    (spare = ''
                    AND
                    DCli.documenttype_id = 1))
                )

            )
        )
    , 0)                IPT,
    ISNULL
    (
        (
        SELECT  SUM(ROUND(currency_match_amount,2))
        FROM    TransMatch  A1,
            Transdetail T1
        WHERE   T1.transdetail_id = A1.transdetail_id
        AND T1.document_id = DCli.document_id
        AND T1.account_id = A.account_id
        AND A1.allocationdetail_id IS NULL
        )
    , 0)                This_Payment,
    ISNULL
    (
        (
        SELECT  SUM(ROUND(currency_match_amount,2))
        FROM    TransMatch  A1,
            Transdetail T1
        WHERE   T1.transdetail_id = A1.transdetail_id
        AND T1.document_id = DCli.document_id
        AND T1.account_id = A.account_id
        AND A1.allocationdetail_id IS NOT NULL
        AND T1.spare <> 'COMM'
        )
    , 0)                Total_Payment,
    (
        CASE
        WHEN (@ReportIndicator = 'I0' OR @ReportIndicator = 'A0')
            THEN ACli.short_code /*Should be payment date but items not yet paid.*/
        WHEN (@ReportIndicator = 'I1' OR @ReportIndicator = 'A1')
            THEN TCli.insurance_ref
        WHEN (@ReportIndicator = 'I2' OR @ReportIndicator = 'A2')
            THEN ACli.short_code
        WHEN (@ReportIndicator = 'I3')
            THEN CONVERT(VARCHAR(20),TransExp.cover_start_date,120)
        WHEN (@ReportIndicator = 'A3')
            THEN CONVERT(VARCHAR(20),InsFile.renewal_date,120)
        WHEN (@ReportIndicator = 'A4')
            THEN RC.description
        ELSE ACli.short_code
        END
    )               Sort_Column,
    ''              Agent_Code,
    ''              Agent_Name,
    0               Agent_Sort,
    CU.iso_code     Currency_Iso_Code,
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
            AND DCli.documenttype_id NOT IN (33,34))
        ELSE
            (SELECT SUM(commission_exc_tax) FROM
            event_policy_coinsurers_section
            WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
            AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
            AND DCli.documenttype_id NOT IN (33,34))
        END
    ELSE EPA.agent_commission_value
        END,
    CASE @PartyTypeCode
    WHEN 'IN' THEN
	CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN
            (SELECT SUM(commission_tax_applied) FROM
            event_insurance_COB_section
            WHERE insurance_file_cnt=EIF.insurance_file_cnt
            AND DCli.documenttype_id NOT IN (33,34))
        ELSE
            (SELECT SUM(commission_inc_tax)-SUM(commission_exc_tax) FROM
            event_policy_coinsurers_section
            WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
            AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
            AND DCli.documenttype_id NOT IN (33,34))
        END
    ELSE EPA.tax_amount
        END,
    CASE @PartyTypeCode
    WHEN 'IN' THEN
	CASE ISNULL(CoIns.party_cnt,-1)
	WHEN -1 THEN
            (SELECT EIF.commission_amount WHERE DCli.documenttype_id NOT IN (33,34))
        ELSE
            (SELECT SUM(commission_inc_tax) FROM
            event_policy_coinsurers_section
            WHERE event_policy_coinsurers_section.insurance_file_cnt=EIF.insurance_file_cnt
            AND event_policy_coinsurers_section.party_cnt=CoIns.party_cnt
            AND DCli.documenttype_id NOT IN (33,34))
        END
    ELSE (ISNULL(EPA.agent_commission_value,0)+ISNULL(EPA.tax_amount,0))
        END,
    P.tax_number,
    P.shortname,
    S.vat_no,
    ISNULL(S.description, ''),
    ISNULL(S.address1, ''),
    ISNULL(S.address2, ''),
    ISNULL(S.address3, ''),
    ISNULL(S.address4, ''),
    ISNULL(S.postal_code, ''),
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
FROM    Account     A
JOIN    Source S
ON A.company_id = S.source_id
JOIN    Party               P
ON A.account_key = P.Party_cnt
JOIN TransDetail    TD
ON TD.account_id = A.account_id
JOIN TransMatch TM
ON TM.transdetail_id = TD.transdetail_id
JOIN Document       DCli
ON DCli.document_id = TD.document_id
JOIN  DocumentType  DTCli
ON DTCli.documenttype_id = DCli.documenttype_id
JOIN TransDetail    TCli
ON TCli.account_id = A.Account_id
AND TCli.document_id = TD.document_id
JOIN Currency CU
ON TCli.currency_id=CU.currency_id
JOIN Account        ACli
ON  ACli.account_id =
(
    SELECT  T2.account_id
    FROM    TransDetail T2
    WHERE   transdetail_id =
    (   SELECT  MIN(transdetail_id)
        FROM    Transdetail T3,
            Account     A3
        WHERE   T3.document_id = DCli.document_id
        AND A3.account_id = T3.account_id
        AND
        (
            (
            DTCli.description <> 'Journal'

            AND
            A3.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' ) --eck4707

            )
            OR
            (
            DTCli.description = 'Journal'
            AND
            TCli.account_id <> A3.account_id
            )
            OR
            (
            DTCli.description = 'Fee'
            AND
            A3.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE' )
            )
        )
    )
)
LEFT JOIN Party PCli
    ON PCli.Party_cnt = ACli.account_key
JOIN Ledger L
    ON L.Ledger_Id = A.Ledger_Id
LEFT OUTER JOIN Transaction_Export_Folder           TransExp
    ON DCli.Document_Ref = TransExp.Document_Ref
    AND TransExp.source_id = DCli.company_id
LEFT OUTER JOIN     Insurance_File              InsFile
    ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt
LEFT JOIN Party P2
    ON InsFile.lead_insurer_cnt=P2.party_cnt
LEFT OUTER JOIN Policy_Coinsurers               CoIns
    ON  InsFile.insurance_file_cnt = CoIns.Insurance_file_cnt
    AND CoIns.Party_cnt = P.Party_cnt
LEFT OUTER JOIN Party_Address_Usage     PAU
    JOIN Address_Usage_Type         AUT
        ON AUT.address_usage_type_id = PAU.address_usage_type_id
    JOIN Address                AD
        ON AD.address_cnt = PAU.address_cnt
    ON PAU.party_cnt = P.party_cnt
    AND AUT.code = '3131 XCO'
LEFT OUTER JOIN Risk_Code RC
    ON RC.risk_code_id = InsFile.risk_code_id
LEFT OUTER JOIN event_log EL
    ON EL.event_cnt = TransExp.event_log_id
LEFT OUTER JOIN event_insurance_file EIF
    ON EIF.insurance_folder_cnt = EL.event_cnt
LEFT JOIN event_policy_agents EPA
    ON EPA.insurance_file_cnt=EIF.insurance_file_cnt And EPA.agent_cnt=p2.party_cnt
JOIN    transdetail tda
ON  tda.document_id = DCli.document_id

WHERE   A.short_code = @insurer_code AND TransExp.accounts_export_status = 'c' 
AND     TM.allocationdetail_id IS NULL
AND
(
    (
        (
            (@company_id > 0 AND DCli.company_id = @company_id)
            OR
            (@company_id = 0)
        )
        AND @PaymentGroups=0
    )
    OR
    (
        @PaymentGroups=1
        AND
        DCli.company_Id in (select company_id from insurerpayment where paymentgroup_id=@company_id and account_id=a.account_id)
    )
)

ORDER BY
    Sort_Column

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
        '',
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
            SELECT ISNULL(SUM(ROUND(TD1.currency_amount,2)),0)
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
                    SELECT ISNULL(SUM(ROUND(TD1.currency_amount,2)),0)
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
                SELECT ISNULL(SUM(ROUND(TD1.currency_amount,2)),0)
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
                        SELECT ISNULL(SUM(ROUND(TD1.currency_amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = td.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
            -
            (
                SELECT ISNULL(SUM(ROUND(currency_amount,2)),0)
                FROM Transdetail TD1
                JOIN Account A1
                    ON A1.account_id = TD1.account_id
                WHERE TD1.document_id = td.Document_id
                AND A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                AND ISNULL(TD1.spare, '') <> 'AGENT ADJ'
                AND 0 =
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.currency_amount,2)),0)
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
        '',
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
    UPDATE #Report_Temp SET Balance_OS=(ISNULL(client_total_amount,0)-ISNULL(broker_total_amount,0))+ISNULL(total_payment,0)
ELSE
    UPDATE #Report_Temp SET Balance_OS=ISNULL(premium,0)+ISNULL(total_payment,0)

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
(ROUND(this_payment,2)<>ROUND(total_payment,2))
AND ISNULL(total_payment,0)<>0

SET NOCOUNT OFF

SELECT
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
    rt.commission,
    rt.fee,
    rt.comm,
    rt.IPT,
    ROUND(rt.This_Payment,2) 'This_Payment',
    rt.Total_Payment,
    rt.Agent_Code,
    rt.Agent_Name,
    rt.Agent_Sort,
    rt.Currency_Iso_Code,
    rt.Sort_Column,
    rt.show_third_party,
    ISNULL(rt.company_premium,0) * -SIGN(ISNULL(rt.premium,0)) as company_premium,
    ISNULL(rt.company_earthquake,0) * -SIGN(ISNULL(rt.premium,0)) as company_earthquake,
    ISNULL(rt.earthquake_levy,0) * -SIGN(ISNULL(rt.premium,0)) as earthquake_levy,
    ISNULL(rt.fire_levy,0) * -SIGN(ISNULL(rt.premium,0)) as fire_levy,
    ISNULL(rt.client_tax,0) * -SIGN(ISNULL(rt.premium,0)) as client_gst,
    ISNULL(rt.client_total_amount,0)  * -SIGN(ISNULL(rt.premium,0)) as client_total,
    ISNULL(rt.broker_fee,0) * -SIGN(ISNULL(rt.premium,0)) as broker_fee,
    ISNULL(rt.broker_tax,0) * -SIGN(ISNULL(rt.premium,0)) as broker_gst,
    ISNULL(rt.broker_total_amount,0) * -SIGN(ISNULL(rt.premium,0)) as broker_total,
    (ISNULL(rt.client_total_amount,0)-ISNULL(rt.broker_total_amount,0)) * -SIGN(ISNULL(rt.premium,0)) as net_premium,
    rt.balance_os * -SIGN(ISNULL(rt.premium,0)) 'balance_os',
    (ISNULL(rt.client_total_amount,0)-ISNULL(rt.broker_fee,0)) * -SIGN(ISNULL(rt.premium,0)) as net_to_policy,
    CASE
    WHEN ISNULL(rt.this_payment,0)=-ISNULL(rt.client_total_amount,0) THEN (ISNULL(rt.this_payment,0)+ISNULL(rt.broker_total_amount,0))
    ELSE ISNULL(rt.this_payment,0)
    END as payment,
    rt.part_paid,
    rt.insurer_tax_number,
    rt.insurer_code,
    rt.branch_tax_number,
    rt.Branch,
    rt.BranchAddress1,
    rt.BranchAddress2,
    rt.BranchAddress3,
    rt.BranchAddress4,
    rt.BranchPostalCode,
    rt.insurer_agency,
    rt.underwriter
FROM #Report_Temp rt
ORDER BY
    rt.Sort_Column,
    rt.document_id,
    rt.agent_sort

DROP TABLE #Report_Temp

GO
