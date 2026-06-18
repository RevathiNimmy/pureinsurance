SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFPremiumFinance_RollbackPlan'
GO

CREATE PROCEDURE spu_PFPremiumFinance_RollbackPlan  
 @pfprem_finance_cnt INT,  
 @pfprem_finance_version INT  
AS  
  
DECLARE @previous_version INT  

IF NOT EXISTS (SELECT NULL FROM pfpremiumfinance WHERE pfprem_finance_cnt=@pfprem_finance_cnt  
AND  pfprem_finance_version=@pfprem_finance_version AND statusind IN ('010','011','012'))
RETURN
  
UPDATE  PFPremiumFinance  
SET  StatusInd='040' --back to live  
WHERE  pfprem_finance_cnt=@pfprem_finance_cnt  
AND  pfprem_finance_version=(@pfprem_finance_version-1)  
AND  StatusInd='990' --superceded  
  
DELETE PFTransaction_id  
WHERE  pfprem_finance_cnt=@pfprem_finance_cnt  
AND  pfprem_finance_version=@pfprem_finance_version  

DELETE FROM PFInstalments_History 
WHERE pfinstalments_id in (Select pfinstalments_id from pfinstalments
WHERE pfprem_finance_cnt=@pfprem_finance_cnt  
AND pfprem_finance_version=@pfprem_finance_version)
  
DELETE PFInstalments  
WHERE  pfprem_finance_cnt=@pfprem_finance_cnt  
AND  pfprem_finance_version=@pfprem_finance_version  

DELETE PFMediaTypeHistory
WHERE  pfprem_finance_cnt=@pfprem_finance_cnt  
AND  pfprem_finance_version=@pfprem_finance_version  

DELETE Tax_Calculation
WHERE  pfprem_finance_cnt=@pfprem_finance_cnt
AND  pfprem_finance_version=@pfprem_finance_version

DELETE PFPremiumFinance  
WHERE  pfprem_finance_cnt=@pfprem_finance_cnt  
AND  pfprem_finance_version=@pfprem_finance_version  



GO
