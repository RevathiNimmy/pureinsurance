--Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_Tax_Calculation_Select_For_Receipt'
GO

CREATE PROCEDURE spu_CLM_Tax_Calculation_Select_For_Receipt       
@claim_Receipt_Item_id int          
AS          
          
BEGIN          
          
   SELECT          
   tc.tax_group_id,          
    tc.tax_band_id,          
    currency.code,          
    tc.percentage,          
    tc.value,          
    tc.is_value,          
    tc.class_of_business_id,          
    tc.sequence,          
   is_manually_changed,          
    tg.description,          
    tb.description                
   FROM tax_calculation  tc          
          
   INNER JOIN currency ON          
    tc.currency_id = currency.currency_id                
   INNER JOIN tax_group tg ON          
    tg.tax_group_id = tc.tax_group_id                
   INNER JOIN tax_band tb ON          
    tb.tax_band_id = tc.tax_band_id          
          
   WHERE tc.claim_Receipt_item_id = @claim_Receipt_Item_id  
          
END    
--End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance