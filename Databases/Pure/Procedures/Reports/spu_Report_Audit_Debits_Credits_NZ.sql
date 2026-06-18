SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Debits_Credits_NZ'
GO

CREATE PROCEDURE spu_Report_Audit_Debits_Credits_NZ
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @report_on VARCHAR(25),
    @report_for VARCHAR(20),
    @group_by VARCHAR(20),
    @then_by VARCHAR(20)
AS

DECLARE
    @document_id INT,
    @client_id INT,
    @share_number INT,
    @total_premium MONEY,
    @total_clients INT,
    @client_rate FLOAT,
    @client_premium MONEY,
    @MaxLines INT,
    @client_number INT,
    @line_number INT,
    @no_of_clients INT,
    @no_of_insurers INT,
    @no_of_agents INT,
    @no_of_subagents INT,
    @no_of_fees INT,
    @no_of_discounts INT,
    @AgencyOrUnderwriting VARCHAR(1),
    @NZConfig INT

/*Broking and underwriting display some headers differently*/
SELECT @AgencyOrUnderwriting = (SELECT value FROM Hidden_Options WHERE option_number = 1)

/*This determines if tax is to be considered*/
SELECT @NZConfig = ISNULL(value, 0) FROM Hidden_Options WHERE option_number = 86

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF ISNULL(@report_on,'') = ''
BEGIN
    SELECT @report_on = 'Transaction Date'
END

SET NOCOUNT ON

CREATE TABLE #transactions
(
    document_id INT,
    share_number INT,
    line_number INT,

    /*Client Details*/
    no_of_clients INT,
    client_id INT,
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    client_type VARCHAR(10),
    client_type_description VARCHAR(255),
    client_premium MONEY,
    client_rate FLOAT,

    /*Insurer Details*/
    insurer_id INT,
    insurer_code VARCHAR(20),
    insurer_premium MONEY,
    insurer_commission MONEY,
    insurer_tax MONEY,

    /*Agent Details*/
    agent_id INT,
    agent_code VARCHAR(20),
    agent_commission MONEY,
    agent_tax MONEY,

    /*Sub-Agent Details*/
    subagent_id INT,
    sub_agent_code VARCHAR(20),
    sub_agent_commission MONEY,
    sub_agent_tax MONEY,

    /*Fee Details*/
    fee_id INT,
    fee_code VARCHAR(20),
    fee MONEY,
    fee_tax MONEY,

    /*Discount Details*/
    discount_id INT,
    discount_code VARCHAR(20),
    discount MONEY,
    discount_tax MONEY,

    /*Net Commission Details*/
    net_commission_id INT,
    net_commission_code VARCHAR(20),
    net_commission MONEY,

    /*Transction And Policy Details*/
    year_name VARCHAR(20) ,
    period_name VARCHAR(15),
    accounting_date DATETIME,
    insurance_ref VARCHAR(30),
    comment VARCHAR(60),
    document_ref VARCHAR(25),
    document_type VARCHAR(255),
    risk_code VARCHAR(10),
    risk_description VARCHAR(255),
    cover_start_date DATETIME,
    commission_percentage FLOAT,
    document_date DATETIME,
    effective_date DATETIME,
    account_handler VARCHAR(255),
    account_executive VARCHAR(255),
    company_id INT,
    company_desc VARCHAR(255),
    insurance_file_cnt INT,
    rounding MONEY
)

CREATE INDEX transactions_document_id ON #transactions (document_id)

CREATE TABLE #transaction_details
(
    document_id INT,
    client_account_id INT,
    client_sequence INT,
    other_account_id INT,
    other_ledger_short_name VARCHAR(2),
    other_sequence INT
)
CREATE INDEX transaction_details_document_id ON #transaction_details (document_id)

CREATE TABLE #transaction_selection
(
    document_id INT,
    no_of_clients INT,
    no_of_insurers INT,
    no_of_agents INT,
    no_of_subagents INT,
    no_of_fees INT,
    no_of_discounts INT
)
CREATE INDEX transaction_selection_document_id ON #transaction_selection (document_id)



