SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_PurgeFrequency'
GO


CREATE PROCEDURE spu_ACT_Check_PurgeFrequency
    @purgefrequency_id smallint OUTPUT
AS


BEGIN
    SELECT @purgefrequency_id = purgefrequency_id
    FROM PurgeFrequency
    WHERE purgefrequency_id = @purgefrequency_id
END
BEGIN
IF @purgefrequency_id = NULL
    SELECT @purgefrequency_id = -1
END
GO


