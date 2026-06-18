SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Renewal_Income'
GO

CREATE PROCEDURE spu_Report_Renewal_Income
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

DECLARE
    @transdetail_id INT,
    @documenttype_id INT,
    @accounting_date DATETIME,
    @amount MONEY,
    @previous_start_date DATETIME,
    @previous_end_date DATETIME

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT @previous_start_date = DATEADD(yy, -1, @start_date) 
SELECT @previous_end_date = DATEADD(yy, -1, @end_date) 

CREATE TABLE #Report_Renewal_Income
(
    /*Main Policy ID and Ref*/
    insurance_folder_cnt INT,
    insurance_ref VARCHAR(30),

    /*Current Year Details*/
    insurance_file_cnt INT,
    cover_start_date DATETIME,
    renewal_date DATETIME,
    income_this_year MONEY,
    
    /*Previous Year Details*/
    previous_insurance_file_cnt INT,
    previous_cover_start_date DATETIME,
    previous_renewal_date DATETIME,
    income_last_year MONEY,
    
    /*Other Policy Details*/
    business_type VARCHAR(255),
    client VARCHAR(20),
    risk_type VARCHAR(255),
    account_handler VARCHAR(20),
    account_exec VARCHAR(20),
    lapsed VARCHAR(9),
    branch_id INT,
    branch_code VARCHAR(10),
    branch_name VARCHAR(255),
    currency_id INT,
    currency_code VARCHAR(10),
    currency_desc VARCHAR(255)
)

/*Add all policies for the current year, base on the date parameters*/
INSERT INTO #report_renewal_income
(  
    insurance_folder_cnt,
    insurance_ref,
    insurance_file_cnt,
    cover_start_date,
    renewal_date
)
SELECT
    i.insurance_folder_cnt,
    i.insurance_ref,
    i.insurance_file_cnt,
    i.cover_start_date,
    i.renewal_date
FROM insurance_file i
WHERE i.source_id = ISNULL(@branch_id, i.source_id)
AND i.cover_start_date BETWEEN @start_date AND @end_date
AND i.insurance_file_type_id = 2
AND ISNULL(i.policy_ignore, 0) = 0

/*Update selected policies with last years policy version*/
UPDATE ri
SET ri.previous_insurance_file_cnt = i.insurance_file_cnt,
    ri.previous_cover_start_date = i.cover_start_date,
    ri.previous_renewal_date = i.renewal_date
FROM #report_renewal_income ri
JOIN insurance_file i
    ON i.insurance_folder_cnt = ri.insurance_folder_cnt
WHERE i.source_id = ISNULL(@branch_id, i.source_id)
AND i.cover_start_date BETWEEN @previous_start_date AND @previous_end_date
AND i.insurance_file_type_id = 2
AND ISNULL(i.policy_ignore, 0) = 0

/*Add all policies for the current year, base on the date parameters*/
INSERT INTO #report_renewal_income
(  
    insurance_folder_cnt,
    insurance_ref,
    previous_insurance_file_cnt,
    previous_cover_start_date,
    previous_renewal_date
)
SELECT
    i.insurance_folder_cnt,
    i.insurance_ref,
    i.insurance_file_cnt,
    i.cover_start_date,
    i.renewal_date
FROM insurance_file i
WHERE i.source_id = ISNULL(@branch_id, i.source_id)
AND i.cover_start_date BETWEEN @previous_start_date AND @previous_end_date
AND i.insurance_file_type_id = 2
AND ISNULL(i.policy_ignore, 0) = 0
AND NOT EXISTS
    (
        SELECT
            NULL
        FROM #report_renewal_income ri
        WHERE ri.insurance_folder_cnt = i.insurance_folder_cnt
    )


UPDATE ri
SET ri.branch_id = s.source_id,
    ri.branch_code = s.code,
    ri.branch_name = s.description,
    ri.currency_id = c.currency_id,
    ri.currency_code = c.code,
    ri.currency_desc = c.description,
    ri.business_type = ISNULL(bc.description, 'No Business Type'),
    ri.client = p.shortname,
    ri.risk_type = rc.description,
    ri.account_handler = ISNULL(p2.shortname, ' '),
    ri.account_exec = ISNULL(p3.shortname, ' '),
    ri.lapsed = ''
FROM #report_renewal_income ri
JOIN insurance_file i
    ON i.insurance_file_cnt = ISNULL(ri.insurance_file_cnt, ri.previous_insurance_file_cnt)
JOIN party p
    ON p.party_cnt = i.insured_cnt 
LEFT JOIN party p2
    ON p2.party_cnt = i.account_handler_cnt
LEFT JOIN party p3
    ON p3.party_cnt = p.consultant_cnt
JOIN risk_code rc
    ON rc.risk_code_id = i.risk_code_id
LEFT JOIN business_type bc
    ON bc.business_type_id = i.business_type_id
JOIN source s 
    ON s.source_id = i.source_id
JOIN currency c
    ON c.currency_id = i.currency_id 



UPDATE ri
SET ri.income_this_year = 
    (
        SELECT
            ISNULL(SUM(td.amount) * -1, 0)
        FROM insurance_file i
        JOIN document d
            ON d.insurance_file_cnt = i.insurance_file_cnt
        JOIN transdetail td
            ON td.document_Id = d.document_id
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE i.insurance_folder_cnt = ri.insurance_folder_cnt
        AND d.documenttype_id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
        AND td.ref_date BETWEEN ri.cover_start_date AND DATEADD(s, -1, ri.renewal_date)
        AND td.spare NOT LIKE 'Revers%'
        AND l.ledger_short_name IN ('CO', 'FE', 'DI') 
    )
FROM #report_renewal_income ri

UPDATE ri
SET ri.income_last_year = 
    (
        SELECT
            ISNULL(SUM(td.amount) * -1, 0)
        FROM insurance_file i
        JOIN document d
            ON d.insurance_file_cnt = i.insurance_file_cnt
        JOIN transdetail td
            ON td.document_Id = d.document_id
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE i.insurance_folder_cnt = ri.insurance_folder_cnt
        AND d.documenttype_Id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
        AND td.ref_date BETWEEN ri.previous_cover_start_date AND DATEADD(s, -1, ri.previous_renewal_date)
        AND td.spare NOT LIKE 'Revers%'
        AND l.ledger_short_name IN ('CO', 'FE', 'DI') 
    )
FROM #report_renewal_income ri

UPDATE ri
SET ri.lapsed = 'LAPSED'
FROM #report_renewal_income ri
JOIN insurance_file i
    ON i.insurance_folder_cnt = ri.insurance_folder_cnt
    AND i.insurance_file_cnt > ISNULL(ri.insurance_file_cnt, ri.previous_insurance_file_cnt)
JOIN insurance_file_status s
    ON s.insurance_file_status_id = i.insurance_file_status_id
WHERE s.code IN ('LAP', 'RENLAP')

UPDATE ri
SET ri.lapsed = 'CANCELLED'
FROM #report_renewal_income ri
JOIN insurance_file i
    ON i.insurance_folder_cnt = ri.insurance_folder_cnt
    AND i.insurance_file_cnt > ISNULL(ri.insurance_file_cnt, ri.previous_insurance_file_cnt)
JOIN insurance_file_status s
    ON s.insurance_file_status_id = i.insurance_file_status_id
WHERE s.code = 'CAN'
        
DELETE
FROM #report_renewal_income
WHERE income_this_year = 0 
AND income_last_year = 0
        
SELECT
    *
FROM #Report_Renewal_Income
ORDER BY 
    currency_id,
    branch_id, 
    business_type,
    insurance_ref

DROP TABLE #Report_Renewal_Income




GO