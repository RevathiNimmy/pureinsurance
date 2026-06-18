SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Commission_Reconciliation'
GO

CREATE PROCEDURE spu_Report_Commission_Reconciliation
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

DECLARE
    @payment_document_id INT, 
    @client_document_id INT, 
    @current_payment_document_id INT, 
    @current_client_document_id INT, 
    @ledger_short_name VARCHAR(2), 
    @match_id INT,
    @full_amount MONEY,
    @this_amount MONEY,
    @ClientMatchAmount MONEY,
    @ClientAlreadyMatchedAmount MONEY,
    @ClientPremiumAmount MONEY,
    @ClientFeeAmount MONEY,
    @ClientCommissionAmount MONEY,
    @ClientOutstandingLessFeeAmount MONEY,
    @ClientPremiumToCommissionRate FLOAT,
    @ClientTotalFeeAmount MONEY,
    @ClientTotalCommissionAmount MONEY,
    @SubAgent BIT,
    @DirectToInsurer BIT,
    @payment_ledger_short_name VARCHAR(2),
    @payment_account_id INT,
    @DI_document_id INT,
    @RemoveTransaction BIT

/*Validate input parameters*/
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT @current_payment_document_id = 0
SELECT @current_client_document_id = 0

/*Create temporary table to store reconciled payments and receipts*/
CREATE TABLE #Payments
(
    payment_document_id INT,
    payment_document_ref VARCHAR(25),
    bank_transdetail_id INT
)

/*Create temporary table to store report info*/
CREATE TABLE #Report_Lines
(
    payment_document_id INT,
    payment_document_ref VARCHAR(25),
    client_document_id INT,
    client_document_ref VARCHAR(25),
    ledger_short_name VARCHAR(2),
    account_code VARCHAR(30),
    account_name VARCHAR(60),
    match_id INT,
    full_amount MONEY,
    this_amount MONEY,
    fsa_disabled BIT,
    remove_transaction BIT
)

IF NOT EXISTS 
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN

    INSERT INTO #Report_Lines
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )

    SELECT
        payment_document_ref,
        client_document_ref,
        ledger_short_name,
        account_code,
        account_name,
        full_amount,
        this_amount,
        fsa_disabled
    FROM #Report_Lines
    
    DROP TABLE #Report_Lines

    RETURN
END

/*Get all reconciled payment and receipts for the period selected*/
INSERT INTO #Payments
SELECT 
    D.document_id, 
    D.document_ref,
    BTD.transdetail_id
FROM document D
JOIN transdetail BTD
    ON BTD.document_id = D.document_id
JOIN bankaccount BA
    ON BA.account_id = BTD.account_id
WHERE D.documenttype_id IN (22, 23, 43)
AND D.document_date BETWEEN @start_date AND @end_date 
AND D.company_id = ISNULL(@branch_id,D.company_id)
AND BTD.spare = 'RECONCILED'
ORDER BY
    D.document_id

/*Get all the transactions that have been allocated by the payments and receipts*/
INSERT INTO #Report_Lines
SELECT 
    P.payment_document_id, 
    P.payment_document_ref, 
    DC.document_id, 
    DC.document_ref, 
    LALL.ledger_short_name,
    AALL.short_code,
    AALL.account_name,
    TMC.match_id,
    SUM(TDALL.amount),
    0,
    NULL,
    NULL
FROM #Payments P
JOIN transdetail TD
    ON TD.document_id = P.payment_document_id
    AND TD.transdetail_id <> P.bank_transdetail_id
JOIN transmatch TMB /*Transmatch for receipt/payment*/
    ON TMB.transdetail_id = TD.transdetail_id
JOIN transmatch TMC /*Transmatch for client*/
    ON TMC.match_id = TMB.match_id
    AND TMC.transdetail_id <> TMB.transdetail_id
JOIN transdetail TDC
    ON TDC.transdetail_id = TMC.transdetail_id
JOIN document DC
    ON DC.document_id = TDC.document_id
JOIN transdetail TDALL
    ON TDALL.document_id = DC.document_id
JOIN account AALL
    ON AALL.account_id = TDALL.account_id
JOIN ledger LALL
    ON LALL.ledger_id = AALL.ledger_id
    AND LALL.ledger_short_name IN ('CO','FE','DI')
GROUP BY 
    P.payment_document_id, 
    P.payment_document_ref, 
    DC.document_id, 
    DC.document_ref, 
    LALL.ledger_short_name,
    AALL.short_code,
    AALL.account_name,
    TMC.match_id
ORDER BY
    P.payment_document_id,
    DC.document_id

DROP TABLE #Payments

