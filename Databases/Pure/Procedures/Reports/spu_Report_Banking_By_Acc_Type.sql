SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Banking_By_Acc_Type'
GO

CREATE PROCEDURE spu_Report_Banking_By_Acc_Type
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

DECLARE @account_id INT
DECLARE @bf_bank_amount MONEY
DECLARE @cf_bank_amount MONEY
DECLARE @sStartDate VARCHAR(60)
DECLARE @sEndDate VARCHAR(60)
DECLARE @TransDetail_id INT
DECLARE @ClientTransDetail_id INT
DECLARE @ClientTransMatch_id INT
DECLARE @ClientMatchAmount MONEY
DECLARE @ClientAlreadyMatchedAmount MONEY
DECLARE @ClientPremiumAmount MONEY
DECLARE @ClientFeeAmount MONEY
DECLARE @ClientCommissionAmount MONEY
DECLARE @ClientTotalFeeAmount MONEY
DECLARE @ClientTotalCommissionAmount MONEY
DECLARE @ClientOutstandingLessFeeAmount MONEY
DECLARE @ClientPremiumToCommissionRate FLOAT
DECLARE @TotalFeeAmount MONEY
DECLARE @TotalCommissionAmount MONEY
DECLARE @payment_or_receipt_type_flag VARCHAR(1)
DECLARE @payment_or_receipt_type_desc VARCHAR(50)
DECLARE @cashlistitem_amount MONEY
DECLARE @document_ref VARCHAR(30)

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #transactions
(
    bank_account_id INT,
    bank_account_code VARCHAR(20),
    bank_account_name VARCHAR(60),
    bf_bank_amount MONEY,
    bank_amount MONEY,
    cf_bank_amount MONEY,
    account_code VARCHAR(30),
    ledger_name VARCHAR(30),
    account_amount MONEY,
    policy_number VARCHAR(30),
    document_date DATETIME,
    documenttype_id INT,
    document_ref VARCHAR(25),
    bank_branch_id INT,
    bank_branch_code VARCHAR(20),
    bank_branch_name VARCHAR(255),
    method VARCHAR(255),
    media_ref VARCHAR(255),
    transdetail_id INT,
    fee_amount MONEY,
    commission_amount MONEY,
    comment VARCHAR(255),
    issuer VARCHAR(255),
    payment_or_receipt_type_flag VARCHAR(1),
    payment_or_receipt_type_desc VARCHAR(50),
    fsa_disabled BIT  
)

IF NOT EXISTS  
    (  
        SELECT NULL  
        FROM hidden_options  
        WHERE option_number = 61  
        AND value = '1'  
    )  
BEGIN  
    INSERT INTO #transactions  
    (  
        fsa_disabled  
    )  
    VALUES  
    (  
        1  
    )  
  
    SELECT 
    	bank_account_code,
	bank_account_name,
    	bf_bank_amount,
    	bank_amount,
    	cf_bank_amount,
    	account_code,
    	account_amount,
    	policy_number,
    	ledger_name,
    	document_date,
    	documenttype_id,
    	document_ref,
    	bank_branch_id,
    	bank_branch_name,
    	bank_branch_code,
    	method,
    	media_ref,
	fee_amount,
    	commission_amount,
    	payment_or_receipt_type_flag,
    	payment_or_receipt_type_desc,
    	fsa_disabled  
    FROM #transactions  
  
    DROP TABLE #transactions  
  
    RETURN  
END

/*Transactions in period - Receipts & Payments*/
INSERT INTO #transactions
(
    bank_account_id,
    bank_account_code,
    bank_account_name,
    bf_bank_amount,
    bank_amount,
    cf_bank_amount,
    account_code,
    ledger_name,
    account_amount, 
    policy_number,
    document_date,
    documenttype_id,
    document_ref,
    bank_branch_id,
    bank_branch_code,
    bank_branch_name,
    method,
    media_ref,
    transdetail_id
)
SELECT 
    BAA.account_id,
    BAA.short_code, 
    B.bank_name, 
    0,
    CASE 
        WHEN BTD.document_sequence = 1 AND TD.document_sequence = 2 THEN BTD.amount 
        WHEN TD.document_sequence = 1 THEN BTD.amount 
        ELSE 0 
    END, 
    0,
    A.short_code, 
    L.ledger_name,
    TD.amount, 
    ISNULL(TD.insurance_ref, ''), 
    D.document_date, 
    D.documenttype_id, 
    D.document_ref, 
    BTD.company_id, 
    BC.code, 
    BC.description, 
    ISNULL(MT.description, ''), 
    ISNULL(CLI.media_ref, ''), 
    TD.transdetail_id 
