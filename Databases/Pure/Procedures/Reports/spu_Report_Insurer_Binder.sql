SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Insurer_Binder'
GO


CREATE PROCEDURE spu_Report_Insurer_Binder 
(
    @insurer_code VARCHAR(50),
    @branch_id INT,
    @sort_id VARCHAR(50),
    @end_date DATETIME,
    @TypeOfCurrency VARCHAR(15)
)

AS

DECLARE @client_settled MONEY
DECLARE @client_amount MONEY
DECLARE @client_transdetail_id INT
DECLARE @document_id INT

SET NOCOUNT ON

/*Initialise the input parameters.*/
IF @insurer_code = 'ALL'
BEGIN
    SELECT @insurer_code = NULL
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @end_date IS NULL OR @end_date = ''
BEGIN
    SELECT @end_date = GETDATE()
END

IF @sort_id IS NULL
BEGIN
    SELECT @sort_id = '1'
END

IF ISNULL(@TypeOfCurrency,'') = ''
BEGIN
    SELECT @TypeOfCurrency = 'Base'
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
    account_id INT,
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
CREATE INDEX I__report_temp__document_id ON #report_temp (document_id)
CREATE INDEX I__report_temp__insurer_code ON #report_temp (insurer_code)


/*Insert valid transactions into temporary table*/
INSERT INTO #report_temp
(  
    document_id,
    document_ref,
    account_id,
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
    td.account_id,    
    s.source_id,
    s.description,    
    p.shortname,
    p.name,    
    d.document_date,
    td.ref_date,    
    dt.code,
    dt.description,    
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN ISNULL(SUM(ROUND(tdx.amount,2)),0)
                WHEN 'Transaction' THEN ISNULL(SUM(ROUND(tdx.currency_amount,2)),0)
            END
        FROM transdetail tdx
        LEFT JOIN transdetail_type ttx
            ON ttx.transdetail_type_id = tdx.transdetail_type_id
        WHERE tdx.document_id = td.document_id
        AND tdx.account_id = td.account_id
        AND ttx.code NOT IN ('COMM', 'COMMADJ', 'IFEE')
    ),
    td.comment
FROM party_type pt
JOIN party p
    ON p.party_type_id = pt.party_type_id
JOIN account a
    ON a.account_key = p.party_cnt
JOIN transdetail td
    ON td.account_id = a.account_id
LEFT JOIN transdetail_type tt
    ON tt.transdetail_type_id = td.transdetail_type_id
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
AND p.shortname = ISNULL(@insurer_code, p.shortname)
AND p.shortname <> 'MULTI'
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND d.document_date <= @end_date
AND /*Insurer amount is still outstanding on the transaction*/
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN SUM(ROUND(ISNULL(tm.base_match_amount,0),2))
                WHEN 'Transaction' THEN SUM(ROUND(ISNULL(tm.currency_match_amount,0),2))
            END
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
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN SUM(ROUND(tda.amount,2))
                WHEN 'Transaction' THEN SUM(ROUND(tda.currency_amount,2))
            END
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

/*Update temporary table with extra fields that need link to insurance_file*/
UPDATE rt
SET renewal_date = i.renewal_date,
    insurance_ref = i.insurance_ref,
    client_code = p.shortname    
FROM #report_temp rt
JOIN document d
    ON d.document_id = rt.document_id
JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt
JOIN party p
    ON p.party_cnt = i.insured_cnt


/*Update temporary table with amounts*/
UPDATE rt
SET ipt_amount = 
        ISNULL((
            SELECT 
                CASE @TypeOfCurrency
                WHEN 'Base' THEN 
	                CASE 
	                    WHEN td.amount > 0 THEN ROUND(td.ref_amount,2) * -1
	                    ELSE ROUND(td.ref_amount,2)
	                END
                WHEN 'Transaction' THEN 
	                CASE 
	                    WHEN td.currency_amount > 0 THEN ROUND(td.ref_amount,2) * -1
	                    ELSE ROUND(td.ref_amount,2)
	                END
                END
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'GROSS'
            WHERE td.document_id = rt.document_id
            AND td.account_id = rt.account_id
        ), 0),
    commission_amount =
        (   
            SELECT 
                CASE @TypeOfCurrency
                    WHEN 'Base' THEN ISNULL(SUM(ROUND(td.amount,2)),0)
                    WHEN 'Transaction' THEN ISNULL(SUM(ROUND(td.currency_amount,2)),0)
                END
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
            WHERE td.document_id = rt.document_id
            AND td.account_id = rt.account_id
            AND 
            (
                (
                    tt.code = 'COMM'
                )
                OR
                (
                    td.ref_date <= @end_date
                    AND
                    tt.code = 'COMMADJ'
                )
            )
        ),
    insurer_paid_amount = 
        (
            SELECT 
                CASE @TypeOfCurrency
                    WHEN 'Base' THEN ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
                    WHEN 'Transaction' THEN ISNULL(SUM(ROUND(tm.currency_match_amount,2)),0)
                END
            FROM transdetail td
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.allocationdetail_id IS NOT NULL
                AND tm.is_reversed IS NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id 
                AND mg.match_date <= @end_date
            WHERE td.document_id = rt.document_id
            AND td.account_id = rt.account_id
        ),
    fee_amount =
        (
            SELECT 
                CASE @TypeOfCurrency
                    WHEN 'Base' THEN ISNULL(SUM(ROUND(td.amount,2)),0)
                    WHEN 'Transaction' THEN ISNULL(SUM(ROUND(td.currency_amount,2)),0)
                END
            FROM transdetail td
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'IFEE'
            WHERE td.document_id = rt.document_id
            AND td.account_id = rt.account_id            
        )
