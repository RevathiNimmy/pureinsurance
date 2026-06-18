SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_agent_commission_add'
GO

CREATE PROCEDURE spu_sir_agent_commission_add    
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
--Start - Renuka - (WPR64 Paralleling)
    @is_value TINYINT=0,  
    @maximum_rate NUMERIC(19,4)=0,   
--End - Renuka - (WPR64 Paralleling)
	--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)
	@is_tax_amended TINYINT=0,
	@amended_tax_value MONEY=0,
	--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)  
    @ViaCalculateAgentCommission INT =0,  
	@commission_level_id INT =NULL,
	@class_of_business_id INT=NULL,
	@peril_type_id INT=NULL
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
    
-- Read the commission display level option (5264: Display Commission at Commission Band Level)
DECLARE @display_band_level BIT = 0
SELECT @display_band_level = ISNULL(
    (SELECT CAST(so.value AS BIT) 
     FROM system_options so
     INNER JOIN insurance_file ifile ON ifile.insurance_file_cnt = @insurance_file_cnt
     INNER JOIN source s ON s.source_id = ifile.source_id
     WHERE so.branch_id = s.source_id 
     AND so.option_number = 5264
     AND so.value = '1'), 0)    
    
--Get Suppress Decimal flag to round whole number
	DECLARE @SuppressDecimalOption AS INT=112
	DECLARE @bIsSuppressDecimal As TINYINT=(Select ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption)

    IF  @bIsSuppressDecimal=1
	BEGIN
	    SET @commission_value=  ROUND(@commission_value,0)
		SET @calculated_commission_value=ROUND(@calculated_commission_value,0)
		SET @base_amount=ROUND(@base_amount,0)
	END    
   
--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)
SELECT  @is_tax_amended=ISNULL(@is_tax_amended,0),
		@amended_tax_value=ISNULL(@amended_tax_value,0)
--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)

SELECT @account_id = account_id    
FROM account    
WHERE account_key = @party_cnt    
    
SELECT    
    @company_id = source_id,    
    @currency_id = currency_id    
FROM insurance_file    
WHERE insurance_file_cnt = @insurance_file_cnt    
    
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
     
/* Insert the Commission line - we need to come back and fill in the tax afterwards */    
--IF @ViaCalculateAgentCommission= 1  
--BEGIN  
--SELECT @commission_value=@base_amount  
--SELECT @calculated_commission_value=@base_amount  
--END  
INSERT INTO Agent_commission    
(    
    insurance_file_cnt,    
    is_lead_agent,    
    party_cnt,    
    risk_type_id,    
    commission_band_id,    
    premium,    
    commission_percentage,    
    commission_value,    
    is_amended,    
    account_currency_id,    
    account_commission_value,    
    base_currency_id,    
    base_commission_value,    
    tax_group_id,    
    tax_amount,    
    tax_account_amount,    
    tax_base_amount,    
    calculated_commission_value,    
    override_reason,    
--Start - Renuka - (WPR64 Paralleling)
	maximum_rate,
	is_value, 
--End - Renuka - (WPR64 Paralleling)
	--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)
	is_tax_amended ,   
	--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)    
	commission_level_id,
	class_of_business_id,
	peril_type_id
)    
VALUES    
(    
    @insurance_file_cnt,    
    @is_lead_agent,    
    @party_cnt,    
    @risk_type_id,    
    @commission_band_id,    
    @premium,    
    @commission_percentage,    
    @commission_value,    
    @is_amended,    
    @account_currency_id,    
    @account_amount,    
    @base_currency_id,    
    @base_amount,    
    @tax_group_id,    
    0,    
    0,    
    0,    
    @calculated_commission_value,    
    @override_reason,    
--Start - Renuka - (WPR64 Paralleling)
	@maximum_rate,
	@is_value,  
--End - Renuka - (WPR64 Paralleling)
	--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)
	@is_tax_amended ,   
	--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)        
	@commission_level_id,
	CASE WHEN @display_band_level = 1 THEN NULL ELSE @class_of_business_id END,
	CASE WHEN @display_band_level = 1 THEN NULL ELSE @peril_type_id END
)    
    
SELECT @agent_commission_cnt=@@IDENTITY    
    
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
	--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)
	@is_tax_amended =@is_tax_amended,
 @amended_tax_value =@amended_tax_value,@premium=@premium    
 --End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.2.2)    
    
/* Convert to Account currency */    
EXECUTE spu_ACT_Do_Currency_Conversion    
    @account_id = @account_id,    
    @company_id = @company_id,    
    @currency_id = @currency_id,    
    @currency_amount_unrounded = @tax_currency_amount,    
    @mode = 'ALL',    
    @account_amount = @tax_account_amount OUTPUT,    
    @return_status = @return_status OUTPUT    
    
/* Revise Tax amounts */    
UPDATE  Agent_Commission    
SET     tax_amount=@tax_currency_amount,    
        tax_account_amount=@tax_account_amount,    
        tax_base_amount=@tax_base_amount    
WHERE   agent_commission_cnt=@agent_commission_cnt    
GO


