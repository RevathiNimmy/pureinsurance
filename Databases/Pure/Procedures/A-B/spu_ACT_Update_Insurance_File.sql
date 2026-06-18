EXEC DDLDropProcedure 'spu_ACT_Update_Insurance_File'
GO

CREATE PROCEDURE spu_ACT_Update_Insurance_File
	@insurance_file_cnt INT,
	@currency_base_xrate FLOAT,
	@currency_base_date DATETIME,
	@account_base_xrate FLOAT,
	@account_base_date DATETIME,
	@system_base_xrate FLOAT,
	@system_base_date DATETIME,
	@exchange_rate_override_reason_id INT,
	@base_currency_id SMALLINT,
	@agent_account_currency_id SMALLINT,
	@UnderwritingCode_Year_Id INT=NULL
AS
	IF @UnderwritingCode_Year_Id IS NULL
 BEGIN
	 UPDATE insurance_file  
	 SET currency_base_xrate = @currency_base_xrate,  
	  currency_base_date = @currency_base_date,  
	  agent_account_base_xrate = @account_base_xrate,  
	  account_base_date = @account_base_date,  
	  system_base_xrate = @system_base_xrate,  
	  system_base_date = @system_base_date,  
	  exchange_rate_override_reason_id = @exchange_rate_override_reason_id,  
	  base_currency_id = @base_currency_id,  
	  agent_account_currency_id = @agent_account_currency_id
	  WHERE insurance_file_cnt = @insurance_file_cnt  
 END
ELSE
 BEGIN
	UPDATE insurance_file  
	 SET currency_base_xrate = @currency_base_xrate,  
	  currency_base_date = @currency_base_date,  
	  agent_account_base_xrate = @account_base_xrate,  
	  account_base_date = @account_base_date,  
	  system_base_xrate = @system_base_xrate,  
	  system_base_date = @system_base_date,  
	  exchange_rate_override_reason_id = @exchange_rate_override_reason_id,  
	  base_currency_id = @base_currency_id,  
	  agent_account_currency_id = @agent_account_currency_id,  
	  underwriting_year_id=@UnderwritingCode_Year_Id  
	 WHERE insurance_file_cnt = @insurance_file_cnt   
 END
GO