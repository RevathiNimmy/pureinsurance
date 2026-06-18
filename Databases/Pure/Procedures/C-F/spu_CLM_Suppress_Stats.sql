SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Suppress_Stats'
GO

CREATE PROCEDURE spu_CLM_Suppress_Stats
@claim_id int,  
@transaction_type_code varchar(10),  
@suppress int OUTPUT  
  
AS  
  
BEGIN  
  
 DECLARE @suppress_reserves_product int  
 DECLARE @suppress_payments_product int  
 DECLARE @suppress_recoveries_product int  
 DECLARE @suppress_reserves_claims int  
 DECLARE @suppress_payments_claims int  
 DECLARE @suppress_recoveries_claims int  
  
 DECLARE @suppress_reserves int  
 DECLARE @suppress_payments int  
 DECLARE @suppress_recoveries int  
  
 -- default suppression indicator to zero  
 SET @suppress = 0  
  
 -- get the suppression indicators from product and claim  
 SELECT  
  @suppress_payments_product =ISNULL(p.suppress_payments,0),  
  @suppress_recoveries_product = ISNULL(p.suppress_recoveries,0),  
  @suppress_reserves_claims = ISNULL(c.suppress_reserves,0),  
  @suppress_payments_claims = ISNULL(c.suppress_payments,0),  
  @suppress_recoveries_claims = ISNULL(c.suppress_recoveries,0)  
  
 FROM claim c  
  
  INNER JOIN insurance_file ifile ON  
   ifile.insurance_file_cnt =c.policy_id  
  
   INNER JOIN product p ON  
    p.product_id = ifile.product_id  
  
 WHERE claim_id = @claim_id  
  
 -- always use the reserve suppression indicator from the claim  
 SET @suppress_reserves = @suppress_reserves_claims  
  
 -- initially use payment suppression indicator from the claim  
 SET @suppress_payments = @suppress_payments_claims  
  
 -- if the initial value is dont suppress  
 IF @suppress_payments = 0  
  -- use the suppress payments indicator from the product  
  SET @suppress_payments = @suppress_payments_product  
  
 -- initially use recoveries suppression indicator from the claim  
 SET @suppress_recoveries = @suppress_recoveries_claims  
  
 -- if the initial value is dont suppress  
 IF @suppress_recoveries = 0  
  -- use the suppress recoveries indicator from the product  
  SET @suppress_recoveries = @suppress_recoveries_product  
  
 -- determine whether the copy work stats to live call needs to be suppressed  
 IF @transaction_type_code = 'C_CO' AND @suppress_reserves = 1  
  SET @suppress = 1  
  
 IF @transaction_type_code = 'C_CR' AND @suppress_reserves = 1  
  SET @suppress = 1  
  
 IF @transaction_type_code = 'C_CP' AND @suppress_payments = 1  
  SET @suppress = 1  
  
 IF @transaction_type_code = 'C_SA' AND @suppress_recoveries = 1  
  SET @suppress = 1  
  
 IF @transaction_type_code = 'C_RV' AND @suppress_recoveries = 1  
  SET @suppress = 1  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
