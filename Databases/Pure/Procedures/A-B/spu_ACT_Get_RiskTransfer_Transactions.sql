SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_RiskTransfer_Transactions'
GO


CREATE  PROCEDURE spu_ACT_Get_RiskTransfer_Transactions
    @account_id INT,
    @date_to DATETIME = NULL,
    @date_by_trans BIT = 0,
    @marked_status INT = NULL,
    @month INT = NULL
AS
/******************************************************************************************/
/* Description: Used by ACTRiskTransferReconciliation                        */
/******************************************************************************************/


DECLARE @settlement_period SMALLINT
DECLARE @amt_settled MONEY
DECLARE @transdetail_id INT
DECLARE @document_id INT
DECLARE @document_id_copy INT
DECLARE @currency_amount MONEY
DECLARE @commadj_amount MONEY
DECLARE @commadj_trans VARCHAR(255)
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
SELECT 
    @settlement_period = settlement_period
FROM Account
WHERE account_id = @account_id

CREATE TABLE #RiskTransferTemp
(
    account_name VARCHAR(255),
    insurer_ref VARCHAR(30),
    document_ref VARCHAR(25),
    gross_transdetail_id INT,
    gross_amount MONEY,
    comm_amount MONEY,
    commadj_transdetail_id VARCHAR(255),
    commadj_amount MONEY,
    fee_amount MONEY,
    amt_settled MONEY,
    document_id INT,
    accounting_date DATETIME,
    currency_id SMALLINT,
    marked_status TINYINT,
    month SMALLINT,
    payment MONEY,
    source_id INT,
    short_code CHAR(20),
    period VARCHAR(15),
    risk_transfer TINYINT
)

INSERT INTO #RiskTransferTemp
SELECT
    '',
    CASE ISNULL(d.comment, '')
        WHEN 'Consolidated Binder' THEN 'Consolidated Binder'
        ELSE ISNULL(t.insurance_ref, '')
    END,
    d.document_ref,
    t.transdetail_id,
    t.currency_amount,
    (
        SELECT 
            ISNULL(MIN(td.currency_amount),0)
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
            ISNULL(MIN(td.currency_amount),0)
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
            AND tm.allocationdetail_id IS NOT NULL
            AND tm.is_reversed IS NULL
        WHERE td.account_id = t.account_id
        AND td.document_id = t.document_id
    ),
    d.document_id,
    t.accounting_date,
    t.currency_id,
    0,
    DATEPART(mm, DATEADD(dd, @settlement_period, t.accounting_date)),
    (
        SELECT 
            ISNULL(SUM(tm.currency_match_amount),0)
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
            AND tm.allocationdetail_id IS NULL
        WHERE td.account_id = t.account_id
        AND td.document_id = t.document_id
    ),
    t.company_id,
    a2.short_code,
    p.period_name,
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
AND (t.risk_transfer = 2 OR t.risk_transfer = 3)
AND (
        (
            t.spare = 'GROSS'
            AND
            t2.document_sequence = 1
        )
        OR  
        (
            t.spare NOT IN ('GROSS', 'COMM', 'COMM ADJ')
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
        @date_to IS NULL
        OR
        (
            @date_to IS NOT NULL
            AND
            (
                (
                    @date_by_trans = 0
                    AND
                    t.accounting_date <= @date_to
                )
                OR
                (
                    @date_by_trans <> 0
                    AND
                    d.created_date <= @date_to
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
            DATEPART(mm, DATEADD(dd, @settlement_period, t.accounting_date)) = @month
        )
    )

/*Commission Adjustments*/

DECLARE it_adjtemp CURSOR FAST_FORWARD FOR
    SELECT
        td.currency_amount,
        it.document_id,
        td.transdetail_id
    FROM #RiskTransferTemp it
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
    @transdetail_id

/*Initialise variables*/
SELECT @commadj_amount = 0
SELECT @commadj_trans = ''
SELECT @document_id_copy = @document_id

WHILE @@FETCH_STATUS = 0

BEGIN

    /*For the same transaction add up all of the commission adjustments and make a note of their transdetail_ids*/
    IF @document_id_copy = @document_id
    BEGIN
        SELECT @commadj_amount = @commadj_amount + @currency_amount
        SELECT @commadj_trans = @commadj_trans + CONVERT(VARCHAR,@transdetail_id) + '|'
    END

    FETCH NEXT FROM it_adjtemp INTO
        @currency_amount,
        @document_id,
        @transdetail_id

    IF @document_id_copy <> @document_id OR @@FETCH_STATUS <> 0
    BEGIN
        /*Update transaction line with commission adjustments*/
        UPDATE #RiskTransferTemp
        SET commadj_amount = ISNULL(@commadj_amount,0),
            commadj_transdetail_id = ISNULL(@commadj_trans,0)
        WHERE document_id = @document_id_copy

        /*Initialise variables*/
        SELECT @commadj_amount = 0
        SELECT @commadj_trans = ''
        SELECT @document_id_copy = @document_id
    END
END

/* Close and Deallocate Cursor */
CLOSE it_adjtemp
DEALLOCATE it_adjtemp

/*Set the correct client name, from either the party resolved_name or if there is no party
record for certain accounts, use the account name instead*/
UPDATE it
SET it.account_name = p.resolved_name
FROM #RiskTransferTemp it
JOIN party p
    ON p.shortname = it.short_code
WHERE it.account_name = ''

UPDATE it
SET it.account_name = a.account_name
FROM #RiskTransferTemp it
JOIN account a
    ON a.short_code = it.short_code
WHERE it.account_name = ''

/*If the marked amount is greater than the outstanding amount then set it to the outstanding amount*/
UPDATE it
SET payment = gross_amount + comm_amount + commadj_amount + fee_amount - amt_settled
FROM #RiskTransferTemp it
WHERE ABS(payment) > ABS(gross_amount + comm_amount + commadj_amount + fee_amount - amt_settled)

UPDATE #RiskTransferTemp
SET marked_status = 1 
WHERE risk_transfer=3

UPDATE #RiskTransferTemp
SET marked_status =0 
WHERE risk_transfer <> 3

/*Select it all. We know the column order so an asterix should suffice.*/
SELECT 
    * 
FROM #RiskTransferTemp 
WHERE marked_status = ISNULL(@marked_status, marked_status)
ORDER BY 
    source_id,  
    insurer_ref, 
    document_ref
        
/* Remove the temp table */
DROP TABLE #RiskTransferTemp

GO