INSERT INTO #transaction_selection
(
    document_id,
    no_of_clients,
    no_of_insurers,
    no_of_agents,
    no_of_subagents,
    no_of_fees,
    no_of_discounts
)
SELECT
    d.document_id,
    /*Get number of clients on the transaction*/
    (
        SELECT
            COUNT(DISTINCT a.account_id)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'SA'
        WHERE td.document_id = d.document_id
    ),
    /*Get number of insurers on the transaction*/
    (
        SELECT
            COUNT(DISTINCT a.account_id)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'IN'
        WHERE td.document_id = d.document_id
    ),
    /*Get number of agents on the transaction*/
    (
        SELECT
            COUNT(DISTINCT a.account_id)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'AG'
        WHERE td.document_id = d.document_id
    ),
    /*Get number of sub-agents on the transaction*/
    (
        SELECT
            COUNT(DISTINCT a.account_id)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'UB'
        WHERE td.document_id = d.document_id
    ),
    /*Get number of fees on the transaction*/
    (
        SELECT
            COUNT(DISTINCT a.account_id)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'FE'
        WHERE td.document_id = d.document_id
    ),
    /*Get number of discounts on the transaction*/
    (
        SELECT
            COUNT(DISTINCT a.account_id)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'DI'
        WHERE td.document_id = d.document_id
    )
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1
WHERE d.company_id = ISNULL(@branch_id, d.company_id)
AND (
        (
            @report_on = 'Transaction Date'
            AND
            d.document_date BETWEEN @start_date AND @end_date
        )
        OR
        (
            @report_on = 'Effective Date'
            AND
            td.ref_date BETWEEN @start_date AND @end_date
        )
    )
AND (
        (
            d.documenttype_id IN (2, 4, 15, 17, 31, 35)
            AND
            @report_for = 'Debits Only'
        )
        OR
        (
            d.documenttype_id IN (3, 5, 16, 18, 32, 36)
            AND
            @report_for = 'Credits Only'
        )
        OR
        (
       	    d.documenttype_id IN (2, 3, 4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
       	    AND
            @report_for = 'Debits & Credits'
        )
    )

INSERT INTO #transaction_details
(
    document_id,
    client_account_id,
    client_sequence,
    other_account_id,
    other_ledger_short_name,
    other_sequence
)
SELECT
    ts.document_id,
    a.account_id,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'SA'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    ),
    a_in.account_id,
    l_in.ledger_short_name,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'IN'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td_in.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    )
FROM #transaction_selection ts
JOIN transdetail td
    ON td.document_id = ts.document_id
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'SA'

JOIN transdetail td_in
    ON td_in.document_id = ts.document_id
    AND td_in.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td_in.document_id
            AND account_id = td_in.account_id
        )
JOIN account a_in
    ON a_in.account_id = td_in.account_id
JOIN ledger l_in
    ON l_in.ledger_id = a_in.ledger_id
    AND l_in.ledger_short_name = 'IN'

UNION

SELECT
    ts.document_id,
    a.account_id,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'SA'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    ),
    a_ag.account_id,
    l_ag.ledger_short_name,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'AG'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td_ag.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    )
FROM #transaction_selection ts
JOIN transdetail td
    ON td.document_id = ts.document_id
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'SA'

JOIN transdetail td_ag
    ON td_ag.document_id = ts.document_id
    AND td_ag.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td_ag.document_id
            AND account_id = td_ag.account_id
        )
JOIN account a_ag
    ON a_ag.account_id = td_ag.account_id
JOIN ledger l_ag
    ON l_ag.ledger_id = a_ag.ledger_id
    AND l_ag.ledger_short_name = 'AG'

UNION

SELECT
    ts.document_id,
    a.account_id,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'SA'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    ),
    a_ub.account_id,
    l_ub.ledger_short_name,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'UB'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td_ub.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    )
FROM #transaction_selection ts
JOIN transdetail td
    ON td.document_id = ts.document_id
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'SA'

JOIN transdetail td_ub
    ON td_ub.document_id = ts.document_id
    AND td_ub.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td_ub.document_id
            AND account_id = td_ub.account_id
        )
JOIN account a_ub
    ON a_ub.account_id = td_ub.account_id
JOIN ledger l_ub
    ON l_ub.ledger_id = a_ub.ledger_id
    AND l_ub.ledger_short_name = 'UB'

UNION

SELECT
    ts.document_id,
    a.account_id,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'SA'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    ),
    a_fe.account_id,
    l_fe.ledger_short_name,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'FE'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td_fe.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    )
