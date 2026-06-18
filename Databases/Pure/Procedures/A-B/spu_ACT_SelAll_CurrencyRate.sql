EXECUTE DDLDropProcedure 'spu_ACT_SelAll_CurrencyRate'
GO

CREATE PROCEDURE spu_ACT_SelAll_CurrencyRate
	@company_id SMALLINT,
	@effective_date DATETIME
AS
 
SELECT
	R.effective_from,
	R.rate_against_base,
	R.currency_id,
	R.company_id
FROM CurrencyRate R
WHERE (R.company_id = @company_id OR @company_id IS NULL)
AND R.effective_from = 
	(
		SELECT MAX(effective_from)
		FROM CurrencyRate
		WHERE effective_from <= @effective_date
		AND company_id = R.company_id
		AND currency_id = R.currency_id
	)
ORDER BY R.currency_id, R.company_id DESC

GO