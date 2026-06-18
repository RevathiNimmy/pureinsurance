SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AllocationStatus'
GO


CREATE PROCEDURE spu_ACT_Select_AllocationStatus
    @allocationstatus_id int
AS


SELECT
    allocationstatus_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM AllocationStatus
WHERE allocationstatus_id = @allocationstatus_id
GO


