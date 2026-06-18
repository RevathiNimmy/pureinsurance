SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Return_Premiums'
GO

CREATE PROCEDURE spu_Report_Return_Premiums
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

DECLARE @document_id INT
DECLARE @account_id INT
DECLARE @match_id INT
DECLARE @order_id INT
DECLARE @first_order_id INT
DECLARE @total_order_id INT
DECLARE @account_order_id INT


IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #temp_trans
(
    document_id INT,
    order_id INT,
    debit_credit INT,
    document_ref VARCHAR(25),
    insurance_ref VARCHAR(30),
    document_date DATETIME,
    commission_amount MONEY,
    
    client_code VARCHAR(20),
    client_amount MONEY,
    client_matched MONEY,
    
    client_payment_document_ref VARCHAR(25),
    client_payment_amount MONEY,
    client_payment_settled_date DATETIME,
    client_payment_days_os INT,
    
    insurer_code VARCHAR(20),
    insurer_amount MONEY,
    insurer_matched MONEY,

    insurer_payment_document_ref VARCHAR(25),
    insurer_payment_amount MONEY,
    insurer_payment_settled_date DATETIME,
    insurer_payment_days_os INT,
    
    max_days_os INT,
    
    fsa_disabled BIT
)
CREATE INDEX I__temp_trans__document_id__order_id ON #temp_trans (document_id, order_id)

IF NOT EXISTS 
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    INSERT INTO #temp_trans
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT *
    FROM #temp_trans
    
    DROP TABLE #temp_trans
    
    RETURN
END

/*Add funded transactions to the temporary table*/
INSERT INTO #temp_trans
(
    document_id,
    order_id,
    debit_credit,
    document_ref,
    insurance_ref,
    document_date,
    commission_amount
)
SELECT
    d.document_id,
    order_id = 1,
    CASE
        WHEN d.documenttype_id IN (4,15,17,31,35) THEN 1 /*Debit*/
        ELSE 2 /*Credit*/
    END debit_credit,
    d.document_ref,
    i.insurance_ref,
    d.document_date,
    (
        SELECT ISNULL(SUM(td.amount),0)
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'CO'
        WHERE td.document_id = d.document_id
    ) 
FROM document d
JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt
WHERE d.documenttype_id IN (5,16,18,32,36) /*SNC,SRC,SEC,SHC,TRC*/
AND d.document_date BETWEEN @start_date AND @end_date
AND d.company_id = ISNULL(@branch_id,d.company_id)
/*We need to pay the client/subagent (i.e. is definitely a credit transaction)*/
AND (
        SELECT 
            SUM(ISNULL(td.amount,0))
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE td.document_id = d.document_id
        AND l.ledger_short_name IN ('SA','UB')
    ) < 0
/*The total amount matched means that the broker has received more money than he's giving out.*/
AND (
        SELECT 
            SUM(ISNULL(tm.base_match_amount,0))
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        LEFT JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
            AND tm.allocationdetail_id IS NOT NULL
        WHERE td.document_id = d.document_id
        AND l.ledger_short_name IN ('SA','IN','AG','UB')
    ) > 0 

/*Remove Direct Debits as these should all be fine.*/
DELETE #temp_trans
FROM #temp_trans tt
JOIN transdetail td
    ON td.document_id = tt.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.allocationdetail_id IS NOT NULL
    AND ISNULL(tm.is_reversed,0) <> 1
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tmx
    ON tmx.match_id = tm.match_id
    AND tmx.allocationdetail_id IS NOT NULL
    AND ISNULL(tmx.is_reversed,0) <> 1
JOIN transdetail tdx
    ON tdx.transdetail_id = tmx.transdetail_id
JOIN document dx
    ON dx.document_id = tdx.document_id
    AND dx.documenttype_id IN (33,34)

    
DECLARE c_transaction CURSOR FORWARD_ONLY STATIC FOR
    SELECT document_id
    FROM #temp_trans
    
OPEN c_transaction

FETCH NEXT FROM c_transaction INTO @document_id

WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT @order_id = 1
    
    /*CHANGE THIS TO RESTRICT SUBAGENT*/
    DECLARE c_client CURSOR FAST_FORWARD FOR
        SELECT DISTINCT 
            a.account_id,
            CASE l.ledger_short_name
                WHEN 'SA' THEN 1
                WHEN 'UB' THEN 2
            END
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name IN ('SA','UB')
        WHERE td.document_id = @document_id
        ORDER BY 
            CASE l.ledger_short_name
                WHEN 'SA' THEN 1
                WHEN 'UB' THEN 2
            END

    OPEN c_client

    FETCH NEXT FROM c_client INTO @account_id, @account_order_id

    WHILE @@FETCH_STATUS = 0
    BEGIN

        IF @order_id = 1
        BEGIN
            UPDATE #temp_trans
            SET client_code = 
                    (
                        SELECT short_code
                        FROM account
                        WHERE account_id = @account_id
                    ),
                client_amount = 
                    (
                        SELECT ISNULL(SUM(td.amount),0)
                        FROM transdetail td
                        WHERE td.document_id = @document_id
                        AND td.account_id = @account_id
                    ),
                client_matched = 
                    (
                        SELECT ISNULL(SUM(tm.base_match_amount),0)
                        FROM transdetail td
                        JOIN transmatch tm
                            ON tm.transdetail_id = td.transdetail_id
                            AND tm.allocationdetail_id IS NOT NULL
                        WHERE td.document_id = @document_id
                        AND td.account_id = @account_id
                    )
            WHERE document_id = @document_id
            AND order_id = @order_id
        END
        ELSE
        BEGIN
            
            INSERT INTO #temp_trans
            (
                document_id,
                order_id,
                debit_credit,
                document_ref,
                insurance_ref,
                document_date,
                commission_amount,
                client_code,
                client_amount,
                client_matched
            )
            SELECT
                document_id,
                @order_id,
                debit_credit,
                document_ref,
                insurance_ref,
                document_date,
                commission_amount,
                (
                    SELECT short_code
                    FROM account
                    WHERE account_id = @account_id
                ),
                (
                    SELECT ISNULL(SUM(td.amount),0)
                    FROM transdetail td
                    WHERE td.document_id = @document_id
                    AND td.account_id = @account_id
                ),
                (
                    SELECT ISNULL(SUM(tm.base_match_amount),0)
                    FROM transdetail td
                    JOIN transmatch tm
                        ON tm.transdetail_id = td.transdetail_id
                        AND tm.allocationdetail_id IS NOT NULL
                    WHERE td.document_id = @document_id
                    AND td.account_id = @account_id
                )
            FROM #temp_trans
            WHERE document_id = @document_id
            AND order_id = 1
            
        END
        
        SELECT @first_order_id = @order_id

        DECLARE c_payment CURSOR FAST_FORWARD FOR
            SELECT DISTINCT tm.match_id
            FROM transdetail td
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.allocationdetail_id IS NOT NULL
            WHERE td.document_id = @document_id
            AND td.account_id = @account_id

        OPEN c_payment

        FETCH NEXT FROM c_payment INTO @match_id

        WHILE @@FETCH_STATUS = 0
        BEGIN
        
            IF @order_id = @first_order_id
            BEGIN
                UPDATE #temp_trans
                SET client_payment_document_ref =
                        (
                            SELECT MAX(ISNULL(dx.document_ref,'N/A'))
                            FROM transmatch tm
                            JOIN transdetail td
                                ON td.transdetail_id = tm.transdetail_id
                            JOIN document d
                                ON d.document_id = td.document_id
                            LEFT JOIN allocationdetail ad
                                    JOIN cashlistitem cli
                                        ON cli.cashlistitem_id = ad.cashlistitem_id
                                    JOIN transdetail tdx
                                        ON tdx.transdetail_id = cli.transdetail_id
                                    JOIN document dx
                                        ON dx.document_id = tdx.document_id
                                ON ad.allocationdetail_id = tm.allocationdetail_id
                                AND ad.transdetail_id = tm.transdetail_id
                            WHERE tm.match_id = @match_id
                            AND d.document_id = @document_id
                        ),
                    client_payment_amount =
                        (
                            SELECT SUM(tm.base_match_amount)
                            FROM transmatch tm
                            JOIN transdetail td
                                ON td.transdetail_id = tm.transdetail_id
                            JOIN document d
                                ON d.document_id = td.document_id
                            WHERE tm.match_id = @match_id
                            AND d.document_id = @document_id
                        ),
                    client_payment_settled_date =
                        (
                            SELECT mg.match_date
                            FROM matchgroup mg
                            WHERE mg.match_id = @match_id
                        )
                WHERE document_id = @document_id
                AND order_id = @order_id
            END
            ELSE
            BEGIN

                INSERT INTO #temp_trans
                (
                    document_id,
                    order_id,
                    debit_credit,
                    document_ref,
                    insurance_ref,
                    document_date,
                    commission_amount,
                    client_code,
                    client_amount,
                    client_matched,
                    client_payment_document_ref,
                    client_payment_amount,
                    client_payment_settled_date
                )
                SELECT
                    document_id,
                    @order_id,
                    debit_credit,
                    document_ref,
                    insurance_ref,
                    document_date,
                    commission_amount,
                    client_code,
                    client_amount,
                    client_matched,
                    (
                        SELECT MAX(ISNULL(dx.document_ref,'N/A'))
                        FROM transmatch tm
                        JOIN transdetail td
                            ON td.transdetail_id = tm.transdetail_id
                        JOIN document d
                            ON d.document_id = td.document_id
                        LEFT JOIN allocationdetail ad
                                JOIN cashlistitem cli
                                    ON cli.cashlistitem_id = ad.cashlistitem_id
                                JOIN transdetail tdx
                                    ON tdx.transdetail_id = cli.transdetail_id
                                JOIN document dx
                                    ON dx.document_id = tdx.document_id
                            ON ad.allocationdetail_id = tm.allocationdetail_id
                            AND ad.transdetail_id = tm.transdetail_id
                        WHERE tm.match_id = @match_id
                        AND d.document_id = @document_id
                    ),
                    (
                        SELECT SUM(tm.base_match_amount)
                        FROM transmatch tm
                        JOIN transdetail td
                            ON td.transdetail_id = tm.transdetail_id
                        JOIN document d
                            ON d.document_id = td.document_id
                        WHERE tm.match_id = @match_id
                        AND d.document_id = @document_id
                    ),
                    (
                        SELECT mg.match_date
                        FROM matchgroup mg
                        WHERE mg.match_id = @match_id
                    )
                FROM #temp_trans
                WHERE document_id = @document_id
                AND order_id = @first_order_id

            END
            
            SELECT @order_id = @order_id + 1

            FETCH NEXT FROM c_payment INTO @match_id
        END

        CLOSE c_payment
        DEALLOCATE c_payment
            
        IF @order_id = @first_order_id 
        BEGIN
            SELECT @order_id = @order_id + 1
        END
        
        FETCH NEXT FROM c_client INTO @account_id, @account_order_id
    END

    CLOSE c_client
    DEALLOCATE c_client
    
    SELECT @total_order_id = @order_id - 1
    SELECT @order_id = 1

    DECLARE c_insurer CURSOR FAST_FORWARD FOR
        SELECT DISTINCT 
            a.account_id,
            CASE l.ledger_short_name
                WHEN 'IN' THEN 1
                WHEN 'AG' THEN 2
            END

        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name IN ('IN','AG')
        WHERE td.document_id = @document_id
        ORDER BY 
            CASE l.ledger_short_name
                WHEN 'IN' THEN 1
                WHEN 'AG' THEN 2
            END

    OPEN c_insurer

    FETCH NEXT FROM c_insurer INTO @account_id, @account_order_id

    WHILE @@FETCH_STATUS = 0
    BEGIN

        IF @order_id <= @total_order_id
        BEGIN
            UPDATE #temp_trans
            SET insurer_code = 
                    (
                        SELECT short_code
                        FROM account
                        WHERE account_id = @account_id
                    ),
                insurer_amount = 
                    (
                        SELECT ISNULL(SUM(td.amount),0)
                        FROM transdetail td
                        WHERE td.document_id = @document_id
                        AND td.account_id = @account_id
                    ),
                insurer_matched = 
                    (
                        SELECT ISNULL(SUM(tm.base_match_amount),0)
                        FROM transdetail td
                        JOIN transmatch tm
                            ON tm.transdetail_id = td.transdetail_id
                            AND tm.allocationdetail_id IS NOT NULL
                        WHERE td.document_id = @document_id
                        AND td.account_id = @account_id
                    )
            WHERE document_id = @document_id
            AND order_id = @order_id
        END
        ELSE
        BEGIN
            
            INSERT INTO #temp_trans
            (
                document_id,
                order_id,
                debit_credit,
                document_ref,
                insurance_ref,
                document_date,
                commission_amount,
                insurer_code,
                insurer_amount,
                insurer_matched
            )
            SELECT
                document_id,
                @order_id,
                debit_credit,
                document_ref,
                insurance_ref,
                document_date,
                commission_amount,
                (
                    SELECT short_code
                    FROM account
                    WHERE account_id = @account_id
                ),
                (
                    SELECT ISNULL(SUM(td.amount),0)
                    FROM transdetail td
                    WHERE td.document_id = @document_id
                    AND td.account_id = @account_id
                ),
                (
                    SELECT ISNULL(SUM(tm.base_match_amount),0)
                    FROM transdetail td
                    JOIN transmatch tm
                        ON tm.transdetail_id = td.transdetail_id
                        AND tm.allocationdetail_id IS NOT NULL
                    WHERE td.document_id = @document_id
                    AND td.account_id = @account_id
                )
            FROM #temp_trans
            WHERE document_id = @document_id
            AND order_id = 1
            
        END
        
        SELECT @first_order_id = @order_id

        DECLARE c_payment CURSOR FAST_FORWARD FOR
            SELECT DISTINCT tm.match_id
            FROM transdetail td
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.allocationdetail_id IS NOT NULL
            WHERE td.document_id = @document_id
            AND td.account_id = @account_id

        OPEN c_payment

        FETCH NEXT FROM c_payment INTO @match_id

        WHILE @@FETCH_STATUS = 0
        BEGIN
        
            IF @order_id = @first_order_id
            BEGIN
                UPDATE #temp_trans
                SET insurer_payment_document_ref =
                        (
                            SELECT MAX(ISNULL(dx.document_ref,'N/A'))
                            FROM transmatch tm
                            JOIN transdetail td
                                ON td.transdetail_id = tm.transdetail_id
                            JOIN document d
                                ON d.document_id = td.document_id
                            LEFT JOIN allocationdetail ad
                                    JOIN cashlistitem cli
                                        ON cli.cashlistitem_id = ad.cashlistitem_id
                                    JOIN transdetail tdx
                                        ON tdx.transdetail_id = cli.transdetail_id
                                    JOIN document dx
                                        ON dx.document_id = tdx.document_id
                                ON ad.allocationdetail_id = tm.allocationdetail_id
                                AND ad.transdetail_id = tm.transdetail_id
                            WHERE tm.match_id = @match_id
                            AND d.document_id = @document_id
                        ),
                    insurer_payment_amount =
                        (
                            SELECT SUM(tm.base_match_amount)
                            FROM transmatch tm
                            JOIN transdetail td
                                ON td.transdetail_id = tm.transdetail_id
                            JOIN document d
                                ON d.document_id = td.document_id
                            WHERE tm.match_id = @match_id
                            AND d.document_id = @document_id
                        ),
                    insurer_payment_settled_date =
                        (
                            SELECT mg.match_date
                            FROM matchgroup mg
                            WHERE mg.match_id = @match_id
                        )
                WHERE document_id = @document_id
                AND order_id = @order_id
            END
            ELSE
            BEGIN
                IF @order_id <= @total_order_id
                BEGIN
                    UPDATE #temp_trans
                    SET insurer_code = 
                            (
                                SELECT insurer_code
                                FROM #temp_trans
                                WHERE document_id = @document_id
                                AND order_id = @first_order_id
                            ),
                        insurer_amount = 
                            (
                                SELECT insurer_amount
                                FROM #temp_trans
                                WHERE document_id = @document_id
                                AND order_id = @first_order_id
                            ),
                        insurer_matched =
                            (
                                SELECT insurer_matched
                                FROM #temp_trans
                                WHERE document_id = @document_id
                                AND order_id = @first_order_id
                            ),
                        insurer_payment_document_ref =
                            (
                                SELECT MAX(ISNULL(dx.document_ref,'N/A'))
                                FROM transmatch tm
                                JOIN transdetail td
                                    ON td.transdetail_id = tm.transdetail_id
                                JOIN document d
                                    ON d.document_id = td.document_id
                                LEFT JOIN allocationdetail ad
                                        JOIN cashlistitem cli
                                            ON cli.cashlistitem_id = ad.cashlistitem_id
                                        JOIN transdetail tdx
                                            ON tdx.transdetail_id = cli.transdetail_id
                                        JOIN document dx
                                            ON dx.document_id = tdx.document_id
                                    ON ad.allocationdetail_id = tm.allocationdetail_id
                                    AND ad.transdetail_id = tm.transdetail_id
                                WHERE tm.match_id = @match_id
                                AND d.document_id = @document_id
                            ),
                        insurer_payment_amount =
                            (
                                SELECT SUM(tm.base_match_amount)
                                FROM transmatch tm
                                JOIN transdetail td
                                    ON td.transdetail_id = tm.transdetail_id
                                JOIN document d
                                    ON d.document_id = td.document_id
                                WHERE tm.match_id = @match_id
                                AND d.document_id = @document_id
                            ),
                        insurer_payment_settled_date =
                            (
                                SELECT mg.match_date
                                FROM matchgroup mg
                                WHERE mg.match_id = @match_id
                            )
                    WHERE document_id = @document_id
                    AND order_id = @order_id
                END
                ELSE
                BEGIN
                    INSERT INTO #temp_trans
                    (
                        document_id,
                        order_id,
                        debit_credit,
                        document_ref,
                        insurance_ref,
                        document_date,
                        commission_amount,
                        insurer_code,
                        insurer_amount,
                        insurer_matched,
                        insurer_payment_document_ref,
                        insurer_payment_amount,
                        insurer_payment_settled_date
                    )
                    SELECT
                        document_id,
                        @order_id,
                        debit_credit,
                        document_ref,
                        insurance_ref,
                        document_date,
                        commission_amount,
                        insurer_code,
                        insurer_amount,
                        insurer_matched,
                        (
                            SELECT MAX(ISNULL(dx.document_ref,'N/A'))
                            FROM transmatch tm
                            JOIN transdetail td
                                ON td.transdetail_id = tm.transdetail_id
                            JOIN document d
                                ON d.document_id = td.document_id
                            LEFT JOIN allocationdetail ad
                                    JOIN cashlistitem cli
                                        ON cli.cashlistitem_id = ad.cashlistitem_id
                                    JOIN transdetail tdx
                                        ON tdx.transdetail_id = cli.transdetail_id
                                    JOIN document dx
                                        ON dx.document_id = tdx.document_id
                                ON ad.allocationdetail_id = tm.allocationdetail_id
                                AND ad.transdetail_id = tm.transdetail_id
                            WHERE tm.match_id = @match_id
                            AND d.document_id = @document_id
                        ),
                        (
                            SELECT SUM(tm.base_match_amount)
                            FROM transmatch tm
                            JOIN transdetail td
                                ON td.transdetail_id = tm.transdetail_id
                            JOIN document d
                                ON d.document_id = td.document_id
                            WHERE tm.match_id = @match_id
                            AND d.document_id = @document_id
                        ),
                        (
                            SELECT mg.match_date
                            FROM matchgroup mg
                            WHERE mg.match_id = @match_id
                        )
                    FROM #temp_trans
                    WHERE document_id = @document_id
                    AND order_id = @first_order_id
                END
            END
            
            SELECT @order_id = @order_id + 1

            FETCH NEXT FROM c_payment INTO @match_id
        END

        CLOSE c_payment
        DEALLOCATE c_payment

        IF @order_id = @first_order_id 
        BEGIN
            SELECT @order_id = @order_id + 1
        END

        FETCH NEXT FROM c_insurer INTO @account_id, @account_order_id
    END

    CLOSE c_insurer
    DEALLOCATE c_insurer
    
    
    FETCH NEXT FROM c_transaction INTO @document_id
