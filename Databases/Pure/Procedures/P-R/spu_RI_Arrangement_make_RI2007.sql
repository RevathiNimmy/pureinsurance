SET quoted_identifier ON 

GO 

SET ansi_nulls ON 

GO 

EXECUTE Ddldropprocedure 'spu_RI_Arrangement_make_RI2007' 

GO 

CREATE PROCEDURE Spu_ri_arrangement_make_ri2007 @ri_arrangement_id     INT,        
                                                @risk_type_id          INTEGER,        
                                                @ri_band_id            INT,        
                                                @effective_date        DATETIME,        
                                                @allow_deferred        TINYINT,        
                                                @sum_insured           MONEY,        
                                                @premium               MONEY,        
                                                @line_limit            MONEY,        
                                                @is_auto_reinsured     TINYINT,        
                                                @source_id             INT,        
                                                @policy_currency_id    SMALLINT,        
                                                @policy_currency_rate  FLOAT,        
                                                @Trans_type            VARCHAR(5) = '',        
                                                @NBExtended_Is_Enabled TINYINT = 0 ,        
                                                @prop_effective_date   DATETIME = NULL,        
            @Original_Risk_Cnt    INT = 0   ,  
   @cover_start_date_ForRi datetime = null  
AS        
  DECLARE @model_currency_id       SMALLINT,        
          @model_currency_rate     FLOAT,        
          @ri_model_id             INT,        
          @reinsurance_type        VARCHAR(3),        
          @Extended_Limits_Enabled TINYINT,        
          @ri_model_type    TINYINT,        
        
          @xol_model_currency_id       SMALLINT,        
          @xol_model_currency_rate     FLOAT,        
          @xol_ri_model_id             INT,        
      @xol_ri_model_type    TINYINT,        
      @Date_for_Treaty_XOL_Calculation INT,        
      @Date_for_Prop_Calculation INT,        
      @Is_original INT       
        
  SET @reinsurance_type = 'T'        
       
	   
