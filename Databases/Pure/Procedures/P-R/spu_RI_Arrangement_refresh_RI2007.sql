SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_Arrangement_refresh_RI2007'
GO

CREATE PROCEDURE spu_ri_arrangement_refresh_ri2007  
 @insurance_file_cnt  INTEGER,  
 @risk_cnt INTEGER,  
 @Trans_type VARCHAR(5) = '',  
 @TMPRisk_cnt_under_renewal INTEGER = NULL,  
 @copy_fac_risk_cnt INTEGER = NULL,  
 @version_id INT = 1,  
 @transferdate datetime = NULL  
AS  
  DECLARE @effective_date                             DATETIME,  
  @is_coinsured                               TINYINT,  
  @retained_percent                           FLOAT,  
  @source_id                                  INT,  
  @policy_currency_id                         SMALLINT,  
  @policy_currency_rate                       FLOAT,  
  @is_auto_reinsured                          TINYINT,  
  @risk_type_id                               INT,  
  @eml_percent                                FLOAT,  
  @allow_deferred                             TINYINT,  
  @line_limit                                 MONEY,  
  @ri_band                                    INT,  
  @premium                                    MONEY,  
  @sum_insured                                MONEY,  
  @ri_arrangement_id                          INT,  
  @ri_arrangement_line_id                     INT,  
  @is_modified                                TINYINT,  
  @Date_for_Treaty_XOL_Calculation            INT,  
  @Transaction_type_id                        INT,  
  @RI_sum_insured                             FLOAT,  
  @RI_premium                                 FLOAT,  
  @RIDetails_Changed                          INT,  
  @Original_Risk_cnt                          INTEGER,  
  @Recalculate_prorata_reinsurance_during_MTA TINYINT,  
  @treaty_id                                  INT,  
  @party_cnt                                  INT,  
  @default_share_percent                      FLOAT,  
  @obligatory_sum_insured                     MONEY,--PN 71440  
  @NBExtended_Is_Enabled                      TINYINT,  
  @prop_effective_date                       DATETIME,  
  @Date_for_Prop_Calculation      INT,  
  @Extended_Limits_Enabled TINYINT  ,  
  @original_flag INT  ,  
  @is_ON_Accounting_Year INT,  
  @new_copy_fac_risk_cnt INT,  
  @original_ri_arrangement_id INT,  
  @original_sum_insured MONEY,  
  @original_premium MONEY,  
  @original_ri_model_id INT,  
  @Original_Extended_Limits_Amount MONEY,  
  @Original_Extended_Limits_Enabled TINYINT,  
  @RI_Effective_Date DATETIME,  
  @RI_Version_Type_id INT = 1,  
  @Limit_Effective_Date DATETIME ,  
  @pro_rata_rate Float,  
  @old_pro_rata_rate Float,  
  @temp_prop_Effective_Date DATETIME ,  
  @temp_XOL_Effective_Date DATETIME ,  
  @is_PT_DRI_Amended BIT,  
  @Extended_Limits_EnabledSystemOption TINYINT,
  @is_true_monthly_product tinyint,
  @use_anniversary_renewal_date tinyint,
  @anniversary_renewal_date datetime,
  @cover_start_date_ForRi datetime   


SET @new_copy_fac_risk_cnt =@copy_fac_risk_cnt  

--Run off Treaty
SELECT @is_true_monthly_product=p.is_true_monthly_policy,
		@anniversary_renewal_date=anniversary_date,
		@cover_start_date_ForRi = inception_date_tpi 
