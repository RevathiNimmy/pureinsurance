EXECUTE DDLDropProcedure 'spu_add_stats_details_gross' 
GO
CREATE PROCEDURE spu_add_stats_details_gross 
	@stats_folder_cnt INT

AS
	DECLARE
	@is_coinsured_policy SMALLINT,
	@insurance_file_cnt  INT,
	@source_id           INT,
	@company_id          INT,
	@out_currency_code   CHAR(10),
	@out_currency_id     INT,
	@out_currency_rate   NUMERIC(19, 8),
	@out_system_rate     NUMERIC(19, 8),
	@return_status       INT,
	@base_decimals		 INT,
	@system_decimals	 INT,
	@out_stats_detail_id INT,
	@business_type_code  VARCHAR(10),
	@lBusiness_type_id int,
	@sCoins_placement  varchar(10),
	@nBusinessTypeIdCoinsLead int = 3,
    	@nBusinessTypeIdCoinsFollow int = 4,
	@nTotal_out_coins_premium numeric(19, 4) = 0,
	@peril_is_levy_tax tinyint,
	@commission_value_system NUMERIC(19, 4),
	@commission_value_home NUMERIC(19, 4),
	@Sum_peril_lead_commission_value_home NUMERIC(19, 8),
	@Sum_peril_lead_commission_value_system NUMERIC(19, 8),
	@RiskTypeId INT,
	@class_of_business_id INT, 
	@peril_type_id INT,
	@commission_band_id INT
    declare @document_ref	varchar(25)

	 SELECT
		@insurance_file_cnt = IFL.insurance_file_cnt,
		@company_id = IFL.source_id,
		@out_currency_id = IFL.currency_id,
		@out_currency_rate = IFL.currency_base_xrate,
		@out_system_rate = IFL.system_base_xrate,
		@out_currency_code = C.code,
		@sCoins_placement = IFL.coins_placement,
		@lBusiness_type_id = IFL.business_type_id,
		@out_currency_code = C.code,
		@business_type_code = BT.code
		,@document_ref = sf.document_ref
    FROM Stats_Folder SF
    JOIN Insurance_File IFL ON SF.insurance_file_cnt = IFL.insurance_file_cnt
    JOIN Currency C			ON IFL.currency_id		 = C.currency_id
    JOIN Business_Type BT   ON IFL.business_type_id = BT.business_type_id
    JOIN Product P			ON IFl.product_id = P.product_id
    WHERE SF.stats_folder_cnt = @stats_folder_cnt

	   DECLARE @display_band_level BIT = 0
    SELECT @display_band_level = ISNULL(
        (SELECT TOP 1 CAST(so.value AS BIT) 
         FROM system_options so
         INNER JOIN insurance_file ifile ON ifile.insurance_file_cnt = @insurance_file_cnt
         INNER JOIN source s ON s.source_id = ifile.source_id
         WHERE so.branch_id = s.source_id 
         AND so.option_number = 5264
         AND so.value = '1'), 0)
                  

	-- RWH Check to see if this policy employs coinsurance.
	SELECT @is_coinsured_policy = COUNT(*)
	FROM   insurance_file ifi
	WHERE  ifi.insurance_file_cnt = @insurance_file_cnt
		   AND ifi.business_type_id IN (SELECT business_type_id
										FROM   business_type
										WHERE  code like 'COIN LEAD%' OR code like 'COIN FOLL%')

    if substring(LTRIM(RTRIM(@document_ref)),0,4) ='SDD'
	begin
	set @out_currency_rate = 0
	end 

	EXEC spu_ACT_Do_Currency_Conversion
            @company_id = @company_id,
            @currency_id = @out_currency_id,
            @currency_amount_unrounded = 100.00,
            @mode = 'ALL',
            @currency_base_xrate = @out_currency_rate OUTPUT,
            @system_base_xrate = @out_system_rate OUTPUT,
            @base_decimals = @base_decimals OUTPUT,
            @system_decimals = @system_decimals OUTPUT,
            @return_status = @return_status OUTPUT
			--Get Suppress Decimal flag to round whole number

	DECLARE @SuppressDecimalOption AS INT=112

	DECLARE @bIsSuppressDecimal As TINYINT=(Select ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption)
	IF @bIsSuppressDecimal=1 
	  SELECT  @base_decimals=0

   IF @out_system_rate = 0 SELECT @out_system_rate=1 ELSE SELECT @out_system_rate=1/@out_system_rate

	-- If this is a coinsured policy then retrieve the party_cnt for Retained.
	IF @is_coinsured_policy > 0
	BEGIN
		-- Retrieve non-retained coinsurance info.
				SELECT cv.party_cnt,
						   p.party_type_id,
						   p.shortname,
						   cv.share_percent,
						   cv.commission_percent,
						   cv.coi_value_id,
						   pin.is_retained

					INTO #Coinsurance

					FROM   Coi_Value cv
					JOIN   Party p ON p.party_cnt = cv.party_cnt
					JOIN   Party_insurer pin ON pin.party_cnt = p.party_cnt

					WHERE  cv.insurance_file_cnt = @insurance_file_cnt

        DECLARE @NO_OF_COIS INT =0

             SELECT @NO_OF_COIS = COUNT(*) FROM Coi_Value WHERE insurance_file_cnt = @insurance_file_cnt

			 IF @NO_OF_COIS=1
			 BEGIN
			      Update #Coinsurance set     share_percent=  0 
              END		
			
			  IF @NO_OF_COIS > 1	   
				BEGIN

					DELETE from #Coinsurance where is_retained = 1

					If (@lBusiness_type_id= 4 and (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS'))

					BEGIN
                		declare @ins_party_cnt integer
						SELECT TOP 1 @ins_party_cnt =  party_cnt from #Coinsurance co order by coi_value_id asc , share_percent desc
						UPDATE  #Coinsurance set share_percent= (select sum(c.share_percent)  FROM  #Coinsurance c)  where party_cnt =  @ins_party_cnt
						DELETE from #Coinsurance where party_cnt <> @ins_party_cnt
					END

				END
	END

	CREATE Table #Perils_Gross (

			ID INT IDENTITY Primary Key,
			Risk_id INT,
			risk_type_id INT,
			Risk_type_code VARCHAR(10),
			peril_id INT,
			Peril_description VARCHAR(255),
			peril_type_id INT,
			Peril_type_code VARCHAR(10),
			peril_policy_section_type_id INT,
			peril_policy_section_Code VARCHAR(10),
			peril_class_of_business_id			INT,
			peril_class_of_business_code		VARCHAR(10),
			peril_annual_premium				numeric(19, 4),
			peril_this_premium_original			numeric(19, 4),
			peril_commission_percent			numeric(12, 8),
			peril_lead_commission_value			numeric(19, 4),
			peril_sub_commission_value			numeric(19, 4),
			peril_sum_insured					numeric(19, 4),
			peril_rating_section_id				INT,
			peril_is_levy_tax					TINYINT,
			peril_original_flag					TINYINT,
			Cover_to_date						datetime ,
			is_value							bit,
			is_stamp_duty_insurer				bit,
			is_stamp_duty_insured				bit,
			peril_this_premium_home				numeric(19, 4),
			peril_this_premium_system			numeric(19, 4),
			peril_lead_commission_value_home    numeric(19, 4),
			peril_lead_commission_value_system  numeric(19, 4),
			peril_sub_commission_value_home     numeric(19, 4),
			peril_sub_commission_value_system   numeric(19, 4),
			peril_sum_insured_home				numeric(19, 4),
			peril_sum_insured_system			numeric(19, 4),
			peril_original_sum_insured			numeric(19, 4),
			peril_sum_insured_change			numeric(19, 4),
			peril_sum_insured_change_home		numeric(19, 4),
			peril_commission_band				int
		)

		IF(@display_band_level=1)
		BEGIN
			INSERT INTO #Perils_Gross
	(
			Risk_id ,
			risk_type_id,
			Risk_type_code,
			peril_id,
			Peril_description,
			peril_type_id ,
			Peril_type_code ,
			peril_policy_section_type_id,
			peril_policy_section_Code,
			peril_class_of_business_id ,
			peril_class_of_business_code,
			peril_annual_premium,
			peril_this_premium_original,
			peril_commission_percent,
			peril_lead_commission_value,
			peril_sub_commission_value,
			peril_sum_insured,
			peril_rating_section_id,
			peril_is_levy_tax,
			peril_original_flag,
			Cover_to_date,
			is_value ,
			is_stamp_duty_insurer,
			is_stamp_duty_insured,
			peril_commission_band
	)


	SELECT  DISTINCT  P.risk_cnt,

			  R.risk_type_id,
			  RT.code,
			  P.peril_id,
			  P.DESCRIPTION,
			  P.peril_type_id,
			  PT.code,
			  RS.policy_section_type_id,
			  PS.code,
			  P.class_of_business_id,
			  CB.code,
			  P.annual_premium,
			  P.this_premium,			--	peril_this_premium_original
			  CASE
				WHEN ISNULL(AC.premium, 0) = 0 THEN CASE
													  WHEN AC.commission_percentage = 0 THEN AC.commission_value
													  ELSE AC.commission_percentage
													END
				ELSE CASE
					   WHEN P.this_premium = 0 THEN 0
					   ELSE ( AC.commission_value / AC.premium ) * 100
					 END
			  END,
			  P.lead_commission_value,
			  P.sub_commission_value,
			  P.sum_insured,
			  P.rating_section_id,
			  IsNull(P.is_levy_tax, 0),
			  RS.original_flag,
			  CASE RS.original_flag
					WHEN 1 THEN I2.expiry_date
					ELSE I.expiry_date
			  END,
			  AC.is_value,
			  IsNull(Pt.is_stamp_duty_insurer, 0),
			  IsNull(Pt.is_stamp_duty_insured, 0),
			  AC.commission_band_id

	FROM      Insurance_File_Risk_Link IFR

	JOIN      Risk R ON R.risk_cnt = IFR.risk_cnt
	JOIN      Peril P ON P.risk_cnt = R.risk_cnt
	JOIN      Peril_Type PT ON PT.peril_type_id = P.peril_type_id
	JOIN      Class_Of_Business CB ON CB.class_of_business_id = P.class_of_business_id
	LEFT JOIN -- Change join so we don't need the union!

	Agent_Commission AC

		ON AC.insurance_file_cnt = @insurance_file_cnt

		   AND AC.commission_band_id = P.lead_commission_band

		   AND AC.risk_type_id = R.risk_type_id

		   AND AC.is_lead_agent = 1

	JOIN      Risk_Type RT

		ON RT.risk_type_id = R.risk_type_id

	JOIN      Rating_Section RS

		ON RS.rating_section_id = P.rating_section_id

		   AND RS.risk_cnt = P.risk_cnt

	JOIN      Insurance_file I

		ON I.insurance_file_cnt = IFR.insurance_file_cnt

	JOIN      Product PROD

		ON I.Product_id = PROD.Product_id

	LEFT JOIN Policy_Section_Type PS

		ON PS.policy_section_type_id = RS.policy_section_type_id

	LEFT JOIN insurance_file_risk_link IFR2

		ON IFR2.risk_cnt = IFR.original_risk_cnt

	LEFT JOIN insurance_file I2

		ON I2.insurance_file_cnt = IFR2.insurance_file_cnt


	WHERE     IFR.insurance_file_cnt = @insurance_file_cnt -- Only select on this policy
			  AND IFR.status_flag NOT IN ('U','R')
			  AND ( IFR2.status_flag = 'C' OR IFR2.status_flag IS NULL or IFR2.status_flag ='D' )
			  AND ( IFR.original_risk_cnt IS NULL

					 OR ( IFR.original_risk_cnt IS NOT NULL

						  AND ISNULL (IFR.is_risk_edited, 0) = 1 )

					 OR IFR.status_flag = 'D'

					 OR ( IFR.status_flag = 'C'

						  AND ISNULL(IFR.is_manually_changed, 0) = 0 ) )

	ORDER     BY P.rating_section_id ASC

	END	
			
	ELSE
			

	BEGIN
		
	INSERT INTO #Perils_Gross
	(
			Risk_id ,
			risk_type_id,
			Risk_type_code,
			peril_id,
			Peril_description,
			peril_type_id ,
			Peril_type_code ,
			peril_policy_section_type_id,
			peril_policy_section_Code,
			peril_class_of_business_id ,
			peril_class_of_business_code,
			peril_annual_premium,
			peril_this_premium_original,
			peril_commission_percent,
			peril_lead_commission_value,
			peril_sub_commission_value,
			peril_sum_insured,
			peril_rating_section_id,
			peril_is_levy_tax,
			peril_original_flag,
			Cover_to_date,
			is_value ,
			is_stamp_duty_insurer,
			is_stamp_duty_insured,
			peril_commission_band
	)


	SELECT  DISTINCT  P.risk_cnt,

			  R.risk_type_id,
			  RT.code,
			  P.peril_id,
			  P.DESCRIPTION,
			  P.peril_type_id,
			  PT.code,
			  RS.policy_section_type_id,
			  PS.code,
			  P.class_of_business_id,
			  CB.code,
			  P.annual_premium,
			  P.this_premium,			--	peril_this_premium_original
			  CASE
				WHEN ISNULL(AC.premium, 0) = 0 THEN CASE
													  WHEN AC.commission_percentage = 0 THEN AC.commission_value
													  ELSE AC.commission_percentage
													END
				ELSE CASE
					   WHEN P.this_premium = 0 THEN 0
					   ELSE ( AC.commission_value / AC.premium ) * 100
					 END
			  END,
			  P.lead_commission_value,
			  P.sub_commission_value,
			  P.sum_insured,
			  P.rating_section_id,
			  IsNull(P.is_levy_tax, 0),
			  RS.original_flag,
			  CASE RS.original_flag
					WHEN 1 THEN I2.expiry_date
					ELSE I.expiry_date
			  END,
			  AC.is_value,
			  IsNull(Pt.is_stamp_duty_insurer, 0),
			  IsNull(Pt.is_stamp_duty_insured, 0),
			  AC.commission_band_id

	FROM      Insurance_File_Risk_Link IFR

	JOIN      Risk R ON R.risk_cnt = IFR.risk_cnt
	JOIN      Peril P ON P.risk_cnt = R.risk_cnt
	JOIN      Peril_Type PT ON PT.peril_type_id = P.peril_type_id
	JOIN      Class_Of_Business CB ON CB.class_of_business_id = P.class_of_business_id
	LEFT JOIN -- Change join so we don't need the union!

	Agent_Commission AC

		ON AC.insurance_file_cnt = @insurance_file_cnt

		   AND AC.commission_band_id = P.lead_commission_band

		   AND AC.risk_type_id = R.risk_type_id

		   AND AC.is_lead_agent = 1

		   AND AC.peril_type_id = p.peril_type_id

		   AND AC.class_of_business_id = P.class_of_business_id

	JOIN      Risk_Type RT

		ON RT.risk_type_id = R.risk_type_id

	JOIN      Rating_Section RS

		ON RS.rating_section_id = P.rating_section_id

		   AND RS.risk_cnt = P.risk_cnt

	JOIN      Insurance_file I

		ON I.insurance_file_cnt = IFR.insurance_file_cnt

	JOIN      Product PROD

		ON I.Product_id = PROD.Product_id

	LEFT JOIN Policy_Section_Type PS

		ON PS.policy_section_type_id = RS.policy_section_type_id

	LEFT JOIN insurance_file_risk_link IFR2

		ON IFR2.risk_cnt = IFR.original_risk_cnt

	LEFT JOIN insurance_file I2

		ON I2.insurance_file_cnt = IFR2.insurance_file_cnt


	WHERE     IFR.insurance_file_cnt = @insurance_file_cnt -- Only select on this policy
			  AND IFR.status_flag NOT IN ('U','R')
			  AND ( IFR2.status_flag = 'C' OR IFR2.status_flag IS NULL or IFR2.status_flag ='D' )
			  AND ( IFR.original_risk_cnt IS NULL

					 OR ( IFR.original_risk_cnt IS NOT NULL

						  AND ISNULL (IFR.is_risk_edited, 0) = 1 )

					 OR IFR.status_flag = 'D'

					 OR ( IFR.status_flag = 'C'

						  AND ISNULL(IFR.is_manually_changed, 0) = 0 ) )

	ORDER     BY P.rating_section_id ASC

END

	IF (@is_coinsured_policy > 0)

	BEGIN

		-- Get next stats_detail_id and set type

		SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)

		FROM    Stats_Detail

		WHERE   stats_folder_cnt = @stats_folder_cnt



		DECLARE @Type TABLE ( ID INT, stats_detail_type VARCHAR(10), is_levy_tax INT)

		INSERT INTO @Type ( ID,stats_detail_type,is_levy_tax) VALUES (1,'TAC',1)

		INSERT INTO @Type ( ID,stats_detail_type,is_levy_tax) VALUES (2,'TAN',1)

		INSERT INTO @Type ( ID, stats_detail_type,is_levy_tax) VALUES (3,'COI',0)


		Update P SET P.peril_this_premium_home = ROUND(P.peril_this_premium_original * @out_currency_rate , @base_decimals),

					P.peril_this_premium_system = ROUND(P.peril_this_premium_original * @out_currency_rate * @out_system_rate , @base_decimals),

					P.peril_sum_insured_home  = ROUND(P.peril_sum_insured * @out_currency_rate , @base_decimals),

					P.peril_sum_insured_system  = ROUND(P.peril_sum_insured * @out_currency_rate * @out_system_rate , @base_decimals)

		FROM #Perils_Gross  P


		select @peril_is_levy_tax = is_levy_tax from @Type


		 -- Insert the Stats Detail

                    INSERT INTO Stats_Detail (

                        stats_folder_cnt,

                        stats_detail_id,

                        stats_detail_type,

                        risk_id,

                        risk_type_id,

                        risk_type_code,


                        peril_id,

                        peril_description,

                        peril_type_id,

                        peril_type_code,

                        policy_section_type_id,

                        policy_section_type_code,

                        class_of_business_id,

                        class_of_business_code,

                        tax_type_code,

                        tax_value,


                        ri_party_cnt,

                        ri_shortname,

                        ri_share_percent,

                        this_premium_original,

                        this_premium_home,

                        this_premium_system,



                        currency_code,

                        currency_rate,

   						original_flag,

   						cover_to_date,

   						ri_party_type,


   						Annual_premium, --3

   						commission_percent,

                        lead_commission_value_home,

                        lead_commission_value_system,

                        sum_insured_home,

                        sum_insured_system,

                        sum_insured_currency_code,

                        cover_share_percent,

                        sum_insured_total

   						)


                    SELECT

                        @stats_folder_cnt,

                        @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY P.ID) As ROW,

                     CASE T.Stats_detail_type

							WHEN 'TAG' THEN

							   CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1
                        THEN 'TAX'
						ELSE T.Stats_detail_type END ELSE T.Stats_detail_type END,

                        P.risk_id,

                        P.risk_type_id,

                        P.risk_type_code,


                        CASE      -- Peril_id

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'COI' THEN P.peril_id

							ELSE NULL end,

						CASE								     -- Peril_Description

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'COI' THEN P.Peril_description

							ELSE NULL end,

						CASE							     -- peril_type_id

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'COI'  THEN P.peril_type_id

							ELSE NULL end,

						CASE								-- peril_type_Code

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'COI' THEN P.Peril_type_code

							ELSE NULL end,

						CASE T.stats_detail_type     -- policy_section_type_id

							WHEN 'TAN' THEN P.peril_policy_section_type_id

							ELSE NULL end,

						CASE T.stats_detail_type     -- policy_section_code

							WHEN 'TAN' THEN P.peril_policy_section_Code

							ELSE NULL end,

						CASE      -- class_of_business_id

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'COI' THEN P.peril_class_of_business_id

							ELSE NULL end,

						CASE      -- class_of_business_code

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'COI' THEN P.peril_class_of_business_code

							ELSE NULL end,

						CASE      -- class_of_business_code

							WHEN T.stats_detail_type = 'TAN' OR T.stats_detail_type = 'TAC' THEN P.peril_class_of_business_code

							ELSE NULL end,

						CASE T.stats_detail_type     -- tax_value

							WHEN 'TAC' THEN P.peril_this_premium_original * C.share_percent / 100

							WHEN 'TAN' THEN -1 * P.peril_this_premium_original * C.share_percent / 100

							ELSE NULL end,

						CASE      -- ri_party_cnt

							WHEN T.stats_detail_type = 'TAC' OR T.stats_detail_type = 'COI' THEN C.party_cnt

							ELSE NULL end,

						CASE       -- RI_Shortname

							WHEN T.stats_detail_type = 'TAC' OR T.stats_detail_type = 'COI' THEN C.shortname

							ELSE 'NOTARI' + RTRIM(P.peril_class_of_business_code) end,


						C.share_percent,


						--this_premium_original
						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN  CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 then 0
							  else	-1 * P.peril_this_premium_original * C.share_percent * C.commission_percent / (100* 100) end
                        WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN  CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 then 0
							  else	P.peril_this_premium_original * C.share_percent / 100 end

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))

						--THEN P.peril_this_premium_system * C.commission_percent / 100
						 THEN  CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 then 0 else	(P.peril_this_premium_original-(P.peril_this_premium_original * C.share_percent / 100))* C.commission_percent/ 100 END
						WHEN T.stats_detail_type = 'COI' THEN P.peril_this_premium_original * C.share_percent / 100

						ELSE NULL end,


						--this_premium_home

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN Round(-1 * P.peril_this_premium_system * C.share_percent * C.commission_percent / (100* 100),@base_decimals)

                        WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_home * C.share_percent / 100,@base_decimals)

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))

						--THEN P.peril_this_premium_system * C.commission_percent / 100
						 THEN ROUND((P.peril_this_premium_home-(P.peril_this_premium_home * C.share_percent / 100))* C.commission_percent/ 100,@base_decimals)

						WHEN T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_home * C.share_percent / 100,@base_decimals)

						ELSE NULL end,


						--this_premium_system

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN ROUND(-1 * P.peril_this_premium_system * C.share_percent * C.commission_percent / (100* 100),@base_decimals)

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_system * C.share_percent / 100,@base_decimals)

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))

						--THEN P.peril_this_premium_system * C.commission_percent / 100
						    THEN ROUND((P.peril_this_premium_home-(P.peril_this_premium_home * C.share_percent / 100))* C.commission_percent/ 100,@base_decimals)

						WHEN T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_system * C.share_percent / 100,@base_decimals)

						ELSE NULL end,



						@out_currency_code,

						@out_currency_rate,

						P.peril_original_flag,

						P.Cover_to_date,


						CASE       -- ri_party_type

						WHEN T.stats_detail_type = 'TAC' OR T.stats_detail_type = 'COI' THEN C.party_type_id

						ELSE NULL end,


						--Annual_premium

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_system * C.share_percent * C.commission_percent / (100* 100),@base_decimals)

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_annual_premium * C.share_percent / 100,@base_decimals)

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))

                     --   THEN P.peril_this_premium_system * C.commission_percent / 100
                          THEN ROUND((P.peril_this_premium_home-(P.peril_this_premium_home * C.share_percent / 100))* C.commission_percent/ 100,@base_decimals)

						WHEN T.stats_detail_type = 'COI' THEN ROUND(P.peril_annual_premium * C.share_percent / 100,@base_decimals)

						ELSE NULL end,



						--Commission_percent

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN 0

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN C.commission_percent

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))
 THEN 0

						WHEN T.stats_detail_type = 'COI' THEN C.commission_percent

						ELSE NULL end,


						--lead_commission_value_home

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN 0

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_home * C.share_percent * C.commission_percent/ (100* 100),@base_decimals)

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI')) 
						 OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))
                         THEN ROUND((P.peril_this_premium_home-(P.peril_this_premium_home * C.share_percent / 100))* C.commission_percent/ 100,@base_decimals)

					--	WHEN T.stats_detail_type = 'COI' THEN P.peril_this_premium_home * C.share_percent * C.commission_percent/ (100* 100)

						ELSE NULL end,


						--lead_commission_value_system

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN 0

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_this_premium_system * C.share_percent * C.commission_percent/ (100* 100),@base_decimals)

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))
 THEN ROUND((P.peril_this_premium_system-(P.peril_this_premium_system * C.share_percent / 100))* C.commission_percent/ 100,@base_decimals)
 
 


					--	WHEN T.stats_detail_type = 'COI' THEN P.peril_this_premium_system * C.share_percent * C.commission_percent/ (100* 100)

						ELSE NULL end,


                        --sum_insured_home

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

						WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI'))

						THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

						WHEN T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

						ELSE NULL end,


						--sum_insured_system

						CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_system * C.share_percent  / 100.00,@base_decimals)

						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_system * C.share_percent  / 100.00,@base_decimals)

                        WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS')
						AND (T.stats_detail_type = 'COI'))

						THEN ROUND(P.peril_sum_insured_system * C.share_percent  / 100.00,@base_decimals)

						WHEN T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_system * C.share_percent  / 100.00,@base_decimals)

						ELSE NULL end,





						CASE T.stats_detail_type     --sum_insured_currency_code

							WHEN 'COI' THEN @out_currency_code

							ELSE NULL end,

						CASE T.stats_detail_type     --@cover_share_percent

							WHEN 'COI' THEN C.share_percent

							ELSE NULL end,


						----sum_insured_total

							CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

    						WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

							WHEN ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT') AND (T.stats_detail_type = 'COI'))  OR ((@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'GROSS') AND (T.stats_detail_type = 'COI')
)

							THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

							WHEN T.stats_detail_type = 'COI' THEN ROUND(P.peril_sum_insured_home * C.share_percent  / 100.00,@base_decimals)

							ELSE NULL end

                 	FROM #Coinsurance C, #Perils_Gross P JOIN @Type T ON P.peril_is_levy_tax = T.is_levy_tax


                 	Order BY P.ID,t.id


	END



	Update P SET P.peril_original_sum_insured = Peril.sum_insured

	FROM #Perils_Gross P

	JOIN Peril ON peril.peril_type_id = P.peril_type_id AND Peril.risk_cnt = P.Risk_id

	JOIN Rating_Section RS ON RS.rating_section_id = Peril.rating_section_id AND RS.risk_cnt = Peril.risk_cnt AND RS.original_flag <> P.peril_original_flag


	-- Calculate the SI change

	Update P SET P.peril_sum_insured_change  = ROUND(P.peril_sum_insured + ISNULL(P.peril_original_sum_insured,0),@base_decimals),

				 P.peril_sum_insured_home = ROUND(P.peril_sum_insured * @out_currency_rate , @base_decimals ),

				 P.peril_this_premium_home = ROUND(P.peril_this_premium_original * @out_currency_rate , @base_decimals),

				 P.peril_sum_insured_system = ROUND(P.peril_sum_insured * @out_currency_rate * @out_system_rate , @base_decimals ),

				 P.peril_this_premium_system = ROUND(P.peril_this_premium_original * @out_currency_rate * @out_system_rate , @base_decimals ),

				 P.peril_sum_insured_change_home  = ROUND((P.peril_sum_insured + ISNULL(P.peril_original_sum_insured,0)) * @out_currency_rate, @base_decimals)

	FROM #Perils_Gross P

	Update P SET P.peril_lead_commission_value_home = P.peril_this_premium_home * P.peril_commission_percent / 100.00,

				P.peril_lead_commission_value_system = P.peril_this_premium_system * P.peril_commission_percent / 100.00

	FROM #Perils_Gross P

	WHERE  ISNULL(P.is_value, 0) = 0 AND P.peril_this_premium_home <> 0

	Update P SET P.peril_lead_commission_value_home =  P.peril_commission_percent,

				  P.peril_lead_commission_value_system =  P.peril_commission_percent

	FROM #Perils_Gross P

	WHERE  ISNULL(P.is_value, 0) = 1


	Update P SET P.peril_lead_commission_value_home =  0.00, P.peril_lead_commission_value_system = 0.00

	FROM #Perils_Gross P

	WHERE  ISNULL(P.is_value, 0) = 0  AND P.peril_this_premium_home  = 0
	DECLARE comm_cursor CURSOR FOR     
		SELECT	commission_value * @out_currency_rate,
				commission_value* @out_currency_rate * @out_system_rate,
				risk_type_id,
				class_of_business_id,	
			    peril_type_id,
			    commission_band_id 
		FROM  Agent_Commission WHERE insurance_file_cnt = @insurance_file_cnt and is_lead_agent = 1  
	  
	OPEN comm_cursor    
  
	FETCH NEXT FROM comm_cursor INTO @commission_value_home,@commission_value_system ,@RiskTypeId, @class_of_business_id, @peril_type_id,@commission_band_id

	WHILE @@FETCH_STATUS = 0    
	BEGIN    

		SELECT @Sum_peril_lead_commission_value_home = SUM(peril_lead_commission_value_home), @Sum_peril_lead_commission_value_system = SUM(peril_lead_commission_value_system) from #Perils_Gross 
		WHERE risk_type_id =@RiskTypeId AND peril_commission_band =  @commission_band_id
		AND peril_type_id = @peril_type_id 
		AND peril_class_of_business_id =@class_of_business_id 

	Update TOP (1) P SET 
		P.peril_lead_commission_value_home = P.peril_lead_commission_value_home + (@commission_value_home - @Sum_peril_lead_commission_value_home),
		peril_lead_commission_value_system = P.peril_lead_commission_value_system + (@commission_value_system - @Sum_peril_lead_commission_value_system) 
		FROM #Perils_Gross P WHERE P.risk_type_id =@RiskTypeId and peril_commission_band =  @commission_band_id 
		AND P.risk_type_id =@RiskTypeId 
		AND p.peril_type_id = @peril_type_id 
		AND p.peril_class_of_business_id = @class_of_business_id
		And peril_commission_band =  @commission_band_id
	
	FETCH NEXT FROM comm_cursor INTO @commission_value_home,@commission_value_system ,@RiskTypeId, @class_of_business_id, @peril_type_id,@commission_band_id 
   
	END     
	CLOSE comm_cursor   
	DEALLOCATE comm_cursor


	DECLARE @Type2 TABLE ( ID INT, stats_detail_type VARCHAR(10), is_levy_tax BIT,  is_stamp_duty_insurer BIT, is_stamp_duty_insured BIT)

	INSERT INTO @Type2 (ID,stats_detail_type,is_levy_tax,is_stamp_duty_insurer,is_stamp_duty_insured) VALUES (1,'TAG',1,0,0 )

	INSERT INTO @Type2 (ID,stats_detail_type,is_levy_tax,is_stamp_duty_insurer,is_stamp_duty_insured) VALUES (2,'TAN',1,0,0 )

	INSERT INTO @Type2 (ID,stats_detail_type,is_levy_tax,is_stamp_duty_insurer,is_stamp_duty_insured) VALUES (3,'TAG',0,1,1 )

	INSERT INTO @Type2 (ID,stats_detail_type,is_levy_tax,is_stamp_duty_insurer,is_stamp_duty_insured) VALUES (4,'TAN',0,1,1 )

	INSERT INTO @Type2 (ID,stats_detail_type,is_levy_tax,is_stamp_duty_insurer,is_stamp_duty_insured) VALUES (5,'GRS',0,0,0 )






		-- Get next stats_detail_id and set type


		SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)

		FROM    Stats_Detail

		WHERE   stats_folder_cnt = @stats_folder_cnt

			IF @is_coinsured_policy > 0

	            Begin

		    		SELECT

			    DISTINCT

				P.ID AS 'P_Id',

				T.ID AS 'T_Id',

				@stats_folder_cnt AS 'stats_folder_cnt',

				@out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY P.ID) As ROW,

                T.stats_detail_type AS 'stats_detail_type',

                P.risk_id AS 'risk_id',

				P.risk_type_id AS 'risk_type_id',

				P.risk_type_code AS 'risk_type_code',

				CASE T.stats_detail_type     -- Peril_id

				    WHEN 'TAG' THEN NULL

				    ELSE P.peril_id end AS 'Peril_id' ,


				CASE T.stats_detail_type     -- Peril_Description

				    WHEN 'TAG' THEN NULL

     			    ELSE P.Peril_description end AS 'Peril_Description',


				CASE T.stats_detail_type     -- peril_type_id

				    WHEN 'TAG' THEN P.peril_type_id

				    ELSE P.peril_type_id end AS 'peril_type_id',


				CASE T.stats_detail_type     -- peril_type_Code

				    WHEN 'TAG' THEN P.Peril_type_code

				    ELSE P.Peril_type_code end AS 'peril_type_Code',


				CASE T.stats_detail_type     -- policy_section_type_id

				    WHEN 'TAG' THEN  NULL

				    ELSE P.peril_policy_section_type_id end AS 'policy_section_type_id',


				CASE T.stats_detail_type     -- policy_section_code

				    WHEN 'TAG' THEN NULL

				    ELSE P.peril_policy_section_Code end 'policy_section_code',


				CASE T.stats_detail_type     -- class_of_business_id

					WHEN 'TAG'THEN NULL

					ELSE P.peril_class_of_business_id end AS 'class_of_business_id',


				CASE T.stats_detail_type     -- class_of_business_code

					WHEN 'TAG' THEN NULL

					ELSE P.peril_class_of_business_code end AS 'class_of_business_code',


				CASE T.stats_detail_type     -- tax_type_code

					WHEN 'GRS' THEN NULL

					ELSE P.peril_class_of_business_code end AS 'tax_type_code',


				CASE T.stats_detail_type     -- tax_value

					WHEN 'TAG' THEN P.peril_this_premium_original

					WHEN 'TAN' THEN - P.peril_this_premium_original

					ELSE NULL end AS 'tax_value',


				CASE P.peril_is_levy_tax  --ri_shortname

					WHEN 1 THEN


						CASE T.Stats_detail_type

							WHEN 'TAG' THEN NULL

             				WHEN 'TAN' THEN 'NOTA' + RTrim(P.peril_class_of_business_code)

						END

					ELSE

						CASE T.Stats_detail_type

							WHEN 'TAG' THEN

                		CASE WHEN P.is_stamp_duty_insurer = 1 THEN 'NOTAOUTSDT' + RTrim(P.peril_class_of_business_code)
						WHEN P.is_stamp_duty_insured = 1 THEN 'NOIN' + RTrim(P.peril_class_of_business_code)

                	ELSE NULL end

						WHEN 'TAN' THEN

                			CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 THEN 'NOTASDT' + RTrim(P.peril_class_of_business_code)end

						ELSE

							NULL

				END

			 END AS 'ri_shortname',


			-- this_premium_original

			CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT'  AND T.stats_detail_type = 'GRS' THEN  
			CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 then 
			  P.peril_this_premium_original
			 else	
			 P.peril_this_premium_original-(P.peril_this_premium_original * (SELECT  Sum(share_percent) from #Coinsurance) / 100)
			 end
				 WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND  @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'GRS' THEN
			 P.peril_this_premium_original

			 WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsFollow AND (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS') AND T.stats_detail_type = 'GRS'  THEN
			 CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 then 
			 P.peril_this_premium_original
			 else
			 P.peril_this_premium_original-(P.peril_this_premium_original * (SELECT  Sum(share_percent) from #Coinsurance) / 100)
			 end
				WHEN T.stats_detail_type = 'TAN' THEN ROUND(- P.peril_this_premium_original,@base_decimals)

    			ELSE  P.peril_this_premium_original

			end AS 'this_premium_original',


			-- this_premium_home

	CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'GRS' 
	THEN ROUND(P.peril_this_premium_home-(P.peril_this_premium_home * (SELECT  Sum(share_percent) from #Coinsurance) / 100),@base_decimals)
	 WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND  @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'GRS' THEN ISNULL(P.peril_this_premium_home,@base_decimals)

	 WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsFollow  AND (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS') AND T.stats_detail_type = 'GRS' 
	  THEN ROUND(P.peril_this_premium_home-(P.peril_this_premium_home * (SELECT  Sum(share_percent) from #Coinsurance) / 100),@base_decimals)

				WHEN T.stats_detail_type = 'TAN' THEN ROUND(-P.peril_this_premium_home,@base_decimals)

				ELSE  ROUND(P.peril_this_premium_home,@base_decimals)

			end AS 'this_premium_home',


			-- this_premium_system
			CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS') AND T.stats_detail_type = 'GRS' THEN (P.peril_this_premium_system)

			WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsFollow AND (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS') AND T.stats_detail_type = 'GRS' 
			THEN ROUND(P.peril_this_premium_system-(P.peril_this_premium_system * (SELECT  Sum(share_percent) from #Coinsurance) / 100),@base_decimals)
				WHEN T.stats_detail_type = 'TAN' THEN - P.peril_this_premium_system
				ELSE  ROUND(P.peril_this_premium_system,@base_decimals)
			end AS 'this_premium_system',



			@out_currency_code AS 'out_currency_code',

			@out_currency_rate AS 'out_currency_rate',

			P.peril_original_flag AS 'peril_original_flag',

			P.Cover_to_date AS 'Cover_to_date',


			--GRS Only


			--Annual_premium

			CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT' AND T.stats_detail_type = 'GRS'

				THEN ROUND(P.peril_annual_premium-(P.peril_annual_premium * (SELECT  Sum(share_percent) from #Coinsurance) / 100),@base_decimals)

			    WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND  @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'GRS'

				THEN ROUND(P.peril_annual_premium,@base_decimals)

				WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsFollow AND (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS') AND T.stats_detail_type = 'GRS' 

				THEN ROUND(P.peril_annual_premium-(P.peril_annual_premium * (SELECT  Sum(share_percent) from #Coinsurance) / 100),@base_decimals)

				WHEN T.stats_detail_type = 'GRS' THEN ROUND(P.peril_annual_premium,@base_decimals)

				ELSE 0.00 end AS 'Annual_premium',


			CASE T.stats_detail_type     -- commission_percent

				WHEN 'GRS' THEN P.peril_commission_percent

				ELSE NULL end AS 'commission_percent',


			-- lead_commission_value_home

			CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' AND T.stats_detail_type = 'GRS' AND P.peril_commission_percent > 0 

				THEN ROUND((P.peril_this_premium_home  *P.peril_commission_percent)/100,@base_decimals)
				 
				WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsFollow OR (@lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'NETT') AND T.stats_detail_type = 'GRS'

				THEN ROUND(((P.peril_this_premium_home-(P.peril_this_premium_home * (SELECT  Sum(share_percent) from #Coinsurance) / 100))*P.peril_commission_percent)/100,@base_decimals)
				
				WHEN T.stats_detail_type = 'GRS' THEN  Round(P.peril_lead_commission_value_home,@base_decimals)  

				ELSE NULL end 'lead_commission_value_home',

			-- lead_commission_value_system

			CASE WHEN (@lBusiness_type_id = @nBusinessTypeIdCoinsLead or @lBusiness_type_id = @nBusinessTypeIdCoinsFollow) AND (@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS') AND T.stats_detail_type = 'GRS'

				THEN ROUND(((P.peril_this_premium_system-(P.peril_this_premium_system *(SELECT  Sum(share_percent) from #Coinsurance) / 100))*P.peril_commission_percent)/100,@base_decimals)

				
				WHEN T.stats_detail_type = 'GRS' THEN P.peril_lead_commission_value_system

				ELSE NULL end AS 'lead_commission_value_system',




			CASE T.stats_detail_type     -- sub_commission_value_home

				WHEN 'GRS' THEN P.peril_sub_commission_value_home

				ELSE NULL end AS 'sub_commission_value_home',

			CASE T.stats_detail_type     -- Sub_commission_value_system

				WHEN 'GRS' THEN P.peril_sub_commission_value_system

			    ELSE NULL end AS 'Sub_commission_value_system',


			CASE T.stats_detail_type     -- sum_insured_home

				 WHEN 'GRS' THEN P.peril_sum_insured_home

				 ELSE NULL end AS 'sum_insured_home',


			CASE T.stats_detail_type     -- sum_insured_system

				WHEN 'GRS' THEN P.peril_sum_insured_system

				ELSE NULL end AS 'sum_insured_system',

			CASE T.stats_detail_type     -- sum_insured_currency_code

				 WHEN 'GRS' THEN @out_currency_code

				 ELSE NULL end AS 'sum_insured_currency_code',

			CASE T.stats_detail_type     -- sum_insured_change

					WHEN 'GRS' THEN P.peril_sum_insured_change_home

            ELSE NULL end AS 'sum_insured_change'


			INTO #TEMP_STATS


			FROM #Perils_Gross P, @Type2 T

			WHERE P.peril_is_levy_tax = T.is_levy_tax AND (P.is_stamp_duty_insured = T.is_stamp_duty_insured OR P.is_stamp_duty_insurer = T.is_stamp_duty_insurer)

			order by P.ID , T.ID --) AS TMP



			  INSERT INTO Stats_Detail (

			     stats_folder_cnt,

				 stats_detail_id,

				 stats_detail_type,

                 risk_id,

				 risk_type_id,

                 risk_type_code,


				 peril_id,

                 peril_description,

				 peril_type_id,

				 peril_type_code,


				 policy_section_type_id,

				 policy_section_type_code,

				 class_of_business_id,

				 class_of_business_code,

				 tax_type_code,

				 tax_value,

				 ri_shortname,


				 this_premium_original,

				 this_premium_home,

				 this_premium_system,


				 currency_code,

				 currency_rate,

				 original_flag,

				 cover_to_date,


    			--GRS

				 annual_premium,

				 commission_percent,

				 lead_commission_value_home,

				 lead_commission_value_system,

				 sub_commission_value_home,

				 sub_commission_value_system,

				 sum_insured_home,

				 sum_insured_system,

				 sum_insured_currency_code,

				 sum_insured_change


    			)





			SELECT

				stats_folder_cnt,

				ROW,

				stats_detail_type,

				risk_id,

				risk_type_id,

				risk_type_code,

				Peril_id,

				Peril_Description,

				peril_type_id,

				peril_type_Code,

				policy_section_type_id,

				policy_section_code,

				class_of_business_id,

				class_of_business_code,

				tax_type_code,

				tax_value,

				ri_shortname,

				this_premium_original,

				this_premium_home,

				this_premium_system,

				out_currency_code,

				out_currency_rate,

				peril_original_flag,

				Cover_to_date,

				Annual_premium,

				commission_percent,

				lead_commission_value_home,

				lead_commission_value_system,

				sub_commission_value_home,

				Sub_commission_value_system,

				sum_insured_home,

				sum_insured_system,

				sum_insured_currency_code,

				sum_insured_change


			FROM #TEMP_STATS
			     End
      Else

         Begin

         -- Insert the Stats Detail

            INSERT INTO Stats_Detail (

                stats_folder_cnt,

                stats_detail_id,

                stats_detail_type,

                risk_id,

                risk_type_id,

                risk_type_code,


                peril_id,

                peril_description,

                peril_type_id,

                peril_type_code,


                policy_section_type_id,

                policy_section_type_code,

                class_of_business_id,

                class_of_business_code,

                tax_type_code,

                tax_value,

                ri_shortname,


                this_premium_original,

                this_premium_home,

                this_premium_system,


                currency_code,

                currency_rate,

    			original_flag,

    			cover_to_date,


    			--GRS

    			annual_premium,

				commission_percent,

				lead_commission_value_home,

				lead_commission_value_system,

				sub_commission_value_home,

				sub_commission_value_system,

				sum_insured_home,

				sum_insured_system,

				sum_insured_currency_code,

				sum_insured_change

    			)

    		SELECT

                @stats_folder_cnt,

                @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY P.ID) As ROW,

                 CASE T.Stats_detail_type

							WHEN 'TAG' THEN

							   CASE WHEN P.is_stamp_duty_insurer = 1 or P.is_stamp_duty_insured = 1
                        THEN 'TAX'
						ELSE T.Stats_detail_type END ELSE T.Stats_detail_type END,

                P.risk_id,

                P.risk_type_id,

                P.risk_type_code,


            CASE T.stats_detail_type     -- Peril_id

				WHEN 'TAG' THEN NULL

				ELSE P.peril_id end,


			CASE T.stats_detail_type     -- Peril_Description

				WHEN 'TAG' THEN NULL

				ELSE P.Peril_description end,

			CASE T.stats_detail_type     -- peril_type_id

				WHEN 'TAG' THEN NULL

				ELSE P.peril_type_id end,

			CASE T.stats_detail_type     -- peril_type_Code

				WHEN 'TAG' THEN NULL

				ELSE P.Peril_type_code end,

			CASE T.stats_detail_type     -- policy_section_type_id

				WHEN 'TAG' THEN  NULL

				ELSE P.peril_policy_section_type_id end,

			CASE T.stats_detail_type     -- policy_section_code

				WHEN 'TAG' THEN NULL

				ELSE P.peril_policy_section_Code end,

			CASE T.stats_detail_type     -- class_of_business_id

				WHEN 'TAG'THEN NULL

				ELSE P.peril_class_of_business_id end,

			CASE T.stats_detail_type     -- class_of_business_code

				WHEN 'TAG' THEN NULL

				ELSE P.peril_class_of_business_code end,

			CASE T.stats_detail_type     -- tax_type_code

				WHEN 'GRS' THEN NULL

				ELSE P.peril_class_of_business_code end,

			CASE T.stats_detail_type     -- tax_value

				WHEN 'TAG' THEN P.peril_this_premium_original

				WHEN 'TAN' THEN - P.peril_this_premium_original

				ELSE NULL end,

			CASE P.peril_is_levy_tax  --ri_shortname

				WHEN 1 THEN

						CASE T.Stats_detail_type

							WHEN 'TAG' THEN NULL

							WHEN 'TAN' THEN 'NOTA' + RTrim(P.peril_class_of_business_code)

						END

				ELSE

					CASE T.Stats_detail_type

						WHEN 'TAG' THEN

							CASE WHEN P.is_stamp_duty_insurer = 1 THEN 'NOTAOUTSDT' + RTrim(P.peril_class_of_business_code)
							WHEN P.is_stamp_duty_insured = 1 THEN 'NOIN' + RTrim(P.peril_class_of_business_code)

							ELSE NULL end

						WHEN 'TAN' THEN

							CASE WHEN P.is_stamp_duty_insurer = 1 OR P.is_stamp_duty_insured = 1 THEN 'NOTASDT' + RTrim(P.peril_class_of_business_code)end

						ELSE

							NULL

					END

			END,


			CASE T.stats_detail_type     -- this_premium_original

				WHEN 'TAN' THEN - P.peril_this_premium_original

				ELSE  P.peril_this_premium_original

			end,


			CASE T.stats_detail_type     -- this_premium_home

				WHEN 'TAN' THEN ROUND(-P.peril_this_premium_home,@base_decimals)

				ELSE  ROUND(P.peril_this_premium_home,@base_decimals)

			end,

			CASE T.stats_detail_type     -- this_premium_system

				WHEN 'TAN' THEN ROUND(- P.peril_this_premium_system,@base_decimals)

				ELSE  ROUND(P.peril_this_premium_system,@base_decimals)

			end,

			@out_currency_code,

			@out_currency_rate,

			P.peril_original_flag,

			P.Cover_to_date,


			--GRS Only


			CASE T.stats_detail_type     -- Annual Premium

				WHEN 'GRS' THEN Round(P.peril_annual_premium,@base_decimals)

				ELSE NULL end,


			CASE T.stats_detail_type     -- commission_percent

				WHEN 'GRS' THEN P.peril_commission_percent

				ELSE NULL end,

			CASE T.stats_detail_type     -- lead_commission_value_home

				WHEN 'GRS' THEN P.peril_lead_commission_value_home

				ELSE NULL end,

			CASE T.stats_detail_type     -- lead_commission_value_system

				WHEN 'GRS' THEN Round(P.peril_lead_commission_value_system,@base_decimals)

				ELSE NULL end,

			CASE T.stats_detail_type     -- sub_commission_value_home

				WHEN 'GRS' THEN P.peril_sub_commission_value_home

				ELSE NULL end,

			CASE T.stats_detail_type     -- Sub_commission_value_system

				WHEN 'GRS' THEN P.peril_sub_commission_value_system

				ELSE NULL end,


			CASE T.stats_detail_type     -- sum_insured_home

				WHEN 'GRS' THEN P.peril_sum_insured_home

				ELSE NULL end,


			CASE T.stats_detail_type     -- sum_insured_system

				WHEN 'GRS' THEN P.peril_sum_insured_system

				ELSE NULL end,

			CASE T.stats_detail_type     -- sum_insured_currency_code

				WHEN 'GRS' THEN @out_currency_code

				ELSE NULL end,

			CASE T.stats_detail_type     -- sum_insured_change

				 WHEN 'GRS' THEN P.peril_sum_insured_change_home

				 ELSE NULL end


			FROM #Perils_Gross P, @Type2 T

			WHERE

			P.peril_is_levy_tax = T.is_levy_tax AND (P.is_stamp_duty_insured = T.is_stamp_duty_insured OR P.is_stamp_duty_insurer = T.is_stamp_duty_insurer)

			order by P.ID , T.ID

	    End


	-- ********************************************************************

    --                     GROSS RECORDS FOR TAXES

    -- ********************************************************************



CREATE TABLE #TaxGross

(

	ID						INT IDENTITY,

	tax_band_id				INT,

	premium					NUMERIC(19,4),

	tax_percentage			FLOAT,

	tax_value				NUMERIC(19,4),

	tax_is_value			BIT,

	tax_band_code			VARCHAR(10),

	is_share_with_co_insurers tinyint,

	tax_level				VARCHAR(10),

	is_not_applied_to_client tinyint,

	risk_id					INT,

	risk_type_id			INT,

	risk_type_code			VARCHAR(10),

	class_of_business_id	INT,

	class_of_business_code 	VARCHAR(10),

	--is_suspended			TINYINT,

	--suspended_caption		VARCHAR(1),

	out_stats_detail_type	VARCHAR(10),

	out_ri_shortname		VARCHAR(100),

	stats_detail_type_temp  VARCHAR(10)

)



 -- Get next stats_detail_id and set type

		SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)

		FROM    Stats_Detail

		WHERE   stats_folder_cnt = @stats_folder_cnt


DECLARE @TaxType TABLE ( ID INT, stats_detail_type VARCHAR(10))

		INSERT INTO @TaxType ( ID , stats_detail_type) VALUES (1, 'TAG')

		INSERT INTO @TaxType ( ID , stats_detail_type) VALUES (2, 'TAN')



INSERT INTO #TaxGross

(

	tax_band_id,

	premium,

	tax_percentage,

	tax_value,

	tax_is_value,

	tax_band_code,

	is_share_with_co_insurers,

	tax_level,

	is_not_applied_to_client,

	risk_id,

	risk_type_id,

	risk_type_code,

	class_of_business_id,

	class_of_business_code,

	--is_suspended,

	--suspended_caption,

	stats_detail_type_temp

)


SELECT

		  rt.tax_band_id,

		  rt.premium,

		  rt.percentage,

		  rt.VALUE tax_value,

		  rt.is_value,

		  tb.code,

		  rty.is_share_with_co_insurers,

		  'RISK',

		  tt.is_not_applied_to_client,

		  r.risk_cnt,

		  r.risk_type_id,

		  rty.code,

		  cob.class_of_business_id,

		  cob.code,

		  --tbr.is_suspended,

		  --tbr.suspended_account_code_suffix,

		  T.stats_detail_type


FROM      Insurance_File_Risk_Link IFR

JOIN      Tax_Calculation RT

	ON rt.risk_cnt = IFR.risk_cnt

	   AND RT.insurance_file_cnt = @insurance_file_cnt

JOIN      Risk R

	ON R.risk_cnt = rt.risk_cnt

JOIN      risk_type RTY

	ON RTY.risk_type_id = r.risk_type_id

JOIN      tax_band TB

	ON tb.tax_band_id = rt.tax_band_id

JOIN      Tax_Type TT

	ON tt.tax_type_id = tb.tax_type_id

LEFT JOIN      Class_of_business COB

	ON cob.class_of_business_id = rt.class_of_business_id

--LEFT JOIN tax_band_rate tbr ON tbr.tax_band_rate_id = RT.tax_band_rate_id --E001

LEFT JOIN @TaxType T ON 1 = 1

WHERE     IFR.insurance_file_cnt = @insurance_file_cnt
		  AND IFR.status_flag NOT IN ('U','R')
		  AND ( IFR.original_risk_cnt IS NULL

				 OR ( IFR.original_risk_cnt IS NOT NULL

					  AND ISNULL (IFR.is_risk_edited, 0) = 1 )

				 OR IFR.status_flag = 'D'

				 OR ( IFR.status_flag = 'C'

					  AND ISNULL(IFR.is_manually_changed, 0) = 0 ) )

		  AND RT.transtype = 'TTR' --AND tbr.is_deleted = 0

		  AND rt.VALUE <> 0

UNION ALL


SELECT		ift.tax_band_id,

			ift.premium,

			ift.percentage,

			ift.VALUE tax_value,

			ift.is_value,

			tb.code,

			CASE WHEN @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' THEN 1 ELSE 0 END,

			'POLICY',

			tt.is_not_applied_to_client,

			NULL,

			NULL,

			NULL,

			cob.class_of_business_id,

			cob.code,

			--tbr.is_suspended,--E001

			--tbr.suspended_account_code_suffix,

	        T.stats_detail_type


FROM      Tax_Calculation IFT

JOIN      tax_band TB

	ON tb.tax_band_id = ift.tax_band_id

JOIN      Tax_Type TT

	ON tt.tax_type_id = tb.tax_type_id

LEFT JOIN      Class_of_business COB

	ON cob.class_of_business_id = ift.class_of_business_id

--LEFT JOIN tax_band_rate tbr ON IFT.tax_band_rate_id = tbr.tax_band_rate_id --E001

LEFT JOIN @TaxType T ON 1 = 1

WHERE     IFT.insurance_file_cnt = @insurance_file_cnt

		  AND IFT.risk_cnt IS NULL

		  AND IFT.transtype = 'TTIF'   --AND tbr.is_deleted = 0

		  AND ift.VALUE <> 0

DECLARE @COI_SHARE_PERC NUMERIC (20,4)
	  
	SELECT @COI_SHARE_PERC = 100

	IF @is_coinsured_policy > 0 BEGIN
		IF @lBusiness_type_id = @nBusinessTypeIdCoinsLead AND @sCoins_placement = 'GROSS' 
			BEGIN
				SELECT @COI_SHARE_PERC = 100
			END 
		ELSE 
			BEGIN
	 		SELECT @COI_SHARE_PERC = 100- SUM(share_percent) from #Coinsurance
		END
	END

 -- Insert the Stats Detail

            INSERT INTO Stats_Detail (

                stats_folder_cnt,

                stats_detail_id,

                stats_detail_type,

                risk_id,

                risk_type_id,

                risk_type_code,

                ri_shortname,

                ri_share_percent,

                this_premium_original,

                this_premium_home,

                this_premium_system,

                currency_rate,

                currency_code,

                tax_type_id,

                tax_type_code,

                tax_value,

                class_of_business_id,

                class_of_business_code

             )

           SELECT

                @stats_folder_cnt,

                @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY T.ID) As ROW,


				CASE

					WHEN t.is_not_applied_to_client = 1 AND T.stats_detail_type_temp = 'TAG' THEN 'TAX'

					WHEN t.is_not_applied_to_client <> 1 AND T.stats_detail_type_temp = 'TAG' THEN 'TAG'

					WHEN T.stats_detail_type_temp = 'TAN' THEN 'TAN'

				end 'out_stats_detail_type',


                T.risk_id,

                T.risk_type_id,

                T.risk_type_code,


                CASE

				WHEN t.is_not_applied_to_client = 1 AND T.stats_detail_type_temp = 'TAG' THEN 'NOTAOUT' + RTRIM(t.tax_band_code)

				WHEN t.is_not_applied_to_client <> 1 AND T.stats_detail_type_temp = 'TAG' THEN  NULL

				WHEN T.stats_detail_type_temp = 'TAN'  THEN 'NOTA' + RTRIM(t.tax_band_code)

				end out_ri_shortname,



                CASE T.stats_detail_type_temp

					WHEN 'TAG'THEN T.tax_percentage

					ELSE NULL end,


                CASE T.stats_detail_type_temp --this_premium_original

					WHEN 'TAG' THEN T.tax_value * @COI_SHARE_PERC/100

					ELSE -T.tax_value * @COI_SHARE_PERC/100 end,

				CASE T.stats_detail_type_temp --this_premium_home

					WHEN 'TAG' THEN ROUND(T.tax_value * @COI_SHARE_PERC/100 * @out_currency_rate,@base_decimals)

					ELSE ROUND(-T.tax_value * @COI_SHARE_PERC/100 * @out_currency_rate,@base_decimals)end,

				CASE T.stats_detail_type_temp --this_premium_system

					WHEN 'TAG' THEN ROUND(T.tax_value * @COI_SHARE_PERC/100 * @out_currency_rate *  @out_system_rate ,@base_decimals)

					ELSE ROUND(-T.tax_value * @COI_SHARE_PERC/100 * @out_currency_rate * @out_system_rate,@base_decimals)end,


                @out_currency_rate,

                @out_currency_code,

                T.tax_band_id,

                T.tax_band_code,


                CASE T.stats_detail_type_temp --this_premium_original

					WHEN 'TAG' THEN T.tax_value * @COI_SHARE_PERC/100

					ELSE -T.tax_value * @COI_SHARE_PERC/100 end,

				CASE T.stats_detail_type_temp --class_of_business_id

					WHEN 'TAN' THEN T.class_of_business_id

					ELSE NULL end,

				CASE T.stats_detail_type_temp --class_of_business_code

					WHEN 'TAN' THEN T.class_of_business_code

					ELSE NULL end


           FROM #TaxGross T






    -- ********************************************************************

    --                   GROSS TAX RECORDS FOR COINSURERS

    -- ********************************************************************

     IF @is_coinsured_policy > 0

     BEGIN

		 -- Get next stats_detail_id and set type

			SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)

			FROM    Stats_Detail

			WHERE   stats_folder_cnt = @stats_folder_cnt


		 -- Insert the Stats Detail

                    INSERT INTO Stats_Detail (

                        stats_folder_cnt,

                        stats_detail_id,

                        stats_detail_type,

                        risk_id,

                        risk_type_id,

                        risk_type_code,

                        ri_party_cnt,

                        ri_shortname,

                        ri_party_type,

                        ri_share_percent,

                        this_premium_original,

                        this_premium_home,

                        this_premium_system,

                        currency_rate,

                        currency_code,

                        tax_type_id,

                        tax_type_code,

                        tax_value,

                        class_of_business_id,

                        class_of_business_code

                        )

                    SELECT

                        @stats_folder_cnt,

                        @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY T.ID) As ROW,

						CASE T.stats_detail_type_temp

							WHEN 'TAG' THEN 'TAC'

							WHEN 'TAN' THEN 'TAN'

						end 'out_stats_detail_type',

    T.Risk_id,

                        T.risk_type_id,

                        T.risk_type_code,

                        CASE T.stats_detail_type_temp --coins_party_cnt

							WHEN 'TAG' THEN C.party_cnt

						ELSE NULL end,

						CASE T.stats_detail_type_temp --ri_shortname

							WHEN 'TAG' THEN C.shortname

							ELSE 'NOTARI' + RTRIM(T.tax_band_code)end,

                        C.party_type_id,

                        C.share_percent,

                        CASE T.stats_detail_type_temp  --@out_tax_value_coins

							WHEN 'TAN' THEN ROUND(T.tax_value * C.share_percent / 100,@base_decimals)

							ELSE ROUND(-T.tax_value * C.share_percent / 100,@base_decimals) end,

						CASE T.stats_detail_type_temp  --@out_tax_value_coins_home

							WHEN 'TAN' THEN ROUND(T.tax_value * C.share_percent * @out_currency_rate / 100,@base_decimals)

							ELSE ROUND(-T.tax_value * C.share_percent * @out_currency_rate / 100,@base_decimals) end,

						CASE T.stats_detail_type_temp  --@out_tax_value_coins_system

							WHEN 'TAN' THEN ROUND(T.tax_value * C.share_percent * @out_currency_rate * @out_system_rate / 100,@base_decimals)

							ELSE ROUND(-T.tax_value * C.share_percent * @out_currency_rate   * @out_system_rate / 100,@base_decimals) end,


                        @out_currency_rate,

                        @out_currency_code,

                        T.tax_band_id,

                        T.tax_band_code,

                        CASE T.stats_detail_type_temp  --@out_tax_value_coins

							WHEN 'TAN' THEN T.tax_value * C.share_percent / 100

							ELSE -T.tax_value * C.share_percent / 100 end,

						CASE T.stats_detail_type_temp --class_of_business_id

							WHEN 'TAN' THEN T.class_of_business_id

						ELSE NULL end,

						CASE T.stats_detail_type_temp --class_of_business_code

						WHEN 'TAN' THEN T.class_of_business_code

						ELSE NULL end



                    FROM #TaxGross T JOIN #Coinsurance C ON 1=1

                    AND T.is_share_with_co_insurers = 1

					ORDER BY T.ID

     END