Select top 1 @cover_start_date_ForRi =  IFL.inception_date_tpi From ri_arrangement RI        
             inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt        
             inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt        
             Where RI.ri_arrangement_id=@ri_arrangement_id

  -- If this Risk is automatically reinsured        
  IF @is_auto_reinsured = 1        
    BEGIN        
        -- This risk is auto-reinsured, so delete all lines, new and original,        
        -- except the new facultative ri as that is never automatic.        
        DELETE tax_calculation        
        WHERE  ri_arrangement_line_id IN (SELECT ri_arrangement_line_id        
                                          FROM   ri_arrangement_line        
                                          WHERE        
               ri_arrangement_id = @ri_arrangement_id        
               AND TYPE <> 'F')        
        
  if not exists(select * From ri_arrangement_Line_Archive RIALAr        
     Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id        
     Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI        
             inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt        
             inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt        
             Where ifl.insurance_file_type_id in (2,5,8,9) and RI.ri_arrangement_id=@ri_arrangement_id))        
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
    Is_Obligatory,manually_added,is_premium_edited)        
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
    Is_Obligatory , manually_added, is_premium_edited       
    FROM RI_Arrangement_Line        
    Where ri_arrangement_id = @ri_arrangement_id AND TYPE NOT IN( 'F', 'FX' )  AND ISNULL(manually_added,0) <> 1 AND ISNULL(is_edited,0) <> 1 AND ISNULL(is_premium_edited,0) <> 1
  End        
        
  DELETE from RI_Arrangement_line_Broker_Participants        
   WHERE  ri_arrangement_line_id IN (select ri_arrangement_line_id from ri_arrangement_line WHERE ri_arrangement_id = @ri_arrangement_id AND TYPE NOT IN( 'F', 'FX' ))        
  -- preserve Fac and manually added treaties
  DELETE ri_arrangement_line   WHERE  ri_arrangement_id = @ri_arrangement_id   AND TYPE NOT IN( 'F', 'FX' ) AND ISNULL(manually_added,0) <> 1 AND ISNULL(is_edited,0) <> 1 AND ISNULL(is_premium_edited,0) <> 1
    
        
 END        
  IF EXISTS (SELECT NULL        
             FROM   ri_arrangement_line        
             WHERE  ri_arrangement_id = @ri_arrangement_id        
                    AND TYPE NOT IN ( 'F', 'FX' ) AND ISNULL(manually_added,0) <> 1 AND ISNULL(is_edited,0) <> 1 AND ISNULL(is_premium_edited,0) <> 1)        
    RETURN        
      -- Get the best RI Model for this ri_band and risk_type        
      SELECT @ri_model_id = NULL        
   SELECT @xol_ri_model_id = NULL        
        
  SELECT @ri_model_id = rmu.ri_model_id,        
         @model_currency_id = currency_id,        
         @ri_model_type = ISNULL(rm.ri_model_type,0)        
  FROM   risk_type_ri_model_usage rmu        
         JOIN ri_model rm        
           ON rm.ri_model_id = rmu.ri_model_id        
  WHERE  rmu.risk_type_id = @risk_type_id        
         AND rmu.ri_band = @ri_band_id        
         AND rmu.is_deleted = 0        
         AND rmu.effective_date <= @prop_effective_date        
         AND ( rmu.expiry_date >= @prop_effective_date        
                OR Isnull(rmu.expiry_date, '1899.12.29') = '1899.12.29' )        
         AND ( rm.ri_model_type = 0        
                OR ( rm.ri_model_type = 2        
                     AND @allow_deferred = 1 ) OR (rm.ri_model_type = 4) )        
         AND rm.is_deleted = 0        
         AND rm.effective_date <= @prop_effective_date        
         AND ( rm.expiry_date >= @prop_effective_date        
                OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )        
  ORDER  BY rm.ri_model_type DESC,        
            -- give priority to none-deferred models        
            rmu.effective_date ASC -- give priority to newer models        
  -- If model is not specified for band, check for a system default model        
  IF @ri_model_id IS NULL        
    SELECT @ri_model_id = rm.ri_model_id,        
           @model_currency_id = currency_id        
    FROM   ri_model rm        
    WHERE  rm.ri_model_type = 1 -- Default        
           AND rm.is_deleted = 0        
           AND rm.effective_date <= @prop_effective_date        
           AND ( rm.expiry_date >= @prop_effective_date        
                  OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )        
    ORDER  BY rm.effective_date -- give priority to newer models        
        
 SELECT @xol_ri_model_id = rmu.ri_model_id,        
         @xol_model_currency_id = currency_id,        
         @xol_ri_model_type = ISNULL(rm.ri_model_type,0)        
  FROM   risk_type_ri_model_usage rmu        
         JOIN ri_model rm        
           ON rm.ri_model_id = rmu.ri_model_id        
  WHERE  rmu.risk_type_id = @risk_type_id        
         AND rmu.ri_band = @ri_band_id        
         AND rmu.is_deleted = 0        
         AND rmu.effective_date <= @effective_date        
         AND ( rmu.expiry_date >= @effective_date        
                OR Isnull(rmu.expiry_date, '1899.12.29') = '1899.12.29' )        
         AND ( rm.ri_model_type = 0        
                OR ( rm.ri_model_type = 2        
                     AND @allow_deferred = 1 ) OR (rm.ri_model_type = 4) )        
         AND rm.is_deleted = 0        
         AND rm.effective_date <= @effective_date        
         AND ( rm.expiry_date >= @effective_date        
        OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )        
  ORDER  BY rm.ri_model_type DESC,        
            -- give priority to none-deferred models        
            rmu.effective_date ASC -- give priority to newer models        
  -- If model is not specified for band, check for a system default model        
  IF @xol_ri_model_id IS NULL        
    SELECT @xol_ri_model_id = rm.ri_model_id,        
           @xol_model_currency_id = currency_id        
    FROM   ri_model rm        
    WHERE  rm.ri_model_type = 1 -- Default        
           AND rm.is_deleted = 0        
           AND rm.effective_date <= @effective_date        
           AND ( rm.expiry_date >= @effective_date        
                  OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )        
    ORDER  BY rm.effective_date -- give priority to newer models        
        
 IF @ri_model_id IS NULL OR @xol_ri_model_id IS NULL        
    return        
        
  -- Update the arrangement        
  UPDATE ri_arrangement        
  SET    xol_ri_model_id = @xol_ri_model_id ,        
   ri_model_id = @ri_model_id        
  WHERE  ri_arrangement_id = @ri_arrangement_id        
        
   SELECT TOP 1 @Date_for_Treaty_XOL_Calculation =        
               ISNULL(@Date_for_Treaty_XOL_Calculation ,date_for_treaty_xol_calculation_id),        
               @Date_for_Prop_Calculation=        
      ISNULL(@Date_for_Prop_Calculation ,Proportional_RI_Cal_Method )        
        FROM   RI_Band_Version        
        WHERE  ri_band_id = @ri_band_id        
  AND CONVERT(DATE, effective_date, 23)  <= CONVERT(DATE, @cover_start_date_ForRi, 23)       
  ORDER BY effective_date DESC      
        
   UPDATE RI_Arrangement        
   SET xol_calc_method_id  = @Date_for_Treaty_XOL_Calculation,        
   prop_calc_method_id = @Date_for_Prop_Calculation        
   WHERE  ri_arrangement_id = @ri_arrangement_id        
 -- E007        
 IF @xol_ri_model_type =4  OR @ri_model_type = 4        
 BEGIN        
  UPDATE  ri_arrangement        
  SET     Cloned = 1        
  WHERE   ri_arrangement_id = @ri_arrangement_id        
 END        
 ELSE        
 BEGIN        
  UPDATE  ri_arrangement        
  SET     Cloned = 0        
  WHERE   ri_arrangement_id = @ri_arrangement_id        
 END        
        
  -- WPR55? Read the options only in case of NB or Ren else it will be copied by Copy SP        
  IF @Trans_type = 'NB'        
      OR @Trans_type = 'REN'        
    SELECT @Extended_Limits_Enabled = Isnull(VALUE, 0)        
    FROM   system_options        
    WHERE  option_number = 5260        
           AND branch_id = 1        
        
  -- If Switched On Then        
  --If @Extended_Limits_Enabled=1        
  IF @Trans_type = 'MTA'        
      OR @Trans_type = 'MTC'        
      OR @Trans_type = 'MTR'        
      OR @Trans_type IN ('MTCR', 'PT', 'DRI')        
    BEGIN        
        UPDATE ri_arrangement        
        SET    is_extended_limit_applied = Isnull(@NBExtended_Is_Enabled, 0),        
               extended_limit_amount = Isnull(@line_limit, 0)        
        WHERE  ri_arrangement_id = @ri_arrangement_id  and original_flag=0        
        
        SET @Extended_Limits_Enabled=@NBExtended_Is_Enabled        
    END        
  ELSE        
    IF Isnull(@Extended_Limits_Enabled, 0) = 1        
      UPDATE ri_arrangement        
      SET    extended_limit_amount = Isnull(@line_limit, 0),        
             is_extended_limit_applied = Isnull(@Extended_Limits_Enabled, 0)        
      WHERE  ri_arrangement_id = @ri_arrangement_id        
    ELSE        
      IF Isnull(@Extended_Limits_Enabled, 0) = 0        
    UPDATE ri_arrangement        
        SET    extended_limit_amount = 0,        
               is_extended_limit_applied = 0        
        WHERE  ri_arrangement_id = @ri_arrangement_id        
        
		--Get currency rate for policy from RI Model 

