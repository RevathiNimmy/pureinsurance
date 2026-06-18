SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_MTA_Payment_Terms'
GO

CREATE Procedure spu_SIR_Get_MTA_Payment_Terms
  @Date DATETIME,
  @InsuranceFileCnt INT,
  @InsuranceFolderCnt INT,
  @InstalmentsEnabled BIT = 0 OUTPUT
AS

DECLARE @PaymentMethod VARCHAR(60)
DECLARE @PolicyVersion INT
DECLARE @MTAPaymentMethod VARCHAR(60)

SELECT @PaymentMethod=payment_method FROM insurance_file
WHERE insurance_folder_cnt=@InsuranceFolderCnt
AND   payment_method='Instalments'
AND   cover_start_date <= @Date AND expiry_date >= @Date

IF @PaymentMethod='Instalments'
	BEGIN
		SELECT @PolicyVersion=policy_version FROM insurance_file
		WHERE insurance_file_cnt=@InsuranceFileCnt
		AND   cover_start_date <= @Date AND expiry_date >= @Date
		
		SELECT @MTAPaymentMethod=Payment_method FROM insurance_file
		WHERE insurance_folder_cnt =@InsuranceFolderCnt
		AND   policy_version=@PolicyVersion - 1
		AND   cover_start_date <= @Date AND expiry_date >= @Date
		
		IF @MTAPaymentMethod='Invoice'
			BEGIN
				SELECT @InstalmentsEnabled=1
			END
			
	END

SELECT InstalmentsEnabled=@InstalmentsEnabled

GO