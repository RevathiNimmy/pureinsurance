SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Payment_Item_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Payment_Item_Details    
 @claim_payment_id int    
    
AS    
    
BEGIN    
--**********************************************    
--  Decide whether we are Underwriter or Broker    
--**********************************************    
    
 SELECT    
 r.reserve_id,    
 rt.description,    
 cpi.this_payment,    
 cpi.tax_amount,    
 cpi.tax_amount_wht,    
 cpi.currency_id,    
 cpi.currency_base_xrate,    
 cpi.tax_group_id,    
 r.initial_reserve + r.revised_reserve as total_reserve,    
 r.paid_to_date,    
 (r.initial_reserve + r.revised_reserve) - r.paid_to_date as balance,    
 cpi.payment_loss_xrate,    
 c.description,    
 tg.description,    
 tg.is_withholding_tax,    
 tg.advanced_tax_script,    
 cpi.claim_payment_item_id,
 r.revised_reserve      
    
 FROM claim_payment_item cpi    
    
  INNER JOIN reserve r ON    
   cpi.reserve_id = r.reserve_id    
    
  INNER JOIN reserve_type rt ON    
   r.reserve_type_id =rt.reserve_type_id    
    
  INNER JOIN Currency c ON    
   c.Currency_Id = cpi.Currency_Id    
    
  LEFT OUTER JOIN Tax_Group tg ON    
   tg.tax_group_id = cpi.tax_group_id    
    
  LEFT OUTER JOIN Claim_Payment cp ON
   cpi.claim_payment_id =cp.claim_payment_id    
    
  WHERE cpi.claim_payment_id =@claim_payment_id   AND ISNULL(cp.is_referred,0) <>2
    
  
END  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
