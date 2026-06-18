SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_AllocationDetail'
GO


CREATE PROCEDURE spu_ACT_Delete_AllocationDetail
    @allocationdetail_id int
AS


DELETE FROM AllocationDetail
WHERE allocationdetail_id = @allocationdetail_id
GO