FROM #report_temp rt



/*Update the client settled amount*/

DECLARE c_client CURSOR FAST_FORWARD FOR
    SELECT 
        document_id
    FROM #report_temp

OPEN c_client

FETCH NEXT FROM c_client INTO
    @document_id

WHILE @@FETCH_STATUS = 0 
BEGIN

    SELECT @client_settled = 0
    SELECT @client_amount = 0
    
    SELECT @client_transdetail_id = 
            (
                CASE    
                    (   
                        SELECT 
                            ISNULL(MIN(t.transdetail_id),0)
                        FROM transdetail t 
                        JOIN account a 
                            ON t.account_id = a.account_id 
                        JOIN ledger l 
                            ON a.ledger_id = l.ledger_id
                        WHERE t.document_id = @document_id
                        AND l.ledger_short_name = 'UB'
                    )

                    WHEN 0 THEN
                        (
                            SELECT     
                                t.transdetail_id
                            FROM transdetail t
                            WHERE t.document_id = @document_id
                            AND t.document_sequence = 1
                            AND t.spare IN ('', 'DIRECTDEBIT')
                        )
                    ELSE
                        (
                            SELECT 
                                MIN(t.transdetail_id)
                            FROM transdetail t 
                            JOIN account a 
                                ON t.account_id = a.account_id 
                            JOIN ledger l 
                                ON a.ledger_id = l.ledger_id
                            WHERE t.document_id = @document_id
                            AND l.ledger_short_name = 'UB'
                        )
                END
            )
        

    EXEC spu_ACT_Do_ClientSettled_for_InsurerPayments 
            @ClientTransdetailID = @client_transdetail_id, 
            @ClientAmount = @client_amount OUTPUT,
            @ClientSettled = @client_settled OUTPUT,
            @EndDate = @end_date

    UPDATE #report_temp
    SET client_paid_amount = ISNULL(@client_settled, 0),
        client_amount = ISNULL(@client_amount, 0)
    WHERE document_id = @document_id

    FETCH NEXT FROM c_client INTO
        @document_id

END

/*Close and Deallocate Cursor*/
CLOSE c_client
DEALLOCATE c_client

/*If insurer is an extra account then only show client paid when the client amount has been completely paid off.*/
UPDATE rt
SET rt.client_paid_amount = 0
FROM #report_temp rt
JOIN party p
    ON p.shortname = rt.insurer_code
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pt.code = 'EX'
AND rt.client_paid_amount <> rt.client_amount

/*Limit the client paid amount to the insurer premium amount.*/
UPDATE #report_temp
SET client_paid_amount = premium_amount * -1
WHERE ABS(client_paid_amount) > ABS(premium_amount)


SET NOCOUNT OFF

/*Select the data to return in the order selected.*/
IF LEFT(@sort_id, 1) = '1'
BEGIN
    SELECT
        *
    FROM #report_temp
    ORDER BY
        company_id,
        insurer_code,
        client_code,
        document_ref,
        effective_date
END
ELSE IF LEFT(@sort_id, 1) = '2'
BEGIN
    SELECT
        *
    FROM #report_temp
    ORDER BY
        company_id,
        insurer_code,
        insurance_ref,
        document_ref,
        effective_date
END
ELSE IF LEFT(@sort_id, 1) = '3'
BEGIN
    SELECT
        *
    FROM #report_temp
    ORDER BY
        company_id,
        insurer_code,
        effective_date,
        document_ref
        
END
ELSE IF LEFT(@sort_id, 1) = '4'
BEGIN
    SELECT
        *
    FROM #report_temp
    ORDER BY
        company_id,
        insurer_code,
        document_date,
        document_ref,
        effective_date
END
ELSE
BEGIN
    SELECT
        *
    FROM #report_temp
    ORDER BY
        company_id,
        insurer_code,
        client_code,
        document_ref
END

DROP TABLE #report_temp
