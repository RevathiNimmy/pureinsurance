SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Claims'
GO

CREATE PROCEDURE spu_Report_Audit_Claims
   @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME

AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Transactions
(
    document_id INT,
    document_ref VARCHAR(25),
    document_date DATETIME,
    description VARCHAR(255),
    cover_date DATETIME,
    documenttype integer,
    
    db_transdetail_id INT,
    db_acc_code VARCHAR(30),
    db_acc_name VARCHAR(100),
    db_comment VARCHAR(540),
    db_amount MONEY,
    db_ledger_id integer,
    
    cr_transdetail_id INT,
    cr_acc_code VARCHAR(30),
    cr_acc_name VARCHAR(100),
    cr_comment VARCHAR(540),
    cr_amount MONEY,
    cr_ledger_id integer,
    
    line_order INT
)


/*Get all relevant transactions where there are pairs of credits and debits*/
INSERT INTO #Transactions
SELECT 
    D.document_id,
    D.document_ref,
    D.document_date,
    DT.description,
    TD1.ref_date,
    D.documenttype_id,
    
    TD1.transdetail_id,
    A1.short_code,
    A1.account_name,
    TD1.comment,
    TD1.amount,
    A1.ledger_id,
    
    TD2.transdetail_id,
    A2.short_code,
    A2.account_name,
    TD2.comment,
    TD2.amount,
    A2.ledger_id,
    
    1
    
FROM Document D
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id

JOIN TransDetail TD1
    ON TD1.document_id = D.document_id
JOIN Account A1
    ON A1.account_id = TD1.account_id

JOIN TransDetail TD2
    ON TD2.document_id = D.document_id
JOIN Account A2
    ON A2.account_id = TD2.account_id

WHERE D.documenttype_id IN (28, 29, 40, 41) /*CLP, CLR, CLO, CLA*/
AND D.document_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND TD1.amount >= 0
AND TD2.amount < 0
AND TD1.amount + TD2.amount = 0
AND
(
    (
        TD1.document_sequence % 2 = 1
        AND 
        TD2.document_sequence = TD1.document_sequence + 1
    )
    OR
    (
        TD1.document_sequence % 2 = 0
        AND 
        TD2.document_sequence = TD1.document_sequence - 1
    )
)


/*Get credits from all relevant transactions where there isn't a matching debit*/
INSERT INTO #Transactions
SELECT 
    D.document_id,
    D.document_ref,
    D.document_date,
    DT.description,
    TD2.ref_date,
    D.documenttype_id,
    
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    
    TD2.transdetail_id,
    A2.short_code,
    A2.account_name,
    TD2.comment,
    TD2.amount,
    A2.ledger_id,
    
    2
    
FROM Document D
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id

JOIN TransDetail TD2
    ON TD2.document_id = D.document_id
JOIN Account A2
    ON A2.account_id = TD2.account_id
    
WHERE D.documenttype_id IN (28, 29, 40, 41) /*CLP, CLR, CLO, CLA*/
AND D.document_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND TD2.amount < 0
AND NOT EXISTS
    (
        SELECT
            NULL
        FROM #Transactions
        WHERE cr_transdetail_id = TD2.transdetail_id
    )

/*Get debits from all relevant transactions where there isn't a matching credit*/
INSERT INTO #Transactions
SELECT 
    D.document_id,
    D.document_ref,
    D.document_date,
    DT.description,
    TD1.ref_date,
    D.documenttype_id,
    
    TD1.transdetail_id,
    A1.short_code,
    A1.account_name,
    TD1.comment,
    TD1.amount,
    A1.ledger_id,
    
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,

    3
    
FROM Document D
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id

JOIN TransDetail TD1
    ON TD1.document_id = D.document_id
JOIN Account A1
    ON A1.account_id = TD1.account_id
    
WHERE D.documenttype_id IN (28, 29, 40, 41) /*CLP, CLR, CLO, CLA*/
AND D.document_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND TD1.amount >= 0
AND NOT EXISTS
    (
        SELECT
            NULL
        FROM #Transactions
        WHERE db_transdetail_id = TD1.transdetail_id
    )
    
UPDATE #transactions
SET db_amount = (db_amount * -1),
cr_amount = (cr_amount * -1)
WHERE 
(cr_ledger_id <> 2 AND 
db_ledger_id = 2 AND 
documenttype = 28 )
or
(cr_ledger_id = 2 AND 
db_ledger_id <> 2 AND 
documenttype = 29 ) 

--PN24754 Ensure that Claim Receipts have amount signs reversed 
UPDATE #transactions
SET    db_amount = (db_amount * -1),
       cr_amount = (cr_amount * -1)
WHERE  db_amount > 0 
AND    documenttype = 29 

/*Select all of the lines for the report*/
SELECT 
    *
FROM #Transactions
ORDER BY 
    document_id,
    line_order,
    cr_acc_code,
    db_acc_code

DROP TABLE #Transactions

GO

