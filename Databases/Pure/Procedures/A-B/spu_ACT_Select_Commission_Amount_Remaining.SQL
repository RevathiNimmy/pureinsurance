SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Commission_Amount_Remaining'
GO

-- Gets the amount of commission left to pay

CREATE PROCEDURE spu_ACT_Select_Commission_Amount_Remaining

    @lTransDetailId INT

AS

-- Get the origial transdetail amount
DECLARE @crOriginalAmount MONEY
SELECT
    @crOriginalAmount = amount
FROM
    TransDetail
WHERE
    transdetail_id = @lTransDetailId

-- Get the amount that has been allocated against the original amount
DECLARE @crAllocatedAmount MONEY
SELECT
    @crAllocatedAmount = SUM(alloc_base_amount)
FROM
    AllocationDetail
WHERE
    transdetail_id = @lTransDetailId


-- Return the amount remaining to pay
SELECT
    @crOriginalAmount - @crAllocatedAmount 'Amount Remaining'
GO


