SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_InsertMissingComissionReleaseToAgent'

GO
CREATE PROCEDURE spu_InsertMissingComissionReleaseToAgent
    @Insurance_Ref			  VARCHAR(30)
    
AS 

DECLARE @insurance_File_cnt INT
DECLARE @pfprem_finance_cnt INT
DECLARE @pfprem_finance_Version INT
DECLARE @account_id INT
DECLARE @PfInstalments_id INT
DECLARE @InstalmentPaidAmount Numeric(19,4)
DECLARE @AmountToFinance Numeric(19,4)
DECLARE @TotalCommissionValue Numeric(19,4)
DECLARE @ComputedPaidCommissonValue Numeric(19,4)
DECLARE @posting_document_id INT 
DECLARE @reference_credit_transdetail_id INT
DECLARE @credit_transdetail_id INT
DECLARE @NewDocRef varchar(25)
DECLARE @TodaysDate DATETIME
SET @TodaysDate = GETDATE()

Create table #ResultFixMissingComm
(
	insurance_File_cnt int,
	insurance_Ref VARCHAR(30),
	pfprem_finance_cnt int,
	pfprem_finance_Version int,
	account_id int
)

insert into #ResultFixMissingComm
select distinct ia.insurance_file_cnt, i.insurance_ref,pf.pfprem_finance_cnt,pf.pfprem_finance_Version,a1.account_id from insurance_file_agent ia
join (select MAX(insurance_file_cnt) AS insurance_file_cnt,insurance_ref from insurance_file where payment_method='Direct Debit' and insurance_file_type_id in (2,5,9) group by insurance_ref) i on i.insurance_file_cnt = ia.insurance_file_cnt
join insurance_file i1 on i.insurance_file_cnt = i1.insurance_file_cnt
join pfpremiumfinance pf on pf.insurance_file_cnt = i1.insurance_file_cnt and pf.statusind in ('040','990','900')
join document d on d.insurance_file_cnt = i1.insurance_file_cnt
join transdetail t on d.document_id = t.document_id
join account a on a.account_id = t.account_id
join account a1 on a1.account_key = ia.party_cnt
where i1.insurance_file_type_id in (2,5,9) and a.short_code = 'COLLACC' and d.document_date >='2020-03-01' --and d.document_ref Like 'JN%'  
and i1.insurance_ref = @Insurance_Ref
order by ia.insurance_file_cnt

Declare c Cursor For select insurance_File_cnt,insurance_Ref,pfprem_finance_cnt,pfprem_finance_Version,account_id from #ResultFixMissingComm
Open c
Fetch next From c into @insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id
While @@Fetch_Status=0 Begin

	select @TotalCommissionValue = Commission_value from agent_commission where insurance_file_cnt = @insurance_File_cnt

	Declare c1 Cursor For select pfi.pfinstalments_id,pfi.amount,p.amounttofinance from PFPremiumFinance p
							join PFInstalments pfi on p.pfprem_finance_cnt = pfi.pfprem_finance_cnt and p.pfprem_finance_version = pfi.pfprem_finance_version
							where p.pfprem_finance_cnt = @pfprem_finance_cnt and p.pfprem_finance_version = @pfprem_finance_Version and pfi.status = 3 
							and instalmentnumber >= 0
	Open c1
	Fetch next From c1 into @PfInstalments_id,@InstalmentPaidAmount,@AmountToFinance
	While @@Fetch_Status=0 Begin

		SET @ComputedPaidCommissonValue = ((@InstalmentPaidAmount / @AmountToFinance) * @TotalCommissionValue)

		select @reference_credit_transdetail_id = transdetail_id from transdetail where document_id in (select document_id from document where insurance_file_cnt = @insurance_File_cnt) and account_id = 3376 and amount < 0

		DECLARE @reference INT
		exec spu_ACT_Generate_Next_Unique_Document_Reference 1,1,1, @reference OUTPUT
		SET @NewDocRef = CONCAT('JN', FORMAT(@reference, '0000000000'))

		exec spu_ACT_add_Document @Document_id=@posting_document_id output, @company_id=19, @postingstatus_id=3, @documenttype_id=1, @auditset_id=NULL, @batch_id=NULL, @document_ref=@NewDocRef, @document_date=@TodaysDate,@created_date=@TodaysDate,@authorised_date=@TodaysDate,@comment=N'', @write_off_reason_id=NULL,@insurance_file_cnt= @Insurance_File_Cnt,@reason=N'',@sub_branch_id=19,@claim_id=NULL,@terms_of_payment_id=0,@payment_due_date=NULL

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
				1,
				--3,
				@TodaysDate,
				-1* @ComputedPaidCommissonValue,
				-1* @ComputedPaidCommissonValue ,
				0,
				-1* @ComputedPaidCommissonValue,
				-1* @ComputedPaidCommissonValue,
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
				-1* @ComputedPaidCommissonValue ,
				-1* @ComputedPaidCommissonValue ,
				account_base_xrate  ,
					system_currency_id ,
				-1*@ComputedPaidCommissonValue   ,
				-1*@ComputedPaidCommissonValue,
				system_base_xrate  ,
				-1*@ComputedPaidCommissonValue ,
				-1*@ComputedPaidCommissonValue,
				-1*@ComputedPaidCommissonValue,
				-1*@ComputedPaidCommissonValue ,
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
				2,
				@TodaysDate,
				@ComputedPaidCommissonValue,
				@ComputedPaidCommissonValue ,
				0,
				@ComputedPaidCommissonValue,
				@ComputedPaidCommissonValue,
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
				@ComputedPaidCommissonValue ,
				@ComputedPaidCommissonValue ,
				account_base_xrate  ,
				system_currency_id ,
				@ComputedPaidCommissonValue   ,
				@ComputedPaidCommissonValue,
				system_base_xrate  ,
				@ComputedPaidCommissonValue ,
				@ComputedPaidCommissonValue,
				@ComputedPaidCommissonValue,
				@ComputedPaidCommissonValue ,
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

			SET @credit_transdetail_id = @@IDENTITY

			exec spu_allocate_transdetail_ids @credit_transdetail_id,@reference_credit_transdetail_id,@ComputedPaidCommissonValue

			COMMIT TRAN

		END TRY
		BEGIN CATCH

			ROLLBACK TRAN

		END CATCH
		
	Fetch next From c1 into @PfInstalments_id,@InstalmentPaidAmount,@AmountToFinance
	End
	Close c1
	Deallocate c1
Fetch next From c into @insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id
End
Close c
Deallocate c

If(OBJECT_ID('tempdb..#ResultFixMissingComm') Is Not Null)
Begin
    Drop Table #ResultFixMissingComm
End