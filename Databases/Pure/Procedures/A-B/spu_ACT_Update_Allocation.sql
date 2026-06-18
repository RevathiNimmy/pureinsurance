EXECUTE DDLDropProcedure 'spu_ACT_Update_Allocation'
GO

CREATE PROCEDURE spu_ACT_Update_Allocation
    @allocation_id int,
    @company_id smallint,
    @account_id int,
	@user_id smallint,
    @allocation_date datetime,
    @allocationstatus_id int,
    @nAllocationbatch_id int = Null 
AS

BEGIN
UPDATE Allocation
    SET
    company_id=@company_id,
    account_id=@account_id,
    user_id=@user_id,
    allocation_date=@allocation_date,
    allocationstatus_id=@allocationstatus_id,
	Allocationbatch_id=@nAllocationbatch_id
WHERE allocation_id = @allocation_id
END
GO


