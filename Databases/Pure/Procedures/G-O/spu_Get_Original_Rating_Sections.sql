SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Original_Rating_Sections'
GO

CREATE  PROCEDURE spu_Get_Original_Rating_Sections    
    @type TINYINT,    
    @insurancefilecnt INT,    
    @riskcnt INT,    
    @roundingsectioncode VARCHAR(10),    
    @renewalproratarate float=NULL    
AS    
    
-- @Type    
-- 1 = Rating sections for MTA defaults.    
-- 2 = Rating sections for returns.    
-- 3 = Rating sections for renewals.    
-- 4 = Rating sections for reinstatements.    
-- 5 = Rating sections for reinstatement returns.    
    
DECLARE @original_risk_cnt INT    
DECLARE @original_currency_id SMALLINT    
DECLARE @original_source_id SMALLINT    
DECLARE @original_currency_rate FLOAT    
DECLARE @original_date DATETIME    
DECLARE @new_currency_id SMALLINT    
DECLARE @new_source_id SMALLINT    
DECLARE @new_currency_rate FLOAT    
DECLARE @new_date DATETIME    
DECLARE @combined_rate FLOAT    
DECLARE @pro_rata_rate FLOAT    
DECLARE @is_mandatory_risk TINYINT    
DECLARE @frequencyId INT =3    
DECLARE @calculate_premium_on_thispremium TINYINT = 0
DECLARE @inception_date_tpi DATETIME  
DECLARE @expiry_date DATETIME
DECLARE @discount_reason_id INT = 0
DECLARE @discount_recurring_type_id INT = 0
--Get the currency and overridden rate of the MTA/Renewal    
    
SELECT    
 @new_currency_id = currency_id,    
 @new_source_id = source_id,    
 @new_currency_rate = currency_base_xrate,    
 @new_date = cover_start_date ,    
 @frequencyId = renewal_frequency_id,
 @inception_date_tpi = inception_date_tpi,
 @expiry_date = expiry_date,
 @discount_reason_id = ISNULL(discount_reason_id, 0),
 @discount_recurring_type_id = ISNULL(discount_recurring_type_id, 0)
FROM insurance_file    
WHERE insurance_file_cnt = @insurancefilecnt    
    
--get the original risk cnt if it hasn't been passed in    
IF @type = 3    
BEGIN    
 SELECT @original_risk_cnt = original_risk_cnt    
 FROM insurance_file_risk_link    
 WHERE insurance_file_cnt = @insurancefilecnt    
 AND risk_cnt = @riskcnt    
END    
ELSE    
 SELECT @original_risk_cnt = @riskcnt    
    
IF ISNULL(@original_risk_cnt,0)<>0    
BEGIN    
IF @type IN (4, 5)    
BEGIN    
 --Get the currency and overridden rate of the original transaction    
 SELECT    
  @original_currency_id = currency_id,    
  @original_source_id = source_id,    
  @original_currency_rate = currency_base_xrate,    
  @original_date = cover_start_date    
 FROM insurance_file    
 WHERE insurance_file_cnt =    
  (    
   SELECT MAX(ifi.insurance_file_cnt)    
   FROM insurance_file_risk_link ifrl,    
   insurance_file ifi    
   WHERE ifrl.risk_cnt = @original_risk_cnt    
   AND ifrl.insurance_file_cnt = ifi.insurance_file_cnt    
   AND ifi.insurance_file_status_id = 1    
   AND ifi.insurance_file_type_id = 8    
  )    
END    
ELSE    
BEGIN    
 --Get the currency and overridden rate of the original transaction    
 SELECT    
  @original_currency_id = currency_id,    
  @original_source_id = source_id,    
  @original_currency_rate = currency_base_xrate,    
  @original_date = cover_start_date    
 FROM insurance_file    
 WHERE insurance_file_cnt =    
  (    
   SELECT MAX(ifi.insurance_file_cnt)    
   FROM insurance_file_risk_link ifrl,    
   insurance_file ifi    
   WHERE ifrl.risk_cnt = @original_risk_cnt    
   AND ifrl.insurance_file_cnt = ifi.insurance_file_cnt    
   AND ISNULL(ifi.insurance_file_status_id, 3) IN (3, 4, 5, 6)    
   AND ifi.insurance_file_type_id IN (2,3,5,4,9)    
  )    
END    
END    
ELSE    
BEGIN    
SELECT    
  @original_currency_id = currency_id,    
  @original_source_id = source_id,    
  @original_currency_rate = currency_base_xrate,    
  @original_date = cover_start_date    
 FROM insurance_file    
 WHERE insurance_file_cnt = @insurancefilecnt    
END    
    
--Calculate single rate to go from old currency to new currency    
IF @new_currency_id = @original_currency_id    
 --Don't convert amounts if both policy versions are in they same currency, even if they have different rates.    
 SELECT @combined_rate = 1    
ELSE    
BEGIN    
 --If new rate wasn't overridden then get the rate from currencyrate table    
 IF ISNULL(@new_currency_rate, 0) = 0    
  EXEC spu_ACT_Get_Currency_Rate    
   @new_currency_id,    
   @new_source_id,    
   @new_date,    
   @new_currency_rate OUTPUT    
    
 --If original rate wasn't overridden then get the rate from currencyrate table    
 IF ISNULL(@original_currency_rate, 0) = 0    
  EXEC spu_ACT_Get_Currency_Rate    
   @original_currency_id,    
   @original_source_id,    
   @original_date,    
   @original_currency_rate OUTPUT    
    
 SELECT @combined_rate = @original_currency_rate / @new_currency_rate    
