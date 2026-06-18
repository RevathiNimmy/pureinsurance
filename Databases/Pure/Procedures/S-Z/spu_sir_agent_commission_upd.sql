SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_agent_commission_upd'
GO


CREATE PROCEDURE spu_sir_agent_commission_upd
    @insurance_file_cnt int,
    @is_lead_agent tinyint,
    @party_cnt int,
    @risk_type_id int,
    @commission_band_id int,
    @commission_percentage numeric(19,10),
    @commission_value numeric(19,4),
    @is_amended tinyint,
    @tax_group_id INT,
    @tax_amount MONEY,
    @calculated_commission_value MONEY = 0,
    @override_reason VARCHAR(255) = NULL
AS

DECLARE @account_id INT
DECLARE @company_id INT
DECLARE @currency_id INT

DECLARE @base_currency_id INT
DECLARE @base_amount MONEY
DECLARE @account_currency_id INT
DECLARE @account_amount MONEY
DECLARE @return_status INT

DECLARE @tax_base_amount MONEY
DECLARE @tax_account_amount MONEY

SELECT @account_id = account_id
FROM account
WHERE account_key = @party_cnt

SELECT 
    @company_id = source_id,
    @currency_id = currency_id
FROM insurance_file
WHERE insurance_file_cnt = @insurance_file_cnt


/*Get the converted amounts*/
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

/*Get the converted amounts*/
EXECUTE spu_ACT_Do_Currency_Conversion
    @account_id = @account_id,
    @company_id = @company_id,
    @currency_id = @currency_id,
    @currency_amount_unrounded = @tax_amount,
    @mode = 'ALL',
    @base_amount = @tax_base_amount OUTPUT,
    @account_amount = @tax_account_amount OUTPUT,
    @return_status = @return_status OUTPUT

UPDATE agent_commission
SET Commission_percentage = @commission_percentage,
    Commission_value = @commission_value,
    is_amended = @is_amended,
    account_currency_id = @account_currency_id,
    account_commission_value = @account_amount,
    base_currency_id = @base_currency_id,
    base_commission_value = @base_amount,
    tax_group_id=@tax_group_id,
    tax_amount=@tax_amount,
    tax_account_amount=@tax_account_amount,
    tax_base_amount=@tax_base_amount,
    calculated_commission_value=@calculated_commission_value,
    override_reason=@override_reason
WHERE insurance_file_cnt = @insurance_file_cnt
AND Party_cnt = @party_cnt
AND risk_type_id = @risk_type_id
AND commission_band_id = @commission_band_id

GO


