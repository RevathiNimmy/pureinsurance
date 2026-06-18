SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_BankAccount'
GO


CREATE PROCEDURE spu_ACT_Check_BankAccount
    @bankaccount_id int OUTPUT
AS


BEGIN
    SELECT @bankaccount_id = bankaccount_id
    FROM BankAccount
    WHERE bankaccount_id = @bankaccount_id
END
BEGIN
IF @bankaccount_id = NULL
    SELECT @bankaccount_id = -1
END
GO


