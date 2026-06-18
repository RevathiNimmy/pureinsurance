SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Risk_Transfer'
GO

CREATE PROCEDURE spu_Report_Risk_Transfer
    @branch_id INT,
    @end_date DATETIME,
    @insurer_code VARCHAR(20),
    @RiskTransferStatus VARCHAR(25)
    
AS

SET NOCOUNT ON

IF @insurer_code = 'ALL' OR @insurer_code = ''
BEGIN
    SELECT @insurer_code = NULL
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Report_Temp
(
    Insurer_Code VARCHAR(20),
    Insurer_Name VARCHAR(255),
    Document_ID INT,
    Document_Ref VARCHAR(20),
    Transaction_Date DATETIME ,
    Effective_Date DATETIME ,
    Client_Code VARCHAR(20),
    Client_Name VARCHAR(255),
    Policy_Ref VARCHAR(30),
    Premium MONEY,
    Commission MONEY,
    Fee MONEY,
    IPT MONEY,
    amount_settled MONEY,
    date_settled DATETIME, 
    date_reconciled DATETIME,
    branch_id INT,
    branch_desc VARCHAR(255),
    risk_transfer INT,
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
    INSERT INTO #Report_Temp
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT *
    FROM #Report_Temp
    
    DROP TABLE #Report_Temp
    
    RETURN
END

INSERT INTO #Report_Temp
SELECT
    P.shortname,
    P.resolved_name,
    D.document_id,
    D.document_ref,
    D.document_date,
    TD.ref_date,
    (
        SELECT
            p.shortname            
        FROM transdetail t
        JOIN account a
            ON a.account_id = t.account_id
        JOIN party p
            ON p.party_cnt = a.account_key
        WHERE t.document_id = TD.document_id
        AND t.document_sequence = 
            (
                SELECT
                    MIN(t.document_sequence)
                FROM transdetail t
                JOIN account a
                    ON a.account_id = t.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                WHERE t.document_id = TD.document_id
                AND l.ledger_short_name = 'SA'        
            )
    ),
    (
        SELECT
            p.resolved_name
        FROM transdetail t
        JOIN account a
            ON a.account_id = t.account_id
        JOIN party p
            ON p.party_cnt = a.account_key
        WHERE t.document_id = TD.document_id
        AND t.document_sequence = 
            (
                SELECT
                    MIN(t.document_sequence)
                FROM transdetail t
                JOIN account a
                    ON a.account_id = t.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                WHERE t.document_id = TD.document_id
                AND l.ledger_short_name = 'SA'        
            )
    ),
    I.insurance_ref,
    (
        SELECT
            ISNULL(SUM(ROUND(amount,2)),0)
        FROM transdetail t
        LEFT JOIN transdetail_type tt
            ON tt.transdetail_type_id = t.transdetail_type_id
        WHERE t.document_id = TD.document_id
        AND t.account_id = TD.account_id
        AND 
        (
            tt.code NOT IN ('COMM', 'COMMADJ', 'IFEE')
            OR
            tt.code IS NULL
        )
    ),
    (
        SELECT  
            ISNULL(SUM(ROUND(amount,2)),0)
        FROM transdetail t
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = t.transdetail_type_id
        WHERE t.document_id = TD.document_id
        AND t.account_id = TD.account_id
        AND tt.code IN ('COMM', 'COMMADJ')
    ),
    (
        SELECT  
            ISNULL(SUM(ROUND(amount,2)),0)
        FROM transdetail t
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = t.transdetail_type_id
        WHERE t.document_id = TD.document_id
        AND t.account_id = TD.account_id
        AND tt.code IN ('IFEE')
    ),
    (
        SELECT 
            ISNULL(SUM(ROUND((ISNULL(ref_amount,0) + ISNULL(ref_quantity,0)) * SIGN(amount),2)),0)
        FROM transdetail t
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = t.transdetail_type_id
        WHERE t.document_id = TD.document_id
        AND t.account_id = TD.account_id
    ),
    (
        SELECT 
            ISNULL(SUM(tm.base_match_amount),0)
        FROM transdetail t
        JOIN transmatch tm
            ON tm.transdetail_id = t.transdetail_id
            AND tm.allocationdetail_id IS NOT NULL
            AND tm.is_reversed IS NULL
        WHERE t.account_id = TD.account_id
        AND t.document_id = TD.document_id
    ),
    (
        SELECT 
            MAX(mg.match_date)
        FROM transdetail t
        JOIN transmatch tm
            ON tm.transdetail_id = t.transdetail_id
            AND tm.allocationdetail_id IS NOT NULL
            AND tm.is_reversed IS NULL
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @end_date 
        WHERE t.account_id = TD.account_id
        AND t.document_id = TD.document_id
    ),
    TD.risk_transfer_reconciliation_date,
    S.source_id,
    S.description,
    TD.risk_transfer,
    0
FROM Account A
JOIN Party P
    ON P.party_cnt = A.account_key
JOIN Party_Type PT
    ON PT.party_type_id = P.party_type_id 
    AND PT.code = 'IN'        
JOIN TransDetail TD
    ON TD.account_id = A.account_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Source S
    ON S.source_id = D.company_id 
LEFT JOIN Insurance_File I
    ON I.insurance_file_cnt = D.Insurance_file_cnt 
WHERE D.document_date <= @end_date 
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND A.short_code = ISNULL(@insurer_code, A.short_code)
AND TD.document_sequence = 
    (
        SELECT
            MIN(t.document_sequence)
        FROM transdetail t
        JOIN account a
            ON a.account_id = t.account_id
        JOIN party p
            ON p.party_cnt = a.account_key
        JOIN party_type pt
            ON pt.party_type_id = p.party_type_id         
        WHERE t.document_id = TD.document_id
        AND pt.code = 'IN'
    )
AND (
        (
            @RiskTransferStatus = 'All'
            AND
            TD.risk_transfer IN (1,2,3)        
        )
        OR
        (
            @RiskTransferStatus = 'Unpaid To Insurer'
            AND
            TD.risk_transfer = 1 
        )
        OR
        (
            @RiskTransferStatus = 'Paid To Insurer'
            AND
            TD.risk_transfer IN (2,3)    
        )
    )
AND 
    (
        TD.risk_transfer <> 1
        OR
        (
            TD.risk_transfer = 1
            AND 
            (
                SELECT 
                    ISNULL(SUM(ROUND(t.amount,2)),0)
                FROM transdetail t
                WHERE t.account_id = TD.account_id
                AND t.document_id = TD.document_id
            )
            <>
            (
                SELECT 
                    ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
                FROM transdetail t
                JOIN transmatch tm
                    ON tm.transdetail_id = t.transdetail_id
                    AND tm.allocationdetail_id IS NOT NULL
                    AND tm.is_reversed IS NULL
                WHERE t.account_id = TD.account_id
                AND t.document_id = TD.document_id
            )
        )
    )
            

/*Get client name if not already retrieved*/
UPDATE rt
SET Client_Code =
        (
            SELECT
                a.short_code
            FROM transdetail t
            JOIN account a
                ON a.account_id = t.account_id
            WHERE t.document_id = rt.document_id
            AND t.document_sequence = 
                (
                    SELECT
                        MIN(t.document_sequence)
                    FROM transdetail t
                    JOIN account a
                        ON a.account_id = t.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                    WHERE t.document_id = rt.document_id
                    AND l.ledger_short_name <> 'IN'        
                )
        ),
    Client_Name = 
        (
            SELECT
                a.account_name
            FROM transdetail t
            JOIN account a
                ON a.account_id = t.account_id
            WHERE t.document_id = rt.document_id
            AND t.document_sequence = 
                (
                    SELECT
                        MIN(t.document_sequence)
                    FROM transdetail t
                    JOIN account a
                        ON a.account_id = t.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                    WHERE t.document_id = rt.document_id
                    AND l.ledger_short_name <> 'IN'        
                )
        )
FROM #Report_Temp rt
WHERE Client_Code IS NULL


SET NOCOUNT OFF

SELECT
    *
FROM #Report_Temp rt
ORDER BY
    rt.branch_id,
    rt.insurer_code,    
    rt.risk_transfer, 
    rt.client_code,
    rt.Document_Ref

/* Remove the temp table */
DROP TABLE #Report_Temp

GO
