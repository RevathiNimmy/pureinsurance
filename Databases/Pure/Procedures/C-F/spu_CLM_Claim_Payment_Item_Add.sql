SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Payment_Item_Add'
GO

CREATE PROCEDURE spu_CLM_Claim_Payment_Item_Add  
	@claim_payment_item_id int OUTPUT,  
	@claim_payment_id int,  
	@reserve_id int,  
	@recovery_id int,  
	@recovery_type_id int,  
	@currency_id smallint,  
	@tax_group_id int,  
	@this_payment money,  
	@tax_amount money,  
	@tax_amount_WHT money,  
	@exchange_rate_override_reason_id int,  
	@currency_base_xrate float,  
	@currency_base_date datetime,  
	@account_base_xrate float,  
	@account_base_date datetime,  
	@system_base_xrate float,  
	@system_base_date datetime,  
	@payment_loss_xrate float,
	@payment_adjustment money 
  
AS  
  
BEGIN  

	DECLARE @version_id int

	EXEC spu_CLM_get_claim_version 
		@claim_payment_id = @claim_payment_id, 
		@version_id = @version_id OUTPUT
  
	INSERT INTO claim_payment_item(  
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
		version_id,
		payment_adjustment)  	
	VALUES (  
		@claim_payment_id,  
		@reserve_id,  
		@recovery_id,  
		@recovery_type_id,  
		@currency_id,  
		@tax_group_id,  
		@this_payment,  
		@tax_amount,  
		@tax_amount_WHT,  
		@exchange_rate_override_reason_id,  
		@currency_base_xrate,  
		@currency_base_date,  
		@account_base_xrate,  
		@account_base_date,  
		@system_base_xrate,  
		@system_base_date,  
		@payment_loss_xrate, 
		@version_id,
		@payment_adjustment)  
	
	SET @claim_payment_item_id = @@IDENTITY  
	
	UPDATE claim_payment_item  
	SET base_claim_payment_item_id = @claim_payment_item_id  
	WHERE claim_payment_item_id = @claim_payment_item_id  

END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
