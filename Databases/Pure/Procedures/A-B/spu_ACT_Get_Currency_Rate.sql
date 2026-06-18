EXEC DDLDropProcedure 'spu_ACT_Get_Currency_Rate'
GO

CREATE PROCEDURE spu_ACT_Get_Currency_Rate
	@currency_id SMALLINT,
	@company_id INT,
	@effective_date DATETIME,
	@rate NUMERIC(12, 8) OUTPUT
	
AS

DECLARE @TypeOfRates TINYINT

EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT

IF @TypeOfRates = 1
BEGIN
	SELECT @company_id = 1
END

SELECT @rate = rate_against_base
FROM CurrencyRate CR
WHERE CR.company_id = @company_id
AND CR.currency_id = @currency_id
AND CR.effective_from =
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= @effective_date
			AND	currency_id = CR.currency_id
			AND company_id = CR.company_id
		)
		
IF @rate = NULL
BEGIN
	SELECT @rate = 1
END

GO

