SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Add_TransDetailEx'
GO

CREATE PROCEDURE [spu_ACT_Add_TransDetailEx] (
	@document_id INT,
	@scale INT = 2 
)
AS
BEGIN

DECLARE @Portion TABLE (
	[portion_no] INT NOT NULL,
	[portion_start_date]  DATETIME NOT NULL,
	[portion_end_date]  DATETIME NULL,
	[portion_days] INT NULL,
	[effective_start_date]  DATETIME NOT NULL,
	[effective_end_date]  DATETIME NULL,
	[effective_days] INT NULL,
	[effective_ratio] DECIMAL(18, 9) NULL,
	[amount_unrounded] DECIMAL(18, 9) NULL,
	[amount] MONEY NULL -- the portion amount that is rounded using financial rounding rules.
)

	DECLARE
		@dtTpi_inception_date  DATETIME, -- the inception date of the policy, if applicable.
		@dtExpiry_date  DATETIME, -- the expiry date of the policy, if applicable.
		@nTpi_month_count INT, -- the number of calendar months that span this period of insurance, if applicable.
		@nPortion_month_count INT, -- the number of calendar months in one full portion, if applicable.
		@dtEffective_date  DATETIME, -- the date when the portion becomes due.
		@nFirst_month_no INT,  -- the month number relative to the inception date when the first portion is effective, if applicable.
		@nDays_in_year INT, -- the days in the year of insurance
		@nDays_on_cover INT; -- the days that we are on cover

	DECLARE
		@nTransdetail_id INT,
		@nTransdetailex_type_id INT,
		@nPeriod_id INT,
		@nAccount_id INT,
		@nCurrency_id smallint,
		@crAmount MONEY,
		@crCurrency_base_xrate NUMERIC(19, 10),
		@nAccount_currency_id smallint,
		@crAccount_amount MONEY,
		@crAccount_base_xrate NUMERIC(19, 10),
		@nSystem_currency_id smallint,
		@crSystem_amount MONEY,
		@crSystem_base_xrate NUMERIC(19, 10),
		@crAccount_Outstanding_amount MONEY,
		@crSystem_Outstanding_amount MONEY,
		@crCurrency_Outstanding_amount MONEY;

	SELECT
		@dtTpi_inception_date = [IF].[inception_date_tpi],
		@dtExpiry_date = [IF].[expiry_date],
		@nTpi_month_count = DATEDIFF(MONTH, [IF].[inception_date_tpi], [IF].[expiry_date]) + 1,
		@nPortion_month_count = 	CASE UPPER([CF].[code])
									WHEN 'ONCE OFF'
										THEN 12 

									WHEN 'BIANNUAL'
										THEN 6

									WHEN 'QUARTERLY'
										THEN 3

									WHEN 'MONTHLY'
										THEN 1
								END,
		@dtEffective_date = [IF].[cover_start_date],
		@nFirst_month_no = DATEDIFF(MONTH, [IF].[inception_date_tpi], [IF].[cover_start_date]) + 1,
		@nDays_on_cover =  DATEDIFF(DAY, [IF].[cover_start_date], [IF].[expiry_date]) + 1,
		@nDays_in_year =  dbo.GetDaysInYearProrata([IF].[inception_date_tpi], [IF].[expiry_date])
		
	FROM [Document] AS [D]
		INNER JOIN [Insurance_File] AS [IF]
			INNER JOIN [CollectionFrequency] AS [CF]
				ON [CF].[CollectionFrequency_id] = [IF].[CollectionFrequency_id]
			ON [IF].[insurance_file_cnt] = [D].[insurance_file_cnt]
	WHERE [D].[document_id] = @document_id;

	-- Return if no records found. Will happen when no collection frequency set
	If @dtTpi_inception_date IS NULL
		RETURN
		
	-- ONCE-OFF payments always creates a sinlge collection portion
	IF @nPortion_month_count = 12
		SELECT @nTpi_month_count = 1
		


	DECLARE TransDetail_Cursor CURSOR FAST_FORWARD FOR
	SELECT
		[TransDetail].[transdetail_id],
		[TransDetail].[period_id],
		[TransDetail].[account_id],
		[TransDetail].[currency_id],
		[TransDetail].[currency_amount],
		[TransDetail].[currency_base_xrate],
		[TransDetail].[account_currency_id],
		[TransDetail].[account_amount],
		[TransDetail].[account_base_xrate],
		[TransDetail].[system_currency_id],
		[TransDetail].[system_amount],
		[TransDetail].[system_base_xrate],
		[TransDetail].[outstanding_system_amount],
		[TransDetail].[outstanding_account_amount],
		[TransDetail].[outstanding_currency_amount]	
			
	FROM [TransDetail]
		INNER JOIN [Transdetail_Type]
			ON [Transdetail_Type].[transdetail_type_id] = [Transdetail].[transdetail_type_id]
		INNER JOIN [Account]
			ON [Account].[account_id] = [Transdetail].[account_id]
		INNER JOIN [Ledger]
			ON [Ledger].[ledger_id] = [Account].[ledger_id]
	WHERE [document_id] = @document_id
			AND [Ledger].ledger_short_name IN ('SA', 'AG', 'IN', 'CO')
				AND ISNULL([Transdetail_Type].[is_extended], 0) = 1;

	OPEN TransDetail_Cursor;

	FETCH NEXT FROM TransDetail_Cursor
	INTO
		@nTransdetail_id,
		@nPeriod_id,
		@nAccount_id,
		@nCurrency_id,
		@crAmount,
		@crCurrency_base_xrate,
		@nAccount_currency_id,
		@crAccount_amount,
		@crAccount_base_xrate,
		@nSystem_currency_id,
		@crSystem_amount,
		@crSystem_base_xrate,
    	@crAccount_Outstanding_amount ,
		@crSystem_Outstanding_amount,
		@crCurrency_Outstanding_amount ;


	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE
			@tpi_portion_count INT, -- the maximum number of portions that may exist for this period of insurance.
			@first_portion_no INT, -- the first portion number in this period of insurance for which an amount is due.
			@portion_count INT; -- the actual number of portions that exist for this document.

		DECLARE
			@annualised_amount MONEY,
			@full_portion_amount MONEY;

		SELECT @annualised_amount = @crAmount  * @nDays_in_year / @nDays_on_cover,
			   @full_portion_amount = ROUND(@annualised_amount / (12/@nPortion_month_count) , @scale)

		SET @tpi_portion_count =
			ISNULL(CEILING(
					CONVERT(DECIMAL(18, 9), @nTpi_month_count) /
						CONVERT(DECIMAL(18, 9), @nPortion_month_count)), 1);

		SET @first_portion_no =
			ISNULL(CEILING(
					CONVERT(DECIMAL(18, 9), @nFirst_month_no) /
						CONVERT(DECIMAL(18, 9), @nPortion_month_count)), 1);

		SET @portion_count = @tpi_portion_count;

		WITH [Portion_CTE] AS (
			SELECT
				@first_portion_no AS 'portion_no'

			UNION ALL

			SELECT
				[portion_no] + 1
			FROM [Portion_CTE]
			WHERE [portion_no] + 1 <= @portion_count
		),
		[PortionDetail_CTE] AS (
			SELECT
				[portion_no],
				CASE
					WHEN @nPortion_month_count IS NULL
						THEN @dtEffective_date
					WHEN [portion_no] = @first_portion_no AND @first_portion_no = 1
						THEN DATEADD(DAY, -1 * DAY(@dtTpi_inception_date) + 1, @dtTpi_inception_date)

					ELSE
						DATEADD(
							MONTH,
							([portion_no] - @first_portion_no) * @nPortion_month_count - 1,
							DATEADD(
								MONTH,
								1,
								DATEADD(
									DAY,
									-1 * DAY(DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)) + 1,
									DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)
								)
							)/*first day of the month following the first portion*/
						)
				END AS 'portion_start_date',
				CASE
					WHEN @nPortion_month_count IS NULL
						THEN @dtExpiry_date
					ELSE
						DATEADD(
							DAY,
							-1,
							DATEADD(
								MONTH,
								([portion_no] - @first_portion_no + 1) * @nPortion_month_count - 1,
								DATEADD(
									MONTH,
									1,
									DATEADD(
										DAY,
										-1 * DAY(DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)) + 1,
										DATEADD (MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)
									)
								)/*first day of the month following the first portion*/
							)
						)
				END AS 'portion_end_date',
				CASE
					WHEN [portion_no] = @first_portion_no
						THEN @dtEffective_date

					ELSE
						DATEADD(
							MONTH,
							([portion_no] - @first_portion_no) * @nPortion_month_count - 1,
							DATEADD(
								MONTH,
								1,
								DATEADD(
									DAY,
									-1 * DAY(DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)) + 1,
									DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)
								)
							)/*first day of the month following the first portion*/
						)
				END AS 'effective_start_date',
				CASE
					WHEN [portion_no] = @portion_count
						THEN @dtExpiry_date

					ELSE
						DATEADD(
							DAY,
							-1,
							DATEADD(
								MONTH,
								([portion_no] - @first_portion_no + 1) * @nPortion_month_count - 1,
								DATEADD(
									MONTH,
									1,
									DATEADD(
										DAY,
										-1 * DAY(DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)) + 1,
										DATEADD(MONTH, (@first_portion_no - 1) * @nPortion_month_count, @dtTpi_inception_date)
									)
								)/*first day of the month following the first portion*/
							)
						)
				END AS 'effective_end_date'
			FROM [Portion_CTE]
		)

		INSERT @Portion (
			[portion_no],
			[portion_start_date],
			[portion_end_date],
			[portion_days],
			[effective_start_date],
			[effective_end_date],
			[effective_days],
			[effective_ratio],
			[amount]
		)
		SELECT
			[portion_no],
			[portion_start_date],
			[portion_end_date],
			NULL,
			[effective_start_date],
			[effective_end_date],
			NULL,
			NULL,
			NULL
		FROM [PortionDetail_CTE];

		UPDATE @Portion
		SET
			[portion_days] = DATEDIFF(DAY, [portion_start_date], [portion_end_date]) + 1,
			[effective_days] = DATEDIFF(DAY, [effective_start_date], [effective_end_date]) + 1 --DAY(DATEADD(mm,DATEDIFF(mm,-1,effective_start_date),-1)) ;--DATEDIFF(DAY, [effective_start_date], [effective_end_date]) + 1;

		UPDATE @Portion
		SET
			[effective_ratio] = CONVERT(DECIMAL(18, 9), [effective_days]) / CONVERT(DECIMAL(18, 9), [portion_days]);

		UPDATE @Portion
		SET
			[amount] = ROUND(@full_portion_amount * [effective_ratio], @scale,1); --TRUNCATE

		DECLARE
			@total_amount MONEY

		SELECT @total_amount = SUM([amount]) FROM @Portion

		--Update first portion with any rounding issues
		UPDATE @Portion
			SET
				[amount] = [amount] + (@crAmount - @total_amount)
		WHERE [portion_no] = @first_portion_no;

		INSERT [TransDetailEx] (
			[document_id],
			[transdetail_id],
			[portion_no],
			[effective_date],
			[period_id],
			[account_id],
			[currency_id],
			[currency_amount],
			[currency_base_xrate],
			[outstanding_currency_amount],
			[account_currency_id],
			[account_amount],
			[account_base_xrate],
			[outstanding_account_amount],
			[system_currency_id],
			[system_amount],
			[system_base_xrate],
			[outstanding_system_amount]
		)
		SELECT
			@document_id,
			@nTransdetail_id,
			[portion_no],
			[effective_start_date],
			@nPeriod_id,
			@nAccount_id,
			@nCurrency_id,
			[amount],
			@crCurrency_base_xrate,
			[amount],
			@nAccount_currency_id,
			Round([amount] * @crCurrency_base_xrate / @crAccount_base_xrate, @Scale),
			@crAccount_base_xrate,
			Round([amount] * @crCurrency_base_xrate / @crAccount_base_xrate, @Scale),
			@nSystem_currency_id,
			Round([amount] * @crCurrency_base_xrate / @crSystem_base_xrate, @Scale),
			@crSystem_base_xrate,
			Round([amount] * @crCurrency_base_xrate / @crSystem_base_xrate, @Scale)
		FROM @Portion

		
		DECLARE
			@total_Calculated_amount MONEY

		SELECT @total_Calculated_amount = ISNULL(SUM(outstanding_system_amount),0) FROM [TransDetailEx]  where transdetail_id=@nTransdetail_id

		UPDATE [TransDetailEx]
			SET
				outstanding_system_amount = outstanding_system_amount + (@crSystem_Outstanding_amount - @total_Calculated_amount)
		WHERE  transdetail_id=@nTransdetail_id and [portion_no] = @first_portion_no;

		SELECT @total_Calculated_amount = ISNULL(SUM(outstanding_account_amount),0) FROM [TransDetailEx]  where transdetail_id=@nTransdetail_id

		UPDATE [TransDetailEx]
			SET
				outstanding_account_amount = outstanding_account_amount + (@crAccount_Outstanding_amount - @total_Calculated_amount)
		WHERE transdetail_id=@nTransdetail_id and [portion_no] = @first_portion_no;

		SELECT @total_Calculated_amount = ISNULL(SUM(outstanding_Currency_amount),0) FROM [TransDetailEx]  where transdetail_id=@nTransdetail_id

		UPDATE [TransDetailEx]
			SET
				outstanding_Currency_amount = outstanding_Currency_amount + (@crCurrency_Outstanding_amount - @total_Calculated_amount)
		WHERE transdetail_id=@nTransdetail_id and [portion_no] = @first_portion_no;

       
	   SELECT @total_Calculated_amount = ISNULL(SUM(account_amount),0) FROM [TransDetailEx]  where transdetail_id=@nTransdetail_id

		UPDATE [TransDetailEx]
			SET
				account_amount = account_amount + (@crAccount_amount - @total_Calculated_amount)
		WHERE transdetail_id=@nTransdetail_id and [portion_no] = @first_portion_no;
		
		SELECT @total_Calculated_amount = ISNULL(SUM(system_amount),0) FROM [TransDetailEx]  where transdetail_id=@nTransdetail_id

		UPDATE [TransDetailEx]
			SET
				system_amount = system_amount + (@crSystem_amount - @total_Calculated_amount)
		WHERE transdetail_id=@nTransdetail_id and [portion_no] = @first_portion_no;
				

		DELETE FROM @Portion

		FETCH NEXT FROM TransDetail_Cursor
		INTO
			@nTransdetail_id,
			@nPeriod_id,
			@nAccount_id,
			@nCurrency_id,
			@crAmount,
			@crCurrency_base_xrate,
			@nAccount_currency_id,
			@crAccount_amount,
			@crAccount_base_xrate,
			@nSystem_currency_id,
			@crSystem_amount,
			@crSystem_base_xrate,
			@crAccount_Outstanding_amount ,
			@crSystem_Outstanding_amount,
			@crCurrency_Outstanding_amount ;

	END;

	CLOSE TransDetail_Cursor;
	DEALLOCATE TransDetail_Cursor;

END;

GO
