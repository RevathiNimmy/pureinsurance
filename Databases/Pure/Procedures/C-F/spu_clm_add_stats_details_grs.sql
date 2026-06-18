SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_add_stats_details_grs'
GO

CREATE PROCEDURE spu_clm_add_stats_details_grs    
 @claimpayment_id INT OUTPUT,    
 @ThisRevesionPresent INT OUTPUT,    
    @stats_folder_cnt INT,    
    @transaction_type_code CHAR(10),    
    @claim_id INT,    
    @StatsDetailType CHAR(10),    
    @CreditAccountCode VARCHAR(30) ,    
    @TaxAmount Numeric(19,4)=0,  
    @PerilId INT = 0    
AS    
    
DECLARE    
 @peril_type_id INT,    
 @peril_type_code CHAR(10),    
 @peril_description VARCHAR(30),    
    
 @annual_premium NUMERIC(19,4),    
 @currency_rate FLOAT,    
 @system_rate FLOAT,    
 @this_premium_original  NUMERIC(19,4),    
 @this_premium_home  NUMERIC(19,4),    
 @this_premium_system  NUMERIC(19,4),    
 @sum_insured  NUMERIC(19,4),    
 @sum_insured_home  NUMERIC(19,4),    
 @sum_insured_system  NUMERIC(19,4),    
 @sum_insured_total  NUMERIC(19,4),    
    
 @risk_id INT,    
 @risk_type_id INT,    
 @risk_type_code CHAR(10),    
 @currency_code CHAR(10),    
 @currency_id INT,    
 @company_id INT,    
 @return_status INT,    
 @payment_id INT,    
 @receipt_id int ,    
    
 @peril_id INT,    
 @stats_detail_type VARCHAR(3),    
 @class_of_business_id INT,    
 @class_of_business_code VARCHAR(10),    
 @ri_party_cnt INT,    
 @ri_shortname VARCHAR(20),    
 @transaction_amount  NUMERIC(19,4),    
 @documenttype_id INT,    
 @bClaimsIsPostTaxes INT,
 @claim_is_post_taxes_separately TINYINT
    
 SELECT @payment_id = NULL    
 SELECT @stats_detail_type=@StatsDetailType    
 SELECT @claimpayment_id=0    
    
  --SELECT DOCUMENT TYPE ID    
  SELECT @DocumentType_id =(SELECT    
       CASE @transaction_type_code    
       WHEN 'C_CP' THEN 28    
       WHEN 'C_CR' THEN 41    
       WHEN 'C_CO' THEN 35    
       WHEN 'C_SA' THEN 29    
       WHEN 'C_RV' THEN 29    
       END)    
    
 /*Get Company ID from stats detail*/    
 SELECT    
  @company_id = source_id    
 FROM Stats_Folder    
 WHERE stats_folder_cnt = @stats_folder_cnt    
    
 IF @@Error <> 0    
  GOTO Err_Add_Stats_Details    
    
 -- Set claim details    
 SELECT  @risk_id = rsk.risk_cnt,    
   @risk_type_id = rty.risk_type_id,    
   @risk_type_code = rty.code,    
   @sum_insured_total = rsk.total_sum_insured,
   @claim_is_post_taxes_separately = rty.claims_is_post_taxes
 FROM    claim clm    
 JOIN    risk rsk ON rsk.risk_cnt = clm.Risk_type_id    
 JOIN    risk_type rty ON rty.risk_type_id = rsk.risk_type_id    
 WHERE   clm.Claim_id = @claim_id    
    
 IF @@Error <> 0    
  GOTO Err_Add_Stats_Details    
    
 IF @documenttype_id = 28    
 BEGIN    
  /*Get Payment Details*/    
  -- note at the moment although we have moved the    
  -- rates to the item level they will be the same for each item    
  -- as they are only collected once.    
    
  IF @PerilId <> 0   
   BEGIN   
    SELECT    
     @payment_id = wcp.claim_payment_id,    
     @currency_id = wcp.currency_id,    
     @currency_rate = ISNULL(MIN(wcpi.currency_base_xrate),0),    
     @system_rate = ISNULL(MIN(wcpi.system_base_xrate),0)    
    FROM claim_payment wcp    
    INNER JOIN claim_payment_item wcpi ON    
    wcp.claim_payment_id = wcpi.claim_payment_id    
    WHERE claim_id = @claim_id    
   AND wcp.claim_payment_id = base_claim_payment_id AND wcp.claim_peril_id = @PerilId    
    GROUP BY wcp.claim_payment_id, wcp.currency_id  
    END  
   ELSE  
    BEGIN  
    SELECT    
     @payment_id = wcp.claim_payment_id,    
     @currency_id = wcp.currency_id,    
     @currency_rate = ISNULL(MIN(wcpi.currency_base_xrate),0),    
     @system_rate = ISNULL(MIN(wcpi.system_base_xrate),0)    
    FROM claim_payment wcp    
    INNER JOIN claim_payment_item wcpi ON    
    wcp.claim_payment_id = wcpi.claim_payment_id    
    WHERE claim_id = @claim_id    
   AND wcp.claim_payment_id = base_claim_payment_id   
    GROUP BY wcp.claim_payment_id, wcp.currency_id  
    END  
          
 END    
    
 IF @documenttype_id = 29    
 BEGIN    
   /*Get Receipt Details*/    
  IF @PerilId <> 0   
   BEGIN   
    SELECT    
     @receipt_id = wcr.claim_receipt_id,    
     @currency_id = wcr.currency_id,    
     @currency_rate = ISNULL(MIN(wcri.currency_base_xrate),0),    
     @system_rate = ISNULL(MIN(wcri.system_base_xrate),0)    
     FROM claim_receipt wcr    
   INNER JOIN claim_receipt_item wcri ON    
   wcr.claim_receipt_id = wcri.claim_receipt_id  AND wcr.claim_peril_id = @PerilId    
    WHERE claim_id = @claim_id    
     AND wcr.claim_receipt_id = base_claim_receipt_id    
    GROUP By wcr.claim_receipt_id, wcr.currency_id    
    END  
   ELSE  
    BEGIN  
     SELECT    
     @receipt_id = wcr.claim_receipt_id,    
     @currency_id = wcr.currency_id,    
     @currency_rate = ISNULL(MIN(wcri.currency_base_xrate),0),    
     @system_rate = ISNULL(MIN(wcri.system_base_xrate),0)    
     FROM claim_receipt wcr    
   INNER JOIN claim_receipt_item wcri ON    
   wcr.claim_receipt_id = wcri.claim_receipt_id    
    WHERE claim_id = @claim_id    
     AND wcr.claim_receipt_id = base_claim_receipt_id    
    GROUP By wcr.claim_receipt_id, wcr.currency_id    
    END    
   END    
    
 IF @documenttype_id NOT IN (28,29)    
 BEGIN    
   /*Get Reserve Details*/    
   SELECT    
    @currency_id = currency_id,    
    @currency_rate = ISNULL(currency_base_xrate,0),    
    @system_rate = ISNULL(system_base_xrate,0)    
   FROM claim    
   WHERE claim_id = @claim_id    
 END    
    
 IF @@Error <> 0    
  GOTO Err_Add_Stats_Details    
    
 /*Get details about the currency*/    
 SELECT    
  @currency_code = LTRIM(RTRIM(code  ))    
 FROM currency    
 WHERE currency_id = @currency_id    
    
 IF @@Error <> 0    
  GOTO Err_Add_Stats_Details    
    
