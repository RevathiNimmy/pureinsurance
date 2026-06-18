/*This Script is used by the following reports:*/
/*Income_Transacted*/
/*Export_Income_Transacted*/
/*Export_Income_Transacted_Extended*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Income_Transacted_NZ'
GO

CREATE PROCEDURE spu_Report_Income_Transacted_NZ
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @report_type VARCHAR(20),
    @adjustments BIT,
    @date_type VARCHAR(25)

AS

DECLARE
    @document_ref VARCHAR(25),
    @document_id INT,
    @account_id INT,
    @account_code VARCHAR(30),
    @account_name VARCHAR(60),
    @company_id INT,
    @effective_date DATETIME,
    @amount MONEY,
    @agencyorunderwriting VARCHAR(1),
    @account_executive_on_policy INT,
    @share_client_code VARCHAR(50),
    @share_client_name VARCHAR(255),
    @share_gross_premium MONEY,
    @share_ipt_amount MONEY,
    @share_gross_commission MONEY,
    @share_agent_commission MONEY,
    @share_fee_amount MONEY,
    @share_discount_amount MONEY,
    @share_extra_premium MONEY,
    @share_extra_commission MONEY,
    @share_this_premium MONEY,
    @share_commission_amount MONEY,
    @share_annual_premium MONEY,
    @share_net_premium MONEY,
    @gross_premium MONEY,
    @ipt_amount MONEY,
    @gross_commission MONEY,
    @agent_commission MONEY,
    @fee_amount MONEY,
    @discount_amount MONEY,
    @extra_premium MONEY,
    @extra_commission MONEY,
    @this_premium MONEY,
    @commission_amount MONEY,
    @annual_premium MONEY,
    @net_premium MONEY,
    @fsa_in_use INT,
    @full_client_amount MONEY,
    @shared_rate FLOAT,
    @shared_amount MONEY,
    @NZConfig INT,
    @client_fees MONEY,
    @discount_code  VARCHAR(50),
    @discount_name VARCHAR(255),
    @third_party_code VARCHAR(50),
    @third_party_name VARCHAR(255)

SET NOCOUNT ON

SELECT @NZConfig = ISNULL(value, 0) FROM hidden_options WHERE option_number = 86

SELECT @agencyorunderwriting = value
FROM hidden_options
WHERE option_number = 1

SELECT @fsa_in_use = value
FROM hidden_options
WHERE option_number = 61

/*Pick up account executive from policy not client when the option is set*/
SELECT @account_executive_on_policy = 0
SELECT
    @account_executive_on_policy = ISNULL(value ,0)
FROM hidden_options
WHERE option_number = 40

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF ISNULL(@date_type,'') ='' 
BEGIN
   SELECT @date_type='Transaction Date' 
END

/*Clear the table*/
CREATE TABLE #IncomeTrans
(
    document_id INT,
    document_ref VARCHAR(30),
    gross_premium MONEY,
    ipt_amount MONEY,
    gross_commission MONEY,
    agent_commission MONEY,
    document_date DATETIME,
    branch_id INT,
    branch_code VARCHAR(20),
    income_code VARCHAR(20),
    income_name VARCHAR(255),
    commission_account_id INT,
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    area_desc VARCHAR(255),
    client_business VARCHAR(255),
    account_exec_code VARCHAR(20),
    account_exec_name VARCHAR(255),
    document_type_desc VARCHAR(255),
    effective_date DATETIME,
    fee_amount MONEY,
    discount_amount MONEY,
    extra_premium MONEY,
    extra_commission MONEY,
    insurance_ref VARCHAR(30),
    transaction_type_desc VARCHAR(255),
    business_type_desc VARCHAR(255),
    risk_code VARCHAR(255),
    risk_code_desc VARCHAR(255),
    insurer_code VARCHAR(20),
    insurer_name VARCHAR(255),
    ipt_rate FLOAT,
    comm_opening_balance MONEY,
    account_handler_code VARCHAR(255),
    account_handler_name VARCHAR(255),
    adjustment BIT,
    cover_start_date DATETIME,
    expiry_date DATETIME,
    renewal_date DATETIME,
    this_premium MONEY,
    commission_amount MONEY,
    analysis_code_desc VARCHAR(255),
    policy_status VARCHAR(255),
    commission_percentage FLOAT,
    annual_premium MONEY,
    net_premium MONEY,
    no_of_fees INT,
    no_of_insurers INT,
    insurer_order INT,
    agent_comm_exists INT,
    agencyorunderwriting VARCHAR(1),
    no_income BIT,
    trans_user_name VARCHAR(255),
    base_currency VARCHAR(255),
    regarding VARCHAR(255),
    payment_method VARCHAR(60),
    insured_account VARCHAR(50),
    no_of_shares INT,
    shared_order INT,
    fsa_customer_category VARCHAR(50),
    fsa_Underwriter VARCHAR(255),
    fsa_in_use INT,
    comm_tax MONEY,
    agent_tax MONEY,
    subagent_tax MONEY,
    client_fees MONEY,
    discount_code  VARCHAR(50),
    discount_name VARCHAR(255),
    third_party_code VARCHAR(50),
    third_party_name VARCHAR(255),
    no_of_third_party INT
)
CREATE INDEX IncomeTrans__document_id ON #IncomeTrans(document_id)

  
/*Get all of the applicable documents and insert them into the working table*/
INSERT INTO #IncomeTrans
(
    document_id,
    document_date,
    adjustment
)
SELECT DISTINCT
    D.document_id,
    D.document_date,
    0
FROM Document D
JOIN Transdetail T
     ON D.document_id = T.document_id
WHERE D.company_id = ISNULL(@branch_id,D.company_id)
AND D.documenttype_id IN (4, 5, 15, 16, 17, 18, 30, 31, 32, 35, 36)
AND 
  (
   (@date_type = 'Transaction Date' 
    AND D.document_date BETWEEN @start_date AND @end_date)
    OR
   (@date_type = 'Effective Date'
    AND T.ref_date >= @start_date AND T.ref_date <= @end_date)
    OR
   (@date_type = 'Trans/Effective Date' 
    AND (
	(D.document_date BETWEEN @start_date AND @end_date) 
	OR 
	(T.ref_date >= @start_date AND T.ref_date <= @end_date)
        )
    )
   )
IF @adjustments = 1
BEGIN
    /*Get all of the applicable adjustments and insert them into the working table*/
    INSERT INTO #IncomeTrans
    (
        document_id,
        document_date,
        adjustment
    )
    SELECT DISTINCT
        D.document_id,
        T.ref_date,
        1
    FROM document D
    JOIN Transdetail T
        ON D.document_id = T.document_id
