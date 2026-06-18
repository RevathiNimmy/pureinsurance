SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_AccountType'
GO


CREATE PROCEDURE spu_ACT_Check_AccountType
    @accounttype_id smallint OUTPUT
AS


BEGIN
    SELECT @accounttype_id = accounttype_id
    FROM AccountType
    WHERE accounttype_id = @accounttype_id
END
BEGIN
IF @accounttype_id = NULL
    SELECT @accounttype_id = -1
END
GO


