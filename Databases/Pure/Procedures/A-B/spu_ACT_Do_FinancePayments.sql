SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_FinancePayments'
GO

CREATE PROCEDURE spu_ACT_Do_FinancePayments
    @account_id INT,
    @date_to DATETIME = NULL,
    @date_by_trans BIT = 0,
    @marked_status INT = NULL,
    @month INT = NULL
AS

DECLARE @settlement_period SMALLINT
DECLARE @amt_settled MONEY
DECLARE @transdetail_id INT
DECLARE @document_id INT
DECLARE @currency_amount MONEY
DECLARE @commadj_amount MONEY
DECLARE @document_id_copy INT
DECLARE @ICCS CHAR(4)

EXEC spu_pm_iccs @ICCS OUTPUT

SELECT 
    @settlement_period = settlement_period
FROM Account
WHERE account_id = @account_id

CREATE TABLE #InsurerTemp
(
    account_name CHAR(255),
    insurer_ref VARCHAR(30),
    document_ref VARCHAR(25),
    gross_transdetail_id INT,
    gross_amount MONEY,
    primary_transdetail_id INT,
    primary_amount MONEY,
    adj_transdetail_id INT,
    adj_amount MONEY,
    fee_transdetail_id INT,
    fee_amount MONEY,
    amt_settled MONEY,
    document_id INT,
    accounting_date DATETIME,
    currency_id SMALLINT,
    marked_status TINYINT,
    month SMALLINT,
    spare VARCHAR(50),
    payment MONEY,
    source_id INT,
    short_code CHAR(20),
    client_transdetail_id INT,
    client_amount MONEY,
    client_settled MONEY,
    period VARCHAR(15)
)

INSERT INTO #InsurerTemp
SELECT 
    pf.ClientName,
    t.insurance_ref,
    d.document_ref,
    t.transdetail_id,
    t.currency_amount,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    d.document_id,
    t.accounting_date,
    t.currency_id,
    0,
    DATEPART(mm, DATEADD(dd, @settlement_period, t.accounting_date)),
    t.spare,
    0,
    t.company_id,
    pf.clientcode,
    0,
    0,
    0,
    p.period_name
FROM account a
JOIN transdetail t 
    ON t.account_id = a.account_id
JOIN document d 
    ON d.document_id = t.document_id
JOIN period p 
    ON p.period_id = t.period_id
LEFT JOIN pfpremiumfinance pf 
    ON pf.plantransaction_id = t.transdetail_id
WHERE a.account_id = @account_id
AND (
        (
            t.accounting_date <= @date_to 
            AND 
            @date_by_trans = 0
        )
        OR
        (
            d.created_date <= @date_to  
            AND @date_by_trans = 1
        )
        OR
        @date_to IS NULL
    )

    UPDATE i
    SET account_name = a.account_name,
        short_code = a.short_code
    FROM #InsurerTemp i
    JOIN transdetail td
        ON td.document_id = i.document_id
    JOIN account a
        ON a.account_id = td.account_id
    WHERE i.short_code IS NULL
    AND td.document_sequence = 
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = i.document_id
            AND account_id <> @account_id    
        )

CREATE TABLE #InsAmtTemp
(
    transdetail_id INT,
    amt_settled MONEY
)

INSERT INTO #InsAmtTemp
SELECT 
    a.transdetail_id,
    SUM(a.alloc_base_amount)
FROM #InsurerTemp p
JOIN transdetail t
    ON t.transdetail_id = p.gross_transdetail_id 
JOIN allocationdetail a
    ON a.transdetail_id = t.transdetail_id  
GROUP BY 
    a.transdetail_id
ORDER BY 
    a.transdetail_id             

UPDATE #InsurerTemp
SET amt_settled = t.amt_settled
FROM #InsurerTemp p 
JOIN #InsAmtTemp t
    ON t.transdetail_id = p.gross_transdetail_id

DROP TABLE #InsAmtTemp

DELETE 
FROM #InsurerTemp
WHERE amt_settled = gross_amount


/* Copy in the Amount owing on the Clients account */
UPDATE it
SET client_amount = t.currency_amount
FROM #insurertemp it
JOIN transmatch m
    ON m.transdetail_id = it.client_transdetail_id
JOIN transmatch m2
    ON m2.match_id = m.match_id
    AND m2.transdetail_id <> m.transdetail_id
    AND m2.transdetail_id <> it.gross_transdetail_id
JOIN transdetail t
    ON t.transdetail_id = m2.transdetail_id

/* Update the Policy Number For ITs4Me*/
UPDATE it
SET insurer_ref = t.insurance_ref
FROM #insurertemp it
JOIN transmatch m
    ON m.transdetail_id = it.client_transdetail_id
JOIN transmatch m2
    ON m2.match_id = m.match_id
    AND m2.transdetail_id <> m.transdetail_id
    AND m2.transdetail_id <> it.gross_transdetail_id
JOIN transdetail t
    ON t.transdetail_id = m2.transdetail_id
WHERE @ICCS = '5570'

IF @month IS NOT NULL 
BEGIN
    DELETE 
    FROM #InsurerTemp
    WHERE MONTH <> @month
END

UPDATE #InsurerTemp
SET marked_status = 1
WHERE gross_transdetail_id IN 
    (
        SELECT 
            transdetail_id
        FROM transmatch
        WHERE allocationdetail_id IS NULL
    )

IF @marked_status IS NOT NULL 
BEGIN
    DELETE 
    FROM #InsurerTemp
    WHERE marked_status = (1 - (@marked_status))
END

UPDATE #InsurerTemp
SET amt_settled = 0
WHERE amt_settled IS NULL

UPDATE  #InsurerTemp   
SET payment = tm.currency_match_amount  
FROM    transmatch tm  
WHERE   gross_transdetail_id = tm.transdetail_id  
AND tm.allocationdetail_id IS null

SELECT 
    *
FROM #InsurerTemp
ORDER BY 
    source_id, 
    insurer_ref, 
    document_ref

DROP TABLE #InsurerTemp


GO