SELECT @policy_currency_rate = 
    ISNULL((
        SELECT conversion_rate
        FROM RIModelCurrencyRates
        WHERE currency_id = @policy_currency_id AND ri_model_id = @ri_model_id
    ), @policy_currency_rate)


  -- If different get combined rate, else set rate as 1        
  IF @model_currency_id <> @policy_currency_id        
    BEGIN        
        EXECUTE Spu_act_get_currency_rate        
          @model_currency_id,        
          @source_id,        
          @prop_effective_date,        
          @model_currency_rate OUTPUT        
        
        SELECT @model_currency_rate = @model_currency_rate /        
                                      @policy_currency_rate        
    END        
  ELSE        
    SELECT @model_currency_rate = 1        
        
  -- Insert the arrangement lines        
  INSERT INTO ri_arrangement_line        
              (ri_arrangement_id,        
               TYPE,        
               treaty_id,        
               default_share_percent,        
               this_share_percent,        
               premium_percent,        
               commission_percent,        
               agreement_code,        
               priority,        
               number_of_lines,        
    line_limit,        
               lower_limit,        
               sum_insured,        
               premium_value,        
               commission_value,        
               is_commission_modified,        
               retained,        
               --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
               ri_model_line_id,        
               is_obligatory,
               manually_added)        
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
  SELECT @ri_arrangement_id,        
         CASE        
           WHEN rt.code = 'RET' THEN 'R'        
           WHEN rt.code = 'XOL'        
                 OR rt.code = 'FAX' THEN 'TX'        
           --When rt.code = 'FAX'  Then  'FX'        
           WHEN rt.code = 'CAT' THEN 'TC'        
           WHEN rt.code = 'PAX' THEN 'PX'        
       WHEN rt.code = '001' THEN 'TFS'        
       WHEN rt.code = '002' THEN 'TFS'        
       WHEN rt.code = '003' THEN 'TFS'        
           ELSE 'T'        
         END,        
         rml.treaty_id,        
         rml.share_percent,        
         0,        
         0,        
         tc.commission_percent,        
         t.agreement_code,        
         rml.priority,        
