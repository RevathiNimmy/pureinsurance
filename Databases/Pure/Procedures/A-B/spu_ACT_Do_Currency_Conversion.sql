EXEC DDLDropProcedure 'spu_ACT_Do_Currency_Conversion'

GO

CREATE PROCEDURE spu_ACT_Do_Currency_Conversion 
		@account_id                INT=NULL,
		@company_id                INT,
		@currency_id               INT,
		@currency_amount_unrounded MONEY,
		@mode                      VARCHAR(10)='ALL',
		@decimal_places			   INT=0,
		@base_currency_id          INT=NULL OUTPUT,
		@base_currency_code        VARCHAR(20)=NULL OUTPUT,
		@base_amount               MONEY=NULL OUTPUT,
		@base_amount_unrounded     MONEY=NULL OUTPUT,
		@account_currency_id       INT=NULL OUTPUT,
		@account_amount            MONEY=NULL OUTPUT,
		@account_amount_unrounded  MONEY=NULL OUTPUT,
		@system_currency_id        INT=NULL OUTPUT,
		@system_amount             MONEY=NULL OUTPUT,
		@system_amount_unrounded   MONEY=NULL OUTPUT,
		@currency_base_xrate       NUMERIC(19, 10)=NULL OUTPUT,
		@currency_base_date        DATETIME=NULL OUTPUT,
		@account_base_xrate        FLOAT=NULL OUTPUT,
		@account_base_date         DATETIME=NULL OUTPUT,
		@system_base_xrate         FLOAT=NULL OUTPUT,
		@system_base_date          DATETIME=NULL OUTPUT,
		@base_decimals             TINYINT = NULL OUTPUT,
		@account_decimals          TINYINT = NULL OUTPUT,
		@system_decimals           TINYINT = NULL OUTPUT,
		@return_status             INT OUTPUT
