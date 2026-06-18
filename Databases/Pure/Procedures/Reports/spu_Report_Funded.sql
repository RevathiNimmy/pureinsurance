SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Funded'
GO

CREATE PROCEDURE spu_Report_Funded
    @branch_id INT,
    @end_date DATETIME
AS

DECLARE @document_id INT
DECLARE @account_id INT
DECLARE @match_id INT
DECLARE @order_id INT
DECLARE @first_order_id INT
DECLARE @total_order_id INT
DECLARE @account_order_id INT
DECLARE @current_date DATETIME

DECLARE @transdetail_id INT
DECLARE @amount MONEY
DECLARE @InterestRate MONEY
DECLARE @pfrf_id INT
DECLARE @DepositPC MONEY
DECLARE @client_amount MONEY
DECLARE @client_payment_amount MONEY

DECLARE @main_document_id INT
DECLARE @instalment_document_id INT
DECLARE @deposit_document_id INT

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT @current_date = GETDATE()

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
    
    fsa_disabled BIT,
    is_premium_finance BIT,
    is_third_party BIT,
    is_subagent BIT
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

CREATE TABLE #instalments
(
    document_id INT,
    third_party BIT,
    instalment_document_id INT,
    deposit_document_id INT
)
CREATE INDEX I__instalments__document_id ON #instalments (document_id)

INSERT INTO #instalments
SELECT DISTINCT
    d.document_id,
    CASE d2.documenttype_id
        WHEN 1 THEN 1
        ELSE 0
    END,
    0,
    0
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
    AND tm.allocationdetail_id IS NOT NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tm2
    ON tm2.match_id = mg.match_id
    AND tm2.is_reversed IS NULL
    AND tm2.allocationdetail_id IS NOT NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
WHERE d.documenttype_id IN (4,5,15,16,17,18,31,32,35,36,28,29) /*SND,SNC,SRD,SRC,SED,SEC,SHD,SHC,TRD,TRC,CLP,CLR*/
AND d.document_date <= @end_date
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND (
        (
            /*Third Party Instalments*/
            d2.documenttype_id = 1
            AND 
            RTRIM(d2.comment) = 'Premium Finance Transfer'
        )
        OR
        (
            /*In-House Instalments*/
            d2.documenttype_id = 43
        )
    )

/*Get transactions that need to be paid off for in-house instalments*/
UPDATE i
SET i.instalment_document_id = td5.document_id,
    i.deposit_document_id = td7.document_id
FROM #instalments i
JOIN transdetail td
    ON td.document_id = i.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
    AND tm.allocationdetail_id IS NOT NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tm2
    ON tm2.match_id = mg.match_id
    AND tm2.transmatch_id <> tm.transmatch_id
    AND tm2.is_reversed IS NULL
    AND tm2.allocationdetail_id IS NOT NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
    AND d2.documenttype_id = 43
JOIN transdetail td3
    ON td3.document_id = d2.document_id
    AND td3.transdetail_id <> td2.transdetail_id
JOIN transmatch tm3
    ON tm3.transdetail_id = td3.transdetail_id
    AND tm3.is_reversed IS NULL
    AND tm3.allocationdetail_id IS NOT NULL
JOIN matchgroup mg3
    ON mg3.match_id = tm3.match_id
    AND mg3.match_date <= @end_date
/*Get the IND transaction*/
JOIN transmatch tm4
    ON tm4.match_id = mg3.match_id
    AND tm4.transmatch_id <> tm3.transmatch_id
    AND tm4.is_reversed IS NULL
    AND tm4.allocationdetail_id IS NOT NULL
JOIN transdetail td4
    ON td4.transdetail_id = tm4.transdetail_id
JOIN document d4
    ON d4.document_id = td4.document_id
    AND d4.documenttype_id = 37
JOIN transdetail td5
    ON td5.document_id = d4.document_id
    AND td5.transdetail_id <> td4.transdetail_id
JOIN account a5
    ON a5.account_id = td5.account_id
JOIN ledger l5
    ON l5.ledger_id = a5.ledger_id
    AND l5.ledger_short_name = 'SA'
