SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_AccountType'
GO


CREATE PROCEDURE spu_ACT_Delete_AccountType
    @accounttype_id smallint
AS


DELETE FROM AccountType
WHERE accounttype_id = @accounttype_id
GO