AS
	DECLARE @currency_per_branch TINYINT
	DECLARE @branch INT
	DECLARE @TypeOfRates TINYINT
	DECLARE @Current_Date DATETIME

	SET @Current_Date = GETDATE()

	-- Get the Consolidated Currency (this is always from System 1)    
	SELECT @system_currency_id = currency_id
	FROM   PMSystem
	WHERE  system_id = 1

	IF @system_currency_id IS NULL
	BEGIN
		SELECT @return_status = -1

		RETURN -- No system currency set    
	END

	-- Convert to the Account Currency    
	IF @account_id IS NOT NULL
	   AND @account_id <> 0
	BEGIN
		SELECT @account_currency_id = A.currency_id
		FROM   ACCOUNT A
		WHERE  A.account_id = @account_id

		IF @account_currency_id IS NULL
		BEGIN
			SELECT @return_status = -2

			RETURN -- No account currency set    
		END
	END

	-- Get the Base Currency   
	SELECT @base_currency_id = base_currency_id
	FROM   SOURCE
	WHERE  source_id = @company_id

	IF ( @currency_id = @system_currency_id
		 AND @currency_id = @base_currency_id
		 AND @currency_id = ISNULL(@account_currency_id, @currency_id) )
	BEGIN
		SELECT @base_currency_id = currency_id,
			   @base_currency_code = code,
			   @base_amount = ROUND(@currency_amount_unrounded, CASE @decimal_places WHEN 0 THEN decimal_places ELSE @decimal_places END),
			   @base_amount_unrounded = @currency_amount_unrounded,
			   @account_currency_id = currency_id,
			   @account_amount = ROUND(@currency_amount_unrounded, CASE @decimal_places WHEN 0 THEN decimal_places ELSE @decimal_places END),
			   @account_amount_unrounded = @currency_amount_unrounded,
			   @system_currency_id = currency_id,
			   @system_amount = ROUND(@currency_amount_unrounded, CASE @decimal_places WHEN 0 THEN decimal_places ELSE @decimal_places END),
			   @system_amount_unrounded = @currency_amount_unrounded,
			   @currency_base_xrate = 1,
			   @currency_base_date = @Current_Date,
			   @account_base_xrate = 1,
			   @account_base_date = @Current_Date,
			   @system_base_xrate = 1,
			   @system_base_date = @Current_Date,
			   @base_decimals = CASE @decimal_places WHEN 0 THEN decimal_places ELSE @decimal_places END,
			   @system_decimals = CASE @decimal_places WHEN 0 THEN decimal_places ELSE @decimal_places END,
			   @Account_decimals = CASE @decimal_places WHEN 0 THEN decimal_places ELSE @decimal_places END,
			   @return_status = 1
		FROM   currency WITH(NOLOCK)
		WHERE  is_deleted = 0
			   AND currency_id = @currency_id

		RETURN
	END

	-- Determine defaults for SQL performance    
	IF @currency_base_date = 0
	BEGIN
		SET @currency_base_date = NULL
	END

	SET @currency_base_date = ISNULL(@currency_base_date, GETDATE())

	IF @currency_base_xrate = 0
	BEGIN
		SET @currency_base_xrate = NULL
	END

	IF @account_base_xrate = 0
	BEGIN
		SET @account_base_xrate = NULL
	END

	IF @system_base_xrate = 0
	BEGIN
		SET @system_base_xrate = NULL
	END

	EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT

	IF @TypeOfRates = 1
		SET @branch=1
	ELSE
		SET @branch=@company_id

	--Get base details    
	SELECT @base_currency_id = S.base_currency_id,
		   @base_currency_code = C.code,
		   @base_amount_unrounded = @currency_amount_unrounded * ISNULL(@currency_base_xrate, CR.rate_against_base),
		   @base_decimals = CASE @decimal_places WHEN 0 THEN C.decimal_places ELSE @decimal_places END,
		   @currency_base_xrate = ISNULL(@currency_base_xrate, CR.rate_against_base)
	FROM   CurrencyRate CR
	JOIN   SOURCE S
		ON S.source_id = CR.company_id
	JOIN   Currency C
		ON C.currency_id = S.base_currency_id
	WHERE  CR.currency_id = @currency_id
		   AND CR.company_id = @branch
		   AND CR.effective_from IN (SELECT MAX(effective_from)
									 FROM   CurrencyRate
									 WHERE  effective_from <= @currency_base_date
											AND currency_id = CR.currency_id
											AND company_id = CR.company_id)

	-- The Mode parameter is used when the caller doesn't need these    
	-- additional values (eg. Stats) and therefore saves time    
	IF @mode = 'ALL'
	BEGIN
		IF @account_base_date = 0
		BEGIN
			SET @account_base_date = NULL
		END

		IF @system_base_date = 0
		BEGIN
			SET @system_base_date = NULL
		END

		SET @account_base_date = ISNULL(@account_base_date, GETDATE())
		SET @system_base_date = ISNULL(@system_base_date, GETDATE())

		IF @account_currency_id IS NOT NULL
		BEGIN
			-- We need to triangulate the conversion back from base    
			SELECT @account_amount_unrounded = @base_amount_unrounded / ISNULL(@account_base_xrate, CR.rate_against_base),
				   @account_decimals = CASE @decimal_places WHEN 0 THEN C.decimal_places ELSE @decimal_places END,
				   @account_base_xrate = ISNULL(@account_base_xrate, CR.rate_against_base)
			FROM   CurrencyRate CR
			JOIN   Currency C
				ON C.currency_id = CR.currency_id
			WHERE  CR.currency_id = @account_currency_id
				   AND CR.company_id = @branch
				   AND CR.effective_from IN (SELECT MAX(effective_from)
											 FROM   CurrencyRate
											 WHERE  effective_from <= @account_base_date
													AND currency_id = CR.currency_id
													AND company_id = CR.company_id)

			IF @account_amount_unrounded IS NULL
			BEGIN
				SELECT @return_status = -3

				RETURN -- No account coversion rate for this currency    
			END

			-- Handle the rounding    
			SELECT @account_amount = ROUND(@account_amount_unrounded, @account_decimals)
		END

		-- We need to triangulate the conversion back from base    
		SELECT @system_amount_unrounded = @base_amount_unrounded / ISNULL(@system_base_xrate, CR.rate_against_base),
			   @system_decimals = CASE @decimal_places WHEN 0 THEN c.decimal_places ELSE @decimal_places END,
			   @system_base_xrate = ISNULL(@system_base_xrate, CR.rate_against_base)
		FROM   CurrencyRate CR
		JOIN   Currency C
			ON C.currency_id = CR.currency_id
		WHERE  CR.currency_id = @system_currency_id
			   AND CR.company_id = @branch
			   AND CR.effective_from IN (SELECT MAX(effective_from)
										 FROM   CurrencyRate
										 WHERE  effective_from <= @system_base_date
												AND currency_id = CR.currency_id
												AND company_id = CR.company_id)

		IF @system_amount_unrounded IS NULL
		BEGIN
			SELECT @return_status = -4

			RETURN -- No system rate for this currency    
		END

		-- Handle the rounding    
		SELECT @system_amount = ROUND(@system_amount_unrounded, @system_decimals)
	END

	-- Handle the rounding    
	SELECT @base_amount = ROUND(@base_amount_unrounded, @base_decimals)

	-- All went okay    
	SELECT @return_status = 1

-- For testing    
/*    
SELECT    
 @base_currency_id 'Base Currency ID',    
 @base_currency_code 'Base Currency Code',    
 @base_amount 'Base Currency Amount',    
 @base_amount_unrounded 'Base Currency Amount UR',    
 @account_currency_id 'Account Currency ID',    
 @account_amount 'Account Currency Amount',    
 @account_amount_unrounded 'Account Currency Amount UR',    
 @system_currency_id 'Cons Currency ID',    
 @system_amount 'Cons Currency Amount',    
 @system_amount_unrounded 'Cons Currency Amount UR',    
 @return_status 'Return'    
*/
GO 
