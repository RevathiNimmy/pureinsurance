EXEC DDLDropProcedure 'spu_ACT_GetNextEffectiveDateForRates'
GO
                 
CREATE PROCEDURE spu_ACT_GetNextEffectiveDateForRates
	@next BIT,
	@company_id SMALLINT,
	@effective_date DATETIME OUTPUT
AS

IF @next = 1
BEGIN
	SELECT
		@effective_date = ISNULL(MIN(R.effective_from),@effective_date)
	FROM CurrencyRate R
	WHERE (R.company_id = @company_id OR @company_id IS NULL)
	AND R.effective_from = 
		(
			SELECT MIN(effective_from)
			FROM CurrencyRate
			WHERE effective_from > @effective_date
			AND (company_id = R.company_id OR @company_id IS NULL)
		)
END
ELSE
BEGIN
	SELECT
		@effective_date = ISNULL(MAX(R.effective_from),@effective_date)
	FROM CurrencyRate R
	WHERE (R.company_id = @company_id OR @company_id IS NULL)
	AND R.effective_from = 
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate
			WHERE effective_from < @effective_date
			AND (company_id = R.company_id OR @company_id IS NULL)
		)
END

GO