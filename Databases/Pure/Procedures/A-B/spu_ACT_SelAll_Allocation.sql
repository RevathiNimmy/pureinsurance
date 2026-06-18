EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Allocation'
GO

CREATE PROCEDURE spu_ACT_SelAll_Allocation
    @user_id SMALLINT,
    @company_id SMALLINT
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
     WHERE @user_id = user_id
           AND @company_id = company_id
GO