WHERE T.spare = 'BROK ADJ'
    AND T.ref_date BETWEEN @start_date AND @end_date
    AND D.company_id = ISNULL(@branch_id,D.company_id)
    AND D.documenttype_id IN (4, 5, 15, 16, 17, 18, 30, 31, 32, 35, 36)
END

/**/
IF @report_type = 'Commission'
BEGIN
    UPDATE IT
    SET IT.income_code =
            (
                SELECT
                    ISNULL(MIN(A.short_code),'N/A')
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id
                JOIN ledger L
                    ON L.ledger_id = A.ledger_id
                WHERE TD.document_id = D.document_id
                AND L.ledger_short_name = 'CO'
            ),
        IT.income_name =
            (
                SELECT
                    ISNULL(MIN(A.account_name),'No Commission Transacted')
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id
                JOIN ledger L
                    ON L.ledger_id = A.ledger_id
                WHERE TD.document_id = D.document_id
                AND L.ledger_short_name = 'CO'
            )
    FROM #IncomeTrans IT
    JOIN Document D
        ON D.document_id = IT.document_id
END
ELSE
BEGIN
    UPDATE IT
    SET IT.income_code =
            (
                SELECT
                    ISNULL(MIN(A.short_code),'N/A')
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id
                JOIN ledger L
                    ON L.ledger_id = A.ledger_id
                WHERE TD.document_id = D.document_id
                AND L.ledger_short_name = 'FE'
                AND TD.document_sequence =
                    (
                        SELECT
                            MIN(TD.document_sequence)
                        FROM transdetail TD
                        JOIN account A
                            ON A.account_id = TD.account_id
                        JOIN ledger L
                            ON L.ledger_id = A.ledger_id
                        WHERE TD.document_id = D.document_id
                        AND L.ledger_short_name = 'FE'
                    )
            ),
        IT.income_name =
            (
                SELECT
                    ISNULL(MIN(A.account_name),'No Fee Transacted')
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id
                JOIN ledger L
                    ON L.ledger_id = A.ledger_id
                WHERE TD.document_id = D.document_id
                AND L.ledger_short_name = 'FE'
                AND TD.document_sequence =
                    (
                        SELECT
                            MIN(TD.document_sequence)
                        FROM transdetail TD
                        JOIN account A
                            ON A.account_id = TD.account_id
                        JOIN ledger L
                            ON L.ledger_id = A.ledger_id
                        WHERE TD.document_id = D.document_id
                        AND L.ledger_short_name = 'FE'
                    )
            )
    FROM #IncomeTrans IT
    JOIN Document D
        ON D.document_id = IT.document_id
END

/*Update all of the non-amount fields for each document_id in the table*/
UPDATE IT
SET IT.document_ref = D.document_ref,
    IT.branch_id = D.company_id,
    IT.branch_code = S.code,
    IT.commission_account_id =
        (
            SELECT ISNULL(A.account_id,0)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
            WHERE TD.document_id = D.document_id
            AND L.ledger_short_name = 'CO'
            GROUP BY A.account_id
        ),
    IT.client_code =
        (
            SELECT ISNULL(P.shortname, '')
            FROM Transdetail T
            JOIN Account A
                ON A.account_id = T.account_id
            JOIN Party P
                ON P.party_cnt = A.account_key
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),
    IT.client_name =
        (
            SELECT ISNULL(P.resolved_name, '')
            FROM Transdetail T
            JOIN Account A
                ON A.account_id = T.account_id
            JOIN Party P
                ON P.party_cnt = A.account_key
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),
    IT.area_desc =
        (
            SELECT ISNULL(AR.description, '')
            FROM Transdetail T
            JOIN Account A
                ON A.account_id = T.account_id
            JOIN Party P
                ON P.party_cnt = A.account_key
            LEFT JOIN Area AR
                ON AR.area_id = P.area_id
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),
    IT.client_business =
        (
            SELECT ISNULL(PCC.party_business_id, '')
            FROM Transdetail T
            JOIN Account A
                ON A.account_id = T.account_id
            JOIN Party P
                ON P.party_cnt = A.account_key
            LEFT JOIN Party_Corporate_Client PCC
                ON PCC.party_cnt = P.party_cnt
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),
    IT.document_type_desc = DT.description,
    IT.effective_date =
        (
            SELECT ref_date
            FROM transdetail
            WHERE document_id = D.document_id
            AND document_sequence = 1
        ),
    IT.no_of_fees =
        (
            SELECT
                ISNULL(SUM(1),0)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN party P
                ON P.party_cnt = A.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
            WHERE PT.code = 'FE'
            AND TD.document_id = D.document_id
        ),
    IT.no_of_insurers =
        (
            SELECT
                ISNULL(SUM(1), 1)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN party P
                ON P.party_cnt = A.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
            WHERE PT.code = 'IN'
            AND TD.document_id = D.document_id
            AND TD.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
        ),

       IT.no_of_third_party =
        (
            SELECT                ISNULL(SUM(1), 1)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND (L.ledger_short_name = 'AG' OR L.ledger_short_name = 'TR' OR L.ledger_short_name = 'UB')
            AND TD.document_id = D.document_id
            AND TD.spare ='AGENT'
        ),
 

    IT.discount_code =
        (
               SELECT
               ISNULL(P.shortname, '')
                 FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN party P
                ON P.party_cnt = A.account_key
             JOIN ledger L
                 ON L.ledger_id = A.ledger_id
              WHERE TD.document_id = D.document_id
                AND L.ledger_short_name = 'DI'
        ),

     IT.discount_name=
        (
                SELECT
                   ISNULL(P.resolved_name, '')
                 FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN party P
                ON P.party_cnt = A.account_key
             JOIN ledger L
                 ON L.ledger_id = A.ledger_id
              WHERE TD.document_id = D.document_id
                AND L.ledger_short_name = 'DI'
        ),

    IT.insurer_order = 1,
    IT.agent_comm_exists = 0,
    IT.agencyorunderwriting = @agencyorunderwriting,
    IT.no_income =
        CASE IT.income_code
            WHEN 'N/A' THEN 1
            ELSE 0
        END,

    IT.trans_user_name = (
            SELECT
                ISNULL(PMU.username, '')
            FROM Transdetail T
            JOIN PMUSER PMU
                ON T.operator_id = PMU.user_id
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),
    IT.no_of_shares =
    (
        SELECT
            ISNULL(SUM(1), 1)
        FROM Transdetail T
        JOIN Account A
            ON A.account_id = T.account_id
        JOIN ledger l
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name = 'SA'
        WHERE T.document_id = D.document_id
        AND T.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = T.document_id
                AND account_id = T.account_id
            )
    ),
    IT.shared_order = 0,
    IT.fsa_in_use = @fsa_in_use,
    IT.base_currency = c.description

FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id
JOIN Source S
    ON S.source_id = D.company_id
JOIN currency C
    ON C.currency_id = S.base_currency_id


/*Update the amount fields for transactions*/
UPDATE IT
SET IT.gross_premium = /*Gross Premium is the amount given to the insurer(s).*/
        (
            SELECT ISNULL(SUM(TDI.amount),0) * -1
            FROM transdetail TDI
            JOIN account AI
                ON AI.account_id = TDI.account_id
            JOIN party P
                ON P.party_cnt = AI.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
                AND PT.code = 'IN'
            WHERE TDI.document_id = D.document_id
            AND TDI.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
        ),
    IT.ipt_amount =
        (
            SELECT
                CASE
                    WHEN SUM(TDI.amount) < 0 THEN ISNULL(SUM(TDI.ref_amount),0)
                    WHEN SUM(TDI.amount) > 0 THEN ISNULL(SUM(TDI.ref_amount),0) * -1
                    ELSE 0
                END
            FROM transdetail TDI
            JOIN account AI
                ON AI.account_id = TDI.account_id
            JOIN party P
                ON P.party_cnt = AI.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
                AND PT.code = 'IN'
            WHERE TDI.document_id = D.document_id
            AND TDI.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
        ),
    IT.gross_commission = /*Gross Commission is the amount given back by the insurer(s).*/
        (
            SELECT ISNULL(SUM(TDI.amount),0)
            FROM transdetail TDI
            JOIN account AI
                ON AI.account_id = TDI.account_id
            JOIN party P
                ON P.party_cnt = AI.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
                AND PT.code = 'IN'
            WHERE TDI.document_id = D.document_id
            AND TDI.spare IN ('COMM', 'Reversed COMM', 'Reversal COMM')
        ),
    IT.agent_commission =
        /*Agent Amount : 0 if no agent*/
        (
            SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
            FROM Transdetail    TD1
            JOIN Account A2
                ON A2.account_id = TD1.account_id
            WHERE TD1.document_id = D.Document_id
            AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name IN ('AG', 'TR') )
            AND ISNULL(TD1.spare, '') <> 'AGENT ADJ'
        )
        +
        /*Sub Agent Amount (DD) : 0 if no sub agent or isn't a direct debit transaction*/
        (
            SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
            FROM Transdetail TD1
            JOIN Account A2
                ON A2.account_id = TD1.account_id
            WHERE TD1.document_id = D.Document_id
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
                    WHERE TD1.document_id = D.Document_id
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
                WHERE TD1.document_id = D.Document_id
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
                        WHERE TD2.document_id = D.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                        AND ISNULL(TD2.spare, '') <> 'AGENT ADJ'
                    )
                AND 0 =
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = D.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
            -
            (
                SELECT ISNULL(SUM(ROUND(amount,2)),0)
                FROM Transdetail TD1
                JOIN Account A1
                    ON A1.account_id = TD1.account_id
                WHERE TD1.document_id = D.Document_id
                AND A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                AND ISNULL(TD1.spare, '') <> 'AGENT ADJ'
                AND 0 =
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = D.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
        ),
    IT.fee_amount =
        (
            SELECT ISNULL(SUM(TDF.amount), 0.0) * -1
            FROM Transdetail TDF
            JOIN account AF
                ON AF.account_id = TDF.account_id
            JOIN party P
                ON P.party_cnt = AF.account_key
            WHERE TDF.document_id = D.document_id
            AND P.party_type_id = 9 /*Fee*/
            AND
            (
                @report_type = 'Commission'
                OR
                (
                    @report_type = 'Fee'
                    AND
                    P.shortname = IT.income_code
                )
            )
        ),
    IT.discount_amount =
        (
            SELECT ISNULL(SUM(TDD.amount), 0.0)
            FROM Transdetail TDD
            JOIN account AD
                ON AD.account_id = TDD.account_id
            JOIN party P
                ON P.party_cnt = AD.account_key
            WHERE TDD.document_id = D.document_id
            AND P.party_type_id = 11 /*Discount*/
        ),
    IT.extra_premium = /*Extra Premium is the amount given to the extra(s).*/
        (
            SELECT ISNULL(SUM(TDE.amount), 0.0)  * -1
            FROM Transdetail TDE
            JOIN account AE
                ON AE.account_id = TDE.account_id
            JOIN party P
                ON P.party_cnt = AE.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
                AND PT.code = 'EX'
            WHERE TDE.document_id = D.document_id
            AND TDE.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
        ),
    IT.extra_commission = /*Extra Commission is the amount given back by the extra(s).*/
        (
            SELECT ISNULL(SUM(TDE.amount), 0.0)
            FROM Transdetail TDE
            JOIN account AE
                ON AE.account_id = TDE.account_id
            JOIN party P
                ON P.party_cnt = AE.account_key
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
                AND PT.code = 'EX'
            WHERE TDE.document_id = D.document_id
            AND TDE.spare IN ('COMM', 'Reversed COMM', 'Reversal COMM')
        ),
     IT.comm_tax =
	(
	    SELECT ROUND(SUM(EICS.commission_tax_applied),2)
	    FROM document d1
	    JOIN 
		transaction_export_folder tef 
		ON tef.source_id=D1.company_id 
	    AND
		tef.document_ref=D1.document_ref 
	    AND
		tef.accounts_export_status='c'
	    JOIN
		event_log el 
		ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt 
	    JOIN 
		event_insurance_folder eif 
		ON eif.insurance_folder_cnt = el.event_cnt
	    JOIN
		event_insurance_file eifi 
		ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
	    JOIN
		event_insurance_cob_section EICS 
		ON EICS.insurance_file_cnt = eifi.insurance_file_cnt
	    WHERE d1.document_id = d.document_id
	),
    IT.agent_tax =
	(
	    SELECT ISNULL(ROUND(SUM(epa1.tax_amount),2),0)
	    FROM document d1
	    JOIN transaction_export_folder tef 
		ON tef.source_id=d1.company_id 
		AND tef.document_ref=d1.document_ref
		AND tef.accounts_export_status='c'
	    JOIN event_log el 
		ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
	    JOIN event_insurance_folder eif 
		ON eif.insurance_folder_cnt = el.event_cnt
	    JOIN event_insurance_file eifi 
		ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
	    JOIN event_policy_agents epa1 
		ON epa1.insurance_file_cnt = eifi.insurance_file_cnt
	    JOIN party_agent pa 
		ON epa1.agent_cnt = pa.party_cnt
	    JOIN party_agent_type pat 
		ON pat.party_agent_type_id = pa.party_agent_type_id
	    WHERE d1.document_id = d.document_id AND pat.description = 'AGENT'
	),
    IT.subagent_tax = 
	(
	    SELECT isnull(ROUND(SUM(epa1.tax_amount),2),0)
	    FROM document d1
	    JOIN transaction_export_folder tef 
		ON tef.source_id=d1.company_id 
		AND tef.document_ref=d1.document_ref
		AND tef.accounts_export_status='c'
	    JOIN event_log el 
		ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
	    JOIN event_insurance_folder eif 
		ON eif.insurance_folder_cnt = el.event_cnt
	    JOIN event_insurance_file eifi 
		ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
	    JOIN event_policy_agents epa1 
		ON epa1.insurance_file_cnt = eifi.insurance_file_cnt
	    JOIN party_agent pa 
		ON epa1.agent_cnt = pa.party_cnt
	    JOIN party_agent_type pat 
		ON pat.party_agent_type_id = pa.party_agent_type_id
	    WHERE d1.document_id = d.document_id AND pat.description = 'SUB AGENT'
	),

   IT.client_fees = /*Cilent Fees*/
        (
	   /*     SELECT ISNULL(SUM(T.amount),0)
	        FROM Transdetail T
	        JOIN Account A
	            ON A.account_id = T.account_id
	        JOIN ledger L
	            ON L.ledger_id = A.ledger_id
	            AND L.ledger_short_name = 'SA'
	        WHERE T.document_id = D.document_id*/

	     SELECT ISNULL(SUM(T.amount),0)
	        FROM Transdetail T
	        JOIN Account A
	        ON A.account_id = T.account_id
                      JOIN Transdetail_type TT on 
	        TT.Transdetail_type_id=T.Transdetail_type_id
	       AND TT.code='CFEE'
                   WHERE T.document_id = D.document_id
        )

FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id

UPDATE #IncomeTrans SET comm_tax=-comm_tax, agent_tax=-agent_tax, subagent_tax=-subagent_tax
WHERE ISNULL(gross_premium,0) < 0

IF @adjustments = 1
BEGIN
    /*Update the amount fields for adjustments*/
    UPDATE IT
    SET IT.gross_premium = 0,
        IT.ipt_amount = 0,
        IT.fee_amount = 0,
        IT.discount_amount = 0,
        IT.extra_premium = 0,
        IT.extra_commission = 0,
        IT.gross_commission =
            (
                SELECT ISNULL(SUM(ROUND(T.amount,2)),0)
                FROM Transdetail T
                WHERE T.document_id = IT.document_id
                AND T.ref_date = IT.document_date
                AND T.spare = 'COMM ADJ'
            ), /*Calculate Insurer Adjustment*/
        IT.agent_commission =
            (
                SELECT ISNULL(SUM(ROUND(T.amount,2)),0)
                FROM Transdetail T
                WHERE T.document_id = IT.document_id
                AND T.ref_date = IT.document_date
                AND T.spare = 'AGENT ADJ'
            ), /*Calculate Agent Adjustment*/
        IT.no_of_insurers = 1,
        IT.no_of_fees = 0
    FROM #IncomeTrans IT
    WHERE IT.adjustment = 1
END 

/*Update values that require a transaction export record.*/
UPDATE IT
SET IT.insurance_ref = TEF.insurance_ref,
    IT.transaction_type_desc = TT.description,
    IT.business_type_desc = BT.description
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN Transaction_Export_Folder TEF
    ON TEF.document_ref = D.document_ref
    AND TEF.source_id = D.company_id
    AND TEF.accounts_export_status = 'c'
JOIN Posting_Type TT
    ON TT.Posting_type_id = TEF.transaction_type_id
JOIN Business_Type BT
    ON BT.business_type_id = TEF.business_type_id

/*Update values, for reversals, that require a transaction export record.*/
UPDATE IT
SET IT.insurance_ref = TEF.insurance_ref,
    IT.transaction_type_desc = TT.description,
    IT.business_type_desc = BT.description,
    IT.document_ref = RTRIM(IT.document_ref) + ' (R)'
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN Transaction_Export_Folder TEF
    ON TEF.document_ref = SUBSTRING(D.comment, 22, 11)
    AND TEF.source_id = D.company_id
    AND TEF.accounts_export_status = 'c'
JOIN Posting_Type TT
    ON TT.Posting_type_id = TEF.transaction_type_id
JOIN Business_Type BT
    ON BT.business_type_id = TEF.business_type_id

    
/*Update values that require a insurance file record.*/
UPDATE IT
SET IT.insurance_ref = ISNULL(IT.insurance_ref,I.insurance_ref),
    IT.risk_code = RC.code,
    IT.risk_code_desc = RC.description,
    IT.insurer_code = P.shortname,
    IT.insurer_name = P.resolved_name,
    IT.ipt_rate = IPT.rate,
    IT.account_handler_code = ISNULL(PA.shortname, ''),
    IT.account_handler_name = ISNULL(PA.resolved_name, ''),

    IT.cover_start_date = I.cover_start_date,
    IT.expiry_date = I.expiry_date,
    IT.renewal_date = I.renewal_date,
    IT.this_premium = ISNULL(I.this_premium, 0),
    IT.commission_amount =
        (
            ISNULL(I.commission_amount, 0)
            +
            (
                SELECT ISNULL(SUM(coinsurer_commission_amount),0)
                FROM policy_coinsurers
                WHERE insurance_file_cnt = I.insurance_file_cnt
            )
        ),
    IT.analysis_code_desc = ISNULL(AC.description, ''),
    IT.policy_status =
        (
            CASE IFT.insurance_file_type_id
                WHEN 1 THEN IFT.description
                ELSE ISNULL(IFS.Description, 'Live')
            END
        ),
    IT.commission_percentage = ISNULL(I.commission_percentage,0),
    IT.annual_premium = ISNULL(I.annual_premium,0),
    IT.net_premium = ISNULL(I.net_premium, 0),
    IT.fsa_customer_category =
        ( SELECT CASE I.fsa_customer_category_id WHEN 0 THEN 'Commercial' WHEN 1 THEN 'Retail' ELSE '' END ),
    IT.fsa_Underwriter = ( SELECT ISNULL(resolved_name, '') FROM party WHERE party_cnt = I.fsa_underwriter_cnt ),
    IT.payment_method = ISNULL(I.payment_method,''),
    IT.regarding = ISNULL(IFSy.last_trans_description, '')
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN Insurance_File I
    ON I.insurance_file_cnt = D.insurance_file_cnt
JOIN Insurance_File_System IFSy
    ON IFSy.insurance_file_cnt = I.insurance_file_cnt
