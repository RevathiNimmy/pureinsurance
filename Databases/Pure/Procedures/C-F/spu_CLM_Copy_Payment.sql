SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Copy_Payment'
GO

CREATE PROCEDURE spu_CLM_Copy_Payment  
 @claim_id int,  
 @copy_claim_id int,  
 @claim_peril_id int,  
 @copy_claim_peril_id int,  
 @version_id int,  
 @status int OUTPUT  
  
AS  
  
BEGIN  
 -- *********************************************************  
 -- * copy all live claim payment entries to the work table *  
 -- *********************************************************  
 INSERT INTO claim_payment (  
  claim_id,  
  claim_peril_id,  
  date_of_payment,  
  amount,  
  tax_amount,  
  tax_amount_WHT,  
  party_cnt,  
  comments,  
  is_referred,  
  created_by,  
  PayeeMediaType,  
  PayeeName,  
  PayeeBankName,  
  PayeeSortCode,  
  PayeeAccountNo,  
  PayeeCountry,  
  PayeeComments,  
  SequenceNo,  
  treaty_id,  
  claim_payment_to_id,  
  payment_party_to,  
  insured_domiciled,  
  insured_percentage,  
  insured_tax_number,  
  payee_domiciled,  
  payee_percentage,  
  payee_tax_number,  
  safe_harbour_id,  
  safe_harbour_percentage,  
  is_tax_exempt,  
  is_wht_exempt,  
  is_settlement,  
  document_id,  
  media_ref,  
  currency_id,  
  excess_amount,  
  payeeaddress1,  
  payeeaddress2,  
  payeeaddress3,  
  payeeaddress4,  
  payeepostalcode,  
  thirdpartyreference,  
  base_claim_payment_id,  
  version_id,
  business_identifier_code,
  international_bank_account_number)  
 SELECT  
  @copy_claim_id,  
  @copy_claim_peril_id,  
  date_of_payment,  
  amount,  
  tax_amount,  
  tax_amount_WHT,  
  party_cnt,  
  comments,  
  0,  
  created_by,  
  PayeeMediaType,  
  PayeeName,  
  PayeeBankName,  
  PayeeSortCode,  
  PayeeAccountNo,  
  PayeeCountry,  
  PayeeComments,  
  SequenceNo,  
  treaty_id,  
  claim_payment_to_id,  
  payment_party_to,  
  insured_domiciled,  
  insured_percentage,  
  insured_tax_number,  
  payee_domiciled,  
  payee_percentage,  
  payee_tax_number,  
  safe_harbour_id,  
  safe_harbour_percentage,  
  is_tax_exempt,  
  is_wht_exempt,  
  is_settlement,  
  document_id,  
  media_ref,  
  currency_id,  
  excess_amount,  
  payeeaddress1,  
  payeeaddress2,  
  payeeaddress3,  
  payeeaddress4,  
  payeepostalcode,  
  thirdpartyreference,  
  base_claim_payment_id,  
  @version_id,
  business_identifier_code,
  international_bank_account_number  
 FROM claim_payment WITH (NOLOCK)  
 WHERE claim_id = @claim_id  
 AND claim_peril_id = @claim_peril_id  
  
 IF @@ERROR <> 0  
  GOTO Error_Routine  
  
  
 -- ***********************************************************  
 -- * copy all claim_payment_item records  to the work tables *  
 -- ***********************************************************  
 INSERT INTO claim_payment_item  
 (  
  claim_payment_id,  
  reserve_id,  
  recovery_id,  
  recovery_type_id,  
  currency_id,  
  tax_group_id,  
  this_payment,  
  tax_amount,  
  tax_amount_WHT,  
  exchange_rate_override_reason_id,  
  currency_base_xrate,  
  currency_base_date,  
  account_base_xrate,  
  account_base_date,  
  system_base_xrate,  
  system_base_date,  
  payment_loss_xrate,  
  base_claim_payment_item_id,  
  version_id  
 )  
 SELECT  
  copy_claim_payment.claim_payment_id,  
  copy_reserve.reserve_id,  
  copy_recovery.recovery_id,  
  cpi.recovery_type_id,  
  cpi.currency_id,  
  cpi.tax_group_id,  
  cpi.this_payment,  
  cpi.tax_amount,  
  cpi.tax_amount_WHT,  
  cpi.exchange_rate_override_reason_id,  
  cpi.currency_base_xrate,  
  cpi.currency_base_date,  
  cpi.account_base_xrate,  
  cpi.account_base_date,  
  cpi.system_base_xrate,  
  cpi.system_base_date,  
  cpi.payment_loss_xrate,  
  cpi.base_claim_payment_item_id,  
  @version_id  
  
 FROM claim_payment_item cpi WITH (NOLOCK)  
  
      -- inner join to get the id of the new version of the claim payment  
  INNER JOIN claim_payment WITH (NOLOCK) ON  
   cpi.claim_payment_id = claim_payment.claim_payment_id  
  
   INNER JOIN (  
    SELECT claim_payment_id, version_id, base_claim_payment_id  
    FROM claim_payment  WITH (NOLOCK) 
    WHERE claim_peril_id = @copy_claim_peril_id  
        AND claim_id = @copy_claim_id  
    AND version_id = @version_id AND ISNULL(is_referred,0)<>2) copy_claim_payment ON  
     copy_claim_payment.base_claim_payment_id = claim_payment.base_claim_payment_id  
  
      -- inner join to get the id of the new version of the recovery  
  LEFT JOIN recovery WITH (NOLOCK) ON  
   cpi.recovery_id = recovery.recovery_id  
  
   LEFT JOIN (  
    SELECT recovery_id, version_id, base_recovery_id  
    FROM recovery WITH (NOLOCK)  
    WHERE claim_peril_id = @copy_claim_peril_id  
    AND version_id = @version_id) copy_recovery ON  
     copy_recovery.base_recovery_id  = recovery.base_recovery_id  
  
      -- inner join to get the id of the new version of the reserve  
  LEFT JOIN reserve WITH (NOLOCK) ON  
   cpi.reserve_id = reserve.reserve_id  
  
   LEFT JOIN (  
    SELECT reserve_id, version_id, base_reserve_id  
    FROM reserve WITH (NOLOCK)  
    WHERE claim_peril_id = @copy_claim_peril_id  
    AND version_id = @version_id) copy_reserve ON  
     copy_reserve.base_reserve_id  = reserve.base_reserve_id  
  
 WHERE claim_payment.claim_peril_id = @claim_peril_id  
 AND claim_payment.claim_id = @claim_id  
 AND ISNULL(claim_payment.is_referred,0)<>2  
  
 IF @@ERROR <> 0  
  GOTO Error_Routine  
  
