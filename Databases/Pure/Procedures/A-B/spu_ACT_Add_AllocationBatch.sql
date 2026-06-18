
EXECUTE DDLDropProcedure 'spu_ACT_Add_AllocationBatch'
GO

CREATE PROCEDURE spu_ACT_Add_AllocationBatch
 @nAllocation_batch_id INT OUTPUT,  
 @nReversed_allocation_batch_id INT = NULL,
 @allocationbatch_date date = NULL
AS  
  
DECLARE @period_id INT  
  
SELECT @period_id = period_id  
FROM   Period  
WHERE  company_id = 1 AND sub_branch_id = 1
AND    period_end_date =  
       (SELECT min (period_end_date)  
        FROM   Period  
        WHERE  sub_branch_id= 1 and  
	period_end_date >= ISNULL(@allocationbatch_date,GETDATE())              
               AND ISNULL(period_end_complete,0)=0)       
  
INSERT INTO AllocationBatch
(  
	allocationbatch_date,
	period_id,
	is_reversed,
	reversed_allocation_batch_id
)  
VALUES  
(  
ISNULL(@allocationbatch_date,GETDATE()),
@period_id,
Case ISNULL(@nReversed_allocation_batch_id, 0)
	WHEN 0 THEN 0
	ELSE 1 END,
@nReversed_allocation_batch_id
)  
  
SELECT @nAllocation_batch_id = @@IDENTITY  

GO