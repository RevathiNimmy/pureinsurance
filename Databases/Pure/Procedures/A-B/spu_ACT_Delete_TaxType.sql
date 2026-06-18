SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_TaxType'
GO


CREATE PROCEDURE spu_ACT_Delete_TaxType
    @taxtype_id smallint
AS


DELETE FROM TaxType
WHERE taxtype_id = @taxtype_id
GO