/*Get a list of all the report lines*/
DECLARE c_Trans CURSOR FORWARD_ONLY FOR
    SELECT 
        payment_document_id,
        client_document_id,
        ledger_short_name,
        match_id,
        full_amount
    FROM #Report_Lines

/*Open cursor*/
OPEN c_Trans

/*Get next report line*/
FETCH NEXT FROM c_Trans INTO @payment_document_id, @client_document_id, @ledger_short_name, @match_id, @full_amount

WHILE @@FETCH_STATUS = 0
BEGIN

    /*If this is a new client document then calculate documents values*/
    IF @current_payment_document_id <> @payment_document_id OR @current_client_document_id <> @client_document_id
    BEGIN
    
        /*Update the current document to the latest one*/
        SELECT @current_payment_document_id = @payment_document_id
        SELECT @current_client_document_id = @client_document_id
        
        /*Initialize variables*/
        SELECT @SubAgent = 0
        SELECT @DirectToInsurer = 0
        SELECT @DI_document_id = 0
        SELECT @RemoveTransaction = 0
        
        /*Does this transaction contain a subagent?*/
        SELECT
            @SubAgent = 1
        WHERE EXISTS  
            (   
                SELECT 
                    NULL
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id 
                JOIN ledger L
                    ON L.ledger_id = A.ledger_id 
                WHERE L.ledger_short_name = 'UB'
                AND TD.document_id = @client_document_id
            )
            
        /*Is this transaction direct to the insurer?*/
        SELECT 
            @DI_document_id = MIN(D2.document_id)
        FROM document D
        JOIN transdetail TD
            ON TD.document_id = D.document_id
        JOIN transmatch TM
            ON TM.transdetail_id = TD.transdetail_id
        JOIN transmatch TM2
            ON TM2.match_id = TM.match_id
            AND TM2.transdetail_id <> TM.transdetail_id
        JOIN transdetail TD2
            ON TD2.transdetail_id = TM2.transdetail_id
        JOIN document D2
            ON D2.document_id = TD2.document_id
        WHERE TD.document_id = @client_document_id
        AND TD.document_sequence = 1
        AND D2.documenttype_id IN (33, 34)
        AND TD2.spare = 'DIRECTDEBIT'
        AND TD2.company_id = D.company_id
        

        SELECT 
            @payment_account_id = MIN(A.account_id),
            @payment_ledger_short_name = MIN(L.ledger_short_name)
        FROM transmatch TM
        JOIN transdetail TD
            ON TD.transdetail_id = TM.transdetail_id
        JOIN account A
            ON A.account_id = TD.account_id 
        JOIN ledger L
            ON L.ledger_id = A.ledger_id 
        WHERE TM.match_id = @match_id
        
        
        IF @DI_document_id <> 0 AND @payment_ledger_short_name = 'IN'
        BEGIN
            SELECT @DirectToInsurer = 1
        END
        
        IF (@payment_ledger_short_name = 'IN' AND @DirectToInsurer = 0)
            OR (@payment_ledger_short_name = 'UB' AND @SubAgent = 1)
            OR (@payment_ledger_short_name = 'SA' AND (@DirectToInsurer = 1 OR @SubAgent = 1))
            OR (@payment_ledger_short_name NOT IN ('SA', 'UB', 'IN'))
        BEGIN
            SELECT @RemoveTransaction = 1
        END
        

        IF @RemoveTransaction = 0
        BEGIN
    
            /*Get the client amount that had already been matched off before this payment/receipt was allocated*/
            SELECT @ClientAlreadyMatchedAmount = ISNULL(SUM(TMALL.base_match_amount),0)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = @payment_ledger_short_name
            JOIN transmatch TM
                ON TM.transdetail_id = TD.transdetail_id 
            JOIN matchgroup MG
                ON MG.match_id = TM.match_id
                AND MG.match_id = @match_id
            JOIN transmatch TMALL
                ON TMALL.transdetail_id = TD.transdetail_id 
            JOIN matchgroup MGALL
                ON MGALL.match_id = TMALL.match_id
                AND 
                (
                    MGALL.match_date < MG.match_date
                    OR
                    (MGALL.match_date = MG.match_date AND MGALL.match_id < MG.match_id)
                )
            WHERE 
            (
                @DirectToInsurer = 0
                AND
                TD.document_id = @current_client_document_id
            )
            OR
            (
                @DirectToInsurer = 1
                AND
                A.account_id = @payment_account_id
                AND 
                (
                    TD.document_id = @current_client_document_id
                    OR
                    TD.document_id = @DI_document_id    
                )
            )

            /*Get the client amount that has been matched off against this payment/receipt*/
            SELECT @ClientMatchAmount = ISNULL(SUM(TM.base_match_amount),0)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = @payment_ledger_short_name
            JOIN transmatch TM
                ON TM.transdetail_id = TD.transdetail_id 
            JOIN matchgroup MG
                ON MG.match_id = TM.match_id
                AND MG.match_id = @match_id
            WHERE 
            (
                @DirectToInsurer = 0
                AND
                TD.document_id = @current_client_document_id
            )
            OR
            (
                @DirectToInsurer = 1
                AND
                A.account_id = @payment_account_id
                AND 
                (
                    TD.document_id = @current_client_document_id
                    OR
                    TD.document_id = @DI_document_id    
                )
            )

            /*Get the clients total premium amount for this transaction*/
            SELECT @ClientPremiumAmount = ISNULL(SUM(TD.amount),0)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = @payment_ledger_short_name
            WHERE 
            (
                @DirectToInsurer = 0
                AND
                TD.document_id = @current_client_document_id
            )
            OR
            (
                @DirectToInsurer = 1
                AND
                A.account_id = @payment_account_id
                AND 
                (
                    TD.document_id = @current_client_document_id
                    OR
                    TD.document_id = @DI_document_id    
                )
            )

            /*Get the fee amount for this transaction*/
            SELECT @ClientFeeAmount = ISNULL(SUM(TDFEE.amount),0) * -1
            FROM transdetail TDFEE
            JOIN account A
                ON A.account_id = TDFEE.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = 'FE'
            WHERE TDFEE.document_id = @current_client_document_id

            /*Get the commission amount for this transaction*/
            SELECT @ClientCommissionAmount = ISNULL(SUM(TDCOMM.amount),0)
            FROM transdetail TDCOMM
            JOIN account A
                ON A.account_id = TDCOMM.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = 'CO'
            WHERE TDCOMM.document_id = @current_client_document_id

            /*Calculate client outstanding amount without the fee added on*/
            IF @ClientPremiumAmount < 0
            BEGIN
                /*Payment, so don't do anything with fee*/
                SELECT @ClientOutstandingLessFeeAmount = (@ClientPremiumAmount - @ClientAlreadyMatchedAmount)
            END
            ELSE
            BEGIN
                /*Receipt, so remove fee amount*/
                SELECT @ClientOutstandingLessFeeAmount = (@ClientPremiumAmount - @ClientAlreadyMatchedAmount) - @ClientFeeAmount
            END

            /*If only fee left to be paid then set outstanding amount to zero*/
            IF (@ClientOutstandingLessFeeAmount < 0 AND @ClientPremiumAmount > 0)
                OR (@ClientOutstandingLessFeeAmount > 0 AND @ClientPremiumAmount < 0)
            BEGIN
                SELECT @ClientOutstandingLessFeeAmount = 0
            END

            /*Calculate the rate to work out amount of commission for part of the total premium*/
            IF @DirectToInsurer = 1
            BEGIN
                SELECT @ClientTotalFeeAmount = 0
                SELECT @ClientTotalCommissionAmount = @ClientMatchAmount * -1
                SELECT @full_amount = 0
            END
            ELSE
            BEGIN
                IF @ClientPremiumAmount < 0
                BEGIN
                    /*Payment, so don't do anything with fee*/
                    IF @ClientPremiumAmount = 0
                    BEGIN
                        SELECT @ClientPremiumToCommissionRate = 0
                    END
                    ELSE
                    BEGIN
                        SELECT @ClientPremiumToCommissionRate = CAST(@ClientCommissionAmount AS FLOAT) / CAST(@ClientPremiumAmount AS FLOAT)
                    END
                END
                ELSE
                BEGIN
                    /*Receipt, so remove fee amount*/
                    IF @ClientPremiumAmount - @ClientFeeAmount = 0
                    BEGIN
                        SELECT @ClientPremiumToCommissionRate = 0
                    END
                    ELSE
                    BEGIN
                        SELECT @ClientPremiumToCommissionRate = CAST(@ClientCommissionAmount AS FLOAT) / CAST((@ClientPremiumAmount - @ClientFeeAmount) AS FLOAT)
                    END     
                END


                /*If client amount outstanding, before the fee is added, is greater than this payment/receipt*/
                IF ABS(@ClientOutstandingLessFeeAmount) > ABS(@ClientMatchAmount)
                BEGIN
                    /*Then, as fee is paid last, use full match amount to calculate commission*/
                    SELECT @ClientTotalFeeAmount = 0
                    SELECT @ClientTotalCommissionAmount = @ClientMatchAmount * @ClientPremiumToCommissionRate
                END 
                ELSE
                BEGIN
                    /*Else, use client amount outstanding, before the fee is added, to calculate commision*/
                    /*and whatever is left of the match goes towards the fee*/
                    IF @ClientPremiumAmount < 0
                    BEGIN
                        /*Payment*/
                        SELECT @ClientTotalFeeAmount = @ClientFeeAmount
                    END
                    ELSE
                    BEGIN
                        /*Receipt*/
                        SELECT @ClientTotalFeeAmount = (@ClientMatchAmount - @ClientOutstandingLessFeeAmount)
                    END
                    SELECT @ClientTotalCommissionAmount = @ClientOutstandingLessFeeAmount * @ClientPremiumToCommissionRate
                END     
            END
        END
    END

    IF @RemoveTransaction = 0
    BEGIN
        IF @ledger_short_name = 'CO'
        BEGIN
            SELECT @this_amount = @ClientTotalCommissionAmount
        END

        IF @ledger_short_name = 'FE'
        BEGIN
            IF @ClientFeeAmount = 0 OR @full_amount = 0
            BEGIN
                SELECT @this_amount = 0
            END
            ELSE
            BEGIN
                /*Split the fee amount earned for this payment over any fees on the transaction*/       
                IF @full_amount = @ClientFeeAmount
                BEGIN
                    SELECT @this_amount = @ClientTotalFeeAmount
                END
                ELSE
                BEGIN
                    SELECT @this_amount = @ClientTotalFeeAmount / (@ClientFeeAmount / @full_amount)
                END
            END
        END

        IF @ledger_short_name = 'DI'
        BEGIN
            /*If client is fully allocated then the discount has been used*/
            IF @ClientOutstandingLessFeeAmount - @ClientMatchAmount = 0
            BEGIN
                SELECT @this_amount = @full_amount
            END
            ELSE
            BEGIN
                SELECT @this_amount = @full_amount
            END
        END
    END
    
    /*Update the total fee and commission amounts for this payment/receipt*/
    UPDATE #Report_Lines
    SET this_amount = @this_amount,
        remove_transaction = @RemoveTransaction
    WHERE CURRENT OF c_Trans

    /*Get next report line*/
    FETCH NEXT FROM c_Trans INTO @payment_document_id, @client_document_id, @ledger_short_name, @match_id, @full_amount