/*Get the deposit JN transaction, if there is one*/
LEFT JOIN transmatch tm6
        JOIN transdetail td6
            ON td6.transdetail_id = tm6.transdetail_id
        JOIN document d6
            ON d6.document_id = td6.document_id
            AND d6.documenttype_id = 1
        JOIN transdetail td7
            ON td7.document_id = d6.document_id
            AND td7.transdetail_id <> td6.transdetail_id
    ON tm6.match_id = mg.match_id
    AND tm6.transmatch_id <> tm.transmatch_id
    AND tm6.transmatch_id <> tm2.transmatch_id
    AND tm6.is_reversed IS NULL
    AND tm6.allocationdetail_id IS NOT NULL
WHERE i.third_party = 0

/*Get transactions that need to be paid off for third party instalments*/
UPDATE i
SET i.instalment_document_id = td5.document_id,
    i.deposit_document_id = td7.document_id
FROM #instalments i
JOIN transdetail td
    ON td.document_id = i.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
    AND tm.allocationdetail_id IS NOT NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tm2
    ON tm2.match_id = mg.match_id
    AND tm2.transmatch_id <> tm.transmatch_id
    AND tm2.is_reversed IS NULL
    AND tm2.allocationdetail_id IS NOT NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
    AND d2.documenttype_id = 1
JOIN transdetail td3
    ON td3.document_id = d2.document_id
    AND td3.transdetail_id <> td2.transdetail_id
JOIN transmatch tm3
    ON tm3.transdetail_id = td3.transdetail_id
    AND tm3.is_reversed IS NULL
    AND tm3.allocationdetail_id IS NOT NULL
JOIN matchgroup mg3
    ON mg3.match_id = tm3.match_id
    AND mg3.match_date <= @end_date
/*Get the main JN transaction*/
JOIN transmatch tm4
    ON tm4.match_id = mg3.match_id
    AND tm4.transmatch_id <> tm3.transmatch_id
    AND tm4.is_reversed IS NULL
    AND tm4.allocationdetail_id IS NOT NULL
    AND tm4.transmatch_id IN
        (
            SELECT
                MAX(transmatch_id)
            FROM transmatch
            WHERE match_id = mg3.match_id
            AND transmatch_id <> tm3.transmatch_id
            AND is_reversed IS NULL
            AND allocationdetail_id IS NOT NULL
        )
JOIN transdetail td4
    ON td4.transdetail_id = tm4.transdetail_id
JOIN document d4
    ON d4.document_id = td4.document_id
    AND d4.documenttype_id = 1
JOIN transdetail td5
    ON td5.document_id = d4.document_id
    AND td5.transdetail_id <> td4.transdetail_id
/*Get the deposit JN transaction*/
LEFT JOIN transmatch tm6
        JOIN transdetail td6
            ON td6.transdetail_id = tm6.transdetail_id
        JOIN document d6
            ON d6.document_id = td6.document_id
            AND d6.documenttype_id = 1
        JOIN transdetail td7
            ON td7.document_id = d6.document_id
            AND td7.transdetail_id <> td6.transdetail_id
    ON tm6.match_id = mg3.match_id
    AND tm6.transmatch_id <> tm3.transmatch_id
    AND tm6.transmatch_id <> tm4.transmatch_id
    AND tm6.is_reversed IS NULL
    AND tm6.allocationdetail_id IS NOT NULL
WHERE i.third_party = 1


/*Add funded transactions to the temporary table*/
INSERT INTO #temp_trans
(
    document_id,
    order_id,
    debit_credit,
    document_ref,
    insurance_ref,
    document_date,
    commission_amount,
    is_premium_finance,
    is_third_party
)
SELECT
    d.document_id,
    1,
    
    CASE
        WHEN EXISTS
                (
                    /*Debit transactions only (SNCs can be debits if they are reversals)*/
                    SELECT
                        NULL
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name IN ('UB', 'SA')
                    WHERE td.document_id = d.document_id
                    HAVING SUM(td.amount) > 0
                ) THEN 1 /*Debit*/
        ELSE 2 /*Credit*/
    END,
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
    ),
    (
        SELECT
            TOP 1 1
        FROM #instalments
        WHERE document_id = d.document_id
    ),
    (
        SELECT
            TOP 1 third_party
        FROM #instalments
        WHERE document_id = d.document_id
    )
