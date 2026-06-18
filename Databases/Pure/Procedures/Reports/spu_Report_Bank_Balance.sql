SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Bank_Balance'
GO

CREATE PROCEDURE spu_Report_Bank_Balance
    @branch_id INT,
    @bank_name VARCHAR(255),
    @end_date DATETIME
AS

SET NOCOUNT ON

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @bank_name = 'ALL'
BEGIN
    SELECT @bank_name = NULL
END

CREATE TABLE #bank_transactions
(
    branch_id INT,
    branch_name VARCHAR(255),
    bank_id INT,
    bank_name VARCHAR(255),
    amount MONEY
)

CREATE TABLE #bank_trans_balance
(
    branch_id INT,
    branch_name VARCHAR(255),
    bank_id INT,
    bank_name VARCHAR(255),
    amount MONEY,
    fsa_disabled BIT
)

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    
    INSERT INTO #bank_trans_balance
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT 
        *
    FROM #bank_trans_balance
    
    DROP TABLE #bank_transactions
    DROP TABLE #bank_trans_balance
        
    RETURN  
END

INSERT INTO #bank_transactions
SELECT  
    S.source_id,
    S.description,
    B.bank_id,
    B.bank_name,
    TD.amount
FROM Transdetail TD
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Account A
    ON TD.account_id = A.account_id
JOIN BankAccount BA 
    ON BA.account_id = TD.account_id
JOIN Bank B 
    ON B.bank_id = BA.bank_id
JOIN Source S
    ON S.source_id = TD.company_id
WHERE B.bank_name = ISNULL(@bank_name, B.bank_name)
AND TD.company_id = ISNULL(@branch_id, TD.company_id)
AND D.document_date <= @end_date

INSERT INTO #bank_trans_balance
SELECT 
    MIN(branch_id), 
    MIN(branch_name), 
    MIN(bank_id), 
    MIN(bank_name), 
    SUM(amount),
    NULL
FROM #bank_transactions 
WHERE amount < 0 
GROUP BY 
    branch_id, 
    bank_name
    
INSERT INTO #bank_trans_balance
SELECT 
    MIN(branch_id), 
    MIN(branch_name), 
    MIN(bank_id), 
    MIN(bank_name), 
    SUM(amount),
    NULL
FROM #bank_transactions 
WHERE amount > 0 
GROUP BY 
    branch_id, 
    bank_name

SET NOCOUNT OFF

SELECT 
    * 
FROM #bank_trans_balance 
ORDER BY 
    branch_id, 
    bank_name

DROP TABLE #bank_transactions
DROP TABLE #bank_trans_balance

GO


