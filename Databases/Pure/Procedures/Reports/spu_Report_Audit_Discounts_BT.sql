SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_report_audit_discounts_BT'
GO

CREATE PROCEDURE spu_report_audit_discounts_BT
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(11)
AS

DECLARE @transdetail_id INT
DECLARE @Client_premium MONEY
DECLARE @Client_code VARCHAR(30)
DECLARE @Client_name VARCHAR(255)

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #transactions
(
    /*Transaction Details*/
    document_ref VARCHAR(25),
    document_type VARCHAR(100),
    transaction_date DATETIME,
    effective_date DATETIME,
    is_reversed BIT,
    
    /*Client Details*/
    client_code VARCHAR(30),
    client_name VARCHAR(255),
    
    /*Discount Details*/
    discount_code VARCHAR(20),
    discount_name VARCHAR(60),
    discount_amount MONEY,
    
    /*Other Details*/
    branch_code VARCHAR(20),
    branch_name VARCHAR(255),
    business_type_code VARCHAR(20),
    business_type_desc VARCHAR(255)
)


INSERT INTO #transactions
SELECT 
    /*Transaction Details*/
    d.document_ref,
    dt.description,
    d.document_date,
    td.ref_date,
    CASE
        WHEN td.spare LIKE 'Revers%' THEN 1
        ELSE 0
    END,
    
    /*Client Details*/
    (
        SELECT 
            ISNULL(MAX(px.shortname),'N/A')
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
        JOIN party px
            ON px.party_cnt = ax.account_key
        WHERE tdx.document_id = d.document_id
        AND lx.ledger_short_name = 'SA'
        AND tdx.document_sequence = 
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail tdx
                JOIN account ax
                    ON ax.account_id = tdx.account_id
                JOIN ledger lx
                    ON lx.ledger_id = ax.ledger_id
                WHERE tdx.document_id = d.document_id
                AND lx.ledger_short_name = 'SA'
            )
    ),
    (
        SELECT 
            ISNULL(MAX(px.resolved_name),'Not Applicable')
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
        JOIN party px
            ON px.party_cnt = ax.account_key
        WHERE tdx.document_id = d.document_id
        AND lx.ledger_short_name = 'SA'
        AND tdx.document_sequence = 
            (
                SELECT
                    MIN(document_sequence)
                FROM transdetail tdx
                JOIN account ax
                    ON ax.account_id = tdx.account_id
                JOIN ledger lx
                    ON lx.ledger_id = ax.ledger_id
                WHERE tdx.document_id = d.document_id
                AND lx.ledger_short_name = 'SA'
            )
    ),
            
    /*Discount Details*/
    (
        SELECT 
            px.shortname
        FROM party px
        WHERE px.party_cnt = a.account_key
    ),
    (
        SELECT 
            px.resolved_name
        FROM party px
        WHERE px.party_cnt = a.account_key
    ),            
    td.amount,
                
    /*Other Details*/
    s.code,
    s.description,
    ISNULL (bt.code, 'N/A')    ,
    ISNULL (bt.description, 'Not Applicable')    
    
FROM transdetail td
JOIN document d
    ON d.document_id = td.document_id
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
JOIN source s
    ON s.source_id = td.company_id
LEFT JOIN insurance_file i
        JOIN business_type bt
            ON bt.business_type_id = i.business_type_id
    ON i.insurance_file_cnt = d.insurance_file_cnt
WHERE L.ledger_short_name = 'DI'
AND (
        (
            @date_type = 'Transaction'
            AND
            D.document_date BETWEEN @start_date AND @end_date
        )
        OR
        (
            @date_type = 'Effective'
            AND
            TD.ref_date BETWEEN @start_date AND @end_date
        )
    )
AND s.source_id=ISNULL(@branch_id,td.company_id)

/*Select all of the records for the report*/
SELECT 
    *
FROM #transactions

GO

