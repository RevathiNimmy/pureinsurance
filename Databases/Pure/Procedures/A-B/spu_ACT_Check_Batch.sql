SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Batch'
GO


CREATE PROCEDURE spu_ACT_Check_Batch
    @batch_id int OUTPUT
AS


BEGIN
    SELECT @batch_id = batch_id
    FROM Batch
    WHERE batch_id = @batch_id
END
BEGIN
IF @batch_id = NULL
    SELECT @batch_id = -1
END
GO