JOIN Insurance_File_Type IFT
    ON IFT.insurance_file_type_id = I.insurance_file_type_id
LEFT JOIN Insurance_File_Status IFS
    ON IFS.insurance_file_status_id = I.insurance_file_status_id
JOIN Party P
    ON P.party_cnt = I.lead_insurer_cnt
JOIN Risk_Code RC
    ON RC.risk_code_id = I.risk_code_id
LEFT JOIN party PA
    ON PA.party_cnt = I.account_handler_cnt
LEFT JOIN IPT
    ON IPT.risk_code_id = RC.risk_code_id
    AND IPT.effective_date =
        (
            SELECT MAX(effective_date)
            FROM IPT
            WHERE effective_date <= D.document_date
            AND risk_code_id = RC.risk_code_id
        )
LEFT JOIN Analysis_Code AC
    ON AC.analysis_code_id = I.analysis_code_id


IF @account_executive_on_policy = 1
BEGIN
    UPDATE IT
    SET IT.account_exec_code = ISNULL(P.shortname, ''),
        IT.account_exec_name = ISNULL(P.resolved_name, '')
    FROM #IncomeTrans IT
    JOIN document D
        ON D.document_id = IT.document_id
    JOIN insurance_file I
        ON I.insurance_file_cnt = D.insurance_file_cnt
    JOIN party P
        ON P.party_cnt = I.account_executive_cnt
END
ELSE
BEGIN
    UPDATE IT
    SET IT.account_exec_code = ISNULL(PE.shortname, ''),
        IT.account_exec_name = ISNULL(PE.resolved_name, '')
    FROM #IncomeTrans IT
    JOIN document D
        ON D.document_id = IT.document_id
    JOIN transdetail TD
        ON TD.document_id = D.document_id
        AND TD.document_sequence = 1
    JOIN Account A
        ON A.account_id = TD.account_id
    JOIN Party PCli
        ON PCli.party_cnt = A.account_key
    JOIN Party PE
    ON PE.party_cnt = PCli.consultant_cnt
END


    UPDATE IT
    SET IT.third_party_code = ISNULL(P.shortname, ''),
        IT.third_party_name = ISNULL(P.resolved_name, '')
    FROM #IncomeTrans IT
    JOIN document D
        ON D.document_id = IT.document_id
    JOIN transdetail TD
        ON TD.document_id = D.document_id
        AND TD.document_sequence = 1
    JOIN Account A
        ON A.account_id = TD.account_id
    JOIN Party P
        ON P.party_cnt = A.account_key
   JOIN Ledger L
           ON L.ledger_id = A.ledger_id
   AND (L.ledger_short_name = 'AG' OR L.ledger_short_name = 'TR' OR L.ledger_short_name = 'UB')
        AND TD.spare = 'AGENT'

