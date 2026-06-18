SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_InsertMissingSubAgentCommSuspenseTD'

GO
CREATE PROCEDURE spu_InsertMissingSubAgentCommSuspenseTD  
    @SubAgentAccount_ID  INT,
    @Insurance_File_Cnt  INT,
	@Document_Cnt		 INT,
	@Transaction_Type_ID INT,
	@Amount				 NUMERIC(19,4)
    
AS  

DECLARE @posting_document_id INT 
DECLARE @document_sequence_id INT
DECLARE @reference_transdetail_id INT
DECLARE @new_transdetail_id INT
DECLARE @NewDocRef varchar(25)
DECLARE @TodaysDate DATETIME
SET @TodaysDate = GETDATE()

IF (@Transaction_Type_ID <> 7) 
BEGIN
	SELECT TOP 1 @posting_document_id = document_id FROM document 
		WHERE insurance_file_Cnt  = @Insurance_File_Cnt and documenttype_id in (43,47)

	SELECT @document_sequence_id = MAX(document_sequence) + 1 FROM transdetail WHERE document_id = @posting_document_id
END
ELSE
BEGIN
	DECLARE @reference INT
	exec spu_ACT_Generate_Next_Unique_Document_Reference 1,1,1, @reference OUTPUT
	SET @NewDocRef = CONCAT('JN', FORMAT(@reference, '0000000000'))
	SET @document_sequence_id = 1

	exec spu_ACT_add_Document @Document_id=@posting_document_id output, @company_id=19, @postingstatus_id=3, @documenttype_id=1, @auditset_id=NULL, @batch_id=NULL, @document_ref=@NewDocRef,@document_date=@TodaysDate,@created_date=@TodaysDate,@authorised_date=@TodaysDate,@comment=N'', @write_off_reason_id=NULL,@insurance_file_cnt=@Insurance_File_Cnt,@reason=N'',@sub_branch_id=19,@claim_id=NULL,@terms_of_payment_id=0,@payment_due_date=NULL

END

SELECT @reference_transdetail_id = transdetail_id FROM transdetail t WHERE t.account_id = @SubAgentAccount_ID and t.document_id = @Document_Cnt

BEGIN TRAN

BEGIN TRY

INSERT INTO [dbo].[TransDetail]
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
	@SubAgentAccount_ID,
	postingstatus_id,
	company_id ,
	sub_branch_id,
	currency_id,
	period_id ,
	@posting_document_id,
	@document_sequence_id,
	@TodaysDate,
	-1*amount,
	-1*base_amount_unrounded ,
	0,
	-1*currency_amount,
	-1* currency_amount_unrounded,
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
	-1*account_amount ,
    -1*account_amount_unrounded ,
	account_base_xrate  ,
	 system_currency_id ,
	-1*system_amount   ,
    -1*system_amount_unrounded,
	system_base_xrate  ,
    -1*amount ,
    -1*currency_amount,
	-1*account_amount,
	-1*system_amount ,
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
WHERE transdetail_id = @reference_transdetail_id

SELECT @new_transdetail_id = @@IDENTITY

exec spu_allocate_transdetail_ids @reference_transdetail_id,@new_transdetail_id,@Amount

INSERT INTO [dbo].[TransDetail]
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
	@document_sequence_id +1 ,
	@TodaysDate,
	-1*amount,
	-1*base_amount_unrounded ,
	0,
	-1*currency_amount,
	-1* currency_amount_unrounded,
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
	-1*account_amount ,
    -1*account_amount_unrounded ,
	account_base_xrate  ,
	 system_currency_id ,
	-1*system_amount   ,
    -1*system_amount_unrounded,
	system_base_xrate  ,
    -1*amount ,
    -1*currency_amount,
	-1*account_amount,
	-1*system_amount ,
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
WHERE transdetail_id = @new_transdetail_id

   COMMIT TRAN

END TRY
BEGIN CATCH

  ROLLBACK TRAN

END CATCH