SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_CurrencyRate'
GO


CREATE PROCEDURE spu_ACT_Delete_CurrencyRate
    @currency_id int, 
    @effective_from datetime
AS

BEGIN

	DELETE
	FROM	CurrencyRate
	WHERE	currency_id = @currency_id
	AND	effective_from = @effective_from 

END

GO
