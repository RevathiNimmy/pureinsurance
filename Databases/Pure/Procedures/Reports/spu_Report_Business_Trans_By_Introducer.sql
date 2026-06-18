SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Business_Trans_By_Introducer'
GO

CREATE PROCEDURE spu_Report_Business_Trans_By_Introducer
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @agent_code VARCHAR(20)
AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @agent_code = 'ALL' OR @agent_code = ''
BEGIN
    SELECT @agent_code = NULL
END


SELECT
    d.document_date 'Transaction_Date',
    d.document_ref 'Document_Ref',
    dt.description 'Document_Type',
    dt.code 'Trans_Code',
    (
        SELECT
            ISNULL(MIN(a.account_name), '')
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE td.document_id = d2.document_id
        AND l.ledger_short_name = 'SA'
    ) 'Client',
    td.insurance_ref 'Policy_No',
    a.short_code 'SubAgent',
    (
        SELECT
            CASE WHEN SUBSTRING(td.spare,1,5)='Revsd' THEN
               ISNULL(SUM(tdx.amount), 0)
            ELSE
               ISNULL(SUM(tdx.amount), 0)* -1
            END
        
        FROM transdetail tdx
        WHERE tdx.document_id = d2.document_id
        AND tdx.document_sequence =
            (
                SELECT
                    MIN(td2.document_sequence)
                FROM transdetail td2
                JOIN account a2
                    ON a2.account_id = td2.account_id
                JOIN ledger l2
                    ON l2.ledger_id = a2.ledger_id
                WHERE td2.document_id = d2.document_id
                AND l2.ledger_short_name = 'SA'
            )
    ) 'Gross_Premium',
    (
        ( /*Amount posted to potential comm*/
            SELECT
                (ISNULL(SUM(tdx.amount), 0) - td.amount) 
            FROM transdetail tdx
            JOIN account ax
                ON ax.account_id = tdx.account_id
            JOIN ledger lx
                ON lx.ledger_id = ax.ledger_id
            WHERE tdx.document_id = d2.document_id
            AND lx.ledger_short_name = 'CO'
        )
    ) 'Net_Commission',
    td.amount * -1 'Agent_Commission',
    td.amount * -1  'This_Agent_Commission',
    0 'This_Subagent_Commission',
    (
        SELECT
            ISNULL(MAX(tc.description), '')
        FROM transaction_comment tc
        WHERE tc.transdetail_id = td.transdetail_id
        AND tc.transaction_comment_id =
            (
                SELECT
                    MAX(tc2.transaction_comment_id)
                FROM transaction_comment tc2
                WHERE tc2.transdetail_id = tc.transdetail_id
                AND tc2.comment_date =
                    (
                        SELECT
                            MAX(tc3.comment_date)
                        FROM transaction_comment tc3
                        WHERE tc3.transdetail_id = tc2.transdetail_id
                    )
            )
    ) 'Latest_Transaction_Comment',
    D2.document_ref 'Orig_Document_Ref',
    (
        CASE
            WHEN LEFT(td.spare, 8) = 'INT ADJ' THEN 'A'
            ELSE ''
        END
    ) 'Adjustment'
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN document d2
    ON d2.document_ref = SUBSTRING(TD.spare, 10, 11)
    AND d2.company_id = td.company_id
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
WHERE l.ledger_short_name = 'TR'
AND d.document_date BETWEEN @start_date AND @end_date
AND d.documenttype_id = 1
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND a.short_code = ISNULL(@agent_code, a.short_code)
AND NOT EXISTS
    (
        SELECT
            NULL
        FROM transdetail td
        WHERE td.document_id = d.document_id
        AND ISNULL(td.not_reported,0) = 1
    )



GO