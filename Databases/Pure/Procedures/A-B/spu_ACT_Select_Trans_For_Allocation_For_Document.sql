SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_Select_Trans_For_Allocation_For_Document'
GO

CREATE PROCEDURE spu_ACT_Select_Trans_For_Allocation_For_Document  
    @account_id INT,    
    @document_id VARCHAR(40)    
AS    
BEGIN    
    DECLARE @transdetail_id INT    
    CREATE TABLE #OSTransactions (    
        transdetail_id INT,    
        amount NUMERIC(19,4),    
        currency_amount NUMERIC(19,4)  ,
		amount_currency_id INT		
    )    
  
    INSERT INTO #OSTransactions    
    SELECT t.transdetail_id,    
        t.amount,    
        t.currency_amount,
		t.amount_currency_id		
        FROM transdetail t, account a    
        WHERE a.account_id = @account_id    
        AND t.document_id  = @document_id  
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
