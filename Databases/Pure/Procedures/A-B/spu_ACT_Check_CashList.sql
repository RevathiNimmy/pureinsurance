SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_CashList'
GO


CREATE PROCEDURE spu_ACT_Check_CashList
    @cashlist_id int OUTPUT
AS


BEGIN
    SELECT @cashlist_id = cashlist_id
    FROM CashList
    WHERE cashlist_id = @cashlist_id
END
BEGIN
IF @cashlist_id = NULL
    SELECT @cashlist_id = -1
END
GO


