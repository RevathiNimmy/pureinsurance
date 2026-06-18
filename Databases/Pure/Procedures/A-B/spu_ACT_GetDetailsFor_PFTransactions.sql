SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_GetDetailsFor_PFTransactions'
GO

-- Object:  Stored Procedure dbo.spu_ACT_GetDetailsFor_PFTransactions  
-- Script Date: 10/21/2003 8:26:13 AM ******/
-- FSA Phase 3.2 Modified to access new suspended_accounts_table
CREATE PROC spu_ACT_GetDetailsFor_PFTransactions
  
    @PremiumFinanceCnt int,
    @PremiumFinanceVersion int,
    @InstalmentID int

AS 

Declare @AgentType tinyint
DECLARE @TotalPendingInstalments INT

select @AgentType=ISNULL(party_agent_type_id,0) FROM 
Suspended_Accounts_Transactions susp
JOIN Account a ON a.account_id=susp.destination_account_id
JOIN Party p ON p.party_cnt=a.account_key
JOIN Party_Agent PA ON pa.party_cnt=p.party_cnt
  WHERE pfprem_finance_cnt=@PremiumFinanceCnt 
 and pfprem_finance_version=@PremiumFinanceVersion

 SELECT @TotalPendingInstalments=COUNT(pfi.pfInstalments_ID) FROM PFInstalments pfi 
WHERE pfi.pfprem_finance_cnt=@PremiumFinanceCnt AND pfi.pfprem_finance_version=@PremiumFinanceVersion AND  pfi.Status<>3 AND pfi.amount<>0
 
SELECT     Susp.suspended_TransDetail_ID as TransDetailID, 
                -- Changed by AAB - Oct 2, 2003
                -- We need to add the deposit to the Amount to finance to get the total amount of the premium to get an accurate percentage

(SELECT Case @AgentType 
WHEN 2 THEN
(SELECT Inst.Amount FROM pfInstalments Inst 
	       WHERE pfInstalments_ID = @InstalmentID) /

             (PFPlan.AmountToFinance  + PFPlan.financefee + tax_cost) 
ELSE 
(SELECT Inst.Amount FROM pfInstalments Inst 
	       WHERE pfInstalments_ID = @InstalmentID) /
             (PFPlan.AmountToFinance + PFPlan.InterestCost + PFPlan.financefee + tax_cost) 
			 END )			  
			  AS ReleasePercentage,

(SELECT Case WHEN  @TotalPendingInstalments=1 THEN 1 ELSE 0 END         
       FROM  PFInstalments pfi INNER JOIN PFPremiumFinance pf 
            ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pf.pfprem_finance_version=pfi.pfprem_finance_version 
        WHERE pfInstalments_ID = @InstalmentID) as IsLastInstalment, 

Susp.Destination_Account_ID as TransAccountID,
(SELECT  MAX(Trans.Currency_Amount) - ISNULL(SUM(Alloc.alloc_ccy_amount), 0)
	 FROM 	TransDetail Trans
	 LEFT OUTER JOIN AllocationDetail Alloc ON Trans.TransDetail_id = Alloc.TransDetail_ID
	WHERE Trans.TransDetail_ID =  Susp.suspended_TransDetail_ID) as ReleaseAmount,@TotalPendingInstalments as PendingInstalments
			
FROM         PFPremiumFinance PFPlan
	     INNER JOIN	PFScheme PFSch ON PFPlan.CompanyNo = PFSch.CompanyNo 
				       AND PFPlan.SchemeNo = PFSch.SchemeNo 
				       AND PFPlan.SchemeVersion = PFSch.SchemeVersion 
	     INNER JOIN Suspended_Accounts_Transactions Susp 
				       ON PFPlan.pfprem_finance_cnt = Susp.PFPrem_Finance_Cnt 
				       AND PFPlan.pfprem_finance_version = Susp.PFPrem_Finance_Version
WHERE     (PFPlan.pfprem_finance_cnt = @PremiumFinanceCnt) 
AND       (PFPlan.pfprem_finance_version = @PremiumFinanceVersion)
AND 	  (Susp.Is_Deleted = 0)  


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

