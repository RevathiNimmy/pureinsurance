
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Ins_File_Details_And_Renewal_Premium'
GO
CREATE PROCEDURE spu_Get_Ins_File_Details_And_Renewal_Premium
	@insurance_file_cnt INT,
	@renewal_premium MONEY = NULL OUTPUT, 
	@renewal_status_cnt AS INTEGER = NULL OUTPUT, 
	@renewal_status_type_id AS INTEGER = NULL OUTPUT, 
	@insurance_file_status_id AS INTEGER = NULL OUTPUT, 
	@insurance_file_type_id AS INTEGER = NULL OUTPUT, 
	@old_insurance_file_cnt AS INTEGER = NULL OUTPUT,
	@insurance_ref AS VARCHAR(255) = NULL OUTPUT
AS
BEGIN

-- Get the Renewal Status ID
SELECT 
	@renewal_status_type_id = renewal_status_type_id, 
	@renewal_status_cnt = renewal_status_cnt, 
	@old_insurance_file_cnt = insurance_file_cnt
FROM 
	renewal_status 
WHERE 
	renewal_insurance_file_cnt = @insurance_file_cnt 

-- Get the Insurance file status ID and the Insurance file type ID
SELECT 
	@insurance_file_status_id = insurance_file_status_id,    
	@insurance_file_type_id = insurance_file_type_id, 
	@insurance_ref = insurance_ref
FROM 
	Insurance_File
WHERE 
	insurance_file_cnt = @insurance_file_cnt 

IF (@renewal_status_type_id = 5)
	BEGIN 

		DECLARE @source_id INT 
		DECLARE @agent_account_id INT 
		DECLARE @currency_id SMALLINT 
		DECLARE @premium MONEY 
		DECLARE @currency_base_xrate FLOAT 
		DECLARE @currency_base_date DATETIME 
		DECLARE @account_base_xrate FLOAT 
		DECLARE @account_base_date DATETIME 
		DECLARE @system_base_xrate FLOAT 
		DECLARE @system_base_date DATETIME 
		DECLARE @exchange_rate_override_reason_id INT 
		
		EXEC 	spu_ACT_Get_Insurance_File_Information @insurance_file_cnt, 
			@source_id output,
			@agent_account_id output,
			@currency_id output,
			@premium output,
			@currency_base_xrate output,
			@currency_base_date output,
			@account_base_xrate output,
		 	@account_base_date output,
		 	@system_base_xrate output,
		 	@system_base_date output,  
			@exchange_rate_override_reason_id output

		SELECT @renewal_premium = @premium
	
	END 
END
