SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Bank'
GO


CREATE PROCEDURE spu_ACT_Check_Bank
    @bank_id smallint OUTPUT
AS


BEGIN
    SELECT @bank_id = bank_id
    FROM Bank
    WHERE bank_id = @bank_id
END
BEGIN
IF @bank_id = NULL
    SELECT @bank_id = -1
END
GO


