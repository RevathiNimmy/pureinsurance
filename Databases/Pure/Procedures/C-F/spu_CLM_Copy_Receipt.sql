SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Copy_Receipt'
GO

CREATE PROCEDURE spu_CLM_Copy_Receipt  
    @claim_id int,  
    @copy_claim_id int,  
    @claim_peril_id int,  
    @copy_claim_peril_id int,  
    @version_id int,  
    @status int OUTPUT  
AS  
  
BEGIN  

 INSERT INTO claim_receipt  
 (  
  claim_id,  
  claim_peril_id,  
  date_of_receipt,  
  party_cnt,  
  Amount,  
  tax_amount,  
  comments,  
  created_by,  
  insured_domiciled,  
  insured_percentage,  
  insured_tax_number,  
  receivable_tax_percentage,  
  is_tax_exempt,  
  is_settlement,  
  PayeeMediaType,  
  PayeeName,  
  PayeeBankName,  
  PayeeSortCode,  
  PayeeAccountNo,  
  PayeeCountry,  
  PayeeComments,  
  PayeeMediaRef,  
  document_id,  
  currency_id,  
  base_claim_receipt_id,  
  version_id  
 )  
 SELECT  
  @copy_claim_id,  
  @copy_claim_peril_id,  
  date_of_receipt,  
  party_cnt,  
--Changed From 0 For Amount and Tax Amount Tech spec QBENZCR004 Claim Recovery Reinsurance      
  Amount,      
  tax_Amount,      
  comments,  
  created_by,  
  insured_domiciled,  
  insured_percentage,  
  insured_tax_number,  
  receivable_tax_percentage,  
  is_tax_exempt,  
  is_settlement,  
  PayeeMediaType,  
  PayeeName,  
  PayeeBankName,  
  PayeeSortCode,  
  PayeeAccountNo,  
  PayeeCountry,  
  PayeeComments,  
  PayeeMediaRef,  
  document_id,  
  currency_id,  
  base_claim_receipt_id,  
  @version_id  
 FROM claim_receipt WITH (NOLOCK)  
 WHERE claim_id = @claim_id  
 AND claim_peril_id = @claim_peril_id  
  
 INSERT INTO claim_receipt_item  
 (  
  claim_receipt_id,  
  recovery_id,  
  reserve_id,  
  recovery_type_id,  
  currency_id,  
  this_receipt,  
  tax_group_id,  
  tax_amount,  
  exchange_rate_override_reason_id,  
  currency_base_xrate,  
  currency_base_date,  
  account_base_xrate,  
  account_base_date,  
  system_base_xrate,  
  system_base_date,  
  receipt_loss_xrate,  
  base_claim_receipt_item_id,  
  version_id  
 )  
 SELECT  
  copy_claim_receipt.claim_receipt_id,  
  copy_recovery.recovery_id,  
  copy_reserve.reserve_id,  
  cri.recovery_type_id,  
  cri.currency_id,  
--Changed From 0 For thisReceipt and Tax Amount Tech spec QBENZCR004 Claim Recovery Reinsurance      
  cri.this_receipt,      
  tax_group_id,  
  cri.tax_amount,      
  exchange_rate_override_reason_id,  
  currency_base_xrate,  
  currency_base_date,  
  account_base_xrate,  
  account_base_date,  
  system_base_xrate,  
  system_base_date,  
  receipt_loss_xrate,  
  base_claim_receipt_item_id,  
  @version_id  
  
 FROM claim_receipt_item cri WITH (NOLOCK)  
  
  -- inner join to get the id of the new version of the claim receipt  
  INNER JOIN claim_receipt WITH (NOLOCK) ON  
   cri.claim_receipt_id = claim_receipt.claim_receipt_id  
  
   INNER JOIN (  
    SELECT claim_receipt_id, version_id, base_claim_receipt_id  
    FROM claim_receipt WITH (NOLOCK)  
    WHERE claim_peril_id = @copy_claim_peril_id  
        AND claim_id = @copy_claim_id  
    AND version_id = @version_id) copy_claim_receipt ON  
     copy_claim_receipt.base_claim_receipt_id = claim_receipt.base_claim_receipt_id  
  
      -- inner join to get the id of the new version of the recovery  
  LEFT JOIN recovery WITH (NOLOCK) ON  
   cri.recovery_id = recovery.recovery_id  
  
   LEFT JOIN (  
    SELECT recovery_id, version_id, base_recovery_id  
    FROM recovery WITH (NOLOCK)  
    WHERE claim_peril_id = @copy_claim_peril_id  
    AND version_id = @version_id) copy_recovery ON  
     copy_recovery.base_recovery_id  = recovery.base_recovery_id  
  
      -- inner join to get the id of the new version of the reserve  
  LEFT JOIN reserve WITH (NOLOCK) ON  
   cri.reserve_id = reserve.reserve_id  
  
   LEFT JOIN (  
    SELECT reserve_id, version_id, base_reserve_id  
    FROM reserve WITH (NOLOCK)  
    WHERE claim_peril_id = @copy_claim_peril_id  
    AND version_id = @version_id) copy_reserve ON  
     copy_reserve.base_reserve_id  = reserve.base_reserve_id  
  
 WHERE claim_receipt.claim_id = @claim_id  
 AND claim_receipt.claim_peril_id = @claim_peril_id  
  
 IF @@ERROR <> 0  
         GOTO Error_Routine  
  
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
