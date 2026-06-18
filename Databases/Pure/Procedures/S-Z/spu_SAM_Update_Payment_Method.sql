SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Update_Payment_Method'
GO
CREATE PROCEDURE spu_SAM_Update_Payment_Method 
	@InsuranceFileKey INT,
	@PaymentMethod VARCHAR(60)
AS
BEGIN
	DECLARE @nInsurance_File_Key INT= @InsuranceFileKey
	DECLARE @sPayment_Method VARCHAR(60)= @PaymentMethod

	BEGIN TRANSACTION
		
	IF (CHARINDEX('PREMIUM',UPPER(@sPayment_Method)) > 0) OR (UPPER(@sPayment_Method)='DIRECT DEBIT') OR (UPPER(@sPayment_Method)='CREDIT CARD')
	     SET @sPayment_Method= 'INSTALMENTS'

	UPDATE insurance_file
	SET payment_method=@sPayment_Method
	WHERE insurance_file_cnt =@nInsurance_File_Key

	IF (UPPER(@sPayment_Method)<>'INSTALMENTS')
	DECLARE @pfprem_finance_cnt INT
	DECLARE @pfprem_finance_version INT
	DECLARE @RETURNSTATUS INT
		SELECT @pfprem_finance_cnt= pfprem_finance_cnt,@pfprem_finance_version=pfprem_finance_version  from PFPremiumFinance Where  insurance_file_cnt = @nInsurance_File_Key
		DELETE PFPremiumFinance WHERE pfprem_finance_cnt = @pfprem_finance_cnt AND pfprem_finance_version = @pfprem_finance_version
	
	IF @@ERROR <> 0
        ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION

END