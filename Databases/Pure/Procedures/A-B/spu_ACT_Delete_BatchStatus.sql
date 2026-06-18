SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_BatchStatus'
GO


CREATE PROCEDURE spu_ACT_Delete_BatchStatus
    @batchstatus_id smallint
AS


DELETE FROM BatchStatus
WHERE batchstatus_id = @batchstatus_id
GO