from Insurance_File ifi join Product p
on ifi.product_id=p.product_id
WHERE insurance_file_cnt=@insurance_file_cnt

  -- Don't use the supplied effective date.  
  -- Note: For an MTA this call may return an MTA date or today's date  
  -- depending on the system option "Use MTA date for reinsurance"  
  EXECUTE  Spu_get_effective_date  
    @insurance_file_cnt = @insurance_file_cnt,  
    @risk_cnt = @risk_cnt,  
    @effective_date = @effective_date OUTPUT,  
    @prop_effective_date = @prop_effective_date OUTPUT,
	@transfer_date=@transferdate	
  
 SELECT @Extended_Limits_EnabledSystemOption = Isnull(VALUE, 0)  
    FROM   system_options  
    WHERE  option_number = 5260  
           AND branch_id = 1  
  
 If @TransferDate IS NULL  
    SET @TransferDate = CONVERT(date,getdate())  
    SET @Limit_Effective_Date = @effective_date  
  Declare @option INT  
 -- Check "Use MTA Date for Reinsurance"  
  SELECT @option = CONVERT(INT, value)  
  FROM system_options  
  WHERE branch_id = 1  
  AND option_number = 1023  
  
  IF @Trans_type IN ('DRI','PT') AND @version_id=1  
  BEGIN  
 SELECT @version_id = MAX(version_id) from RI_Arrangement where risk_cnt=@risk_cnt and original_flag=0  
 IF @version_id>1  
 BEGIN  
 SET @is_PT_DRI_Amended = 1  
    SELECT @pro_rata_rate=pro_rata_rate from RI_Arrangement where risk_cnt=@risk_cnt and original_flag=0  and  version_id = @version_id  
    END  
    ELSE  
    SET @version_id=1  
  END  
  
   IF @Trans_type = 'NB'  
      OR @Trans_type = 'REN'  
      SELECT @option =0  
  --SET @prop_effective_date=@effective_date  
  -- Check coinsurance and rate  
  EXECUTE Spu_insurance_file_is_coinsured  
    @insurance_file_cnt,  
    @is_coinsured OUTPUT,  
    @retained_percent OUTPUT  
  
  -- Get currency of policy, and therefore the currency of new ri_arrangement  
  SELECT @policy_currency_id = currency_id,  
         @policy_currency_rate = currency_base_xrate,  
         @source_id = source_id  
  FROM   insurance_file  
  WHERE  insurance_file_cnt = @insurance_file_cnt  
  
  -- If policy rate wasn't overridden then get the rate from currencyrate table  
  IF Isnull(@policy_currency_rate, 0) = 0  
    EXECUTE Spu_act_get_currency_rate  
      @policy_currency_id,  
      @source_id,  
      @effective_date,  
      @policy_currency_rate OUTPUT  
  
  -- Get config values from risk and risk_type  
  SELECT @is_auto_reinsured = rt.is_auto_reinsured,  
         @risk_type_id = rt.risk_type_id,  
         @eml_percent = Isnull(r.eml_percentage, 100) / 100,  
         @allow_deferred = rt.is_deferred_ri_permitted  
  FROM   risk r  
         JOIN risk_type rt  
           ON r.risk_type_id = rt.risk_type_id  
  WHERE  r.risk_cnt = @risk_cnt  
  
  -- Copy any original reinsurance  
  EXECUTE Spu_ri_arrangement_copy  
    @insurance_file_cnt = @insurance_file_cnt,  
    @risk_cnt = @risk_cnt,  
    @effective_date = @effective_date,  
    @Trans_type = @Trans_type,  
    @version_id  = @version_id,  
    @RI_Effective_Date = @TransferDate  
  
  IF @Trans_type = 'NB'  
      OR @Trans_type = 'REN'  
    SELECT @Extended_Limits_Enabled = Isnull(VALUE, 0)  
    FROM   system_options  
    WHERE  option_number = 5260  
           AND branch_id = 1  
  
   IF @version_id >1  
     BEGIN  
       SELECT @RI_Effective_Date = @transferdate,@RI_Version_Type_id = 2  
       SET @Limit_Effective_Date = @transferdate  
     END  
   ELSE  
     BEGIN  
       SELECT @RI_Effective_Date = @effective_date  
     END  
  
  -- Declare active ri_band cursor and get premiums  
  DECLARE ri_band_cursor CURSOR FAST_FORWARD FOR  
    SELECT p.ri_band,  
           sum_insured = Isnull((SELECT SUM(rs2.sum_insured)  
                                 FROM   rating_section rs2  
                                 WHERE  rs2.risk_cnt = @risk_cnt  
                                        AND rs2.rating_section_id IN  
                                            (SELECT  
                                            rating_section_id  
                                                                      FROM  
                                            peril  
                                            p2  
                                                                      WHERE  
                                            p2.risk_cnt = @risk_cnt  
                                            AND p2.is_sum_insured = 1  
                                            AND p2.ri_band = p.ri_band)  
                                        AND rs2.original_flag = rs.original_flag), 0),  
           premium = Isnull(SUM(CASE  
                                  WHEN p.is_premium = 1 THEN p.this_premium  
                                  ELSE 0  
                                END), 0),rs.original_flag  
    FROM   peril p WITH (nolock)  
           JOIN rating_section rs WITH (nolock)  
             ON rs.risk_cnt = p.risk_cnt  
                AND rs.rating_section_id = p.rating_section_id  
           LEFT JOIN peril_type pt  
             ON p.peril_type_id = pt.peril_type_id  
    -- PN:61917 : Added By Upendra : The Premimum should not reflect on RI-Insurance Screen when the Levy Tax is selected.  
    WHERE  p.risk_cnt = @risk_cnt  
           AND ( p.is_premium = 1  
                  OR p.is_sum_insured = 1 )  
           AND Isnull(pt.is_levy_tax, 0) = 0  
           AND (rs.original_flag = 0 or @version_id=1)  
           -- PN:61917 : Added By Upendra : The Premimum should not reflect on RI-Insurance Screen when the Levy Tax is selected.  
    GROUP  BY p.ri_band,rs.original_flag  
  
  -- Open the RI Bands Cursor and get first row  
  OPEN ri_band_cursor  
  
  FETCH NEXT FROM ri_band_cursor INTO @ri_band, @sum_insured, @premium ,@original_flag  
  
  -- Start processing  
  WHILE ( @@FETCH_STATUS = 0 )  
    BEGIN  
        -- Apply the EML Percentage and Coinsurance  
        SELECT @sum_insured = @sum_insured * @retained_percent * @eml_percent,  
               @premium = @premium * @retained_percent  
  
        -- Check for existing arrangement  
        SELECT @ri_arrangement_id = NULL , @temp_prop_Effective_Date = @prop_effective_date  ,@temp_XOL_Effective_Date = @effective_date  
  
        SELECT @ri_arrangement_id = ri_arrangement_id,  
               @is_modified = Isnull(is_modified, 1),  
               @Date_for_Treaty_XOL_Calculation = xol_calc_method_id,  
               @Date_for_Prop_Calculation= prop_calc_method_id  
        FROM   ri_arrangement  
        WHERE  risk_cnt = @risk_cnt  
               AND ri_band_id = @ri_band  
               AND original_flag = @original_flag  
               AND version_id = @version_id  
  
        -- Execute the SP to update premium percent in accordance to  
        -- Premium distributed  
        EXEC Spu_ri_arrangement_line_premiumpercent_upd @ri_arrangement_id=  
        @ri_arrangement_id  
  
        -- Either insert or update our arrangement  
        SET @RIDetails_Changed=0

        -- MTC: zero F/FX premium_value before calc so grossnet_premium is computed correctly
        IF @Trans_type = 'MTC' AND @original_flag = 0
        BEGIN
            UPDATE ri_arrangement_line
            SET    premium_value    = 0,
                   premium_tax      = 0,
                   commission_value = 0,
                   commission_tax   = 0
            FROM   ri_arrangement_line ral
            JOIN   ri_arrangement ra ON ra.ri_arrangement_id = ral.ri_arrangement_id
            WHERE  ral.ri_arrangement_id = @ri_arrangement_id
            AND    (ISNULL(ral.manually_added, 0) = 1 OR ral.type IN ('F', 'FX'))
        END
  
         SELECT TOP 1  @Date_for_Treaty_XOL_Calculation = ISNULL(@Date_for_Treaty_XOL_Calculation,date_for_treaty_xol_calculation_id),                
		 @Date_for_Prop_Calculation= ISNULL(@Date_for_Prop_Calculation,Proportional_RI_Cal_Method) ,               
		@use_anniversary_renewal_date = ISNULL(use_anniversary_date_for_TMP,0)                
		FROM   RI_Band_Version                
		WHERE  ri_band_id = @ri_band              
		AND CONVERT(DATE, effective_date, 23)  <= CONVERT(DATE, @cover_start_date_ForRi, 23)          
		ORDER BY effective_date DESC

		--Run off Treaty
		IF @is_true_monthly_product=1 AND @use_anniversary_renewal_date=1 AND (@Date_for_Prop_Calculation=1 or @Date_for_Treaty_XOL_Calculation=1) AND @version_id=1 AND @option<>1
		BEGIN		
			SELECT @effective_date=min(inception_date) FROM risk
			WHERE risk_folder_cnt=(select risk_folder_cnt from risk where risk_cnt=@risk_cnt)

			SELECT @prop_effective_date=@effective_date

			if @effective_date<Dateadd(year,-1,@anniversary_renewal_date)
				BEGIN
					SELECT @effective_date=Dateadd(year,-1,@anniversary_renewal_date), 
						   @prop_effective_date=Dateadd(year,-1,@anniversary_renewal_date)
				END
			
			SELECT @temp_prop_Effective_Date = @prop_effective_date  ,@temp_XOL_Effective_Date = @effective_date
			
		END
  
        IF @Date_for_Prop_Calculation = 2 AND @option<>1  
        BEGIN  
        SET @is_ON_Accounting_Year =1  
        SELECT @temp_prop_Effective_Date = CASE WHEN cover_start_date> @TransferDate  THEN cover_start_date ELSE @TransferDate  END  
        FROM   insurance_file  
        WHERE  insurance_file_cnt = @insurance_file_cnt  
 IF @original_flag<>1  
    BEGIN  
         SET @Limit_Effective_Date = @temp_prop_Effective_Date  
    END  
 END  
  -- Check for ri_limits from DM  
  -- Note: This call returns an single row of Null  
  IF @is_auto_reinsured = 1  
  BEGIN  
  If @original_flag =1  
   BEGIN  
    SELECT @Original_Risk_cnt  = original_risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt =@insurance_file_cnt AND risk_cnt =@risk_cnt  
    IF @Extended_Limits_EnabledSystemOption=1  
     BEGIN  
      EXECUTE Spu_get_ri_values  
      @insurance_file_cnt = @insurance_file_cnt,  
      @risk_cnt = @Original_Risk_cnt,  
      @effective_date = @Limit_Effective_Date,  
      @value = @line_limit OUTPUT  
     END  
    IF @Trans_type ='DRI' AND EXISTS(select null from RI_Arrangement where ri_band_id =@ri_band and  
             risk_cnt =@risk_cnt and version_id =@version_id  
              group by ri_model_id having COUNT(ri_arrangement_id) > 1 )  
     SET @is_ON_Accounting_Year = 1  
   END  
  ELSE  
   IF @Extended_Limits_EnabledSystemOption=1  
    BEGIN  
     EXECUTE Spu_get_ri_values  
     @insurance_file_cnt = @insurance_file_cnt,  
     @risk_cnt = @risk_cnt,  
     @effective_date = @Limit_Effective_Date,  
     @value = @line_limit OUTPUT  
    END  
         END  
  
        IF Isnull(@ri_arrangement_id, 0) > 0  
          BEGIN  
              -- Update totals on current arrangement  
              SELECT @RI_sum_insured = sum_insured,  
                     @RI_premium = premium  
              FROM   ri_arrangement  
              WHERE ri_arrangement_id = @ri_arrangement_id  
  
              IF @original_flag = 1  
              SET @sum_insured = @RI_sum_insured  
  
              IF @is_modified = 1 AND @original_flag <>1  
               BEGIN  
                UPDATE ri_arrangement  
                SET    extended_limit_amount = @line_limit  
                WHERE  ri_arrangement_id = @ri_arrangement_id  
                IF @Extended_Limits_Enabled=1 and ISNULL(@line_limit,0)>0  and @original_flag <> 1  
     Update RI_Arrangement_Line set line_limit=ISNULL(@line_limit,0) where type in ('TFS','T','R')   and ri_arrangement_id = @ri_arrangement_id  
      END  
              IF @RI_sum_insured <> @sum_insured  
                  OR @RI_premium <> @premium OR @Trans_type='DRI'  
                SET @RIDetails_Changed=1  
       IF @original_flag <>1  
              UPDATE ri_arrangement  
              SET    sum_insured = Round(@sum_insured,2),  
                     premium = @premium  
              WHERE  ri_arrangement_id = @ri_arrangement_id  
          END  
        ELSE  
     BEGIN  
              -- Insert new arrangement  
              INSERT INTO ri_arrangement  
                          (risk_cnt,  
                           ri_band_id,  
                           sum_insured,  
                           premium,  
                           original_flag,  
       is_modified,  
                           version_id,  
                           RI_Version_Type_id,  
                           Effective_Date)  
              VALUES      (@risk_cnt,  
                           @ri_band,  
                           Round(@sum_insured,2),  
                           @premium,  
                           @original_flag,  
                           1^@is_auto_reinsured,  
                           @version_id,  
                           @RI_Version_Type_id,  
                           @RI_Effective_Date)  
  
              -- Get new id and assume we have not modified  
              SELECT @ri_arrangement_id = @@IDENTITY,  
                     @is_modified = 0  
          END  
  
        IF @Trans_type = 'MTA'  
            OR @Trans_type = 'MTC'  
            OR @Trans_type = 'MTR'  
            OR @Trans_type = 'MTCR'  
 OR @Trans_type IN ('PT','DRI')  
          BEGIN  
             If Exists( SELECT Null  
              FROM   insurance_file ifl1  
                     JOIN insurance_file ifl2  
                       ON ifl1.insurance_folder_cnt = ifl2.insurance_folder_cnt  
                     JOIN insurance_file_risk_link ifrl  
                       ON ifl1.insurance_file_cnt = ifrl.insurance_file_cnt  
                     JOIN ri_arrangement ra  
                       ON ifrl.risk_cnt = ra.risk_cnt  
              WHERE  ifl2.insurance_file_cnt = @insurance_file_cnt  
                     AND ifl1.insurance_file_type_id = 2  
                     AND ra.is_extended_limit_applied =1 )  
               SELECT @NBExtended_Is_Enabled = 1  
             Else  
                SELECT @NBExtended_Is_Enabled = 0  
          END  
  
        IF @Date_for_Treaty_XOL_Calculation = 2 AND @option<>1 --AND @Trans_type NOT IN ('PT')  
          BEGIN  
              SELECT @temp_XOL_Effective_Date = @TransferDate  
          FROM   insurance_file  
          WHERE  insurance_file_cnt = @insurance_file_cnt  
          END  
        ELSE IF @Date_for_Treaty_XOL_Calculation = 3 AND @option<>1 --AND  @Trans_type <> 'PT'  
         BEGIN  
              SET @is_ON_Accounting_Year =1  
              SELECT @temp_XOL_Effective_Date = CASE WHEN cover_start_date>@TransferDate  THEN cover_start_date ELSE @TransferDate END  
          FROM   insurance_file  
          WHERE  insurance_file_cnt = @insurance_file_cnt  
         END  
  
 
   
        -- Check if the band has been modified  
        IF ( @is_modified = 0 ) OR @Trans_type='DRI'  
          BEGIN  
              IF @Trans_type = 'REN'  
                 AND @TMPRisk_cnt_under_renewal IS NOT NULL  
                BEGIN  
                    EXECUTE Spu_ri_arrangement_copy_tmpfac  
                      @TMPRisk_cnt_under_renewal,  
                      @ri_arrangement_id  
                END  
		
  IF (@new_copy_fac_risk_cnt IS NOT NULL)  
     BEGIN  
   EXECUTE Spu_ri_arrangement_copy_tmpfac  
                      @new_copy_fac_risk_cnt,  
                      @ri_arrangement_id  
                  END  
  
              -- It hasn't, or it's new so refresh it  
              EXECUTE Spu_ri_arrangement_make_ri2007  
                @ri_arrangement_id = @ri_arrangement_id,  
                @risk_type_id = @risk_type_id,  
                @ri_band_id = @ri_band,  
                @effective_date = @temp_XOL_Effective_Date,  
                @allow_deferred = @allow_deferred,  
                @sum_insured = @sum_insured,  
                @premium = @premium,  
                @line_limit = @line_limit,  
                @is_auto_reinsured = @is_auto_reinsured,  
                @source_id = @source_id,  
                @policy_currency_id = @policy_currency_id,  
                @policy_currency_rate = @policy_currency_rate,  
                @Trans_type =@Trans_type,  
                @NBExtended_Is_Enabled = @NBExtended_Is_Enabled,  
                @prop_effective_date=@temp_prop_Effective_Date,  
    @Original_Risk_Cnt = @Original_Risk_cnt  
           END  
        ELSE  
          IF @RIDetails_Changed = 1  
            BEGIN  
                DECLARE @RI_Model_id INT  
  
                EXECUTE Spu_getrimodel  
                  @Effective_Date = @temp_XOL_Effective_Date,  
                  @risk_type_id = @risk_type_id,  
                  @ri_band_id =@ri_band,  
                  @allow_deferred = @allow_deferred,  
                  @RI_Model_id =@RI_Model_id OUTPUT  
  
                EXECUTE Spu_ri_arrangement_calc_ri2007  
                  @ri_arrangement_id = @ri_arrangement_id,  
                  @band_si = @sum_insured,  
                  @band_premium = @premium,  
                  @ri_model_id = @ri_model_id,  
                  @Trans_type = @Trans_type  
            END  
  