FROM #transaction_selection ts
JOIN transdetail td
    ON td.document_id = ts.document_id
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'SA'

JOIN transdetail td_fe
    ON td_fe.document_id = ts.document_id
    AND td_fe.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td_fe.document_id
            AND account_id = td_fe.account_id
        )
JOIN account a_fe
    ON a_fe.account_id = td_fe.account_id
JOIN ledger l_fe
    ON l_fe.ledger_id = a_fe.ledger_id
    AND l_fe.ledger_short_name = 'FE'

UNION

SELECT
    ts.document_id,
    a.account_id,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'SA'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    ),
    a_di.account_id,
    l_di.ledger_short_name,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'DI'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td_di.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    )
FROM #transaction_selection ts
JOIN transdetail td
    ON td.document_id = ts.document_id
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'SA'

JOIN transdetail td_di
    ON td_di.document_id = ts.document_id
    AND td_di.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td_di.document_id
            AND account_id = td_di.account_id
        )
JOIN account a_di
    ON a_di.account_id = td_di.account_id
JOIN ledger l_di
    ON l_di.ledger_id = a_di.ledger_id
    AND l_di.ledger_short_name = 'DI'

UNION

SELECT
    ts.document_id,
    a.account_id,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'SA'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    ),
    a_co.account_id,
    l_co.ledger_short_name,
    (
        SELECT
            SUM(1)
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
            AND lx.ledger_short_name = 'CO'
        WHERE tdx.document_id = ts.document_id
        AND tdx.document_sequence <= td_co.document_sequence
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail
                WHERE document_id = tdx.document_id
                AND account_id = tdx.account_id
            )
    )
FROM #transaction_selection ts
JOIN transdetail td
    ON td.document_id = ts.document_id
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'SA'

JOIN transdetail td_co
    ON td_co.document_id = ts.document_id
    AND td_co.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td_co.document_id
            AND account_id = td_co.account_id
        )
JOIN account a_co
    ON a_co.account_id = td_co.account_id
JOIN ledger l_co
    ON l_co.ledger_id = a_co.ledger_id
    AND l_co.ledger_short_name = 'CO'


/*Get the maximum number of clients on the transactions*/
SELECT
    @total_clients = MAX(no_of_clients)
FROM #transaction_selection

SELECT
    @MaxLines = MAX(no_of_accounts)
FROM
    (
        SELECT
            MAX(no_of_insurers) 'no_of_accounts'
        FROM #transaction_selection
        UNION
        SELECT
            MAX(no_of_agents) 'no_of_accounts'
        FROM #transaction_selection
        UNION
        SELECT
            MAX(no_of_subagents) 'no_of_accounts'
        FROM #transaction_selection
        UNION
        SELECT
            MAX(no_of_fees) 'no_of_accounts'
        FROM #transaction_selection
        UNION
        SELECT
            MAX(no_of_discounts) 'no_of_accounts'
        FROM #transaction_selection
    ) max_numbers


/*Insert lines into the transactions table for each client and other accounts, just using IDs at the moment*/
SELECT @client_number = 1

WHILE @client_number <= @total_clients
BEGIN

    SELECT @line_number = 1

    WHILE @line_number <= @MaxLines
    BEGIN

        INSERT INTO #transactions
        (
            document_id,
            share_number,
            line_number,

            no_of_clients,
            client_rate,

            client_id,
            insurer_id,
            agent_id,
            subagent_id,
            fee_id,
            discount_id,
            net_commission_id
        )
        SELECT
            ts.document_id,
            @client_number,
            @line_number,
            ts.no_of_clients,
            1,
            (
                SELECT
                    MIN(client_account_id)
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_sequence = 1
            ),
            (
                SELECT
                    other_account_id
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_ledger_short_name = 'IN'
            ),
            (
                SELECT
                    other_account_id
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_ledger_short_name = 'AG'
            ),
            (
                SELECT
                    other_account_id
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_ledger_short_name = 'UB'
            ),
            (
                SELECT
                    other_account_id
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_ledger_short_name = 'FE'
            ),
            (
                SELECT
                    other_account_id
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_ledger_short_name = 'DI'
            ),
            (
                SELECT
                    other_account_id
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
                AND other_ledger_short_name = 'CO'
            )
        FROM #transaction_selection ts
        WHERE EXISTS
            (
                SELECT
                    NULL
                FROM #transaction_details
                WHERE document_id = ts.document_id
                AND client_sequence = @client_number
                AND other_sequence = @line_number
            )

        SELECT @line_number = @line_number + 1

    END

    SELECT @client_number = @client_number + 1
