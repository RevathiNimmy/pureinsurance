/*This stored procedure is used by the following reports:*/
/*Audit_Report_For_Payments_and_Receipts.rpt*/
/*Weekly_Payments_and_Receipts_Currency.rpt*/

EXECUTE DDLDropProcedure 'spu_Report_ReceiptPayment'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS  ON
GO


CREATE PROCEDURE spu_Report_ReceiptPayment
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @bank VARCHAR(60)

AS

DECLARE 
    @account_id INT,
    @company_id INT,
    @bf_bank_amount MONEY,
    @bf_bank_currency_amount MONEY,
    @cf_bank_amount MONEY,
    @cf_bank_currency_amount MONEY,
    @ClientTransDetail_id INT,
    @ClientTransMatch_id INT,
    @ClientMatchAmount MONEY,
    @ClientAlreadyMatchedAmount MONEY,
    @ClientPremiumAmount MONEY,
    @ClientFeeAmount MONEY,
    @ClientCommissionAmount MONEY,
    @ClientTotalFeeAmount MONEY,
    @ClientTotalCommissionAmount MONEY,
    @ClientOutstandingLessFeeAmount MONEY,
    @ClientPremiumToCommissionRate FLOAT,
    @TotalFeeAmount MONEY,
    @TotalCommissionAmount MONEY,
    @MediaMethod VARCHAR(255),
    @DocumentComment VARCHAR(255),
    @DocumentRef VARCHAR(25),
    @CompanyId INT,
    @Transdetail_id INT,
    @ClientMatchID INT,
    @ClientMatchDate DATETIME

SET NOCOUNT ON

/*Check parameters*/
IF ISNULL(RTRIM(@bank),'ALL') = 'ALL'
BEGIN
    SELECT @bank = NULL
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Payment_And_Receipt
(
    /*Bank Details*/
    bank_account_id INT,
    bank_account_code CHAR(20),
    bank_name VARCHAR(60),
    bank_transdetail_id INT,
    bf_bank_amount MONEY,
    bf_bank_currency_amount MONEY,
    cf_bank_amount MONEY,
    cf_bank_currency_amount MONEY,

    /*Transaction Details*/
    document_id INT,
    document_ref VARCHAR(25),
    document_date DATETIME,
    document_comment VARCHAR(255),
    transaction_amount MONEY,
    transaction_currency_amount MONEY,

    /*Company Details*/
    company_id INT,
    company_code VARCHAR(10),
    company_name VARCHAR(255),
    
    /*Policy Details*/
    insurance_file_cnt INT,
    insurance_ref VARCHAR(30),
    policy_document_ref VARCHAR(25),
    allocated_transdetail_id INT,
    allocated_amount MONEY,
    unallocated_amount MONEY,
    
    /*Cash List Details*/
    media_ref VARCHAR(255),
    media_type_desc VARCHAR(255),
    media_type_issuer_desc VARCHAR(255),
    
    /*Client Amounts*/
    client_account_id INT,
    client_account_code CHAR(30),
    client_ledger_code VARCHAR(2),    
    client_transdetail_id INT,
    client_transmatch_id INT,
    client_match_id INT,
    client_match_date DATETIME,
    
    /*Opposing Transaction Details*/
    fee_amount MONEY,
    commission_amount MONEY,
    this_fee_amount MONEY,
    this_commission_amount MONEY,

    /*Miscellaneous*/
    first_line BIT
    
)

/*Insert all valid transactions into temporary table*/
INSERT INTO #Payment_And_Receipt
(
    /*Bank Details*/
    bank_account_id,
    bank_account_code,
    bank_name,
    bank_transdetail_id,

    /*Transaction Details*/
    document_id,
    document_ref,
    document_date,
    document_comment,
    transaction_amount,
    transaction_currency_amount,

    /*Company Details*/
    company_id,
    company_code,
    company_name,
    
    /*Miscellaneous*/
    first_line
   
)
SELECT 
    BAA.account_id,
    BAA.short_code,
    B.bank_name,
    BTD.transdetail_id,

    D.document_id,
    D.document_ref,
    D.document_date,
    D.comment,
    BTD.amount,
    BTD.currency_amount,

    C.company_id,
    C.code,
    C.description,
    
    1
    
FROM Bank B
JOIN BankAccount BA
    ON BA.bank_id = B.bank_id
