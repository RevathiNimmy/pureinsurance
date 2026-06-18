SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_AllocationStatus'
GO


CREATE PROCEDURE spu_ACT_Check_AllocationStatus
    @allocationstatus_id int OUTPUT
AS


BEGIN
    SELECT @allocationstatus_id = allocationstatus_id
    FROM AllocationStatus
    WHERE allocationstatus_id = @allocationstatus_id
END
BEGIN
IF @allocationstatus_id = NULL
    SELECT @allocationstatus_id = -1
END
GO


