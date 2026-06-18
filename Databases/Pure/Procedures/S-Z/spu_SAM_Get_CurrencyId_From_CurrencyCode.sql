SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SAM_Get_CurrencyId_From_CurrencyCode'
GO


CREATE PROCEDURE spu_SAM_Get_CurrencyId_From_CurrencyCode
@currencyCode AS VARCHAR(5) =NULL,
@currencyId AS INT OUTPUT
AS


SELECT @currencyId=currency_id
FROM Currency
WHERE iso_code=@currencyCode
ORDER BY code
GO


