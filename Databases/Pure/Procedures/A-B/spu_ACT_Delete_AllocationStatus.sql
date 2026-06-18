SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_AllocationStatus'
GO


CREATE PROCEDURE spu_ACT_Delete_AllocationStatus
    @allocationstatus_id int
AS


DELETE FROM AllocationStatus
WHERE allocationstatus_id = @allocationstatus_id
GO