IF @version_id >1  
BEGIN  
SELECT @original_ri_arrangement_id = RI_Arrangement_id,@original_sum_insured = sum_insured ,  
  @original_premium = premium ,@original_ri_model_id = ri_model_id,  
  @Original_Extended_Limits_Enabled = Is_extended_limit_applied  ,  
  @Original_Extended_Limits_Amount = Extended_limit_amount  FROM RI_Arrangement WHERE risk_cnt =@risk_cnt AND version_id =@version_id AND original_flag =1 AND ri_band_id = @ri_band  
  
-- PBI 35359: Restore is_edited on obligatory lines in original snapshot BEFORE calc runs
-- so calc preserves edited obligatory SI/premium and FAC calculates from correct gross-net base
IF ISNULL(@Original_Risk_cnt, 0) > 0 AND @Trans_type NOT IN ('REN', 'PT')
BEGIN
    UPDATE snap_line
    SET    snap_line.sum_insured   = -ABS(nb_line.sum_insured),
           snap_line.premium_value = -ABS(nb_line.premium_value),
           snap_line.is_edited     = 1
    FROM   ri_arrangement_line snap_line
    JOIN   ri_arrangement_line nb_line
                               ON nb_line.ri_arrangement_id IN (
                                   SELECT ri_arrangement_id FROM ri_arrangement
                                   WHERE risk_cnt = @Original_Risk_cnt AND original_flag = 0 AND ri_band_id = @ri_band)
                              AND nb_line.treaty_id         = snap_line.treaty_id
                              AND nb_line.type              = snap_line.type
                              AND ISNULL(nb_line.is_edited, 0) = 1
    WHERE  snap_line.ri_arrangement_id = @original_ri_arrangement_id
    AND    snap_line.type NOT IN ('F', 'FX')
