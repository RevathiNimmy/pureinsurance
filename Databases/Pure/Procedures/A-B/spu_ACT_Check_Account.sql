SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Account'
GO


CREATE PROCEDURE spu_ACT_Check_Account
    @account_id int OUTPUT
AS


BEGIN
    SELECT @account_id = account_id
    FROM Account
    WHERE account_id = @account_id
END
BEGIN
IF @account_id = NULL
    SELECT @account_id = -1
END
GO


