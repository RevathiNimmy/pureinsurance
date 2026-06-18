SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_PFInstalments_get_instalments_tax'
GO

CREATE PROCEDURE spu_PFInstalments_get_instalments_tax
	@pfprem_finance_cnt INT,
	@pfprem_finance_version INT,
	@tax_amount MONEY
AS

DECLARE @band_count INT
DECLARE @total_tax MONEY

-- Get the total tax for the plan
SELECT 	@band_count=COUNT(TC.tax_calculation_cnt),
		@total_tax=SUM(TC.value)
FROM	Tax_Calculation TC
WHERE	TC.pfprem_finance_cnt=@pfprem_finance_cnt
AND		TC.pfprem_finance_version=@pfprem_finance_version

IF @band_count<=1 BEGIN
	-- We have one band so just return the amount and account

	SELECT	A.account_id,
			@tax_amount tax_amount,
			'NOTA'+TB.code tax_account_code
	FROM	Tax_Calculation TC
	JOIN	Tax_Band TB ON TB.tax_band_id=TC.tax_band_id
	LEFT JOIN
			Account A ON A.short_code='NOTA'+TB.code
	WHERE	TC.pfprem_finance_cnt=@pfprem_finance_cnt
	AND		TC.pfprem_finance_version=@pfprem_finance_version
END
ELSE BEGIN
	DECLARE @account_id INT,
			@remainder MONEY,
			@tax_band_amount MONEY,
			@tax_account_code VARCHAR(20),
			@row_count INT

	SELECT @remainder=@tax_amount
	SELECT @row_count=1

	-- Build a split of bands
	CREATE TABLE #band_amounts
	(	account_id INT,
		tax_amount MONEY,
		tax_account_code VARCHAR(20))

	DECLARE bands_cursor CURSOR FAST_FORWARD FOR
		SELECT	A.account_id,
				ROUND(@tax_amount*(TC.value/@total_tax),2),
				'NOTA'+TB.code tax_account_code
		FROM	Tax_Calculation TC
		JOIN	Tax_Band TB ON TB.tax_band_id=TC.tax_band_id
		LEFT JOIN
				Account A ON A.short_code='NOTA'+TB.code
		WHERE	TC.pfprem_finance_cnt=@pfprem_finance_cnt
		AND		TC.pfprem_finance_version=@pfprem_finance_version

	OPEN bands_cursor

	FETCH NEXT FROM bands_cursor 
	INTO @account_id, @tax_band_amount, @tax_account_code
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- the last row has the remainder
		IF @row_count=@band_count
			SELECT @tax_band_amount=@remainder
		ELSE
			SELECT @remainder=@remainder-@tax_band_amount

		INSERT INTO #band_amounts 
		VALUES (@account_id, @tax_band_amount, @tax_account_code)

		FETCH NEXT FROM bands_cursor 
		INTO @account_id, @tax_band_amount, @tax_account_code

		SELECT @row_count=@row_count+1
	END
	SELECT account_id, tax_amount, tax_account_code 
	FROM #band_amounts

	CLOSE bands_cursor
	DEALLOCATE bands_cursor

	DROP TABLE #band_amounts
END
GO