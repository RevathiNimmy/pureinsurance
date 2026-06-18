SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_extension_pro_rata_rate'
Go

CREATE PROCEDURE spu_get_extension_pro_rata_rate 
	@insurance_file_cnt INT,
	@risk_cnt           INT,
	@original_risk_cnt  INT,
	@pro_rata_rate      FLOAT OUTPUT
AS
	DECLARE @pro_rata_flag       TINYINT,
			@is_midnight_renewal TINYINT,
			@cover_start_date    DATETIME,
			@inception_date_tpi  DATETIME,
			@expiry_date         DATETIME,
			@policy_scaling      FLOAT,
			@days                INT,
			@year                INT,
			@annual_premium      NUMERIC(19, 4),
			@premium             NUMERIC(19, 4),
		        @bIs_true_monthly_policy tinyint,
		        @nTotalDaysInMonth int

	SELECT @pro_rata_flag = p.mta_prorata,
		   @is_midnight_renewal = p.is_midnight_renewal,
                   @bIs_true_monthly_policy = p.is_true_monthly_policy 
	FROM   product p
	JOIN   insurance_file ifi
		ON p.product_id = ifi.product_id
	WHERE  insurance_file_cnt = @insurance_file_cnt

	IF @pro_rata_flag = 0
		SELECT @pro_rata_rate = 1.00
	ELSE
	BEGIN
		SELECT @cover_start_date = ifi.cover_start_date,
			   @expiry_date = ifi.expiry_date,
			   @inception_date_tpi = inception_date_tpi
		FROM   insurance_file ifi
		WHERE  ifi.insurance_file_cnt = @insurance_file_cnt

		SELECT @days = Datediff(DAY, @cover_start_date, @expiry_date)

		IF Isnull(@is_midnight_renewal, 0) = 1
			SELECT @days = @days + 1

		IF @days = 0
			SELECT @days = 1

		SELECT @annual_premium = SUM(Abs(annual_premium))
		FROM   rating_section
		WHERE  Risk_cnt = @risk_cnt
			   AND original_flag = 1

		SELECT @premium = SUM(Abs(this_premium))
		FROM   rating_section
		WHERE  Risk_cnt = @risk_cnt
			   AND original_flag = 1

		SELECT @policy_scaling = CASE
								   WHEN ( Isnull(@annual_premium, 0) = 0 )
										 OR ( Isnull(@premium, 0) = 0 ) THEN CONVERT(FLOAT, 1)
								   ELSE CONVERT(FLOAT, @premium) / CONVERT(FLOAT, @annual_premium)
								 END

		SELECT @year = Datediff(DAY, @inception_date_tpi, Dateadd(YEAR, 1, @inception_date_tpi))
            IF ISNULL(@bIs_true_monthly_policy,0) = 1 
            BEGIN
            SET @nTotalDaysInMonth = DateDiff(day, @cover_start_date, DateAdd(MONTH , 1, @cover_start_date))
            SELECT  @pro_rata_rate = 1/((CONVERT(float, @days) / CONVERT(float, @nTotalDaysInMonth)) / @policy_scaling)
            END
            ELSE 
		SELECT @pro_rata_rate = 1 / ( ( CONVERT(FLOAT, @days) / CONVERT(FLOAT, @year) ) / @policy_scaling )
	END 
