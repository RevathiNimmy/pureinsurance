SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_TaxType'
GO


CREATE PROCEDURE spu_ACT_Check_TaxType
    @taxtype_id smallint OUTPUT
AS


BEGIN
    SELECT @taxtype_id = taxtype_id
    FROM TaxType
    WHERE taxtype_id = @taxtype_id
END
BEGIN
IF @taxtype_id = NULL
    SELECT @taxtype_id = -1
END
GO


