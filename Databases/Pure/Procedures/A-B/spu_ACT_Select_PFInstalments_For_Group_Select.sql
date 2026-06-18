SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_PFInstalments_For_Group_Select'
GO

CREATE PROCEDURE spu_ACT_Select_PFInstalments_For_Group_Select

@group_Id int,
@instalmentAmount float = Null

AS
BEGIN

Declare @totalGroupAmount float
select @totalGroupAmount = Sum(Round(Amount,2))  FROM pfinstalments
WHERE group_id = @group_Id  and Status in (2,3)

if @totalGroupAmount = @instalmentAmount
Begin
SELECT pfinstalments_id,
	pfinstalments_transaction.code instalment_transaction_code,
	PlanTransaction_id,
	PFTransaction_id,
	status instalment_status_id
FROM pfinstalments
	INNER JOIN pfinstalments_transaction ON
		pfinstalments.transactioncode = pfinstalments_transaction.pfinstalments_transaction_id
	INNER JOIN pfpremiumfinance ON
		pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt
	  AND pfinstalments.pfprem_finance_version = pfpremiumfinance.pfprem_finance_version
WHERE group_id = @group_Id and Status in (2,3) AND pfinstalments.Amount <> 0
End
Else
SELECT Top 1 pfinstalments_id,
	pfinstalments_transaction.code instalment_transaction_code,
	PlanTransaction_id,
	PFTransaction_id,
	status instalment_status_id
FROM pfinstalments
	INNER JOIN pfinstalments_transaction ON
		pfinstalments.transactioncode = pfinstalments_transaction.pfinstalments_transaction_id
	INNER JOIN pfpremiumfinance ON
		pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt
	  AND pfinstalments.pfprem_finance_version = pfpremiumfinance.pfprem_finance_version
WHERE group_id = @group_Id AND (@instalmentAmount is Null or Round(Amount,2) = @instalmentAmount) and Status in (2,3)
Order by InstalmentNumber desc
END

GO
