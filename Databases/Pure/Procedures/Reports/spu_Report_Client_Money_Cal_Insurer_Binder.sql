SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Money_Cal_Insurer_Binder'
GO

CREATE PROCEDURE spu_Report_Client_Money_Cal_Insurer_Binder
    @branch_id INT,
    @end_date DATETIME,
    @insurer_comm_os MONEY OUTPUT 

AS

/*Initialise the input parameters.*/

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @end_date IS NULL OR @end_date = ''
BEGIN
    SELECT @end_date = GETDATE()
END

/*Create temporary working table.*/
CREATE TABLE #report_temp
(
    insurer_code VARCHAR(20),
    insurer_name VARCHAR(255),
    document_date DATETIME,
    renewal_date DATETIME,
    client_code VARCHAR(20),
    insurance_ref VARCHAR(30),
    doctype_code VARCHAR(10),
    doctype_desc VARCHAR(255),
    premium_amount MONEY,
    transdetail_id INT,
    effective_date DATETIME,
    comment VARCHAR(540),
    commission_amount MONEY,
    commission_match_amount MONEY,
    ipt_amount MONEY,
    company_id INT,
    company_desc VARCHAR(255),
    document_id INT,
    document_ref VARCHAR(25),
    insurer_paid_amount MONEY,
    client_paid_amount MONEY,
    client_amount MONEY,
    fee_amount MONEY
)
CREATE INDEX I__report_temp__transdetail_id ON #report_temp (transdetail_id)
CREATE INDEX I__report_temp__company_id_insurer_code ON #report_temp (company_id, insurer_code)

/*Insert valid transactions into temporary table*/
INSERT INTO #report_temp
(
    document_id,
    document_ref,
    transdetail_id,
    company_id,
    company_desc,
    insurer_code,
    insurer_name,
    document_date,
    effective_date,
    doctype_code,
    doctype_desc,
    premium_amount,
    comment
)
SELECT
    d.document_id,
    d.document_ref,
    td.transdetail_id,
    s.source_id,
    s.description,
    p.shortname,
    p.name,
    d.document_date,
    td.ref_date,
    dt.code,
    dt.description,
    ROUND(td.amount,2),
    td.comment
FROM party_type pt
JOIN party p
    ON p.party_type_id = pt.party_type_id
JOIN account a
    ON a.account_key = p.party_cnt
JOIN transdetail td
    ON td.account_id = a.account_id
JOIN document d
    ON d.document_id = td.document_id
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
JOIN source s
    ON s.source_id = d.company_id
WHERE pt.code IN ('IN', 'EX')
AND td.document_sequence =
    (
        SELECT MIN(document_sequence)
        FROM transdetail
        WHERE document_id = td.document_id
        AND account_id = td.account_id
    )
AND p.shortname <> 'MULTI'
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND d.document_date <= @end_date
AND /*Insurer amount is still outstanding on the transaction*/
    (
        SELECT SUM(ROUND(ISNULL(tm.base_match_amount,0),2))
        FROM transdetail tda
        LEFT JOIN transmatch tm
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
            ON tm.transdetail_id = tda.transdetail_id
            AND tm.allocationdetail_id IS NOT NULL
        WHERE tda.document_id = td.document_id
        AND tda.account_id = td.account_id
    )
    <>
    (
        SELECT SUM(ROUND(tda.amount,2))
        FROM transdetail tda
        WHERE tda.document_id = td.document_id
        AND tda.account_id = td.account_id
        AND
        (
            (
                ISNULL(tda.spare, '') <> 'COMM ADJ'
            )
            OR
            (
                tda.ref_date <= @end_date
                AND
                ISNULL(tda.spare, '') = 'COMM ADJ'
            )
        )
    )

/*Update temporary table with amounts*/
UPDATE rt
SET commission_amount =
        (
            SELECT ISNULL(SUM(ROUND(tdc.amount,2)),0)
            FROM transdetail td
            JOIN transdetail tdc
                ON tdc.document_id = td.document_id
                AND tdc.account_id = td.account_id
                AND tdc.document_sequence <> td.document_sequence
            WHERE td.transdetail_id = rt.transdetail_id
            AND
            (
                (
                    ISNULL(tdc.spare, '') <> 'COMM ADJ'
                )
                OR
                (
                    tdc.ref_date <= @end_date
                    AND
                    ISNULL(tdc.spare, '') = 'COMM ADJ'
                )
            )
        )
FROM #report_temp rt


/*Select the data to return in the order selected.*/
SELECT 
    @insurer_comm_os = ISNULL(SUM(commission_amount),0)
FROM #report_temp

DROP TABLE #report_temp

GO
