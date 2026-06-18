SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_Currency_For_Risk'
GO

CREATE PROCEDURE spu_Get_Policy_Currency_For_Risk
    @insurance_file_cnt INT,
	@source_id INT OUTPUT,
	@original_currency_iso_code VARCHAR(4) OUTPUT,
	@original_currency_name VARCHAR(255) OUTPUT,
	@original_currency_symbol VARCHAR(4) OUTPUT,
	@new_currency_iso_code VARCHAR(4) OUTPUT,
	@new_currency_name VARCHAR(255) OUTPUT,
	@new_currency_symbol VARCHAR(4) OUTPUT,
	@combined_rate FLOAT OUTPUT
AS

DECLARE @original_currency_id SMALLINT
DECLARE @original_source_id SMALLINT
DECLARE @original_currency_rate FLOAT
DECLARE @original_date DATETIME
DECLARE @new_currency_id SMALLINT
DECLARE @new_source_id SMALLINT
DECLARE @new_currency_rate FLOAT
DECLARE @new_date DATETIME

DECLARE @original_risk_cnt INT

/*Get new policy details*/
SELECT
	@new_currency_id = i.currency_id,
	@new_source_id = i.source_id,
	@new_currency_rate = i.currency_base_xrate,
	@new_date = i.cover_start_date,
	@source_id = i.source_id
FROM insurance_file i
WHERE i.insurance_file_cnt = @insurance_file_cnt

/*Get new policy currency details*/
SELECT 
	@new_currency_iso_code = iso_code,
	@new_currency_name = description,
	@new_currency_symbol = symbol
FROM currency
WHERE currency_id = @new_currency_id 

/*Get the original risk cnt*/
SELECT @original_risk_cnt = MAX(ISNULL(original_risk_cnt,0))
FROM insurance_file_risk_link
WHERE insurance_file_cnt = @insurance_file_cnt

IF ISNULL(@original_risk_cnt,0) = 0
BEGIN
	/*Default values*/
	SELECT @original_currency_iso_code = @new_currency_iso_code
	SELECT @original_currency_name = @new_currency_name
	SELECT @original_currency_symbol = @new_currency_symbol
	SELECT @combined_rate = 1
END
ELSE
BEGIN

	/*Get original policy details*/
	SELECT 
		@original_currency_id = currency_id,
		@original_source_id = source_id,
		@original_currency_rate = currency_base_xrate,
		@original_date = cover_start_date
	FROM insurance_file
	WHERE insurance_file_cnt = 
		(
			SELECT MAX(ifi.insurance_file_cnt)
			FROM insurance_file_risk_link ifrl
			JOIN insurance_file ifi
				ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
			WHERE ifrl.risk_cnt = @original_risk_cnt
			AND ISNULL(ifi.insurance_file_status_id,3) in (3, 4, 5)
			AND ifi.insurance_file_type_id IN (2,5)
		)

	SELECT 
		@original_currency_iso_code = iso_code,
		@original_currency_name = description,
		@original_currency_symbol = symbol
	FROM currency
	WHERE currency_id = @original_currency_id 

	/*Calculate single rate to go from old currency to new currency*/
	IF @new_currency_id = @original_currency_id
	BEGIN
		/*Don't convert amounts if both policy versions are in they same currency, even if they have different rates.*/
		SELECT @combined_rate = 1
	END
	ELSE
	BEGIN
		/*If new rate wasn't overridden then get the rate from currencyrate table*/
		IF ISNULL(@new_currency_rate,0) = 0
		BEGIN

			EXEC spu_ACT_Get_Currency_Rate 
				@new_currency_id, 
				@new_source_id,
				@new_date,
				@new_currency_rate OUTPUT	

		END

		/*If original rate wasn't overridden then get the rate from currencyrate table*/
		IF ISNULL(@original_currency_rate,0) = 0
		BEGIN

			EXEC spu_ACT_Get_Currency_Rate 
				@original_currency_id, 
				@original_source_id,
				@original_date,
				@original_currency_rate OUTPUT

		END

		SELECT @combined_rate = @original_currency_rate / @new_currency_rate
	END
END