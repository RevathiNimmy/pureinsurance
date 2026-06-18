SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Currency'
GO


CREATE PROCEDURE spu_ACT_Check_Currency
    @currency_id smallint OUTPUT
AS


BEGIN
    SELECT @currency_id = currency_id
    FROM Currency
    WHERE currency_id = @currency_id
END
BEGIN
IF @currency_id = NULL
    SELECT @currency_id = -1
END
GO


