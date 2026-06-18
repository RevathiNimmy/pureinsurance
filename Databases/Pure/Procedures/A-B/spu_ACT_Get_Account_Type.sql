SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Account_Type'
GO


CREATE PROCEDURE spu_ACT_Get_Account_Type
    @account_id int,
    @code varchar(10) OUTPUT
AS


DECLARE @accounttype_id smallint
SELECT @code = CONVERT(varchar(10), accounttype_id)
FROM Account
WHERE account_id = @account_id
GO


