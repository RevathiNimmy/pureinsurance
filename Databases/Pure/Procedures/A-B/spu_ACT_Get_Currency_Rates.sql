EXEC DDLDropProcedure 'spu_ACT_Get_Currency_Rates'
GO

CREATE PROCEDURE spu_ACT_Get_Currency_Rates
	@company_id INT
AS

DECLARE @TypeOfRates TINYINT

EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT

IF @TypeOfRates = 1
BEGIN
	SELECT @company_id = 1
END

SELECT
	effective_from,
	rate_against_base,
	currency_id
FROM CurrencyRate
WHERE company_id = @company_id
ORDER BY
	currency_id ASC,
	effective_from DESC

GO