END


/*Add client details*/
UPDATE t
SET client_code = a.short_code,
    client_name = p.resolved_name,
    client_type = pt.code,
    client_type_description = pt.description,
    client_premium = 
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0)
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.account_id = t.client_id        
            AND NOT EXISTS
                (
                    SELECT 
                        NULL
                    FROM transdetail td1
                    JOIN account a1
                        ON a1.account_id = td1.account_id
                    JOIN ledger l1
                        ON l1.ledger_id = a1.ledger_id
                        AND l1.ledger_short_name = 'UB'
                    WHERE td1.document_id = td.document_id
                    AND NOT EXISTS
                        (
                            SELECT
                                NULL
                            FROM transdetail td
                            JOIN transmatch tm
                                ON tm.transdetail_id = td.transdetail_id
                                AND tm.is_reversed IS NULL
                                AND tm.allocationdetail_id IS NOT NULL
                            JOIN matchgroup mg
                                ON mg.match_id = tm.match_id
                                AND mg.match_date <= @end_date
                            JOIN transmatch tmx
                                ON tmx.match_id = tm.match_id
                                AND tmx.is_reversed IS NULL
                                AND tmx.allocationdetail_id IS NOT NULL
                            JOIN transdetail tdx
                                ON tdx.transdetail_id = tmx.transdetail_id
                            JOIN document dx
                                ON dx.document_id = tdx.document_id
                                AND dx.documenttype_id IN (33,34)                                                
                            WHERE td.document_id = td1.document_id
                            AND td.document_sequence = 1
                        )
                )                 
        ),
    account_executive = ISNULL(pae.resolved_name, '')
FROM #transactions t
JOIN account a
    ON a.account_id = t.client_id
JOIN party p
    ON p.party_cnt = a.account_key
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
LEFT JOIN party pae
    ON pae.party_cnt = p.consultant_cnt


/*Only do this bit if there is at least one transaction that has a policy share*/
IF @total_clients > 1
BEGIN
    /*Get the clients rate of the share for all poicy shared transactions*/
    DECLARE c_cursor CURSOR FAST_FORWARD FOR
        SELECT
            document_id,
            client_id,
            share_number,
            no_of_clients
        FROM #transactions
        WHERE no_of_clients > 1
        AND client_id IS NOT NULL

    OPEN c_cursor

    FETCH NEXT FROM c_cursor INTO
        @document_id,
        @client_id,
        @share_number,
        @no_of_clients

    WHILE @@FETCH_STATUS = 0
    BEGIN

        /*Get the premium amount for this client*/
        SELECT
            @client_premium = SUM(ISNULL(ROUND(td.amount,2),0))
        FROM transdetail td
        WHERE td.document_id = @document_id
        AND td.account_id = @client_id
        AND (
                NOT EXISTS
                    (
                        SELECT
                            NULL
                        FROM transdetail td
                        JOIN account a
                            ON a.account_id = td.account_id
                        JOIN ledger l
                            ON l.ledger_id = a.ledger_id
                            AND l.ledger_short_name = 'UB'
                        WHERE td.document_id = @document_id
                    )
                OR
                (
                    EXISTS
                        (
                            SELECT
                                NULL
                            FROM transdetail td
                            JOIN account a
                                ON a.account_id = td.account_id
                            JOIN ledger l
                                ON l.ledger_id = a.ledger_id
                                AND l.ledger_short_name = 'UB'
                            WHERE td.document_id = @document_id
                        )
                    AND td.document_sequence NOT IN
                        (
                            SELECT
                                MAX(document_sequence)
                            FROM transdetail
                            WHERE account_id = td.account_id
                        )
                )
            )

        /*Get the total premium amount*/
        SELECT
            @total_premium = SUM(ISNULL(ROUND(td.amount,2),0))
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'SA'
        WHERE td.document_id = @document_id
        AND (
                NOT EXISTS
                    (
                        SELECT
                            NULL
                        FROM transdetail td
                        JOIN account a
                            ON a.account_id = td.account_id
                        JOIN ledger l
                            ON l.ledger_id = a.ledger_id
                            AND l.ledger_short_name = 'UB'
                        WHERE td.document_id = @document_id
                    )
                OR
                (
                    EXISTS
                        (
                            SELECT
                                NULL
                            FROM transdetail td
                            JOIN account a
                                ON a.account_id = td.account_id
                            JOIN ledger l
                                ON l.ledger_id = a.ledger_id
                                AND l.ledger_short_name = 'UB'
                            WHERE td.document_id = @document_id
                        )
                    AND td.document_sequence NOT IN
                        (
                            SELECT
                                MAX(document_sequence)
                            FROM transdetail
                            WHERE account_id = td.account_id
                        )
                )
            )

        /*Get this clients percentage rate*/
        IF @total_premium = 0
        BEGIN
            IF @no_of_clients = 1
            BEGIN
                SELECT @client_rate = 1
            END
            ELSE
            BEGIN
                SELECT @client_rate = 0
            END
        END
        ELSE
        BEGIN
            SELECT @client_rate = (@client_premium / @total_premium)
        END

        /*Update all rows for the share_number with the client rate so that all other accounts will be affected by it.*/
        UPDATE #transactions
        SET client_rate = @client_rate
        WHERE document_id = @document_id
        AND share_number = @share_number


        FETCH NEXT FROM c_cursor INTO
            @document_id,
            @client_id,
            @share_number,
            @no_of_clients

    END

    CLOSE c_cursor
    DEALLOCATE c_cursor

