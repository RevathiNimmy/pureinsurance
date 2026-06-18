SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Comm_For_Allocation_For_Document'
GO

CREATE PROCEDURE spu_ACT_Select_Comm_For_Allocation_For_Document  
    @account_id int,  
    @document_id varchar(40)  
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
        AND t.document_id  = @document_id  
        AND t.account_id = a.account_id
        AND spare = 'COMM'  
  
    DECLARE c_OS CURSOR FAST_FORWARD FOR  
        SELECT transdetail_id  
        FROM #OSTransactions  
    OPEN c_OS  
    FETCH NEXT FROM c_OS INTO @transdetail_id  
    WHILE @@FETCH_STATUS = 0 BEGIN  
        UPDATE #OSTransactions  
            SET amount = amount - (  
                SELECT ISNULL(SUM(base_match_amount), 0)  
                FROM TransMatch  
                WHERE transdetail_id = @transdetail_id  
                AND allocationdetail_id IS NOT NULL  
            ),  
            currency_amount = currency_amount - (  
                SELECT ISNULL(SUM(currency_match_amount), 0)  
                FROM TransMatch  
                WHERE transdetail_id = @transdetail_id  
                AND allocationdetail_id IS NOT NULL  
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
