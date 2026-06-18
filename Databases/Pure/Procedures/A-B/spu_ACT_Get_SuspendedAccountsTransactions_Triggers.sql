SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_SuspendedAccountsTransactions_Triggers'
GO

CREATE PROCEDURE spu_ACT_Get_SuspendedAccountsTransactions_Triggers
    @DocumentId INT,
    @TransdetailId INT = NULL
AS

DECLARE @commission_option VARCHAR(255)
DECLARE @branch_id INT
DECLARE @totalgross FLOAT
DECLARE @totalnet FLOAT
DECLARE @DDtrans INT

SELECT @DDTrans = 0
SELECT 
    @branch_id = company_id 
FROM document 
WHERE document_Id = @DocumentId

SELECT 
    @commission_option = RTRIM(value) 
FROM system_options
WHERE option_number = 16
AND branch_id = @branch_id

/*If commission taken when transaction is debited we don't need a trigger transaction*/
IF @commission_option  = '0'
BEGIN
    SELECT 0,1
    RETURN 
END

/*If commission taken when policy is effective we don't want a trigger transaction*/
IF @commission_option  = '3'
BEGIN
    SELECT 0,1
    RETURN 
END 

/*Insurer Triggers*/
IF @commission_option  = '2'
BEGIN
    
    IF EXISTS 
        (
            SELECT
                NULL
            FROM transdetail td
            JOIN transdetail_type tt 
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'BROK ADJ'
            JOIN transdetail td2
                ON td2.document_id = td.document_id
                AND td2.document_sequence = td.document_sequence - 1
            JOIN transdetail_type tt2
                ON tt2.transdetail_type_id = td2.transdetail_type_id
                AND tt2.code = 'COMMADJ'
            JOIN account a2
                ON a2.account_id = td2.account_id
            JOIN ledger l2 
                ON l2.ledger_id = a2.ledger_id
                AND l2.ledger_short_name = 'IN'
            WHERE td.transdetail_id = @TransdetailId 
        )
    BEGIN
        
        /*BROKADJ amounts are linked 100% to the COMMADJ amount*/
        SELECT 
            td2.transdetail_id,
            100        
        FROM transdetail td
        JOIN transdetail_type tt 
            ON tt.transdetail_type_id = td.transdetail_type_id
            AND tt.code = 'BROK ADJ'
        JOIN transdetail td2
            ON td2.document_id = td.document_id
            AND td2.document_sequence = td.document_sequence - 1
        JOIN transdetail_type tt2
            ON tt2.transdetail_type_id = td2.transdetail_type_id
            AND tt2.code = 'COMMADJ'
        JOIN account a2
            ON a2.account_id = td2.account_id
        JOIN ledger l2 
            ON l2.ledger_id = a2.ledger_id
            AND l2.ledger_short_name = 'IN'
        WHERE td.transdetail_id = @TransdetailId

    END
    ELSE
    BEGIN
    
        SELECT 
            @totalgross = ISNULL(SUM(td.amount), 0)
        FROM transdetail td
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
            AND tt.code = 'COMM'    
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l 
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'IN'
        WHERE td.document_id = @DocumentId
        /*Insurer/Extras that have 100% commission should not be included when assigning the payment*/
        AND td.amount <>
            (
                SELECT
                    t.amount * -1
                FROM transdetail t
                JOIN transdetail_type tt
                    ON tt.transdetail_type_id = t.transdetail_type_id
                WHERE t.document_id = td.document_id
                AND t.account_id = td.account_id
                AND tt.code = 'GROSS'
            )
        
        IF @totalgross = 0
        BEGIN
        
            SELECT 
                ISNULL(MIN(td.transdetail_id),0),
                100
            FROM transdetail td 
            JOIN transdetail_type tt
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'COMM'
            JOIN account a 
                ON a.account_id = td.account_id
            JOIN ledger l 
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name = 'IN'
            WHERE td.document_id = @DocumentId
            /*Insurer/Extras that have 100% commission should not be included when assigning the payment*/
            AND td.amount <>
                (
                    SELECT
                        t.amount * -1
                    FROM transdetail t
                    JOIN transdetail_type tt
                        ON tt.transdetail_type_id = t.transdetail_type_id
                    WHERE t.document_id = td.document_id
                    AND t.account_id = td.account_id
                    AND tt.code = 'GROSS'
                )      
        
            RETURN 
        END

        SELECT 
            td.transdetail_id,
            SUM(ROUND((td.amount * 100 / @totalgross),2))        
        FROM transdetail td 
        JOIN transdetail_type tt
            ON tt.transdetail_type_id = td.transdetail_type_id
            AND tt.code = 'COMM'
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l 
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'IN'
        WHERE td.document_id = @DocumentId
        /*Insurer/Extras that have 100% commission should not be included when assigning the payment*/
        AND td.amount <>
            (
                SELECT
                    t.amount * -1
                FROM transdetail t
                JOIN transdetail_type tt
                    ON tt.transdetail_type_id = t.transdetail_type_id
                WHERE t.document_id = td.document_id
                AND t.account_id = td.account_id
                AND tt.code = 'GROSS'
            )
        GROUP BY td.transdetail_id
    END
    
    RETURN