END

EXECUTE  Spu_ri_arrangement_calc_ri2007  
                  @ri_arrangement_id = @original_ri_arrangement_id,  
                  @band_si = @original_sum_insured,  
                  @band_premium = @original_premium,  
                  @ri_model_id = @original_ri_model_id,  
                  @Trans_type = @Trans_type,  
                  @Extended_Limits_Enabled = @Original_Extended_Limits_Enabled,  
        @Extended_Limits_Amount = @Original_Extended_Limits_Amount  
  
END  
  
-- PBI 35359: After both calc calls, sync overridden values from the NB source arrangement
-- into the MTA current and original snapshot arrangements for lines where the NB has
-- is_edited=1. This handles the timing gap where spu_RI_Arrangement_copy ran before the
-- NB OK was clicked (is_edited=0 at copy time), so edited lines were recalculated from
-- RI model defaults instead of carrying forward the user overrides.
-- @Original_Risk_cnt is the NB risk key, always available in this SP.
-- Excluded: REN (Renewal must start fresh per spec - same behaviour as NB)
--           PT  (Portfolio Transfer has its own is_edited reset block below)
IF ISNULL(@Original_Risk_cnt, 0) > 0
AND @Trans_type NOT IN ('REN', 'PT')
BEGIN
    -- 1. Sync into MTA current arrangement (original_flag=0)
    UPDATE mta_line
    SET    mta_line.commission_percent = nb_line.commission_percent,
           mta_line.agreement_code     = nb_line.agreement_code,
           mta_line.sum_insured        = nb_line.sum_insured,
           mta_line.premium_value      = nb_line.premium_value,
           mta_line.is_edited          = 1,
           mta_line.is_premium_edited  = nb_line.is_premium_edited
    FROM   ri_arrangement_line mta_line
    JOIN   ri_arrangement mta_ra  ON mta_ra.ri_arrangement_id = mta_line.ri_arrangement_id
    JOIN   ri_arrangement nb_ra   ON nb_ra.risk_cnt      = @Original_Risk_cnt
                                 AND nb_ra.original_flag = 0
                                 AND nb_ra.ri_band_id    = mta_ra.ri_band_id
    JOIN   ri_arrangement_line nb_line
                               ON nb_line.ri_arrangement_id = nb_ra.ri_arrangement_id
                              AND nb_line.treaty_id         = mta_line.treaty_id
                              AND nb_line.type              = mta_line.type
                              AND ISNULL(nb_line.is_edited, 0) = 1
    WHERE  mta_ra.risk_cnt      = @risk_cnt
    AND    mta_ra.original_flag = 0
    AND    mta_ra.version_id    = @version_id
    AND    ISNULL(mta_line.is_edited, 0) = 0

    -- 2. Sync into original snapshot (original_flag=1) - preserve edited obligatory values
    --    so calc proc uses correct gross-net base for FAC calculation
    UPDATE snap_line
    SET    snap_line.commission_percent = nb_line.commission_percent,
           snap_line.agreement_code     = nb_line.agreement_code,
           snap_line.sum_insured        = -ABS(nb_line.sum_insured),
           snap_line.premium_value      = -ABS(nb_line.premium_value),
           snap_line.is_edited          = 1
    FROM   ri_arrangement_line snap_line
    JOIN   ri_arrangement snap_ra ON snap_ra.ri_arrangement_id = snap_line.ri_arrangement_id
    JOIN   ri_arrangement nb_ra   ON nb_ra.risk_cnt      = @Original_Risk_cnt
                                 AND nb_ra.original_flag = 0
                                 AND nb_ra.ri_band_id    = snap_ra.ri_band_id
    JOIN   ri_arrangement_line nb_line
                               ON nb_line.ri_arrangement_id = nb_ra.ri_arrangement_id
                              AND nb_line.treaty_id         = snap_line.treaty_id
                              AND nb_line.type              = snap_line.type
                              AND ISNULL(nb_line.is_edited, 0) = 1
    WHERE  snap_ra.risk_cnt      = @risk_cnt
    AND    snap_ra.original_flag = 1
    AND    snap_ra.version_id    = @version_id
    AND    snap_line.type NOT IN ('F', 'FX')

    -- 3. Sync ri_override_reason_id on MTA current arrangement header
    UPDATE mta_ra
    SET    mta_ra.ri_override_reason_id = nb_ra.ri_override_reason_id
    FROM   ri_arrangement mta_ra
    JOIN   ri_arrangement nb_ra ON nb_ra.risk_cnt      = @Original_Risk_cnt
                               AND nb_ra.original_flag = 0
                               AND nb_ra.ri_band_id    = mta_ra.ri_band_id
    WHERE  mta_ra.risk_cnt      = @risk_cnt
    AND    mta_ra.original_flag = 0
    AND    mta_ra.version_id    = @version_id
    AND    ISNULL(mta_ra.ri_override_reason_id, 0) = 0
    AND    ISNULL(nb_ra.ri_override_reason_id, 0) > 0