JOIN Account BAA
    ON BAA.account_id = BA.account_id
JOIN TransDetail BTD
    ON BTD.account_id = BAA.account_id
JOIN Document D
    ON D.document_id = BTD.document_id
JOIN Company C
    ON C.company_id = D.company_id    
WHERE D.documenttype_id IN (1,10, 12, 22, 23, 28, 29, 43)
AND D.document_date BETWEEN @start_date AND @end_date 
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND RTRIM(B.bank_name) = ISNULL(@Bank, RTRIM(B.bank_name))


/*Remove batch journals from the selection*/
DELETE FROM #Payment_And_Receipt
FROM #Payment_And_Receipt PR
JOIN Document D
    ON D.document_id = PR.document_id
WHERE D.documenttype_id = 1
AND 0 = 
    (
        SELECT
            SUM(amount)
        FROM TransDetail
        WHERE document_id = D.document_id
        AND account_id = PR.bank_account_id
    )
    

/*Update transactions with cash list details*/  
UPDATE PR
SET media_ref = ISNULL(CLI.media_ref,''), 
    media_type_desc = ISNULL(MT.description,''),
    media_type_issuer_desc = ISNULL(MTI.description,'')
FROM #Payment_And_Receipt PR
JOIN TransDetail TD
    ON TD.document_id = PR.document_id
JOIN CashListItem CLI
    ON CLI.transdetail_id = TD.transdetail_id
LEFT JOIN MediaType MT
    ON MT.mediatype_id = CLI.mediatype_id
LEFT JOIN MediaType_Issuer MTI
    ON MTI.mediatype_issuer_id = CLI.mediatype_issuer_id
    
    
/*Update reversal transactions with cash list details*/  
UPDATE PR
SET media_ref = ISNULL(CLI.media_ref,''), 
    media_type_desc = ISNULL(MT.description,''),
    media_type_issuer_desc = ISNULL(MTI.description,'')
FROM #Payment_And_Receipt PR
JOIN Document D
    ON D.document_id = PR.document_id
    AND D.comment LIKE 'Reversal of Document%'
JOIN Document D2
    ON D2.document_ref = SUBSTRING(D.comment,22,11)
    AND D2.company_id = D.company_id
JOIN TransDetail TD
    ON TD.document_id = D2.document_id
JOIN CashListItem CLI
    ON CLI.transdetail_id = TD.transdetail_id
LEFT JOIN MediaType MT
    ON MT.mediatype_id = CLI.mediatype_id
LEFT JOIN MediaType_Issuer MTI
    ON MTI.mediatype_issuer_id = CLI.mediatype_issuer_id
    

/*Update transaction with client details*/
UPDATE PR
SET PR.client_account_id = A.account_id,
    PR.client_account_code = A.short_code,    
    PR.client_transdetail_id = TD.transdetail_id,
    PR.client_ledger_code=L.ledger_short_name,
    PR.allocated_amount=-(ISNULL(TD.amount,0)-ISNULL(TD.outstanding_amount,0)),
    PR.unallocated_amount=-ISNULL(TD.outstanding_amount,0)
FROM #Payment_And_Receipt PR
JOIN TransDetail TD
    ON TD.document_id = PR.document_id
    AND TD.transdetail_id <> PR.bank_transdetail_id
JOIN Account A
    ON A.account_id = TD.account_id
JOIN Ledger L
    ON A.ledger_id = L.ledger_id
WHERE /*Only show a client for transactions that only have one bank line and one client line*/ 
    (
        SELECT
            SUM(1)
        FROM transdetail
        WHERE document_id = PR.document_id
    ) = 2
    
UPDATE #Payment_And_Receipt
SET client_account_code = '-',
    fee_amount = 0,
    commission_amount = 0
WHERE client_transdetail_id IS NULL

/*Go through each bank account and calculate brought forward and carried forward amounts*/
DECLARE c_account CURSOR FORWARD_ONLY FOR
    SELECT bank_account_id, company_id
    FROM #Payment_And_Receipt
    GROUP BY bank_account_id, company_id

OPEN c_account

