SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Account_Enquiry'
GO

CREATE PROCEDURE spu_Report_Account_Enquiry
    @short_code VARCHAR(30),
    @start_date DATETIME,
    @end_date DATETIME,
    @AllYN VARCHAR(30),
    @branch_id INT    
AS

DECLARE 
    @bf_balance MONEY,
    @cf_balance MONEY,
    @transdetail_id INT,
    @match_total MONEY,
    @ClientAccountId INT,
    @ClientAccountCode VARCHAR(30),
    @ClientAccountName VARCHAR(100)

CREATE TABLE #Report_Transaction
(
    transdetail_id INT,
    account_code VARCHAR(30),
    account_name VARCHAR(100),
    document_date DATETIME,
    document_ref VARCHAR(25),
    insurance_ref VARCHAR(30),
    media_ref VARCHAR(100), 
    amount MONEY,
    bf_balance MONEY,
    cf_balance MONEY,
    matched MONEY, 
    client_account_id INT,
    client_account_code VARCHAR(30),
    client_account_name VARCHAR(100),
    ledger_short_name VARCHAR(2),
    is_bank BIT,
    reconciled BIT
)

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

-- Create Individual Records
INSERT INTO #Report_Transaction
(
    transdetail_id,
    account_code,
    account_name,
    document_date,
    document_ref,
    insurance_ref,
    media_ref, 
    amount,
    bf_balance,
    cf_balance,
    matched, 
    client_account_id,
    client_account_code,
    client_account_name,
    ledger_short_name,
    is_bank,
    reconciled
)
SELECT 
    TD.transdetail_id,
    A.short_code,
    A.account_name,
    D.document_date,
    D.document_ref,
    ISNULL(TD.insurance_ref, ''),
    ISNULL(CLI.media_ref, TD.spare),
    TD.amount,
    0,
    0,
    0,
    0,
    '',
    '',
    L.ledger_short_name,
    CASE
        WHEN BA.bankaccount_id IS NOT NULL THEN 1
        ELSE 0
    END,
    CASE
        WHEN TD.spare = 'RECONCILED' THEN 1
        ELSE 0
    END
FROM Account A
JOIN Transdetail TD
    ON TD.account_id = A.account_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Ledger L 
    ON L.ledger_id = A.ledger_id
LEFT JOIN CashlistItem CLI
    ON CLI.transdetail_id = TD.transdetail_id
LEFT JOIN BankAccount BA
    ON BA.account_id = A.account_id
