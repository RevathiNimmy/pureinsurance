SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Receipt_Item_Add'
GO

CREATE PROCEDURE spu_CLM_Claim_Receipt_Item_Add  
  
@claim_receipt_item_id int OUTPUT,  
@claim_receipt_id int,  
@reserve_id int,  
@recovery_id int,  
@recovery_type_id int,  
@currency_id smallint,  
@tax_group_id int,  
@this_receipt money,  
@tax_amount money,  
@exchange_rate_override_reason_id int,  
@currency_base_xrate float,  
@currency_base_date datetime,  
@account_base_xrate float,  
@account_base_date datetime,  
@system_base_xrate float,  
@system_base_date datetime,  
@receipt_loss_xrate float  
  
AS  
  
BEGIN  
  
 DECLARE @version_id int

 EXEC spu_CLM_get_claim_version 
		@claim_receipt_id = @claim_receipt_id, 
		@version_id = @version_id OUTPUT

 INSERT INTO claim_receipt_item(  
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
  version_id
  )  
  
 VALUES (  
  @claim_receipt_id,  
  @recovery_id,  
  @reserve_id,  
  @recovery_type_id,  
  @currency_id,  
  @this_receipt,  
  @tax_group_id,  
  @tax_amount,  
  @exchange_rate_override_reason_id,  
  @currency_base_xrate,  
  @currency_base_date,  
  @account_base_xrate,  
  @account_base_date,  
  @system_base_xrate,  
  @system_base_date,  
  @receipt_loss_xrate, 
  @version_id  
  )  
  
 SET @claim_receipt_item_id = @@IDENTITY  
  
 UPDATE claim_receipt_item 
 SET base_claim_receipt_item_id = @claim_receipt_item_id 
 WHERE claim_Receipt_item_id =  @claim_receipt_item_id
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
