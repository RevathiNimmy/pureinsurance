SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Add_Allocation'
GO

CREATE PROCEDURE spu_ACT_Add_Allocation
    @allocation_id INT OUTPUT,
    @company_id SMALLINT,
    @account_id INT,
    @user_id SMALLINT,
    @allocation_date DATETIME,
    @allocationstatus_id INT,
    @nAllocationbatch_id INT = Null
AS

BEGIN
	INSERT INTO Allocation (
		company_id,
		account_id,
		user_id,
		allocation_date,
		allocationstatus_id,
		allocationbatch_id)
	VALUES (
		@company_id,
		@account_id,
		@user_id,
		@allocation_date,
		@allocationstatus_id,
		@nAllocationbatch_id)
END
BEGIN
	SELECT @allocation_id = @@IDENTITY
END
GO