FETCH NEXT FROM c_account INTO @account_id, @company_id

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Calculate brought forward amount*/
    SELECT
        @bf_bank_amount = ISNULL(SUM(TD.amount),0),
        @bf_bank_currency_amount = ISNULL(SUM(TD.currency_amount),0)
    FROM TransDetail TD
    JOIN Document D
        ON D.document_id = TD.document_id
    WHERE D.documenttype_id IN (1,10, 12, 22, 23, 28, 29, 43)
    AND D.document_date < @start_date 
    AND D.company_id = ISNULL(@branch_id, D.company_id)
    AND TD.account_id = @account_id
    AND D.company_id = @company_id
    
    /*Calculate carried forward amount*/
    SELECT 
        @cf_bank_amount = ISNULL(SUM(TD.amount),0),
        @cf_bank_currency_amount = ISNULL(SUM(TD.currency_amount),0)
    FROM TransDetail TD
    JOIN Document D
        ON D.document_id = TD.document_id
    WHERE D.documenttype_id IN (1,10, 12, 22, 23, 28, 29, 43)
    AND D.document_date <= @end_date 
    AND D.company_id = ISNULL(@branch_id, D.company_id)
    AND TD.account_id = @account_id
    AND D.company_id = @company_id
    
    /*Update temporary table with calculated amounts*/
    UPDATE #Payment_And_Receipt
    SET bf_bank_amount = @bf_bank_amount,
        bf_bank_currency_amount = @bf_bank_currency_amount,
        cf_bank_amount = @cf_bank_amount,
        cf_bank_currency_amount = @cf_bank_currency_amount
    WHERE bank_account_id = @account_id
    AND company_id = @company_id
    
    FETCH NEXT FROM c_account INTO @account_id, @company_id
END

CLOSE c_account
DEALLOCATE c_account

/*Go through each payment/receipt and calculate total fee and commission amounts.*/
DECLARE c_transaction CURSOR FORWARD_ONLY FOR
    SELECT client_transdetail_id, company_id, document_comment
    FROM #Payment_And_Receipt
    WHERE client_transdetail_id IS NOT NULL

OPEN c_transaction

FETCH NEXT FROM c_Transaction INTO @Transdetail_id, @CompanyId, @DocumentComment

WHILE @@FETCH_STATUS = 0
BEGIN

    /*AR PN11448 - if this is a reversal, find the media type of the document it reversed*/
    /*             comment on reversed document is in format 'Reversal of Document XXXX' */
    /*             where XXXX is the reference of the document that was reversed         */
    IF LEN(@DocumentComment)>21
    BEGIN
        IF UPPER(LEFT(@DocumentComment,21))='REVERSAL OF DOCUMENT '
        BEGIN

            SELECT @DocumentRef=SUBSTRING(@DocumentComment,22,LEN(@DocumentComment)-21)
            SELECT @MediaMethod=NULL

            SELECT @MediaMethod=MT.description
            FROM MediaType MT INNER JOIN CashListItem CLI ON CLI.mediatype_id=MT.mediatype_id
            INNER JOIN TransDetail TD ON TD.transdetail_id=CLI.transdetail_id
            INNER JOIN Document D ON D.document_id=TD.document_id
            WHERE D.document_ref=@DocumentRef AND D.company_id=@CompanyId

            UPDATE #Payment_And_Receipt 
            SET media_type_desc = @MediaMethod 
            WHERE CURRENT OF c_Transaction

        END
    END

    /*Initialise the total variables*/
    SELECT @TotalFeeAmount = 0
    SELECT @TotalCommissionAmount = 0 

    /*Go through each client transaction paid and calculate fee and commission amounts*/
    DECLARE c_ClientTrans CURSOR FORWARD_ONLY FOR
        SELECT TMC.transdetail_id, TMC.transmatch_id, TMC.base_match_amount, MG.match_id, MG.match_date
        FROM transmatch TMB /*Transmatch for receipt/payment*/
        JOIN transmatch TMC /*Transmatch for client*/
            ON TMC.match_id = TMB.match_id
            AND TMC.transdetail_id <> TMB.transdetail_id
        JOIN matchgroup MG
            ON MG.match_id = TMC.match_id
        JOIN transdetail TD
            ON TD.transdetail_id = TMC.transdetail_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name IN ('SA', 'UB')
        WHERE TMB.transdetail_id = @Transdetail_id
        AND TMB.is_reversed IS NULL

    OPEN c_ClientTrans

    FETCH NEXT FROM c_ClientTrans INTO @ClientTransDetail_id, @ClientTransMatch_id, @ClientMatchAmount, @ClientMatchID, @ClientMatchDate 

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
                            AND D.documenttype_id IN (1,10, 12, 22, 23, 28, 29, 43)
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
            JOIN transdetail TDALL
                ON TDALL.document_id = TD.document_id
                AND TDALL.account_id = TD.account_id
            JOIN transmatch TMALL
                ON TMALL.transdetail_id = TDALL.transdetail_id 
            JOIN matchgroup MGALL
                ON MGALL.match_id = TMALL.match_id
                AND MGALL.match_date <= @ClientMatchDate  
                AND MGALL.match_id < @ClientMatchID
            WHERE TD.transdetail_id = @ClientTransDetail_id

            /*Get the clients total premium amount for this transaction*/
            SELECT @ClientPremiumAmount = ISNULL(SUM(TDALL.amount),0)
            FROM transdetail TD
            JOIN transdetail TDALL
                ON TDALL.document_id = TD.document_id
                AND TDALL.account_id = TD.account_id
            WHERE TD.transdetail_id = @ClientTransDetail_id

            /*If we have a premium and it has not been over allocated*/
            IF @ClientPremiumAmount <> 0 AND ABS(@ClientPremiumAmount) > ABS(@ClientAlreadyMatchedAmount)
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
                IF @ClientPremiumAmount < 0
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
                    IF @ClientPremiumAmount < 0
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
        FETCH NEXT FROM c_ClientTrans INTO @ClientTransDetail_id, @ClientTransMatch_id, @ClientMatchAmount, @ClientMatchID, @ClientMatchDate  
    END

    CLOSE c_ClientTrans
    DEALLOCATE c_ClientTrans

    /*Update the total fee and commission amounts for this payment/receipt*/
    UPDATE #Payment_And_Receipt
    SET fee_amount = @TotalFeeAmount,
        commission_amount = @TotalCommissionAmount
    WHERE CURRENT OF c_Transaction

    /*Get next payment/receipt*/
    FETCH NEXT FROM c_Transaction INTO @Transdetail_id, @CompanyId, @DocumentComment
