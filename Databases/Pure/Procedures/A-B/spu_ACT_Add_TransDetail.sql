
EXECUTE DDLDropProcedure 'spu_ACT_Add_TransDetail'
GO

CREATE PROCEDURE spu_ACT_Add_TransDetail  
 @transdetail_id INT OUTPUT,  
 @account_id INT,  
 @postingstatus_id  SMALLINT,  
 @company_id INT,  
 @currency_id  SMALLINT,  
 @period_id INT,  
 @document_id INT,  
 @document_sequence  SMALLINT,  
 @accounting_date datetime,  
 @amount NUMERIC(19,4),  
 @base_amount_unrounded NUMERIC(19,4),  
 @fully_matched BIT,  
 @currency_amount NUMERIC(19,4),  
 @currency_amount_unrounded NUMERIC(19,4),  
 @euro_currency_id  SMALLINT,  
 @euro_amount NUMERIC(19,4),  
 @euro_base_xrate NUMERIC(12,8),  
 @euro_ccy_xrate NUMERIC(12,8),  
 @comment VARCHAR(60),  
 @insurance_ref VARCHAR(30),  
 @operator_id  SMALLINT,  
 @purchase_order_no VARCHAR(40),  
 @purchase_invoice_no VARCHAR(40),  
 @department VARCHAR(20),  
 @spare VARCHAR(100),  
 @ref_date datetime,  
 @ref_amount NUMERIC(19,4),  
 @ref_quantity NUMERIC(19,6),  
 @ref_units VARCHAR(30),  
 @insurance_ref_index INT,  
 @department_id  SMALLINT,  
 @underwriting_year_id INT,  
 @currency_base_xrate NUMERIC(19,10),
 @currency_base_date DATETIME,  
 @account_base_xrate FLOAT,  
 @account_base_date DATETIME,  
 @system_base_xrate FLOAT,  
 @system_base_date DATETIME,  
 @transdetail_type_id SMALLINT,  
 @reference VARCHAR(80),  
 @type_code VARCHAR(20),  
 @tax_group_id INT,  
 @tax_band_id INT,  
 @claim_ref VARCHAR(30),  
 @balance_type VARCHAR(2),  
 @risk_transfer tinyint=Null,
 @due_date DATETIME,
 @feetype VARCHAR(50)=Null   
AS  
  
DECLARE @nSub_branch_id INT
DECLARE @nBase_currency_id INT
DECLARE @nAccount_currency_id INT
DECLARE @crAccount_amount MONEY
DECLARE @crAccount_amount_unrounded MONEY
DECLARE @nSystem_currency_id INT
DECLARE @crSystem_amount MONEY
DECLARE @crSystem_amount_unrounded MONEY
DECLARE @nReturn_status INT
  
/*Get the converted amounts*/
EXECUTE spu_ACT_Do_Currency_Conversion
	@account_id = @account_id,
	@company_id = @company_id,
	@currency_id = @currency_id,
	@currency_amount_unrounded = @currency_amount_unrounded,
	@mode = 'ALL',
	@Base_currency_id = @nBase_currency_id OUTPUT,
	@Account_currency_id = @nAccount_currency_id OUTPUT,
	@Account_amount = @crAccount_amount OUTPUT,
	@Account_amount_unrounded = @crAccount_amount_unrounded OUTPUT,
	@System_currency_id = @nSystem_currency_id OUTPUT,
	@System_amount = @crSystem_amount OUTPUT,
	@System_amount_unrounded = @crSystem_amount_unrounded OUTPUT,
	@currency_base_xrate = @currency_base_xrate OUTPUT,
	@currency_base_date = @currency_base_date OUTPUT,
	@account_base_xrate = @account_base_xrate OUTPUT,
	@account_base_date = @account_base_date OUTPUT,
	@system_base_xrate = @system_base_xrate OUTPUT,
	@system_base_date = @system_base_date OUTPUT,
	@Return_status = @nReturn_status OUTPUT
  
/*Round to the Decimal Places for Postings */  
EXEC spu_ACT_Round_Amounts_For_Posting  
 @currency_id=@currency_id,  
 @currency_amount=@currency_amount OUTPUT,  
 @base_currency_id=@nBase_currency_id,  
 @base_amount=@amount OUTPUT,  
 @account_currency_id=@nAccount_currency_id,  
 @account_amount=@crAccount_amount OUTPUT,  
 @system_currency_id=@nSystem_currency_id,  
 @system_amount=@crSystem_amount OUTPUT  
  