--@peril_id    
IF @DocumentType_id=28    
BEGIN    
    
 SELECT @bClaimsIsPostTaxes = ISNULL(risk_type.claims_is_post_taxes,0)    
 FROM claim    
 LEFT JOIN risk ON claim.risk_type_id = risk.risk_cnt    
 LEFT JOIN risk_type ON risk.risk_type_id = risk_type.risk_type_id    
 WHERE claim_id = @claim_id    
    
   
 IF @PerilId <> 0  
 BEGIN  
   SELECT @peril_id = CP.CLAIM_PERIL_ID ,    
   @class_of_business_id =COB.class_of_business_id ,    
   @class_of_business_code =COB.code ,    
   @ri_party_cnt=P.party_cnt  ,    
   @ri_shortname =P.shortname ,    
   @transaction_amount=  (CASE 
								WHEN ISNULL(@claim_is_post_taxes_separately,0) = 0 
								THEN CP.amount + ISNULL(CP.tax_amount_WHT,0)
								Else CP.amount
							END),    
   @claimpayment_id=CP.claim_payment_id ,    
		@ThisRevesionPresent=(CASE
							WHEN ROUND(res.this_revision,2) <>0 THEN  1
							ELSE 0
							END),
   @peril_description = cpr.description,    
   @peril_type_code = pt.code,    
   @peril_type_id = pt.peril_type_id,    
   @sum_insured = cpr.sum_insured    
  FROM Claim_Payment_Item cpi    
  LEFT OUTER JOIN Claim_payment cp ON cp.claim_payment_id =cpi.claim_payment_id    
  LEFT OUTER JOIN Claim_Peril cpr ON cpr.Claim_Peril_id =cp.claim_peril_id AND cpr.Claim_id =cp.claim_id    
  LEFT OUTER JOIN Peril_Type pt ON pt.peril_type_id =cpr.Peril_type_id    
  LEFT OUTER JOIN Class_Of_Business cob ON cob.class_of_business_id =pt.class_of_business_id    
  LEFT OUTER JOIN Party P ON p.party_cnt =cp.party_cnt    
  LEFT OUTER JOIN Reserve Res ON Res.Reserve_id =cpi.reserve_id    
  WHERE CP.claim_id =@CLAIM_ID AND CPI.this_payment <>0    
  AND CP.claim_payment_id = CP.base_claim_payment_id  AND cpr.claim_peril_id = @PerilId    
 END  
  ELSE  
     BEGIN  
   SELECT @peril_id = CP.CLAIM_PERIL_ID ,    
   @class_of_business_id =COB.class_of_business_id ,    
   @class_of_business_code =COB.code ,    
   @ri_party_cnt=P.party_cnt  ,    
   @ri_shortname =P.shortname ,    
   @transaction_amount=(CASE 
								WHEN ISNULL(@claim_is_post_taxes_separately,0) = 0 
								THEN CP.amount + ISNULL(CP.tax_amount_WHT,0)
								Else CP.amount
							END),    
   @claimpayment_id=CP.claim_payment_id ,    
  		@ThisRevesionPresent=(CASE
							WHEN ROUND(res.this_revision,2) <>0 THEN  1
							ELSE 0
							END),   
   @peril_description = cpr.description,    
   @peril_type_code = pt.code,    
   @peril_type_id = pt.peril_type_id,    
   @sum_insured = cpr.sum_insured    
  FROM Claim_Payment_Item cpi    
  LEFT OUTER JOIN Claim_payment cp ON cp.claim_payment_id =cpi.claim_payment_id    
  LEFT OUTER JOIN Claim_Peril cpr ON cpr.Claim_Peril_id =cp.claim_peril_id AND cpr.Claim_id =cp.claim_id    
  LEFT OUTER JOIN Peril_Type pt ON pt.peril_type_id =cpr.Peril_type_id    
  LEFT OUTER JOIN Class_Of_Business cob ON cob.class_of_business_id =pt.class_of_business_id    
  LEFT OUTER JOIN Party P ON p.party_cnt =cp.party_cnt    
  LEFT OUTER JOIN Reserve Res ON Res.Reserve_id =cpi.reserve_id    
  WHERE CP.claim_id =@CLAIM_ID AND CPI.this_payment <>0    
  AND CP.claim_payment_id = CP.base_claim_payment_id    
 END  
    
 IF ISNULL(@ri_party_cnt,0) =0  AND @StatsDetailType='GRS'    
 SELECT @ri_shortname='CLMPAYABLE'    
    
 IF  @StatsDetailType='TAG' AND @TaxAmount<>0    
 BEGIN    
  SELECT @ri_shortname=@CreditAccountCode    
  SELECT @transaction_amount=@TaxAmount    
 END    
