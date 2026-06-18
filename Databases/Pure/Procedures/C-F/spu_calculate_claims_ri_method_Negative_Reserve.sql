
EXECUTE Ddldropprocedure 'spu_calculate_claims_ri_method_Negative_Reserve'
GO
CREATE PROCEDURE spu_calculate_claims_ri_method_negative_reserve    
 @claim_id          INT,    
 @ri_arrangement_id INT,    
 @total_reserve     MONEY,    
 @total_payment     MONEY,    
 @Recovery          INT=2    
AS    
DECLARE    
@line_id AS INT,    
@sum_insured AS MONEY,    
@this_reserve AS MONEY,    
@this_payment AS MONEY,    
@os_reserve AS MONEY,    
@os_payment AS MONEY,    
@ri_type AS VARCHAR (3),    
@product_option AS VARCHAR (20),    
@lower_limit AS MONEY,    
@line_limit AS MONEY,    
@remaining_limit AS MONEY,    
@default_share_percent AS MONEY,    
@Gross_SumInsured AS MONEY,    
@Net_SumInsured AS MONEY,    
@os_SumInsured AS MONEY,    
@this_SumInsured AS MONEY,    
@Reserve AS MONEY,    
@Payment AS MONEY,    
@Gross_Reserve_to_date AS MONEY,    
@Gross_This_reserve AS MONEY,    
@Gross_Net_Reserve AS MONEY,    
@Gross_Net_Payment AS MONEY,    
@Gross_Payment_to_date AS MONEY,    
@Gross_This_Payment AS MONEY,    
@total_reserve_used AS MONEY,    
@total_payment_done AS MONEY,    
@this_reserve_used AS MONEY,    
@this_payment_done AS MONEY,    
@ReserveEdited AS INT,    
@this_share_percent AS FLOAT,    
@Running_Reserve AS MONEY,    
@XOLReserve AS MONEY,    
@XOLPayment AS MONEY,    
@Running_payment AS MONEY,    
@FACTotalReserve money,    
@FACTotalPayment money,    
@IsObligatory INT,    
@is_created TinyInt,    
@GrossNetTotalReserve Money,    
@GrossNetTotalPayment Money,    
    
