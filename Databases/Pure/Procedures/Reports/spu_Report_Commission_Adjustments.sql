SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Commission_Adjustments'
GO

CREATE PROCEDURE spu_Report_Commission_Adjustments
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #transactions
(

    /*ID fields*/
    document_id INT,
    account_id INT,
    
    /*Columns on the report*/
    document_ref VARCHAR(30),
    document_date DATETIME,
    effective_date DATETIME,
    client_name VARCHAR(255),
    gross_comm_adj MONEY,
    agent_comm_adj MONEY,
    
    /*Group fields*/
    commission_code VARCHAR(20),
    commission_name VARCHAR(60),
    
    /*Miscellaneous fields*/
    agencyorunderwriting VARCHAR(20)
    
)
CREATE INDEX I_#transactions_document_id_line_no ON #transactions (document_id, account_id)

/*Select all documents that have had adjustments applied to them within the date range*/
INSERT INTO #transactions
(
    document_id,
    account_id,
    document_date
)
SELECT
    d.document_id,
    a.account_id,
    td.ref_date
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.spare = 'BROK ADJ'
    AND td.ref_date BETWEEN @start_date AND @end_date
JOIN account a
    ON a.account_id = td.account_id
WHERE d.company_id = ISNULL(@branch_id, d.company_id)
AND d.documenttype_id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
GROUP BY
    d.document_id,
    a.account_id,
    td.ref_date
    
/*Update each line with all the details*/
UPDATE t
SET document_ref = d.document_ref,
    effective_date =
        (
            SELECT
                ref_date
            FROM transdetail
            WHERE document_id = t.document_id
            AND document_sequence = 1
        ),
    client_name =
        (
            SELECT  
                ISNULL(p.resolved_name, '')
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN party p
                ON p.party_cnt = a.account_key
            WHERE td.document_id = t.document_id
            AND td.document_sequence = 1
        ),
    gross_comm_adj =
        (
            SELECT 
                ISNULL(SUM(td.amount),0) 
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.ref_date = t.document_date
            AND td.spare = 'COMM ADJ'
        ),
    agent_comm_adj =
        (
            SELECT 
                ISNULL(SUM(td.amount),0) 
            FROM transdetail td
            WHERE td.document_id = t.document_id
            AND td.ref_date = t.document_date
            AND td.spare = 'AGENT ADJ'
        ),

    commission_code = a.short_code,
    commission_name = a.account_name,
    
    agencyorunderwriting = 
        (
            SELECT 
                value 
            FROM hidden_options 
            WHERE option_number = 1
        ) 
FROM #transactions t
JOIN document d
    ON d.document_id = t.document_id
JOIN account a
    ON a.account_id = t.account_id


/*Return all of the transactions in the appropriate order*/
SELECT
    *
FROM #transactions
ORDER BY
    commission_code,
    document_ref,
    document_date

GO
