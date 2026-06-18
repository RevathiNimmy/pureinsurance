Execute DDLDropProcedure 'spu_calculate_claims_ri_method_2_full'
GO

CREATE PROCEDURE spu_calculate_claims_ri_method_2_full    
    @claim_id int,    
    @ri_arrangement_id int,    
    @total_reserve money,    
    @total_payment money,    
    @Reapply_TX int = 0,    
    @Recovery tinyint=2,    
    @Reapply_Treaty int = 0,    
    @is_created int= 0    
AS    
     DECLARE    
        @line_id int,    
        @sum_insured money,    
        @this_reserve money,    
        @this_payment money,    
        @os_reserve money,    
        @os_payment money,    
        @ri_type VARCHAR(3),    
        @product_option VARCHAR(20),    
        @lower_limit money,    
        @line_limit money,    
        @remaining_limit money,    
        @default_share_percent Numeric(19,8) ,    
        @Gross_SumInsured float,    
        @Net_SumInsured float,    
        @os_SumInsured float,    
        @this_SumInsured money,    
        @Reserve money,    
        @Payment money,    
        @Gross_Reserve_to_date money,    
        @Gross_This_reserve money,    
        @Gross_Net_Reserve money,    
        @Gross_Net_Payment money,    
        @Gross_Payment_to_date money,    
        @Gross_This_Payment money,    
        @total_reserve_used money,    
        @total_payment_done money,    
        @this_reserve_used money,    
        @this_payment_done money,    
  @treaty_reserve money,    
  @treaty_payment money,    
  @this_share_percent float,    
  @Recovery_payment money,    
  @retained money,    
  @FACRetained money,    
  @retained_reserve money,    
  @retained_payment money,    
  @is_obligatory tinyint,    
  @obligatory_reserve money,    
  @obligatory_payment money,    
  @obligatory_thisreserve money,    
  @obligatory_thispayment money,    
  @FACTotalReserve money,    
  @FACTotalPayment money,    
  @extended_limit_amount money,    
  @extended_limit_Enabled INT,    
  @tx_SumInsured money,    
  @allocated_tx_SumInsured money,    
  @grouping INT, @old_grouping INT,    
  @QS_Total_SI money,    
  @ri_band_id INT,    
  @prop_ri_calculation_method INT,    
  @risk_cnt INT,    
  @insurance_file_cnt INT,    
  @Limit_Effective_Date DATE,    
  @ri_model_id INT,    
  @xol_ri_model_id INT,    
  @NetFAC_SumInsured Money,    
  @NetTreatyReserve Money  ,    
  @NetTreatyPayment Money,    
  @GrossNetTotalReserve Money,    
  @GrossNetTotalPayment Money,    
  @GrossReserve Money,    
  @GrossPayment Money,    
  @FACSummaryReserve MONEY,    
  @FACSummaryPayment MONEY,    
  @flag tinyint,    
  @recovery_to_date MONEY,    
  @FACPTSummaryReserve MONEY  ,    
  @FACPTSummaryPayment MONEY  ,    
  @TFS_Total_SI money,    
  @IsPortfolioTransferred tinyint,    
  @extended_limit_amountXOL Money  ,  
  @cover_start_date_ForRi DATETIME   
    
  SELECT @old_grouping=0, @flag=0  ,@IsPortfolioTransferred=0    
    
  SELECT  @Insurance_file_cnt = Policy_id from claim where claim_id = @claim_id    
  SELECT @cover_start_date_ForRi = inception_date_tpi          
  FROM insurance_file  (NOLOCK)          
   WHERE Insurance_file_cnt = @Insurance_file_cnt    
  
  If Exists( Select NULL from Claim_pt_log CPT inner join claim clm on CPT.base_claim_id = clm.base_claim_id where clm.Claim_id =@claim_id )    
 SELECT @IsPortfolioTransferred = 1    
    
  SELECT @GrossNetTotalReserve = ISNULL(Reserve_to_date,0)+ISNULL(this_reserve,0),    
  @GrossNetTotalPayment=ISNULL(payment_to_date,0)+ISNULL(this_payment,0)+ISNULL(salvage_to_date,0)+ISNULL(this_salvage,0)+ISNULL(recovery_to_date,0)+ISNULL(this_recovery,0)    
  from Claim_RI_Arrangement    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id    
    
  SELECT @GrossReserve=@GrossNetTotalReserve, @GrossPayment=@GrossNetTotalPayment    
    
  SELECT @FACSummaryReserve =ISNULL(SUM(ISNULL(Reserve_to_date,0)),0),    
  @FACSummaryPayment=ISNULL(SUM(ISNULL(payment_to_date,0)),0) - ISNULL(SUM(ISNULL(salvage_to_date,0)),0)- ISNULL(SUM(ISNULL(recovery_to_date,0)),0)    
  from Claim_RI_Arrangement_Line    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id    
  AND type ='F'    
    
  SELECT @GrossNetTotalReserve = @GrossNetTotalReserve - ISNULL(SUM(ISNULL(Reserve_to_date,0)),0),    
  @GrossNetTotalPayment=@GrossNetTotalPayment - ISNULL(SUM(ISNULL(payment_to_date,0)),0) - ISNULL(SUM(ISNULL(salvage_to_date,0)),0)- ISNULL(SUM(ISNULL(recovery_to_date,0)),0)    
  from Claim_RI_Arrangement_Line    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id and is_pt_archive=0    
  AND type in ('F','FX') and retained<>1    
    
  SELECT @FACPTSummaryReserve = ISNULL(SUM(ISNULL(reserve_to_date,0)),0) ,    
