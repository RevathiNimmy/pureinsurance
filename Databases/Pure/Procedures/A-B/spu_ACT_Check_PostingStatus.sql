SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_PostingStatus'
GO


CREATE PROCEDURE spu_ACT_Check_PostingStatus
    @postingstatus_id smallint OUTPUT
AS


BEGIN
    SELECT @postingstatus_id = postingstatus_id
    FROM PostingStatus
    WHERE postingstatus_id = @postingstatus_id
END
BEGIN
IF @postingstatus_id = NULL
    SELECT @postingstatus_id = -1
END
GO