/*Get sub branch*/  
SELECT @nSub_branch_id = sub_branch_id  
FROM   document WITH(NOLOCK) 
WHERE  document_id = @document_id  
  
/*Get underwriting year*/  
IF (@underwriting_year_id IS NULL)  
BEGIN  
 SELECT  @underwriting_year_id=underwriting_year_id  
 FROM Underwriting_Year WITH(NOLOCK)  
 WHERE @accounting_date BETWEEN start_date AND end_date  
END  
  
IF (@transdetail_type_id IS NULL AND @spare='WRITEOFF')  
BEGIN  
 SELECT  @transdetail_type_id=transdetail_type_id  
 FROM transdetail_type WITH(NOLOCK)  
 WHERE code ='WRITEOFF'  
END  

IF (@insurance_ref is  NULL)
BEGIN
Select @insurance_ref= Insurance_ref from insurance_file insf inner join document d on insf.insurance_file_cnt = d.insurance_file_cnt where d.document_id = @document_id
END
  
INSERT INTO TransDetail  
(  
 account_id,  
 postingstatus_id,  
 company_id,  
 sub_branch_id,  
 currency_id,  
 period_id,  
 document_id,  
 document_sequence,  
 accounting_date,  
 amount,  
 base_amount_unrounded,  
 fully_matched,  
 currency_amount,  
 currency_amount_unrounded,  
 currency_base_xrate,  
 euro_currency_id,  
 euro_amount,  
 euro_base_xrate,  
 euro_ccy_xrate,  
 comment,  
 insurance_ref,  
 operator_id,  
 purchase_order_no,  
 purchase_invoice_no,  
 department,  
 spare,  
 ref_date,  
 ref_amount,  
 ref_quantity,  
 ref_units,  
 insurance_ref_index,  
 department_id,  
 amount_currency_id,  
 account_currency_id,  
 account_amount,  
 account_amount_unrounded,  
 account_base_xrate,  
 system_currency_id,  
 system_amount,  
 system_amount_unrounded,  
 system_base_xrate,  
 outstanding_amount,  
 outstanding_currency_amount,  
 outstanding_account_amount,  
 outstanding_system_amount,  
 amount_updated,  
 transdetail_type_id,  
 reference,  
 type_code,  
 underwriting_year_id,  
 tax_group_id,  
 tax_band_id,  
 claim_ref,  
        balance_type,  
        risk_transfer,
		due_date,
        fee_type    
)  VALUES  
(  
 @account_id,  
 @postingstatus_id,  
 @company_id,  
 @nSub_branch_id,  
 @currency_id,  
 @period_id,  
 @document_id,  
 @document_sequence,  
 @accounting_date,  
 @amount,  
 @base_amount_unrounded,  
 @fully_matched,  
 @currency_amount,  
 @currency_amount_unrounded,  
 @currency_base_xrate,  
 @euro_currency_id,  
 @euro_amount,  
 @euro_base_xrate,  
 @euro_ccy_xrate,  
 @comment,  
 @insurance_ref,  
 @operator_id,  
 @purchase_order_no,  
 @purchase_invoice_no,  
 @department,  
 @spare,  
 @ref_date,  
 @ref_amount,  
 @ref_quantity,  
 @ref_units,  
 @insurance_ref_index,  
 @department_id,  
 @nBase_currency_id,  
 @nAccount_currency_id,  
 @crAccount_amount,  
 @crAccount_amount_unrounded,  
 @account_base_xrate,  
 @nSystem_currency_id,  
 @crSystem_amount,  
 @crSystem_amount_unrounded,  
 @system_base_xrate,  
 @amount,  
 @currency_amount,  
 @crAccount_amount,  
 @crSystem_amount,  
 GETDATE(),  
 @transdetail_type_id,  
 @reference,  
 @type_code,  
 @underwriting_year_id,  
 @tax_group_id,  
 @tax_band_id,  
 @claim_ref,  
 @balance_type,  
  @risk_transfer,  
  @due_date,
  @feetype
)  
  
SELECT @transdetail_id = @@IDENTITY  

GO


