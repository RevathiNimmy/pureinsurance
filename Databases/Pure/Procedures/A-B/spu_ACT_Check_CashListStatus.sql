SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_CashListStatus'
GO


CREATE PROCEDURE spu_ACT_Check_CashListStatus
    @cashliststatus_id int OUTPUT
AS


BEGIN
    SELECT @cashliststatus_id = cashliststatus_id
    FROM CashListStatus
    WHERE cashliststatus_id = @cashliststatus_id
END
BEGIN
IF @cashliststatus_id = NULL
    SELECT @cashliststatus_id = -1
END
GO


