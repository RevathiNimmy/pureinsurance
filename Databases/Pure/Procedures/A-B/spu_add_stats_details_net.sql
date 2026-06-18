EXECUTE DDLDropProcedure 'spu_add_stats_details_net'
GO

CREATE PROCEDURE spu_add_stats_details_net
    @stats_folder_cnt int      ,
    @is_pt int=0
AS
DECLARE @RI2007Enabled INT,
	@out_stats_detail_type		VARCHAR(3),
	@out_ri_party_type			VARCHAR(3),
	@insurance_file_cnt			INT,
	@source_id					INT,
	@company_id					INT,
	@out_currency_id			INT,
	@out_currency_rate			numeric(19, 8),
    @out_system_rate			numeric(19, 8),
    @out_currency_code			VARCHAR(10),
    @is_coinsured_policy		TINYINT,
    @net_dri_party_cnt			INT,
    @net_dri_shortname			VARCHAR(20),
    @retained_coins_percent		FLOAT,
    @out_stats_detail_id		INT,
    @base_decimals				INT,
    @system_decimals			INT,
    @return_status				INT,
    @business_type_code   VARCHAR(10)  ,
    @effective_ri_date                      DATETIME
    declare @document_ref	varchar(25) 

DECLARE @Is_PortFolioTransfer INT
SET   @Is_PortFolioTransfer = @is_pt

Select @RI2007Enabled = isnull(value,0) From hidden_options Where option_number = 88 --RI2007Enabled

-- Set record and ri types or NET record
SELECT  @out_stats_detail_type = 'NET',
        @out_ri_party_type = 'RET'


SELECT
		@insurance_file_cnt = IFL.insurance_file_cnt,
		@company_id = IFL.source_id,
		@out_currency_id = IFL.currency_id,
		@out_currency_rate = IFL.currency_base_xrate,
		@out_system_rate = IFL.system_base_xrate,
		@out_currency_code = C.code,
		@business_type_code = BT.code
        ,@document_ref = sf.document_ref
		    
    FROM Stats_Folder SF
    JOIN Insurance_File IFL ON SF.insurance_file_cnt = IFL.insurance_file_cnt
    JOIN Currency C			ON IFL.currency_id		 = C.currency_id
    JOIN Business_Type BT   ON IFL.business_type_id = BT.business_type_id

    WHERE SF.stats_folder_cnt = @stats_folder_cnt

	Select @RI2007Enabled = ISNULL(value,0) From Hidden_options  WHERE option_number= 88

    --If this is a coinsured policy then retrieve the party_cnt for Retained.
    IF @business_type_code like 'COIN%'
    BEGIN
		SET @is_coinsured_policy = 1
        --Establish percentage retained.
        SELECT  @retained_coins_percent = ISNULL(SUM(cv.share_percent), 0) / 100
        FROM    Coi_Value cv
        JOIN    Party_Insurer pin
            ON  pin.party_cnt = cv.party_cnt
        WHERE   insurance_file_cnt = @insurance_file_cnt
        AND     pin.is_retained = 1
    END


--Establish party_cnt for Defered RI RETAINED party.
SELECT  @net_dri_party_cnt = party_cnt,
        @net_dri_shortname = RTRIM(shortname)
FROM    Party
WHERE   shortname = 'RETDEFER'


--If this is a coinsured policy then retrieve the party_cnt for Retained.
SET @retained_coins_percent = 1
IF @is_coinsured_policy > 0
BEGIN
    --Establish percentage retained.
    SELECT  @retained_coins_percent = ISNULL(SUM(cv.share_percent), 0) / 100
    FROM    Coi_Value cv
    JOIN    Party_Insurer pin
        ON  pin.party_cnt = cv.party_cnt
    WHERE   insurance_file_cnt = @insurance_file_cnt
    AND     pin.is_retained = 1