END
  
        --ReCalc this_share_percent & premium_percent  
        --Gaurav  
        --Start PN 71440  
        SELECT @obligatory_sum_insured = @sum_insured  
  
        IF EXISTS(SELECT COUNT(*)  
                  FROM   ri_arrangement_line  
                  WHERE  is_obligatory = 1  
                         AND ri_arrangement_id = @ri_arrangement_id)  
          BEGIN  
              UPDATE ri_arrangement_line  
              SET    this_share_percent = CASE  
                                            WHEN (  
                     CONVERT(FLOAT, @sum_insured) =  
                     0  
                      OR sum_insured = 0 ) THEN CASE  
                                            WHEN  
                                            premium_value = 0 THEN 0  
                                            ELSE  
                                            default_share_percent  
                                                END  
                                            ELSE ( sum_insured / CONVERT(FLOAT,  
                                                                 @sum_insured)  
                                                 ) *  
                                                 100.0000  
                                          END  
              WHERE  ri_arrangement_id = @ri_arrangement_id  
                     AND is_obligatory = 1  
                     AND ISNULL(is_edited, 0) = 0  
  
              SELECT @obligatory_sum_insured =  
                     @obligatory_sum_insured - sum_insured  
              FROM   ri_arrangement_line  
              WHERE  ri_arrangement_id = @ri_arrangement_id  
                     AND is_obligatory = 1  
          END  
  
    IF @premium != 0  
  UPDATE ri_arrangement_line
