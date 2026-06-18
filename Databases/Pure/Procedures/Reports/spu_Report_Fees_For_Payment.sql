--*****************************************************************
/*Change    Date        PN  */
/*Jitendra  09-09-2005  23464           */
--****************************************************************
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Fees_For_Payment'
GO

CREATE PROCEDURE spu_Report_Fees_For_Payment
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT
    0,
    td.document_id,
    a.short_code,
    a.account_name,
    d.document_date,
    d.document_ref,
    td.amount,
    (
        SELECT 
            p.resolved_name
        FROM transdetail td 
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id    
            AND l.ledger_short_name = 'SA'
        JOIN party p
            ON p.party_cnt = a.account_key
        WHERE td.document_id = d.document_id
        AND td.document_sequence = 1
    ) 'client_name'
FROM transdetail td
JOIN account a
    ON td.account_id = a.account_id
JOIN ledger l
    ON a.ledger_id = l.ledger_id
JOIN document d
    ON td.document_id = d.document_id
WHERE l.ledger_short_name = 'FE'
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND NOT EXISTS /*The transaction hasn't got a sub agent*/
    (   
        SELECT NULL
        FROM transdetail td_ub
        JOIN account a_ub
            ON a_ub.account_id = td_ub.account_id 
        JOIN ledger l_ub
            ON l_ub.ledger_id = a_ub.ledger_id 
        WHERE l_ub.ledger_short_name = 'UB'
        AND td_ub.document_id = d.document_id
    )
AND EXISTS
    (   /*The client has been allocated since the start date*/
        SELECT mg_sa.match_date
        FROM matchgroup mg_sa
        JOIN transmatch tm_sa
            ON tm_sa.match_id = mg_sa.match_id
        JOIN transdetail td_sa
            ON td_sa.transdetail_id = tm_sa.transdetail_id
        JOIN account a_sa
            ON a_sa.account_id = td_sa.account_id 
        JOIN ledger l_sa
            ON l_sa.ledger_id = a_sa.ledger_id 
        WHERE l_sa.ledger_short_name = 'SA'
        AND td_sa.document_id = d.document_id
        AND tm_sa.allocationdetail_id IS NOT NULL
        AND mg_sa.match_date >= @start_date
    )
AND (   /*The client has paid in full before the end date*/
        (
            SELECT SUM(ROUND(tm_sa.base_match_amount,2))
            FROM matchgroup mg_sa
            JOIN transmatch tm_sa
                ON tm_sa.match_id = mg_sa.match_id
            JOIN transdetail td_sa
                ON td_sa.transdetail_id = tm_sa.transdetail_id
            JOIN account a_sa
                ON a_sa.account_id = td_sa.account_id 
            JOIN ledger l_sa
                ON l_sa.ledger_id = a_sa.ledger_id 
            WHERE l_sa.ledger_short_name = 'SA'
            AND td_sa.document_id = d.document_id
            AND tm_sa.allocationdetail_id IS NOT NULL
            AND mg_sa.match_date <= @end_date
        )
        =
        (
            SELECT SUM(ROUND(td_sa.amount,2))
            FROM transdetail td_sa
            JOIN account a_sa
                ON a_sa.account_id = td_sa.account_id 
            JOIN ledger l_sa
                ON l_sa.ledger_id = a_sa.ledger_id 
            WHERE l_sa.ledger_short_name = 'SA'
            AND td_sa.document_id = d.document_id
        )       
    )
UNION
SELECT
    0,
    td.document_id,
    a.short_code,
    a.account_name,
    d.document_date,
    d.document_ref,
    td.amount,
    (
        SELECT 
            p.resolved_name
        FROM transdetail td 
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id    
            AND l.ledger_short_name = 'SA'
        JOIN party p
            ON p.party_cnt = a.account_key
        WHERE td.document_id = d.document_id
        AND td.document_sequence = 1
    ) 'client_name'
FROM transdetail td
JOIN account a
    ON td.account_id = a.account_id
JOIN ledger l
    ON a.ledger_id = l.ledger_id
JOIN document d
    ON td.document_id = d.document_id
WHERE l.ledger_short_name = 'FE'
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND EXISTS /*The transaction has got a sub agent*/
    (   
        SELECT NULL
        FROM transdetail td_ub
        JOIN account a_ub
            ON a_ub.account_id = td_ub.account_id 
        JOIN ledger l_ub
            ON l_ub.ledger_id = a_ub.ledger_id 
        WHERE l_ub.ledger_short_name = 'UB'
        AND td_ub.document_id = d.document_id
    )
AND EXISTS
    (   /*The sub agent has been allocated since the start date*/
        SELECT mg_ub.match_date
        FROM matchgroup mg_ub
        JOIN transmatch tm_ub
            ON tm_ub.match_id = mg_ub.match_id
        JOIN transdetail td_ub
            ON td_ub.transdetail_id = tm_ub.transdetail_id
        JOIN account a_ub
            ON a_ub.account_id = td_ub.account_id 
        JOIN ledger l_ub
            ON l_ub.ledger_id = a_ub.ledger_id 
        WHERE l_ub.ledger_short_name = 'UB'
        AND td_ub.document_id = d.document_id
        AND tm_ub.allocationdetail_id IS NOT NULL
        AND mg_ub.match_date >= @start_date
    )
AND (   /*The sub agent has paid in full before the end date*/
        (
            SELECT SUM(ROUND(tm_ub.base_match_amount,2))
            FROM matchgroup mg_ub
            JOIN transmatch tm_ub
                ON tm_ub.match_id = mg_ub.match_id
            JOIN transdetail td_ub
                ON td_ub.transdetail_id = tm_ub.transdetail_id
            JOIN account a_ub
                ON a_ub.account_id = td_ub.account_id 
            JOIN ledger l_ub
                ON l_ub.ledger_id = a_ub.ledger_id 
            WHERE l_ub.ledger_short_name = 'UB'
            AND td_ub.document_id = d.document_id
            AND tm_ub.allocationdetail_id IS NOT NULL
            AND mg_ub.match_date <= @end_date
        )
        =
        (
            SELECT SUM(ROUND(td_ub.amount,2))
            FROM transdetail td_ub
            JOIN account a_ub
                ON a_ub.account_id = td_ub.account_id 
            JOIN ledger l_ub
                ON l_ub.ledger_id = a_ub.ledger_id 
            WHERE l_ub.ledger_short_name = 'UB'
            AND td_ub.document_id = d.document_id
        )       
    )   


GO