IF @report_type = 'Commission'
BEGIN

    DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT document_id
    FROM #IncomeTrans
    WHERE no_of_insurers > 1 AND no_of_shares = 1

    OPEN c_cursor

    FETCH NEXT FROM c_cursor INTO @document_id

    WHILE @@FETCH_STATUS = 0
    BEGIN

        INSERT INTO #IncomeTrans
        (
            document_id,
            document_ref,
            gross_premium,
            ipt_amount,
            gross_commission,
            agent_commission,
            document_date,
            branch_id,
            branch_code,
            income_code,
            income_name,
            commission_account_id,
            client_code,
            client_name,
            area_desc,
            client_business,
            account_exec_code,
            account_exec_name,
            document_type_desc,
            effective_date,
            fee_amount,
            discount_amount,
            extra_premium,
            extra_commission,
            insurance_ref,
            transaction_type_desc,
            business_type_desc,
            risk_code_desc,
            insurer_code,
            insurer_name,
            ipt_rate,
            comm_opening_balance,
            account_handler_code,
            account_handler_name,
            adjustment,
            cover_start_date,
            expiry_date,
            renewal_date,
            this_premium,
            commission_amount,
            analysis_code_desc,
            policy_status,
            commission_percentage,
            annual_premium,
            net_premium,
            no_of_insurers,
            insurer_order,
            agent_comm_exists,
            agencyorunderwriting,
            no_income,
            trans_user_name,
            base_currency,
            regarding,
            payment_method,
            insured_account,
            no_of_shares,
            shared_order,
            fsa_customer_category,
            fsa_Underwriter,
            fsa_in_use,
	    comm_tax
        )
        SELECT
            IT.document_id,
            IT.document_ref,
            (
                SELECT ISNULL(SUM(TDI.amount), 0.0) * -1
                FROM Transdetail TDI
                JOIN account AI
                    ON AI.account_id = TDI.account_id
                JOIN party P
                    ON P.party_cnt = AI.account_key
                JOIN party_type PT
                    ON PT.party_type_id = P.party_type_id
                    AND PT.code = 'IN'
                WHERE TDI.document_id = IT.document_id
                AND TDI.account_id = A.account_id
                AND TDI.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
            ),
            (
                SELECT
                    CASE
                        WHEN SUM(TDI.amount) < 0 THEN ISNULL(SUM(TDI.ref_amount),0)
                        WHEN SUM(TDI.amount) > 0 THEN ISNULL(SUM(TDI.ref_amount),0) * -1
                        ELSE 0
                    END
                FROM transdetail TDI
                JOIN account AI
                    ON AI.account_id = TDI.account_id
                JOIN party P
                    ON P.party_cnt = AI.account_key
                JOIN party_type PT
                    ON PT.party_type_id = P.party_type_id
                    AND PT.code = 'IN'
                WHERE TDI.document_id = IT.document_id
                AND TDI.account_id = A.account_id
                AND TDI.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
            ),
            (
                SELECT ISNULL(SUM(TDI.amount), 0.0)
                FROM Transdetail TDI
                JOIN account AI
                    ON AI.account_id = TDI.account_id
                JOIN party P
                    ON P.party_cnt = AI.account_key
                JOIN party_type PT
                    ON PT.party_type_id = P.party_type_id
                    AND PT.code = 'IN'
                WHERE TDI.document_id = IT.document_id
                AND TDI.account_id = A.account_id
                AND TDI.spare IN ('COMM', 'Reversed COMM', 'Reversal COMM')
            ),
            0,
            IT.document_date,
            IT.branch_id,
            IT.branch_code,
            IT.income_code,
            IT.income_name,
            IT.commission_account_id,
            IT.client_code,
            IT.client_name,
            IT.area_desc,
            IT.client_business,
            IT.account_exec_code,
            IT.account_exec_name,
            IT.document_type_desc,
            IT.effective_date,
            0,
            0,
            0,
            0,
            IT.insurance_ref,
            IT.transaction_type_desc,
            IT.business_type_desc,
            IT.risk_code_desc,
            P.shortname,
            P.resolved_name,
            IT.ipt_rate,
            IT.comm_opening_balance,
            IT.account_handler_code,
            IT.account_handler_name,
            IT.adjustment,
            IT.cover_start_date,
            IT.expiry_date,
            IT.renewal_date,
            IT.this_premium,
            IT.commission_amount,
            IT.analysis_code_desc,
            IT.policy_status,
            IT.commission_percentage,
            IT.annual_premium,
            IT.net_premium,
            -1,
            2,
            IT.agent_comm_exists,
            IT.agencyorunderwriting,
            IT.no_income,
            IT.trans_user_name,
            IT.base_currency,
            IT.regarding,
            IT.payment_method,
            IT.insured_account,
            IT.no_of_shares,
            IT.shared_order,
            '',
            '',
            IT.fsa_in_use,
	    (
	    SELECT ROUND(SUM(ETC.value),2)
	    FROM
		document D1
	    JOIN 
		transaction_export_folder tef 
		ON tef.source_id=D1.company_id 
	    AND
		tef.document_ref=D1.document_ref 
	    AND
		tef.accounts_export_status='c'
	    JOIN
		event_log el 
		ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt 
	    JOIN 
		event_insurance_folder eif 
		ON eif.insurance_folder_cnt = el.event_cnt
	    JOIN
		event_insurance_file eifi 
		ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
	    JOIN
		event_policy_coinsurers_section EPCS
		ON EIFI.insurance_file_cnt=EPCS.insurance_file_cnt
		AND EPCS.party_cnt=P.party_cnt
	    JOIN event_tax_calculation ETC
		ON ETC.policy_coinsurers_section_id=EPCS.policy_coinsurers_section_id
		AND ETC.is_commission_tax=1
	    WHERE d1.document_id = IT.document_id 
	    ) * SIGN(IT.gross_premium)
        FROM #IncomeTrans IT
        JOIN transdetail TD
            ON TD.document_id = IT.document_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key
        JOIN party_type PT
            ON PT.party_type_id = P.party_type_id
        WHERE IT.document_id = @document_id
        AND PT.code = 'IN'
        AND TD.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
        AND IT.adjustment = 0

        FETCH NEXT FROM c_Cursor INTO @document_id

    END

    CLOSE c_cursor
    DEALLOCATE c_cursor
    UPDATE #IncomeTrans
    SET insurer_code = 'MULTI',
        insurer_name = 'Multi Insurer',
        gross_premium = 0,
        ipt_amount = 0--,
        --gross_commission=0
    WHERE no_of_insurers > 1

    /*Third PArty Details*/

    DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT document_id
    FROM #IncomeTrans
    WHERE no_of_third_Party > 1 

    OPEN c_cursor

    FETCH NEXT FROM c_cursor INTO @document_id

    WHILE @@FETCH_STATUS = 0
    BEGIN

        INSERT INTO #IncomeTrans
        (
            document_id,
            document_ref,
            gross_premium,
            ipt_amount,
            gross_commission,
            agent_commission,
            document_date,
            branch_id,
            branch_code,
            income_code,
            income_name,
            commission_account_id,
            client_code,
            client_name,
            area_desc,
            client_business,
            account_exec_code,
            account_exec_name,
            document_type_desc,
            effective_date,
            fee_amount,
            discount_amount,
            extra_premium,
            extra_commission,
            insurance_ref,
            transaction_type_desc,
            business_type_desc,
            risk_code_desc,
            insurer_code,
            insurer_name,
            ipt_rate,
            comm_opening_balance,
            account_handler_code,
            account_handler_name,
            adjustment,
            cover_start_date,
            expiry_date,
            renewal_date,
            this_premium,
            commission_amount,
            analysis_code_desc,
            policy_status,
            commission_percentage,
            annual_premium,
            net_premium,
            no_of_insurers,
            insurer_order,
            agent_comm_exists,
            agencyorunderwriting,
            no_income,
            trans_user_name,
            base_currency,
            regarding,
            payment_method,
            insured_account,
            no_of_shares,
            shared_order,
            fsa_customer_category,
            fsa_Underwriter,
            fsa_in_use,
            client_fees,
            discount_code,
            discount_name,
            third_party_code,
            third_party_name,
            no_of_third_party
        )
        SELECT
            IT.document_id,
            IT.document_ref,
            (
                SELECT ISNULL(SUM(TDI.amount), 0.0) 
                FROM Transdetail TDI
                JOIN account AI
                    ON AI.account_id = TDI.account_id
                JOIN ledger L
               ON L.ledger_id = A.ledger_id
              AND (L.ledger_short_name = 'AG' OR L.ledger_short_name = 'TR' OR L.ledger_short_name = 'UB')
              AND TDI.document_id = IT.document_id
              AND TDI.spare = 'AGENT'
            ),
            (
                SELECT
                    CASE
                        WHEN SUM(TDI.amount) < 0 THEN ISNULL(SUM(TDI.ref_amount),0)
                        WHEN SUM(TDI.amount) > 0 THEN ISNULL(SUM(TDI.ref_amount),0) * -1
                        ELSE 0
                    END
                FROM transdetail TDI
                JOIN account AI
                    ON AI.account_id = TDI.account_id
                JOIN ledger L
               ON L.ledger_id = A.ledger_id
              AND (L.ledger_short_name = 'AG' OR L.ledger_short_name = 'TR' OR L.ledger_short_name = 'UB')
              AND TDI.document_id =  IT.document_id
              AND TDI.spare = 'AGENT'
            ),
            (
                SELECT ISNULL(SUM(TDI.amount), 0.0)
                FROM Transdetail TDI
                JOIN account AI
                    ON AI.account_id = TDI.account_id
                JOIN ledger L
               ON L.ledger_id = A.ledger_id
              AND (L.ledger_short_name = 'AG' OR L.ledger_short_name = 'TR' OR L.ledger_short_name = 'UB')
              AND TDI.document_id =  IT.document_id
              AND TDI.spare = 'AGENT'
            ),
            0,
            IT.document_date,
            IT.branch_id,
            IT.branch_code,
            IT.income_code,
            IT.income_name,
            IT.commission_account_id,
            IT.client_code,
            IT.client_name,
            IT.area_desc,
            IT.client_business,
            IT.account_exec_code,
            IT.account_exec_name,
            IT.document_type_desc,
            IT.effective_date,
            0,
            0,
            0,
            0,
            IT.insurance_ref,
            IT.transaction_type_desc,
            IT.business_type_desc,
            IT.risk_code_desc,
            IT.insurer_code,
            IT.insurer_name,
            IT.ipt_rate,
            IT.comm_opening_balance,
            IT.account_handler_code,
            IT.account_handler_name,
            IT.adjustment,
            IT.cover_start_date,
            IT.expiry_date,
            IT.renewal_date,
            IT.this_premium,
            IT.commission_amount,
            IT.analysis_code_desc,
            IT.policy_status,
            IT.commission_percentage,
            IT.annual_premium,
            IT.net_premium,
            -1,
            2,
            IT.agent_comm_exists,
            IT.agencyorunderwriting,
            IT.no_income,
            IT.trans_user_name,
            IT.base_currency,
            IT.regarding,
            IT.payment_method,
            IT.insured_account,
            IT.no_of_shares,
            IT.shared_order,
            '',
            '',
            IT.fsa_in_use,
            0,
            IT.discount_code,
            IT.discount_name,
            P.shortname,
            P.resolved_name,
            0
        FROM #IncomeTrans IT
        JOIN transdetail TD
            ON TD.document_id = IT.document_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key
        JOIN Ledger L
           ON L.ledger_id = A.ledger_id
        WHERE IT.document_id = @document_id
       AND (L.ledger_short_name = 'AG' OR L.ledger_short_name = 'TR' OR L.ledger_short_name = 'UB')
        AND TD.spare = 'AGENT'
        AND IT.adjustment = 0

        FETCH NEXT FROM c_Cursor INTO @document_id

    END

    CLOSE c_cursor
    DEALLOCATE c_cursor





    /*Policy Shares*/

    INSERT INTO #IncomeTrans
    (
        document_id,
        document_ref,
        gross_premium,
        ipt_amount,
        gross_commission,
        agent_commission,
        document_date,
        branch_id,
        branch_code,
        income_code,
        income_name,
        commission_account_id,
        client_code,
        client_name,
        area_desc,
        client_business,
        account_exec_code,
        account_exec_name,
        document_type_desc,
        effective_date,
        fee_amount,
        discount_amount,
        extra_premium,
        extra_commission,
        insurance_ref,
        transaction_type_desc,
        business_type_desc,
        risk_code_desc,
        insurer_code,
        insurer_name,
        ipt_rate,
        comm_opening_balance,
        account_handler_code,
        account_handler_name,
        adjustment,
        cover_start_date,
        expiry_date,
        renewal_date,
        this_premium,
        commission_amount,
        analysis_code_desc,
        policy_status,
        commission_percentage,
        annual_premium,
        net_premium,
        no_of_insurers,
        insurer_order,
        agent_comm_exists,
        agencyorunderwriting,
        no_income,
        trans_user_name,
        base_currency,
        regarding,
        payment_method,
        insured_account,
        no_of_shares,
        shared_order,
        fsa_customer_category,
        fsa_Underwriter,
        fsa_in_use
    )
    SELECT
        IT.document_id,
        IT.document_ref,
        0,
        0,
        0,
        0,
        IT.document_date,
        IT.branch_id,
        IT.branch_code,
        IT.income_code,
        IT.income_name,
        IT.commission_account_id,
        P.shortname,
        P.resolved_name,
        IT.area_desc,
        IT.client_business,
        IT.account_exec_code,
        IT.account_exec_name,
        IT.document_type_desc,
        IT.effective_date,
        0,
        0,
        0,
        0,
        IT.insurance_ref,
        IT.transaction_type_desc,
        IT.business_type_desc,
        IT.risk_code_desc,
        IT.insurer_code,
        IT.insurer_name,
        IT.ipt_rate,
        IT.comm_opening_balance,
        IT.account_handler_code,
        IT.account_handler_name,
        IT.adjustment,
        IT.cover_start_date,
        IT.expiry_date,
        IT.renewal_date,
        0,
        0,
        IT.analysis_code_desc,
        IT.policy_status,
        IT.commission_percentage,
        0,
        0,
        IT.no_of_insurers,
        IT.insurer_order,
        IT.agent_comm_exists,
        IT.agencyorunderwriting,
        IT.no_income,
        IT.trans_user_name,
        IT.base_currency,
        IT.regarding,
        IT.payment_method,
        IT.insured_account,
        -1,
        1,
        IT.fsa_customer_category,
        IT.fsa_Underwriter,
        IT.fsa_in_use
    FROM #IncomeTrans IT
    JOIN transdetail TD
        ON TD.document_id = IT.document_id
    JOIN account A
        ON A.account_id = TD.account_id
    JOIN ledger l
        ON L.ledger_id = A.ledger_id
        AND L.ledger_short_name = 'SA'
    JOIN party P
        ON P.party_cnt = A.account_key
    WHERE IT.no_of_insurers = 1
    AND IT.no_of_shares > 1
    AND NOT EXISTS
        (
            SELECT
                NULL
            FROM #IncomeTrans
            WHERE document_id = IT.document_id
            AND client_code = A.short_code
        )
    AND TD.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = TD.document_id
            AND account_id = TD.account_id
        )

    DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT
        document_id,
        gross_premium,
        ipt_amount,
        gross_commission,
        agent_commission,
        fee_amount,
        discount_amount,
        extra_premium,
        extra_commission,
        this_premium,
        commission_amount,
        annual_premium,
        net_premium
    FROM #IncomeTrans
    WHERE no_of_insurers = 1
    AND no_of_shares > 1
    AND shared_order = 0

    OPEN c_cursor

    FETCH NEXT FROM c_cursor INTO
        @document_id,
        @gross_premium,
        @ipt_amount,
        @gross_commission,
        @agent_commission,
        @fee_amount,
        @discount_amount,
        @extra_premium,
        @extra_commission,
        @this_premium,
        @commission_amount,
        @annual_premium,
        @net_premium

    WHILE @@FETCH_STATUS = 0
    BEGIN

    
        SELECT
            @full_client_amount = SUM(amount)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'SA'
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
        WHERE td.document_id = @document_id
        AND tt.code = 'NET'



        DECLARE c_cursor2 CURSOR FAST_FORWARD FOR
            SELECT
                a.short_code,
                SUM(td.amount)
            FROM #IncomeTrans it
            JOIN transdetail td
                ON td.document_id = it.document_id
            JOIN account a
                ON a.account_id = td.account_id
                AND a.short_code = it.client_code
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
            WHERE it.document_id = @document_id
            AND tt.code = 'NET'
            GROUP BY
                a.short_code

        OPEN c_cursor2

        FETCH NEXT FROM c_cursor2 INTO
            @share_client_code,
            @shared_amount

        WHILE @@FETCH_STATUS = 0
        BEGIN

            /*Get this clients percentage rate*/
            IF @full_client_amount = 0
            BEGIN
                SELECT @shared_rate = 0
            END
            ELSE
            BEGIN
                SELECT @shared_rate = @shared_amount / @full_client_amount
            END

            SELECT @share_gross_premium = ROUND(ISNULL(@gross_premium,0) * @shared_rate, 2)
            SELECT @share_ipt_amount = ROUND(ISNULL(@ipt_amount,0) * @shared_rate, 2)
            SELECT @share_gross_commission = ROUND(ISNULL(@gross_commission,0) * @shared_rate, 2)
            SELECT @share_agent_commission = ROUND(ISNULL(@agent_commission,0) * @shared_rate, 2)
            SELECT @share_fee_amount = ROUND(ISNULL(@fee_amount,0) * @shared_rate, 2)
            SELECT @share_discount_amount = ROUND(ISNULL(@discount_amount,0) * @shared_rate, 2)
            SELECT @share_extra_premium = ROUND(ISNULL(@extra_premium,0) * @shared_rate, 2)
            SELECT @share_extra_commission = ROUND(ISNULL(@extra_commission,0) * @shared_rate, 2)
            SELECT @share_this_premium = ROUND(ISNULL(@this_premium,0) * @shared_rate, 2)
            SELECT @share_commission_amount = ROUND(ISNULL(@commission_amount,0) * @shared_rate, 2)
            SELECT @share_annual_premium = ROUND(ISNULL(@annual_premium,0) * @shared_rate, 2)
            SELECT @share_net_premium = ROUND(ISNULL(@net_premium,0) * @shared_rate, 2)

            UPDATE #IncomeTrans
            SET gross_premium = @share_gross_premium,
                ipt_amount = @share_ipt_amount,
                gross_commission = @share_gross_commission,
                agent_commission = @share_agent_commission,
                fee_amount = @share_fee_amount,
                discount_amount = @share_discount_amount,
                extra_premium = @share_extra_premium,
                extra_commission = @share_extra_commission,
                this_premium = @share_this_premium,
                commission_amount = @share_commission_amount,
                annual_premium = @share_annual_premium,
                net_premium = @share_net_premium
            WHERE document_id = @document_id
            AND client_code = @share_client_code

            FETCH NEXT FROM c_Cursor2 INTO
                @share_client_code,
                @shared_amount

        END

        CLOSE c_cursor2
        DEALLOCATE c_cursor2

        FETCH NEXT FROM c_Cursor INTO
            @document_id,
            @gross_premium,
            @ipt_amount,
            @gross_commission,
            @agent_commission,
            @fee_amount,
            @discount_amount,
            @extra_premium,
            @extra_commission,
            @this_premium,
            @commission_amount,
            @annual_premium,
            @net_premium

    END

    CLOSE c_cursor
    DEALLOCATE c_cursor

