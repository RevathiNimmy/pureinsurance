SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_Get_Version'
GO

CREATE PROCEDURE spu_PFPremiumFinance_Get_Version  
    @pfprem_finance_cnt Int  
AS  
SELECT TOP 1 pfprem_finance_version   
FROM PFPremiumFinance   
WHERE pfprem_finance_cnt=@pfprem_finance_cnt  
ORDER BY pfprem_finance_version DESC 
GO