FROM document d
LEFT JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt
WHERE d.documenttype_id IN (4,5,15,16,17,18,31,32,35,36,28,29) /*SND,SNC,SRD,SRC,SED,SEC,SHD,SHC,TRD,TRC,CLP,CLR*/
AND d.document_date <= @end_date
AND d.company_id = ISNULL(@branch_id,d.company_id)
AND 
    (
        ( /*Broker is in debt after all of these payments/receipts (don't include the client amount if this is a instalment*/
            SELECT
                ISNULL(SUM(ISNULL(tm.base_match_amount,0)),0)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.is_reversed IS NULL
                AND tm.allocationdetail_id IS NOT NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
            JOIN document dx 
                ON td.document_id = dx.document_id
            WHERE td.document_id = d.document_id
            AND EXISTS
                (
                    /*Debit transactions only (SNCs can be debits if they are reversals)*/
                    SELECT
                        NULL
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name IN ('UB', 'SA')
                    WHERE td.document_id = d.document_id
                    HAVING SUM(td.amount) > 0
                )
            AND
            (
                l.ledger_short_name IN ('IN', 'AG', 'UB')
                OR
                (
                    l.ledger_short_name = 'SA'
                    AND
                    NOT EXISTS
                        (
                            SELECT
                                NULL
                            FROM #instalments
                            WHERE document_id = d.document_id
                        )
                )
            )
        )
    +
        ( /*This section reports on Credit amount for Insurer and Third Parties and not on client amounts*/
            SELECT
                ISNULL(SUM(ISNULL(tm.base_match_amount,0)),0)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.is_reversed IS NULL
                AND tm.allocationdetail_id IS NOT NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
            JOIN document dx
                ON td.document_id = dx.document_id
            WHERE td.document_id = d.document_id
            AND EXISTS
                (
                    /*Credit transactions only (SNDs can be credits if they are reversals)*/
                    SELECT
                        NULL
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name IN ('UB', 'SA')
                    WHERE td.document_id = d.document_id
                    HAVING SUM(td.amount) < 0
                )
            AND
            (
                l.ledger_short_name IN ('IN', 'AG', 'UB')
            )
        )
    +
        ( /*This section reports on Credits and Client amount*/
            SELECT
                ISNULL(SUM(ISNULL(tm.base_match_amount,0)),0)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.is_reversed IS NULL
                AND tm.allocationdetail_id IS NOT NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
            JOIN document dx    
                ON td.document_id = dx.document_id
            WHERE td.document_id = d.document_id
            AND EXISTS
                (
                    /*Credit transactions only (SNDs can be credits if they are reversals)*/
                    SELECT
                        NULL
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name IN ('UB', 'SA')
                    WHERE td.document_id = d.document_id
                    HAVING SUM(td.amount) < 0
                )
            AND
            (
                l.ledger_short_name = 'SA'
                AND
                NOT EXISTS
                    (
                        SELECT
                            NULL
                        FROM #instalments
                        WHERE document_id = d.document_id
                    )
            )
            AND     
            (
                EXISTS
                (
                    SELECT 
                        NULL
                    FROM transmatch tm2
                    JOIN transdetail td2
                        ON td2.transdetail_id = tm2.transdetail_id
                    JOIN document d2
                        ON d2.document_id = td2.document_id
                        AND d2.documenttype_id = 23 /*SPY*/
                    WHERE tm2.match_id = tm.match_id
                    AND tm2.transdetail_id <> tm.transdetail_id
                    AND tm2.is_reversed IS NULL
                    AND tm2.allocationdetail_id IS NOT NULL
                )   
            )
        )
        +
        ( /*Get the allocated client amount from the instalment and deposit*/
            SELECT 
                ISNULL(SUM(ISNULL(tm.base_match_amount,0)),0)
            FROM #instalments i
            JOIN transdetail td
                ON (td.document_id = i.instalment_document_id OR td.document_id = i.deposit_document_id)
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
                AND tm.is_reversed IS NULL
                AND tm.allocationdetail_id IS NOT NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
            WHERE i.document_id = d.document_id
            AND l.ledger_short_name IN ('SA', 'RF')
            AND a.short_code NOT LIKE 'ISUSP%'
        )
        +
        ( /*And can't pay for it from the money he owes*/
            SELECT 
                ISNULL(SUM(td.amount),0)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            WHERE td.document_id = d.document_id
            AND l.ledger_short_name NOT IN ('SA','IN','AG','UB')
            AND (
                    SELECT 
                        ISNULL(SUM(td.amount),0)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                    WHERE td.document_id = d.document_id
                    AND l.ledger_short_name NOT IN ('SA','IN','AG','UB')
                ) > 0
        )       
    ) < 0 