END    
ELSE IF @documenttype_id = 29    
BEGIN    
  IF @PerilId <> 0  
    BEGIN  
  SELECT @peril_id = CP.CLAIM_PERIL_ID ,    
   @class_of_business_id = COB.class_of_business_id ,    
   @class_of_business_code = COB.code ,    
   @ri_party_cnt=P.party_cnt  ,    
   @ri_shortname =P.shortname ,    
   @transaction_amount = CP.amount,    
   @claimpayment_id=CP.claim_receipt_id,    
   @peril_description = cpr.description,    
   @peril_type_code = pt.code,    
   @peril_type_id = pt.peril_type_id,    
   @sum_insured = cpr.sum_insured    
  FROM Claim_Receipt_Item cpi    
  LEFT OUTER JOIN Claim_Receipt  cp ON cp.Claim_Receipt_id = cpi.Claim_Receipt_id    
  LEFT OUTER JOIN Claim_Peril cpr ON cpr.Claim_Peril_id = cp.claim_peril_id AND cpr.Claim_id =cp.claim_id    
  LEFT OUTER JOIN Peril_Type pt ON pt.peril_type_id = cpr.Peril_type_id    
  LEFT OUTER JOIN Class_Of_Business cob ON cob.class_of_business_id =pt.class_of_business_id    
  LEFT OUTER JOIN Party P ON p.party_cnt =cp.party_cnt    
  WHERE CP.claim_id =@CLAIM_ID AND CPI.this_receipt <> 0    
  AND cp.claim_receipt_id=cp.base_claim_receipt_id  AND cpr.claim_peril_id = @PerilId    
 END  
  ELSE  
    BEGIN  
  SELECT @peril_id = CP.CLAIM_PERIL_ID ,    
   @class_of_business_id = COB.class_of_business_id ,    
   @class_of_business_code = COB.code ,    
   @ri_party_cnt=P.party_cnt  ,    
   @ri_shortname =P.shortname ,    
   @transaction_amount = CP.amount,    
   @claimpayment_id=CP.claim_receipt_id,    
   @peril_description = cpr.description,    
   @peril_type_code = pt.code,    
   @peril_type_id = pt.peril_type_id,    
   @sum_insured = cpr.sum_insured    
  FROM Claim_Receipt_Item cpi    
  LEFT OUTER JOIN Claim_Receipt  cp ON cp.Claim_Receipt_id = cpi.Claim_Receipt_id    
  LEFT OUTER JOIN Claim_Peril cpr ON cpr.Claim_Peril_id = cp.claim_peril_id AND cpr.Claim_id =cp.claim_id    
  LEFT OUTER JOIN Peril_Type pt ON pt.peril_type_id = cpr.Peril_type_id    
  LEFT OUTER JOIN Class_Of_Business cob ON cob.class_of_business_id =pt.class_of_business_id    
  LEFT OUTER JOIN Party P ON p.party_cnt =cp.party_cnt    
  WHERE CP.claim_id =@CLAIM_ID AND CPI.this_receipt <> 0    
  AND cp.claim_receipt_id=cp.base_claim_receipt_id    
   
 END    
 IF ISNULL(@ri_party_cnt,0) = 0 AND @StatsDetailType='GRS'   
  SELECT @ri_shortname='CLMRECEIVABLE'    
    
 IF  @StatsDetailType='TAN' AND @TaxAmount<>0    
 BEGIN    
  SELECT @ri_shortname=@CreditAccountCode    
  SELECT @ri_party_cnt=0    
  SELECT @transaction_amount=@TaxAmount    
 END    
    
 IF  @StatsDetailType='TAG' AND @TaxAmount<>0    
 BEGIN    
  SELECT @transaction_amount=@TaxAmount    
 END    
