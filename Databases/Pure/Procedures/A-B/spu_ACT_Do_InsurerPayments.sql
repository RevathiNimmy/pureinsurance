SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_InsurerPayments'
GO

CREATE PROCEDURE spu_ACT_Do_InsurerPayments
    @account_id INT,
    @date_to DATETIME = NULL,
    @date_by_filter INT = 0,
    @marked_status INT = NULL,
    @month INT = NULL
AS



DECLARE @settlement_period SMALLINT
DECLARE @amt_settled MONEY
DECLARE @transdetail_id INT

DECLARE @document_id INT
DECLARE @document_id_copy INT
DECLARE @currency_amount MONEY
DECLARE @tax_amount MONEY
DECLARE @commadj_amount MONEY
DECLARE @commadj_trans VARCHAR(255)
DECLARE @commadj_tax_amount MONEY

DECLARE @client_amount MONEY
DECLARE @client_transdetail_id INT
DECLARE @client_settled MONEY

DECLARE @Financed TINYINT    

DECLARE @client_name VARCHAR(30)
DECLARE @short_code VARCHAR(20)


IF @date_to IS NOT NULL
BEGIN
    SELECT @date_to = DATEADD(hh,23,@date_to)
    SELECT @date_to = DATEADD(mi,59,@date_to)       
    SELECT @date_to = DATEADD(ss,59,@date_to)
END

IF @month < 1 OR @month > 12
BEGIN
    SELECT @month = NULL
END

/* Get the settlement period */
SELECT @settlement_period = settlement_period
FROM Account
WHERE account_id = @account_id

CREATE TABLE #InsurerTemp 
(
    account_name VARCHAR(255), 
    insurer_ref VARCHAR(30),
    document_ref VARCHAR(25),
    gross_transdetail_id INT,
    gross_amount MONEY,
    comm_transdetail_id INT,
    comm_amount MONEY,
    commadj_transdetail_id VARCHAR(255),
    commadj_amount MONEY,
    fee_transdetail_id INT,
    fee_amount MONEY,
    amt_settled MONEY,
    document_id INT,
    effective_date DATETIME,
    currency_id SMALLINT,
    marked_status TINYINT,
    month SMALLINT,
    spare VARCHAR(25),
    payment MONEY,
    source_id INT,
    short_code CHAR(20),
    client_transdetail_id INT,
    client_amount MONEY,
    client_settled MONEY,
    period VARCHAR(15),
    tax MONEY,
    finance_method TINYINT,
    is_direct_to_insurer TINYINT,
    dti_document_id INT,
    payment_due_date DATETIME,
    risk_transfer TINYINT
)


