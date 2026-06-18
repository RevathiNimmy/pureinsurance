SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_SuspendedAccountsTransactions_Rewrite'
GO

CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactions_Rewrite
    @OldTriggerTransdetailId INT,
    @NewTriggerTransdetailId INT = NULL,
    @PFPremFinanceCnt INT = NULL,
    @PFPremFinanceVersion INT = NULL
AS

DECLARE 
    @SuspendedTransdetailId INT,
    @RecordCount INT,
    @commission_option VARCHAR(255),
    @branch_id INT,
    @SubAgentTransdetailId INT,
    @linked_document_id INT,
    @extra_percentage FLOAT


SELECT 
    @branch_id = d.company_id 
FROM document d
JOIN transdetail t 
    ON t.document_id = d.document_id
WHERE t.transdetail_Id = @OldTriggerTransdetailID
  
SELECT 
    @commission_option = RTRIM(value)
FROM system_options
WHERE option_number = 16
AND branch_id = @branch_id

/*Only have to update triggers when commission taken when client pays*/
IF @commission_option <> '1'
BEGIN
    RETURN
END


--Old trigger superceded by SubAgent Transaction
SELECT 
    @SubAgentTransdetailId = ISNULL(MAX(t.transdetail_id), 0)
FROM transdetail t
JOIN account a 
    ON t.account_id = a.account_id
JOIN ledger l 
    ON a.ledger_id = l.ledger_id
JOIN transdetail_type ty 
    ON ty.transdetail_type_id = t.transdetail_type_id 
JOIN transdetail t2 
    ON t.document_id = t2.document_id 
WHERE t2.transdetail_id = @OldTriggerTransdetailId
AND l.ledger_short_name = 'UB'
AND ty.code = 'NET'


IF @SubAgentTransdetailId <> 0
BEGIN
    SELECT @OldTriggerTransdetailId = @SubAgentTransdetailId
END 


IF @PFPremFinanceCnt IS NOT NULL
BEGIN

    IF @PFPremFinanceCnt = 0 
    BEGIN
        SELECT @PFPremFinanceCnt = NULL 
    END
    
    UPDATE suspended_accounts_transactions 
    SET pfprem_finance_cnt = @PFPremFinanceCnt
    WHERE linked_transdetail_id = @OldTriggerTransdetailId
    AND is_deleted = 0
    
END

IF @PFPremFinanceVersion IS NOT NULL
BEGIN
    
    IF @PFPremFinanceVersion = 0
    BEGIN
        SELECT @PFPremFinanceVersion = NULL  
    END
    
    UPDATE suspended_accounts_transactions 
    SET pfprem_finance_version = @PFPremFinanceVersion
    WHERE linked_transdetail_id = @OldTriggerTransdetailId
    AND is_deleted = 0
    
END

IF @NewTriggerTransdetailId IS NOT NULL
BEGIN

    SELECT 
        @SuspendedTransdetailId = ISNULL(MIN(sat.suspended_transdetail_id), 0)
    FROM suspended_accounts_transactions sat
    JOIN transdetail t 
        ON t.transdetail_id = sat.suspended_transdetail_id
    JOIN account a 
        ON a.account_id = t.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id
    WHERE linked_transdetail_id = @OldTriggerTransdetailId
    AND (
            l.ledger_short_name <> 'FE' 
            OR 
            @PFPremFinanceCnt IS NOT NULL
        )
    AND sat.is_deleted = 0
                            
                            
    SELECT 
        @RecordCount = ISNULL(SUM(1), 0)
    FROM suspended_accounts_transactions
    WHERE suspended_transdetail_id = @SuspendedTransdetailId
    AND is_deleted = 0
                     
    
    -- If we have more than one record reversed out remove the duplicates 
    IF @RecordCount > 1
    BEGIN
    
        DELETE FROM  suspended_accounts_transactions
        WHERE suspended_transdetail_id = @SuspendedTransdetailId
        AND linked_transdetail_id <> @OldTriggerTransdetailId
        AND is_deleted = 0
    
    END
    
    SELECT
        @linked_document_id = document_id
    FROM transdetail
    WHERE transdetail_id = @OldTriggerTransdetailId
        
    
    IF @PFPremFinanceCnt IS NULL 
        AND EXISTS
            (
                SELECT
                    NULL
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN party p
                    ON p.party_cnt = a.account_key
                JOIN party_type pt
                    ON pt.party_type_id = p.party_type_id
                    AND pt.code = 'EX'
                WHERE td.document_id = @linked_document_id
            )
    BEGIN

        SELECT
            @extra_percentage = 
                (
                    SELECT 
                        ISNULL(SUM(td.amount), 0)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN party p
                        ON p.party_cnt = a.account_key
                    JOIN party_type pt
                        ON pt.party_type_id = p.party_type_id
                        AND pt.code = 'EX'
                    JOIN transdetail_type tt
                        ON tt.transdetail_type_id = td.transdetail_type_id
                        AND tt.code = 'COMM'
                    WHERE td.document_id = @linked_document_id
                )
                /
                (
                    SELECT 
                        ISNULL(SUM(td.amount), 1)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                    JOIN transdetail_type tt
                        ON tt.transdetail_type_id = td.transdetail_type_id
                        AND tt.code = 'COMM'
                    WHERE td.document_id = @linked_document_id
                )
                
        UPDATE sat
        SET sat.linked_transdetail_id = @NewTriggerTransdetailId,
            sat.linked_percentage = (1 - @extra_percentage) * 100
        FROM suspended_accounts_transactions sat
        JOIN transdetail t 
            ON t.transdetail_id = sat.suspended_transdetail_id
        JOIN account a 
            ON a.account_id = t.account_id
        JOIN ledger l 
            ON l.ledger_id = a.ledger_id        
        WHERE linked_transdetail_id = @OldTriggerTransdetailId
        AND l.ledger_short_name <> 'FE' 
        AND sat.is_deleted = 0   

        IF @extra_percentage <> 0
        BEGIN
            INSERT INTO Suspended_Accounts_Transactions
            (
                suspended_transdetail_id,
                linked_transdetail_id,
                linked_percentage,
                pfprem_finance_cnt, 
                pfprem_finance_version, 
                insurance_file_cnt,
                destination_account_id, 
                documenttype_id, 
                transdetail_type_id,
                spare,
                is_deleted
            )
            SELECT
                suspended_transdetail_id,
                @OldTriggerTransdetailId,
                @extra_percentage * 100,
                pfprem_finance_cnt, 
                pfprem_finance_version, 
                insurance_file_cnt,
                destination_account_id, 
                documenttype_id, 
                transdetail_type_id,
                spare,
                is_deleted
            FROM suspended_accounts_transactions sat
            WHERE suspended_transdetail_id = @SuspendedTransdetailId
        END
             
    END
    ELSE
    BEGIN
    
        UPDATE sat
        SET sat.linked_transdetail_id = @NewTriggerTransdetailId,
            sat.linked_percentage = 100
        FROM suspended_accounts_transactions sat
        JOIN transdetail t 
            ON t.transdetail_id = sat.suspended_transdetail_id
        JOIN account a 
            ON a.account_id = t.account_id
        JOIN ledger l 
            ON l.ledger_id = a.ledger_id        
        WHERE linked_transdetail_id = @OldTriggerTransdetailId
        AND (
                l.ledger_short_name <> 'FE' 
                OR 
                @PFPremFinanceCnt IS NOT NULL
            )
        AND sat.is_deleted = 0
    END

END 

GO

