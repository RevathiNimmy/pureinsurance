SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Currency'
GO


CREATE PROCEDURE spu_ACT_Delete_Currency
    @currency_id smallint
AS


DELETE FROM Currency
WHERE currency_id = @currency_id
GO


