SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_stats_details_claims'
GO

CREATE PROCEDURE spu_add_stats_details_claims  
    @stats_folder_cnt INT,  
    @claim_id INT,  
    @peril_id INT,  
    @stats_detail_type VARCHAR(3),  
    @class_of_business_id INT,  
    @class_of_business_code VARCHAR(10),  
    @ri_party_cnt INT,  
    @ri_shortname VARCHAR(20),  
    @ri_party_type INT,  
    @ri_share_percent FLOAT,  
    @transaction_amount MONEY,  
    @documenttype_id INT  
AS  
  
-- *************************************************************************************************  
-- Name: sp_add_stats_details_claims    creates a Stats_detail record for the current transaction  
--  
-- Version: 1.00.0000  
--          1.00.0001    Don't write out peril id & description. Also get  
--                       currency_rate from Orion.                           29/08/01    RWH  
--          1.00.0002    Get the sum insured from claim_peril.          10/09/01    RWH  
--          1.00.0003    Re-include peril description and populate  
--                       sum_insured_total.                                  24/09/01    RWH  
-- *************************************************************************************************  
  
DECLARE  
 @peril_type_id INT,  
 @peril_type_code CHAR(10),  
 @peril_description VARCHAR(30),  
  
 @annual_premium MONEY,  
 @currency_rate FLOAT,  
 @system_rate FLOAT,  
 @this_premium_original MONEY,  
 @this_premium_home MONEY,  
 @this_premium_system MONEY,  
 @sum_insured MONEY,  
 @sum_insured_home MONEY,  
 @sum_insured_system MONEY,  
 @sum_insured_total MONEY,  
  
 @risk_id INT,  
 @risk_type_id INT,  
 @risk_type_code CHAR(10),  
 @currency_code CHAR(10),  
 @currency_id INT,  
 @company_id INT,  
 @return_status INT,  
 @payment_id INT,  
 @receipt_id int,  
 @Is_Excess int  
  
SELECT @payment_id = NULL  
  
-- Set claim details  
SELECT  @risk_id = rsk.risk_cnt,  
   @risk_type_id = rty.risk_type_id,  
   @risk_type_code = rty.code,  
   @sum_insured_total = rsk.total_sum_insured  
FROM    claim clm  
JOIN    risk rsk ON rsk.risk_cnt = clm.Risk_type_id  
JOIN    risk_type rty ON rty.risk_type_id = rsk.risk_type_id  
WHERE   clm.Claim_id = @claim_id  
  
IF @@Error <> 0  
 GOTO Err_Add_Stats_Details  
  
-- Get claim peril details  
SELECT  @peril_description = clp.description,  
   @peril_type_code = pty.code,  
   @peril_type_id = pty.peril_type_id,  
   @sum_insured = clp.sum_insured  
FROM    claim_peril clp  
JOIN    peril_type pty ON pty.peril_type_id = clp.peril_type_id  
WHERE   claim_peril_id = @peril_id  
  
IF @@Error <> 0  
 GOTO Err_Add_Stats_Details  
  
IF @documenttype_id = 28  
BEGIN  
 /*Get Payment Details*/  
-- note at the moment although we have moved the  
-- rates to the item level they will be the same for each item  
-- as they are only collected once.  
 SELECT  
   @payment_id = wcp.claim_payment_id,  
   @currency_id = wcp.currency_id,  
   @currency_rate = ISNULL(MIN(currency_base_xrate),0),  
   @system_rate = ISNULL(MIN(system_base_xrate),0)  
 FROM claim_payment wcp  
 INNER JOIN claim_payment_item wcpi ON  
   wcp.claim_payment_id = wcpi.claim_payment_id  
 WHERE claim_id = @claim_id  
 AND wcp.claim_payment_id = base_claim_payment_id  AND wcp.claim_peril_id = @peril_id 
 GROUP BY wcp.claim_payment_id, wcp.currency_id  
  
 --Check payment type is Excess or not.  
 SELECT @Is_Excess=Isnull(ResT.Is_Excess,0)  
 FROM claim_payment wcp  
 INNER JOIN claim_payment_item wcpi ON wcp.claim_payment_id = wcpi.claim_payment_id  
 INNER JOIN Reserve Res ON Res.Reserve_id =wcpi.reserve_id and IsNull(Res.this_payment,0)<>0  
 INNER JOIN Reserve_type ResT ON res.Reserve_type_id=rest.Reserve_type_id  
 WHERE  wcp.claim_payment_id=@payment_id  
 AND wcp.claim_payment_id = base_claim_payment_id  
  
If @Is_Excess=1  
Begin  
 Set @transaction_amount=@transaction_amount  
End  
  
END  
  
IF @documenttype_id = 29  
BEGIN  
 /*Get Receipt Details*/  
 SELECT  
 @receipt_id = wcr.claim_receipt_id,  
  @currency_id = wcr.currency_id,  
 @currency_rate = ISNULL(MIN(currency_base_xrate),0),  
 @system_rate = ISNULL(MIN(system_base_xrate),0)  
 FROM claim_receipt wcr  
  INNER JOIN claim_receipt_item wcri ON  
  wcr.claim_receipt_id = wcri.claim_receipt_id   
WHERE claim_id = @claim_id  
AND wcr.claim_receipt_id = base_claim_receipt_id AND wcr.claim_peril_id = @peril_id  
GROUP By wcr.claim_receipt_id, wcr.currency_id  
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
  
/*Get Company ID from stats detail*/  
SELECT  
 @company_id = source_id  
FROM Stats_Folder  
WHERE stats_folder_cnt = @stats_folder_cnt  
  
IF @@Error <> 0  
 GOTO Err_Add_Stats_Details  
  
-- Calculate share of transaction.  
IF (@ri_share_percent <> 0)  
 SELECT @this_premium_original = @transaction_amount * @ri_share_percent / 100  
ELSE  
 SELECT @this_premium_original = @transaction_amount  
  
IF @@Error <> 0  
 GOTO Err_Add_Stats_Details  
  
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
 @ri_party_type,  
 @ri_share_percent,  
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