INSERT INTO #InsurerTemp
SELECT
    '',
    ISNULL((   
        CASE d.comment
            WHEN 'Consolidated Binder' THEN d.comment
            ELSE t.insurance_ref
        END
    ),''),
    d.document_ref,
    t.transdetail_id,
    (
        SELECT 
            ISNULL(MAX(currency_amount), 0) 
        FROM transdetail td
        WHERE td.transdetail_id = t.transdetail_id
        AND td.spare <> 'IFee'
    ),
    (
        SELECT 
            ISNULL(MIN(td.transdetail_id),0)
        FROM transdetail td
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
        WHERE td.document_id = t.document_id
        AND td.account_id = t.account_id
        AND tt.code = 'COMM'
    ),
    (
        SELECT 
            ROUND(ISNULL(MIN(td.currency_amount),0),2)
        FROM transdetail td
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
        WHERE td.document_id = t.document_id
        AND td.account_id = t.account_id
        AND tt.code = 'COMM'
    ),
    '',
    0,
    (
        SELECT 
            ISNULL(MIN(td.transdetail_id),0)
        FROM transdetail td
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
        WHERE td.document_id = t.document_id
        AND td.account_id = t.account_id
        AND tt.code = 'IFEE'
    ),
    (
        SELECT 
            ROUND(ISNULL(MIN(td.currency_amount),0),2)
        FROM transdetail td
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
        WHERE td.document_id = t.document_id
        AND td.account_id = t.account_id
        AND tt.code = 'IFEE'
    ),
    (
        SELECT 
            ISNULL(SUM(tm.currency_match_amount),0)
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
        WHERE tm.allocationdetail_id IS NOT NULL
        AND td.account_id = t.account_id
        AND td.document_id = t.document_id
    ),
    d.document_id,
    (    
        CASE @date_by_filter 
        WHEN 0 THEN
            t.ref_date
        ELSE
            d.document_date
        END
    ),
    t.currency_id,
    (
        SELECT 
            ISNULL(SUM(0)+1,0)
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
        WHERE tm.allocationdetail_id IS NULL
        AND td.account_id = t.account_id
        AND td.document_id = t.document_id
    ),
    DATEPART(mm, DATEADD(dd, @settlement_period, t.ref_date)),
    t.spare,
    (
        SELECT 
            ISNULL(SUM(tm.currency_match_amount),0)
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
        WHERE tm.allocationdetail_id IS NULL
        AND td.account_id = t.account_id
        AND td.document_id = t.document_id
    ),
    t.company_id,
    a2.short_code,
    ISNULL((
        CASE    
            (   
                SELECT 
                    ISNULL(MIN(t.transdetail_id),0)
                FROM transdetail t 
                JOIN account a 
                    ON t.account_id = a.account_id 
                JOIN ledger l 
                    ON a.ledger_id = l.ledger_id
                WHERE t.document_id = d.document_id
                AND l.ledger_short_name = 'UB'
            )

            WHEN 0 THEN
                (
                    SELECT     
                        t.transdetail_id
                    FROM transdetail t
                    WHERE t.document_id = d.document_id 
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
                    WHERE t.document_id = d.document_id
                    AND l.ledger_short_name = 'UB'
                )
        END
    ),0),
    0,
    0,
    p.period_name,
    (
        SELECT 
            ROUND(ISNULL(MIN(td.ref_amount),0),2)
        FROM transdetail td
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
        WHERE td.document_id = t.document_id
        AND td.account_id = t.account_id
        AND tt.code = 'COMM'
    ),
    (
        SELECT 
            MAX(
            CASE 
                WHEN pfst.code = 'TP' THEN 2 /*Premium Finance*/
                WHEN pfst.code = 'IH' THEN 1 /*Instalment*/
                WHEN pfst.code = 'TPSG' THEN 3 /*SG Premium Finance*/
            END)
        FROM transdetail td
        JOIN pftransaction_id pft
            ON pft.pftransaction_id = td.transdetail_id
        JOIN pfpremiumfinance pfpf
            ON pfpf.pfprem_finance_cnt = pft.pfprem_finance_cnt
            AND pfpf.statusind = '040' /*Live*/
        JOIN pfscheme pfs
            ON pfs.companyno = pfpf.companyno
            AND pfs.schemeno = pfpf.schemeno
            AND pfs.schemeversion = pfpf.schemeversion
        JOIN pfscheme_type pfst
            ON pfst.pfscheme_type_id = pfs.pfscheme_type_id
        WHERE td.document_id = d.document_id
    ),
    (
        CASE
            WHEN d.documenttype_id IN (33,34) THEN 1
            ELSE 0
        END                    
    ),
    d.document_id,
    d.payment_due_date,
    t.risk_transfer
FROM account a
JOIN transdetail t
    ON t.account_id = a.account_id
JOIN document d
    ON d.document_id = t.document_id
JOIN period p
    ON p.period_id = t.period_id
JOIN transdetail t2
    ON t2.document_id = t.document_id
JOIN account a2
    ON a2.account_id = t2.account_id
WHERE a.account_id = @account_id
AND (
        (
            t.spare = 'GROSS'
            AND
            t2.document_sequence = 1
        )
        OR  
        (
            t.spare NOT IN ('GROSS', 'COMM', 'COMM ADJ')
            AND NOT 
                (
                    t.spare LIKE 'DDREV%' 
                    AND 
                    t.currency_amount = 0
                )                
            AND 
            t.document_sequence = 
                (
                    SELECT
                        MIN(document_sequence)
                    FROM transdetail
                    WHERE document_id = t.document_id
                    AND account_id = t.account_id
                )
            AND 
            NOT EXISTS
                (
                    SELECT
                        NULL
                    FROM transdetail tdx
                    JOIN bankaccount bax
                        ON bax.account_id = tdx.account_id
                    WHERE tdx.document_id = t.document_id
                    AND transdetail_id <> t.transdetail_id
                )
            AND 
            t2.document_sequence = 
                (
                    SELECT
                        MIN(document_sequence)
                    FROM transdetail
                    WHERE document_id = t.document_id
                    AND transdetail_id <> t.transdetail_id
                )
        )
    )
