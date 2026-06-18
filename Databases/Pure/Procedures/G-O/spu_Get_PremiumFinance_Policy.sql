SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_PremiumFinance_Policy'
GO

CREATE PROCEDURE spu_Get_PremiumFinance_Policy  
   @pfprem_finance_cnt Int  
As  
SELECT pmf.insurance_file_cnt,  
       pmf.clientid   
FROM PFPremiumFinance pmf   
WHERE pmf.pfprem_finance_cnt=@pfprem_finance_cnt  


GO