END    
ELSE IF @documenttype_id =35 or @documenttype_id =41    
BEGIN    
 SELECT @ri_party_cnt=0, @ThisRevesionPresent=0    
    
 -- cursor only needed in case of Open\Maintain claims as there are chances of multiple peril edit    
 IF @PerilId <> 0  
   BEGIN  
   DECLARE c_stats_detail CURSOR FAST_FORWARD FOR    
   SELECT CPR.CLAIM_PERIL_ID,    
    COB.class_of_business_id,    
    COB.code,    
    @CreditAccountCode + COB.code,    
    SUM(RSV.this_revision),    
    cpr.description,    
    pt.code,    
    pt.peril_type_id,    
    AVG(cpr.sum_insured)    
   FROM Reserve RSV    
   LEFT OUTER JOIN Claim_Peril cpr ON cpr.Claim_Peril_id  =RSV.claim_peril_id    
   LEFT OUTER JOIN Peril_Type pt ON pt.peril_type_id =cpr.Peril_type_id    
   LEFT OUTER JOIN Class_Of_Business cob ON cob.class_of_business_id =pt.class_of_business_id    
   WHERE cpr.claim_id = @CLAIM_ID AND RSV.this_revision <> 0 AND  cpr.claim_peril_id = @PerilId  
   GROUP BY CPR.CLAIM_PERIL_ID,COB.class_of_business_id, COB.code, cpr.description, pt.code, pt.peril_type_id    
   END  
 ELSE  
     BEGIN  
   DECLARE c_stats_detail CURSOR FAST_FORWARD FOR    
   SELECT CPR.CLAIM_PERIL_ID,    
    COB.class_of_business_id,    
    COB.code,    
    @CreditAccountCode + COB.code,    
    SUM(RSV.this_revision),    
    cpr.description,    
    pt.code,    
    pt.peril_type_id,    
    AVG(cpr.sum_insured)    
   FROM Reserve RSV    
   LEFT OUTER JOIN Claim_Peril cpr ON cpr.Claim_Peril_id  =RSV.claim_peril_id    
   LEFT OUTER JOIN Peril_Type pt ON pt.peril_type_id =cpr.Peril_type_id    
   LEFT OUTER JOIN Class_Of_Business cob ON cob.class_of_business_id =pt.class_of_business_id    
   WHERE cpr.claim_id = @CLAIM_ID AND RSV.this_revision <> 0    
   GROUP BY CPR.CLAIM_PERIL_ID,COB.class_of_business_id, COB.code, cpr.description, pt.code, pt.peril_type_id    
    END   
    
    OPEN c_stats_detail    
    FETCH NEXT FROM c_stats_detail    
        INTO    @peril_id, @class_of_business_id, @class_of_business_code, @ri_shortname,    
    @transaction_amount, @peril_description, @peril_type_code, @peril_type_id, @sum_insured    
    
    WHILE (@@FETCH_STATUS = 0) BEGIN    
  SELECT @this_premium_original = @transaction_amount    
    
  /*Calculate home and system currency values*/    
  EXEC spu_ACT_Do_Currency_Conversion    
    @company_id = @company_id,    
    @currency_id = @currency_id,    
    @currency_amount_unrounded = @sum_insured,    
    @mode = 'ALL',    
    @base_amount = @sum_insured_home OUTPUT,    
    @system_amount = @sum_insured_system OUTPUT,    
    @currency_base_xrate = @currency_rate OUTPUT,    
    @system_base_xrate = @system_rate OUTPUT,    
    @return_status = @return_status OUTPUT    
    
  EXEC spu_ACT_Do_Currency_Conversion    
    @company_id = @company_id,    
    @currency_id = @currency_id,    
    @currency_amount_unrounded = @this_premium_original,    
    @mode = 'ALL',    
    @base_amount = @this_premium_home OUTPUT,    
    @system_amount = @this_premium_system OUTPUT,    
    @currency_base_xrate = @currency_rate OUTPUT,    
    @system_base_xrate = @system_rate OUTPUT,    
    @return_status = @return_status OUTPUT    
    
  IF @@Error <> 0    
   GOTO Err_Add_Stats_Details    
    
  -- Insert the Stats Detail    
  INSERT INTO Stats_Detail    
  (    
   stats_folder_cnt,    
   stats_detail_id,    
   stats_detail_type,    
   risk_id,    
   risk_type_id,    
   risk_type_code,    
   peril_description,    
   peril_type_id,    
   peril_type_code,    
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
   sum_insured_home,    
   sum_insured_system,    
   sum_insured_total    
  )    
  SELECT    
   @stats_folder_cnt,    
   ISNULL(MAX(stats_detail_id), 0) + 1,    
   @stats_detail_type,    
   @risk_id,    
   @risk_type_id,    
   @risk_type_code,    
   @peril_description,    
   @peril_type_id,    
   @peril_type_code,    
   @class_of_business_id,    
   @class_of_business_code,    
   @ri_party_cnt,    
   @ri_shortname,    
   0,    
   0,    
   @transaction_amount,    
   @currency_code,    
   @currency_rate,    
   @this_premium_original,    
   @this_premium_home,    
   @this_premium_system,    
   @sum_insured_home,    
   @sum_insured_system,    
   @sum_insured_total    
  FROM Stats_Detail    
  WHERE stats_folder_cnt = @stats_folder_cnt    
    
  IF @@Error <> 0    
   GOTO Err_Add_Stats_Details    
    
  FETCH NEXT FROM c_stats_detail    
   INTO    @peril_id, @class_of_business_id, @class_of_business_code, @ri_shortname,    
     @transaction_amount, @peril_description, @peril_type_code, @peril_type_id, @sum_insured    
 END    
    
 CLOSE c_stats_detail    
 DEALLOCATE c_stats_detail    
    
 -- terminate reserves postings here    
 RETURN    