AND (
        (  
            SELECT 
                SUM(currency_amount)
            FROM transdetail
            WHERE document_id = t.document_id
            AND account_id = t.account_id
        ) 
        <> 
        (
            SELECT 
                ISNULL(SUM(aa.alloc_ccy_amount),0)          
            FROM transdetail tt
            JOIN allocationdetail aa    
                ON aa.transdetail_id = tt.transdetail_id
            WHERE tt.document_id = t.document_id
            AND tt.account_id = t.account_id
        )
    OR
        (
            (
                SELECT 
                    SUM(currency_amount)
                FROM transdetail
                WHERE document_id = t.document_id
                AND account_id = t.account_id
            ) = 0
            AND NOT EXISTS
                (
                    SELECT
                        NULL
                    FROM transdetail tt
                    JOIN transmatch tm
                        ON tm.transdetail_id = tt.transdetail_id
                    WHERE tt.document_id = t.document_id
                    AND tt.account_id = t.account_id
                    AND tm.is_reversed IS NULL AND tm.allocationdetail_id IS NOT NULL
                )
        )

    )
AND (
        @date_to IS NULL 
        OR
        (
            @date_to IS NOT NULL 
            AND
            (
                (
                    @date_by_filter = 0 
                    AND 
                    t.ref_date <= @date_to
                )
                OR
                (
                    @date_by_filter = 1 
                    AND 
                    d.document_date <= @date_to
                )
                OR
                (
                    @date_by_filter = 2
                    AND 
                    d.payment_due_date <= @date_to
                )
            )
        )
    )    
AND (
        @month IS NULL
        OR
        (
            @month IS NOT NULL 
            AND 
            DATEPART(mm, DATEADD(dd, @settlement_period, t.ref_date)) = @month
        )
    )
AND (
        @marked_status IS NULL
        OR
        (
            @marked_status IS NOT NULL 
            AND @marked_status = 
                (
                    SELECT 
                        ISNULL(SUM(0)+1,0)
                    FROM transmatch tm
                    WHERE tm.allocationdetail_id IS NULL
                    AND tm.transdetail_id = t.transdetail_id
                )
        ) 
    )

UPDATE it
SET it.dti_document_id = td2.document_id
FROM #InsurerTemp it
JOIN transdetail td
    ON td.document_id = it.document_id
    AND td.document_sequence = 1 /*Always the client line*/
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
JOIN transmatch tm2
    ON tm2.match_id = tm.match_id
    AND tm2.transdetail_id <> tm.transdetail_id 
    AND tm2.is_reversed IS NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
WHERE it.is_direct_to_insurer = 1
AND EXISTS
    (
        SELECT
            NULL
        FROM document
        WHERE document_id IN
            (
                SELECT
                    tdx.document_id
                FROM transmatch tmx
                JOIN transdetail tdx
                    ON tdx.transdetail_id = tmx.transdetail_id
                WHERE tmx.match_id = tm.match_id
                GROUP BY tdx.document_id
            )
        HAVING SUM(1) = 2
    )
AND tm.match_id = 
    (
        SELECT
            MAX(match_id)
        FROM transmatch
        WHERE transdetail_id = td.transdetail_id
    )

/*Commission Adjustments*/

DECLARE it_adjtemp CURSOR FAST_FORWARD FOR
    SELECT 
        td.currency_amount,
        it.document_id,
        td.transdetail_id,
        round(td.ref_amount,2)
    FROM #InsurerTemp it
    JOIN transdetail td
        ON td.document_id = it.document_id
    JOIN transdetail_type tt
        ON tt.transdetail_type_id = td.transdetail_type_id
    WHERE td.account_id = @account_id
    AND tt.code = 'COMMADJ'
    ORDER BY it.document_id 

OPEN it_adjtemp

FETCH NEXT FROM it_adjtemp INTO
    @currency_amount,
    @document_id,
    @transdetail_id,
    @tax_amount