END

    if substring(LTRIM(RTRIM(@document_ref)),0,4) ='SDD'
	begin
	set @out_currency_rate = 0
	end
  
	EXEC spu_ACT_Do_Currency_Conversion
        @company_id = @company_id,
        @currency_id = @out_currency_id,
        @currency_amount_unrounded = 1,
        @mode = 'ALL',
        @currency_base_xrate = @out_currency_rate OUTPUT,
        @system_base_xrate = @out_system_rate OUTPUT,
        @Base_decimals = @Base_Decimals OUTPUT,
        @system_decimals = @system_decimals OUTPUT,
        @return_status = @return_status OUTPUT


	DECLARE @SuppressDecimalOption AS INT=112

	DECLARE @bIsSuppressDecimal As TINYINT=(Select ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption)
	IF @bIsSuppressDecimal=1 
	  SELECT  @base_decimals=0,@system_decimals=0

   IF @out_system_rate = 0 SELECT @out_system_rate=1 SELECT   @out_system_rate=1/@out_system_rate

-- Get next stats_detail_id
	SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)
	FROM    Stats_Detail
	WHERE   stats_folder_cnt = @stats_folder_cnt

CREATE TABLE #PerilsNet
(
	ID								INT IDENTITY Primary Key,
	peril_risk_id					INT,
	peril_risk_type_id				INT,
	peril_risk_type_code			VARCHAR(10),
	peril_id						INT,
	peril_description				VARCHAR(255),
	peril_type_id					INT,
	peril_type_code					VARCHAR(10),
	peril_policy_section_type_id	INT,
	peril_policy_section_type_code	VARCHAR(10),
	peril_class_of_business_id		INT,
	peril_class_of_business_code	VARCHAR(10),
	peril_annual_premium			NUMERIC(19, 4),
	peril_this_premium_original		NUMERIC(19, 4),
	peril_lead_commission_value     NUMERIC(19, 4),
	peril_sub_commission_value      NUMERIC(19, 4),
	peril_this_sum_insured			NUMERIC(19, 4),
	peril_rating_section_id			INT,
	peril_ri_band					INT,
	peril_share_with_coinsurer		TINYINT,
	peril_original_flag				INT,
	net_ri_share_percent			FLOAT,
    net_premium_percent				FLOAT,
    net_commission_percent			FLOAT,
    net_sum_insured					NUMERIC(19, 4),
    net_original_sum_insured		NUMERIC(19, 4),
    net_is_deferred					INT,
    net_treaty_id					INT,

    net_premium_value			    NUMERIC(19, 4),
    net_commission_value			NUMERIC(19, 4),
    ri_arrangement_line_id			INT,
 out_sum_insured_change   NUMERIC(19, 4),
      risk_pro_rata_rate                        FLOAT ,
      ri_pro_rata_rate                    FLOAT,
      orig_ri_arrangement_id INT
)
 --,ri_arr_id int,
 --orig_ri_arr_id int


SELECT @effective_ri_date = MAX(effective_date)
      FROM RI_Arrangement RA
      JOIN insurance_file_risk_link IFRL ON RA.risk_cnt = IFRL.risk_cnt
      WHERE IFRL.insurance_file_cnt = @insurance_file_cnt

IF @Is_PortFolioTransfer = 1
BEGIN
;
WITH RICTE(risk_cnt,ri_arrangement_id,original_flag,ri_band_id,ri_model_id ,pro_rata_rate )
AS
(SELECT  RA.risk_cnt, RA.RI_arrangement_id, RA.original_flag, RA.ri_band_id, RA.ri_model_id,isnull(ra.pro_rata_rate,1) From RI_Arrangement RA
JOIN insurance_file_risk_link IFRL ON
RA.risk_cnt = IFRL.risk_cnt
Where IFRL.insurance_file_cnt = @insurance_file_cnt
AND  RA.effective_date = @effective_ri_date)

