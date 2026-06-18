SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Set_BankAccount_IsDeleted'
GO


CREATE PROCEDURE spu_ACT_Set_BankAccount_IsDeleted
    @bankaccount_id int
AS


UPDATE bankaccount
SET Is_deleted = 1
WHERE bankaccount_id = @bankaccount_id
GO
