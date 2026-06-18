EXECUTE DDLDropProcedure 'spu_ACT_Get_ReleasedAccountsTransactions_ForReversal'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC spu_ACT_Get_ReleasedAccountsTransactions_ForReversal
    @DocumentID INT,
    @TransdetailID INT

AS BEGIN
SET NOCOUNT ON
DECLARE @Accounting_Period  INT
DECLARE @Year_name  INT

SELECT TOP 1 @Year_name = Year(period_end_date) FROM period WHERE period_end_complete = 0  

SELECT DISTINCT @Accounting_Period = year(accounting_date)  FROM transdetail WHERE document_id= @DocumentID
 
IF EXISTS 
     (SELECT document_id FROM document d
        JOIN documenttype dt ON d.documenttype_id = dt.documenttype_id
    WHERE d.document_id = @DocumentID
    AND dt.code IN('INC','IRC'))
    begin
/*Instalment Reversal*/
        SELECT  
            t2.document_id,
            rat.suspended_transdetail_id, 
            ad2.alloc_base_amount, 
            ad2.transdetail_id, 
            ad2.alloc_base_amount * -1, 
            rat.allocation_id
        FROM    Transdetail t
        JOIN    AllocationDetail ad ON ad.transdetail_id = t.transdetail_id
        JOIN    Released_Accounts_Transactions rat ON rat.allocation_id = ad.allocation_id
        JOIN    Transdetail t2 ON t2.transdetail_id = rat.destination_transdetail_id
        JOIN    Transdetail t3 ON t2.document_id = t3.document_id
        JOIN    AllocationDetail ad2 ON ad2.transdetail_id = t3.transdetail_id  
            WHERE t.transdetail_id = @TransdetailID
            AND t3.transdetail_id <> rat.destination_transdetail_id
        AND rat.recall_date is NULL

         
    END
ELSE
/* Standard Reversal */
/* Reversal of Allocated Amount in Same Accounting Period */
IF @Accounting_Period = @Year_name
 BEGIN
    SELECT  
       t2.document_id,
       rat.suspended_transdetail_id, 
       ad2.alloc_base_amount, 
       ad2.transdetail_id, 
       ad.alloc_base_amount, 
       NULL
    FROM   Released_Accounts_Transactions rat 
        JOIN   transdetail t ON rat.suspended_transdetail_id = t.transdetail_id
    JOIN   allocationdetail ad ON ad.transdetail_Id = rat.suspended_transdetail_id
    JOIN   allocationdetail ad2 ON ad.allocation_id = ad2.allocation_id
        JOIN   transdetail t2 ON t2.transdetail_id = ad2.transdetail_id
        WHERE t.document_id = @DocumentID
        AND ad2.transdetail_id <> rat.suspended_transdetail_id
    AND rat.recall_date is NULL
 
END



END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 