END

/*Remove cursor*/
CLOSE c_Trans
DEALLOCATE c_Trans

/*Delete payments/receipts that were irrelevant to the transcation allocated*/
DELETE 
FROM #Report_Lines
WHERE remove_transaction = 1

/*Now add the writeoff amounts*/    
INSERT INTO #Report_Lines
SELECT 
    RL.payment_document_id, 
    RL.payment_document_ref, 
    DW.document_id, 
    DW.document_ref, 
    'NO',
    AWW.short_code,
    AWW.account_name,
    RL.match_id,
    TDWW.amount,
    TDWW.amount,
    NULL,
    NULL
FROM #Report_Lines RL
JOIN transdetail TD
    ON TD.document_id = RL.client_document_id
JOIN transmatch TM
    ON TM.transdetail_id = TD.transdetail_id
    AND TM.match_id = RL.match_id
JOIN transmatch TMWC 
    ON TMWC.allocationdetail_id = TM.allocationdetail_id
    AND TMWC.transmatch_id <> TM.transmatch_id
JOIN transdetail TDWC /*Write Off transaction, Client account*/
    ON TDWC.transdetail_id = TMWC.transdetail_id
    AND TDWC.account_id = TD.account_id
JOIN document DW
    ON DW.document_id = TDWC.document_id
JOIN transdetail TDWW /*Write Off transaction, Write Off account*/
    ON TDWW.document_id = TDWC.document_id
    AND TDWW.transdetail_id <> TDWC.transdetail_id
JOIN account AWW
    ON AWW.account_id = TDWW.account_id
JOIN DocumentType DTW   /*ensure that we are getting a write off document*/
    ON DTW.documentType_id = DW.DocumentType_id
WHERE DTW.code = 'SWD'
GROUP BY
    RL.payment_document_id, 
    RL.payment_document_ref, 
    DW.document_id, 
    DW.document_ref, 
    AWW.short_code,
    AWW.account_name,
    RL.match_id,
    TDWW.amount

/*Select all of the data*/
SELECT
    payment_document_ref,
    client_document_ref,
    ledger_short_name,
    account_code,
    account_name,
    full_amount,
    this_amount,
    fsa_disabled
FROM #Report_Lines
ORDER BY 
    ledger_short_name, 
    account_code, 
    payment_document_ref, 
    client_document_ref

DROP TABLE #Report_Lines

GO