END    
    
-- ensure we have a valid rate    
Set @combined_rate  =ISNULL(@combined_rate, 1)    
--Get the pro_rata rate via proper calculation routine, in case we have a policy    
--where none was stored. Do not supply original risk count as we only want the    
--current pro_rata rate to be applied!!!    
    exec spu_get_pro_rata_rate    
        @insurance_file_cnt = @insurancefilecnt,    
        @risk_cnt = @riskcnt,    
        @original_risk_cnt = NULL,    
        @pro_rata_rate = @pro_rata_rate OUTPUT    
    
SET @pro_rata_rate = ISNULL(@pro_rata_rate,1)    
    
IF @pro_rata_rate = 0    
 Set @pro_rata_rate = 1    
    
  -- we need a new @type if mandatory risk and return annual premium as is    
-- because we can disable pro rata through output for few ratings. This makes pro rata rate of risk table irrelevant.    
IF @type IN (2, 5)    
BEGIN    
    Select @is_mandatory_risk = ISNULL(Is_Mandatory_Risk, 0)    
    From risk Where risk_cnt = @riskcnt    
    If @is_mandatory_risk = 1    
 Set @type = 7    
END    
    
--set pro_rata_rate and= 1 to show Actual 'This Premium' in Original Rating Section during MTC    
IF EXISTS(SELECT NULL FROM product p INNER JOIN insurance_file i ON i.product_id=p.product_id    
   WHERE ISNULL(enable_mtc_rating_rule,0)=1 AND insurance_file_type_id=12 AND insurance_file_cnt=@insurancefilecnt)    
BEGIN
	-- For short term policy
	IF(DATEDIFF(day, @inception_date_tpi, @expiry_date) < DATEDIFF(day, @inception_date_tpi, DATEADD(year, 1, @inception_date_tpi)))
			SET @calculate_premium_on_thispremium=1
	SELECT @pro_rata_rate=1,@combined_rate=1
END  
    
--Get rating section details    
SELECT    
 rs.risk_cnt,    
 rs.rating_section_id,    
 NULL Decline_reason,    
 NULL Refer_reason,    
 NULL Message,    
 NULL policy_rating_section,    
 rst.code,    
 ROUND(rs.sum_insured * @combined_rate, 2) sum_insured,    
 CASE @type    
  WHEN 1 THEN ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4)    
  WHEN 2 THEN CASE WHEN (@pro_rata_rate =1 and ISNULL(rs.is_amended,0) <> 1) and @calculate_premium_on_thispremium <> 1 AND @discount_reason_id = 0 THEN ROUND(rs.annual_premium * @combined_rate, 4) ELSE ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4) END  
  WHEN 3 THEN ROUND((rs.annual_premium *  @renewalproratarate ) * @combined_rate, 4)    
  WHEN 4 THEN -ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4)    
  WHEN 5 THEN ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4)    
  WHEN 6 THEN ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4)    
  WHEN 7 THEN ROUND(rs.annual_premium * @combined_rate, 4)    
 END premium,    
 CASE rt.code    
  WHEN 'V' THEN ROUND(rs.annual_rate * @combined_rate, 4)    
  ELSE rs.annual_rate    
 END annual_rate,    
 CASE @type
  WHEN 1 THEN CASE WHEN ISNULL(rs.is_amended,0) = 1 THEN ROUND(rs.annual_premium * @combined_rate, 4) ELSE 0 END  
  WHEN 2 THEN CASE WHEN @pro_rata_rate =1 AND @discount_reason_id = 0 THEN ROUND(rs.annual_premium * @combined_rate, 4) ELSE ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4) END
  WHEN 3 THEN ROUND((rs.annual_premium) * @combined_rate, 4)
  WHEN 5 THEN ROUND((rs.this_premium / @pro_rata_rate ) * @combined_rate, 4)    
  WHEN 7 THEN ROUND(rs.annual_premium * @combined_rate, 4)    
  ELSE 0    
 END original_premium,    
 CASE @type    
        WHEN 2 THEN 1    
        WHEN 5 THEN 1    
        WHEN 7 THEN 1    
        ELSE 0    
 END flag,    
 rs.rate_type_id,    
    rs.country_id,    
    rs.state_id,    
    rs.auto_calculated,    
    rs.earning_pattern_id,    
    0 as 'disable_original_prorata',    
    0 as 'disable_new_prorata',    
 is_amended    
FROM rating_section rs    
JOIN rating_section_type rst    
 ON rs.rating_section_type_id = rst.rating_section_type_id    
JOIN risk r    
 ON rs.risk_cnt = r.risk_cnt    
JOIN rate_type rt    
 ON rt.rate_type_id = rs.rate_type_id    
WHERE rs.risk_cnt = @riskcnt    
AND rs.original_flag = CASE @type WHEN 4 THEN 1 ELSE 0 END -- We actually want the old ones for reinstatement    
AND (rst.code <> @roundingsectioncode OR @type = 2 )--OR @type = 2    
ORDER BY rs.rating_section_id 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