WHERE A.short_code = @short_code
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND 
( 
    ( 
        D.document_date BETWEEN @start_date AND @end_date
        AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
        AND TD.Document_Sequence NOT IN
            (
                SELECT Document_Sequence + 1
                FROM TransDetail
                WHERE document_id = D.document_id
                AND spare = 'COMM ADJ'
            )
    )
    OR
    ( 
        TD.ref_date BETWEEN @start_date AND @end_date
        AND 
        (
            ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
            OR TD.Document_Sequence IN
                (
                    SELECT Document_Sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
        )
    )
)

/*Get B/F Balance*/
SELECT 
    @bf_balance = ISNULL(SUM(TD.amount), 0)
FROM Account A
JOIN Transdetail TD
    ON TD.account_id = A.account_id
JOIN Document D
    ON D.document_id = TD.document_id
WHERE A.short_code = @short_code
AND
( 
    ( 
        D.document_date < @start_date
        AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
        AND TD.Document_Sequence NOT IN
            (
                SELECT Document_Sequence + 1
                FROM TransDetail
                WHERE document_id = D.document_id
                AND spare = 'COMM ADJ'
            )
    )
    OR
    ( 
        TD.ref_date < @start_date 
        AND 
        (
            ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
            OR TD.Document_Sequence IN
                (
                    SELECT Document_Sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
        )
    )
)

/*Get C/F Balance*/
SELECT 
    @cf_balance = ISNULL(SUM(TD.amount), 0)
FROM Account A
JOIN Transdetail TD
    ON TD.account_id = A.account_id
JOIN Document D
    ON D.document_id = TD.document_id
WHERE A.short_code = @short_code
AND
( 
    ( 
        D.document_date < @end_date
        AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
        AND TD.Document_Sequence NOT IN
            (
                SELECT Document_Sequence + 1
                FROM TransDetail
                WHERE document_id = D.document_id
                AND spare = 'COMM ADJ'
            )
    )
    OR
    ( 
        TD.ref_date < @end_date
        AND 
        (
            ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
            OR TD.Document_Sequence IN
                (
                    SELECT Document_Sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
        )
    )
)

/*Update table with BF and CF amounts.*/
UPDATE #Report_Transaction
SET bf_balance = @bf_balance,
    cf_balance = @cf_balance

/*Add Match totals*/
DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT DISTINCT rt.transdetail_id
    FROM #Report_Transaction rt

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO @transdetail_id

WHILE @@FETCH_STATUS = 0 
BEGIN

    /*Get Match Total*/
    SELECT 
        @match_total = ISNULL(SUM(TM.base_match_amount),0)
    FROM TransMatch TM
    JOIN MatchGroup MG
        ON MG.match_id = TM.match_id 
    WHERE TM.transdetail_id = @transdetail_id
    AND MG.match_date <= @end_date

    /*Update table with match amount*/
    UPDATE #Report_Transaction
    SET matched = @match_total
    WHERE transdetail_id = @transdetail_id

    
    SELECT @ClientAccountId = 0
    SELECT @ClientAccountCode = ''
    SELECT @ClientAccountName = ''

    IF  (   
            SELECT SUM(1)
            FROM transdetail TD1
            JOIN transdetail TD2
                ON TD2.document_id = TD1.document_id
            JOIN account A
                ON A.account_id = TD2.account_id 
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
            WHERE TD1.transdetail_id = @transdetail_id
            AND L.ledger_short_name = 'SA'
        ) > 0
    BEGIN

        /*Get the client account on the transction unless we are running the report for that account*/
        SELECT 
            @ClientAccountId = A2.account_id,
            @ClientAccountCode = A2.short_code,
            @ClientAccountName = A2.account_name
        FROM transdetail TD1
        JOIN transdetail TD2
            ON TD2.document_id = TD1.document_id
        JOIN account A2
            ON A2.account_id = TD2.account_id 
        WHERE TD1.Transdetail_id = @transdetail_id
        AND TD2.document_sequence =
            ( 
                SELECT MIN(document_sequence)
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id
                JOIN ledger L
                    ON L.ledger_id = A.ledger_id    
                WHERE TD.document_id = TD1.document_id
                AND A.account_id <> TD1.account_id            
                AND L.ledger_short_name = 'SA'
            )
        
    END
    ELSE
    BEGIN

        /*No client so use the first account that isn't the one we are running the report for*/
        SELECT 
            @ClientAccountId = A2.account_id,
            @ClientAccountCode = A2.short_code,
            @ClientAccountName = A2.account_name
        FROM transdetail TD1
        JOIN transdetail TD2
            ON TD2.document_id = TD1.document_id
        JOIN account A2
            ON A2.account_id = TD2.account_id 
        WHERE TD1.Transdetail_id = @transdetail_id
        AND TD2.document_sequence =
            ( 
                SELECT MIN(document_sequence)
                FROM transdetail TD
                WHERE TD.document_id = TD1.document_id
                AND TD.account_id <> TD1.account_id            
            )
    END


    UPDATE #Report_Transaction
    SET client_account_id = @ClientAccountId,
        client_account_code = @ClientAccountCode,
        client_account_name = @ClientAccountName
    WHERE transdetail_id = @transdetail_id
    AND ledger_short_name <> 'SA'

    FETCH NEXT FROM c_cursor INTO @transdetail_id
END

CLOSE c_cursor
DEALLOCATE c_cursor


IF LEFT(@AllYN, 1) = 'N' BEGIN
    /*Remove fully allocated/reconciled transactions*/
    DELETE FROM #Report_Transaction
    WHERE matched = amount
    OR (is_bank = 1 AND reconciled = 1)
END

-- Return Results
SELECT 
    account_code,
    account_name,
    document_date,
    document_ref,
    insurance_ref,
    media_ref,
    amount,
    bf_balance,
    cf_balance,
    matched,
    client_account_code,
    client_account_name
FROM #Report_Transaction

DROP TABLE #Report_Transaction

GO