@FACPTSummaryPayment=ISNULL(SUM(ISNULL(claim_incurred_to_date,0)),0)    
  from Claim_RI_Arrangement_Line    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id and is_pt_archive=1    
  AND type in ('F')    
    
  SELECT @extended_limit_Enabled=ISNULL(Is_extended_limit_applied,0),@extended_limit_amount=ISNULL(ra.Extended_limit_amount,0)    
  ,@ri_band_id=cra.ri_band_id , @risk_cnt = ra.risk_cnt  from RI_Arrangement ra join Claim_RI_Arrangement cra    
  on ra.ri_arrangement_id=cra.original_ri_arrangement_id    
  WHERE cra.claim_ri_arrangement_id=@ri_arrangement_id and cra.claim_id =@claim_id    
    
  SELECT top 1 @prop_ri_calculation_method= Proportional_RI_Cal_Method    
  from RI_Band_Version    
  where ri_band_id  = @ri_band_id    
  AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)        
  ORDER BY effective_date DESC  
    
  --IF Model doesnt have QS or Surplus then use old calc logic    
  IF NOT EXISTS(SELECT NULL FROM Claim_RI_Arrangement_line WHERE ri_arrangement_id=@ri_arrangement_id AND type in ('T','TFS'))    
  SET @prop_ri_calculation_method=1    
    
  IF @prop_ri_calculation_method = 2 AND @extended_limit_Enabled =1    
  BEGIN    
 SELECT @insurance_file_cnt = insurance_file_cnt  from insurance_file_risk_link where risk_cnt = @risk_cnt    
 SELECT @Limit_Effective_Date = GETDATE()    
 EXECUTE Spu_get_ri_values    
   @insurance_file_cnt = @insurance_file_cnt,    
   @risk_cnt = @risk_cnt,    
   @effective_date = @Limit_Effective_Date,    
   @value = @extended_limit_amount OUTPUT    
  END    
 UPDATE Claim_RI_Arrangement SET extended_limit_amount =@extended_limit_amount WHERE claim_id =@claim_id and ri_arrangement_id=@ri_arrangement_id    
    
  IF @extended_limit_Enabled=1 and ISNULL(@extended_limit_amount,0)>0    
     UPDATE Claim_RI_Arrangement_Line SET line_limit=ISNULL(@extended_limit_amount,0) WHERE TYPE IN ('TFS','T') AND  ri_arrangement_id=@ri_arrangement_id    
    Select  @os_reserve = ISNULL(@total_reserve,0),    
            @os_payment = ISNULL(@total_payment,0),    
      @Gross_Net_Reserve= ISNULL(@total_reserve,0),    
      @Gross_Net_Payment= ISNULL(@total_Payment,0)    
    
    Set @total_reserve_used = 0    
    Set @total_payment_done = 0    
    Set @this_reserve_used = 0    
    Set @this_payment_done = 0    
    
    SELECT @FACTotalReserve = 0, @FACTotalPayment=0    
  SELECT @product_option='0'    
  SELECT @product_option=ISNULL(value,0) FROM Hidden_Options WHERE option_number=88    
  SELECT @Gross_Reserve_to_date = Sum(ISNULL(reserve,0)) from CLAIM_RI_ARRANGEMENT_LINE    
  WHERE Claim_id = @Claim_id AND     ri_arrangement_id = @ri_arrangement_id and not (Type='FX' and retained=1)    
  SELECT @Gross_Payment_to_date = ISNULL(Sum(Payment),0)+ (ISNULL(sum(Salvage),0)+ISNULL(sum(Recovery),0)),    
  @Recovery_payment = (ISNULL(sum(Salvage),0)+ISNULL(sum(Recovery),0))    
  FROM CLAIM_RI_ARRANGEMENT_LINE    
  WHERE Claim_id = @Claim_id  AND     ri_arrangement_id = @ri_arrangement_id and not (Type='FX' and retained=1)    
  SELECT @Gross_This_Reserve = @Total_reserve - @Gross_Reserve_to_date    
  SELECT @Gross_This_Payment = @Total_Payment - @Gross_Payment_to_date    
  --Use Table variable instead of temp tables (RC) PN 39575    
  --Calculate the Sort Order of RI Lines    
  --Sort Order Set According to Type (F, FX, T, TX, R )    
--Create table #temp( RIType varchar(2), RIPriority int)    
    