rml.number_of_lines,        
         CASE        
           WHEN Isnull(@line_limit, 0) <> 0        
                AND ( rt.code NOT IN ( 'XOL', 'CAT', 'PAX' ) )        
                 OR ( @Extended_Limits_Enabled = 1        
                      AND rt.code IN ( 'TFS', 'R', 'T' ) )THEN Isnull(@line_limit, 0)        
           ELSE ( rml.line_limit ) * @model_currency_rate        
         END,        
         rml.lower_limit * @model_currency_rate,        
         0,        
         0,        
         0,        
         0,        
         CASE        
           WHEN rt.code = 'RET' THEN 100        
           ELSE NULL        
         END retained,        
         --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
         rml.ri_model_line_id,        
         rml.is_obligatory,
         0 AS manually_added        
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
  FROM   ri_model_line rml        
         JOIN treaty t        
           ON t.treaty_id = rml.treaty_id        
         JOIN reinsurance_type rt        
           ON rt.reinsurance_type_id = t.reinsurance_type_id        
         LEFT JOIN        
         -- Calculate a summary commission rate for each treaty        
         (SELECT treaty_id,        
                 SUM(commission_percent * ( share_percent / 100 ))        
                 commission_percent        
          FROM   treaty_party        
          GROUP  BY treaty_id) tc        
           ON tc.treaty_id = t.treaty_id        
  WHERE  ri_model_id = @ri_model_id  and rt.code NOT IN ( 'XOL', 'CAT','RET','PAX' )
         AND NOT EXISTS (
            SELECT 1 
            FROM ri_arrangement_line ral
            WHERE ral.ri_arrangement_id = @ri_arrangement_id
                AND ral.treaty_id = rml.treaty_id
              AND (ISNULL(ral.manually_added, 0) = 1
                     OR ISNULL(ral.is_edited, 0) = 1
                     OR ISNULL(ral.is_premium_edited, 0) = 1)
        ) 
  SELECT @policy_currency_rate = 
    ISNULL((
        SELECT conversion_rate
        FROM RIModelCurrencyRates
        WHERE currency_id = @policy_currency_id AND ri_model_id = @xol_ri_model_id
    ), @policy_currency_rate)  
 
        
