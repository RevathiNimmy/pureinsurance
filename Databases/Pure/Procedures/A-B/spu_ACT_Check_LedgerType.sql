SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_LedgerType'
GO


CREATE PROCEDURE spu_ACT_Check_LedgerType
    @ledgertype_id smallint OUTPUT
AS


BEGIN
    SELECT @ledgertype_id = ledgertype_id
    FROM LedgerType
    WHERE ledgertype_id = @ledgertype_id
END
BEGIN
IF @ledgertype_id = NULL
    SELECT @ledgertype_id = -1
END
GO


