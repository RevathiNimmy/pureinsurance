CREATE FUNCTION [dbo].[GetTaxFactor]
(
	@tax_group_id int,
	@effective_date	datetime
)
RETURNS money
AS
BEGIN
	DECLARE @result money
	DECLARE @rate   float
	
	 
	
	SELECT TOP 1 @rate = dbo.tax_band_rate.rate 
	FROM	dbo.tax_band_rate 
	INNER JOIN	dbo.tax_group_tax_band ON dbo.tax_group_tax_band.tax_band_id =dbo.tax_band_rate.tax_band_id
	WHERE		dbo.tax_group_tax_band.tax_group_id = @tax_group_id
	AND			dbo.tax_band_rate.effective_date <= @effective_date
	ORDER BY	dbo.tax_band_rate.effective_date DESC
	
	IF (@rate IS NOT NULL)
		SELECT @result = 1 + (@rate/100)
	ELSE
		SELECT @result = 1
	
	RETURN @result

END

GO


