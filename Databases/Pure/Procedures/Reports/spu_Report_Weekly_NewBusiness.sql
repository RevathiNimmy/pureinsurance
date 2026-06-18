SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Weekly_NewBusiness'
GO


CREATE PROCEDURE spu_Report_Weekly_NewBusiness
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(12) 

AS

DECLARE
    @document_id INT,
    @multidocument_id INT,
    @amount MONEY,
    @agencyorunderwriting VARCHAR(1)

SELECT 
    @agencyorunderwriting = value 
FROM hidden_options 
WHERE option_number = 1

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Report_Weekly_NewBusiness
(
    branch VARCHAR(255) NULL,
    business_type VARCHAR(255) NULL,
    client VARCHAR(20) NULL,
    insurer_ref VARCHAR(255) NULL,
    agent VARCHAR(20) NULL,
    account_handler VARCHAR(20) NULL,
    risk_code VARCHAR(255) NULL,
    premium MONEY NULL,
    gross_commission MONEY NULL,
    fee MONEY NULL,
    discount MONEY NULL,
    agent_commission MONEY NULL,
    net_commission MONEY NULL,
    commission_adjustment MONEY NULL,
    document_id INT,
    rev CHAR(1)
)



INSERT INTO #Report_Weekly_NewBusiness
(
    branch,
    business_type,
    client,
    insurer_ref,
    agent,
    account_handler,
    risk_code,
    premium,
    gross_commission,
    fee,
    discount,
    agent_commission,
    net_commission,
    commission_adjustment,
    document_id,
    rev
)
SELECT 
    (
        SELECT 
            ISNULL(MAX(c.description), 'No Branch Specified')
        FROM company c
        WHERE c.company_id = d.company_id 
    ),
    (
        SELECT 
            ISNULL(MAX(BT.description), 'No Business Specified')
        FROM insurance_file i
        JOIN business_type bt
            ON bt.business_type_id = i.business_type_id
        WHERE i.insurance_file_cnt = d.insurance_file_cnt
    ),
    (
        SELECT 
            ISNULL(MAX(a.short_code), '')
        FROM account a
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name = 'SA'
        WHERE a.account_id = td.account_id
    ),
    (
        SELECT 
            ISNULL(MAX(i.insurance_ref), '')
        FROM insurance_file i
        WHERE i.insurance_file_cnt = d.insurance_file_cnt
    ),         
    (
        SELECT 
            ISNULL(MAX(a.short_code), '')
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        WHERE td.document_id = d.document_id
        AND td.document_sequence =
            (
                SELECT 
                    MIN(td.document_sequence)
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                    AND ledger_short_name IN ('AG', 'TR', 'UB')
                WHERE td.document_id = d.document_id
            )
    ),
    (
        SELECT 
            ISNULL(MAX(p.shortname), '')
        FROM insurance_file i
        JOIN party p
            ON p.party_cnt = i.account_handler_cnt
        WHERE i.insurance_file_cnt = d.insurance_file_cnt
    ),
    (
        SELECT 
            ISNULL(MAX(r.description), '')
        FROM insurance_file i
        JOIN risk_code r
            ON r.risk_code_id = i.risk_code_id
        WHERE i.insurance_file_cnt = d.insurance_file_cnt
    ),
    (
        SELECT 
            ISNULL(SUM(td.amount), 0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name = 'SA'
        WHERE td.document_id = d.document_id
        AND td.document_sequence = 1
    ),        
    (
        SELECT 
            ISNULL(SUM(td.amount), 0)  * -1
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name = 'IN'
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
            AND tt.code IN ('COMM', 'COMM ADJ')
        WHERE td.document_id = d.document_id  
    ),
    (
        SELECT 
            ISNULL(SUM(td.amount), 0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name = 'FE'
        WHERE td.document_id = d.document_id  
    ),
    (
        SELECT 
            ISNULL(SUM(td.amount), 0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name = 'DI'
        WHERE td.document_id = d.document_id  
    ),
    /*Agent Amount : 0 if no agent*/
    (
        SELECT 
            ISNULL(SUM(ROUND(td.amount,2)),0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name  = 'AG'
        WHERE td.document_id = d.document_id
        AND ISNULL(td.spare, '') <> 'AGENT ADJ'
    )
    +
    /*Sub Agent Amount (DD) : 0 if no sub agent or isn't a direct debit transaction*/
   (
        SELECT 
            ISNULL(SUM(ROUND(td.amount,2)),0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name  = 'UB'
        WHERE td.document_id = d.Document_id
        AND ISNULL(td.spare, '') <> 'AGENT ADJ'
        AND td.document_sequence =
            (
                SELECT 
                    MIN(td2.document_sequence)
                FROM transdetail td2
                WHERE td2.document_id = td.document_id
                AND td2.account_id = td.account_id
            )
        AND 0 <>
            (
                SELECT 
                    ISNULL(SUM(ROUND(td.amount,2)),0)
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                    AND l.ledger_short_name  = 'SA'
                WHERE td.document_id = d.Document_id
            )
    )
    /*Sub Agent Amount (Not DD) : 0 if no sub agent or is a direct debit transaction*/
    -
    (
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'SA'
            WHERE td.document_id = d.Document_id
            AND td.document_sequence =
                (
                    SELECT 
                        MIN(document_sequence)
                    FROM transdetail td2
                    WHERE td2.document_id = td.document_id
                )
            AND EXISTS
                (
                    SELECT NULL
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name  = 'UB'
                    WHERE td.document_id = d.Document_id
                    AND ISNULL(td.spare, '') <> 'AGENT ADJ'
                )
            AND 0 =
                (
                    SELECT 
                        ISNULL(SUM(ROUND(td.amount,2)),0)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name  = 'SA'
                    WHERE td.document_id = d.Document_id
                )
        )
        -
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'UB'
            WHERE td.document_id = d.Document_id
            AND ISNULL(td.spare, '') <> 'AGENT ADJ'
            AND 0 =
                (
                    SELECT 
                        ISNULL(SUM(ROUND(td.amount,2)),0)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name  = 'SA'
                    WHERE td.document_id = d.Document_id        
                )
        )
    ),    
    (
        SELECT 
            ISNULL(SUM(td.amount), 0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name = 'CO'
        WHERE td.document_id = d.document_id  
    ),
    (
        SELECT 
            ISNULL(SUM(td.amount), 0) * -1
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND ledger_short_name IN ('NO', 'CO')
        WHERE td.document_id = d.document_id  
        AND @date_type = 'Effective'    
        AND (
                td.ref_date < @start_date
                OR 
                td.ref_date > @end_date
            )
    ),
    d.document_id,
    (
        SELECT 
            'R'
        WHERE d.comment like 'Revers%'
    )
FROM document d
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1
WHERE dt.code IN ('SND','SNC')
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND (
        (
            @date_type = 'Transaction'
            AND
            d.document_date BETWEEN @start_date AND @end_date
        )
        OR  
        (
            @date_type = 'Effective'
            AND
            td.ref_date BETWEEN @start_date AND @end_date
            AND
            td.spare NOT LIKE 'REVERS%'
        )
    )


SELECT
    branch,
    business_type,
    client,
    insurer_ref,
    agent,
    account_handler,
    risk_code,
    premium,
    gross_commission,
    fee,
    discount,
    agent_commission,
    net_commission,
    commission_adjustment,
    document_id,
    rev,
    @agencyorunderwriting 'agencyorunderwriting'
FROM #Report_Weekly_NewBusiness
ORDER BY
    branch,
    business_type,
    client,
    insurer_ref

DROP TABLE #Report_Weekly_NewBusiness

GO

