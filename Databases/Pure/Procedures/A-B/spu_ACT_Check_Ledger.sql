SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Ledger'
GO


CREATE PROCEDURE spu_ACT_Check_Ledger
    @ledger_id smallint OUTPUT
AS


BEGIN
    SELECT @ledger_id = ledger_id
    FROM Ledger
    WHERE ledger_id = @ledger_id
END
BEGIN
IF @ledger_id = NULL
    SELECT @ledger_id = -1
END
GO


