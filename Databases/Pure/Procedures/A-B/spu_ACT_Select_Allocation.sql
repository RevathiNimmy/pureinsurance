EXECUTE DDLDropProcedure 'spu_ACT_Select_Allocation'
GO

CREATE PROCEDURE spu_ACT_Select_Allocation
    @allocation_id INT
AS

SELECT
    allocation_id,
    company_id,
    account_id,
    user_id,
    allocation_date,
    allocationstatus_id,	
    allocationbatch_id
FROM Allocation
WHERE allocation_id = @allocation_id

GO


