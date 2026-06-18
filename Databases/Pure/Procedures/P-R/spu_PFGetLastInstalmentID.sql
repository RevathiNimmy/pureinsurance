SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

DDLDropProcedure 'spu_PFGetLastInstalmentID'
GO

-- Object:  Stored Procedure dbo.spu_PFGetLastInstalmentID 
-- Script Date: 10/23/2003 9:44:14 AM 

CREATE PROCEDURE spu_PFGetLastInstalmentID
       @PremiumFinanceCnt Int,    
        @PremiumFinanceVersion Int    
    
AS    
    
-- TransactionCode = 6 Transalate to Last payment    
SELECT     TOP 1 pfinstalments_id ,TransactionCode   
FROM         PFInstalments pfi INNER JOIN PFPremiumFinance pf ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pf.pfprem_finance_version=pfi.pfprem_finance_version     
WHERE      pf.pfprem_finance_cnt = @PremiumFinanceCnt AND    
           pf.pfprem_finance_version = @PremiumFinanceVersion AND pfi.InstalmentNumber <> 0
		       ORDER BY pfi.InstalmentNumber DESC, pfinstalments_id DESC  


  GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