/*Remove Direct Debits as these should all be fine.*/
DELETE #temp_trans
FROM #temp_trans tt
JOIN transdetail td
    ON td.document_id = tt.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
    AND tm.allocationdetail_id IS NOT NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tmx
    ON tmx.match_id = tm.match_id
    AND tmx.is_reversed IS NULL
    AND tmx.allocationdetail_id IS NOT NULL
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
                WHEN 'RF' THEN 3
            END
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE td.document_id = @document_id
        AND l.ledger_short_name IN ('SA', 'UB')
        UNION
        SELECT DISTINCT 
            a.account_id,
            CASE l.ledger_short_name
                WHEN 'SA' THEN 1
                WHEN 'UB' THEN 2
                WHEN 'RF' THEN 3
            END
        FROM #instalments i
        JOIN transdetail td
            ON (td.document_id = i.instalment_document_id OR td.document_id = i.deposit_document_id)
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE i.document_id = @document_id
        AND l.ledger_short_name = 'RF'
        AND a.short_code NOT LIKE 'ISUSP%'
            
        ORDER BY 
            CASE l.ledger_short_name
                WHEN 'SA' THEN 1
                WHEN 'UB' THEN 2
                WHEN 'RF' THEN 3
            END

    OPEN c_client

    FETCH NEXT FROM c_client INTO @account_id, @account_order_id

    WHILE @@FETCH_STATUS = 0
    BEGIN
        
        /*If this transaction is being paid by premium finance then use the amounts on the instalment transactions for client and premium finance providers*/
        IF EXISTS
            (
                SELECT
                    NULL
                FROM #instalments
                WHERE document_id = @document_id
            )
        BEGIN
        
            SELECT
                @main_document_id = NULL,
                @instalment_document_id = instalment_document_id,
                @deposit_document_id = deposit_document_id
            FROM #instalments
            WHERE document_id = @document_id

        END
        ELSE
        BEGIN
            
            SELECT 
                @main_document_id = @document_id,
                @instalment_document_id = NULL,
                @deposit_document_id = NULL
        END

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
                        WHERE td.account_id = @account_id
                        AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                    ),
                client_matched = 
                    (
                        SELECT ISNULL(SUM(tm.base_match_amount),0)
                        FROM transdetail td
                        JOIN transmatch tm
                            ON tm.transdetail_id = td.transdetail_id
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
                        JOIN matchgroup mg
                            ON mg.match_id = tm.match_id
                            AND mg.match_date <= @end_date
                        WHERE td.account_id = @account_id
                        AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                    )
            WHERE document_id = @document_id
            AND order_id = @order_id
      
      
              IF EXISTS
                  (
                      SELECT
                          NULL
                      FROM #instalments
                      WHERE document_id = @document_id
                  )
              BEGIN
      
            SELECT 
            @transdetail_id=transdetail_id,
            @amount=ISNULL(amount,0)
            FROM Transdetail
            WHERE document_id=@document_id
            AND account_id=@account_id
      
            SELECT 
            @InterestRate=ISNULL(InterestRate,0),
            @pfrf_id=pfrf_id
            FROM pfpremiumfinance
            WHERE pfprem_finance_cnt=
                                    (
                                    SELECT DISTINCT
                                    pfprem_finance_cnt 
                                    FROM pftransaction_id
                                    WHERE pftransaction_id=@transdetail_id
                                    )
			AND pfprem_finance_version=
						(SELECT MAX(pfprem_finance_version) 
						FROM pfpremiumfinance
						WHERE pfprem_finance_cnt =  
									(    
									SELECT DISTINCT  
									pfprem_finance_cnt    
									FROM pftransaction_id    
									WHERE pftransaction_id=@transdetail_id   
									)) 

            /*31582 */
            SELECT @InterestRate = ISNULL((SELECT
        CASE WHEN is_premium_finance = 1 AND is_third_party = 1 THEN
        0
        END
        FROM #temp_trans 
        WHERE document_id = @document_id), @InterestRate)

            SELECT @DepositPC=ISNULL(DepositPC,0)
            FROM pfrf
            WHERE pfrf_id=@pfrf_id
      
            IF @amount<=0
            BEGIN 
                SET @client_amount=0
            END
            ELSE
            BEGIN
                IF @DepositPC<>0 AND @InterestRate<>0
                BEGIN
                    SET @client_amount = @amount+((@amount-(@amount*@DepositPC)/100)*@InterestRate)/100
                END
      
                IF @DepositPC=0 AND @InterestRate=0
                BEGIN
                    SET @client_amount = @amount
                END
      
                IF @DepositPC=0 AND @InterestRate<>0
                BEGIN
                    SET @client_amount = @amount+(@amount*@InterestRate)/100
                END
      
                IF @DepositPC<>0 AND @InterestRate=0
                BEGIN
                    SET @client_amount = @amount-((@amount*@DepositPC)/100)
                END
            END
                  
            UPDATE #temp_trans
            SET client_amount = @client_amount
            WHERE document_id = @document_id
            AND order_id = @order_id
      
        END
      
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
                is_premium_finance,
                is_third_party
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
                    WHERE td.account_id = @account_id
                    AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                ),
                (
                    SELECT ISNULL(SUM(tm.base_match_amount),0)
                    FROM transdetail td
                    JOIN transmatch tm
                        ON tm.transdetail_id = td.transdetail_id
                        AND tm.is_reversed IS NULL
                        AND tm.allocationdetail_id IS NOT NULL
                    JOIN matchgroup mg
                        ON mg.match_id = tm.match_id
                        AND mg.match_date <= @end_date
                    WHERE td.account_id = @account_id
                    AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                ),
                is_premium_finance,
                is_third_party
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
                AND tm.is_reversed IS NULL
                AND tm.allocationdetail_id IS NOT NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
            WHERE td.account_id = @account_id            
            AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)

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
                            FROM transdetail td
                            JOIN transmatch tm
                                ON tm.transdetail_id = td.transdetail_id
                                AND tm.is_reversed IS NULL
                                AND tm.allocationdetail_id IS NOT NULL
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
                            AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                        ),
                    client_payment_amount =
                        (
                            SELECT SUM(tm.base_match_amount)
                            FROM transdetail td
                            JOIN transmatch tm
                                ON tm.transdetail_id = td.transdetail_id
                                AND tm.is_reversed IS NULL
                                AND tm.allocationdetail_id IS NOT NULL
                            WHERE tm.match_id = @match_id
                            AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
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
                    client_payment_settled_date,
                    is_premium_finance,
                    is_third_party
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
                        SELECT ISNULL(MAX(ISNULL(dx.document_ref, 'N/A')), 'N/A')
                        FROM transdetail td
                        JOIN transmatch tm
                            ON tm.transdetail_id = td.transdetail_id
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
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
                        AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                    ),
                    (
                        SELECT SUM(tm.base_match_amount)
                        FROM transdetail td
                        JOIN transmatch tm
                            ON tm.transdetail_id = td.transdetail_id
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
                        WHERE tm.match_id = @match_id
                        AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                    ),
                    (
                        SELECT mg.match_date
                        FROM matchgroup mg
                        WHERE mg.match_id = @match_id
                    ),
                    is_premium_finance,
                    is_third_party
                FROM #temp_trans
                WHERE document_id = @document_id
                AND order_id = @first_order_id

            END
            
            /*If a payment or receipt did not allocate this transaction and it was matched to a single item,*/
            /*then show the document ref of that item. (Shows INC transactions when paying instalments)*/
            IF EXISTS
                (
                    SELECT
                        NULL
                    FROM #temp_trans
                    WHERE document_id = @document_id
                    AND order_id = @order_id
                    AND client_payment_document_ref = 'N/A'
                )
            BEGIN
                UPDATE #temp_trans
                SET client_payment_document_ref =
                    (
                        SELECT 
                            ISNULL(MAX(ISNULL(d2.document_ref, 'N/A')), 'N/A')
                        FROM transdetail td
                        JOIN transmatch tm
                            ON tm.transdetail_id = td.transdetail_id
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
                        JOIN transmatch tm2
                            ON tm2.match_id = tm.match_id
                            AND tm2.transmatch_id <> tm.transmatch_id
                            AND tm2.is_reversed IS NULL
                            AND tm2.allocationdetail_id IS NOT NULL
                        JOIN transdetail td2
                            ON td2.transdetail_id = tm2.transdetail_id
                        JOIN document d2
                            ON d2.document_id = td2.document_id
                        WHERE tm.match_id = @match_id
                        AND (
                                SELECT
                                    SUM(1)
                                FROM transmatch
                                WHERE match_id = @match_id
                                AND is_reversed IS NULL
                                AND allocationdetail_id IS NOT NULL
                            ) = 2
                        AND td.document_id IN (@main_document_id, @instalment_document_id, @deposit_document_id)
                    )
                WHERE document_id = @document_id
                AND order_id = @order_id
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

    /*CHANGE THIS TO RESTRICT SUBAGENT*/
    DECLARE c_insurer CURSOR FAST_FORWARD FOR
        SELECT DISTINCT 
            a.account_id,
            CASE l.ledger_short_name
                WHEN 'IN' THEN 1
                WHEN 'AG' THEN 2
                WHEN 'UB' THEN 3
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
                WHEN 'UB' THEN 3
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
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
                        JOIN matchgroup mg
                            ON mg.match_id = tm.match_id
                            AND mg.match_date <= @end_date
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
                insurer_matched,
                is_premium_finance,
                is_third_party
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
                        AND tm.is_reversed IS NULL
                        AND tm.allocationdetail_id IS NOT NULL
                    JOIN matchgroup mg
                        ON mg.match_id = tm.match_id
                        AND mg.match_date <= @end_date
                    WHERE td.document_id = @document_id
                    AND td.account_id = @account_id
                ),
                is_premium_finance,
                is_third_party
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
                AND tm.is_reversed IS NULL
                AND tm.allocationdetail_id IS NOT NULL
            JOIN matchgroup mg
                ON mg.match_id = tm.match_id
                AND mg.match_date <= @end_date
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
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
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
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
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
                                AND tm.is_reversed IS NULL
                                AND tm.allocationdetail_id IS NOT NULL
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
                                AND tm.is_reversed IS NULL
                                AND tm.allocationdetail_id IS NOT NULL
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
                        insurer_payment_settled_date,
                        is_premium_finance,
                        is_third_party
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
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
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
                            AND tm.is_reversed IS NULL
                            AND tm.allocationdetail_id IS NOT NULL
                        ),
                        (
                            SELECT mg.match_date
                            FROM matchgroup mg
                            WHERE mg.match_id = @match_id
                        ),
                        is_premium_finance,
                        is_third_party
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