END

CLOSE c_transaction
DEALLOCATE c_transaction


/*Update the transaction with the first policies details from the matched transactions*/
UPDATE PR
SET PR.insurance_file_cnt = IX.insurance_file_cnt,
    PR.insurance_ref = IX.insurance_ref,
    PR.policy_document_ref=DX.document_ref,
    PR.allocated_transdetail_id=TDX.transdetail_id,
    PR.allocated_amount=TMX.base_match_amount,
    PR.unallocated_amount=(SELECT ABS(ISNULL(outstanding_amount,0)) FROM transdetail WHERE transdetail.transdetail_id=PR.client_transdetail_id),
    PR.client_transmatch_id=TMX.transmatch_id,
    PR.client_match_id=MG.match_id,
    PR.client_match_date=MG.match_date
FROM #Payment_And_Receipt PR
JOIN transmatch TM
    ON TM.transdetail_id = PR.client_transdetail_id
JOIN matchgroup MG
    ON MG.match_id = TM.match_id
JOIN transmatch TMX
    ON TMX.match_id = TM.match_id
    AND TMX.transdetail_id <> TM.transdetail_id
JOIN transdetail TDX
    ON TDX.transdetail_id = TMX.transdetail_id
JOIN document DX
    ON DX.document_id = TDX.document_id
JOIN insurance_file IX
    ON IX.insurance_file_cnt = DX.insurance_file_cnt
WHERE EXISTS
    (
        SELECT
            NULL
        FROM account A
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
        WHERE A.account_id = PR.client_account_id
        AND L.ledger_short_name IN ('SA','UB')
    )

/*Add extra records for any other policies included in the match*/
INSERT INTO #Payment_And_Receipt
SELECT DISTINCT
    /*Bank Details*/
    PR.bank_account_id,
    PR.bank_account_code,
    PR.bank_name,
    PR.bank_transdetail_id,
    PR.bf_bank_amount,
    PR.bf_bank_currency_amount,
    PR.cf_bank_amount,
    PR.cf_bank_currency_amount,

    /*Transaction Details*/
    PR.document_id,
    PR.document_ref,
    PR.document_date,
    PR.document_comment,
    0,
    0,

    /*Company Details*/
    PR.company_id,
    PR.company_code,
    PR.company_name,
    
    /*Policy Details*/
    IX.insurance_file_cnt,
    IX.insurance_ref,
    DX.document_ref,
    TDX.transdetail_id,
    TMX.base_match_amount,
    NULL,

    /*Cash List Details*/
    PR.media_ref,
    PR.media_type_desc,
    PR.media_type_issuer_desc,
    
    /*Client Amounts*/
    PR.client_account_id,
    PR.client_account_code,
    PR.client_ledger_code,    
    PR.client_transdetail_id,
    TMX.transmatch_id,
    MG.match_id,
    MG.match_date,

    /*Opposing Transaction Details*/
    0,
    0,
    NULL,
    NULL,

    /*Miscellaneous*/
    0
