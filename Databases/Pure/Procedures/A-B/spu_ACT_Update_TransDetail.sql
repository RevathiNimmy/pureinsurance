EXECUTE DDLDropProcedure 'spu_ACT_Update_TransDetail'
GO

CREATE PROCEDURE spu_ACT_Update_TransDetail
	@transdetail_id  INT,
	@account_id  INT,
	@postingstatus_id SMALLINT,
	@company_id  INT,
	@currency_id SMALLINT,
	@period_id  INT,
	@document_id  INT,
	@document_sequence SMALLINT,
	@accounting_date DATETIME,
	@amount NUMERIC(19,4),
	@base_amount_unrounded NUMERIC(19,4),
	@fully_matched BIT,
	@currency_amount NUMERIC(19,4),
	@currency_amount_unrounded NUMERIC(19,4),
	@euro_currency_id SMALLINT,
	@euro_amount NUMERIC(19,4),
	@euro_base_xrate NUMERIC(12,8),
	@euro_ccy_xrate NUMERIC(12,8),
	@comment VARCHAR(60),
	@insurance_ref VARCHAR(30),
	@operator_id SMALLINT,
	@purchase_order_no VARCHAR(40),
	@purchase_invoice_no VARCHAR(40),
	@department VARCHAR(20),
	@spare VARCHAR(20),
	@ref_date DATETIME,
	@ref_amount NUMERIC(19,4),
	@ref_quantity NUMERIC(19,6),
	@ref_units VARCHAR(30),
	@insurance_ref_index  INT,
	@department_id SMALLINT,
	@underwriting_year_id INT,
	@currency_base_xrate FLOAT,
	@currency_base_date DATETIME,
	@account_base_xrate FLOAT,
	@account_base_date DATETIME,
	@system_base_xrate FLOAT,
	@system_base_date DATETIME,
	@transdetail_type_id SMALLINT,
	@reference VARCHAR(50),
	@type_code VARCHAR(20),
	@tax_group_id INT,
	@tax_band_id INT,
	@claim_ref VARCHAR(30),
    @balance_type VARCHAR(2),
    @risk_transfer tinyint=Null,
    @due_date DATETIME=NULL,
	@feetype VARCHAR(50)=Null
AS

DECLARE @sub_branch_id INT
DECLARE @base_currency_id INT
DECLARE @account_currency_id INT
DECLARE @account_amount MONEY
DECLARE @account_amount_unrounded MONEY
DECLARE @system_currency_id INT
DECLARE @system_amount MONEY
DECLARE @system_amount_unrounded MONEY
DECLARE @return_status INT

/*Get the converted amounts*/
EXECUTE spu_ACT_Do_Currency_Conversion
	@account_id = @account_id,
	@company_id = @company_id,
	@currency_id = @currency_id,
	@currency_amount_unrounded = @currency_amount_unrounded,
	@mode = 'ALL',
	@base_currency_id = @base_currency_id OUTPUT,
	@account_currency_id = @account_currency_id OUTPUT,
	@account_amount = @account_amount OUTPUT,
	@account_amount_unrounded = @account_amount_unrounded OUTPUT,
	@system_currency_id = @system_currency_id OUTPUT,
	@system_amount = @system_amount OUTPUT,
	@system_amount_unrounded = @system_amount_unrounded OUTPUT,
	@currency_base_xrate = @currency_base_xrate OUTPUT,
	@currency_base_date = @currency_base_date OUTPUT,
	@account_base_xrate = @account_base_xrate OUTPUT,
	@account_base_date = @account_base_date OUTPUT,
	@system_base_xrate = @system_base_xrate OUTPUT,
	@system_base_date = @system_base_date OUTPUT,
	@return_status = @return_status OUTPUT

/*Round to the Decimal Places for Postings */
EXEC spu_ACT_Round_Amounts_For_Posting
	@currency_id=@currency_id,    
	@currency_amount=@currency_amount OUTPUT,
	@base_currency_id=@base_currency_id,
	@base_amount=@amount OUTPUT,
	@account_currency_id=@account_currency_id,
	@account_amount=@account_amount OUTPUT,	
	@system_currency_id=@system_currency_id,
	@system_amount=@system_amount OUTPUT

-- Get the sub_branch from the document
SELECT @sub_branch_id = sub_branch_id
FROM   document
WHERE  document_id = @document_id

IF (@underwriting_year_id IS NULL)
BEGIN
	SELECT 	@underwriting_year_id=underwriting_year_id
	FROM	Underwriting_Year
	WHERE	@accounting_date BETWEEN start_date AND end_date
END

IF (@insurance_ref is  NULL)
BEGIN
Select @insurance_ref= Insurance_ref from insurance_file insf inner join document d on insf.insurance_file_cnt = d.insurance_file_cnt where d.document_id = @document_id
END


UPDATE TransDetail SET
	account_id=@account_id,
	postingstatus_id=@postingstatus_id,
	company_id=@company_id,
	sub_branch_id=@sub_branch_id,
	currency_id=@currency_id,
	period_id=@period_id,
	document_id=@document_id,
	document_sequence=@document_sequence,
	accounting_date=@accounting_date,
	amount=@amount,
	base_amount_unrounded=@base_amount_unrounded,
	fully_matched=@fully_matched,
	currency_amount=@currency_amount,
	currency_amount_unrounded=@currency_amount_unrounded,
	euro_currency_id=@euro_currency_id,
	euro_amount=@euro_amount,
	euro_base_xrate=@euro_base_xrate,
	euro_ccy_xrate=@euro_ccy_xrate,
	currency_base_xrate=@currency_base_xrate,
	comment=@comment,
	insurance_ref=@insurance_ref,
	operator_id=@operator_id,
	purchase_order_no=@purchase_order_no,
	purchase_invoice_no=@purchase_invoice_no,
	department=@department,
	spare=@spare,
	ref_date=@ref_date,
	ref_amount=@ref_amount,
	ref_quantity=@ref_quantity,
	ref_units=@ref_units,
	department_id=@department_id,
	amount_currency_id = @base_currency_id,
	account_currency_id = @account_currency_id,
	account_amount = @account_amount,
	account_amount_unrounded = @account_amount_unrounded,
	account_base_xrate = @account_base_xrate,
	system_currency_id = @system_currency_id,
	system_amount = @system_amount,
	system_amount_unrounded = @system_amount_unrounded,
	system_base_xrate = @system_base_xrate,
	transdetail_type_id = @transdetail_type_id,
	reference = @reference,
	type_code = @type_code,
	underwriting_year_id=@underwriting_year_id,
	tax_group_id=@tax_group_id,
	tax_band_id=@tax_band_id,
	claim_ref=@claim_ref,
	due_date=ISNULL(@due_date,due_date)
	--fee_type=@feetype
WHERE transdetail_id = @transdetail_id

IF ISNULL(@spare,'')= 'RECONCILED'
BEGIN
	UPDATE transdetail SET
	bank_reconciliation_date=GETDATE()
	WHERE transdetail_id = @transdetail_id
END

GO
