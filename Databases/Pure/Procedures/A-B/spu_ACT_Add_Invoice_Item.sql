SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Invoice_Item'
GO


CREATE PROCEDURE spu_ACT_Add_Invoice_Item
    @invoice_id int,
    @invoice_item_no int,
    @description varchar(255),
    @nominal_code varchar(30),
    @value numeric(19,4),
    @currency_id smallint,
    @department_id smallint,
    @dept_amount numeric(19, 4),
    @vat_rate numeric(10, 4),
    @has_vat tinyint
AS

DECLARE @company_id INT
DECLARE @invoice_date DATETIME
DECLARE @base_currency_id INT
DECLARE @base_amount MONEY
DECLARE @base_value MONEY
DECLARE @return_status INT


SELECT
	@invoice_date = invoice_date,
	@company_id = company_id
FROM Invoice
WHERE invoice_id = @invoice_id

/*Get the converted amounts*/
EXECUTE spu_ACT_Do_Currency_Conversion
	@company_id = @company_id,
	@currency_id = @currency_id,
	@currency_amount_unrounded = @value,
	@mode = 'BASE',
	@base_currency_id = @base_currency_id OUTPUT,
	@base_amount = @base_value OUTPUT,
	@currency_base_date = @invoice_date OUTPUT,
	@return_status = @return_status OUTPUT
	
EXECUTE spu_ACT_Do_Currency_Conversion
	@company_id = @company_id,
	@currency_id = @currency_id,
	@currency_amount_unrounded = @dept_amount,
	@mode = 'BASE',
	@base_amount = @base_amount OUTPUT,
	@currency_base_date = @invoice_date OUTPUT,
	@return_status = @return_status OUTPUT

INSERT INTO Invoice_item 
(
    invoice_id,
    invoice_item_no,
    description ,
    nominal_code ,
    value ,
    currency_id,
    department_id,
    dept_amount,
    vat_rate,
    has_vat,
    base_currency_id,
    dept_base_amount,
    base_value
)
VALUES 
(
    @invoice_id,
    @invoice_item_no,
    @description ,
    @nominal_code ,
    @value ,
    @currency_id,
    @department_id,
    @dept_amount,
    @vat_rate,
    @has_vat,
    @base_currency_id,
    @base_amount,
    @base_value
)

GO


