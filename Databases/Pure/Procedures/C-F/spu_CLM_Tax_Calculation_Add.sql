SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Tax_Calculation_Add'
GO

CREATE PROCEDURE spu_CLM_Tax_Calculation_Add    
    
@tax_calculation_cnt int OUTPUT,    
@claim_peril_id int,    
@claim_payment_id int,    
@claim_receipt_id int,    
@claim_payment_item_id int,    
@claim_receipt_item_id int,    
@tax_band_id int,    
@premium money,    
@percentage float,    
@value money,    
@is_value tinyint,    
@currency_id smallint,    
@class_of_business_id int,    
@tax_group_id int,    
@sequence tinyint,    
@transtype varchar(10),    
@is_manually_changed int    
    
AS    
    
BEGIN    
    
 DECLARE @version_id int   
  
 EXEC spu_CLM_get_claim_version   
  @claim_peril_id = @claim_peril_id,   
  @version_id = @version_id OUTPUT  
  
 INSERT INTO tax_calculation (    
  claim_peril_id,    
  claim_payment_id,    
  claim_receipt_id,    
  claim_payment_item_id,    
  claim_receipt_item_id,    
  tax_band_id,    
  premium,    
  percentage,    
  value,    
  is_value,    
  currency_id,    
  class_of_business_id,    
  tax_group_id,    
  sequence,    
  transtype,    
  is_manually_changed,   
  version_id, 
  allow_tax_credit  
  )    
 VALUES (    
  @claim_peril_id,    
  @claim_payment_id,    
  @claim_receipt_id,    
  @claim_payment_item_id,    
  @claim_receipt_item_id,    
  @tax_band_id,    
  @premium,    
  @percentage,    
  @value,    
  @is_value,    
  @currency_id,    
  @class_of_business_id,    
  @tax_group_id,    
  @sequence,    
  @transtype,    
  @is_manually_changed,   
  @version_id, 
  0)    
    
 SELECT @tax_calculation_cnt = @@IDENTITY    
  
 UPDATE tax_calculation   
 SET base_tax_calculation_cnt = @tax_calculation_cnt  
 WHERE tax_calculation_cnt = @tax_calculation_cnt  
    
END    
  




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