@ri_model_id INT,    
@XOL_ri_model_id INT,    
@NetFAC_SumInsured MONEY,    
  @extended_limit_amount money,    
  @extended_limit_Enabled INT,    
  @treaty_reserve MONEY,    
  @treaty_payment MONEY,    
    
  @recovery_to_date MONEY,    
    
  @GrossReserve Money,    
  @GrossPayment Money,    
  @FACSummaryReserve MONEY,    
  @FACSummaryPayment MONEY,    
  @FACPTSummary MONEY,    
  @flag TINYINT,    
  @prop_ri_calculation_method INT,    
  @ri_band_id INT,    
  @IsPortfolioTransferred tinyint,  
  @cover_start_date_ForRi DATETIME ,    
  @Insurance_file_cnt int    
    
  SELECT @flag=0, @IsPortfolioTransferred=0    
  
  Select  @Insurance_file_cnt = Policy_id from claim where claim_id = @claim_id    
  SELECT @cover_start_date_ForRi = inception_date_tpi          
    FROM insurance_file  (NOLOCK)          
    WHERE Insurance_file_cnt = @Insurance_file_cnt    
  
  DECLARE @temp TABLE( RIType varchar(3), RIPriority int)    
   Insert into @temp(RItype,RIPriority) values('F',1)    
   Insert into @temp(RItype,RIPriority) values('FX',2)    
   Insert into @temp(RItype,RIPriority) values('TFS',3)    
   Insert into @temp(RItype,RIPriority) values('T',4)    
   Insert into @temp(RItype,RIPriority) values('TX',5)    
   Insert into @temp(RItype,RIPriority) values('TC',6)    
   Insert into @temp(RItype,RIPriority) values('R',7)    
    
  If Exists( Select NULL from Claim_pt_log CPT inner join claim clm on CPT.base_claim_id = clm.base_claim_id where clm.Claim_id =@claim_id )    
 SELECT @IsPortfolioTransferred = 1    
    
  SELECT @GrossNetTotalReserve = ISNULL(Reserve_to_date,0)+ISNULL(this_reserve,0),    
  @GrossNetTotalPayment=ISNULL(payment_to_date,0)+ISNULL(this_payment,0)+ISNULL(salvage_to_date,0)+ISNULL(this_salvage,0)+ISNULL(recovery_to_date,0)+ISNULL(this_recovery,0),    
  @Gross_SumInsured = ISNULL(Sum_insured,0)    
  from Claim_RI_Arrangement    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id    
    
  SELECT @GrossReserve=@GrossNetTotalReserve, @GrossPayment=@GrossNetTotalPayment    
    
  SELECT @FACSummaryReserve =ISNULL(SUM(ISNULL(Reserve_to_date,0)),0),    
  @FACSummaryPayment=ISNULL(SUM(ISNULL(payment_to_date,0)),0) - ISNULL(SUM(ISNULL(salvage_to_date,0)),0)- ISNULL(SUM(ISNULL(recovery_to_date,0)),0)    
  from Claim_RI_Arrangement_Line    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id    
  AND type ='F'    
    
  SELECT @FACPTSummary = ISNULL(SUM(ISNULL(claim_incurred_to_date,0)),0) from Claim_RI_Arrangement_Line    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id and is_pt_archive=1    
  AND type in ('F')    
    
  SELECT @GrossNetTotalReserve = @GrossNetTotalReserve - ISNULL(SUM(ISNULL(Reserve_to_date,0)),0),    
  @GrossNetTotalPayment=@GrossNetTotalPayment - ISNULL(SUM(ISNULL(payment_to_date,0)),0) - ISNULL(SUM(ISNULL(salvage_to_date,0)),0)- ISNULL(SUM(ISNULL(recovery_to_date,0)),0)  ,@Net_SumInsured=@Gross_SumInsured - SUM(ISNULL(Sum_insured,0))    
  from Claim_RI_Arrangement_Line    
  WHERE claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id and is_pt_archive=0    
  AND (type in ('F','FX') OR (Type= 'T' and Is_Obligatory = 1)) and ISNULL(retained, 0) <> 1    
    
SELECT @extended_limit_Enabled=ISNULL(Is_extended_limit_applied,0),@extended_limit_amount=ISNULL(ra.Extended_limit_amount,0),    
    @ri_band_id=cra.ri_band_id    
from RI_Arrangement ra join Claim_RI_Arrangement cra    
  on ra.ri_arrangement_id=cra.original_ri_arrangement_id    
  WHERE cra.claim_ri_arrangement_id=@ri_arrangement_id and cra.claim_id =@claim_id    
    
  --IF Model doesnt have QS or Surplus then use old calc logic    
  IF NOT EXISTS(SELECT NULL FROM Claim_RI_Arrangement_line WHERE ri_arrangement_id=@ri_arrangement_id AND type in ('T','TFS'))    
 SET @prop_ri_calculation_method=1    
    
  SELECT TOP 1 @prop_ri_calculation_method= Proportional_RI_Cal_Method    
  from RI_Band_version   
  where ri_band_id  = @ri_band_id    
  AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)        
  ORDER BY effective_date DESC      
  
  
SELECT @os_reserve = @total_reserve,    
       @os_payment = @total_payment,    
       @Gross_Net_Reserve = @total_reserve,    
       @Gross_Net_Payment = @total_Payment,    
       @Running_Reserve = @total_reserve,    
       @Running_Payment = @total_Payment    
    
SET @total_reserve_used = 0    
SET @total_payment_done = 0    
SET @this_reserve_used = 0    
SET @this_payment_done = 0    
SET @XOLReserve = 0    
SET @XOLPayment =0    
    
SELECT @FACTotalReserve = 0, @FACTotalPayment=0    
SELECT @product_option = '0'    
SELECT @product_option = ISNULL(value, 0)    
FROM   Hidden_Options    
WHERE  option_number = 88    
SELECT @Gross_Reserve_to_date = Sum(reserve)    
FROM   CLAIM_RI_ARRANGEMENT_LINE    
WHERE  Claim_id = @Claim_id    
       AND ri_arrangement_id = @ri_arrangement_id    
