SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Batch'
GO


CREATE PROCEDURE spu_ACT_Delete_Batch
    @batch_id int
AS


DELETE FROM Batch
WHERE batch_id = @batch_id
GO


