SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_CancellationTransactions_Get'
GO

CREATE PROCEDURE spu_PFPremiumFinance_CancellationTransactions_Get
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int
AS  
SELECT pfct.transdetail_id,
    td.outstanding_amount,
    td.account_id
FROM PFPremiumFinance_Cancellation_Transactions pfct
LEFT JOIN transdetail td ON td.transdetail_id=pfct.transdetail_id
WHERE pfct.pfprem_finance_cnt=@pfprem_finance_cnt
AND pfct.pfprem_finance_version=@pfprem_finance_version
GO
