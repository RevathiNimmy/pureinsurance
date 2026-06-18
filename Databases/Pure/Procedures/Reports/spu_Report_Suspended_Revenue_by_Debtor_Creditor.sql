SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Suspended_Revenue_by_Debtor_Creditor'
GO
  
CREATE PROCEDURE spu_Report_Suspended_Revenue_by_Debtor_Creditor 
    @end_date DATETIME,
    @branch_id INT,
    @date_type VARCHAR(30)
AS
 
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

DECLARE @Ledger_Name VARCHAR(30)
DECLARE @Account_Id INT
DECLARE @Account_code VARCHAR(30)
DECLARE @Account_name VARCHAR(60)
DECLARE @Document_id INT
DECLARE @Document_ref VARCHAR(30)
DECLARE @Transdetail_Id INT
DECLARE @Amount MONEY
DECLARE @Amount_outstanding MONEY
DECLARE @split_perc FLOAT
DECLARE @total_fee_for_doc MONEY
DECLARE @total_comm_for_doc MONEY

CREATE TABLE #TempRevenueDue
(
    account_ledger VARCHAR(30),
    account_id INT,
    account_code VARCHAR(30),
    account_name VARCHAR(255),
    account_balance MONEY,
    document_id INT,
    document_ref VARCHAR(30),
    debtor_transacted MONEY,
    debtor_outstanding MONEY, 
    fees_transacted MONEY, 
    fees_settled MONEY,
    commission_transacted MONEY,
    commission_settled MONEY,
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
    
    INSERT INTO #TempRevenueDue
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT 
        *
    FROM #TempRevenueDue
    
    DROP TABLE #TempRevenueDue
        
    RETURN  
END


DECLARE c_debtors CURSOR FAST_FORWARD FOR
    SELECT DISTINCT 
        L.ledger_name,
        A.account_id,
        A.short_code,
        A.account_name,
        D.document_id,
        D.document_ref,
        TD.Transdetail_id,
        (
            SELECT 
                SUM(amount) 
            FROM transdetail
            WHERE document_id = D.document_id
            AND account_id = A.account_id
        ),
        (
            SELECT 
                SUM(outstanding_amount) 
            FROM transdetail
            WHERE document_id = D.document_id
            AND account_id = A.account_id
        ),
        SAT.linked_percentage              
    FROM Account A  
    JOIN Transdetail TD 
        ON TD.account_id = A.account_id
    JOIN Ledger L 
        ON L.ledger_id = A.ledger_id
    JOIN Suspended_Accounts_Transactions SAT 
        ON SAT.linked_transdetail_id = TD.transdetail_id
    JOIN Document D 
        ON D.document_id = TD.document_id
    WHERE SAT.is_deleted = 0
    AND     
    (
        (
            D.document_date <= @end_date
            AND 
            @date_type = 'Transaction Date'
        )
        OR  
        (
            TD.ref_date <= @end_date
            AND 
            @date_type = 'Effective Date'
        )
    )
    AND D.company_id = ISNULL(@branch_id,D.company_id) 

OPEN c_debtors

FETCH NEXT FROM c_debtors INTO
    @Ledger_Name,
    @Account_Id,
    @Account_code,
    @Account_name,
    @Document_id,
    @Document_ref,
    @Transdetail_Id,    
    @Amount,
    @Amount_outstanding,
    @split_perc 

WHILE (@@FETCH_STATUS = 0)
BEGIN
    
    INSERT INTO #TempRevenueDue
    SELECT
        @Ledger_name,
        @Account_Id,
        @Account_code,
        (
            SELECT
                p.resolved_name
            FROM party p
            JOIN account a
                ON a.account_key = p.party_cnt
            WHERE a.account_id = @account_id
        ),
        (
            SELECT 
                SUM(TBal.outstanding_amount)
            FROM Transdetail TBal 
            JOIN Document DBal 
                ON DBal.document_id = TBal.document_id
            WHERE TBal.account_id = @account_id
            AND DBal.company_id = ISNULL(@branch_id,DBal.company_id)
            AND 
            (
                (
                    DBal.document_date <= @end_date
                    AND
                    @date_type = 'Transaction Date'
                )
                OR
                (
                    TBal.ref_date <= @end_date
                    AND
                    @date_type = 'EffectiveDate'
                )
            )
        ), 
        @Document_id,
        @Document_ref,
        @Amount,
        @Amount_Outstanding,
        (
            SELECT 
                ISNULL(SUM(TFee.amount),0) * ISNULL(SUM(SAT.linked_percentage),0) / 100 
            FROM Transdetail TFee
            JOIN Suspended_Accounts_Transactions SAT 
                ON SAT.suspended_transdetail_id = TFee.Transdetail_id
            JOIN Account A 
                ON A.account_id = TFee.Account_id
            JOIN Ledger L 
                ON L.ledger_id = A.ledger_id
            WHERE SAT.linked_transdetail_id = @Transdetail_id
            AND L.ledger_short_name = 'FE'   
        ),
        (
            SELECT 
                ISNULL(SUM(TFeE.amount),0)  
            FROM Transdetail TFeE
            JOIN Released_Accounts_Transactions RAT 
                ON RAT.destination_transdetail_id = TFeE.transdetail_id
            JOIN Suspended_Accounts_Transactions SAT 
                ON SAT.suspended_transdetail_id = RAT.suspended_transdetail_id
            JOIN Allocation Al 
                ON Al.allocation_id = RAT.allocation_id
            JOIN Transdetail TFe 
                ON TFe.document_id = TFeE.document_id
            JOIN Account A 
                ON A.account_id = TFe.Account_id
            JOIN Ledger L 
                ON L.ledger_id = A.ledger_id
            WHERE SAT.linked_transdetail_id = @Transdetail_id
            AND TFeE.transdetail_id <> TFe.transdetail_id
            AND Al.account_id = @Account_id
            AND L.ledger_short_name = 'FE'
        ),
        (
            SELECT 
                ISNULL(SUM(TCo.amount),0) * ISNULL(SUM(SAT.linked_percentage),0) / 100 
            FROM Transdetail TCo
            JOIN Suspended_Accounts_Transactions SAT 
                ON SAT.suspended_transdetail_id = TCo.Transdetail_id
            JOIN Account A 
                ON A.account_id = TCo.Account_id
            JOIN Ledger L 
                ON L.ledger_id = A.ledger_id
            WHERE SAT.linked_transdetail_id = @Transdetail_id
            AND L.ledger_short_name = 'CO'
        ),
        (
            SELECT 
                ISNULL(SUM(TCoE.amount),0)  
            FROM Transdetail TCoE 
            JOIN Released_Accounts_Transactions RAT 
                ON RAT.destination_transdetail_id = TCoE.transdetail_id
            JOIN Suspended_Accounts_Transactions SAT 
                ON SAT.suspended_transdetail_id = RAT.suspended_transdetail_id
            JOIN Allocation Al 
                ON Al.allocation_id = RAT.allocation_id
            JOIN Transdetail TCo 
                ON TCo.document_id = TCoE.document_id
            JOIN Account A 
                ON A.account_id = TCo.Account_id
            JOIN Ledger L 
                ON L.ledger_id = A.ledger_id
            WHERE SAT.linked_transdetail_id = @Transdetail_id
            AND TCoE.transdetail_id <> TCo.Transdetail_id
            AND Al.account_id = @Account_id
            AND L.ledger_short_name = 'CO'
        ),
        NULL
        
    FETCH NEXT FROM c_debtors INTO
        @Ledger_Name,
        @Account_Id,
        @Account_code,
        @Account_name,
        @Document_id,
        @Document_ref,
        @Transdetail_Id,    
        @Amount,
        @Amount_outstanding,
        @split_perc 
END

CLOSE c_debtors
DEALLOCATE c_debtors 

SELECT 
    *
FROM #TempRevenueDue

DROP TABLE #TempRevenueDue
    
     
 
