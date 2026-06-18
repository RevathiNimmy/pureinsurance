SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Claim_Receipt_Item_Recovery_Update'
GO

CREATE PROCEDURE spu_SAM_CLM_Claim_Receipt_Item_Recovery_Update
 @recovery_id int,  
 @this_revision money,  
 @this_receipt money,  
 @tax_amount money  
  
AS  
  
BEGIN  

UPDATE recovery  
SET revised_reserve = ISNULL(revised_reserve, 0) + @this_revision,  
received_to_date = ISNULL(received_to_date,0) + @this_receipt,  
tax_amount = ISNULL(tax_amount, 0) + @tax_amount,  
revision_count = ISNULL(revision_count,0) + 1,
this_receipt_net = ISNULL(@this_receipt,0)    
WHERE recovery_id = @recovery_id   


END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
