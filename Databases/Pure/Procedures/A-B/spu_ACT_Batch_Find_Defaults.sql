EXECUTE DDLDropProcedure 'spu_ACT_Batch_Find_Defaults'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_ACT_Batch_Find_Defaults
    @Mode VARCHAR(20)
AS BEGIN

    DECLARE @BatchTypeID INT
    DECLARE @BatchStatusID INT

    IF @Mode = 'APPROVE'
    BEGIN
        SELECT @BatchTypeID = Batch_type_id
        FROM batch_type
        WHERE code = 'PRUN'

        SELECT @BatchStatusID = BatchStatus_id
        FROM BatchStatus
        WHERE code = 'R'

	SELECT 
            @BatchTypeID AS BatchTypeID,
            @BatchStatusID AS BatchStatusID
    END

    IF @Mode = 'DELETE'
    BEGIN
        SELECT @BatchTypeID = Batch_type_id
        FROM batch_type
        WHERE code = 'PRUN'

	SELECT 
            @BatchTypeID AS BatchTypeID,
            0 AS BatchStatusID
    END

END

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
