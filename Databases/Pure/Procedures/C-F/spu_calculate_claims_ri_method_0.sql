SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_method_0'
GO

CREATE PROCEDURE spu_calculate_claims_ri_method_0  
    @claim_id int,  
    @ri_arrangement_id int,  
    @reserve money,  
    @payment money,  
    @Recovery tinyint=2  
AS  
  
    DECLARE @GrossNetThisReserve Money,  
            @GrossNetThisPayment Money,  
            @TotalThisReserve money,  
         @TotalThisPayment money,  
   @product_option   INT  --PN69495  
   DECLARE @RI2007Enabled int 
   SELECT @RI2007Enabled = ISNULL(value, 0) From hidden_options Where option_number = 88 --RI2007Enabled  
   SELECT  @GrossNetThisReserve = 0,  
            @GrossNetThisPayment = 0,  
            @TotalThisReserve = 0,  
            @TotalThisPayment = 0,  
            @product_option=0 --PN69495  
  
 SELECT @product_option=ISNULL(value,0) FROM Hidden_Options WHERE option_number=88 --PN69495  
    -- Update the FAC ri lines  
 IF @product_option=1  --PN69495  
 BEGIN      --PN69495  
  IF @Recovery = 2  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET     this_share_percent = ISNULL(this_share_percent, default_share_percent),  
     this_reserve = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @reserve)),  
     this_payment = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @payment))  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     (type = 'F' OR ISNULL(@RI2007Enabled, 0) = 0) 
  END  
  IF @Recovery = 1  
  BEGIN  
   -- Update the FAC ri lines  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_salvage = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @payment))  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'F'  
  END  
  IF @Recovery = 0  
  BEGIN  
   -- Update the FAC ri lines  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_recovery = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @payment))  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'F'  
  END  
  
  --Get the Gross Net values for later calculations  
  SELECT  
   @GrossNetThisReserve = @reserve,  
   @GrossNetThisPayment = @payment  
  
  SELECT  
   @GrossNetThisReserve = @GrossNetThisReserve - SUM(this_reserve),  
   @GrossNetThisPayment = @GrossNetThisPayment - (SUM(this_payment) + SUM(this_salvage) + SUM(this_recovery))  
  FROM  
   claim_ri_arrangement_line  
  WHERE   claim_id = @claim_id  
  AND     ri_arrangement_id = @ri_arrangement_id  
  AND     type = 'F'  
  
  -- Update the Treaty ri lines  
  IF @Recovery = 2  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET  
    this_reserve = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * ISNULL(@GrossNetThisReserve,@reserve))),  
    this_payment = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * ISNULL(@GrossNetThisPayment,@payment)))  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'T'  
  END  
  IF @Recovery = 1  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_salvage = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * ISNULL(@GrossNetThisPayment,@payment)))  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'T'  
  END  
  IF @Recovery = 0  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_recovery = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * ISNULL(@GrossNetThisPayment,@payment)))  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'T'  
  END  
  
  --Get the total of all the values to calculate the retained figures  
  SELECT  
   @TotalThisReserve = SUM(this_reserve),  
   @TotalThisPayment = SUM(this_payment)  + SUM(this_salvage) + SUM(this_recovery)  
  FROM  
   claim_ri_arrangement_line  
  WHERE   claim_id = @claim_id  
  AND     ri_arrangement_id = @ri_arrangement_id  
  AND     (type = 'T' OR type = 'F')  
  
  --Update the Retained Line  
  IF @Recovery = 2  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_reserve =  ISNULL(@Reserve,0) - ISNULL(@TotalThisReserve,0),  
     this_payment =  ISNULL(@Payment,0) - ISNULL(@TotalThisPayment,0)  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'R'  
  END  
  
  IF @Recovery = 1  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_salvage =  ISNULL(@Payment,0) - ISNULL(@TotalThisPayment,0)  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'R'  
  END  
  IF @Recovery = 0  
  BEGIN  
   UPDATE  claim_ri_arrangement_line  
   SET  
     this_recovery =  ISNULL(@Payment,0) - ISNULL(@TotalThisPayment,0)  
   WHERE   claim_id = @claim_id  
   AND     ri_arrangement_id = @ri_arrangement_id  
   AND     type = 'R'  
  END  
 END  
 --START PN69495  
 ELSE  
 BEGIN  
     UPDATE  claim_ri_arrangement_line  
  SET     this_share_percent = ISNULL(this_share_percent, default_share_percent),  
    this_reserve = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @reserve)),  
    this_payment = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @payment))  
  WHERE   claim_id = @claim_id  
  AND     ri_arrangement_id = @ri_arrangement_id  
 END  
 --END PN69495  
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
