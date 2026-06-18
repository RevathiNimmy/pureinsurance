SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_ACT_SuspendedAccountsTransactions_Sel'
GO


CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactions_Sel
    @AllocationId INT = NULL,
    @LinkedTransdetailId INT = NULL,
    @DocumentID INT = NULL,
    @PFPremFinanceCnt INT  = NULL,
    @PFPremFinanceVersion INT = NULL,
    @SuspendedTransdetailID INT = NULL
    
AS

DECLARE 
    @Branch_id INT,
    @SysOption16 INT,
    @SysOption94 INT

SELECT 
    @branch_id = company_id 
FROM transdetail 
WHERE transdetail_id = @LinkedTransdetailId

SELECT 
    @SysOption16  = RTRIM(value)
FROM system_options
WHERE option_number = 16
AND branch_id = @branch_id

SELECT @Sysoption94 = 0

IF @sysoption16 = 1
BEGIN
    SELECT 
        @sysoption94 = RTRIM(value)
    FROM system_options
    WHERE option_number = 94
    AND branch_id = @branch_id
END

IF @PFPremFinanceCnt = 0
BEGIN
    SELECT @PFPremFinanceCnt = NULL
END

IF @PFPremFinanceVersion = 0
BEGIN
    SELECT @PFPremFinanceVersion = NULL
END

IF @SuspendedTransdetailID = 0
BEGIN
    SELECT @SuspendedTransdetailID = NULL
END

If  @SuspendedTransdetailID IS NOT NULL
BEGIN
    SELECT DISTINCT
    sat.suspended_transdetail_id,
    sat.linked_transdetail_id,
    sat.linked_percentage/100,
    sat.pfprem_finance_cnt,
    sat.pfprem_finance_version,
    isnull(sat.insurance_file_cnt,0),
    sat.destination_account_id,
    sat.documenttype_id,
    sat.transdetail_type_id,
    sat.spare,
    t.outstanding_amount,
    d.document_ref,
    tt.code
    FROM suspended_accounts_transactions sat
    join transdetail t on t.transdetail_id = sat.suspended_transdetail_id
    join document d on t.document_id = d.document_id
    join transdetail_type tt on sat.transdetail_type_id = tt.transdetail_type_id
    where sat.suspended_transdetail_id = @SuspendedTransdetailID
    and sat.is_deleted = 0 
    AND sat.manually_released = 0  --'(RC) PLICO 9-10 
    RETURN
END

IF @DocumentID IS NOT NULL
BEGIN
    SELECT DISTINCT
        sat.suspended_transdetail_id,
        sat.linked_transdetail_id,
        sat.linked_percentage/100,
        sat.pfprem_finance_cnt,
        sat.pfprem_finance_version,
        ISNULL(sat.insurance_file_cnt,0),
        sat.destination_account_id,
        sat.documenttype_id,
        sat.transdetail_type_id,
        sat.spare,
        td.outstanding_amount,
        d.document_ref,
        tt.code
    FROM suspended_accounts_transactions sat
    JOIN transdetail td 
        ON td.transdetail_id = sat.suspended_transdetail_id
    JOIN document d 
        ON d.document_id = td.document_id
    JOIN transdetail_type tt
        ON sat.transdetail_type_id = tt.transdetail_type_id
    WHERE d.document_id = @DocumentID
    AND sat.is_deleted = 0
    AND sat.manually_released = 0 --'(RC) PLICO 9-10 

    RETURN

END


IF @LinkedTransdetailId IS NOT NULL
BEGIN
    SELECT DISTINCT
        sat.suspended_transdetail_id,
        sat.linked_transdetail_id,
        sat.linked_percentage/100,
        sat.pfprem_finance_cnt,
        sat.pfprem_finance_version,
        ISNULL(sat.insurance_file_cnt,0),
        sat.destination_account_id,
        sat.documenttype_id,
        sat.transdetail_type_id,
        sat.spare,
        td.outstanding_amount,
        d.document_ref,
        tt.code
    FROM suspended_accounts_transactions sat
    JOIN transdetail td 
        ON td.transdetail_id = sat.suspended_transdetail_id
    JOIN document d 
        ON d.document_id = td.document_id
    JOIN transdetail td2 
        ON td2.transdetail_id = sat.linked_transdetail_id   
    JOIN account a 
        ON a.account_id = td2.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id
    JOIN transdetail_type tt
        ON sat.transdetail_type_id = tt.transdetail_type_id
    WHERE sat.linked_transdetail_id = @LinkedTransdetailId
    AND sat.is_deleted = 0
    --AND sat.manually_released = 0  --'(RC) PLICO 9-10 
    AND (
            td2.outstanding_amount = 0 
            OR 
            (
                @Sysoption94 = 1 
                AND 
                l.ledger_short_name <> "IN"
            )
        ) 
    AND NOT EXISTS 
        (
            SELECT 
                NULL 
            FROM released_accounts_transactions
            WHERE suspended_transdetail_id = sat.suspended_transdetail_id
            AND allocation_id = @AllocationId
        )
    AND NOT EXISTS 
        (
            SELECT
                NULL
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name = 'SA'
            JOIN allocationdetail ad
                ON ad.transdetail_id = td.transdetail_id
            JOIN allocationdetail ad2
                ON ad2.allocation_id = ad.allocation_id
                AND ad2.allocationdetail_id <> ad.allocationdetail_id
            JOIN transdetail td2
                ON td2.transdetail_id = ad2.transdetail_id
            JOIN transdetail_type tt2
                ON tt2.transdetail_type_id = td2.transdetail_type_id
                AND tt2.code = 'DIRECTDEBIT'
            WHERE td.transdetail_id = @LinkedTransdetailId
            AND ad.allocation_id = @AllocationId
            AND EXISTS
                (
                    SELECT
                        NULL
                    FROM allocationdetail
                    WHERE allocation_id = ad.allocation_id
                    HAVING SUM(1) = 2
                )
        )

        
    RETURN

END

IF @PFPremFinanceCnt IS NOT NULL AND @PFPremFinanceVersion IS NOT NULL
BEGIN
    SELECT DISTINCT
        sat.suspended_transdetail_id,
        sat.linked_transdetail_id,
        sat.linked_percentage/100,
        sat.pfprem_finance_cnt,
        sat.pfprem_finance_version,
        ISNULL(sat.insurance_file_cnt,0),
        sat.destination_account_id,
        sat.documenttype_id,
        sat.transdetail_type_id,
        sat.spare,
        td.outstanding_amount,
        d.document_ref,
        tt.code
    FROM suspended_accounts_transactions sat
    JOIN transdetail td 
        ON td.transdetail_id = sat.suspended_transdetail_id
    JOIN document d 
        ON d.document_id = td.document_id
    JOIN transdetail_type tt
        ON sat.transdetail_type_id = tt.transdetail_type_id
    WHERE sat.pfprem_finance_cnt = @PFPremFinanceCnt
    AND sat.pfprem_finance_version = @PFPremFinanceVersion
    AND sat.is_deleted = 0 
    AND sat.manually_released = 0  --'(RC) PLICO 9-10 
    AND (
            @SuspendedTransdetailID IS NULL 
            OR
            @SuspendedTransdetailID = sat.suspended_transdetail_id
        )
END


GO

