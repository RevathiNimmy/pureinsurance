SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


DDLDropProcedure 'spu_PFGetDepositInfo'
GO

-- Object:  Stored Procedure dbo.spu_PFGetLastInstalmentID   
-- Script Date: 10/23/2003 9:44:14 AM 

CREATE PROCEDURE spu_PFGetDepositInfo

        @PremiumFinanceCnt Int,
        @PremiumFinanceVersion Int        

AS
-- Start PN: 56850-Prakash Varghese
-- Even though the current premium finance version may not have a deposit entry in the instalments table,
-- It could be possible that the option DepositAsInstalment is set for that PFScheme. 
-- Restructuring the query to make sure that we are getting the correct DepositAsInstalment 
-- even if there is not deposit entry in pfinstalment table for given premium finance count and version
BEGIN
	DECLARE @DepositAsInstalment AS INT  
	SET @DepositAsInstalment=NULL

	
	SELECT 
		@DepositAsInstalment=ISNULL(deposit_as_instalment, 0) 
	FROM 
		PFScheme
		INNER JOIN PFPremiumFinance
			ON PFPremiumFinance.CompanyNo = PFScheme.CompanyNo
			AND PFPremiumFinance.SchemeNo = PFScheme.SchemeNo
			AND PFPremiumFinance.SchemeVersion = PFScheme.SchemeVersion
	WHERE 
		PFPremiumFinance.pfprem_finance_cnt = @PremiumFinanceCnt

	SELECT 
		Status,  
  @DepositAsInstalment deposit_as_instalment,
  Amount,  
  Fee,  
  Tax  
 FROM  
  PFInstalments  
  INNER JOIN PFPremiumFinance  
   ON PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt  
   AND PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version  
 WHERE  
  (PFInstalments.InstalmentNumber = 0)   -- TransactionCode = 7 means Deposit record  
        AND (PFInstalments.pfprem_finance_version = @PremiumFinanceVersion)  
        AND (PFInstalments.pfprem_finance_cnt = @PremiumFinanceCnt)  
 AND PFInstalments.Amount<>0    
    
END
-- End PN: 56850-Prakash Varghese
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

