SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Add_Stats_Details_Deposit'
GO


CREATE PROCEDURE spu_Add_Stats_Details_Deposit
     @stats_folder_cnt int
AS BEGIN


DECLARE @insurance_file_cnt int


    DECLARE @Deposit numeric(19,4),
            @DepositHome numeric(19,4),
            @DepositSystem numeric(19,4),
			@currency_code char (10),
            @currency_id int,
            @currency_rate numeric(19, 4),
            @system_rate numeric(19, 4),
			@stats_detail_id int,
            @stats_detail_type char(3),
			@company_id INT,
			@return_status INT


    ---Get the policy from the stats folder
    SELECT  @insurance_file_cnt = insurance_file_cnt
    FROM	stats_folder
    WHERE   stats_folder.stats_folder_cnt = @stats_folder_cnt


    SELECT @Deposit = ISNULL(Deposit,0) 
    FROM pfPremiumFinance 
    WHERE Insurance_File_Cnt = @insurance_file_cnt


    IF @Deposit > 0
    BEGIN
            
        /*Get details from insurance file*/
		SELECT
			@company_id = source_id,
			@currency_id = currency_id,
			@currency_rate = currency_base_xrate,
			@system_rate = system_base_xrate
		FROM insurance_file
		WHERE insurance_file_cnt = @insurance_file_cnt

		/*Get details about the currency*/
		SELECT
			@currency_code = code
		FROM currency
		WHERE currency_id = @currency_id

		/*Get deposit in base currency*/
		EXEC spu_ACT_Do_Currency_Conversion
					@company_id = @company_id,
					@currency_id = @currency_id,
					@currency_amount_unrounded = @Deposit,
					@mode = 'BASE',
					@base_amount = @DepositHome OUTPUT,
					@base_amount = @DepositSystem OUTPUT,
					@currency_base_xrate = @currency_rate OUTPUT,
					@system_base_xrate = @system_rate OUTPUT,
					@return_status = @return_status OUTPUT		

		--Insert into this premium total NOT tax value so that it gets added to the total for the client account
		SELECT  @stats_detail_id = MAX(stats_detail_id) + 1
		FROM    Stats_Detail
		WHERE   stats_folder_cnt = @stats_folder_cnt


		IF  @stats_detail_id is  NULL
			SELECT  @stats_detail_id = 1


		SET @stats_detail_type = 'JN'


		INSERT INTO Stats_Detail
		(
			stats_folder_cnt,
			stats_detail_id,
			stats_detail_type,
			this_premium_original,
			this_premium_home,
			this_premium_system,
			currency_code,
			currency_rate
		)
		VALUES 
		(
			@stats_folder_cnt,
			@stats_detail_id,
			@stats_detail_type,
			@Deposit * -1,
			@DepositHome * -1,
			@DepositSystem * -1,
			@currency_code,
			@currency_rate
		)


		SELECT  @stats_detail_id = MAX(stats_detail_id) + 1
		FROM    Stats_Detail
		WHERE   stats_folder_cnt = @stats_folder_cnt


		INSERT INTO Stats_Detail 
		(
			stats_folder_cnt,
			stats_detail_id,
			stats_detail_type,
			this_premium_original,
			this_premium_home,
			this_premium_system,
			currency_code,
			currency_rate
		)
		VALUES
		(
			@stats_folder_cnt,
			@stats_detail_id,
			@stats_detail_type,
			@Deposit,
			@DepositHome,
			@DepositSystem,
			@currency_code,
			@currency_rate
		)


    END


    UPDATE stats_folder
    SET Premium_total = @Deposit
    WHERE stats_folder_cnt = @stats_folder_cnt


END


GO