SET premium_percent = (ISNULL(premium_value, 0) / CONVERT(float, ISNULL(@premium, 1))) * 100.0000
WHERE ri_arrangement_id = @ri_arrangement_id
      AND ISNULL(Is_Obligatory, 0) = 0
      AND ISNULL(is_edited, 0) = 0;
  
        --End PN 71440  
        -- We should recalc all taxes, just to be safe  
        -- Note, this will also refresh the premium & comm shares  
        -- just in case the si/premium ratio has changed  
        EXECUTE Spu_ri_arrangement_taxes  
          @insurance_file_cnt = @insurance_file_cnt,  
          @risk_cnt = @risk_cnt,  
          @ri_arrangement_id = @ri_arrangement_id,  
          @band_premium = @premium  
  
          SELECT @Date_for_Prop_Calculation = NULL, @Date_for_Treaty_XOL_Calculation = NULL  
  
        -- Get next record  
        FETCH NEXT FROM ri_band_cursor INTO @ri_band, @sum_insured, @premium, @original_flag  
    END  
  
  -- Close the cursor  
  CLOSE ri_band_cursor  
  
  DEALLOCATE ri_band_cursor  

  -- Zero out premiums for manually added, FAC Prop (F) and FAC XOL (FX) treaties on cancellation (MTC)
  -- Non-manually-added non-FAC lines already have 0 premium from the peril data
  IF @Trans_type = 'MTC'
  BEGIN
      UPDATE ral
      SET    ral.premium_value    = 0,
             ral.premium_tax      = 0,
             ral.commission_value = 0,
             ral.commission_tax   = 0
      FROM   ri_arrangement_line ral
      JOIN   ri_arrangement ra ON ra.ri_arrangement_id = ral.ri_arrangement_id
      WHERE  ra.risk_cnt      = @risk_cnt
      AND    ra.original_flag = 0
      AND    ra.version_id    = @version_id
      AND    (ISNULL(ral.manually_added, 0) = 1 OR ral.type IN ('F', 'FX'))
  END
  
  IF ISNULL(@is_PT_DRI_Amended,0) = 1  
  BEGIN  
   SELECT @old_pro_rata_rate = ISNULL(pro_rata_rate,1)  FROM risk WHERE risk_cnt = @risk_cnt  
   UPDATE RI_Arrangement SET premium=@pro_rata_rate*premium/@old_pro_rata_rate,pro_rata_rate=@pro_rata_rate WHERE risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0  
  
   UPDATE RI_Arrangement_Line SET  premium_value=@pro_rata_rate*premium_value/@old_pro_rata_rate,  
   premium_tax=@pro_rata_rate*premium_tax/@old_pro_rata_rate, commission_tax=@pro_rata_rate*commission_tax/@old_pro_rata_rate,  
   commission_value=@pro_rata_rate*commission_value/@old_pro_rata_rate WHERE ri_arrangement_id IN  
   (SELECT ri_arrangement_id  FROM RI_Arrangement WHERE risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0)  
  
   UPDATE Tax_Calculation SET premium=premium*@pro_rata_rate/@old_pro_rata_rate, value=value*@pro_rata_rate/@old_pro_rata_rate  
   WHERE ri_arrangement_line_id IN (SELECT ral.ri_arrangement_line_id  FROM RI_Arrangement ra join RI_Arrangement_Line ral ON ra.ri_arrangement_id=ral.ri_arrangement_id WHERE risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0)  
  
  END  
  

  -- Delete any tax on arrangements on bands that are no longer in use  
  DELETE tax_calculation  
  WHERE  ri_arrangement_line_id IN (SELECT rl.ri_arrangement_line_id  
                                    FROM   ri_arrangement_line rl  
                                           INNER JOIN ri_arrangement ri  
                                             ON  
                ri.ri_arrangement_id = rl.ri_arrangement_id  
                                    WHERE  ri.risk_cnt = @risk_cnt  
                                           AND ri.ri_band_id NOT IN  
                                               (SELECT p.ri_band  
                                                FROM   peril p  
                                           JOIN rating_section rs  
                                             ON rs.risk_cnt =  
                                                p.risk_cnt  
                                                AND rs.rating_section_id =  
                                                    p.rating_section_id  
                                                WHERE  
                                           p.risk_cnt = @risk_cnt  
                                           AND ( p.is_premium = 1  
                                                  OR p.is_sum_insured  
                                                     = 1 )  
                                          AND rs.original_flag = 0)  
                                           AND ri.original_flag = 0)  
  
  -- Copy manually added treaty lines from original to current arrangement  
  -- EXECUTE Spu_RI_Arrangement_copy_ManualTreaty  
  --   @risk_cnt   = @risk_cnt,  
  --   @version_id = @version_id 
    -- Delete any arrangements on bands that are no longer in use  
  if not exists(select * From RI_Arrangement_line_Broker_Participants_Archive RIBrAr  
       Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
       Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
  Begin  
   Insert Into RI_Arrangement_line_Broker_Participants_Archive  
    (ri_arrangement_line_id,  
    ri_party_cnt,  
    participation_percent)  
      SELECT ri_arrangement_line_id,  
    ri_party_cnt,  
    participation_percent FROM RI_Arrangement_line_Broker_Participants  
    WHERE  ri_arrangement_line_id IN (Select ri_arrangement_line_id From ri_arrangement_line  
            WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id  
                    FROM   ri_arrangement  
                    WHERE  risk_cnt = @risk_cnt  
                    AND ri_band_id NOT IN  
                    (SELECT p.ri_band  
                     FROM   peril p  
                     JOIN rating_section rs  
                     ON rs.risk_cnt =p.risk_cnt  
                     AND rs.rating_section_id =p.rating_section_id  
                     WHERE p.risk_cnt = @risk_cnt  
                     AND ( p.is_premium = 1 OR p.is_sum_insured = 1 )  
                    AND rs.original_flag = 0)  
                   AND original_flag = 0)  
            )  
  
  End  
  DELETE RI_Arrangement_line_Broker_Participants  
  WHERE  ri_arrangement_line_id IN (Select ri_arrangement_line_id From ri_arrangement_line  
   WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id  
                               FROM   ri_arrangement  
                               WHERE  risk_cnt = @risk_cnt  
                                      AND ri_band_id NOT IN (SELECT p.ri_band  
                                                             FROM   peril p  
                                          JOIN rating_section  
                                               rs  
                                            ON rs.risk_cnt =  
                                     p.risk_cnt  
                                               AND rs.rating_section_id =  
                                                   p.rating_section_id  
                                                             WHERE  
                                          p.risk_cnt = @risk_cnt  
                                          AND ( p.is_premium = 1  
                                                 OR p.is_sum_insured = 1 )  
                                          AND rs.original_flag = 0)  
                                      AND original_flag = 0))  
  
  -- Delete any arrangements on bands that are no longer in use  
  if not exists(select * From ri_arrangement_Line_Archive RIALAr  
        Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id  
        Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                 inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                 inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                 Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
   Begin  
     Insert Into  RI_Arrangement_Line_Archive  
      (ri_arrangement_line_id,  
      ri_arrangement_id,  
      type,  
      treaty_id,  
      party_cnt,  
      default_share_percent,  
      this_share_percent,  
      premium_percent,  
      commission_percent,  
      agreement_code,  
      priority,  
      number_of_lines,  
      line_limit,  
      sum_insured,  
      premium_value,  
      commission_value,  
      premium_tax,  
      commission_tax,  
      is_commission_modified,  
      retained,  
      lower_limit,  
      participation_percent,  
      grouping,  
      ri_model_line_id,  
      Is_Obligatory,  
      manually_added,  
      is_premium_edited)  
      SELECT ri_arrangement_line_id,  
      ri_arrangement_id,  
      type,  
      treaty_id,  
      party_cnt,  
 default_share_percent,  
      this_share_percent,  
      premium_percent,  
      commission_percent,  
      agreement_code,  
      priority,  
      number_of_lines,  
      line_limit,  
      sum_insured,  
      premium_value,  
      commission_value,  
      premium_tax,  
      commission_tax,  
      is_commission_modified,  
      retained,  
      lower_limit,  
      participation_percent,  
      grouping,  
      ri_model_line_id,  
      Is_Obligatory,  
      manually_added,  
      is_premium_edited  
      FROM RI_Arrangement_Line  
     WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id FROM   ri_arrangement  
                WHERE  risk_cnt = @risk_cnt  
             AND ri_band_id NOT IN (SELECT p.ri_band  
             FROM   peril p  
             JOIN rating_section rs  
             ON rs.risk_cnt =p.risk_cnt  
             AND rs.rating_section_id =p.rating_section_id  
                      WHERE  p.risk_cnt = @risk_cnt  
             AND ( p.is_premium = 1  OR p.is_sum_insured = 1 )  
             AND rs.original_flag = 0)  
        AND original_flag = 0)  
  END
  
  DELETE ri_arrangement_line  
  WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id  
                               FROM   ri_arrangement  
                               WHERE  risk_cnt = @risk_cnt  
                                      AND ri_band_id NOT IN (SELECT p.ri_band  
                                                             FROM   peril p  
                                          JOIN rating_section  
                                               rs  
                                            ON rs.risk_cnt =  
                                     p.risk_cnt  
                                               AND rs.rating_section_id =  
                                                   p.rating_section_id  
                                                             WHERE  
                                          p.risk_cnt = @risk_cnt  
                                          AND ( p.is_premium = 1  
                                                 OR p.is_sum_insured = 1 )  
                                          AND rs.original_flag = 0)  
                                      AND original_flag = 0)  
         AND ISNULL(manually_added, 0) = 0  
  
  DELETE ri_arrangement  
  WHERE  risk_cnt = @risk_cnt  
         AND ri_band_id NOT IN (SELECT p.ri_band  
                 FROM   peril p  
                                       JOIN rating_section rs  
                                         ON rs.risk_cnt = p.risk_cnt  
                                            AND rs.rating_section_id =  
                                                p.rating_section_id  
                                WHERE  p.risk_cnt = @risk_cnt  
                                       AND ( p.is_premium = 1  
                                              OR p.is_sum_insured = 1 )  
                                       AND rs.original_flag = 0)  
         AND original_flag = 0  
  
  -- PN 52372 - START  
  DECLARE @isEdited INT  
  
  SELECT @Recalculate_prorata_reinsurance_during_MTA = Isnull(VALUE, 0)  
  FROM   system_options  
  WHERE  option_number = 5070  
         AND branch_id = 1  
  
  IF @Recalculate_prorata_reinsurance_during_MTA = 1  
     AND @is_auto_reinsured = 1  
    -- recalculate original reinsurance  
    BEGIN  
        -- Arrangement Line cursor  
        DECLARE arrangement_line_cursor CURSOR FAST_FORWARD FOR  
          SELECT ra.ri_arrangement_id,  
 ral.ri_arrangement_line_id,  
                 ral.treaty_id,  
                 party_cnt  
          FROM   ri_arrangement_line ral  
                 INNER JOIN ri_arrangement ra  
                   ON ra.ri_arrangement_id = ral.ri_arrangement_id  
                 INNER JOIN ri_model_line rml  
