EXECUTE DDLDropProcedure 'spu_ACT_Do_GetCurrenciesByDate'
GO


CREATE PROCEDURE spu_ACT_Do_GetCurrenciesByDate
	@effective_date datetime
AS

SELECT	cr.*, cur.iso_code
FROM	currencyrate cr
JOIN	currency cur
ON	cur.currency_id = cr.currency_id
WHERE	cr.effective_from IN 
	(SELECT	max(effective_from)
	FROM	currencyrate
	WHERE	effective_from < @effective_date
	AND	currency_id = cr.currency_id)
ORDER BY cr.currency_id


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