DECLARE @temp TABLE( RIType varchar(3), RIPriority int)    
   Insert into @temp(RItype,RIPriority) values('F',1)    
   Insert into @temp(RItype,RIPriority) values('FX',2)    
   Insert into @temp(RItype,RIPriority) values('TFS',3)    
   Insert into @temp(RItype,RIPriority) values('T',4)    
   Insert into @temp(RItype,RIPriority) values('TX',5)    
   Insert into @temp(RItype,RIPriority) values('TC',6)    
   Insert into @temp(RItype,RIPriority) values('R',7)    
    
  IF @product_option='0'    
 BEGIN    
         DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR    
      SELECT  ri_arrangement_line_id,    
          ISNULL(sum_insured,0),    
          type,    
          ISNULL(lower_limit,0),    
          ISNULL(line_limit,0),    
          Reserve,    
          Payment,    
          ISNULL(default_share_percent,0),    
     ISNULL(this_share_percent,0),    
    ISNULL(is_obligatory,0),    
 ISNULL(recovery_to_date,0)    
      FROM    claim_ri_arrangement_line    
             WHERE   claim_id = @claim_id    
             AND     ri_arrangement_id = @ri_arrangement_id    
             AND     type IN ('R', 'T', 'F')    
             ORDER BY    
                     Is_obligatory,priority ASC, number_of_lines ASC    
    END    
  ELSE    
 BEGIN    
    CREATE TABLE #RICursor    
     (RI_id INT IDENTITY(1,1),    
     ri_arrangement_line_id INT,    
     sum_insured MONEY,    
     type varchar(3),    
     lower_limit MONEY,    
     line_limit MONEY,    
     Reserve    MONEY,    
     Payment    MONEY,    
     default_share_percent FLOAT,    
     this_share_percent FLOAT,    
  is_obligatory tinyint,    
  grouping INT,    
  recovery_to_date MONEY)    
  IF @Reapply_TX=1  OR @Reapply_Treaty=1    
  BEGIN    
   Select @Gross_SumInsured = ISNULL(Sum_insured,0)    
    FROM Claim_ri_Arrangement    
    WHERE Claim_id=@Claim_id    
    AND ri_arrangement_id=  @ri_arrangement_id    
   Select @Net_SumInsured = @Gross_SumInsured - Sum(ISNULL(Sum_insured,0)) From Claim_ri_Arrangement_line    
    WHERE Claim_id=@Claim_id  AND ri_arrangement_id=  @ri_arrangement_id    
    AND  Type in ('F','FX') and isnull(retained,0)=0    
    SET @os_SumInsured=ISNULL(@Net_SumInsured,@Gross_SumInsured)    
   IF @Reapply_Treaty=1    
  BEGIN    
   UPDATE  claim_ri_arrangement_line    
   SET     Sum_Insured = CASE WHEN @os_SumInsured*default_share_percent*0.01 <= ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01    
       THEN @os_SumInsured*default_share_percent*0.01 ELSE ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01 END    
   WHERE   claim_id = @claim_id  and ri_arrangement_id=@ri_arrangement_id    
   AND     type ='T' AND ISNULL(is_pt_archive,0)=0    
  END    
  ELSE    
  BEGIN    
   UPDATE  claim_ri_arrangement_line    
   SET     Sum_Insured = @os_SumInsured*default_share_percent*0.01    
   WHERE   claim_id = @claim_id  and ri_arrangement_id=@ri_arrangement_id    
   AND     type ='T' AND ISNULL(is_pt_archive,0)=0    
  END    
  If ISNULL(@os_SumInsured,0)>0    
  BEGIN    
   UPDATE  claim_ri_arrangement_line    
   SET     this_share_percent=(Sum_Insured/@os_SumInsured)*100.00000000    
  WHERE   claim_id = @claim_id  and ri_arrangement_id=@ri_arrangement_id    
  AND     type ='T' AND ISNULL(is_pt_archive,0)=0    
  END    
  SELECT @QS_Total_SI = ISNULL(Sum(Sum_insured),0)    
  FROM    claim_ri_arrangement_line    
  WHERE   claim_id = @claim_id    
  AND     ri_arrangement_id = @ri_arrangement_id    
  AND     type ='T' AND ISNULL(is_pt_archive,0)=0    
  SELECT @os_SumInsured=@os_SumInsured-@QS_Total_SI    
    
  Select @ri_model_id = ri_model_id,@xol_ri_model_id=xol_ri_model_id from Claim_RI_Arrangement where Claim_id=@Claim_id  AND ri_arrangement_id=@ri_arrangement_id    
  IF @ri_model_id<>@xol_ri_model_id    
  BEGIN    
      SELECT @extended_limit_amountXOL=extended_limit_amount  from Claim_RI_Arrangement where Claim_id=(select base_claim_id from claim where claim_id=@claim_id)    
      IF @Reapply_Treaty =0    
      BEGIN    
  SELECT @TFS_Total_SI = ISNULL(Sum(Sum_insured),0)    
  FROM    claim_ri_arrangement_line    
 WHERE   claim_id = @claim_id    
  AND     ri_arrangement_id = @ri_arrangement_id    
  AND     type ='TFS' AND ISNULL(is_pt_archive,0)=0    
  SELECT @os_SumInsured=@os_SumInsured- @TFS_Total_SI    
      END    
   END    
   ELSE    
   BEGIN    
   SELECT @extended_limit_amountXOL=@extended_limit_amount    
   END    
    
  DECLARE Update_TX Cursor for    
  SELECT  Claim_RI_Arrangement_Line.ri_arrangement_line_id,    
       ISNULL(Claim_RI_Arrangement_Line.sum_insured,0),    
       Claim_RI_Arrangement_Line.type,    
       ISNULL(Claim_RI_Arrangement_Line.lower_limit,0),    
       ISNULL(Claim_RI_Arrangement_Line.line_limit,0),    
       ISNULL(Claim_RI_Arrangement_Line.default_share_percent,0)    
              FROM    Claim_RI_Arrangement_Line    
              INNER JOIN  Claim_RI_Arrangement CRA ON CRA.ri_arrangement_id = Claim_RI_Arrangement_Line.ri_arrangement_id    
     INNER JOIN RI_Model_Line ON (RI_Model_Line.ri_model_id  = CRA.xol_ri_model_id and Claim_RI_Arrangement_Line.treaty_id =RI_Model_Line.treaty_id )    
              WHERE   Claim_RI_Arrangement_Line.claim_id = @claim_id    
              AND     Claim_RI_Arrangement_Line.ri_arrangement_id = @ri_arrangement_id    
              AND     Claim_RI_Arrangement_Line.type IN ('R', 'TX')    
              AND   ISNull(RI_Model_Line.cede_premium_only, 0) = 0    
              ORDER BY    
                      Claim_RI_Arrangement_Line.type DESC, ISNULL(Claim_RI_Arrangement_Line.line_limit,0) DESC    
  OPEN Update_TX    
   FETCH NEXT FROM Update_TX    
   INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit,@default_share_percent    
  WHILE @@FETCH_STATUS = 0    
  BEGIN    
    IF @ri_type='TX'    
  BEGIN    
   If @extended_limit_Enabled = 1 and ISNULL(@extended_limit_amountXOL,0)>0 and @extended_limit_amountXOL < @os_SumInsured    
    BEGIN    
     Set @tx_SumInsured  = @extended_limit_amountXOL -@QS_Total_SI    
     IF @tx_SumInsured>@lower_limit    
      BEGIN    
       IF @tx_SumInsured>@line_limit    
        SELECT @this_SumInsured = @line_limit - @lower_limit    
       ELSE    
        SELECT @this_SumInsured = (@tx_SumInsured) - @lower_limit    
      END    
    END    
   ELSE    
    BEGIN    
     IF @os_SumInsured>@lower_limit    
      BEGIN    
       IF @os_SumInsured>@line_limit    
         SELECT @this_SumInsured = @line_limit - @lower_limit    
       ELSE    
        SELECT @this_SumInsured = @os_SumInsured - @lower_limit    
      END    
    END    
   SELECT @os_SumInsured = @os_SumInsured - ISNULL(@this_SumInsured,0)    
   SELECT @allocated_tx_SumInsured = ISNULL(@allocated_tx_SumInsured,0) + ISNULL(@this_SumInsured,0)    
  END    
    IF @ri_type='R'    
      IF @os_SumInsured>0    
      BEGIN    
  IF @extended_limit_Enabled = 1 and @extended_limit_amount>0 and @extended_limit_amount<@os_SumInsured    
  BEGIN    
   IF (@extended_limit_amount - @allocated_tx_SumInsured)>@line_limit    
     SELECT @this_SumInsured = @line_limit    
   ELSE    
     SELECT @this_SumInsured = (@extended_limit_amount - @allocated_tx_SumInsured-@QS_Total_SI )    
  END    
  Else    
  BEGIN    
        IF @os_SumInsured>@line_limit    
          SELECT @this_SumInsured = @line_limit    
        ELSE    
          SELECT @this_SumInsured = @os_SumInsured    
         END    
        SELECT @os_SumInsured = @os_SumInsured - ISNULL(@this_SumInsured,0)    
      END    
      If  @ri_type IN ('TX','R')    
        UPDATE  claim_ri_arrangement_line    
        SET     Sum_Insured = isnull(@this_SumInsured,0)    
        WHERE   claim_id = @claim_id    
        AND     ri_arrangement_line_id = @line_id    
    FETCH NEXT FROM Update_TX    
               INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit,@default_share_percent    
    END    
    IF (@Reapply_TX=1 OR @Reapply_Treaty=1) AND ISNULL(@os_SumInsured,0)>0    
    BEGIN    
  UPDATE  claim_ri_arrangement_line    
  SET     Sum_Insured = CASE WHEN @os_SumInsured <= ISNULL(line_limit,0) * ISNULL(number_of_lines,0)    
       THEN @os_SumInsured ELSE ISNULL(line_limit,0) * ISNULL(number_of_lines,0) END    
  WHERE   claim_id = @claim_id  and ri_arrangement_id=@ri_arrangement_id    
  AND     type ='TFS' AND ISNULL(is_pt_archive,0)=0    
    
  IF EXISTS(SELECT NULL FROM Claim_RI_Arrangement WHERE claim_id=@claim_id AND ri_arrangement_id=@ri_arrangement_id AND ri_model_id<>xol_ri_model_id)    
  BEGIN    
            BEGIN    
  UPDATE  claim_ri_arrangement_line    
            SET     Sum_Insured =(ISNULL(@Net_SumInsured,@Gross_SumInsured)-line_limit)    
            WHERE   claim_id = @claim_id  and ri_arrangement_id=@ri_arrangement_id    
            AND     type ='TFS' AND ISNULL(is_pt_archive,0)=0    
            AND     sum_insured>((ISNULL(@Net_SumInsured,@Gross_SumInsured)-line_limit))    
            AND ISNULL(number_of_lines,0) >1 AND ISNULL(@Net_SumInsured,@Gross_SumInsured)>line_limit    
    
            END    
  END    
  IF ISNULL(@Net_SumInsured,@Gross_SumInsured) > 0    
  BEGIN    
  UPDATE  claim_ri_arrangement_line    
  SET     this_share_percent=(Sum_Insured/ISNULL(@Net_SumInsured,@Gross_SumInsured))*100.00000000    
  WHERE   claim_id = @claim_id  and ri_arrangement_id=@ri_arrangement_id    
  AND     type ='TFS' AND ISNULL(is_pt_archive,0)=0    
  END    
  IF isnull(@os_SumInsured,0) > 0    
  BEGIN    
  SELECT @os_SumInsured = @os_SumInsured - (Sum_Insured) FROM claim_ri_arrangement_line WHERE   claim_id = @claim_id    
  AND     ri_arrangement_id = @ri_arrangement_id    
  AND     type ='TFS' AND ISNULL(is_pt_archive,0)=0    
  END    
  END    
    UPDATE  claim_ri_arrangement_line    
        SET     Sum_Insured =Sum_Insured+ isnull(@os_SumInsured,0)    
        WHERE   claim_id = @claim_id and type ='R' and ri_arrangement_id=@ri_arrangement_id    
    Close Update_TX    
    Deallocate Update_TX    
 END    
