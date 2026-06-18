SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Allocation'
GO


CREATE PROCEDURE spu_ACT_Delete_Allocation
    @allocation_id int
AS


DELETE FROM Allocation
WHERE allocation_id = @allocation_id
GO


