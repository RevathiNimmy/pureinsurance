SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_CorrectComissionPosting'

GO
CREATE PROCEDURE spu_CorrectComissionPosting
    @Insurance_File_Cnt		  INT,
	@PfInstalments_id		  INT,
	@account_id				  INT,
	@DiffInPaidCommissonValue NUMERIC(19,4)
    
AS 

DECLARE @posting_document_id INT 
DECLARE @reference_credit_transdetail_id INT
DECLARE @reference_debit_transdetail_id INT
DECLARE @allocation_transdetail_id INT
DECLARE @debit_transdetail_id INT
DECLARE @credit_transdetail_id INT
DECLARE @NewDocRef varchar(25)
DECLARE @TodaysDate DATETIME
SET @TodaysDate = GETDATE()


DECLARE @reference INT
exec spu_ACT_Generate_Next_Unique_Document_Reference 1,1,1, @reference OUTPUT
SET @NewDocRef = CONCAT('JN', FORMAT(@reference, '0000000000'))

exec spu_ACT_add_Document @Document_id=@posting_document_id output, @company_id=19, @postingstatus_id=3, @documenttype_id=1, @auditset_id=NULL, @batch_id=NULL, @document_ref=@NewDocRef, @document_date=@TodaysDate,@created_date=@TodaysDate,@authorised_date=@TodaysDate,@comment=N'', @write_off_reason_id=NULL,@insurance_file_cnt= @Insurance_File_Cnt,@reason=N'',@sub_branch_id=19,@claim_id=NULL,@terms_of_payment_id=0,@payment_due_date=NULL

SELECT @reference_credit_transdetail_id = rat.destination_transdetail_id from Released_Accounts_Transactions rat 
		join transdetail t on t.transdetail_id = rat.destination_transdetail_id
		where rat.pfinstalments_id = @PfInstalments_id

BEGIN TRAN

