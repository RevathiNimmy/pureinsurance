EXECUTE DDLDropProcedure 'spu_Get_Rounding_Section_Amounts'
GO


SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_Get_Rounding_Section_Amounts
	@risk_cnt INT,
	@original_flag INT = NULL,
	@annual_premium MONEY OUTPUT,
    @this_premium MONEY OUTPUT
AS

DECLARE
	@total_annual_premium MONEY,
    	@total_this_premium MONEY, 
	@decimal_point_location INT,
	@decimal_val MONEY,
	@whole_val INT,
	@str_val VARCHAR(255)

SELECT 
	@total_annual_premium = SUM(annual_premium),
	@total_this_premium = SUM(this_premium)
FROM rating_section
WHERE risk_cnt = @risk_cnt
AND original_flag = ISNULL(@original_flag,original_flag)
SELECT
	@str_val = CONVERT(varchar,@total_this_premium)

SELECT 
	@decimal_point_location = CHARINDEX('.',@str_val)

IF @decimal_point_location>0 
BEGIN
	 SELECT @decimal_val = convert(MONEY,substring(@str_val,@decimal_point_location,LEN(@str_val)))
	 SELECT @whole_val = convert(int,left(@str_val,@decimal_point_location-1))
END
IF @whole_val>0 AND @decimal_val>.5
BEGIN
	SELECT @whole_val = @whole_val + 1
END
ELSE IF @whole_val<0 AND @decimal_val>.5
BEGIN 
	SELECT @whole_val = @whole_val - 1
END
ELSE IF @decimal_val =.5 AND (@whole_val % 2) = 0
BEGIN 
	IF @whole_val > 0
	BEGIN
		SET @whole_val = @whole_val + 1
	END
	ELSE IF @whole_val < 0
	BEGIN
		SET @whole_val = @whole_val - 1
	END
END
ELSE IF @decimal_val > .5
BEGIN
	IF @whole_val > 0
	BEGIN
		SET @whole_val = 1
	END
	ELSE IF @whole_val < 0
	BEGIN
		SET @whole_val =  - 1
	END	
END

SELECT
	@annual_premium = ROUND(@total_annual_premium,0) - @total_annual_premium,
	@this_premium = @whole_val - @total_this_premium


GO