/*Initialise variables*/
SELECT @commadj_amount = 0
SELECT @commadj_trans = ''
SELECT @document_id_copy = @document_id
SELECT @commadj_tax_amount = 0

WHILE @@FETCH_STATUS = 0 
BEGIN

    /*For the same transaction add up all of the commission adjustments and make a note of their transdetail_ids*/
    IF @document_id_copy = @document_id 
    BEGIN
        SELECT @commadj_amount = @commadj_amount + @currency_amount
        SELECT @commadj_trans = @commadj_trans + CONVERT(VARCHAR,@transdetail_id) + '|'
        SELECT @commadj_tax_amount = @commadj_tax_amount + @tax_amount
    END

    FETCH NEXT FROM it_adjtemp INTO
        @currency_amount,
        @document_id,
        @transdetail_id,
        @tax_amount

    IF @document_id_copy <> @document_id OR @@FETCH_STATUS <> 0
    BEGIN
        /*Update transaction line with commission adjustments*/
        UPDATE #InsurerTemp
        SET commadj_amount = ISNULL(@commadj_amount,0),
            commadj_transdetail_id = ISNULL(@commadj_trans,0),
            tax = tax + @commadj_tax_amount
        WHERE document_id = @document_id_copy

        /*Initialise variables*/
        SELECT @commadj_amount = 0
        SELECT @commadj_trans = ''
        SELECT @document_id_copy = @document_id
    SELECT @commadj_tax_amount = 0
    END
END

/* Close and Deallocate Cursor */
CLOSE it_adjtemp
DEALLOCATE it_adjtemp


/*Update the client settled amount*/

DECLARE it_instemp CURSOR FAST_FORWARD FOR
    SELECT 
        gross_transdetail_id,
        client_transdetail_id,
        short_code
    FROM #InsurerTemp

OPEN it_instemp

FETCH NEXT FROM it_instemp INTO
    @transdetail_id, 
    @client_transdetail_id, 
    @short_code

WHILE @@FETCH_STATUS = 0 
BEGIN

    SELECT @client_settled = 0
    SELECT @client_amount = 0

    EXEC spu_ACT_Do_ClientSettled_for_InsurerPayments 
        @ClientTransdetailID=@client_transdetail_id, 
        @ClientAmount=@client_amount OUTPUT,
        @ClientSettled=@client_settled OUTPUT 

    UPDATE #InsurerTemp
    SET client_settled = ISNULL(@client_settled, 0),
        client_amount = ISNULL(@client_amount, 0)
    WHERE gross_transdetail_id = @transdetail_id
    AND @client_settled IS NOT NULL

    FETCH NEXT FROM it_instemp INTO
        @transdetail_id,
        @client_transdetail_id,
        @short_code

END

/*Close and Deallocate Cursor*/
CLOSE it_instemp
DEALLOCATE it_instemp


/*Set the correct client name, from either the party resolved_name or if there is no party
record for certain accounts, use the account name instead*/
UPDATE it
SET it.account_name = p.resolved_name
FROM #InsurerTemp it
JOIN party p
    ON p.shortname = it.short_code
WHERE it.account_name = ''

UPDATE it
SET it.account_name = a.account_name
FROM #InsurerTemp it
JOIN account a
    ON a.short_code = it.short_code
WHERE it.account_name = ''

UPDATE it
SET it.finance_method = 4 /*Not Financed*/
FROM #InsurerTemp it
WHERE it.finance_method IS NULL

/*Remove the transactions that have been fully settled*/
DELETE FROM #InsurerTemp
WHERE amt_settled = gross_amount + comm_amount + commadj_amount + fee_amount
AND amt_settled <> 0


/*If the marked amount is greater than the outstanding amount then set it to the outstanding amount*/
UPDATE it
SET payment = gross_amount + comm_amount + commadj_amount + fee_amount - amt_settled
FROM #InsurerTemp it
WHERE ABS(payment) > ABS(gross_amount + comm_amount + commadj_amount + fee_amount - amt_settled)


/*Select it all. We know the column order so an asterix should suffice.*/
SELECT *
FROM #InsurerTemp
ORDER BY
    dti_document_id,
    is_direct_to_insurer


/* Remove the temp table */
DROP TABLE #InsurerTemp

GO

