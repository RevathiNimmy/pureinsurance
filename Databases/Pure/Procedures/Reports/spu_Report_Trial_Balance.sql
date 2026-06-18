SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Trial_Balance'
GO

CREATE PROCEDURE spu_Report_Trial_Balance
    @branch_id INT,
    @end_date DATETIME,
    @date_type VARCHAR(20)
AS

DECLARE
    @company_id INT,
    @default_bf_end_date DATETIME,
    @default_period_end_date DATETIME,
    @bf_end_date DATETIME,
    @period_start_date DATETIME,
    @period_end_date DATETIME,
    @period_company_id INT


CREATE TABLE #trial_balance
(
    group_header_code1 VARCHAR(50),
    group_header_name1 VARCHAR(50),
    group_header_code2 VARCHAR(50),
    group_header_name2 VARCHAR(50),
    group_header_code3 VARCHAR(50),
    group_header_name3 VARCHAR(50),
    group_header_code4 VARCHAR(50),
    group_header_name4 VARCHAR(50),
    group_header_code5 VARCHAR(50),
    group_header_name5 VARCHAR(50),
    group_header_code6 VARCHAR(50),
    group_header_name6 VARCHAR(50),
    group_header_code7 VARCHAR(50),
    group_header_name7 VARCHAR(50),
    group_header_code8 VARCHAR(50),
    group_header_name8 VARCHAR(50),
    group_header_code9 VARCHAR(50),
    group_header_name9 VARCHAR(50),
    account_code VARCHAR(30),
    bf_amount MONEY,
    amount MONEY,
    cf_amount MONEY    
)

/*Validate parameters*/
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @date_type IS NULL
BEGIN
    SELECT @date_type = 'Transaction Date'
END


/*Empty Table*/
TRUNCATE TABLE Report_TreePathNames


/*If multi accounting use node 0 (therefore all nodes)*/
IF EXISTS
    (
        SELECT 
            NULL 
        FROM hidden_options 
        WHERE option_number = 16
    )
BEGIN
    EXEC spu_Report_FullTreePathNames 0    
    SELECT @period_company_id = @branch_id
END
ELSE
BEGIN
    EXEC spu_Report_FullTreePathNames 1
    SELECT @period_company_id = 1
END


IF @date_type = 'Transaction Date'
BEGIN

    /*Calculate the dates to use*/
    SELECT @default_bf_end_date = DATEADD(DAY, -DATEPART(DAY, @end_date), @end_date)
    
    SELECT 
        @bf_end_date = ISNULL(MAX(p.period_end_date), @default_bf_end_date)
    FROM period p
    WHERE p.company_id = @period_company_id
    AND p.period_end_date < @end_date

    SELECT @period_start_date = DATEADD(second, 1, @bf_end_date)
    
    SELECT @period_end_date = @end_date


    /*Add all account details using transaction date of the transactions*/
    INSERT INTO #trial_balance 
    (
        group_header_code1,
        group_header_name1,
        group_header_code2,
        group_header_name2,
        group_header_code3,
        group_header_name3,
        group_header_code4,
        group_header_name4,
        group_header_code5,
        group_header_name5,
        group_header_code6,
        group_header_name6,
        group_header_code7,
        group_header_name7,
        group_header_code8,
        group_header_name8,
        group_header_code9,
        group_header_name9,
        account_code,
        bf_amount,
        amount,
        cf_amount
    )
    SELECT
        STR(ISNULL(rtpn.report_map_id2, 0), 5, 0) + ISNULL(rtpn.element_name2, ''),
        ISNULL(rtpn.element_name2, ''),
        STR(ISNULL(rtpn.report_map_id3, 0), 5, 0) + ISNULL(rtpn.element_name3, ''),
        ISNULL(rtpn.element_name3, ''),
        STR(ISNULL(rtpn.report_map_id4, 0), 5, 0) + ISNULL(rtpn.element_name4, ''),
        ISNULL(rtpn.element_name4, ''),
        STR(ISNULL(rtpn.report_map_id5, 0), 5, 0) + ISNULL(rtpn.element_name5, ''),
        ISNULL(rtpn.element_name5, ''),
        STR(ISNULL(rtpn.report_map_id6, 0), 5, 0) + ISNULL(rtpn.element_name6, ''),
        ISNULL(rtpn.element_name6, ''),
        STR(ISNULL(rtpn.report_map_id7, 0), 5, 0) + ISNULL(rtpn.element_name7, ''),
        ISNULL(rtpn.element_name7, ''),
        STR(ISNULL(rtpn.report_map_id8, 0), 5, 0) + ISNULL(rtpn.element_name8, ''),
        ISNULL(rtpn.element_name8, ''),
        STR(ISNULL(rtpn.report_map_id9, 0), 5, 0) + ISNULL(rtpn.element_name9, ''),
        ISNULL(rtpn.element_name9, ''),
        STR(ISNULL(rtpn.report_map_id10, 0), 5, 0) + ISNULL(rtpn.element_name10, ''),
        ISNULL(rtpn.element_name10, ''),
        a.short_code,
        (
            SELECT
                ISNULL(SUM(ROUND(td.amount, 2)), 0)
            FROM transdetail td 
            JOIN document d 
                ON d.document_id = td.document_id
            WHERE td.account_id = a.account_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND (
                    (
                        d.document_date <= @bf_end_date
                        AND
                        td.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                    )
                    OR
                    (
                        td.ref_date <= @bf_end_date
                        AND
                        td.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                    )
                )
        ),
        (
            SELECT
                ISNULL(SUM(ROUND(td.amount, 2)), 0)
            FROM transdetail td 
            JOIN document d 
                ON d.document_id = td.document_id
            WHERE td.account_id = a.account_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND (
                    (
                        d.document_date BETWEEN @period_start_date AND @period_end_date
                        AND
                        td.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                    )
                    OR
                    (
                        td.ref_date BETWEEN @period_start_date AND @period_end_date
                        AND
                        td.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                    )
                )
        ),
        (
            SELECT
                ISNULL(SUM(ROUND(td.amount, 2)), 0)
            FROM transdetail td 
            JOIN document d 
                ON d.document_id = td.document_id
            WHERE td.account_id = a.account_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND (
                    (
                        d.document_date <= @period_end_date
                        AND
                        td.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                    )
                    OR
                    (
                        td.ref_date <= @period_end_date
                        AND
                        td.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                    )
                )
        )
    FROM report_treepathnames rtpn 
    JOIN account a
        ON a.account_id = rtpn.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id
    JOIN ledgertype lt 
        ON lt.ledgertype_id = l.ledgertype_id
    JOIN accounttype at 
        ON at.accounttype_id = a.accounttype_id
        