INSERT INTO #PerilsNet
 (
   peril_risk_id,
   peril_risk_type_id,
   peril_risk_type_code,
   peril_id,
   peril_description,
   peril_type_id,
   peril_type_code,
   peril_policy_section_type_id,
   peril_policy_section_type_code,
   peril_class_of_business_id,
   peril_class_of_business_code,
   peril_annual_premium,
   peril_this_premium_original,
   peril_lead_commission_value,
   peril_sub_commission_value,
   peril_this_sum_insured,
   peril_rating_section_id,
   peril_ri_band,
   peril_share_with_coinsurer,
   peril_original_flag,

   net_ri_share_percent,
   net_premium_percent,
   net_commission_percent,
   --net_sum_insured,
   net_original_sum_insured,
   net_is_deferred,
   net_treaty_id,
   ri_arrangement_line_id,
   net_premium_value,
   net_sum_insured,
   net_commission_value, risk_pro_rata_rate, ri_pro_rata_rate, orig_ri_arrangement_id
   --,ri_arr_id,orig_ri_arr_id
 )
 SELECT
   P.risk_cnt,
            R.risk_type_id,
            RT.code,
            P.peril_id,
            P.description,
            P.peril_type_id,
            PT.code,
            RS.policy_section_type_id,
            PS.code,
            P.class_of_business_id,
            CB.code,
            P.annual_premium,
            P.this_premium,
            P.lead_commission_value,
            P.sub_commission_value,
            P.sum_insured,
            P.rating_section_id,
            P.ri_band,
            RT.is_share_with_co_insurers,
            RS.original_flag,

            RAL.this_share_percent,
            RAL.premium_percent,
            RAL.commission_percent,
            --RAL.sum_insured,
            (SELECT r2.sum_insured
                FROM   RI_Arrangement_Line r2
                WHERE  r2.treaty_id = RAL.treaty_id
                AND    r2.ri_arrangement_id = RI2.ri_arrangement_id),

   0 is_deferred,

            RAL.treaty_id,
            RAL.ri_arrangement_line_id,

            P.this_premium * RAL.premium_percent / 100, --net_premium_value
            P.sum_insured * RAL.premium_percent / 100, --net_sum_insured
            (P.this_premium * RAL.premium_percent / 100)* RAL.commission_percent / 100,
            r.pro_rata_rate,
             RI.pro_rata_rate,RAL.ri_arrangement_id

            --,RI.ri_arrangement_id, RI2.ri_arrangement_id
FROM      Insurance_File_Risk_Link IFR
JOIN      Peril P
 ON IFR.risk_cnt = P.risk_cnt
JOIN      Peril_Type PT
 ON P.peril_type_id = PT.peril_type_id
JOIN      Rating_Section RS
 ON P.rating_section_id = RS.rating_section_id
    AND P.Risk_cnt = RS.Risk_cnt
JOIN      Class_Of_Business CB
 ON P.class_of_business_id = CB.class_of_business_id
JOIN      Risk R
 ON P.risk_cnt = R.risk_cnt
JOIN      Risk_Type RT
 ON R.risk_type_id = RT.risk_type_id
LEFT JOIN Policy_Section_Type PS
 ON RS.policy_section_type_id = PS.policy_section_type_id

JOIN RICTE RI ON RI.original_flag = RS.original_flag AND RI.ri_band_id = P.ri_band and ri.risk_cnt = P.risk_cnt
LEFT JOIN RICTE RI2 ON RI2.original_flag <> RS.original_flag AND RI2.ri_band_id = P.ri_band and RI2.risk_cnt = P.risk_cnt

JOIN      RI_Arrangement_line RAL
 ON (RI.ri_arrangement_id = RAL.ri_arrangement_id  OR (RI2.ri_arrangement_id = RAL.ri_arrangement_id ))

  
WHERE     IFR.insurance_file_cnt = @insurance_file_cnt
    AND ( IFR.original_risk_cnt IS NULL
     OR ( IFR.original_risk_cnt IS NOT NULL
       AND ISNULL (IFR.is_risk_edited, 0) = 1 )
     OR ( IFR.status_flag = 'C'
       AND IFR.is_manually_changed IS NOT NULL ) )
    AND ( P.is_premium = 1 -- Only select perils which are 'FAP' or 'SI'
     OR P.is_sum_insured = 1 )
     AND  ISNULL(P.this_premium, 0) != 0
     AND     (RS.original_flag=0  )
    AND RAL.TYPE = 'R'

ORDER     BY P.rating_section_id ASC

 END