END


/*Add insurer details*/
UPDATE t
SET t.insurer_code = a.short_code,
    t.insurer_premium =
        (
            SELECT
                ISNULL(ROUND(SUM(td.amount) * t.client_rate, 2),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'COMMADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.insurer_id
        ),
    t.insurer_commission =
        (
            SELECT
                ISNULL(ROUND(td.amount * t.client_rate,2) * -1,0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'COMM'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.insurer_id
        ),
    t.insurer_tax =
        (
            SELECT
                ISNULL(ROUND(SUM(EICS.commission_tax_applied) * t.client_rate,2),0)
            FROM document d
            JOIN transaction_export_folder tef
                ON tef.source_id = d.company_id
                AND tef.document_ref = d.document_ref
                AND tef.accounts_export_status = 'c'
            JOIN event_log el
                ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
            JOIN event_insurance_folder eif
                ON eif.insurance_folder_cnt = el.event_cnt
            JOIN event_insurance_file eifi
                ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
            JOIN event_insurance_cob_section EICS
                ON EICS.insurance_file_cnt = eifi.insurance_file_cnt
            WHERE d.document_id = t.document_id
        )
FROM #transactions t
JOIN account a
    ON a.account_id = t.insurer_id


/*Add agent details*/
UPDATE t
SET t.agent_code = a.short_code,
    t.agent_commission =
        (
            SELECT
                ISNULL(ROUND(SUM(td.amount) * t.client_rate, 2),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'AGENTADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.agent_id
        ),
    t.agent_tax =
        (
            SELECT
                ISNULL(ROUND(SUM(epa.tax_amount) * t.client_rate, 2),0)
            FROM document d
            JOIN transaction_export_folder tef
                ON tef.source_id = d.company_id
                AND tef.document_ref = d.document_ref
                AND tef.accounts_export_status = 'c'
            JOIN event_log el
                ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
            JOIN event_insurance_folder eif
                ON eif.insurance_folder_cnt = el.event_cnt
            JOIN event_insurance_file eifi
                ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
            JOIN event_policy_agents epa
                ON epa.insurance_file_cnt = eifi.insurance_file_cnt
            JOIN party_agent pa
                ON epa.agent_cnt = pa.party_cnt
            JOIN party_agent_type pat
                ON pat.party_agent_type_id = pa.party_agent_type_id
            WHERE d.document_id = t.document_id
            AND pat.description = 'AGENT'
        )
FROM #transactions t
JOIN account a
    ON a.account_id = t.agent_id


/*Add sub-agent details*/
UPDATE t
SET t.sub_agent_code = a.short_code,
    t.sub_agent_commission =
        (
            SELECT
                ISNULL(ROUND(SUM(td.amount) * t.client_rate, 2),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'AGENTADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.subagent_id
        ),
    t.sub_agent_tax =
        (
            SELECT
                ISNULL(ROUND(SUM(epa.tax_amount) * t.client_rate, 2),0)
            FROM document d
            JOIN transaction_export_folder tef
                ON tef.source_id = d.company_id
                AND tef.document_ref = d.document_ref
                AND tef.accounts_export_status = 'c'
            JOIN event_log el
                ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
            JOIN event_insurance_folder eif
                ON eif.insurance_folder_cnt = el.event_cnt
            JOIN event_insurance_file eifi
                ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
            JOIN event_policy_agents epa
                ON epa.insurance_file_cnt = eifi.insurance_file_cnt
            JOIN party_agent pa
                ON epa.agent_cnt = pa.party_cnt
            JOIN party_agent_type pat
                ON pat.party_agent_type_id = pa.party_agent_type_id
            WHERE d.document_id = t.document_id
            AND pat.description = 'SUB AGENT'
        )
FROM #transactions t
JOIN account a
    ON a.account_id = t.subagent_id


/*Add fee details*/
UPDATE t
SET t.fee_code = a.short_code,
    t.fee =
        (
            SELECT
                ISNULL(ROUND(SUM(td.amount) * t.client_rate, 2),0)
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.account_id = t.fee_id
        ),
    t.fee_tax =
        (
            SELECT
                ISNULL(ROUND(SUM(epf.tax_amount) * t.client_rate,2),0)
            FROM document d
            JOIN transaction_export_folder tef
                ON tef.source_id = d.company_id
                AND tef.document_ref = d.document_ref
                AND tef.accounts_export_status = 'c'
            JOIN event_log el
                ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
            JOIN event_insurance_folder eif
                ON eif.insurance_folder_cnt = el.event_cnt
            JOIN event_insurance_file eifi
                ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
            JOIN event_policy_fee epf
                ON epf.insurance_file_cnt = eifi.insurance_file_cnt
            JOIN party pa
                ON epf.party_cnt = pa.party_cnt
            WHERE d.document_id = t.document_id
            AND pa.party_type_id = 9
        )
FROM #transactions t
JOIN account a
    ON a.account_id = t.fee_id


/*Add discount details*/
UPDATE t
SET t.discount_code = a.short_code,
    t.discount =
        (
            SELECT
                ISNULL(ROUND(SUM(td.amount) * t.client_rate, 2),0)
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.account_id = t.discount_id
        ),
    t.discount_tax =
        (
            SELECT
                ISNULL(ROUND(SUM(epf.tax_amount) * t.client_rate,2),0)
            FROM document d
            JOIN transaction_export_folder tef
                ON tef.source_id = d.company_id
                AND tef.document_ref = d.document_ref
                AND tef.accounts_export_status = 'c'
            JOIN event_log el
                ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
            JOIN event_insurance_folder eif
                ON eif.insurance_folder_cnt = el.event_cnt
            JOIN event_insurance_file eifi
                ON eifi.insurance_folder_cnt = eif.insurance_folder_cnt
            JOIN event_policy_fee epf
                ON epf.insurance_file_cnt = eifi.insurance_file_cnt
            JOIN party pa
                ON epf.party_cnt = pa.party_cnt
            WHERE d.document_id = t.document_id
            AND pa.party_type_id = 11
        )
FROM #transactions t
JOIN account a
    ON a.account_id = t.discount_id


/*Add net commission details*/
UPDATE t
SET t.net_commission_code = a.short_code,
    t.net_commission =
        (
            SELECT
                ISNULL(ROUND(SUM(td.amount) * t.client_rate, 2),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'BROK ADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.net_commission_id
        )
FROM #transactions t
JOIN account a
    ON a.account_id = t.net_commission_id


/*Only do this bit if there is at least one transaction that has a policy share*/
IF @total_clients > 1
BEGIN

    /*Fix any rounding problems when we split across each policy share*/

    /*Check rounding for net insurer premium*/
    UPDATE t
    SET insurer_premium = insurer_premium +
        (
            SELECT
                SUM(insurer_premium)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.insurer_id = t.insurer_id
        )
        -
        (
            SELECT
                ISNULL(SUM(td.amount),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'COMMADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.insurer_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.insurer_id IS NOT NULL

    /*Check rounding for insurer commission*/
    UPDATE t
    SET insurer_commission = insurer_commission +
        (
            SELECT
                SUM(insurer_commission)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.insurer_id = t.insurer_id
        )
        -
        (
            SELECT
                ISNULL(SUM(td.amount) * -1,0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'COMM'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.insurer_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.insurer_id IS NOT NULL

    /*Check rounding for agents*/
    UPDATE t
    SET agent_commission = agent_commission +
        (
            SELECT
                SUM(agent_commission)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.agent_id = t.agent_id
        )
        -
        (
            SELECT
                ISNULL(SUM(td.amount),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'AGENTADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.agent_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.agent_id IS NOT NULL

    /*Check rounding for sub-agents*/
    UPDATE t
    SET sub_agent_commission = sub_agent_commission +
        (
            SELECT
                SUM(sub_agent_commission)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.subagent_id = t.subagent_id
        )
        -
        (
            SELECT
                ISNULL(SUM(td.amount),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'AGENTADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.subagent_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.subagent_id IS NOT NULL

    /*Check rounding for fees*/
    UPDATE t
    SET fee = fee +
        (
            SELECT
                SUM(fee)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.fee_id = t.fee_id
        )
        -
        (
            SELECT
                ISNULL(SUM(td.amount),0)
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.account_id = t.fee_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.fee_id IS NOT NULL

    /*Check rounding for discounts*/
    UPDATE t
    SET discount = discount +
        (
            SELECT
                SUM(discount)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.discount_id = t.discount_id
        )
        -
        (
            SELECT
               ISNULL(SUM(td.amount),0)
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.account_id = t.discount_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.discount_id IS NOT NULL

    /*Check rounding for net commission*/
    UPDATE t
    SET net_commission = net_commission +
        (
            SELECT
                SUM(net_commission)
            FROM #transactions tx
            WHERE tx.document_id = t.document_id
            AND tx.net_commission_id = t.net_commission_id
        )
        -
        (
            SELECT
                ISNULL(SUM(td.amount),0)
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code <> 'BROK ADJ'
            WHERE td.document_id = t.document_id
            AND td.account_id = t.net_commission_id
        )
    FROM #transactions t
    WHERE t.no_of_clients > 1
    AND t.share_number = t.no_of_clients
    AND t.net_commission_id IS NOT NULL

END

/*Check for actual rounding on transaction. Just show against first line.*/
UPDATE t
SET rounding =
    (
        SELECT
            SUM(td.amount)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'NO'
        WHERE td.document_id = t.document_id
    )
FROM #transactions t
WHERE share_number = 1
AND line_number = 1
AND (
        SELECT
            SUM(td.amount)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'NO'
        WHERE td.document_id = t.document_id
    ) <> 0

/*Update records with transaction/policy infomation*/
UPDATE t
SET year_name = p.year_name,
    period_name = p.period_name,
    accounting_date = td.accounting_date,
    insurance_ref = ISNULL(i.insurance_ref, ''),
    comment = LEFT(d.comment,60),
    document_ref = d.document_ref,
    document_type = dt.description,
    risk_code = ISNULL(rc.code, ''),
    risk_description = ISNULL(rc.description, ''),
    cover_start_date = ISNULL(i.cover_start_date, ''),
    commission_percentage = ISNULL(i.commission_percentage, 0),
    document_date = d.document_date,
    account_handler = ISNULL(pah.resolved_name, ''),
    company_id = d.company_id,
    company_desc = c.description,
    insurance_file_cnt = i.insurance_file_cnt
FROM #transactions t
JOIN document d
    ON d.document_id = t.document_id
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
JOIN company c
    ON c.company_id = d.company_id
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1
JOIN period p
    ON p.period_id = td.period_id
LEFT JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt
LEFT JOIN party pah
    ON pah.party_cnt = i.account_handler_cnt
LEFT JOIN risk_code rc
    ON rc.risk_code_id = i.risk_code_id


SET NOCOUNT OFF

SELECT
    t.*,
    (
        CASE @group_by
            WHEN 'Company' THEN
                (
                    SELECT
                        code
                    FROM company
                    WHERE company_id = t.company_id
                )
            WHEN 'Account Handler' THEN
                (
                    SELECT
                        ISNULL(MAX(AccH.shortname), '')
                    FROM insurance_file IFI
                    LEFT JOIN Party AccH
                        ON AccH.party_cnt = IFI.account_handler_cnt
                    WHERE IFI.insurance_file_cnt = t.insurance_file_cnt
                )
            WHEN 'Account Executive' THEN
                (
                    SELECT
                        ISNULL(MAX(P_AccExec.shortname), '')
                    FROM #transactions R3
                    JOIN account A
                        ON A.account_id = R3.client_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
                    WHERE R3.document_id = t.document_id
                    AND R3.share_number = 1
                    AND R3.line_number = 1
                )
            WHEN 'Business Type' THEN
                (
                    SELECT
                        ISNULL(MAX(BT.code), '')
                    FROM insurance_file IFI
                    LEFT JOIN business_type BT
                        ON BT.business_type_id = IFI.business_type_id
                    WHERE IFI.insurance_file_cnt = t.insurance_file_cnt
                )
        END
    ) group_code,
    (
        CASE @group_by
            WHEN 'Company' THEN
                (
                    SELECT
                        description
                    FROM company
                    WHERE company_id = t.Company_id
                )
            WHEN 'Account Handler' THEN
                (
                    SELECT
                        ISNULL(MAX(AccH.resolved_name), '')
                    FROM insurance_file IFI
                    LEFT JOIN Party AccH
                        ON AccH.party_cnt = IFI.account_handler_cnt
                    WHERE IFI.insurance_file_cnt = t.insurance_file_cnt
                )
            WHEN 'Account Executive' THEN
                (
                    SELECT
                        ISNULL(MAX(P_AccExec.resolved_name), '')
                    FROM #transactions R3
                    JOIN account A
                        ON A.account_id = R3.client_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
                    WHERE R3.document_id = t.document_id
                    AND R3.share_number = 1
                    AND R3.line_number = 1
                )
            WHEN 'Business Type' THEN
                (
                    SELECT
                        ISNULL(MAX(BT.description), '')
                    FROM insurance_file IFI
                    LEFT JOIN business_type BT
                        ON BT.business_type_id = IFI.business_type_id
                    WHERE IFI.insurance_file_cnt = t.insurance_file_cnt
                )
        END
    ) group_desc,
    (
        CASE @then_by
            WHEN 'None' THEN
                ''
            WHEN 'Client' THEN
                ISNULL(t.client_code,'')
            WHEN 'Document Reference'  then
		ISNULL(t.document_ref,'')
	    WHEN 'Cover Date' THEN
		ISNULL(CONVERT (varchar,t.cover_start_date,103 ),'')
	    WHEN 'Transaction Date' THEN
		ISNULL(CONVERT (varchar,t.accounting_date,103 ),'')
	    WHEN 'Account Executive' THEN
                (
                    SELECT
                        ISNULL(MAX(P_AccExec.shortname), '')
                    FROM #transactions R3
                    JOIN account A
                        ON A.account_id = R3.client_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
                    WHERE R3.document_id = t.document_id
                    AND R3.share_number = 1
                    AND R3.line_number = 1
                )
        END
    ) then_code,
    (
        CASE @then_by
            WHEN 'None' THEN
                ''
            WHEN 'Client' THEN
                ISNULL(t.client_name,'')
	    WHEN 'Document Reference'  then
		ISNULL(t.document_ref,'')
	    WHEN 'Cover Date' THEN
		ISNULL(CONVERT (varchar,t.cover_start_date,103 ),'')
	    WHEN 'Transaction Date' THEN
		ISNULL(CONVERT (varchar,t.accounting_date,103 ),'')
	    WHEN 'Account Executive' THEN
                (
                    SELECT
                        ISNULL(MAX(P_AccExec.resolved_name), '')
                    FROM #transactions R3
                    JOIN account A
                        ON A.account_id = R3.client_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
                    WHERE R3.document_id = t.document_id
                    AND R3.share_number = 1
                    AND R3.line_number = 1
                )

        END
    ) then_desc,
    @AgencyOrUnderwriting 'agencyorunderwriting',
    @NZConfig 'nz_config'
FROM #transactions t
ORDER BY
    company_id,
    document_ref,
    share_number,
    line_number


DROP TABLE #transactions
DROP TABLE #transaction_details
DROP TABLE #transaction_selection

GO


