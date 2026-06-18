SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_FindAccount'
GO


CREATE PROCEDURE spu_ACT_Do_FindAccount
    @CompanyID int,
    @FullKey varchar(64) = NULL,
    @LedgerID smallint = NULL,
    @AccountName varchar(60) = NULL,
    @AccountType smallint = NULL,
    @ShortCode varchar(20) = NULL,
    @InsuranceRef varchar(30) = NULL,
    @OperatorID smallint = NULL,
    @PurchaseOrderNo varchar(40) = NULL,
    @PurchaseInvoiceNo varchar(40) = NULL,
    @Department varchar(20) = NULL,
    @Spare varchar(20) = NULL,
    @ShowDeleted smallint = NULL,
    @sub_branch_id int
AS


IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @CompanyID, @sub_branch_id OUTPUT


IF (@InsuranceRef IS NULL AND @OperatorID IS NULL AND
    @PurchaseOrderNo IS NULL AND @PurchaseInvoiceNo IS NULL AND
    @Department IS NULL AND @Spare IS NULL)
    BEGIN
    SELECT DISTINCT
    'Full key',
    a.short_code,
    a.account_name,
    a.ledger_id,
    a.accounttype_id,
    a.account_id,
    a.account_key,
    a.nominal_account_id,
    ast.description,
    a.accountstatus_id
    FROM Account a, AccountStatus ast
    WHERE a.sub_branch_id = @sub_branch_id
        AND (a.ledger_id = @LedgerID OR @LedgerID IS NULL)
        AND (a.account_name LIKE @AccountName OR @AccountName IS NULL)
        AND (a.accounttype_id = @AccountType OR @AccountType IS NULL)
        AND (a.short_code LIKE @ShortCode OR @ShortCode IS NULL)
        AND (a.delete_at_purge = 0 OR @ShowDeleted IS NOT NULL)
    AND (a.accountstatus_id = ast.accountstatus_id)
    ORDER BY short_code
    END
ELSE
    BEGIN
    SELECT DISTINCT
    'Full key',
    a.short_code,
    a.account_name,
    a.ledger_id,
    a.accounttype_id,
    a.account_id,
    a.account_key,
    a.nominal_account_id,
    ast.description,
    a.accountstatus_id
    FROM Account a, AccountStatus ast, TransDetail t
    WHERE a.sub_branch_id = @sub_branch_id
        AND a.account_id = t.account_id
        AND (a.ledger_id = @LedgerID OR @LedgerID IS NULL)
        AND (a.account_name LIKE @AccountName OR @AccountName IS NULL)
        AND (a.accounttype_id = @AccountType OR @AccountType IS NULL)
        AND (a.short_code LIKE @ShortCode OR @ShortCode IS NULL)
    AND (a.accountstatus_id = ast.accountstatus_id)
        AND (a.delete_at_purge = 0 OR @ShowDeleted IS NOT NULL)
        AND (   (t.insurance_ref like @InsuranceRef OR @InsuranceRef IS NULL) OR
                (t.operator_id = @OperatorID OR @OperatorID IS NULL) OR
                (t.purchase_order_no like @PurchaseOrderNo OR @PurchaseOrderNo IS NULL) OR
                (t.purchase_invoice_no like @PurchaseInvoiceNo OR @PurchaseInvoiceNo IS NULL) OR
                (t.department like @Department OR @Department IS NULL) OR
                (t.spare like @Spare OR @Spare IS NULL))
    ORDER BY short_code
    END
GO