IF @Is_PortFolioTransfer = 0
BEGIN
;
WITH RICTE(risk_cnt,ri_arrangement_id,original_flag,ri_band_id,ri_model_id )
AS
(SELECT  RA.risk_cnt, RA.RI_arrangement_id, RA.original_flag, RA.ri_band_id, RA.ri_model_id From RI_Arrangement RA
JOIN insurance_file_risk_link IFRL ON
RA.risk_cnt = IFRL.risk_cnt
Where IFRL.insurance_file_cnt = @insurance_file_cnt
AND version_id = 1 )

	INSERT INTO #PerilsNet
	(
			peril_risk_id,
			peril_risk_type_id,
			peril_risk_type_code,
			peril_id,
			peril_description,
			peril_type_id,
			peril_type_code,
			peril_policy_section_type_id,
			peril_policy_section_type_code,
			peril_class_of_business_id,
			peril_class_of_business_code,
			peril_annual_premium,
			peril_this_premium_original,
			peril_lead_commission_value,
			peril_sub_commission_value,
			peril_this_sum_insured,
			peril_rating_section_id,
			peril_ri_band,
			peril_share_with_coinsurer,
			peril_original_flag,

			net_ri_share_percent,
			net_premium_percent,
			net_commission_percent,
			--net_sum_insured,
			net_original_sum_insured,
			net_is_deferred,
			net_treaty_id,
			ri_arrangement_line_id,
			net_premium_value,
			net_sum_insured,
			net_commission_value
	)
	SELECT
			P.risk_cnt,
            R.risk_type_id,
            RT.code,
            P.peril_id,
            P.description,
            P.peril_type_id,
            PT.code,
            RS.policy_section_type_id,
            PS.code,
            P.class_of_business_id,
            CB.code,
            P.annual_premium,
            P.this_premium,
            P.lead_commission_value,
            P.sub_commission_value,
            P.sum_insured,
            P.rating_section_id,
            P.ri_band,
            RT.is_share_with_co_insurers,
            RS.original_flag,

            RAL.this_share_percent,
            RAL.premium_percent,
            RAL.commission_percent,
            --RAL.sum_insured,
            (SELECT r2.sum_insured
                FROM   RI_Arrangement_Line r2
                WHERE  r2.treaty_id = RAL.treaty_id
                AND    r2.ri_arrangement_id = RI2.ri_arrangement_id),

			Case When rm.ri_model_type = 2 Then 1 Else 0 End is_deferred,

            RAL.treaty_id,
            RAL.ri_arrangement_line_id,

            P.this_premium * RAL.premium_percent / 100, --net_premium_value
            P.sum_insured * RAL.premium_percent / 100, --net_sum_insured
            (P.this_premium * RAL.premium_percent / 100)* RAL.commission_percent / 100
FROM      Insurance_File_Risk_Link IFR
JOIN      Peril P
	ON IFR.risk_cnt = P.risk_cnt
JOIN      Peril_Type PT
	ON P.peril_type_id = PT.peril_type_id
JOIN      Rating_Section RS
	ON P.rating_section_id = RS.rating_section_id
	   AND P.Risk_cnt = RS.Risk_cnt
JOIN      Class_Of_Business CB
	ON P.class_of_business_id = CB.class_of_business_id
JOIN      Risk R
	ON P.risk_cnt = R.risk_cnt
JOIN      Risk_Type RT
	ON R.risk_type_id = RT.risk_type_id
LEFT JOIN Policy_Section_Type PS
	ON RS.policy_section_type_id = PS.policy_section_type_id

JOIN RICTE RI ON RI.original_flag = RS.original_flag AND RI.ri_band_id = P.ri_band and ri.risk_cnt = P.risk_cnt
LEFT JOIN RICTE RI2 ON RI2.original_flag <> RS.original_flag AND RI2.ri_band_id = P.ri_band and RI2.risk_cnt = P.risk_cnt

JOIN      ri_model rm
	ON rm.ri_model_id = RI.ri_model_id
JOIN      RI_Arrangement_line RAL
	ON RI.ri_arrangement_id = RAL.ri_arrangement_id


WHERE     IFR.insurance_file_cnt = @insurance_file_cnt
		  AND IFR.status_flag NOT IN ('U','R')
		  AND ( IFR.original_risk_cnt IS NULL
				 OR ( IFR.original_risk_cnt IS NOT NULL
					  AND ISNULL (IFR.is_risk_edited, 0) = 1 )
				 OR ( IFR.status_flag = 'C'
					  AND IFR.is_manually_changed IS NOT NULL ) )
		  AND ( P.is_premium = 1 -- Only select perils which are 'FAP' or 'SI'
				 OR P.is_sum_insured = 1 )
		  AND RAL.TYPE = 'R'

		AND ( (P.this_premium * RAL.premium_percent) <> 0 OR (P.sum_insured * RAL.premium_percent ) <>0 OR  ((P.this_premium * RAL.premium_percent)* RAL.commission_percent ) <> 0)
ORDER     BY P.rating_section_id ASC

 END