UPDATE #temp_trans 
SET is_subagent = 0
 
UPDATE ttx 
SET ttx.is_subagent = 1 
FROM #temp_trans ttx
WHERE EXISTS
    (
        SELECT 
            NULL 
        FROM party p 
        JOIN #temp_trans tt 
            ON p.shortname = tt.client_code
        JOIN party_agent pa 
            ON p.party_cnt = pa.party_cnt
        JOIN party_agent_type pta 
            ON pa.party_agent_type_id = pta.party_agent_type_id 
            AND pta.description = 'SUB AGENT'
        WHERE tt.document_id = ttx.document_id
    )


UPDATE #temp_trans
SET insurer_payment_days_os = DATEDIFF(dd,insurer_payment_settled_date,@current_date)
WHERE debit_credit = 1

UPDATE tt
SET max_days_os = 
        (
            SELECT 
                ISNULL(MAX(ISNULL(insurer_payment_days_os,0)),0)
            FROM #temp_trans
            WHERE document_id = tt.document_id
        )
FROM #temp_trans tt
WHERE debit_credit = 1


UPDATE #temp_trans
SET client_payment_days_os = DATEDIFF(dd,client_payment_settled_date,@current_date)
WHERE debit_credit = 2

UPDATE tt
SET max_days_os = 
        (
            SELECT 
                ISNULL(MAX(ISNULL(client_payment_days_os,0)),0)
            FROM #temp_trans
            WHERE document_id = tt.document_id
        )
FROM #temp_trans tt
WHERE debit_credit = 2

/*31582 Delete finance provider transactions */
DELETE 
FROM #temp_trans 
WHERE client_code 
IN(SELECT shortname FROM party WHERE party_type_id = 14)

SELECT *
FROM #temp_trans
ORDER BY 
    debit_credit,
    document_id,
    order_id

DROP TABLE #temp_trans
DROP TABLE #instalments

GO