END 

/*Client Triggers*/

/*If we have been allocated to a directedebit reversal*/
SELECT 
    @DDTrans = ISNULL(t2.transdetail_id, 0)
FROM transdetail t 
JOIN account a 
    ON t.account_id = a.account_id
JOIN ledger l 
    ON a.ledger_id = l.ledger_id
JOIN transdetail_type ty 
    ON ty.transdetail_type_id = t.transdetail_type_id
JOIN transmatch tm 
    ON tm.transdetail_id = t.transdetail_id
JOIN matchgroup mg 
    ON tm.match_id = mg.match_id
JOIN transmatch tm2 
    ON tm2.match_id = mg.match_id
JOIN transdetail t2 
    ON tm2.transdetail_id = t2.transdetail_id
JOIN transdetail_type ty2 
    ON ty2.transdetail_type_id = t2.transdetail_type_id
WHERE t.document_id = @DocumentId
AND l.ledger_short_name = 'SA'
AND ty.code = 'NET'
AND ty2.code = 'DIRECTDEBIT'
AND tm.transdetail_id <> tm2.transdetail_id

IF @DDTrans <> 0
BEGIN
    SELECT 
        t2.transdetail_id,
        100
    FROM transdetail t 
    JOIN document d 
        ON t.document_id = d.document_id
    JOIN transdetail t2 
        ON t2.document_id = d.document_id
    WHERE t.transdetail_id = @DDTrans
    AND t2.transdetail_id <> t.transdetail_id
    
    RETURN  
END

/*If we have a subagent this is the trigger*/
IF EXISTS 
    (
        SELECT 
            NULL
        FROM transdetail t
        JOIN account a 
            ON t.account_id = a.account_id
        JOIN ledger l 
            ON a.ledger_id = l.ledger_id
        JOIN transdetail_type ty 
            ON ty.transdetail_type_id = t.transdetail_type_id  
        WHERE document_id = @DocumentId
        AND l.ledger_short_name = 'UB'
        AND ty.code = 'NET'
    )
BEGIN
    SELECT 
        MAX(t.transdetail_id),
        100 
    FROM transdetail t
    JOIN account a 
        ON t.account_id = a.account_id
    JOIN ledger l 
        ON a.ledger_id = l.ledger_id
    JOIN transdetail_type ty 
        ON ty.transdetail_type_id = t.transdetail_type_id
    WHERE document_id = @DocumentId
    AND l.ledger_short_name = 'UB'
    AND ty.code = 'NET'
    GROUP BY t.account_id 
    
    RETURN
END 

/*Otherwise the client triggers*/
SELECT 
    @totalnet = ISNULL(SUM(t.amount), 0) 
FROM document d
JOIN transdetail t 
    ON t.document_id = d.document_id
JOIN account a 
    ON t.account_id = a.account_id
JOIN ledger l 
    ON a.ledger_id = l.ledger_id
JOIN transdetail_type ty 
    ON ty.transdetail_type_id = t.transdetail_type_id
WHERE d.document_id = @DocumentId
AND l.ledger_short_name = 'SA'
AND ty.code = 'NET' 
                    
IF @totalnet = 0
BEGIN
    
    SELECT 
        ISNULL(MIN(t.transdetail_id),0),
        100        
    FROM document d
    JOIN transdetail t 
        ON t.document_id = d.document_id
    JOIN account a 
        ON t.account_id = a.account_id
    JOIN ledger l 
        ON a.ledger_id = l.ledger_id
    JOIN transdetail_type ty 
        ON ty.transdetail_type_id = t.transdetail_type_id
    WHERE d.document_id = @DocumentId
    AND l.ledger_short_name = 'SA'
    AND ty.code = 'NET'
    
    RETURN 
END
                    
SELECT 
    MIN(t.transdetail_id),
    SUM(ROUND((t.amount * 100 / @totalnet),2))        
FROM document d
JOIN transdetail t 
    ON t.document_id = d.document_id
JOIN account a 
    ON t.account_id = a.account_id
JOIN ledger l 
    ON a.ledger_id = l.ledger_id
JOIN transdetail_type ty 
    ON ty.transdetail_type_id = t.transdetail_type_id
WHERE d.document_id = @DocumentId
AND l.ledger_short_name = 'SA'
AND ty.code = 'NET'
GROUP BY t.account_id 

 
GO
 