END

CLOSE c_transaction
DEALLOCATE c_transaction

/*Remove any lines where the insurer/agent amounts have not been paid*/
DELETE #temp_trans
FROM #temp_trans tt
WHERE NOT EXISTS
    (
        SELECT
            NULL
        FROM #temp_trans
        WHERE document_id = tt.document_id
        AND ISNULL(insurer_matched, 0) <> 0
    )

/*Remove any lines where the total client/subagent amounts show as fully paid*/
DELETE #temp_trans
FROM #temp_trans tt
WHERE NOT EXISTS
    (
        SELECT
            NULL
        FROM #temp_trans
        WHERE document_id = tt.document_id
        AND ISNULL(client_amount, 0) - ISNULL(client_matched, 0) <> 0
    )

/*Calculate days outstanding using the end date of this report*/
UPDATE #temp_trans
SET insurer_payment_days_os = DATEDIFF(dd, insurer_payment_settled_date, @end_date)

UPDATE tt
SET max_days_os = 
        (
            SELECT ISNULL(MAX(ISNULL(insurer_payment_days_os,0)),0)
            FROM #temp_trans
            WHERE document_id = tt.document_id
        )
FROM #temp_trans tt

SELECT *
FROM #temp_trans
ORDER BY 
    document_id,
    order_id

DROP TABLE #temp_trans

 
GO