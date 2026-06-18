SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_BankAccount_Cash'
GO


CREATE PROCEDURE spu_ACT_Get_BankAccount_Cash
    @bankaccount_id int
AS


SELECT cashlist_id 
FROM cashlist
WHERE bankaccount_id = @bankaccount_id
GO

 