IF @xol_model_currency_id <> @policy_currency_id        
    BEGIN        
        EXECUTE Spu_act_get_currency_rate        
          @xol_model_currency_id,   
          @source_id,        
          @effective_date,        
          @xol_model_currency_rate OUTPUT        
        
        SELECT @xol_model_currency_rate = @xol_model_currency_rate /        
                                      @policy_currency_rate        
    END        
  ELSE        
    SELECT @xol_model_currency_rate = 1        
        
  -- Insert the arrangement lines        
  INSERT INTO ri_arrangement_line        
              (ri_arrangement_id,        
               TYPE,        
               treaty_id,        
               default_share_percent,        
               this_share_percent,        
               premium_percent,        
               commission_percent,        
               agreement_code,        
               priority,        
               number_of_lines,        
               line_limit,        
               lower_limit,        
               sum_insured,        
               premium_value,        
               commission_value,        
               is_commission_modified,        
               retained,        
               --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
               ri_model_line_id,        
               is_obligatory,
               manually_added)        
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
  SELECT @ri_arrangement_id,        
         CASE        
           WHEN rt.code = 'RET' THEN 'R'        
           WHEN rt.code = 'XOL'        
                 OR rt.code = 'FAX' THEN 'TX'        
           --When rt.code = 'FAX'  Then  'FX'        
           WHEN rt.code = 'CAT' THEN 'TC'        
           WHEN rt.code = 'PAX' THEN 'PX'        
           WHEN rt.code = '001' THEN 'TFS'        
           WHEN rt.code = '002' THEN 'TFS'        
           WHEN rt.code = '003' THEN 'TFS'        
           ELSE 'T'             END,        
         rml.treaty_id,        
         rml.share_percent,        
         0,        
         0,        
         tc.commission_percent,        
         t.agreement_code,        
         rml.priority,        
rml.number_of_lines,        
         CASE        
           WHEN Isnull(@line_limit, 0) <> 0        
                AND ( rt.code NOT IN ( 'XOL', 'CAT', 'PAX' ) )        
                 OR ( @Extended_Limits_Enabled = 1        
                      AND rt.code IN ( 'TFS', 'R' , 'T') )THEN Isnull(@line_limit, 0)        
           ELSE ( rml.line_limit ) * @xol_model_currency_rate        
         END,        
         rml.lower_limit * @xol_model_currency_rate,        
         0,        
         0,        
         0,        
         0,        
         CASE        
           WHEN rt.code = 'RET' THEN 100        
           ELSE NULL        
         END retained,        
         --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
         rml.ri_model_line_id,        
         rml.is_obligatory,
         0 manually_added        
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)        
  FROM   ri_model_line rml        
         JOIN treaty t        
           ON t.treaty_id = rml.treaty_id        
         JOIN reinsurance_type rt        
           ON rt.reinsurance_type_id = t.reinsurance_type_id        
         LEFT JOIN        
         -- Calculate a summary commission rate for each treaty        
         (SELECT treaty_id,        
                 SUM(commission_percent * ( share_percent / 100 ))        
                 commission_percent        
          FROM   treaty_party        
          GROUP  BY treaty_id) tc        
           ON tc.treaty_id = t.treaty_id        
  WHERE  ri_model_id = @xol_ri_model_id and rt.code IN ( 'XOL', 'CAT','RET','PAX' )
         AND NOT EXISTS (
            SELECT 1 s
            FROM ri_arrangement_line ral
            WHERE ral.ri_arrangement_id = @ri_arrangement_id
                AND ral.treaty_id = rml.treaty_id
                AND (ISNULL(ral.manually_added, 0) = 1
                     OR ISNULL(ral.is_edited, 0) = 1
                     OR ISNULL(ral.is_premium_edited, 0) = 1)
        )        
  -- Recalculate the RI Arrangement Lines        
  Select @Is_original = original_flag from RI_Arrangement where ri_arrangement_id =@ri_arrangement_id        
  IF @Extended_Limits_Enabled=1 And @Is_original <>1 And ISNULL(@line_limit,0) >0        
      Update RI_Arrangement_Line set line_limit=ISNULL(@line_limit,0) where type in ('TFS','T','R')   and ri_arrangement_id = @ri_arrangement_id        
        
  IF @is_auto_reinsured = 1        
    IF @reinsurance_type = 'R'        
        OR @reinsurance_type = 'T'        
      EXECUTE Spu_ri_arrangement_calc_ri2007        
        @ri_arrangement_id = @ri_arrangement_id,        
        @band_si = @sum_insured,        
        @band_premium = @premium,        
        @ri_model_id = @xol_ri_model_id,        
        @Trans_type = @Trans_type,        
        @Extended_Limits_Enabled = @Extended_Limits_Enabled,        
        @Extended_Limits_Amount = @line_limit,        
  @Original_Risk_Cnt = @Original_Risk_Cnt 

  GO