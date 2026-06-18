SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_BatchStatus'
GO


CREATE PROCEDURE spu_ACT_Check_BatchStatus
    @batchstatus_id smallint OUTPUT
AS


BEGIN
    SELECT @batchstatus_id = batchstatus_id
    FROM BatchStatus
    WHERE batchstatus_id = @batchstatus_id
END
BEGIN
IF @batchstatus_id = NULL
    SELECT @batchstatus_id = -1
END
GO