IF @RI2007Enabled = 0
BEGIN
	Update P SET P.net_commission_value = 0
	FROM #PerilsNet P
	WHERE (Select sum(Round(premium, 2)) From ri_arrangement Where risk_cnt = P.peril_risk_id Group By risk_cnt) = 0
END

	
 -- Calculate the sum_insured_change figure
 Update P SET P.out_sum_insured_change = net_sum_insured + ISNULL(P.net_original_sum_insured, 0)
 FROM #PerilsNet P

 
 IF @Is_PortFolioTransfer =1
                   BEGIN

    UPDATE P SET  P.net_premium_value  = -P.net_premium_value, P.net_commission_value  = -P.net_commission_value
                FROM #PerilsNet P
                JOIN RI_Arrangement RAL2 ON RAL2.ri_arrangement_id = P.orig_ri_arrangement_id
                WHERE RAL2.original_flag=1

    UPDATE P SET  P.net_premium_value  = P.net_premium_value * P.ri_pro_rata_rate / P.risk_pro_rata_rate,
                P.net_commission_value  = P.net_commission_value* P.ri_pro_rata_rate / P.risk_pro_rata_rate
    FROM #PerilsNet P
 END

-- Insert the Stats Detail
    	INSERT INTO Stats_Detail
			(
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

				ri_party_cnt,
				ri_shortname,
				ri_party_type,
				ri_share_percent,
				annual_premium,
				currency_code,
				currency_rate,
				this_premium_original,
				this_premium_home,
				this_premium_system,
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
			@out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY P.ID ),
			@out_stats_detail_type,
			P.peril_risk_id,
			P.peril_risk_type_id,
			P.peril_risk_type_code,
			P.peril_id,
			P.peril_DESCRIPTION,
			P.peril_type_id,
			P.peril_type_code,
			P.peril_policy_section_type_id,
			P.peril_policy_section_type_code,
			P.peril_class_of_business_id,
			P.peril_class_of_business_code,


CASE WHEN P.net_is_deferred = 1 THEN @net_dri_party_cnt
	 ELSE (SELECT  TOP 1 tp.party_cnt
            FROM    treaty_party tp
            JOIN    party_insurer pin ON pin.party_cnt = tp.party_cnt
            WHERE   tp.treaty_id = P.net_treaty_id AND     pin.is_retained = 1) end,

CASE WHEN P.net_is_deferred = 1 THEN @net_dri_shortname
	 ELSE (SELECT  TOP 1 RTRIM(p1.shortname)
            FROM    treaty_party tp
            JOIN    party p1 ON p1.party_cnt = tp.party_cnt
            JOIN    party_insurer pin ON pin.party_cnt = p1.party_cnt
            WHERE   tp.treaty_id = P.net_treaty_id AND     pin.is_retained = 1) end,

		@out_ri_party_type,
		P.net_ri_share_percent,
		-ROUND(P.peril_annual_premium,@base_decimals),
		@out_currency_code,
		@out_currency_rate,

		-ROUND(P.net_premium_value * @retained_coins_percent,@base_decimals) ,--Net_premium_value
		- ROUND(P.net_premium_value * @retained_coins_percent * @out_currency_rate  , @base_decimals), ---@out_this_premium_home
		- ROUND(P.net_premium_value * @retained_coins_percent * @out_currency_rate * @out_system_rate , @system_decimals), ---@out_this_premium_system
		P.net_commission_percent,
		- ROUND(P.peril_lead_commission_value * @out_currency_rate, @base_decimals),
		- ROUND(P.peril_lead_commission_value * @out_currency_rate * @out_system_rate, @system_decimals),
		- ROUND(P.peril_sub_commission_value * @out_currency_rate, @base_decimals),
		- ROUND(P.peril_sub_commission_value * @out_currency_rate * @out_system_rate, @system_decimals),
		- ROUND(P.net_sum_insured * @retained_coins_percent * @out_currency_rate,@base_decimals),
		- ROUND(P.net_sum_insured  * @retained_coins_percent * @out_currency_rate * @out_system_rate,@system_decimals),
		@out_currency_code,
		- ROUND(P.out_sum_insured_change * @out_currency_rate,@base_decimals)

		
FROM #PerilsNet P
WHERE ((P.net_premium_value <> 0) OR (P.net_sum_insured <> 0) OR (P.net_commission_value <> 0 ))
ORDER BY P.ID


