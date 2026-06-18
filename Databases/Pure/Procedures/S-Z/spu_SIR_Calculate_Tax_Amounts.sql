SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Calculate_Tax_Amounts'
GO
CREATE PROCEDURE spu_SIR_Calculate_Tax_Amounts
    @company_id INT,
    @tax_group_id INT,
    @transtype VARCHAR(20),
    @currency_id INT,
    @amount MONEY,
    @tax_currency_amount MONEY OUTPUT,
    @tax_base_amount MONEY OUTPUT,
    @associated_key_id INT = NULL,
    @insurance_file_cnt INT,
    @risk_cnt INT,
    @calculate_only TINYINT = 0,
    @associated_key_id2 INT = NULL,
	
	@is_tax_amended TINYINT =0,
    @amended_tax_value MONEY=0,
    @premium MONEY=0
	
AS
    DECLARE @tax_rate_is_value INT
    DECLARE @tax_rate MONEY
    DECLARE @tax_currency_id INT
    DECLARE @effective_date DATETIME
    DECLARE @individual_tax_amount MONEY
    DECLARE @tax_type_id INT
    DECLARE @tax_band_id INT
    DECLARE @tax_calculation_cnt INT
    DECLARE @tax_sequence INT
    DECLARE @tax_rate_allow_tax_credit INT
    DECLARE @tax_rate_country_id INT
    DECLARE @tax_rate_state_id INT
    DECLARE @tax_rate_class_of_business_id INT
    DECLARE @calc_basis INT
    DECLARE @tax_band_rate_id INT
    DECLARE @is_suspended TINYINT
    DECLARE @system_option VARCHAR(20)
    DECLARE @is_include_tax_in_instalments TINYINT

	--Get Suppress Decimal flag to round whole number
    DECLARE @SuppressDecimalOption AS INT=112
	DECLARE @bIsSuppressDecimal As TINYINT=(Select ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption)
	
	--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.1)
	DECLARE @Remaining_Tax_Value MONEY

	SELECT  @is_tax_amended=ISNULL(@is_tax_amended,0),
			@amended_tax_value=ISNULL(@amended_tax_value,0)

	DECLARE @Is_Manually_Changed TINYINT
	IF @transtype='TTAC' AND @is_tax_amended=1
	BEGIN
		--As per new requirement given by Amit, system option "Override Agent Tax Group Allowed" needs to be checked to allow tax editing
		DECLARE @OverrideAgentTaxGroupAllowedOption AS INT
		SET @OverrideAgentTaxGroupAllowedOption=5081

		IF EXISTS ( SELECT 1 FROM System_Options
					WHERE
						Branch_ID=@company_id
						AND Option_Number=@OverrideAgentTaxGroupAllowedOption
						AND [Value]=1)
		BEGIN
			SET @Remaining_Tax_Value=@amended_tax_value
		END
		ELSE
		BEGIN
			--The system option is not enabled but is tax amended value has been passed as 1
			--In this case cancel the tax amendment
			SET @is_tax_amended=0
		END
	END
	--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.1)

    SELECT  @tax_currency_amount = 0,
            @tax_base_amount = 0

    -- cannot calculate tax when there is no tax group so just exit...
    IF ISNULL(@tax_group_id, 0) = 0
        RETURN

	SELECT 	@system_option='0'

	SELECT 	@system_option=value
	FROM	System_Options
	WHERE	option_number=5019 AND branch_id=1

	IF @system_option='1' BEGIN
	    -- Get details from insurance file
	    SELECT @effective_date = ifi.cover_start_date
	    FROM insurance_file ifi
	    WHERE ifi.insurance_file_cnt = @Insurance_File_Cnt
	END
	ELSE IF @system_option='0' BEGIN
	    -- Get system_base_date from insurance file
	    SELECT @effective_date = ifi.system_base_date
	    FROM insurance_file ifi
	    WHERE ifi.insurance_file_cnt = @Insurance_File_Cnt
	END
	ELSE IF @system_option='3' BEGIN
	    -- Get details from insurance file
	    SELECT @effective_date = ifi.inception_date_tpi
	    FROM insurance_file ifi
	    WHERE ifi.insurance_file_cnt = @Insurance_File_Cnt
	END
	ELSE BEGIN --System Option will be 2 as RIsk Inception Date
	    -- Get system_base_date from insurance file
	    SELECT @effective_date = inception_date
	    FROM risk
	    WHERE risk_cnt = @risk_cnt
	END

    --- create temporary table to hold rates
    CREATE TABLE #Rates (
        tax_type_id INT,
        description VARCHAR(255),
        tax_band_id INT,
        description1 VARCHAR(255),
        is_value INT,
        rate MONEY,
        currency_id INT,
        code VARCHAR(10),
        sequence INT,
        allow_tax_credit TINYINT,
        country_id SMALLINT ,
        state_id SMALLINT,
        class_of_business_id INT,
        calc_basis INT,
        tax_band_rate_id INT,
        is_suspended TINYINT,
        is_include_tax_in_instalments TINYINT)

    -- save rates into temporary table
    INSERT INTO #Rates
        EXEC spu_Get_Tax_Types_and_Bands
            @tax_group_id =  @tax_group_id,
            @effective_date = @effective_date,
            @transtype = @transtype,
			@insurance_file_cnt=@insurance_file_cnt

    DECLARE CURSOR_Tax_Rates CURSOR FAST_FORWARD FOR SELECT  tax_type_id,
        tax_band_id,
        is_value,
        rate,
        currency_id,
        sequence,
        allow_tax_credit,
        country_id,
        state_id,
        class_of_business_id, calc_basis,
        tax_band_rate_id,
        is_suspended,
        is_include_tax_in_instalments
    FROM #Rates

    OPEN CURSOR_Tax_Rates
    FETCH NEXT FROM CURSOR_Tax_Rates INTO
        @tax_type_id,
        @tax_band_id,
        @tax_rate_is_value,
        @tax_rate,
        @tax_currency_id,
        @tax_sequence,
        @tax_rate_allow_tax_credit,
        @tax_rate_country_id,
        @tax_rate_state_id,
        @tax_rate_class_of_business_id, @calc_basis,
        @tax_band_rate_id,
        @is_suspended,
        @is_include_tax_in_instalments

    WHILE @@FETCH_STATUS = 0
    BEGIN
        --Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.1)
        IF @transtype='TTAC' AND @is_tax_amended=1
  BEGIN
   SELECT  @Is_Manually_Changed =1,
     @tax_rate=@Remaining_Tax_Value,
     @tax_rate_is_value=1,
     @Remaining_Tax_Value=@Remaining_Tax_Value-@tax_rate,
     @tax_currency_id = @currency_id --Sankar - PN 68535
  END
  ELSE
  BEGIN
   SET @Is_Manually_Changed=0
  END
        --End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.1)

        -- if this is a value rather than a percentage
        IF @tax_rate_is_value = 1
        BEGIN
            -- the routine needs to convert the tax value
            -- into the same currency as the fee
            -- before it is added to the total tax amount
            EXEC spu_ACT_Do_Currency_To_Currency_Conversion
                @currency_id_from =  @tax_currency_id,
                @currency_amount_from = @tax_rate,
                @company_id =  @company_id,
                @currency_id_to=  @currency_id,
                @currency_amount_to = @individual_tax_amount OUTPUT
        END
        ELSE
        BEGIN
            -- the tax amount is simply a percentage of the passed in amount
    IF @calc_basis = 4
        SET @individual_tax_amount = @premium * (@tax_rate/100)
    ELSE
        SET @individual_tax_amount = @amount * (@tax_rate/100)

        END

		--If suppress decimal is ON then round up to zero decimals
		IF @bIsSuppressDecimal=1
		BEGIN
	      	SET @individual_tax_amount=ROUND(@individual_tax_amount,0)
		END
		ELSE
		BEGIN
			SET @individual_tax_amount=ROUND(@individual_tax_amount,4)
		END


        SET @tax_currency_amount = @tax_currency_amount + @individual_tax_amount

        -- UI can request to just get a tax calculation without
        -- writing back to the database
        IF (@calculate_only=0) BEGIN
                -- insert an entry into tax_calculation for each item...
                -- @associatedkeyid

                DECLARE @bSpreadTaxAcrossInstalments TINYINT = 0

                SELECT @bSpreadTaxAcrossInstalments = ISNULL(is_spread_tax_across_instalments,0)
                FROM Tax_Type
                WHERE tax_type_id = @tax_type_id

                INSERT INTO tax_calculation (
                    insurance_file_cnt,
                    risk_cnt,
                    tax_band_id,
                    premium,
                    percentage,
                    value,
                    is_value,
                    is_manually_changed,
                    Calc_Basis,
                    Basis_Value,
                    Sum_Insured,
                    Sum_Insured_Rounded,
                    currency_id,
                    allow_tax_credit,
                    original_sum_insured,
                    country_id,
                    state_id,
                    class_of_business_id,
                    tax_group_id,
                    sequence,
                    transtype,
                    policy_fee_u_id,
                    agent_commission_cnt,
                    ri_party_cnt,
                    ri_arrangement_line_id,
                    pfprem_finance_cnt,
                    pfprem_finance_version,
                    tax_band_rate_id,
                    is_suspended,
                    include_tax_in_instalments,
                    spread_tax_across_instalments
                )
                VALUES (
                    @insurance_file_cnt,
                    @risk_cnt,
                    @tax_band_id,
                    @amount,
                    CASE  WHEN @tax_rate_is_value = 0 THEN @tax_rate
                    ELSE 0
                    END,
                    @individual_tax_amount,
                    @tax_rate_is_value,
                    --Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.1)
                    @Is_Manually_Changed,
                    --End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.1)
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    @currency_id,
                    @tax_rate_allow_tax_credit,
                    NULL,
                    @tax_rate_country_id,
                    @tax_rate_state_id,
                    @tax_rate_class_of_business_id,
                    @tax_group_id,
                    @tax_sequence,
                    @transtype,
                    CASE WHEN @transtype = 'TTF' and ISNULL(@associated_key_id, 0) <> 0 THEN
                        @associated_key_id ELSE NULL END,
                    CASE WHEN @transtype = 'TTAC' and ISNULL(@associated_key_id, 0) <> 0 THEN
                        @associated_key_id ELSE NULL END,
                    CASE WHEN @transtype In ('TTRITP', 'TTRITC', 'TTRIFP', 'TTRIFC') and ISNULL(@associated_key_id, 0) <> 0 THEN
                        @associated_key_id ELSE NULL END,
                    CASE WHEN @transtype In ('TTRITP', 'TTRITC', 'TTRIFP', 'TTRIFC') and ISNULL(@associated_key_id2, 0) <> 0 THEN
                        @associated_key_id2 ELSE NULL END,
                    CASE WHEN @transtype = 'TTI' and ISNULL(@associated_key_id, 0) <> 0 THEN
                        @associated_key_id ELSE NULL END,
                    CASE WHEN @transtype = 'TTI' and ISNULL(@associated_key_id2, 0) <> 0 THEN
                        @associated_key_id2 ELSE NULL END,
                    @tax_band_rate_id,
                    @is_suspended,
                    @is_include_tax_in_instalments,
                    @bSpreadTaxAcrossInstalments
                )
        END

        FETCH NEXT FROM CURSOR_Tax_Rates INTO
            @tax_type_id,
            @tax_band_id,
            @tax_rate_is_value,
            @tax_rate,
            @tax_currency_id,
            @tax_sequence,
            @tax_rate_allow_tax_credit,
            @tax_rate_country_id,
            @tax_rate_state_id,
            @tax_rate_class_of_business_id, @calc_basis,
            @tax_band_rate_id,
            @is_suspended,
            @is_include_tax_in_instalments
    END

    CLOSE CURSOR_Tax_Rates
    DEALLOCATE CURSOR_Tax_Rates

    DROP TABLE #Rates

    EXEC spu_ACT_Do_Currency_Conversion
        @company_id = @company_id,
        @currency_id = @currency_id,
        @currency_amount_unrounded = @tax_currency_amount,
        @currency_base_date = @effective_date,
        @mode ='1',
        @base_amount_unrounded = @tax_base_amount OUTPUT,
        @return_status = 1
