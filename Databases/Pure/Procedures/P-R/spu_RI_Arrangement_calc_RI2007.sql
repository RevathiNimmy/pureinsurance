
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_RI_Arrangement_calc_RI2007'
GO
CREATE PROCEDURE spu_RI_Arrangement_calc_RI2007
 @ri_arrangement_id integer,
 @band_si money,
 @band_premium money,
 @ri_model_id INT,
 @trans_type varchar(5)='',
 @calc_original int = 0,
 @Extended_Limits_Enabled int = 0,
 @Extended_Limits_Amount numeric(19,5) = 0,
 @Original_Risk_Cnt INT = 0
AS
Declare
 @SetBand_Premium SMALLINT,
 @grossnet_premium float,
 @grossnet_SI float,
 @ret_line_id int,
 @line_id int,
 @number_of_lines float,
 @priority_limit money,
 @priority_allocated_si money,
 @priority_allocated_premium money,
 @priority_share float,
 @retained float,
 @treaty_id integer,
 @RetainedPercentage float,
 @priority int,
 @last_priority int,
 @first_priority int,
 @this_premium money,
 @running_premium money,
 @fac_si numeric(19,5),
 @fac_premium money,
 @priority_si money,
 @priority_premium money,
 @Priority_share_Premium numeric(19,5),
 @this_si numeric(19,5),
 @running_si money,
 @type varchar(3),
 @line_limit money,
 @lower_limit money,
 @default_percent Float,
 @ceding_rate float,
 @ri_model_line_id int,
 @negative_si int,
 @retained_share int,
 @Is_Obligatory tinyint,
 @cede_premium_only tinyint,
 @SetBand_Premium_For_CAT SMALLINT,
 @FACTotalPremium money,
 @RIArrangementSI money,
 @GrossSI money,
 @Allocated_SI money,
 @is_original INT,
 @reverse_Values int,
 @XOL_Totals_SI MONEY,
 @XOL_Premium MONEY,
 @version_id INT,
 @insurance_file_type_id INT,
 @total_si_FAC numeric(19,5),
 @total_premium_FAC numeric(19,5),
 @QsTotal numeric(19,5),
 @insurance_status_id INT,
 @xol_ri_model_id INT,
 @upper_limit money,
 @prop_ri_model_id INT,
 @premium_percent Float,
 @Grouping INT,
 @Total_premium float,
 @this_si_Obligatory numeric(19,5),
 @this_premium_Obligatory numeric(19,5),
 @treaty_type_id int,
 @is_variableQuotaShare TINYINT,
 @fac_premium_type TINYINT,
 @is_edited_line BIT,
 @is_manually_added BIT,
 @reinsurance_type_id INT,
 @is_premium_edited_line BIT

 DECLARE @IS_SettleTFSAmountIntoRetain tinyint = 0
 SELECT @is_original=original_flag, @version_id=version_id,
        @xol_ri_model_id=xol_ri_model_id, @prop_ri_model_id=ri_model_id
 FROM ri_arrangement WHERE ri_arrangement_id=@ri_arrangement_id

 IF @Extended_Limits_Enabled=1 AND ISNULL(@Extended_Limits_Amount,0)=0
   SELECT @Extended_Limits_Amount=Extended_limit_amount FROM ri_arrangement WHERE ri_arrangement_id=@ri_arrangement_id

 IF @is_original=1 AND @Extended_Limits_Enabled=1 AND ISNULL(@Extended_Limits_Amount,0)>0
 BEGIN
   IF @trans_type='PT'
   BEGIN
     SELECT @Extended_Limits_Amount=Extended_limit_amount FROM ri_arrangement WHERE ri_arrangement_id=@ri_arrangement_id
     IF ISNULL(@Extended_Limits_Amount,0)>0
       UPDATE RI_Arrangement_Line SET line_limit=ISNULL(@Extended_Limits_Amount,0)
       WHERE TYPE IN ('TFS','T','R') AND ri_arrangement_id=@ri_arrangement_id
   END
 END

 IF @is_original=1 AND @version_id>1
 BEGIN
   SELECT @XOL_Totals_SI=SUM(sum_insured), @XOL_Premium=SUM(premium_value)
   FROM RI_Arrangement_Line WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE IN ('TX','TC','PX')
   UPDATE RI_Arrangement_Line SET premium_value=0, premium_percent=0
   WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE IN ('TX','TC','PX')
   UPDATE RI_Arrangement_Line SET premium_value=ISNULL(premium_value,0)+ISNULL(@XOL_Premium,0)
   WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE IN ('R')
 END
 ELSE
 BEGIN
   IF @is_original=1 AND @band_si<0
   BEGIN
     SET @reverse_Values=1
     SET @band_si=ABS(@band_si)
     SET @band_premium=ABS(@band_premium)
   END

   SELECT @running_si=@band_si, @running_premium=@band_premium, @last_priority=-666,
          @priority_share=0, @priority_si=0, @priority_premium=0, @SetBand_Premium=0,
          @grossnet_premium=@band_premium, @fac_premium=0, @fac_si=0,
          @SetBand_Premium_For_CAT=0, @FACTotalPremium=0, @RIArrangementSI=@band_si,
          @GrossSI=@band_si, @grossnet_SI=@band_si, @Total_premium=@band_premium

   IF @band_si<0 SELECT @negative_si=-1 ELSE SELECT @negative_si=1

   SELECT @ret_line_id=ri_arrangement_line_id FROM ri_arrangement_line
   WHERE ri_arrangement_id=@ri_arrangement_id AND type='R'
   ORDER BY ri_arrangement_line_id

   UPDATE RI_Arrangement_Line
   SET sum_insured=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(@band_si*default_share_percent/100) ELSE @band_si*default_share_percent/100 END,
       premium_value=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(@band_premium*default_share_percent/100) ELSE @band_premium*default_share_percent/100 END
   WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE='T' AND ISNULL(is_obligatory,0)=1 AND ISNULL(is_edited,0)=0

   SELECT @this_si_Obligatory=SUM(CASE WHEN ISNULL(@reverse_Values,0)=1 THEN ABS(sum_insured) ELSE sum_insured END),
          @this_premium_Obligatory=SUM(CASE WHEN ISNULL(@reverse_Values,0)=1 THEN ABS(premium_value) ELSE premium_value END)
   FROM RI_Arrangement_Line WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE='T' AND ISNULL(is_obligatory,0)=1
   
   SELECT @grossnet_SI=@band_si-ISNULL(@this_si_Obligatory,0),
          @grossnet_premium=@band_premium-ISNULL(@this_premium_Obligatory,0)

   SELECT @fac_premium_type=ISNULL(fac_premium_type,0) FROM RI_Model
   WHERE ri_model_id IN (SELECT ri_model_id FROM RI_Arrangement WHERE RI_Arrangement_id=@ri_arrangement_id)

   UPDATE RI_Arrangement_Line
   SET sum_insured=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(@grossnet_SI*this_share_percent/100) ELSE @grossnet_SI*this_share_percent/100 END,
       premium_value=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(@grossnet_premium*CASE WHEN ISNULL(FACPropPremiumPerc,0)<>0 AND @fac_premium_type=1 THEN ISNULL(FACPropPremiumPerc,0) ELSE this_share_percent END/100) ELSE @grossnet_premium*CASE WHEN ISNULL(FACPropPremiumPerc,0)<>0 AND @fac_premium_type=1 THEN ISNULL(FACPropPremiumPerc,0) ELSE this_share_percent END/100 END
   WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE='F' AND ISNULL(is_edited,0)=0

   SELECT @total_si_FAC=SUM(sum_insured), @total_premium_FAC=SUM(premium_value)
   FROM RI_Arrangement_Line WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE='F'
   -- When reverse_Values=1 (original arrangement with negative SI), FAC values are negative;
   -- use ABS to ensure correct deduction from the ABS'd grossnet base
   SELECT @grossnet_SI=@grossnet_SI-ISNULL(CASE WHEN ISNULL(@reverse_Values,0)=1 THEN ABS(@total_si_FAC) ELSE @total_si_FAC END,0),
          @grossnet_premium=@grossnet_premium-ISNULL(CASE WHEN ISNULL(@reverse_Values,0)=1 THEN ABS(@total_premium_FAC) ELSE @total_premium_FAC END,0)

   SELECT @total_si_FAC=SUM(sum_insured), @total_premium_FAC=SUM(premium_value)
   FROM RI_Arrangement_Line WHERE ri_arrangement_id=@ri_arrangement_id AND TYPE='FX' AND ISNULL(retained,0)=0
   SELECT @grossnet_SI=@grossnet_SI-ISNULL(CASE WHEN ISNULL(@reverse_Values,0)=1 THEN ABS(@total_si_FAC) ELSE @total_si_FAC END,0),
          @grossnet_premium=@grossnet_premium-ISNULL(CASE WHEN ISNULL(@reverse_Values,0)=1 THEN ABS(@total_premium_FAC) ELSE @total_premium_FAC END,0)

   -- R included for SI/this_share_percent; its premium zeroed in loop so remainder sets it correctly
   -- manually_added lines: skip UPDATE (arrangement_copy already set correct signed values)
   Declare Line_Cursor Cursor Fast_Forward For
   Select ri_arrangement_line_id,
          Case When type='F' then this_share_percent/100 Else default_share_percent/100 End,
          ral.priority, ral.number_of_lines, ral.line_limit, ral.treaty_id, ral.type,
          ral.ri_model_line_id, ISNULL(retained,0), ISNULL(ral.is_obligatory,0),
          ral.lower_limit, premium_percent, grouping,
          -- For manually added lines ri_model_line_id is NULL so rml.treaty_type_id is NULL;
          -- derive treaty_type_id from the line type to ensure correct sort order
          CASE WHEN ISNULL(ral.manually_added,0)=1
               THEN CASE WHEN ral.type IN ('TX','PX') THEN 2 ELSE 1 END
               ELSE rml.treaty_type_id
          END,
          ISNULL(ral.is_edited,0), ISNULL(ral.manually_added,0),
          ISNULL(t.reinsurance_type_id,0),
          ISNULL(ral.is_premium_edited,0)
   From ri_arrangement_line ral
   left join ri_model_line rml on ral.ri_model_line_id=rml.ri_model_line_id
   left join treaty t on ral.treaty_id=t.treaty_id
   Where ri_arrangement_id=@ri_arrangement_id AND ISNULL(ral.Is_Obligatory,0)=0
   And Type In ('R','T','TX','TC','TFS','PX')
   Order By ISNULL(ral.is_obligatory,0) Desc, ral.priority,
            CASE WHEN ISNULL(ral.manually_added,0)=1
                 THEN CASE WHEN ral.type IN ('TX','PX') THEN 2 ELSE 1 END
                 ELSE rml.treaty_type_id
            END,
            ral.line_limit DESC

   Open Line_Cursor
   Fetch Next From Line_Cursor
     Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit, @treaty_id, @type,
          @ri_model_line_id, @retained_share, @Is_Obligatory, @lower_limit, @premium_percent,
          @Grouping, @treaty_type_id, @is_edited_line, @is_manually_added,
          @reinsurance_type_id, @is_premium_edited_line

   Select @first_priority=@priority, @Running_si=@grossnet_SI,
          @running_premium=@grossnet_premium,
          @priority_si=@grossnet_SI, @priority_premium=@grossnet_premium

   While (@@Fetch_Status=0)
   Begin
     -- Skip edited lines - preserve user overrides
     IF @is_edited_line=1 OR @is_premium_edited_line=1
     BEGIN
       SELECT @this_si=ISNULL(sum_insured,0), @this_premium=ISNULL(premium_value,0)
       FROM ri_arrangement_line WHERE ri_arrangement_line_id=@line_id
       SELECT @this_si=ABS(@this_si), @this_premium=ABS(@this_premium)
       SELECT @priority_allocated_si=ISNULL(@priority_allocated_si,0)+@this_si,
              @priority_allocated_premium=ISNULL(@priority_allocated_premium,0)+@this_premium
       SELECT @running_si=@running_si-@this_si, @running_premium=@running_premium-@this_premium
       Fetch Next From Line_Cursor
       Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit, @treaty_id, @type,
            @ri_model_line_id, @retained_share, @Is_Obligatory, @lower_limit, @premium_percent,
            @Grouping, @treaty_type_id, @is_edited_line, @is_manually_added,
            @reinsurance_type_id, @is_premium_edited_line
       CONTINUE
     END

     -- manually_added: arrangement_copy already set correct signed values; just account for them
     -- in running totals and skip the UPDATE to avoid overwriting with wrong sign
     IF ISNULL(@is_manually_added,0)=1
     BEGIN
       SELECT @this_si=ISNULL(sum_insured,0), @this_premium=ISNULL(premium_value,0)
       FROM ri_arrangement_line WHERE ri_arrangement_line_id=@line_id
       -- ABS for running totals (band_si/premium are ABS'd when reverse_Values=1)
       SELECT @this_si=ABS(@this_si), @this_premium=ABS(@this_premium)
       SELECT @priority_allocated_si=ISNULL(@priority_allocated_si,0)+@this_si,
              @priority_allocated_premium=ISNULL(@priority_allocated_premium,0)+@this_premium
       SELECT @running_si=@running_si-@this_si, @running_premium=@running_premium-@this_premium
       Fetch Next From Line_Cursor
       Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit, @treaty_id, @type,
            @ri_model_line_id, @retained_share, @Is_Obligatory, @lower_limit, @premium_percent,
            @Grouping, @treaty_type_id, @is_edited_line, @is_manually_added,
            @reinsurance_type_id, @is_premium_edited_line
       CONTINUE
     END

     -- Skip QSR nodes - handled by QSR split after the loop
     IF @reinsurance_type_id = 14
     BEGIN
       Fetch Next From Line_Cursor
       Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit, @treaty_id, @type,
            @ri_model_line_id, @retained_share, @Is_Obligatory, @lower_limit, @premium_percent,
            @Grouping, @treaty_type_id, @is_edited_line, @is_manually_added,
            @reinsurance_type_id, @is_premium_edited_line
       CONTINUE
     END

     SELECT @is_variableQuotaShare=ISNULL(Is_VariableQuotaShare,0) FROM ri_model_line
     WHERE ri_model_id=@ri_model_id AND ri_model_line_id=@ri_model_line_id

     IF @is_variableQuotaShare=1
     Begin
       IF @type='T' AND @treaty_id IS NOT NULL
       BEGIN
         EXEC spu_GetRIModelVariableQuotaSharePercent_RI2007
              @treaty_id=@treaty_id, @ri_model_line_id=@ri_model_line_id,
              @sum_insured=@band_si, @ri_arrangement_line_id=@line_id
         SELECT @default_percent=default_share_percent/100 FROM ri_arrangement_line WHERE ri_arrangement_line_id=@line_id
       END
     End

     If IsNull(@last_priority,-666)<>@priority
     Begin
       Select @last_priority=@priority, @priority_limit=@line_limit,
              @priority_si=@running_si, @priority_premium=@running_premium, @QsTotal=0
       If ISNULL(@line_limit,0)=0 SELECT @priority_limit=@grossnet_SI
       IF @Extended_Limits_Enabled=1 AND ISNULL(@Extended_Limits_Amount,0)>0
         IF @priority_limit>ISNULL(@Extended_Limits_Amount,0)
           SELECT @priority_limit=ISNULL(@Extended_Limits_Amount,0)
     End

     IF ISNULL(@treaty_type_id,0)=1 AND ISNULL(@is_manually_added,0)=0 AND @reinsurance_type_id <> 14
     BEGIN
       Select @this_si=@priority_si*@default_percent
       IF @this_si>(@priority_limit*@number_of_lines*@default_percent)
         SELECT @this_si=@priority_limit*@number_of_lines*@default_percent
       IF ISNULL(@grossnet_SI,0)<>0
         SELECT @this_premium=@grossnet_premium*(ISNULL(@this_si,0)/ISNULL(@grossnet_SI,0))
       ELSE SELECT @this_premium=0
       If (@this_si>0) Or (@this_premium>0)
         Select @priority_allocated_si=ISNULL(@priority_allocated_si,0)+@this_si,
                @priority_allocated_premium=ISNULL(@priority_allocated_premium,0)+@this_premium
       If @Type='T' SELECT @QsTotal=ISNULL(@QsTotal,0)+@this_si
       -- R: SI/this_share_percent calculated above; zero premium so remainder logic sets it
       IF @type='R' SELECT @this_premium=0
     END
     ELSE IF (ISNULL(@treaty_type_id,0)=2 OR (@treaty_type_id IS NULL AND @type IN ('TX','TC','PX'))) AND ISNULL(@is_manually_added,0)=0
     BEGIN
       IF @calc_original=0
       BEGIN
         SELECT @ceding_rate=ceding_rate/100 FROM ri_model_line WHERE ri_model_id=@ri_model_id AND ri_model_line_id=@ri_model_line_id
         UPDATE ri_arrangement_line SET default_share_percent=ISNULL(@ceding_rate,0)*100 WHERE ri_arrangement_line_id=@line_id
       END
       ELSE
         SELECT @ceding_rate=default_share_percent/100 FROM ri_arrangement_line WHERE ri_arrangement_line_id=@line_id
       SELECT @cede_premium_only=ISNULL(cede_premium_only,0) FROM ri_model_line WHERE ri_model_line_id=@ri_model_line_id
       SET @this_si=0
       IF @cede_premium_only=0 AND @type<>'TC'
       BEGIN
         IF @priority_limit-@QsTotal<@line_limit AND @QsTotal<>0 SELECT @line_limit=@priority_limit-@QsTotal
         IF ABS(@priority_si-@QsTotal)>@lower_limit AND ABS(@priority_si-@QsTotal)<=@line_limit
           SELECT @this_si=Round((@priority_si-@QsTotal-(@lower_limit*@negative_si)),2)
         IF ABS(@priority_si-@QsTotal)>@line_limit
           SELECT @this_si=Round((@line_limit-@lower_limit)*@negative_si,2)
       END
       IF @version_id>1 AND @is_original=1 SELECT @this_premium=0
       ELSE SELECT @this_premium=@priority_premium*ISNULL(@ceding_rate,0)
     END

     Update ri_arrangement_line
     Set this_share_percent=
           Case When @band_si=0 Then 0
                When ISNULL(@treaty_type_id,0)=1 AND @grossnet_SI<>0 Then (Convert(float,ABS(@this_si))/@grossnet_SI)*100
                Else (Convert(float,ABS(@this_si))/@band_si)*100 End,
         premium_percent=CASE WHEN ISNULL(@band_premium,0)=0 THEN 0 ELSE (CONVERT(float,ABS(ISNULL(@this_premium,0)))/ABS(@band_premium))*100 END,
         sum_insured=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -@this_si ELSE @this_si END,
         premium_value=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -ISNULL(@this_premium,0) ELSE ISNULL(@this_premium,0) END,
         commission_value=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -ISNULL(@this_premium,0)*(commission_percent/100) ELSE ISNULL(@this_premium,0)*(commission_percent/100) END
     Where ri_arrangement_line_id=@line_id

     Select @running_si=@running_si-@this_si, @running_premium=@running_premium-@this_premium

     Fetch Next From Line_Cursor
     Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit, @treaty_id, @type,
          @ri_model_line_id, @retained_share, @Is_Obligatory, @lower_limit, @premium_percent,
          @Grouping, @treaty_type_id, @is_edited_line, @is_manually_added,
          @reinsurance_type_id, @is_premium_edited_line
   End

   -- Set retained premium from remainder (@running_premium = what's left after all non-R lines)
   IF @ret_line_id IS NOT NULL
   BEGIN
     SELECT @this_premium=CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -@running_premium ELSE @running_premium END
     UPDATE ri_arrangement_line
     SET premium_value=ISNULL(@this_premium,0),
         premium_percent=CASE WHEN ISNULL(@band_premium,0)=0 THEN 0 ELSE (CONVERT(float,ABS(ISNULL(@this_premium,0)))/ABS(@band_premium))*100 END,
         commission_value=ISNULL(@this_premium,0)*(ISNULL(commission_percent,0)/100)
     WHERE ri_arrangement_line_id=@ret_line_id AND ISNULL(is_edited,0)=0
   END

   -- Split RET SI/Premium across QSR lines based on each QSR's default_share_percent.
   -- If a QSR line is edited (is_edited=1), preserve its persisted values.
   -- RET net = original RET - sum(QSR), guaranteeing RET + sum(QSR) = full retention pool.
   IF EXISTS (SELECT 1 FROM ri_arrangement_line ral
              INNER JOIN treaty t ON ral.treaty_id=t.treaty_id
              WHERE ral.ri_arrangement_id=@ri_arrangement_id
              AND t.reinsurance_type_id=14)
   BEGIN
     DECLARE @ret_si NUMERIC(19,5), @ret_premium MONEY
     DECLARE @total_qsr_si NUMERIC(19,5) = 0, @total_qsr_premium MONEY = 0
     DECLARE @qsr_line_id INT, @qsr_default_perc FLOAT, @qsr_is_edited BIT
     DECLARE @qsr_si NUMERIC(19,5), @qsr_premium MONEY

     SELECT @ret_si = sum_insured, @ret_premium = premium_value
     FROM ri_arrangement_line WHERE ri_arrangement_line_id = @ret_line_id

     DECLARE QSR_Cursor CURSOR FAST_FORWARD FOR
       SELECT ral.ri_arrangement_line_id, ral.default_share_percent, ISNULL(ral.is_edited, 0)
       FROM ri_arrangement_line ral
       INNER JOIN treaty t ON ral.treaty_id=t.treaty_id
       WHERE ral.ri_arrangement_id = @ri_arrangement_id
         AND t.reinsurance_type_id = 14

     OPEN QSR_Cursor
     FETCH NEXT FROM QSR_Cursor INTO @qsr_line_id, @qsr_default_perc, @qsr_is_edited

     WHILE @@FETCH_STATUS = 0
     BEGIN
       IF @qsr_is_edited = 1
       BEGIN
         -- Preserve user-entered values; just accumulate for RET deduction
         SELECT @qsr_si = ISNULL(sum_insured, 0), @qsr_premium = ISNULL(premium_value, 0)
         FROM ri_arrangement_line WHERE ri_arrangement_line_id = @qsr_line_id
         SELECT @qsr_si = ABS(@qsr_si), @qsr_premium = ABS(@qsr_premium)
       END
       ELSE
       BEGIN
         SELECT @qsr_si    = ROUND(ABS(@ret_si)    * @qsr_default_perc / 100, 2)
         SELECT @qsr_premium = ROUND(ABS(@ret_premium) * @qsr_default_perc / 100, 2)
         UPDATE ri_arrangement_line
         SET sum_insured     = CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -@qsr_si    ELSE @qsr_si    END,
             premium_value   = CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -@qsr_premium ELSE @qsr_premium END,
             commission_value= CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -@qsr_premium ELSE @qsr_premium END * (ISNULL(commission_percent,0)/100)
         WHERE ri_arrangement_line_id = @qsr_line_id
       END
       SELECT @total_qsr_si = @total_qsr_si + @qsr_si,
              @total_qsr_premium = @total_qsr_premium + @qsr_premium

       FETCH NEXT FROM QSR_Cursor INTO @qsr_line_id, @qsr_default_perc, @qsr_is_edited
     END
     CLOSE QSR_Cursor
     DEALLOCATE QSR_Cursor

     -- Reduce RET by total QSR so that RET + sum(QSR) = full retention pool
     UPDATE ri_arrangement_line
     SET sum_insured   = CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(ABS(@ret_si)    - @total_qsr_si)    ELSE ABS(@ret_si)    - @total_qsr_si    END,
         premium_value = CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(ABS(@ret_premium) - @total_qsr_premium) ELSE ABS(@ret_premium) - @total_qsr_premium END,
         commission_value = (CASE WHEN ISNULL(@reverse_Values,0)=1 THEN -(ABS(@ret_premium) - @total_qsr_premium) ELSE ABS(@ret_premium) - @total_qsr_premium END) * (ISNULL(commission_percent,0)/100)
     WHERE ri_arrangement_line_id = @ret_line_id --AND ISNULL(is_edited,0) = 0
   END

   DECLARE @iTreatyPremiumType int
   SELECT @iTreatyPremiumType=ISNULL(treaty_premium_type,0) FROM RI_Model WHERE ri_model_id=@ri_model_id
   IF @iTreatyPremiumType=1 EXEC spu_RI_treaty_premium_calc_RI2007 @ri_arrangement_id

   Close Line_Cursor
   Deallocate Line_Cursor
 END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
