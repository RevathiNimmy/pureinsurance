SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_UnSettled_PF_In_Suspense'
GO

CREATE PROCEDURE spu_Report_UnSettled_PF_In_Suspense
    @end_date DATETIME,
    @branch_id INT

AS

DECLARE 
    @dEndDate DATETIME,
    @orig_document_id INT,
    @suspense_transdetail_id INT,
    @orig_commission MONEY

SET NOCOUNT ON

SELECT @dEndDate = ISNULL(@End_date, GETDATE())

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #in_suspense
(
    branch_name VARCHAR(255),
    suspense_transdetail_id INT,
    suspense_document_date DATETIME,
    suspense_document_ref VARCHAR(50),
    suspense_code VARCHAR(50),
    client_code VARCHAR(50),
    plan_reference VARCHAR(50),
    orig_amount MONEY,
    orig_commission MONEY
)

INSERT INTO #in_suspense
SELECT
    c.description,
    td.transdetail_id,
    d.document_date,
    d.document_ref,
    a.short_code,
    a_c.short_code,
    td.insurance_ref,
    td.amount,
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
JOIN transdetail td_c
    ON td_c.document_id = d.document_id
    AND td_c.transdetail_id <> td.transdetail_id
JOIN account a_c
    ON a_c.account_id = td_c.account_id
JOIN ledger l_c
    ON l_c.ledger_id = a_c.ledger_id
    AND l_c.ledger_short_name = 'SA'
WHERE EXISTS
    (
        SELECT
            NULL
        FROM transdetail
        WHERE document_id = d.document_id
        HAVING SUM(1) = 2
    )
AND NOT EXISTS 
    (
        SELECT 
            NULL 
        FROM party_finance_provider 
        WHERE party_cnt = a.account_key 
    )
    
AND td.amount <>
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
        WHERE td2.transdetail_id = td.transdetail_id
    )
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND d.document_date <= @dEndDate



DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT
        suspense_transdetail_id
    FROM #in_suspense

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO   
    @suspense_transdetail_id
    
    
WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT
        @orig_document_id = NULL,
        @orig_commission = 0

    SELECT 
        @orig_document_id = ISNULL(td3.document_id, 0)
    FROM transdetail td
    JOIN transdetail td2
        ON td2.document_id = td.document_id
        AND td2.transdetail_id <> td.transdetail_id
    JOIN transmatch tm2
        ON tm2.transdetail_id = td2.transdetail_id
        AND tm2.allocationdetail_id IS NOT NULL
    JOIN transmatch tm3
        ON tm3.match_id = tm2.match_id
        AND tm3.transmatch_id <> tm2.transmatch_id
        AND tm3.allocationdetail_id IS NOT NULL
    JOIN transdetail td3
        ON td3.transdetail_id = tm3.transdetail_id
    WHERE td.transdetail_id = @suspense_transdetail_id
    
    SELECT 
        @orig_commission = ISNULL(SUM(td.amount),0)
    FROM transdetail td
    JOIN account a
        ON a.account_id = td.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id 
        AND l.ledger_short_name = 'CO'
    WHERE td.document_id = @orig_document_id
    
    UPDATE #in_suspense
    SET orig_commission = @orig_commission
    WHERE suspense_transdetail_id = @suspense_transdetail_id
    
    
    FETCH NEXT FROM c_cursor INTO
        @suspense_transdetail_id
END 

CLOSE c_cursor
DEALLOCATE c_cursor

SET NOCOUNT OFF

SELECT  
    *
FROM #in_suspense

DROP TABLE #in_suspense

GO

