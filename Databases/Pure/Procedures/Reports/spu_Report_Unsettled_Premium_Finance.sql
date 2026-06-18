SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_UnSettled_Premium_Finance'
GO

CREATE PROCEDURE spu_Report_UnSettled_Premium_Finance
    @End_date DATETIME,
    @branch_id INT

AS

DECLARE 
    @dEndDate DATETIME,
    @dStartDate DATETIME,
    @branch_name VARCHAR(255),
    @client_code VARCHAR(50),
    @plan_reference VARCHAR(50),
    @suspense_transdetail_id INT,
    @suspense_document_date DATETIME,
    @suspense_document_ref VARCHAR(50),
    @suspense_code VARCHAR(50),
    @orig_amount MONEY,
    @orig_commission MONEY,
    @provider_transdetail_id INT,
    @provider_code VARCHAR(50),
    @os_amount MONEY,
    @os_commission MONEY

SET NOCOUNT ON

SET @dEndDate = ISNULL(@End_date, GETDATE())

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #unsettled
(
    branch_name VARCHAR(255),
    client_code VARCHAR(50),
    plan_reference VARCHAR(50),
    suspense_transdetail_id INT,
    suspense_document_date DATETIME,
    suspense_document_ref VARCHAR(50),
    suspense_code VARCHAR(50),
    orig_amount MONEY,
    orig_commission MONEY,
    provider_transdetail_id INT,
    provider_code VARCHAR(50),
    os_amount MONEY,
    os_commission MONEY
)
CREATE INDEX I__#unsettled__suspense_transdetail_id ON #unsettled (suspense_transdetail_id)

CREATE TABLE #trans_on_plan
(
    suspense_transdetail_id INT,
    document_id INT
)
CREATE INDEX I__#trans_on_plan__suspense_transdetail_id ON #trans_on_plan (suspense_transdetail_id)

INSERT INTO #unsettled
SELECT
    c.description,
    '',
    td.insurance_ref,
    td.transdetail_id,
    d.document_date,
    d.document_ref,
    a.short_code,
    td_pf.amount * -1,
    0,
    td_pf.transdetail_id,
    a_pf.short_code,
    (   
        SELECT 
            (td_pf.amount - ISNULL(SUM(tm2.base_match_amount),0.0)) * -1
        FROM transdetail td2
        JOIN transmatch tm2 
            ON tm2.transdetail_id = td2.transdetail_id
            AND tm2.allocationdetail_id IS NOT NULL
        JOIN matchgroup mg2
            ON mg2.match_id = tm2.match_id
            AND mg2.match_date <= @dEndDate
        WHERE td2.transdetail_id = td_pf.transdetail_id
    ),
    0
FROM document d
JOIN company c
    ON c.company_id = d.company_id
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN account a 
    ON a.account_id = td.account_id
JOIN ledger l 
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'RF'
JOIN transdetail td_pf
    ON td_pf.document_id = d.document_id
    AND td_pf.transdetail_id <> td.transdetail_id
JOIN account a_pf
    ON a_pf.account_id = td_pf.account_id
JOIN ledger l_pf
    ON l_pf.ledger_id = a_pf.ledger_id
    AND l_pf.ledger_short_name = 'RF'
WHERE EXISTS
    (
        SELECT
            NULL
        FROM transdetail
        WHERE document_id = d.document_id
        HAVING SUM(1) = 2
    )
AND EXISTS 
    (
        SELECT 
            NULL 
        FROM party_finance_provider 
        WHERE party_cnt = a_pf.account_key 
    )
AND NOT EXISTS 
    (
        SELECT 
            NULL 
        FROM party_finance_provider 
        WHERE party_cnt = a.account_key 
    )
AND td_pf.amount <>
    (   
        SELECT 
            ISNULL(SUM(tm2.base_match_amount),0.0)
        FROM transdetail td2
        JOIN transmatch tm2 
            ON tm2.transdetail_id = td2.transdetail_id
            AND tm2.allocationdetail_id IS NOT NULL
        JOIN matchgroup mg2
            ON mg2.match_id = tm2.match_id
            AND mg2.match_date <= @dEndDate
        WHERE td2.transdetail_id = td_pf.transdetail_id
    )
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND d.document_date <= @dEndDate


INSERT INTO #trans_on_plan
SELECT
    us.suspense_transdetail_id,
    td4.document_id
FROM #unsettled us
JOIN transdetail td
    ON td.transdetail_id = us.suspense_transdetail_id
JOIN transmatch tm 
    ON tm.transdetail_id = td.transdetail_id
    AND tm.allocationdetail_id IS NOT NULL
JOIN transmatch tm2
    ON tm2.match_id = tm.match_id
    AND tm2.transmatch_id <> tm.transmatch_id
    AND tm2.allocationdetail_id IS NOT NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN transdetail td3
    ON td3.document_id = td2.document_id
    AND td3.transdetail_id <> td2.transdetail_id
JOIN transmatch tm3
    ON tm3.transdetail_id = td3.transdetail_id
    AND tm3.allocationdetail_id IS NOT NULL
JOIN transmatch tm4
    ON tm4.match_id = tm3.match_id
    AND tm4.transmatch_id <> tm3.transmatch_id
    AND tm4.allocationdetail_id IS NOT NULL
JOIN transdetail td4
    ON td4.transdetail_id = tm4.transdetail_id
GROUP BY 
    us.suspense_transdetail_id,
    td4.document_id

UPDATE us
SET us.orig_commission = 
        (
            SELECT
                ISNULL(SUM(td.amount),0)
            FROM #trans_on_plan tp
            JOIN transdetail td
                ON td.document_id = tp.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l 
                ON l.ledger_id = a.ledger_id 
                AND l.ledger_short_name = 'CO'
            WHERE tp.suspense_transdetail_id = us.suspense_transdetail_id
        ),
    us.client_code = 
        (
            SELECT
                MAX(ISNULL(a.short_code, ''))
            FROM #trans_on_plan tp
            JOIN transdetail td
                ON td.document_id = tp.document_id
            JOIN account a
                ON a.account_id = td.account_id
            WHERE tp.suspense_transdetail_id = us.suspense_transdetail_id
            AND td.document_sequence = 
                (
                    SELECT
                        MIN(document_sequence)
                    FROM transdetail td2
                    JOIN account a2
                        ON a2.account_id = td2.account_id
                    JOIN ledger l2 
                        ON l2.ledger_id = a2.ledger_id 
                        AND l2.ledger_short_name = 'SA'
                    WHERE td2.document_id = td.document_id
                )
        )
FROM #unsettled us
            
UPDATE #unsettled
SET os_commission = orig_commission * (os_amount / orig_amount)    

SELECT 
    *
FROM #unsettled

DROP TABLE #unsettled
DROP TABLE #trans_on_plan

GO