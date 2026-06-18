SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_SuspendedTaxTransactions_Triggers'
GO

-- Object:  Stored Procedure spu_ACT_Get_SuspendedTaxTransactions_Triggers 
-- Script Date: 12 10 2005 12:06:42 PM

CREATE PROCEDURE spu_ACT_Get_SuspendedTaxTransactions_Triggers
    @DocumentId INT
AS

DECLARE @fee_option VARCHAR(255)
DECLARE @branch_id INT
DECLARE @totalgross FLOAT
DECLARE @totalnet FLOAT
DECLARE @DDtrans INT
DECLARE @DocumentType VARCHAR(30)
DECLARE @accounting_basis tinyint

SELECT @DDTrans = 0
SELECT 
    @branch_id = company_id 
FROM document 
WHERE document_Id = @DocumentId

SELECT @accounting_basis=ISNULL(value,0) FROM system_options WHERE branch_id=@branch_id AND option_number=4012


--Client Triggers

-- If we have been allocated to a directdebit reversal

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

-- If we have a subagent this is the trigger
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
        t.transdetail_id,
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
    
    RETURN
END 

SELECT @DocumentType = DT.code 
FROM document D
JOIN documenttype DT ON D.documenttype_id = DT.documenttype_id
WHERE D.document_id = @DocumentId

IF @DocumentType IN ('PIN', 'PCN')
BEGIN

    --else client triggers
    SELECT 
        @totalnet = ISNULL(SUM(t.amount), 0) 
    FROM document d
    JOIN transdetail t 
        ON t.document_id = d.document_id
    JOIN account a 
        ON t.account_id = a.account_id
    JOIN ledger l 
        ON a.ledger_id = l.ledger_id
    WHERE d.document_id = @DocumentId
    AND l.ledger_short_name = 'PU'

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
        WHERE d.document_id = @DocumentId
        AND l.ledger_short_name = 'PU'

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
    WHERE d.document_id = @DocumentId
    AND l.ledger_short_name = 'PU'
    GROUP BY t.account_id 

END
ELSE
BEGIN

    IF @accounting_basis=0
	BEGIN
		SELECT 0,1
		RETURN
	END

    --else client triggers
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


END

GO
 