SELECT @Gross_Payment_to_date = Sum(Payment) + ISNULL(Sum(Salvage), 0) + ISNULL(Sum(Recovery), 0)    
FROM   CLAIM_RI_ARRANGEMENT_LINE    
WHERE  Claim_id = @Claim_id    
       AND ri_arrangement_id = @ri_arrangement_id    
SET @ReserveEdited = 0    
IF @Total_reserve <> @Gross_Reserve_to_date    
    SET @ReserveEdited = 1    
IF @Total_Payment <> @Gross_Payment_to_date    
    SET @ReserveEdited = 0    
SELECT @Gross_This_Reserve = @Total_reserve    
SELECT @Gross_This_Payment = @Total_Payment    
    
CREATE TABLE #RITreaty    
     (RI_id INT IDENTITY(1,1),    
     treaty_id INT,    
     sum_insured FLOAT,    
     type varchar(3),    
     lower_limit MONEY,    
     line_limit MONEY,    
     number_of_lines INT,    
     ceded_premium_only INT,    
     Reserve    MONEY,    
     Payment    MONEY,    
     default_share_percent FLOAT,    
     this_share_percent FLOAT,    
    is_obligatory INT)    
    
 Select @ri_model_id = ri_model_id,@xol_ri_model_id=xol_ri_model_id from Claim_RI_Arrangement where ri_arrangement_id=@ri_arrangement_id    
    
 IF @ri_model_id<>@xol_ri_model_id AND @prop_ri_calculation_method=2 AND @IsPortfolioTransferred=1    
  BEGIN    
    INSERT INTO #RITreaty (type,treaty_id,lower_limit,line_limit,number_of_lines,ceded_premium_only,default_share_percent,is_obligatory)    
    Select  CASE WHEN rt.code = 'RET' THEN 'R'    
   WHEN rt.code = 'XOL'  THEN  'TX'    
   WHEN rt.code = 'CAT' THEN 'TC'    
   WHEN rt.code = '001' THEN 'TFS'    
   ELSE 'T'  END,r.treaty_id,    
    lower_limit,line_limit,number_of_lines,cede_premium_only,share_percent  , Is_Obligatory    
    from RI_Model_Line r join treaty t on r.treaty_id=t.Treaty_id    
     join Reinsurance_type rt on rt.reinsurance_type_id=t.reinsurance_type_id    
    where ri_model_id=@xol_ri_model_id    
    
  IF @Net_SumInsured IS NULL    
     SET @Net_SumInsured = @Gross_SumInsured    
    
  SELECT @NetFAC_SumInsured = @Net_SumInsured    
    
  IF @extended_limit_Enabled=1 AND ISNULL(@extended_limit_amount,0)>0 AND ISNULL(@Net_SumInsured,0)>ISNULL(@extended_limit_amount,0)    
    BEGIN    
      SELECT @NetFAC_SumInsured=@extended_limit_amount    
      UPDATE #RITreaty SET line_limit=@extended_limit_amount WHERE type in ('T','TFS','R')    
    END    
    
  UPDATE  #RITreaty    
  SET    Sum_Insured =    
   CASE WHEN @NetFAC_SumInsured*default_share_percent*0.01 <= ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01    
    THEN @NetFAC_SumInsured*default_share_percent*0.01 ELSE ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01 END    
  WHERE   type ='T'  and is_obligatory = 0    
    
 IF ISNULL(@Net_SumInsured,0)>0    
    BEGIN    
  UPDATE  #RITreaty    
  SET     this_share_percent=(Sum_Insured/@Net_SumInsured)    
  WHERE   type ='T'  and is_obligatory = 0    
    END    
    
 SELECT @os_SumInsured=ISNULL(SUM(ISNULL(sum_insured,0)),0) from #RITreaty WHERE type ='T' and is_obligatory = 0    
    
 SELECT @os_SumInsured=@NetFAC_SumInsured-@os_SumInsured    
    
 UPDATE #RITreaty    
   SET Sum_Insured = CASE WHEN @os_SumInsured > lower_limit    
        THEN    
        CASE WHEN @os_SumInsured > line_limit    
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
    SET     Sum_Insured = CASE WHEN @os_SumInsured*default_share_percent*0.01 <= ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01    
    THEN @os_SumInsured*default_share_percent*0.01 ELSE ISNULL(line_limit,0) * ISNULL(number_of_lines,0)* default_share_percent*0.01 END    
    WHERE   type ='TFS'    
    
 IF ISNULL(@Net_SumInsured,@Gross_SumInsured)>0    
    BEGIN    
    UPDATE  #RITreaty    
    SET     this_share_percent=(Sum_Insured/ISNULL(@Net_SumInsured,@Gross_SumInsured))    
    WHERE   type ='TFS'    
 END    
 END    
    
CREATE TABLE #RICursor1    
(    
    ri_arrangement_line_id INT        ,    
    sum_insured            MONEY      ,    
    type                   VARCHAR (3),    
    lower_limit            MONEY      ,    
    line_limit             MONEY      ,    
    Reserve                MONEY      ,    
    Payment                MONEY      ,    
    default_share_percent  FLOAT      ,    
    this_share_percent     FLOAT      ,    
    recovery_to_date       MONEY  ,    
 is_obligatory INT    
)    
    
INSERT INTO #RICursor1    
SELECT   ri_arrangement_line_id,    
         sum_insured,    
         TYPE,    
         lower_limit,    
         line_limit,    
         Reserve,    
         Payment + (ISNULL(salvage, 0) + ISNULL(recovery, 0)),    
         CASE WHEN this_share_percent = 0 THEN default_share_percent    
   ELSE this_share_percent    
   END,    
         this_share_percent,    
         (ISNULL(salvage, 0) + ISNULL(recovery, 0))  ,    
   Is_Obligatory    
FROM     claim_ri_arrangement_line    
WHERE    claim_id = @claim_id    
         AND ri_arrangement_id = @ri_arrangement_id    
         AND (type IN ('F','T','TFS','FX','TX','R')  AND is_pt_archive =0)    
    
DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY    
    FOR SELECT ri_arrangement_line_id,    
               sum_insured,    
               TYPE,    
               lower_limit,    
               line_limit,    
               Reserve,    
               Payment,    
               default_share_percent,    
               this_share_percent,    
               recovery_to_date  ,    
      is_obligatory    
        FROM   #RICursor1 INNER Join @temp tmp On tmp.RIType = #RICursor1.type    
  ORDER BY Is_Obligatory DESC , tmp.RIPriority    
    
  DECLARE @PaymentInitiated As MONEY    
  select @PaymentInitiated = this_payment from claim_ri_arrangement where claim_id = @claim_id    
  and ri_arrangement_id = @ri_arrangement_id    
    
OPEN RI_Cursor    
FETCH NEXT FROM RI_Cursor    
 INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit, @Reserve, @Payment, @default_share_percent, @this_share_percent, @recovery_to_date , @IsObligatory    