FROM #Payment_And_Receipt PR
JOIN transmatch TM
    ON TM.transdetail_id = PR.client_transdetail_id
JOIN matchgroup MG
    ON MG.match_id = TM.match_id
JOIN transmatch TMX
    ON TMX.match_id = TM.match_id
    AND TMX.transdetail_id <> TM.transdetail_id
JOIN transdetail TDX
    ON TDX.transdetail_id = TMX.transdetail_id
JOIN document DX
    ON DX.document_id = TDX.document_id
JOIN insurance_file IX
    ON IX.insurance_file_cnt = DX.insurance_file_cnt
WHERE DX.document_ref <> PR.policy_document_ref 
AND EXISTS
    (
        SELECT
            NULL
        FROM account A
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
        WHERE A.account_id = PR.client_account_id
        AND L.ledger_short_name IN ('SA','UB')
    )

DECLARE c_ClientTrans CURSOR FORWARD_ONLY FOR
    SELECT 
        PR.allocated_transdetail_id, 
        PR.client_transmatch_id, 
        PR.allocated_amount,
        PR.client_match_id,
        PR.client_match_date
    FROM #Payment_And_Receipt PR
    WHERE client_transmatch_id IS NOT NULL

OPEN c_ClientTrans

FETCH NEXT FROM c_ClientTrans INTO @ClientTransDetail_id, @ClientTransMatch_id, @ClientMatchAmount, @ClientMatchID, @ClientMatchDate

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Get the client amount that had already been matched off before this payment/receipt was allocated*/           
            SELECT @ClientAlreadyMatchedAmount = ISNULL(SUM(TMALL.base_match_amount),0)
            FROM transdetail TD
            JOIN transdetail TDALL
                ON TDALL.document_id = TD.document_id
                AND TDALL.account_id = TD.account_id
            JOIN transmatch TMALL
                ON TMALL.transdetail_id = TDALL.transdetail_id 
            JOIN matchgroup MGALL
                ON MGALL.match_id = TMALL.match_id
                AND MGALL.match_date <= @ClientMatchDate  
                AND MGALL.match_id < @ClientMatchID
            WHERE TD.transdetail_id = @ClientTransDetail_id

            /*Get the clients total premium amount for this transaction*/
            SELECT @ClientPremiumAmount = ISNULL(SUM(TDALL.amount),0)
            FROM transdetail TD
            JOIN transdetail TDALL
                ON TDALL.document_id = TD.document_id
                AND TDALL.account_id = TD.account_id
            WHERE TD.transdetail_id = @ClientTransDetail_id

            /*If we have a premium and it has not been over allocated*/
            IF @ClientPremiumAmount <> 0 AND ABS(@ClientPremiumAmount) > ABS(@ClientAlreadyMatchedAmount)
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
                IF @ClientPremiumAmount < 0
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
                    IF @ClientPremiumAmount < 0
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

        END

        UPDATE #Payment_And_Receipt 
        SET this_fee_amount = @ClientTotalFeeAmount, 
            this_commission_amount = @ClientTotalCommissionAmount
        WHERE CURRENT OF c_ClientTrans
            
    FETCH NEXT FROM c_ClientTrans INTO @ClientTransDetail_id, @ClientTransMatch_id, @ClientMatchAmount, @ClientMatchID, @ClientMatchDate
END

CLOSE c_ClientTrans
DEALLOCATE c_ClientTrans

/*Get the data*/
SELECT 
    *
FROM #Payment_And_Receipt
ORDER BY 
    company_id, 
    bank_account_code, 
    document_date, 
    document_ref,
    first_line DESC

DROP TABLE #Payment_And_Receipt    

SET NOCOUNT OFF

GO

