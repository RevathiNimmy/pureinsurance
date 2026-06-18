SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Account'
GO


CREATE PROCEDURE spu_ACT_Delete_Account
    @account_id int
AS


DELETE FROM Account
WHERE account_id = @account_id
GO