WHILE @@FETCH_STATUS = 0    
    BEGIN    
    
  SELECT @this_reserve = 0, @this_payment = 0    
    
        DECLARE @ParticipationPercent AS FLOAT    
        DECLARE @IsMultiact AS INT    
        IF @ri_type = 'FX'    
    BEGIN    
       IF @ri_model_id <> @xol_ri_model_id AND @prop_ri_calculation_method=2 AND @IsPortfolioTransferred =1    
      BEGIN    
       SELECT @ParticipationPercent = Participation_Percent / 100    
       FROM   Claim_RI_Arrangement_line    
       WHERE  claim_id = @Claim_Id    
           AND ri_arrangement_line_id = @line_id    
    
       IF @ParticipationPercent = 0    
        SET @ParticipationPercent = 1    
    
       IF (@GrossReserve-@FACSummaryReserve) > @lower_limit    
        BEGIN    
         IF (@GrossReserve-@FACSummaryReserve) > @line_limit    
          BEGIN    
           SELECT @this_reserve = (@line_limit - @lower_limit) * @ParticipationPercent    
          END    
         ELSE    
          BEGIN    
           SELECT @this_reserve = ((@GrossReserve-@FACSummaryReserve) - @lower_limit) * @ParticipationPercent    
          END    
    
         IF @this_reserve > @Gross_this_reserve - @this_reserve_used    
          SET @this_reserve = (@Gross_this_reserve - @this_reserve_used) * @ParticipationPercent    
          SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve    
        END    
    
       IF (@GrossPayment-@FACSummaryPayment) > @lower_limit AND @line_limit > @lower_limit    
        BEGIN    
         IF (@GrossPayment-@FACSummaryPayment) > @line_limit    
          BEGIN    
           SELECT @this_payment = (@line_limit - @lower_limit) * @ParticipationPercent    
          END    
         ELSE    
          IF (@GrossPayment-@FACSummaryPayment) >= @line_limit - @Payment    
           SELECT @this_payment = ((@GrossPayment-@FACSummaryPayment) - @lower_limit) * @ParticipationPercent    
          ELSE    
           SELECT @this_Payment = (@os_payment - @lower_limit) * @ParticipationPercent    
    
         IF @this_payment > @Gross_this_payment - @this_payment_done    
          SET @this_payment = @Gross_this_payment - @this_payment_done    
        END    
      END    
       ELSE    
      BEGIN    
       SELECT @ParticipationPercent = Participation_Percent / 100    
       FROM   Claim_RI_Arrangement_line    
       WHERE  claim_id = @Claim_Id  AND ri_arrangement_line_id = @line_id    
    
       IF @ParticipationPercent = 0    
        SET @ParticipationPercent = 1    
    
       IF @os_reserve > @lower_limit    
        BEGIN    
         IF @os_reserve > @line_limit    
          BEGIN    
           SELECT @this_reserve = (@line_limit - @lower_limit) * @ParticipationPercent    
          END    
         ELSE    
          BEGIN    
           SELECT @this_reserve = (@os_reserve - @lower_limit) * @ParticipationPercent    
        END    
    
         IF @this_reserve > @Gross_this_reserve - @this_reserve_used    
          SET @this_reserve = (@Gross_this_reserve - @this_reserve_used) * @ParticipationPercent    
          SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve    
        END    
    
        IF @Running_Reserve = 0    
         SET @Running_Reserve = @Gross_Net_reserve - @this_reserve    
        ELSE    
         SET @Running_Reserve = @Running_Reserve - @this_reserve    
    
       IF @PaymentInitiated <> 0 or @recovery <> 2    
        BEGIN    
         IF @os_payment > @lower_limit    
            AND @line_limit > @lower_limit    
          BEGIN    
           IF @os_payment > @line_limit    
            BEGIN    
             SELECT @this_payment = (@line_limit - @lower_limit) * @ParticipationPercent    
            END    
           ELSE    
            IF @os_payment >= @line_limit - @Payment    
             SELECT @this_payment = (@os_payment - @lower_limit) * @ParticipationPercent    
            ELSE    
             SELECT @this_Payment = (@os_payment - @lower_limit) * @ParticipationPercent    
             IF @this_payment > @Gross_this_payment - @this_payment_done    
              SET @this_payment = @Gross_this_payment - @this_payment_done    
          END    
    
         IF @Running_Payment = 0    
          SET @Running_Payment = @Gross_Net_Payment - @this_payment    
         ELSE    
          SET @Running_Payment = @Running_Payment - @this_payment    
         SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment    
        END    
      END    
    END    
  ELSE IF @ri_type = 'TX' OR @ri_type = 'R'    
                BEGIN    
     IF @ri_model_id<>@xol_ri_model_id AND @prop_ri_calculation_method=2 AND @IsPortfolioTransferred =1    
      BEGIN    
         UPDATE #RITreaty SET Reserve = ROUND((@GrossNetTotalReserve-(@FACTotalReserve)-@FACPTSummary)* this_share_percent, 4),    
         Payment = ROUND((@GrossNetTotalPayment-(@FACTotalPayment)-@FACPTSummary)* this_share_percent, 4)    
         WHERE type in ('TFS','T')    
    
         SELECT @treaty_reserve=SUM(ISNULL(Reserve,0)),    
         @treaty_payment=SUM(ISNULL(payment,0))    
         from #RITreaty WHERE type in ('TFS','T')    
    
        UPDATE #RITreaty    
        SET    
        Reserve =    
        CASE WHEN (@GrossNetTotalReserve-(@FACTotalReserve)-@FACPTSummary-@treaty_reserve) > lower_limit    
         THEN    
          CASE WHEN (@GrossNetTotalReserve-(@FACTotalReserve)-@FACPTSummary-@treaty_reserve) > line_limit    
           THEN line_limit - lower_limit    
          ELSE    
           CASE WHEN (@GrossNetTotalReserve-(@FACTotalReserve)-@FACPTSummary-@treaty_reserve) - lower_limit > @Running_Reserve    
            THEN (@GrossNetTotalReserve-(@FACTotalReserve)-@FACPTSummary-@treaty_reserve) - lower_limit    
           ELSE @Running_Reserve - lower_limit END    
          END    
        ELSE 0 END,    
    
           Payment =    
        CASE WHEN (@GrossNetTotalPayment-(@FACTotalPayment)-@FACPTSummary-@treaty_payment) > lower_limit    
         THEN    
          CASE WHEN (@GrossNetTotalPayment-(@FACTotalPayment)-@FACPTSummary-@treaty_payment) > line_limit    
           THEN line_limit - lower_limit    
          ELSE    
           CASE WHEN  (@GrossNetTotalPayment-(@FACTotalPayment)-@FACPTSummary-@treaty_payment) - lower_limit > @Running_payment    
            THEN (@GrossNetTotalPayment-(@FACTotalPayment)-@FACPTSummary-@treaty_payment) - lower_limit    
           ELSE @Running_payment  - lower_limit END    
          END    
         ELSE 0 END    
        WHERE   type IN ('TX','R') and lower_limit=@lower_limit and line_limit=@line_limit    
    
        SELECT @this_reserve= CASE WHEN Reserve = 0 THEN 0 ELSE Reserve END,    
        @this_payment= CASE WHEN payment = 0 THEN 0 ELSE payment END    
        FROM #RITreaty WHERE   type IN ('TX','R') and lower_limit=@lower_limit and line_limit=@line_limit    
      END    
     ELSE    
      BEGIN    
       IF @Running_Reserve > @lower_limit    
        BEGIN    
         IF @Running_Reserve > @line_limit    
          BEGIN    
           SELECT @this_reserve = (@line_limit - @lower_limit)    
          END    
         ELSE    
          BEGIN    
           SELECT @this_reserve = @Running_Reserve - @lower_limit    
          END    
    
         IF @Gross_this_reserve > 0    
            AND @this_reserve > @Gross_this_reserve - @this_reserve_used    
          SET @this_reserve = @Gross_this_reserve - @this_reserve_used    
    
        END    
    
        if @this_reserve > 0 AND @ri_type ='TX'    
       SET @XOLReserve = @XOLReserve + @this_reserve    
    
        IF @ri_type ='R'    
         BEGIN    
          --For the Retained line , the whole remaining reserve should get allocated at this point    
          if @XOLReserve > 0    
           SET @this_reserve = @Running_Reserve - @XOLReserve    
         END    
    
       IF @PaymentInitiated <> 0 or @recovery <> 2    
        BEGIN    
         IF (@Running_Payment) > @lower_limit    
          BEGIN    
           IF (@Running_Payment) > @line_limit    
            BEGIN    
             IF (@Running_Payment) > 0    
              BEGIN    
               SELECT @this_payment = -(@line_limit - @lower_limit)    
                if @recovery <> 2    
                 SET @this_payment = -@this_payment    
              END    
             ELSE    
              BEGIN    
               SELECT @this_payment = (@line_limit - @lower_limit)    
              END    
            END    
           ELSE    
            BEGIN    
             IF @Running_Payment > 0    
              BEGIN    
               SELECT @this_Payment = @Running_Payment - @lower_limit    
              END    
             ELSE    
              BEGIN    
               SELECT @this_Payment = @Running_Payment + @lower_limit    
              END    
            END    
          END    
    
          IF (@Running_Payment) < 0 and @ri_type = 'R'    
           BEGIN    
            SELECT @this_Payment = @Running_Payment + @lower_limit    
           END    
    
          IF Abs(@this_payment) > Abs(@Gross_this_payment) - @this_payment_done    
           SET @this_Payment = @Gross_this_Payment - @this_Payment_done    
    
          if @this_Payment > 0 AND @ri_type ='TX'    
           SET @XOLPayment = @XOLPayment + @this_Payment    
    
          IF @ri_type ='R'    
           BEGIN    
            --For the Retained line , the whole remaining payment should get allocated at this point    
            if @XOLPayment > 0    
             SET @this_Payment = @Running_payment - @XOLPayment    
           END    
        END    
      END    
    END    
  ELSE IF @ri_type = 'F'    
                BEGIN    
     SELECT @this_reserve = ROUND((@Gross_This_reserve-@Gross_Reserve_to_date-@FACTotalReserve) * @this_share_percent / 100,4),    
       @os_reserve = @os_reserve - @this_reserve-@Reserve,    
       @Gross_Net_reserve = @os_reserve    
    
     IF @Running_Reserve = 0    
                        SET @Running_Reserve = @os_reserve    
                    ELSE    
                        SET @Running_Reserve = @Running_Reserve - @this_reserve  - @Reserve    
    
     IF @PaymentInitiated <> 0 or @recovery <> 2    
      BEGIN    
       SELECT @this_payment = ROUND((@Gross_this_payment-@Gross_Payment_to_date-@FACTotalPayment) * @this_share_percent / 100,4),    
         @os_payment = @os_payment - @this_payment-@Payment,    
         @Gross_Net_Payment = @os_payment    
    
       IF @Running_Payment = 0    
        SET @Running_Payment = @os_payment    
       ELSE    
        SET @Running_Payment = @Running_Payment - @this_payment - @Payment    
      END    
                END    
  ELSE IF @ri_type = 'T'    
                BEGIN    
     IF @sum_insured = 0 AND @this_share_percent = 0    
     BEGIN    
      SET @this_share_percent = @default_share_percent    
     END    
    
     SELECT @this_reserve = ROUND((@Gross_This_reserve-@Gross_Reserve_to_date-@FACTotalReserve) * @this_share_percent / 100.0000,4),    
                                @os_reserve = @os_reserve - @this_reserve - @Reserve    
    
        IF @Running_Reserve = 0    
                        SET @Running_Reserve = @Gross_Net_reserve - @this_reserve - @Reserve    
                    ELSE    
                        SET @Running_Reserve = @Running_Reserve - @this_reserve  - @Reserve    
    
     IF @PaymentInitiated <> 0 or @recovery <> 2    
     BEGIN    
      SELECT @this_payment = ROUND((@Gross_This_Payment - @Gross_Payment_to_date - @FACTotalPayment )* @this_share_percent / 100.0000,4),    
        @os_payment = @os_payment - @this_payment - @Payment    
    
      IF @Running_Payment = 0    
       SET @Running_Payment = @Gross_Net_Payment - @this_payment  - @Payment    
      ELSE    
       SET @Running_Payment = @Running_Payment - @this_payment  - @Payment    
     END    
    END    
  ELSE IF @ri_type = 'TFS'    
    BEGIN    
     SELECT @this_reserve = ROUND((@Gross_This_reserve-@Gross_Reserve_to_date-@FACTotalReserve) * @this_share_percent / 100.0000,4),    
        @os_reserve = @os_reserve - @this_reserve - @Reserve,    
        @Running_Reserve = @Gross_Net_reserve - @this_reserve - @Reserve    
    
     IF @PaymentInitiated <> 0 or @recovery <> 2    
      BEGIN    
       SELECT @this_payment = ROUND((@Gross_This_Payment-@Gross_Payment_to_date-@FACTotalPayment) * @this_share_percent / 100.0000,4),    
       @os_payment = @os_payment - @this_payment - @Payment,    
       @Running_payment = @Gross_Net_Payment - @this_payment - @Payment    
      END    
    END    
  ELSE    
    BEGIN    
     IF ABS(@Total_Reserve - @total_reserve_used) > 0    
       BEGIN    
       SELECT @This_reserve = @Total_Reserve - @total_reserve_used    
       END    
    
     IF @PaymentInitiated <> 0 or @recovery <> 2    
      SELECT @this_payment = @Total_payment - @total_Payment_done    
    END    
    
  IF @ri_type NOT IN ('T','TFS','F')    
   BEGIN    
    SET @this_reserve = @this_reserve - @Reserve    
    
    IF @PaymentInitiated <> 0 or @recovery <> 2    
     SET @this_payment = @this_payment - @payment    
   END    
    
  SET @total_reserve_used = @total_reserve_used + @This_Reserve + @Reserve    
  IF @PaymentInitiated <> 0 or @recovery <> 2    
   SET @total_payment_done = @total_payment_done + @This_Payment + @Payment    
    
  SET @this_reserve_used = @this_reserve_used + @This_Reserve  + @Reserve    
  IF @PaymentInitiated <> 0 or @recovery <> 2    
   SET @this_payment_done = @this_payment_done + @this_payment  + @Payment    
    
        IF @ri_type IN ('F','FX')  OR (@ri_type ='T' and @IsObligatory = 1)    
   BEGIN    
                SELECT @FACTotalReserve = @FACTotalReserve + @this_reserve    
    IF @PaymentInitiated <> 0 or @recovery <> 2    
     SELECT @FACTotalPayment = @FACTotalPayment + @this_payment    
   END    
    
     IF @ri_type IN ('F')  OR (@ri_type ='T' and @IsObligatory = 1)    
   BEGIN    
    SELECT @FACSummaryReserve = ISNULL(@FACSummaryReserve,0) + ISNULL(@FACTotalReserve,0)    
    IF @PaymentInitiated <> 0 or @recovery <> 2    
     SELECT @FACSummaryPayment= ISNULL(@FACTotalPayment,0) + ISNULL(@FACSummaryPayment,0)    
   END    
    
  IF @ReserveEdited = 0    
            BEGIN    
                IF (@Recovery = 2)    
                    BEGIN    
                        UPDATE  claim_ri_arrangement_line    
                            SET this_reserve = ISNULL(@this_reserve, 0),    
       this_payment = ISNULL(@this_payment, 0)    
                        WHERE   claim_id = @claim_id    
                                AND ri_arrangement_line_id = @line_id    
                    END    
                ELSE    
                    IF (@Recovery = 1)    
                        BEGIN    
       UPDATE  claim_ri_arrangement_line    
                                SET this_Salvage = ISNULL(@this_payment, 0)    
                            WHERE   claim_id = @claim_id    
                                    AND ri_arrangement_line_id = @line_id    
                        END    
                    ELSE    
      IF (@Recovery = 0)    
                            BEGIN    
        UPDATE  claim_ri_arrangement_line    
                                    SET this_Recovery = ISNULL(@this_payment, 0)    
                                WHERE   claim_id = @claim_id    
                                        AND ri_arrangement_line_id = @line_id    
                            END    
            END    
        ELSE    
            UPDATE  claim_ri_arrangement_line    
                SET this_reserve = ISNULL(@this_reserve, 0),    
     this_payment = ISNULL(@this_payment, 0)    
            WHERE   claim_id = @claim_id    
   AND ri_arrangement_line_id = @line_id    
    
FETCH NEXT FROM RI_Cursor    
                INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit,@Reserve,@Payment,@default_share_percent,@this_share_percent, @recovery_to_date  ,@IsObligatory    
    END    
CLOSE RI_Cursor    
DEALLOCATE RI_Cursor    
    
EXEC spu_Calculate_Claims_Incurred_to_date @claim_id,@ri_arrangement_id    
    
EXECUTE Ddldroptable '#RICursor1'    
EXECUTE Ddldroptable '#RITreaty' 