IF @Gross_This_Reserve = 0 And @Gross_this_payment = 0    
Return    
IF @Gross_This_Reserve < 0 or @Gross_this_payment < 0  OR @Recovery = 1 OR @Recovery = 0    
BEGIN    
 EXEC spu_calculate_claims_ri_method_Negative_Reserve    
    @claim_id = @claim_id,    
    @ri_arrangement_id = @ri_arrangement_id,    
    @total_reserve = @Total_reserve ,    
    @total_payment = @Total_Payment,    
    @Recovery=@Recovery    
    
    Return    
END    
   INSERT INTO #RICursor    
   (ri_arrangement_line_id ,    
     sum_insured ,    
     type ,    
     lower_limit ,    
     line_limit ,    
     Reserve    ,    
     Payment    ,    
     default_share_percent ,    
     this_share_percent ,    
  is_obligatory ,    
  grouping, recovery_to_date    
  )    
            SELECT ri_arrangement_line_id,    
        ISNULL(sum_insured,0),    
        type,    
        ISNULL(lower_limit,0),    
        ISNULL(line_limit,0),    
        ISNULL(Reserve,0),    
        ISNULL(Payment,0),    
        ISNULL(default_share_percent,0),    
  ISNULL(this_share_percent,0),ISNULL(is_obligatory,0)  , ISNULL(grouping,ri_arrangement_line_id) ,    
  ISNULL(recovery,0)+ISNULL(salvage,0)    
            FROM    claim_ri_arrangement_line    
            WHERE   claim_id = @claim_id    
            AND     ri_arrangement_id = @ri_arrangement_id    
            AND type IN ('T') AND is_obligatory = 1  AND ISNULL(is_pt_archive,0)=0    
            ORDER BY    
                    priority ASC, number_of_lines ASC    
   INSERT INTO #RICursor    
      (ri_arrangement_line_id ,    
     sum_insured ,    
     type ,    
     lower_limit ,    
     line_limit ,    
     Reserve    ,    
     Payment    ,    
     default_share_percent ,    
     this_share_percent ,    
  is_obligatory ,    
  grouping, recovery_to_date )    
            SELECT ri_arrangement_line_id,    
        ISNULL(sum_insured,0),    
        type,    
        ISNULL(lower_limit,0),    
        ISNULL(line_limit,0),    
        ISNULL(Reserve,0),    
 ISNULL(Payment,0) + ISNULL(salvage,0) + ISNULL(recovery,0),    
        ISNULL(default_share_percent,0),    
        ISNULL(this_share_percent,0),ISNULL(is_obligatory,0), ri_arrangement_line_id  ,    
        ISNULL(recovery,0)+ISNULL(salvage,0)    
            FROM    claim_ri_arrangement_line    
            WHERE   claim_id = @claim_id    
            AND     ri_arrangement_id = @ri_arrangement_id    
            AND     type IN ('F')  AND ISNULL(is_pt_archive,0)=0    
            ORDER BY    
                    priority ASC, number_of_lines ASC    
   INSERT INTO #RICursor    
      (ri_arrangement_line_id ,    
     sum_insured ,    
     type ,    
     lower_limit ,    
     line_limit ,    
     Reserve    ,    
     Payment    ,    
     default_share_percent ,    
     this_share_percent ,    
  is_obligatory ,    
  grouping,recovery_to_date )    
            SELECT  ri_arrangement_line_id,    
        ISNULL(sum_insured,0),    
        type,    
        ISNULL(lower_limit,0),    
        ISNULL(line_limit,0),    
        ISNULL(Reserve,0),    
 ISNULL(Payment,0) + ISNULL(salvage,0) + ISNULL(recovery,0),    
        ISNULL(default_share_percent,0),    
  ISNULL(this_share_percent,0),ISNULL(is_obligatory,0) , ISNULL(grouping,ri_arrangement_line_id)  ,    
  ISNULL(recovery,0)+ISNULL(salvage,0)    
            FROM    claim_ri_arrangement_line    
            WHERE   claim_id = @claim_id    
            AND     ri_arrangement_id = @ri_arrangement_id    
            AND     type IN ('FX', 'T','TFS')    AND ISNULL(is_obligatory,0)=0  AND ISNULL(is_pt_archive,0)=0    
     and     isnull(retained,0)=0    
       ORDER BY    
                    type ASC, line_limit    
   INSERT INTO #RICursor    
      (ri_arrangement_line_id ,    
     sum_insured ,    
     type ,    
     lower_limit ,    
     line_limit ,    
     Reserve    ,    
     Payment    ,    
     default_share_percent ,    
     this_share_percent ,    
  is_obligatory ,    
  grouping,recovery_to_date )    
            SELECT  ri_arrangement_line_id,    
        ISNULL(sum_insured,0),    
        type,    
        ISNULL(lower_limit,0),    
        ISNULL(line_limit,0),    
        ISNULL(Reserve,0),    
 ISNULL(Payment,0) + ISNULL(salvage,0) + ISNULL(recovery,0),    
        ISNULL(default_share_percent,0),    
        ISNULL(this_share_percent,0),ISNULL(is_obligatory,0)  , ISNULL(grouping,ri_arrangement_line_id)  ,    
        ISNULL(recovery,0)+ISNULL(salvage,0)    
            FROM    claim_ri_arrangement_line    
            WHERE   claim_id = @claim_id    
            AND     ri_arrangement_id = @ri_arrangement_id    
     AND    type IN ('TX','R')    
            ORDER BY    
     type DESC, priority ASC, number_of_lines ASC    
 SELECT @treaty_reserve=sum(Reserve),    
  @treaty_payment=sum(Payment)  FROM  #RICursor    
 WHERE type in ('T','TFS')   and  ISNULL(is_obligatory,0)=0    
 SELECT @retained_reserve=sum(Reserve)    
  ,@retained_payment=sum(Payment)    
 FROM  #RICursor    
 WHERE type ='R'    
 Set @treaty_reserve=ISNULL(@treaty_reserve,0)    
 Set @treaty_payment=ISNULL(@treaty_payment,0)    
 Set @retained_reserve=ISNULL(@retained_reserve,0)    
 SET @NetTreatyReserve=ISNULL(@treaty_reserve,0)    
 Set @NetTreatyPayment=ISNULL(@treaty_payment,0)    
      CREATE TABLE #RITreaty    
     (RI_id INT IDENTITY(1,1),    
     treaty_id INT,    
     sum_insured MONEY,    
     type varchar(3),    
     lower_limit MONEY,    
     line_limit MONEY,    
     number_of_lines INT,    
     ceded_premium_only INT,    
     Reserve    MONEY,    
     Payment    MONEY,    
     default_share_percent FLOAT,    
     this_share_percent FLOAT)    
    
 IF @ri_model_id<>@xol_ri_model_id  AND @prop_ri_calculation_method = 2 And @IsPortfolioTransferred=1    
 BEGIN    
  INSERT INTO #RITreaty (type,treaty_id,lower_limit,line_limit,number_of_lines,ceded_premium_only,default_share_percent)    
  Select  CASE WHEN rt.code = 'RET' THEN 'R'    
    WHEN rt.code = 'XOL'  THEN  'TX'    
    WHEN rt.code = 'CAT' THEN 'TC'    
    WHEN rt.code = '001' THEN 'TFS'    
    ELSE 'T'  END,r.treaty_id,    
  lower_limit,line_limit,number_of_lines,cede_premium_only,share_percent    
  from RI_Model_Line r join treaty t on r.treaty_id=t.Treaty_id    
   join Reinsurance_type rt on rt.reinsurance_type_id=t.reinsurance_type_id    
  where ri_model_id=@xol_ri_model_id    
  IF @Net_SumInsured IS NULL    
   SET @Net_SumInsured = @Gross_SumInsured    
  SELECT @NetFAC_SumInsured = @Net_SumInsured    
  IF @extended_limit_Enabled=1 AND ISNULL(@extended_limit_amount,0)>0 AND ISNULL(@Net_SumInsured,0)>ISNULL(@extended_limit_amount,0)    
  BEGIN    
   SELECT @extended_limit_amount=extended_limit_amount  from Claim_RI_Arrangement where Claim_id=(select base_claim_id from claim where claim_id=@claim_id)    
   SELECT @NetFAC_SumInsured=@extended_limit_amount    
   UPDATE #RITreaty SET line_limit=@extended_limit_amount WHERE type in ('T','TFS','R')    
  END    
  UPDATE  #RITreaty    
  SET     Sum_Insured = CASE WHEN @NetFAC_SumInsured*default_share_percent*0.01 <= ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01    
  THEN @NetFAC_SumInsured*default_share_percent*0.01 ELSE ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01 END    
  WHERE   type ='T'    
    IF ISNULL(@Net_SumInsured,0)>0    
    BEGIN    
    UPDATE  #RITreaty    
    SET     this_share_percent=(Sum_Insured/@Net_SumInsured)    
    WHERE   type ='T'    
    END    
    SELECT @os_SumInsured=ISNULL(SUM(ISNULL(sum_insured,0)),0) from #RITreaty WHERE type ='T'    
    SELECT @os_SumInsured=@NetFAC_SumInsured-@os_SumInsured    
    
    UPDATE #RITreaty    
    SET Sum_Insured = CASE WHEN @os_SumInsured > lower_limit    
        THEN CASE WHEN @os_SumInsured > line_limit    
          THEN line_limit - lower_limit    
          ELSE @os_SumInsured - lower_limit END    
         ELSE 0 END    
    WHERE   type ='TX' AND ceded_premium_only=0    
    
    SELECT @os_SumInsured=ISNULL(@os_SumInsured - SUM(ISNULL(sum_insured,0)),0) from #RITreaty WHERE type ='TX'    
    UPDATE #RITreaty    
    SET sum_insured = CASE WHEN @os_SumInsured > line_limit THEN line_limit ELSE @os_SumInsured END    
    WHERE   type ='R'    
    SELECT @os_SumInsured=ISNULL(@Net_SumInsured - SUM(ISNULL(sum_insured,0)),0) from #RITreaty    
   UPDATE  #RITreaty    
   SET     Sum_Insured = CASE WHEN @os_SumInsured <= ISNULL(line_limit,0) * ISNULL(number_of_lines,0)    
   THEN @os_SumInsured ELSE ISNULL(line_limit,0) * ISNULL(number_of_lines,0) END    
   WHERE   type ='TFS'    
    IF ISNULL(@Net_SumInsured,@Gross_SumInsured)>0    
    BEGIN    
    UPDATE  #RITreaty    
    SET     this_share_percent=(Sum_Insured/ISNULL(@Net_SumInsured,@Gross_SumInsured))    
    WHERE   type ='TFS'    
 END    
 END    
  DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR    
    SELECT    
      ISNULL(grouping,ri_arrangement_line_id),    
      SUM(ISNULL(sum_insured,0)),    
      type,    
      MAX(ISNULL(lower_limit,0)),    
      MAX(ISNULL(line_limit,0)),    
      SUM(ISNULL(Reserve,0)),    
      SUM(ISNULL(Payment,0)),    
      MAX(ISNULL(default_share_percent,0)) ,    
      MAX(ISNULL(this_share_percent,0)),    
      MAX(ISNULL(is_obligatory,0)),    
      SUM(ISNULL(recovery_to_date,0))    
    FROM #RICursor   Join @temp tmp On tmp.RIType = #RICursor.type    
    GROUP BY ISNULL(grouping,ri_arrangement_line_id),type    
    ORDER By MAX(RI_id)    
 END    
        OPEN RI_Cursor    
     FETCH NEXT FROM RI_Cursor    
            INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit,@Reserve,@Payment,@default_share_percent , @this_share_percent,@is_obligatory , @recovery_to_date    
  set @retained=0    
  WHILE @@FETCH_STATUS = 0 BEGIN    
  Declare @ParticipationPercent float    
  Declare @IsMultiact int    
  Declare @IsRetained tinyint    
  SELECT  @this_reserve = 0,@this_payment = 0,@IsRetained=0    
  IF @ri_type='FX' BEGIN    
    IF @ri_model_id<>@xol_ri_model_id AND @is_created = 0 AND @prop_ri_calculation_method = 2 AND @IsPortfolioTransferred=1 BEGIN    
  Select @ParticipationPercent= ISNULL(Participation_Percent,0)/100.0000, @IsRetained = ISNULL(retained,0)    
  From Claim_RI_Arrangement_line Where claim_id=@Claim_Id and ri_arrangement_line_id= @line_id    
  IF  @ParticipationPercent=0    
          Set @ParticipationPercent=1    
    
          IF (@GrossReserve-@FACSummaryReserve)>@lower_limit    
    BEGIN    
    IF (@GrossReserve-@FACSummaryReserve)>@line_limit    
     BEGIN    
      IF @Reserve>= (@line_limit - @lower_limit)    
      SELECT @this_reserve = 0    
      ELSE    
      SELECT @this_reserve = (@line_limit - @lower_limit )  - @Reserve    
     END    
    ELSE    
     BEGIN    
      IF @Reserve>=( @line_limit - @lower_limit )    
       SELECT @this_reserve = 0    
      ELSE    
      SELECT @this_reserve = ((@GrossReserve-@FACSummaryReserve) - @lower_limit )  - @Reserve    
     END    
    
      IF @this_reserve > @Gross_this_reserve - @this_reserve_used    
     Set @this_reserve = (@Gross_this_reserve - @this_reserve_used)    
    
     IF @IsRetained = 0    
     BEGIN    
      SELECT @FACTotalReserve = @FACTotalReserve + @this_reserve    
      SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve - @Reserve    
     END    
     ELSE    
    BEGIN    
     SELECT @FACRetained = ISNULL(@FACRetained,0) + ISNULL(@this_reserve,0)    
    END    
       END    
  ELSE    
   SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @Reserve    
    
    IF (@GrossPayment-@FACSummaryPayment)>@lower_limit    
    BEGIN    
      IF (@GrossPayment-@FACSummaryPayment)>@line_limit    
      BEGIN    
        IF @payment>= (@line_limit - @lower_limit)    
  SELECT @this_payment = 0    
        ELSE    
          SELECT @this_payment = (@line_limit - @lower_limit ) - @payment    
      END    
      ELSE    
  BEGIN    
        IF @payment>=( @line_limit - @lower_limit )    
          SELECT @this_payment = 0    
        ELSE    
          SELECT @this_payment = ((@GrossPayment-@FACSummaryPayment) - @lower_limit )  - @payment    
      END    
   IF @this_payment > @Gross_this_payment - @this_payment_done    
        Set @this_payment = (@Gross_this_payment - @this_payment_done)    
   if @retained=0    
SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment - @Payment    
SELECT @FACTotalPayment = @FACTotalPayment + @this_payment    
      IF @this_payment > @Gross_this_payment - @this_payment_done    
        Set @this_payment = @Gross_this_payment - @this_payment_done    
    END    
    Else    
    SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment - @Payment    
    
    END    
    ELSE IF @is_created = 0 BEGIN    
    Select @ParticipationPercent= ISNULL(Participation_Percent,0)/100.0000, @IsRetained = ISNULL(retained,0)    
    From Claim_RI_Arrangement_line Where claim_id=@Claim_Id and ri_arrangement_line_id= @line_id    
      if  @ParticipationPercent=0    
          Set @ParticipationPercent=1    
    IF @os_reserve>@lower_limit    
    BEGIN    
      IF @os_reserve>@line_limit    
      BEGIN    
        IF @Reserve>= (@line_limit - @lower_limit)    
     SELECT @this_reserve = 0    
        ELSE    
          SELECT @this_reserve = (@line_limit - @lower_limit )  - @Reserve    
      END    
      ELSE    
      BEGIN    
        IF @Reserve>=( @line_limit - @lower_limit )    
          SELECT @this_reserve = 0    
        ELSE    
          SELECT @this_reserve = (@os_reserve - @lower_limit )  - @Reserve    
      END    
      IF @this_reserve > @Gross_this_reserve - @this_reserve_used    
        Set @this_reserve = (@Gross_this_reserve - @this_reserve_used)    
       IF @IsRetained = 0    
  BEGIN    
   SELECT @FACTotalReserve = @FACTotalReserve + @this_reserve    
   SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve - @Reserve    
  END    
  ELSE    
  BEGIN    
   SELECT @FACRetained = ISNULL(@FACRetained,0) + ISNULL(@this_reserve,0)    
  END    
    END    
    ELSE    
      SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @Reserve    
    
   IF @os_payment>@lower_limit    
    BEGIN    
      IF @os_payment>@line_limit    
      BEGIN    
        IF @payment>= (@line_limit - @lower_limit)    
  SELECT @this_payment = 0    
        ELSE    
          SELECT @this_payment = (@line_limit - @lower_limit ) - @payment    
  END    
      ELSE    
      BEGIN    
        IF @payment>=( @line_limit - @lower_limit )    
          SELECT @this_payment = 0    
        ELSE    
          SELECT @this_payment = (@os_payment - @lower_limit )  - @payment    
      END    
      IF @this_payment > @Gross_this_payment - @this_payment_done    
        Set @this_payment = (@Gross_this_payment - @this_payment_done)    
   if @retained=0    
SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment - @Payment    
SELECT @FACTotalPayment = @FACTotalPayment + @this_payment    
      IF @this_payment > @Gross_this_payment - @this_payment_done    
        Set @this_payment = @Gross_this_payment - @this_payment_done    
    END    
    Else    
    SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment - @Payment    
    END    
    Else    
  BEGIN    
   SELECT @this_reserve=SUM(this_reserve),@this_payment=SUM(this_payment)   From Claim_RI_Arrangement_Line Where ISNULL(grouping,ri_arrangement_line_id)=@line_id Group By grouping    
   SELECT @FACTotalReserve = @FACTotalReserve + @this_reserve    
   SELECT @FACTotalPayment = @FACTotalPayment + @this_payment    
   SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve    
   SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment    
  END    
  END    
  ELSE IF @ri_type='TX'    
  BEGIN    
    IF @ri_model_id<>@xol_ri_model_id AND @is_created = 0  AND @prop_ri_calculation_method = 2 AND @IsPortfolioTransferred=1    
    BEGIN    
    
  UPDATE #RITreaty SET Reserve = (@GrossNetTotalReserve-@FACTotalReserve-@FACPTSummaryReserve)* this_share_percent,    
  Payment = (@GrossNetTotalPayment-@FACTotalPayment-@FACPTSummaryPayment)* this_share_percent    
  WHERE type in ('TFS','T')    
    
  SELECT @treaty_reserve=SUM(ISNULL(Reserve,0)),    
  @treaty_payment=SUM(ISNULL(payment,0))    
  from #RITreaty WHERE type in ('TFS','T')    
    
    UPDATE #RITreaty    
    SET Reserve = CASE WHEN (@GrossNetTotalReserve-@FACTotalReserve-@FACPTSummaryReserve-@treaty_reserve) > lower_limit    
        THEN CASE WHEN (@GrossNetTotalReserve-@FACTotalReserve-@FACPTSummaryReserve-@treaty_reserve) > line_limit    
          THEN line_limit - lower_limit    
          ELSE (@GrossNetTotalReserve-@FACTotalReserve-@FACPTSummaryReserve-@treaty_reserve) - lower_limit END    
         ELSE 0 END,    
   Payment= CASE WHEN (@GrossNetTotalPayment-@FACTotalPayment-@FACPTSummaryPayment-@treaty_payment) > lower_limit    
        THEN CASE WHEN (@GrossNetTotalPayment-@FACTotalPayment-@FACPTSummaryPayment-@treaty_payment) > line_limit    
          THEN line_limit - lower_limit    
          ELSE (@GrossNetTotalPayment-@FACTotalPayment-@FACPTSummaryPayment-@treaty_payment) - lower_limit END    
         ELSE 0 END    
    WHERE   type ='TX' and lower_limit=@lower_limit and line_limit=@line_limit    
    
    SELECT @this_reserve=Reserve-@Reserve,    
    @this_payment=payment-@Payment    
    FROM #RITreaty WHERE   type ='TX' and lower_limit=@lower_limit and line_limit=@line_limit    
    END    
    ELSE BEGIN    
    IF @is_created = 0    
    BEGIN    
    IF @Gross_Net_reserve-@treaty_reserve >= @line_limit    
    SELECT @this_reserve=@line_limit - @lower_limit - @Reserve    
   ELSE    
    IF @Gross_Net_reserve-@treaty_reserve <= @lower_limit    
     SELECT @this_reserve = 0    
    ELSE    
     SELECT @this_reserve=(@Gross_Net_reserve-@treaty_reserve)- @lower_limit- @Reserve    
    IF @is_created = 0    
    BEGIN    
     IF @Gross_Net_Payment-@treaty_payment >=@line_limit    
      SELECT @this_payment=@line_limit - @lower_limit - @Payment    
     ELSE    
      IF @Gross_Net_Payment-@treaty_payment <=@lower_limit    
       SELECT @this_payment = 0    
      ELSE    
       SELECT @this_payment=(@Gross_Net_Payment-@treaty_payment )- @lower_limit- @Payment    
  END    
    ELSE    
       SELECT @this_payment=this_payment From Claim_RI_Arrangement_Line Where ri_arrangement_line_id=@line_id    
  END    
    ELSE    
    SELECT @this_reserve=this_reserve,@this_payment=this_payment  From Claim_RI_Arrangement_Line Where ri_arrangement_line_id=@line_id    
  END    
  END    
  ELSE IF @ri_type='T'    
 BEGIN    
             IF @os_reserve > 0    
             BEGIN    
                SELECT  @this_reserve = ROUND((@Gross_this_reserve - @FACTotalReserve)* @this_share_percent/100.0000, 4) ,    
                             @os_reserve = @os_reserve - @this_reserve - @Reserve    
    If @is_obligatory=0    
      SELECT @treaty_reserve=@treaty_reserve+@this_reserve    
    If @is_obligatory=1    
                BEGIN    
     SET @obligatory_thisreserve = ISNULL(@obligatory_thisreserve,0) + @this_reserve  + @reserve    
                    SET @Gross_Net_reserve = @os_reserve    
    END    
              END    
    
    IF @os_payment <> 0    
             BEGIN    
                SELECT @os_payment = @os_payment - @this_payment - @payment    
    If Round((@Gross_this_Payment -@FACTotalPayment)* @this_share_percent / 100.0000, 4) <> 0    