-- *************************************************************  
-- * link claim payment item with the new version of the claim payment  
-- *************************************************************  
/* UPDATE claim_payment_item  
 SET claim_payment_id = copy.claim_payment_id  
 FROM claim_payment_item cpi  
  
  INNER JOIN claim_payment ON  
   cpi.claim_payment_id = claim_payment.claim_payment_id  
  
   INNER JOIN (  
    SELECT claim_payment_id, version_id, base_claim_payment_id  
    FROM claim_payment  
    WHERE claim_peril_id = @copy_claim_peril_id  
        AND claim_id = @copy_claim_id  
    AND version_id = @version) copy ON  
     copy.base_claim_payment_id = claim_payment_id.base_claim_payment_id  
  
  WHERE cpi.version_id = @version_id  
  
 IF @@ERROR <> 0  
  GOTO Error_Routine  
*/  
-- ********************************************************  
-- * link claim payment item with the new version of the recovery  
-- ********************************************************  
/* UPDATE claim_payment_item  
 SET claim_payment_item.recovery_id = copy.recovery_id  
 FROM claim_payment_item cpi  
  
  INNER JOIN recovery ON  
   claim_payment_item.recovery_id = recovery.recovery_id  
  
   INNER JOIN (  
    SELECT recovery_id, version_id, base_recovery_id  
    FROM recovery  
    WHERE claim_peril_id = @copy_claim_peril_id  
    AND version = @version) copy ON  
     copy.base_recovery_id  = cpi.base_recovery_id  
  
 WHERE cpi.version_id = @version_id  
  
 IF @@ERROR <> 0  
         GOTO Error_Routine  
*/  
-- ********************************************************  
-- * link claim payment item with the new version of the reserve  
-- ********************************************************  
/* UPDATE claim_payment_item  
 SET claim_payment_item.reserve_id = copy.reserve_id  
 FROM claim_payment_item cpi  
  
  INNER JOIN reserve ON  
   claim_payment_item.reserve_id = recovery.reserve_id  
  
   INNER JOIN (  
    SELECT reserve_id, version_id, base_reserve_id  
    FROM reserve  
    WHERE claim_peril_id = @copy_claim_peril_id  
    AND version = @version) copy ON  
     copy.base_reserve_id  = cpi.base_reserve_id  
  
 WHERE cpi.version_id = @version_id  
  
 IF @@ERROR <> 0  
         GOTO Error_Routine  
  
*/  
  
 --**********************************************  
 --* copy tax calculation entries to work table *  
 --**********************************************  
 --INSERT INTO tax_calculation  
 --(  
 -- claim_peril_id,  
 -- claim_payment_id,  
 -- claim_receipt_id,  
 -- claim_payment_item_id,  
 -- claim_receipt_item_id,  
 -- tax_band_id,  
 -- premium,  
 -- percentage,  
 -- value,  
 -- is_value,  
 -- currency_id,  
 -- class_of_business_id,  
 -- tax_group_id,  
 -- sequence,  
 -- transtype,  
 -- is_manually_changed,  
 -- state_id,  
 -- country_id,  
 -- allow_tax_credit  
 --)  
 --SELECT  
 -- @copy_claim_peril_id,  
 -- copy_claim_payment.claim_payment_id,  
 -- copy_claim_receipt.claim_receipt_id,  
 -- copy_claim_payment_item.claim_payment_item_id,  
 -- copy_claim_receipt_item.claim_receipt_item_id,  
 -- tc.tax_band_id,  
 -- tc.premium,  
 -- tc.percentage,  
 -- tc.value,  
 -- tc.is_value,  
 -- tc.currency_id,  
 -- tc.class_of_business_id,  
 -- tc.tax_group_id,  
 -- tc.sequence,  
 -- tc.transtype,  
 -- tc.is_manually_changed,  
 -- tc.state_id,  
 -- tc.country_id,  
 -- 0 allow_tax_credit  
  
 --FROM tax_calculation tc WITH (NOLOCK)  
  
 --     -- inner join to get the id of the new version of the claim payment  
 -- LEFT JOIN claim_payment WITH (NOLOCK) ON  
 --  tc.claim_payment_id = claim_payment.claim_payment_id  
  
 --  LEFT JOIN (SELECT claim_payment_id, version_id, base_claim_payment_id  
 --       FROM claim_payment WITH (NOLOCK)  
 --       WHERE claim_id = @copy_claim_id  
 --       AND claim_peril_id = @copy_claim_peril_id 
	--	AND ISNULL(is_referred,0)<>2 
 --       AND version_id = @version_id) copy_claim_payment ON  
 --   copy_claim_payment.base_claim_payment_id = claim_payment.base_claim_payment_id  
  
 --     -- inner join to get the id of the new version of the claim payment item  
 -- LEFT JOIN claim_payment_item WITH (NOLOCK) ON  
 --  tc.claim_payment_item_id = claim_payment_item.claim_payment_item_id  
  
 --  LEFT JOIN (SELECT claim_payment_item_id, version_id, base_claim_payment_item_id  
 --       FROM claim_payment_item WITH (NOLOCK)  
 --       WHERE version_id = @version_id) copy_claim_payment_item ON  
 --   copy_claim_payment_item.base_claim_payment_item_id = claim_payment_item.base_claim_payment_item_id  
  
 --     -- inner join to get the id of the new version of the claim receipt  
 -- LEFT JOIN claim_receipt WITH (NOLOCK) ON  
 --  tc.claim_receipt_id = claim_receipt.claim_receipt_id  
  
 --  LEFT JOIN (SELECT claim_receipt_id, version_id, base_claim_receipt_id  
 --       FROM claim_receipt WITH (NOLOCK)  
 --       WHERE claim_id = @copy_claim_id  
 --       AND claim_peril_id = @copy_claim_peril_id  
 --       AND version_id = @version_id) copy_claim_receipt ON  
 --   copy_claim_receipt.base_claim_receipt_id = claim_receipt.base_claim_receipt_id  
  
 --     -- inner join to get the id of the new version of the claim receipt item  
 -- LEFT JOIN claim_receipt_item WITH (NOLOCK) ON  
 --  tc.claim_receipt_item_id = claim_receipt_item.claim_receipt_item_id  
  
 --  LEFT JOIN (SELECT claim_receipt_item_id, version_id, base_claim_receipt_item_id  
 --       FROM claim_receipt_item WITH (NOLOCK)  
 --       WHERE version_id = @version_id) copy_claim_receipt_item ON  
 --   copy_claim_receipt_item.base_claim_receipt_item_id = claim_receipt_item.base_claim_receipt_item_id  
  
 --WHERE tc.claim_peril_id = @claim_peril_id  AND  ISNULL(claim_payment.is_referred,0)<>2
  
 SELECT  @status = 0  
  
 RETURN  
  
Error_Routine:  

  
 SELECT  @status = -1  
  
 RETURN  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