ON rml.ri_model_id = ra.ri_model_id  
                 LEFT JOIN treaty t  
                   ON t.replaced_by_treaty_id = ral.treaty_id  
                      AND t.treaty_id = rml.treaty_id  
          WHERE  ra.risk_cnt = @risk_cnt  
                 AND ra.original_flag = 1  
                 AND ral.treaty_id IS NOT NULL  
            AND t.treaty_id IS NOT NULL  
     AND version_id= @version_id  
                 AND ISNULL(ral.manually_added, 0) = 0  
          ORDER  BY ral.ri_arrangement_line_id  
  
        OPEN arrangement_line_cursor  
  
        FETCH NEXT FROM arrangement_line_cursor INTO @ri_arrangement_id,  
        @ri_arrangement_line_id, @treaty_id, @party_cnt  
  
        -- For each of the old arrangements  
        WHILE ( @@FETCH_STATUS = 0 )  
          BEGIN  
              -- Copy Default percent value from New RI to Original on the basis of Treaty ID  
              -- here make sure Replaced by treaty should exact with new ri model else this cannot be update  
              SELECT @default_share_percent = default_share_percent  
              FROM   ri_arrangement_line ral  
                     INNER JOIN ri_arrangement ra  
                       ON ra.ri_arrangement_id = ral.ri_arrangement_id  
              WHERE  ra.risk_cnt = @risk_cnt  
                     AND ra.original_flag = 0  
                     AND treaty_id = @treaty_id  AND version_id = @version_id  
  
              IF @default_share_percent IS NOT NULL  
                BEGIN  
                    UPDATE ri_arrangement_line  
       SET    default_share_percent = @default_share_percent  
                    WHERE  ri_arrangement_line_id = @ri_arrangement_line_id  
  
                    SELECT @isEdited = 1  
                END  
  
              -- Get next arrangement line  
              FETCH NEXT FROM arrangement_line_cursor INTO @ri_arrangement_id,  
              @ri_arrangement_line_id, @treaty_id, @party_cnt  
          END  
  
        -- Close and release cursor  
        CLOSE arrangement_line_cursor  
  
        DEALLOCATE arrangement_line_cursor  
  
        IF @isEdited = 1  
          BEGIN  
              DECLARE arrangement_band_cursor CURSOR FAST_FORWARD FOR  
                SELECT ri_arrangement_id,  
                       xol_ri_model_id,  
                       sum_insured,  
                       premium  
                FROM   ri_arrangement  
                WHERE  risk_cnt = @risk_cnt  
                       AND original_flag = 1  AND version_id = @version_id  
  
              OPEN arrangement_band_cursor  
  
              FETCH NEXT FROM arrangement_band_cursor INTO @ri_arrangement_id,  
              @ri_model_id, @sum_insured, @premium  
  
              -- For each of the old arrangements  
              WHILE ( @@FETCH_STATUS = 0 )  
                BEGIN  
                    EXECUTE Spu_ri_arrangement_calc_ri2007  
                      @ri_arrangement_id = @ri_arrangement_id,  
                      @band_si = @sum_insured,  
                      @band_premium = @premium,  
                      @ri_model_id = @ri_model_id,  
                      @Trans_type = @Trans_type,  
                      @calc_original = 1  
                    --Send this 1 to calculate original RI Model  
  
                    -- Get next arrangement line  
                    FETCH NEXT FROM arrangement_band_cursor INTO  
                    @ri_arrangement_id,  
                    @ri_model_id, @sum_insured, @premium  
                END  
                CLOSE arrangement_band_cursor  
                DEALLOCATE arrangement_band_cursor  
          END  
    END  

GO