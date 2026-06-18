SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Money_Cal_Unpaid_Direct_Business'
GO

CREATE PROCEDURE spu_Report_Client_Money_Cal_Unpaid_Direct_Business
    @branch_id INT,
    @end_date DATETIME,
    @direct_comm_os MONEY OUTPUT

AS

DECLARE @dEndDate DATETIME
    
SET @dEndDate = ISNULL(@end_date, GETDATE())

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #unsettled
(
    document_ref VARCHAR(50),
    commission MONEY
)

INSERT INTO #unsettled
SELECT
    d.document_ref,
    (
        SELECT 
            ISNULL(SUM(td_comm.amount), 0)
        FROM document d_did
        JOIN transdetail td_did
            ON td_did.document_id = d_did.document_id
        JOIN account a_did
            ON a_did.account_id = td_did.account_id
        JOIN ledger l_did
            ON l_did.ledger_id = a_did.ledger_id
            AND l_did.ledger_short_name = 'SA'
        JOIN transmatch tm_did
            ON tm_did.transdetail_id = td_did.transdetail_id
            AND tm_did.allocationdetail_id IS NOT NULL
            AND tm_did.is_reversed IS NULL
        JOIN matchgroup mg
            ON mg.match_id = tm_did.match_id
            AND mg.match_date <= @end_date
        JOIN transmatch tm_orig
            ON tm_orig.match_id = tm_did.match_id
            AND tm_orig.transdetail_id <> tm_did.transdetail_id
            AND tm_orig.allocationdetail_id IS NOT NULL
            AND tm_orig.is_reversed IS NULL
        JOIN transdetail td_orig
            ON td_orig.transdetail_id = tm_orig.transdetail_id
        JOIN transdetail td_comm
            ON td_comm.document_id = td_orig.document_id
        JOIN account a_comm
            ON a_comm.account_id = td_comm.account_id
        JOIN ledger l_comm
            ON l_comm.ledger_id = a_comm.ledger_id
            AND l_comm.ledger_short_name = 'CO'
        WHERE d_did.document_id = d.document_id
        /*Only work out commission for DIDs that have a single, two lined, allocation on their client side*/
        AND EXISTS 
            (
                SELECT
                    NULL
                FROM transmatch tm_did
                JOIN matchgroup mg
                    ON mg.match_id = tm_did.match_id
                    AND mg.match_date <= @end_date
                WHERE tm_did.transdetail_id = td_did.transdetail_id
                AND tm_did.allocationdetail_id IS NOT NULL
                AND tm_did.is_reversed IS NULL
                HAVING SUM(1) = 1
            )
        AND EXISTS
            (
                SELECT
                    NULL
                FROM transmatch tm_did
                JOIN matchgroup mg
                    ON mg.match_id = tm_did.match_id
                    AND mg.match_date <= @end_date
                JOIN transmatch tm_all
                    ON tm_all.match_id = tm_did.match_id
                    AND tm_all.allocationdetail_id IS NOT NULL
                    AND tm_all.is_reversed IS NULL
                WHERE tm_did.transdetail_id = td_did.transdetail_id
                AND tm_did.allocationdetail_id IS NOT NULL
                AND tm_did.is_reversed IS NULL
                HAVING SUM(1) = 2
            )
    )
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name = 'IN'
JOIN company c
    ON c.company_id = d.company_id
WHERE d.documenttype_id IN (33, 34)
AND d.document_date <= @end_date
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND td.amount <>
    (
        SELECT 
            ISNULL(SUM(tm.base_match_amount), 0)
        FROM transmatch tm
        JOIN matchgroup  mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @end_date                
        WHERE tm.transdetail_id = td.transdetail_id                
        AND tm.allocationdetail_id IS NOT NULL
        AND tm.is_reversed IS NULL
    )

SELECT 
    @direct_comm_os = ISNULL(SUM(commission),0) * -1
FROM #unsettled

DROP TABLE #unsettled

GO