BEGIN TRY

	INSERT INTO [TransDetail]
		([account_id]
		,[postingstatus_id]
		,[company_id]
		,[sub_branch_id]
		,[currency_id]
		,[period_id]
		,[document_id]
		,[document_sequence]
		,[accounting_date]
		,[amount]
		,[base_amount_unrounded]
		,[fully_matched]
		,[currency_amount]
		,[currency_amount_unrounded]
		,[currency_base_xrate]
		,[euro_currency_id]
		,[euro_amount]
		,[euro_base_xrate]
		,[euro_ccy_xrate]
		,[comment]
		,[insurance_ref]
		,[operator_id]
		,[purchase_order_no]
		,[purchase_invoice_no]
		,[department]
		,[spare]
		,[ref_date]
		,[ref_amount]
		,[ref_quantity]
		,[ref_units]
		,[insurance_ref_index]
		,[department_id]
		,[not_reported]
		,[underwriting_year_id]
		,[amount_currency_id]
		,[account_currency_id]
		,[account_amount]
		,[account_amount_unrounded]
		,[account_base_xrate]
		,[system_currency_id]
		,[system_amount]
		,[system_amount_unrounded]
		,[system_base_xrate]
		,[outstanding_amount]
		,[outstanding_currency_amount]
		,[outstanding_account_amount]
		,[outstanding_system_amount]
		,[amount_updated]
		,[reference]
		,[type_code]
		,[transdetail_type_id]
		,[tax_group_id]
		,[tax_band_id]
		,[batch_id]
		,[claim_ref]
		,[balance_type]
		,[risk_transfer]
		,[risk_transfer_reconciliation_date]
		,[bank_reconciliation_date]
		,[PFInstalments_id]
		,[due_date]
		,[commission_payment_batch_id]
		,[fee_type])

	SELECT 	
		@account_id,
		postingstatus_id,
		company_id ,
		sub_branch_id,
		currency_id,
		period_id ,
		@posting_document_id,
		--document_id,
		document_sequence,
		--3,
		@TodaysDate,
		-1*@DiffInPaidCommissonValue,
		-1*@DiffInPaidCommissonValue ,
		0,
		-1*@DiffInPaidCommissonValue,
		-1* @DiffInPaidCommissonValue,
		currency_base_xrate,
		euro_currency_id ,
		euro_amount,
		euro_base_xrate,
		euro_ccy_xrate,
		NULL,
		insurance_ref,
		1,
		purchase_order_no,
		purchase_invoice_no,
		department,
		NULL,
		ref_date,
		ref_amount,
		ref_quantity,
		ref_units,
		insurance_ref_index,
		department_id ,
		not_reported ,
		underwriting_year_id,
		amount_currency_id,
		account_currency_id,
		-1*@DiffInPaidCommissonValue ,
		-1*@DiffInPaidCommissonValue ,
		account_base_xrate  ,
			system_currency_id ,
		-1*@DiffInPaidCommissonValue   ,
		-1*@DiffInPaidCommissonValue,
		system_base_xrate  ,
		-1*@DiffInPaidCommissonValue ,
		-1*@DiffInPaidCommissonValue,
		-1*@DiffInPaidCommissonValue,
		-1*@DiffInPaidCommissonValue ,
		amount_updated  ,
		reference ,
		type_code ,
		13,
		tax_group_id ,
		tax_band_id,
		NULL,
		claim_ref,
		balance_type,
		risk_transfer,
		risk_transfer_reconciliation_date,
		bank_reconciliation_date,
		PFInstalments_id,
		due_date,
		NULL,
		fee_type
		FROM transdetail
		WHERE transdetail_id = @reference_credit_transdetail_id

	SET @debit_transdetail_id = @@IDENTITY

	SELECT @reference_debit_transdetail_id = t2.transdetail_id from transdetail t1
	join transdetail t2 on t1.document_id = t2.document_id
		where t1.transdetail_id = @reference_credit_transdetail_id and t2.account_id = 3376

	INSERT INTO [TransDetail]
		([account_id]
		,[postingstatus_id]
		,[company_id]
		,[sub_branch_id]
		,[currency_id]
		,[period_id]
		,[document_id]
		,[document_sequence]
		,[accounting_date]
		,[amount]
		,[base_amount_unrounded]
		,[fully_matched]
		,[currency_amount]
		,[currency_amount_unrounded]
		,[currency_base_xrate]
		,[euro_currency_id]
		,[euro_amount]
		,[euro_base_xrate]
		,[euro_ccy_xrate]
		,[comment]
		,[insurance_ref]
		,[operator_id]
		,[purchase_order_no]
		,[purchase_invoice_no]
		,[department]
		,[spare]
		,[ref_date]
		,[ref_amount]
		,[ref_quantity]
		,[ref_units]
		,[insurance_ref_index]
		,[department_id]
		,[not_reported]
		,[underwriting_year_id]
		,[amount_currency_id]
		,[account_currency_id]
		,[account_amount]
		,[account_amount_unrounded]
		,[account_base_xrate]
		,[system_currency_id]
		,[system_amount]
		,[system_amount_unrounded]
		,[system_base_xrate]
		,[outstanding_amount]
		,[outstanding_currency_amount]
		,[outstanding_account_amount]
		,[outstanding_system_amount]
		,[amount_updated]
		,[reference]
		,[type_code]
		,[transdetail_type_id]
		,[tax_group_id]
		,[tax_band_id]
		,[batch_id]
		,[claim_ref]
		,[balance_type]
		,[risk_transfer]
		,[risk_transfer_reconciliation_date]
		,[bank_reconciliation_date]
		,[PFInstalments_id]
		,[due_date]
		,[commission_payment_batch_id]
		,[fee_type])

	SELECT 	
		3376,
		postingstatus_id,
		company_id ,
		sub_branch_id,
		currency_id,
		period_id ,
		@posting_document_id,
		document_sequence,
		@TodaysDate,
		@DiffInPaidCommissonValue,
		@DiffInPaidCommissonValue ,
		0,
		@DiffInPaidCommissonValue,
		@DiffInPaidCommissonValue,
		currency_base_xrate,
		euro_currency_id ,
		euro_amount,
		euro_base_xrate,
		euro_ccy_xrate,
		NULL,
		insurance_ref,
		1,
		purchase_order_no,
		purchase_invoice_no,
		department,
		NULL,
		ref_date,
		ref_amount,
		ref_quantity,
		ref_units,
		insurance_ref_index,
		department_id ,
		not_reported ,
		underwriting_year_id,
		amount_currency_id,
		account_currency_id,
		@DiffInPaidCommissonValue ,
		@DiffInPaidCommissonValue ,
		account_base_xrate  ,
		system_currency_id ,
		@DiffInPaidCommissonValue   ,
		@DiffInPaidCommissonValue,
		system_base_xrate  ,
		@DiffInPaidCommissonValue ,
		@DiffInPaidCommissonValue,
		@DiffInPaidCommissonValue,
		@DiffInPaidCommissonValue ,
		amount_updated  ,
		reference ,
		type_code ,
		13,
		tax_group_id ,
		tax_band_id,
		NULL,
		claim_ref,
		balance_type,
		risk_transfer,
		risk_transfer_reconciliation_date,
		bank_reconciliation_date,
		PFInstalments_id,
		due_date,
		NULL,
		fee_type
		FROM transdetail
		WHERE transdetail_id = @reference_debit_transdetail_id

	SET @credit_transdetail_id = @@IDENTITY

	select @allocation_transdetail_id = ad1.transdetail_id from allocationdetail ad
		join allocationdetail ad1 on ad.allocation_id = ad1.allocation_id
		where ad.transdetail_id = @reference_debit_transdetail_id and ad1.transdetail_id <> @reference_debit_transdetail_id

	SET @DiffInPaidCommissonValue = -1 * @DiffInPaidCommissonValue
	exec spu_allocate_transdetail_ids @allocation_transdetail_id,@credit_transdetail_id,@DiffInPaidCommissonValue

	INSERT INTO [Released_Accounts_Transactions]
           ([suspended_transdetail_id]
           ,[destination_transdetail_id]
           ,[allocation_id]
           ,[release_date]
           ,[recall_date]
           ,[pfinstalments_id])
     Select 
           [suspended_transdetail_id]
           ,@debit_transdetail_id
           ,[allocation_id]
           ,[release_date]
           ,[recall_date]
           ,[pfinstalments_id] from [Released_Accounts_Transactions]
		   where [destination_transdetail_id] = @reference_credit_transdetail_id


	COMMIT TRAN

END TRY
BEGIN CATCH

	ROLLBACK TRAN

END CATCH