EXECUTE DDLDropProcedure 'spu_SAM_Get_Claim_Payment_Item_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Payment_Item_Details  
  
@claim_id INT,  
@iFetchAllVersionAmounts TINYINT = 0

AS  
  
SELECT  
  
 cpi.claim_payment_item_id,  
 cpi.claim_payment_id,  
 cpi.reserve_id,  
 cpi.recovery_id,  
 cpi.tax_group_id,  
 tg.code as tax_group_code,  
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0  
 ELSE cpi.this_payment END AS this_payment,  
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0  
 ELSE  ISNULL(cpi.tax_amount,0) + ISNULL(cpi.tax_amount_WHT,0) END AS tax_amount,  
 cpi.base_claim_payment_item_id,  
 cpi.version_id,  
 cpi.payment_adjustment,
 R.base_reserve_id,
 R.This_Revision,  
CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0    
 ELSE ISNULL(cpi.this_payment,0)*ISNULL(cpi.payment_loss_xrate,1) END as LossAmount,  
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0
 ELSE ISNULL(cpi.this_payment,0)*ISNULL(cpi.currency_base_xrate,1) END as BaseAmount ,
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0
 ELSE  ( ISNULL(cpi.tax_amount,0) + ISNULL(cpi.tax_amount_WHT,0))*ISNULL(cpi.payment_loss_xrate,1) END as LossTaxAmount 
  
FROM claim_payment_item cpi  
 INNER JOIN claim_payment cp ON cp.claim_payment_id=cpi.claim_payment_id  
 LEFT JOIN tax_group tg ON  
  tg.tax_group_id = cpi.tax_group_id  
 LEFT JOIN Reserve R ON  
 R.Reserve_id =cpi.reserve_id  
  
WHERE cp.claim_id= @claim_id

      AND cpi.reserve_id>0  
  
ORDER BY cpi.claim_payment_id, cpi.claim_payment_item_id 