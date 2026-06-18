
EXECUTE DDLDropProcedure 'spu_ACT_Select_Primary_Allocation'
GO

CREATE PROCEDURE spu_ACT_Select_Primary_Allocation
    @allocationbatch_id INT
AS

DECLARE @nallocation_id INT
DECLARE @ntransdetail_id INT
DECLARE @dtallocation_date DATETIME

SELECT
    @nallocation_id = MIN(a.allocation_id),
    @dtallocation_date = MIN(AB.allocationbatch_date)
FROM Allocation a
LEFT JOIN AllocationBatch AB ON ab.allocationbatch_id = a.allocationbatch_id
WHERE a.allocationbatch_id = @allocationbatch_id

SELECT
    @ntransdetail_id = MIN(transdetail_id)
FROM AllocationDetail
WHERE allocation_id = @nallocation_id

-- return first item in the batch
SELECT @nallocation_id 'allocation_id', @ntransdetail_id 'transdetail_id', @dtallocation_date 'allocation_date'
GO