FROM Document D 
JOIN TransDetail TD
    ON TD.document_id = D.document_id
JOIN Account A
    ON A.account_id = TD.account_id
JOIN Ledger L
    ON L.ledger_id = A.ledger_id
JOIN TransDetail BTD
    ON BTD.document_id = D.document_id
    AND BTD.transdetail_id <> TD.transdetail_id
JOIN Account BAA
    ON BAA.account_id = BTD.account_id
JOIN Company BC
    ON BC.company_id = BTD.company_id
JOIN BankAccount BA
    ON BA.account_id = BTD.account_id
JOIN Bank B
    ON B.bank_id = BA.bank_id
LEFT JOIN CashListItem CLI
    ON CLI.transdetail_id = TD.transdetail_id
LEFT JOIN MediaType MT
    ON MT.mediatype_id = CLI.mediatype_id
WHERE D.documenttype_id IN (1, 22, 23, 43)
AND D.document_date BETWEEN @start_date AND @end_date
AND BTD.company_id = ISNULL(@branch_id, BTD.company_id)
AND (
        SELECT
            SUM(amount)
        FROM transdetail
        WHERE document_id = TD.document_id
        AND account_id = TD.account_id
    ) <> 0

/*Go through each bank account and calculate brought forward and carried forward amounts*/
DECLARE c_account CURSOR FORWARD_ONLY FOR
    SELECT bank_account_id
    FROM #transactions
    GROUP BY bank_account_id
OPEN c_account

FETCH NEXT FROM c_account INTO @account_id
WHILE @@FETCH_STATUS = 0
BEGIN

    /*Calculate brought forward amount*/
    SELECT
        @bf_bank_amount = ISNULL(SUM(TD.amount),0)
    FROM TransDetail TD
    JOIN Document D
        ON D.document_id = TD.document_id
    WHERE D.documenttype_id IN (1, 22, 23, 43) 
    AND D.document_date < @start_date 
    AND D.company_id = ISNULL(@branch_id, D.company_id)
    AND TD.account_id = @account_id 

    /*Calculate carried forward amount*/
    SELECT 
        @cf_bank_amount = ISNULL(SUM(TD.amount),0)
    FROM TransDetail TD
    JOIN Document D
        ON D.document_id = TD.document_id
    WHERE D.documenttype_id IN (1, 22, 23, 43)
    AND D.document_date <= @end_date 
    AND D.company_id = ISNULL(@branch_id, D.company_id)
    AND TD.account_id = @account_id
    
    /*Update temporary table with calculated amounts*/
    UPDATE #transactions
    SET bf_bank_amount = @bf_bank_amount,
        cf_bank_amount = @cf_bank_amount
    WHERE bank_account_id = @account_id
    
    FETCH NEXT FROM c_account INTO @account_id
END

CLOSE c_account
DEALLOCATE c_account


/*Go through each payment/receipt and calculate total fee and commission amounts.*/
/*PN 23475 Also update 2 new columns for report grouping enhancements */
DECLARE c_transaction CURSOR FORWARD_ONLY FOR
    SELECT transdetail_id 
    FROM #transactions

OPEN c_transaction

FETCH NEXT FROM c_Transaction INTO @Transdetail_id

WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT 
        @cashlistitem_amount = ISNULL(CLI.amount, T.bank_amount),
        @document_ref = RTRIM(T.document_ref)
    FROM #transactions T
    LEFT JOIN CashListItem CLI
        ON CLI.transdetail_id = T.transdetail_id
    WHERE T.transdetail_id = @Transdetail_id

    IF LEFT(@document_ref,3) = 'SRP'
    BEGIN
        --Cashlist Item type is RECEIPT if document ref prefix is SRP
        SELECT @payment_or_receipt_type_flag = 'R' 

        SELECT 
            @payment_or_receipt_type_desc = ISNULL(rt.description,'Unknown Receipt Type')
        FROM transdetail td
        LEFT JOIN cashlistitem cli    
            JOIN cashlistitem_receipt_type rt 
                ON rt.cashlistitem_receipt_type_id = cli.cashlistitem_receipt_type_id
            ON cli.transdetail_id = td.transdetail_id 
        WHERE td.transdetail_id = @transdetail_id

        --Signify if this is a reversed receipt (it will have a negative amount)
        IF @cashlistitem_amount < 0
           SELECT @document_ref = @document_ref + ' (R)'

    END
    ELSE
    BEGIN
        --Cashlist Item type is PAYMENT (as document ref prefix is SPY)
        SELECT @payment_or_receipt_type_flag = 'P'

        SELECT 
            @payment_or_receipt_type_desc = ISNULL(pt.description,'Unknown Payment Type')
        FROM transdetail td
        LEFT JOIN cashlistitem cli
            JOIN cashlistitem_payment_type pt
                ON pt.cashlistitem_payment_type_id = cli.cashlistitem_payment_type_id
            ON cli.transdetail_id = td.transdetail_id 
        WHERE td.transdetail_id = @transdetail_id

        --Signify if this is a reversed payment (it will have a positive amount)
        IF @cashlistitem_amount > 0
           SELECT @document_ref = @document_ref + ' (R)'

    END

    /*Initialise the total variables*/
    SELECT @TotalFeeAmount = 0
    SELECT @TotalCommissionAmount = 0 

    /*Go through each client transaction paid and calculate fee and commission amounts*/
    DECLARE c_ClientTrans CURSOR FORWARD_ONLY FOR
        SELECT TMC.transdetail_id, TMC.transmatch_id, TMC.base_match_amount
        FROM transmatch TMB /*Transmatch for receipt/payment*/
        JOIN transmatch TMC /*Transmatch for client*/
            ON TMC.match_id = TMB.match_id
            AND TMC.transdetail_id <> TMB.transdetail_id
        JOIN transdetail TD
            ON TD.transdetail_id = TMC.transdetail_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name = 'SA'
        WHERE TMB.transdetail_id = @Transdetail_id
        AND TMB.is_reversed IS NULL

    OPEN c_ClientTrans

    FETCH NEXT FROM c_ClientTrans INTO @ClientTransDetail_id, @ClientTransMatch_id, @ClientMatchAmount 

    WHILE @@FETCH_STATUS = 0
    BEGIN


        /*Only calculate values from matches where this bank transaction is matched last.*/        
        /*When using cash/cheque payments/receipts the payment is always the last matched.*/
        IF @Transdetail_id = 
            (
                SELECT TM.transdetail_id
                FROM transmatch TM
                WHERE TM.transmatch_id = 
                    (
                        SELECT MAX(TM.transmatch_id)
                        FROM bankaccount BA
                        JOIN bank B
                            ON B.bank_id = BA.bank_id
                        JOIN transdetail BTD
                            ON BTD.account_id = BA.account_id
                        JOIN document D
                            ON D.document_id = BTD.document_id
                            AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43)
                        JOIN transdetail TD
                            ON TD.document_id = BTD.document_id
                            AND TD.transdetail_id <> BTD.transdetail_id
                        JOIN transmatch TM
                            ON TM.transdetail_id = TD.transdetail_id
                        JOIN transmatch CTM
                            ON CTM.match_id = TM.match_id
                        WHERE CTM.transmatch_id = @ClientTransMatch_id
                    )               
            )
        BEGIN

        /*Get the client amount that had already been matched off before this payment/receipt was allocated*/
        SELECT @ClientAlreadyMatchedAmount = ISNULL(SUM(TMALL.base_match_amount),0)
        FROM transdetail TD
        JOIN transmatch TM
            ON TM.transdetail_id = TD.transdetail_id 
            AND TM.transmatch_id = @ClientTransMatch_id
        JOIN matchgroup MG
            ON MG.match_id = TM.match_id
        JOIN transdetail TDALL
            ON TDALL.document_id = TD.document_id
            AND TDALL.account_id = TD.account_id
        JOIN transmatch TMALL
            ON TMALL.transdetail_id = TDALL.transdetail_id 
        JOIN matchgroup MGALL
            ON MGALL.match_id = TMALL.match_id
            AND MGALL.match_date <= MG.match_date AND MGALL.match_id < MG.match_id  
        WHERE TD.transdetail_id = @ClientTransDetail_id

        /*Get the clients total premium amount for this transaction*/
        SELECT @ClientPremiumAmount = ISNULL(SUM(TDALL.amount),0)
        FROM transdetail TD
        JOIN transdetail TDALL
            ON TDALL.document_id = TD.document_id
            AND TDALL.account_id = TD.account_id
        WHERE TD.transdetail_id = @ClientTransDetail_id

        IF @ClientPremiumAmount <> 0
        BEGIN

            /*Get the fee amount for this transaction*/
            SELECT @ClientFeeAmount = ISNULL(SUM(TDFEE.amount),0) * -1
            FROM transdetail TD
            JOIN transdetail TDFEE
                ON TDFEE.document_id = TD.document_id
            JOIN account A
                ON A.account_id = TDFEE.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = 'FE'
            WHERE TD.transdetail_id = @ClientTransDetail_id

            /*Get the commission amount for this transaction*/
            SELECT @ClientCommissionAmount = ISNULL(SUM(TDCOMM.amount),0)
            FROM transdetail TD
            JOIN transdetail TDCOMM
                ON TDCOMM.document_id = TD.document_id
            JOIN account A
                ON A.account_id = TDCOMM.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
                AND L.ledger_short_name = 'CO'
            WHERE TD.transdetail_id = @ClientTransDetail_id

            /*Calculate client outstanding amount without the fee added on*/
            --IF @ClientPremiumAmount < 0
            IF @payment_or_receipt_type_flag ='P'
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
            --IF @ClientPremiumAmount < 0
            IF @payment_or_receipt_type_flag = 'P'
            BEGIN
                /*Payment, so don't do anything with fee*/
                /*DC150904 PN14468 check for division by zero*/
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
                /*DC150904 PN14468 check for division by zero*/
                IF (@ClientPremiumAmount - @ClientFeeAmount) = 0
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
                --IF @ClientPremiumAmount < 0
                IF @payment_or_receipt_type_flag = 'P'
                BEGIN
                    /*Payment*/
                    SELECT @ClientTotalFeeAmount = @ClientFeeAmount * -1
                END
                ELSE
                BEGIN
                    /*Receipt*/
                    SELECT @ClientTotalFeeAmount = (@ClientMatchAmount - @ClientOutstandingLessFeeAmount) * -1
                END
                SELECT @ClientTotalCommissionAmount = @ClientOutstandingLessFeeAmount * @ClientPremiumToCommissionRate
            END         

            /*Update the total amounts for this payment/receipt*/
            SELECT @TotalFeeAmount = @TotalFeeAmount + @ClientTotalFeeAmount 
            SELECT @TotalCommissionAmount = @TotalCommissionAmount + @ClientTotalCommissionAmount 
        END

		END

        /*Get next client transaction*/
        FETCH NEXT FROM c_ClientTrans INTO @ClientTransDetail_id, @ClientTransMatch_id, @ClientMatchAmount 
    END

    CLOSE c_ClientTrans
    DEALLOCATE c_ClientTrans

    --Update the total fee and commission amounts for this payment/receipt
    --Also update the document_ref (as it may signify a reversal now)
    UPDATE #transactions
    SET fee_amount = @TotalFeeAmount,
        commission_amount = @TotalCommissionAmount,
        payment_or_receipt_type_flag = @payment_or_receipt_type_flag,
        payment_or_receipt_type_desc = @payment_or_receipt_type_desc,
        document_ref = @document_ref
    WHERE CURRENT OF c_Transaction

    /*Get next payment/receipt*/
    FETCH NEXT FROM c_Transaction INTO @Transdetail_id
END

CLOSE c_transaction
DEALLOCATE c_transaction

/*Get the data*/
SET NOCOUNT OFF

SELECT DISTINCT 
    bank_account_code,
    bank_account_name,
    bf_bank_amount,
    bank_amount,
    cf_bank_amount,
    account_code,
    account_amount,
    policy_number,
    ledger_name,
    document_date,
    documenttype_id,
    document_ref,
    bank_branch_id,
    bank_branch_name,
    bank_branch_code,
    method,
    media_ref,
    ISNULL(fee_amount, 0) fee_amount,
    ISNULL(commission_amount, 0) commission_amount,
    payment_or_receipt_type_flag,
    payment_or_receipt_type_desc,
    fsa_disabled

FROM #transactions 
WHERE
    (bank_amount <> 0)
OR
    (bf_bank_amount <> 0)
OR
    (cf_bank_amount <> 0)

DROP TABLE #transactions

GO

