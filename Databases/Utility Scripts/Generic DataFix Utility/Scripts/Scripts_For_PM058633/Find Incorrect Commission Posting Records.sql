DECLARE @insurance_File_cnt INT
DECLARE @insurance_Ref VARCHAR(30)
DECLARE @pfprem_finance_cnt INT
DECLARE @pfprem_finance_Version INT
DECLARE @account_id INT
DECLARE @PfInstalments_id INT
DECLARE @InstalmentPaidAmount Numeric(19,4)
DECLARE @AmountToFinance Numeric(19,4)
DECLARE @TotalCommissionValue Numeric(19,4)
DECLARE @PaidCommissonValue Numeric(19,4)
DECLARE @ComputedPaidCommissonValue Numeric(19,4)
DECLARE @DiffInPaidCommissonValue Numeric(19,4)
DECLARE @posting_document_id INT 
DECLARE @reference_credit_transdetail_id INT
--DECLARE @reference_debit_transdetail_id INT
DECLARE @reference_credit_transdetail_date DATETIME
DECLARE @NewDocRef varchar(25)
DECLARE @TodaysDate DATETIME
SET @TodaysDate = GETDATE()

Create table #Result
(
	insurance_File_cnt int,
	insurance_Ref VARCHAR(30),
	pfprem_finance_cnt int,
	pfprem_finance_Version int,
	account_id int
)

Create table #Result1
(
	insurance_File_cnt int,
	insurance_Ref VARCHAR(30),
	pfprem_finance_cnt int,
	pfprem_finance_Version int,
	account_id int,
	PfInstalments_id int,
	InstalmentPaidAmount Numeric(19,2),
	AmountToFinance Numeric(19,2),
	TotalCommission Numeric(19,2),
	PaidCommissonValue Numeric(19,2),
	ComputedPaidCommissonValue Numeric(19,2),
	DiffInPaidCommissonValue Numeric(19,2),
	--reference_credit_transdetail_id int,
	reference_credit_transdetail_date date
)

insert into #Result
select distinct ia.insurance_file_cnt, i.insurance_ref,pf.pfprem_finance_cnt,pf.pfprem_finance_Version,a1.account_id from insurance_file_agent ia
join (select MAX(insurance_file_cnt) AS insurance_file_cnt,insurance_ref from insurance_file where payment_method='Direct Debit' and insurance_file_type_id in (2,5,9) group by insurance_ref) i on i.insurance_file_cnt = ia.insurance_file_cnt
join insurance_file i1 on i.insurance_file_cnt = i1.insurance_file_cnt
join pfpremiumfinance pf on pf.insurance_file_cnt = i1.insurance_file_cnt and pf.statusind in ('040','990','900','999')
join document d on d.insurance_file_cnt = i1.insurance_file_cnt
join transdetail t on d.document_id = t.document_id
join account a on a.account_id = t.account_id
join account a1 on a1.account_key = ia.party_cnt
where i1.insurance_file_type_id in (2,5,9) and a.short_code = 'COLLACC' and d.document_ref Like 'JN%'  
--and i1.insurance_ref = 'HEPHOM00048286'
order by ia.insurance_file_cnt

Declare c Cursor For select insurance_File_cnt,insurance_Ref,pfprem_finance_cnt,pfprem_finance_Version,account_id from #Result
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
		SELECT @PaidCommissonValue = SUM(ISNULL(t.amount,0)) from Released_Accounts_Transactions rat 
		join transdetail t on t.transdetail_id = rat.destination_transdetail_id
		where rat.pfinstalments_id = @PfInstalments_id group by rat.pfinstalments_id

		select @reference_credit_transdetail_date = t.accounting_date from pfinstalments pfi
		join transdetail t on pfi.pftransaction_id = t.transdetail_id
		where pfi.pfinstalments_id = @PfInstalments_id

		SET @ComputedPaidCommissonValue = ((@InstalmentPaidAmount / @AmountToFinance) * @TotalCommissionValue) * -1
		SET @DiffInPaidCommissonValue = @PaidCommissonValue - @ComputedPaidCommissonValue
		IF (CAST(@DiffInPaidCommissonValue AS INT) <> 0 AND CAST(@PaidCommissonValue AS INT) <> 0)
		BEGIN
			INSERT INTO #Result1 values(@insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id,@PfInstalments_id,ROUND(@InstalmentPaidAmount,2), ROUND(@AmountToFinance,2), ROUND(@TotalCommissionValue,2),ROUND(@PaidCommissonValue,2),ROUND(@ComputedPaidCommissonValue,2), ROUND(@DiffInPaidCommissonValue,2),@reference_credit_transdetail_date)
		END
		SET @PaidCommissonValue = 0
	Fetch next From c1 into @PfInstalments_id,@InstalmentPaidAmount,@AmountToFinance
	End
	Close c1
	Deallocate c1
Fetch next From c into @insurance_File_cnt,@insurance_Ref,@pfprem_finance_cnt,@pfprem_finance_Version,@account_id
End
Close c
Deallocate c

select * from #Result1

If(OBJECT_ID('tempdb..#Result1') Is Not Null)
Begin
    Drop Table #Result1
End

If(OBJECT_ID('tempdb..#Result') Is Not Null)
Begin
    Drop Table #Result
End