SELECT @this_payment = ROUND((@Gross_this_Payment - @FACTotalPayment )* @this_share_percent / 100.0000, 4)    
     if @is_obligatory=0    
      SELECT @treaty_payment=@treaty_payment+@this_payment    
     if @is_obligatory=1    
     BEGIN    
          SET @obligatory_thispayment = ISNULL(@obligatory_thispayment,0) + @this_payment  + @payment    
       SET @Gross_Net_Payment = @os_payment    
     END    
             END    
 END    
  ELSE IF @ri_type='F'    
   BEGIN    
    IF @os_reserve > 0    
              BEGIN    
                      SELECT  @this_reserve = ROUND((@Gross_this_reserve) * @this_share_percent/100.0000, 4),    
                              @os_reserve = @os_reserve - @this_reserve - @Reserve,    
                              @Gross_Net_reserve = @os_reserve    
                       SELECT @FACTotalReserve = @FACTotalReserve + @this_reserve    
                       SELECT @FACSummaryReserve = ISNULL(@FACSummaryReserve,0) + ISNULL(@FACTotalReserve,0)    
               END    
              IF @os_payment <> 0    
              BEGIN    
                      SELECT  @this_payment = ROUND((@Gross_This_Payment) * @this_share_percent/100.0000, 4),    
                          @os_payment = @os_payment - @this_payment - @Payment,    
                              @Gross_Net_Payment = @os_payment    
                      SELECT @FACTotalPayment = @FACTotalPayment + @this_payment    
                      SELECT @FACSummaryPayment= ISNULL(@FACTotalPayment,0) + ISNULL(@FACSummaryPayment,0)    
              END    
    
    END    
   ELSE IF @ri_type='TFS'    
   BEGIN    
  IF @os_reserve > 0    
  BEGIN    
   SELECT  @this_reserve = (@Gross_this_reserve - @FACTotalReserve)* @this_share_percent/100,    
     @os_reserve = @os_reserve - @this_reserve - @Reserve    
   SELECT @treaty_reserve=@treaty_reserve+@this_reserve    
  END    
  IF @os_payment <> 0    
  BEGIN    
   SELECT  @this_payment = (@Gross_this_Payment - @FACTotalPayment)* @this_share_percent/100,    
     @os_payment = @os_payment - @this_payment - @Payment    
   SELECT @treaty_payment =@treaty_payment+@this_payment    
  END    
    
   END    
   ELSE    
   BEGIN    
             BEGIN    
                     SELECT @This_reserve = ISNULL(@Total_Reserve,0) - ISNULL(@total_reserve_used,0) -ISNULL(@Reserve,0)    
             END    
           SELECT  @this_payment = ISNULL(@Total_payment,0) - ISNULL(@total_Payment_done,0) - ISNULL(@Payment,0)    
     END    
      Set @total_reserve_used = ISNULL(@total_reserve_used,0) +  ISNULL(@This_Reserve,0) + ISNULL(@Reserve,0)    
      Set @total_payment_done = ISNULL(@total_payment_done,0) +  ISNULL(@This_Payment,0) + ISNULL(@Payment,0)    
      Set @this_reserve_used = ISNULL(@this_reserve_used,0) + ISNULL(@This_Reserve,0)    
      Set @this_payment_done = ISNULL(@this_payment_done,0) + ISNULL(@this_payment,0)    
    
  if @Recovery= 1    
       BEGIN    
    UPDATE  claim_ri_arrangement_line    
         SET     this_reserve = ISNULL(@this_reserve,0),    
         this_salvage = ISNULL(@this_payment,0)    
               WHERE   claim_id = @claim_id    
               AND     ri_arrangement_line_id = @line_id    
    END    
  Else if @Recovery= 0    
     BEGIN    
   UPDATE  claim_ri_arrangement_line    
         SET     this_reserve = ISNULL(@this_reserve,0),    
         this_recovery = ISNULL(@this_payment,0)    
               WHERE   claim_id = @claim_id    
               AND     ri_arrangement_line_id = @line_id    
    END    
 ELSE    
      BEGIN    
              If @ri_type='FX'    
             UPDATE  claim_ri_arrangement_line    
             SET     this_reserve = ISNULL(@this_reserve,0)*participation_percent/100,    
                     this_payment = ISNULL(@this_payment,0)*participation_percent/100    
             WHERE   claim_id = @claim_id    
             AND   grouping = @line_id  and retained=0    
           ELSE    
                  IF @is_created =1    
                  UPDATE  claim_ri_arrangement_line    
    SET     this_reserve = ISNULL(@this_reserve,0),    
                                                this_payment = ISNULL(@this_payment,0)    
                  WHERE   claim_id = @claim_id    
                    AND   ri_arrangement_line_id = @line_id    
             ELSE    
             UPDATE  claim_ri_arrangement_line    
             SET     this_reserve = ISNULL(@this_reserve,0),    
                     this_payment = ISNULL(@this_payment,0)    
             WHERE   claim_id = @claim_id    
             AND   ri_arrangement_line_id = @line_id    
       END    
    
             FETCH NEXT FROM RI_Cursor    
                 INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit,@Reserve,@Payment,@default_share_percent, @this_share_percent,@is_obligatory , @recovery_to_date    
    
        END    
        CLOSE RI_Cursor    
        DEALLOCATE RI_Cursor    
    
                  EXEC spu_Calculate_Claims_Incurred_to_date @claim_id,@ri_arrangement_id,@is_created    
    
  EXEC DDLDropTable '#RI_Cursor'    
  EXEC DDLDropTable '#RITreaty'    
    
SET QUOTED_IDENTIFIER OFF 
GO
