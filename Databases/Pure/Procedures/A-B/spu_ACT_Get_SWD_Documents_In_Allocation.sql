EXECUTE DDLDropProcedure 'spu_ACT_Get_SWD_Documents_In_Allocation'
GO

CREATE PROCEDURE spu_ACT_Get_SWD_Documents_In_Allocation
        @TransDetailID  INT = 0,
    @CashListItemID  INT = 0,
    @AllocationID  INT = 0
AS

DECLARE @sSQL NVARCHAR(4000)  

SELECT @sSQL = 'SELECT DISTINCT  
    AD.allocation_id,  
    D.document_ref,  
    D.document_id , TD.spare
FROM TransDetail TD  with (nolock)
JOIN Document D  with (nolock)
    ON D.document_id = TD.document_id      
JOIN AllocationDetail TM with (nolock)    ON TM.transdetail_id = TD.transdetail_id  
JOIN Allocation A1    with (nolock)  ON A1.allocation_id = TM.allocation_id  
JOIN Allocation A2    with (nolock) ON A2.allocationbatch_id = a1.allocationbatch_id  
JOIN AllocationDetail AD with (nolock)    ON AD.allocation_id = A1.allocation_id  
  
WHERE d.documenttype_id IN (14,49) ' 

IF @TransDetailID <> 0  
BEGIN             
SELECT @sSQL = @sSQL + 'AND A2.allocation_id  
                IN  
                (  
                    SELECT  
                        AD2.allocation_id  
                    FROM  
                        AllocationDetail AD2  with (nolock)
                    WHERE  
                        AD2.transdetail_id =  '  + CONVERT(NVARCHAR, @TransDetailID) + ')'
                
END
            
IF @CashListItemID <> 0              
BEGIN
SELECT @sSQL = @sSQL + ' AND A2.allocation_id  
                IN  
                (  
                    SELECT  
                        AD2.allocation_id  
                    FROM  
                        TransDetail TD2  with (nolock)
                    INNER JOIN  
                          CashListItem CLI with (nolock) ON TD2.transdetail_id = CLI.transdetail_id  
                    INNER JOIN  
                          AllocationDetail AD2 with (nolock) ON TD2.transdetail_id = AD2.transdetail_id  
                    WHERE   CLI.cashlistitem_id = ' + CONVERT(NVARCHAR,@CashListItemID)  + '  )  '
            
END

IF @AllocationID <> 0              
BEGIN
        SELECT @sSQL = @sSQL + ' AND A2.allocation_id =' + CONVERT(NVARCHAR,@AllocationID)  
END

    
   SELECT @sSQL = @sSQL + 'AND (TM.is_reversed IS NULL OR TM.is_reversed =0)'
   
   SELECT @sSQL = @sSQL + 'AND (TD.spare NOT IN (''Reversed'',''Reversal''))'

   EXECUTE sp_executesql @sSQL

GO