END
ELSE
BEGIN

    DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT document_id
    FROM #IncomeTrans
    WHERE no_of_fees > 1

    OPEN c_cursor

    FETCH NEXT FROM c_cursor INTO @document_id

    WHILE @@FETCH_STATUS = 0
    BEGIN

        INSERT INTO #IncomeTrans
        (
            document_id,
            document_ref,
            gross_premium,
            ipt_amount,
            gross_commission,
            agent_commission,
            document_date,
            branch_id,
            branch_code,
            income_code,
            income_name,
            commission_account_id,
            client_code,
            client_name,
            area_desc,
            client_business,
            account_exec_code,
            account_exec_name,
            document_type_desc,
            effective_date,
            fee_amount,
            discount_amount,
            extra_premium,
            extra_commission,
            insurance_ref,
            transaction_type_desc,
            business_type_desc,
            risk_code_desc,
            insurer_code,
            insurer_name,
            ipt_rate,
            comm_opening_balance,
            account_handler_code,
            account_handler_name,
            adjustment,
            cover_start_date,
            expiry_date,
            renewal_date,
            this_premium,
            commission_amount,
            analysis_code_desc,
            policy_status,
            commission_percentage,
            annual_premium,
            net_premium,
            no_of_fees,
            no_of_insurers,
            insurer_order,
            agent_comm_exists,
            agencyorunderwriting,
            no_income,
            trans_user_name,
            base_currency,
            regarding,
            payment_method,
            insured_account,
            no_of_shares,
            shared_order,
            fsa_customer_category,
            fsa_Underwriter,
            fsa_in_use
        )
        SELECT
            IT.document_id,
            IT.document_ref,
            0,
            0,
            0,
            0,
            IT.document_date,
            IT.branch_id,
            IT.branch_code,
            P.shortname,
            P.resolved_name,
            IT.commission_account_id,
            IT.client_code,
            IT.client_name,
            IT.area_desc,
            IT.client_business,
            IT.account_exec_code,
            IT.account_exec_name,
            IT.document_type_desc,
            IT.effective_date,
            TD.amount * -1,
            0,
            0,
            0,
            IT.insurance_ref,
            IT.transaction_type_desc,
            IT.business_type_desc,
            IT.risk_code_desc,
            IT.insurer_code,
            IT.insurer_name,
            IT.ipt_rate,
            IT.comm_opening_balance,
            IT.account_handler_code,
            IT.account_handler_name,
            IT.adjustment,
            IT.cover_start_date,
            IT.expiry_date,
            IT.renewal_date,
            IT.this_premium,
            IT.commission_amount,
            IT.analysis_code_desc,
            IT.policy_status,
            IT.commission_percentage,
            IT.annual_premium,
            IT.net_premium,
            -1,
            IT.no_of_insurers,
            IT.insurer_order,
            IT.agent_comm_exists,
            IT.agencyorunderwriting,
            IT.no_income,
            IT.trans_user_name,
            IT.base_currency,
            IT.regarding,
            IT.payment_method,
            IT.insured_account,
            IT.no_of_shares,
            IT.shared_order,
            '',
            '',
            IT.fsa_in_use
        FROM #IncomeTrans IT
        JOIN transdetail TD
            ON TD.document_id = IT.document_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key
        JOIN party_type PT
            ON PT.party_type_id = P.party_type_id
        WHERE IT.document_id = @document_id
        AND PT.code = 'FE'
        AND IT.income_code <> P.shortname
        AND IT.adjustment = 0

        FETCH NEXT FROM c_Cursor INTO @document_id

    END

    CLOSE c_cursor
    DEALLOCATE c_cursor

END

SET NOCOUNT OFF

/*Return required data*/
SELECT
    *,
    @NZConfig
FROM #IncomeTrans
ORDER BY
    no_income,
    income_code,
    document_id,
    adjustment,
    insurer_order,
    shared_order

DROP TABLE #IncomeTrans
GO
