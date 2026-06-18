SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Trans_For_Allocation_For_Claim_Payment'
GO


CREATE PROCEDURE spu_ACT_Select_Trans_For_Allocation_For_Claim_Payment
    @account_id int,
    @purchaseinvoiceno varchar(40)	
AS
BEGIN
    DECLARE @transdetail_id int
    CREATE TABLE #OSTransactions (
        transdetail_id int,
        amount numeric(19,4),
        currency_amount numeric(19,4)
    )
    INSERT INTO #OSTransactions
    SELECT t.transdetail_id,
        t.amount,
        t.currency_amount
        FROM transdetail t, account a
        WHERE a.account_id = @account_id
        AND t.purchase_invoice_no = @purchaseinvoiceno
        AND t.account_id = a.account_id
    DECLARE c_OS CURSOR FAST_FORWARD FOR
        SELECT transdetail_id
        FROM #OSTransactions
    OPEN c_OS
    FETCH NEXT FROM c_OS INTO @transdetail_id
    WHILE @@FETCH_STATUS = 0 BEGIN
        UPDATE #OSTransactions
            SET amount = amount - (
                SELECT ISNULL(SUM(alloc_base_amount), 0)
                FROM AllocationDetail 
                WHERE transdetail_id = @transdetail_id and ISNULL(is_reversed,0)=0
                
            ),
            currency_amount = currency_amount - (
                SELECT ISNULL(SUM(alloc_ccy_amount), 0)
                FROM AllocationDetail 
                WHERE transdetail_id = @transdetail_id and ISNULL(is_reversed,0)=0
            )
            WHERE transdetail_id = @transdetail_id
        FETCH NEXT FROM c_OS INTO @transdetail_id
    END
    CLOSE c_OS
    DEALLOCATE c_OS
    DELETE FROM #OSTransactions
        WHERE amount = 0 AND currency_amount = 0
    SELECT * FROM #OSTransactions
    DROP TABLE #OSTransactions
END
GO

