SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Direct_Business'
GO 

CREATE PROCEDURE spu_Report_Direct_Business  
    @start_date DATETIME,  
    @end_date DATETIME,  
    @branch_id INT,  
    @insurer_code VARCHAR(20),  
    @report_type VARCHAR(20)  
AS  
  
IF @insurer_code = '' OR @insurer_code = 'ALL'  
BEGIN  
    SELECT @insurer_code = NULL  
END  
  
IF @branch_id = 0  
BEGIN  
    SELECT @branch_id = NULL  
END  
  
SELECT  
    c.description 'company_desc',  
    d.document_date 'document_date',  
    d.document_ref 'document_ref',  
    a.short_code 'insurer_code',  
    td.insurance_ref 'insurance_ref',  
    (  
        SELECT  
            a.short_code  
        FROM transdetail td  
        JOIN account a  
            ON a.account_id = td.account_id  
        JOIN ledger l  
            ON l.ledger_id = a.ledger_id  
            AND l.ledger_short_name = 'SA'  
        WHERE td.document_id = d.document_id  
  
    ) 'client_code',  
    td.amount * -1 'premium',  
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
    ) 'commission'  
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
AND d.document_date BETWEEN @start_date AND @end_date  
AND d.company_id = ISNULL(@branch_id, d.company_id)  
AND a.short_code = ISNULL(@insurer_code, a.short_code)  
AND (  
        (  
            @report_type = 'Unsettled'  
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
        )  
        OR  
        (  
            @report_type = 'Settled'  
            AND td.amount =  
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
        )  
    )  
ORDER BY  
    c.description,  
   2,  
    3  