END    
    
SELECT @this_premium_original = @transaction_amount    
    
/*Calculate home and system currency values*/    
EXEC spu_ACT_Do_Currency_Conversion    
  @company_id = @company_id,    
  @currency_id = @currency_id,    
  @currency_amount_unrounded = @sum_insured,    
  @mode = 'ALL',    
  @base_amount = @sum_insured_home OUTPUT,    
  @system_amount = @sum_insured_system OUTPUT,    
  @currency_base_xrate = @currency_rate OUTPUT,    
  @system_base_xrate = @system_rate OUTPUT,    
  @return_status = @return_status OUTPUT    
    
EXEC spu_ACT_Do_Currency_Conversion    
  @company_id = @company_id,    
  @currency_id = @currency_id,    
  @currency_amount_unrounded = @this_premium_original,    
  @mode = 'ALL',    
  @base_amount = @this_premium_home OUTPUT,    
  @system_amount = @this_premium_system OUTPUT,    
  @currency_base_xrate = @currency_rate OUTPUT,    
  @system_base_xrate = @system_rate OUTPUT,    
  @return_status = @return_status OUTPUT    
    
IF @@Error <> 0    
 GOTO Err_Add_Stats_Details    
    
-- Insert the Stats Detail    
INSERT INTO Stats_Detail    
(    
 stats_folder_cnt,    
 stats_detail_id,    
 stats_detail_type,    
 risk_id,    
 risk_type_id,    
 risk_type_code,    
 peril_description,    
 peril_type_id,    
 peril_type_code,    
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
 sum_insured_home,    
 sum_insured_system,    
 sum_insured_total    
)    
SELECT    
 @stats_folder_cnt,    
 ISNULL(MAX(stats_detail_id), 0) + 1,    
 @stats_detail_type,    
 @risk_id,    
 @risk_type_id,    
 @risk_type_code,    
 @peril_description,    
 @peril_type_id,    
 @peril_type_code,    
 @class_of_business_id,    
 @class_of_business_code,    
 @ri_party_cnt,    
 @ri_shortname,    
 0,    
 0,    
 @transaction_amount,    
 @currency_code,    
 @currency_rate,    
 @this_premium_original,    
 @this_premium_home,    
 @this_premium_system,    
 @sum_insured_home,    
 @sum_insured_system,    
 @sum_insured_total    
FROM Stats_Detail    
WHERE stats_folder_cnt = @stats_folder_cnt    
    
UPDATE Stats_Folder    
SET payment_id=@payment_id ,    
    receipt_id=@receipt_id    
WHERE stats_folder_cnt = @stats_folder_cnt    
    
IF @@Error <> 0    
 GOTO Err_Add_Stats_Details    
    
RETURN    
    
Err_Add_Stats_Details:    
    
-- Delete all stats details for this folder    
DELETE FROM Stats_Detail    
 WHERE stats_folder_cnt = @stats_folder_cnt    
    
-- Delete the stats folder record    
DELETE FROM Stats_Folder    
 WHERE stats_folder_cnt = @stats_folder_cnt    
    
RETURN 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
