SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sam_agent_commission_upd'
GO

CREATE PROCEDURE spu_sam_agent_commission_upd  
   @insurance_file_cnt INT,  
    @is_lead_agent TINYINT,  
    @party_cnt INT,  
    @risk_type_id INT,  
    @commission_band_id INT,  
    @premium NUMERIC(19,4),  
    @Commission_percentage NUMERIC(19,10),  
    @commission_value NUMERIC(19,4),  
    @is_amended TINYINT,  
    @tax_group_id INT,  
    @calculated_commission_value MONEY = 0,  
    @override_reason VARCHAR(255)= NULL,  
    @is_value TINYINT=0,  
    @maximum_rate NUMERIC(19,4)=0,  
    @is_tax_amended TINYINT=0,  
    @amended_tax_value MONEY=0,  
    @ViaCalculateAgentCommission INT =0,
	@peril_type_id INT = 0 	
AS  
  
DECLARE @account_id INT  
DECLARE @company_id INT  
DECLARE @currency_id INT  
  
DECLARE @base_currency_id INT  
DECLARE @base_amount MONEY  
DECLARE @account_currency_id INT  
DECLARE @account_amount MONEY  
DECLARE @return_status INT  
  
DECLARE @agent_commission_cnt INT  
  
DECLARE @tax_currency_amount MONEY  
DECLARE @tax_base_amount MONEY  
DECLARE @tax_account_amount MONEY  
DECLARE @peril_type_id_cursor INT
  
SELECT  @is_tax_amended=ISNULL(@is_tax_amended,0),  
  @amended_tax_value=ISNULL(@amended_tax_value,0)  
  
SELECT @account_id = account_id  
FROM account  
WHERE account_key = @party_cnt  
  
SELECT  
    @company_id = source_id,  
    @currency_id = currency_id  
FROM insurance_file  
WHERE insurance_file_cnt = @insurance_file_cnt  

DECLARE Agent_Comm_Cursor CURSOR forward_only FOR 
SELECT agent_commission_cnt,premium , peril_type_id 
FROM Agent_Commission  
WHERE insurance_file_cnt = @insurance_file_cnt  
AND Party_cnt = @party_cnt  
AND risk_type_id = @risk_type_id  
AND commission_band_id = @commission_band_id  

OPEN Agent_Comm_Cursor
FETCH NEXT FROM Agent_Comm_Cursor into  @agent_commission_cnt,@premium, @peril_type_id_cursor

While @@fetch_Status = 0  
  
Begin  

IF @is_value=0 AND @Commission_percentage<>0 
SELECT @commission_value=@premium*@Commission_percentage/100, @calculated_commission_value=@premium*@Commission_percentage/100

/*Convert the Commission amount*/  
EXECUTE spu_ACT_Do_Currency_Conversion  
    @account_id = @account_id,  
    @company_id = @company_id,  
    @currency_id = @currency_id,  
    @currency_amount_unrounded = @commission_value,  
    @mode = 'ALL',  
    @base_currency_id = @base_currency_id OUTPUT,  
    @base_amount = @base_amount OUTPUT,  
    @account_currency_id = @account_currency_id OUTPUT,  
    @account_amount = @account_amount OUTPUT,  
    @return_status = @return_status OUTPUT  
  
 DELETE Tax_Calculation  
 WHERE transtype='TTAC'  
 AND  insurance_file_cnt = @insurance_file_cnt  
 AND agent_commission_cnt =@agent_commission_cnt  
  
/* Calculate the tax breakdown */  
EXEC spu_SIR_Calculate_Tax_Amounts  
    @company_id=@company_id,  
    @tax_group_id=@tax_group_id,  
    @transtype='TTAC',  
    @currency_id=@currency_id,  
    @amount=@commission_value,  
    @tax_currency_amount=@tax_currency_amount OUTPUT,  
    @tax_base_amount=@tax_base_amount OUTPUT,  
    @associated_key_id=@agent_commission_cnt,  
    @insurance_file_cnt=@insurance_file_cnt,  
    @risk_cnt=NULL,  
    @is_tax_amended =@is_tax_amended,  
    @amended_tax_value =@amended_tax_value  
  
/* Convert to Account currency */  
EXECUTE spu_ACT_Do_Currency_Conversion  
    @account_id = @account_id,  
    @company_id = @company_id,  
    @currency_id = @currency_id,  
    @currency_amount_unrounded = @tax_currency_amount,  
    @mode = 'ALL',  
    @account_amount = @tax_account_amount OUTPUT,  
    @return_status = @return_status OUTPUT  
  
  
If  @peril_type_id = 0 
	BEGIN
			UPDATE agent_commission    
			SET Commission_percentage = @commission_percentage,    
			Commission_value = @commission_value,    
			is_amended = @is_amended,    
			account_currency_id = @account_currency_id,    
			account_commission_value = @account_amount,    
			base_currency_id = @base_currency_id,    
			base_commission_value = @base_amount,    
			tax_group_id=@tax_group_id,    
		    tax_amount=@tax_currency_amount,    
			tax_account_amount=@tax_account_amount,    
			tax_base_amount=@tax_base_amount,    
			calculated_commission_value=@calculated_commission_value,    
			override_reason=@override_reason,    
			maximum_rate=@maximum_rate,    
			is_value=@is_value,    
			is_tax_amended=@is_tax_amended    
			WHERE insurance_file_cnt = @insurance_file_cnt    
			AND Party_cnt = @party_cnt    
			AND risk_type_id = @risk_type_id    
			AND commission_band_id = @commission_band_id    
			AND agent_commission_cnt=@agent_commission_cnt    
    END 
ELSE
   IF @peril_type_id = @peril_type_id_cursor 
	  BEGIN
			UPDATE agent_commission    
			SET Commission_percentage = @commission_percentage,    
			Commission_value = @commission_value,    
			is_amended = @is_amended,    
			account_currency_id = @account_currency_id,    
			account_commission_value = @account_amount,    
			base_currency_id = @base_currency_id,    
			base_commission_value = @base_amount,    
			tax_group_id=@tax_group_id,    
			tax_amount=@tax_currency_amount,    
			tax_account_amount=@tax_account_amount,    
			tax_base_amount=@tax_base_amount,    
			calculated_commission_value=@calculated_commission_value,    
			override_reason=@override_reason,    
			maximum_rate=@maximum_rate,    
			is_value=@is_value,    
			is_tax_amended=@is_tax_amended    
			WHERE insurance_file_cnt = @insurance_file_cnt    
			AND Party_cnt = @party_cnt    
			AND risk_type_id = @risk_type_id    
			AND commission_band_id = @commission_band_id    
			AND agent_commission_cnt=@agent_commission_cnt
	  END
FETCH NEXT FROM Agent_Comm_Cursor into  @agent_commission_cnt,@premium, @peril_type_id_cursor

END

--Close and Deallocate the Cursor  
    Close Agent_Comm_Cursor  
  
    Deallocate Agent_Comm_Cursor  
  
GO
