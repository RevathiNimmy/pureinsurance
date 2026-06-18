SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_claim_payment_del'
GO

CREATE PROCEDURE spu_SAM_claim_payment_del  
@claim_peril_id INT,  
@process_type INT=1  
AS  
  
DELETE FROM tax_calculation WHERE claim_peril_id=@claim_peril_id  
   
DELETE FROM claim_Payment_item WHERE claim_payment_id  
  IN  
  (SELECT claim_payment_id FROM claim_payment WHERE claim_peril_id=@claim_peril_id)  
  and claim_payment_item_id=base_claim_payment_item_id
  
DELETE FROM claim_payment WHERE claim_peril_id=@claim_peril_id  
  and claim_payment_id=base_claim_payment_id
  
if @process_type=4  
UPDATE reserve SET paid_to_date=paid_to_date-this_payment, this_payment=0,  
     revised_reserve = revised_reserve - this_revision 
WHERE claim_peril_id=@claim_peril_id  
else
UPDATE reserve SET paid_to_date=paid_to_date-this_payment, this_payment=0,  
     revised_reserve = revised_reserve - this_revision, this_revision=0  
WHERE claim_peril_id=@claim_peril_id  

GO

