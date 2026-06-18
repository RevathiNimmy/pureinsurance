SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_DocumentType'
GO


CREATE PROCEDURE spu_ACT_Check_DocumentType
    @documenttype_id smallint OUTPUT
AS


BEGIN
    SELECT @documenttype_id = documenttype_id
    FROM DocumentType
    WHERE documenttype_id = @documenttype_id
END
BEGIN
IF @documenttype_id = NULL
    SELECT @documenttype_id = -1
END
GO