END
ELSE
BEGIN

    /*Calculate the dates to use*/
    SELECT @default_bf_end_date = DATEADD(DAY, -DATEPART(DAY, @end_date), @end_date)
    SELECT @default_period_end_date = DATEADD(DAY, -DATEPART(DAY, DATEADD(MONTH, 1, @end_date)), DATEADD(MONTH, 1, @end_date))
    
    SELECT 
        @bf_end_date = ISNULL(MAX(p.period_end_date), @default_bf_end_date)
    FROM period p
    WHERE p.company_id = @period_company_id
    AND p.period_end_date < @end_date

    SELECT @period_start_date = DATEADD(second, 1, @bf_end_date)
    
    SELECT 
        @period_end_date = ISNULL(MIN(p.period_end_date), @default_period_end_date)
    FROM period p
    WHERE p.company_id = @period_company_id
    AND p.period_end_date >= @end_date

    /*Add all account details using transaction date of the transactions*/
    INSERT INTO #trial_balance 
    (
        group_header_code1,
        group_header_name1,
        group_header_code2,
        group_header_name2,
        group_header_code3,
        group_header_name3,
        group_header_code4,
        group_header_name4,
        group_header_code5,
        group_header_name5,
        group_header_code6,
        group_header_name6,
        group_header_code7,
        group_header_name7,
        group_header_code8,
        group_header_name8,
        group_header_code9,
        group_header_name9,
        account_code,
        bf_amount,
        amount,
        cf_amount
    )
    SELECT
        STR(ISNULL(rtpn.report_map_id2, 0), 5, 0) + ISNULL(rtpn.element_name2, ''),
        ISNULL(rtpn.element_name2, ''),
        STR(ISNULL(rtpn.report_map_id3, 0), 5, 0) + ISNULL(rtpn.element_name3, ''),
        ISNULL(rtpn.element_name3, ''),
        STR(ISNULL(rtpn.report_map_id4, 0), 5, 0) + ISNULL(rtpn.element_name4, ''),
        ISNULL(rtpn.element_name4, ''),
        STR(ISNULL(rtpn.report_map_id5, 0), 5, 0) + ISNULL(rtpn.element_name5, ''),
        ISNULL(rtpn.element_name5, ''),
        STR(ISNULL(rtpn.report_map_id6, 0), 5, 0) + ISNULL(rtpn.element_name6, ''),
        ISNULL(rtpn.element_name6, ''),
        STR(ISNULL(rtpn.report_map_id7, 0), 5, 0) + ISNULL(rtpn.element_name7, ''),
        ISNULL(rtpn.element_name7, ''),
        STR(ISNULL(rtpn.report_map_id8, 0), 5, 0) + ISNULL(rtpn.element_name8, ''),
        ISNULL(rtpn.element_name8, ''),
        STR(ISNULL(rtpn.report_map_id9, 0), 5, 0) + ISNULL(rtpn.element_name9, ''),
        ISNULL(rtpn.element_name9, ''),
        STR(ISNULL(rtpn.report_map_id10, 0), 5, 0) + ISNULL(rtpn.element_name10, ''),
        ISNULL(rtpn.element_name10, ''),
        a.short_code,
        (
            SELECT
                ISNULL(SUM(ROUND(td.amount, 2)), 0)
            FROM transdetail td 
            JOIN document d 
                ON d.document_id = td.document_id
            JOIN period p
                ON p.period_id = td.period_id
            WHERE td.account_id = a.account_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND p.period_end_date <= @bf_end_date
        ),
        (
            SELECT
                ISNULL(SUM(ROUND(td.amount, 2)), 0)
            FROM transdetail td 
            JOIN document d 
                ON d.document_id = td.document_id
            JOIN period p
                ON p.period_id = td.period_id
            WHERE td.account_id = a.account_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND p.period_end_date BETWEEN @period_start_date AND @period_end_date
        ),
        (
            SELECT
                ISNULL(SUM(ROUND(td.amount, 2)), 0)
            FROM transdetail td 
            JOIN document d 
                ON d.document_id = td.document_id
            JOIN period p
                ON p.period_id = td.period_id
            WHERE td.account_id = a.account_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND p.period_end_date <= @period_end_date
        )
    FROM report_treepathnames rtpn 
    JOIN account a
        ON a.account_id = rtpn.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id
    JOIN ledgertype lt 
        ON lt.ledgertype_id = l.ledgertype_id
    JOIN accounttype at 
        ON at.accounttype_id = a.accounttype_id

END


/*Delete any accounts that have no values on them*/
DELETE
FROM #trial_balance
WHERE bf_amount = 0
AND amount = 0
AND cf_amount = 0


/*Remove the account from the folder list*/
UPDATE #trial_balance
SET group_header_code1 = STR(0, 5, 0),
    group_header_name1 = ''
WHERE account_code = group_header_name1

UPDATE #trial_balance
SET group_header_code2 = STR(0, 5, 0),
    group_header_name2 = ''
WHERE account_code = group_header_name2

UPDATE #trial_balance
SET group_header_code3 = STR(0, 5, 0),
    group_header_name3 = ''
WHERE account_code = group_header_name3

UPDATE #trial_balance
SET group_header_code4 = STR(0, 5, 0),
    group_header_name4 = ''
WHERE account_code = group_header_name4

UPDATE #trial_balance
SET group_header_code5 = STR(0, 5, 0),
    group_header_name5 = ''
WHERE account_code = group_header_name5

UPDATE #trial_balance
SET group_header_code6 = STR(0, 5, 0),
    group_header_name6 = ''
WHERE account_code = group_header_name6

UPDATE #trial_balance
SET group_header_code7 = STR(0, 5, 0),
    group_header_name7 = ''
WHERE account_code = group_header_name7

UPDATE #trial_balance
SET group_header_code8 = STR(0, 5, 0),
    group_header_name8 = ''
WHERE account_code = group_header_name8

UPDATE #trial_balance
SET group_header_code9 = STR(0, 5, 0),
    group_header_name9 = ''
WHERE account_code = group_header_name9


/*Select all of the accounts*/
SELECT
    group_header_code1,
    group_header_name1,
    group_header_code2,
    group_header_name2,
    group_header_code3,
    group_header_name3,
    group_header_code4,
    group_header_name4,
    group_header_code5,
    group_header_name5,
    group_header_code6,
    group_header_name6,
    group_header_code7,
    group_header_name7,
    group_header_code8,
    group_header_name8,
    group_header_code9,
    group_header_name9,
    account_code,
    bf_amount,
    amount,
    cf_amount
FROM #trial_balance
ORDER BY
    group_header_code1,
    group_header_code2,
    group_header_code3,
    group_header_code4,
    group_header_code5,
    group_header_code6,
    group_header_code7,
    group_header_code8,
    group_header_code9,
    account_code

/*Empty Table*/
TRUNCATE TABLE Report_TreePathNames

DROP TABLE #trial_balance

GO
