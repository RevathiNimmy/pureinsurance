SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_GetPFFromInstalmentsID'
GO

-- Object:  Stored Procedure dbo.spu_ACT_GetPFFromInstalmentsID   
-- Script Date: 10/21/2003 8:31:45 AM

CREATE PROCEDURE spu_ACT_GetPFFromInstalmentsID
    @InstalmentsID int
AS 

SELECT	PFPremiumFinance.pfprem_finance_cnt, 
		PFPremiumFinance.pfprem_finance_version, 
		ISNULL(PFScheme.spread_commission, 0) AS spread_commission, 
		PFInstalments.DueDate, 
		ISNULL(PFScheme.spread_ri, 0) AS spread_ri,
		PFInstalments.tax,
		PFPremiumFinance.NoOfInstallments As NoOfInstallments
FROM    PFPremiumFinance INNER JOIN
		PFInstalments ON PFPremiumFinance.pfprem_finance_cnt = PFInstalments.pfprem_finance_cnt AND 
		PFPremiumFinance.pfprem_finance_version = PFInstalments.pfprem_finance_version INNER JOIN
		PFScheme ON PFPremiumFinance.CompanyNo = PFScheme.CompanyNo AND PFPremiumFinance.SchemeNo = PFScheme.SchemeNo AND 
		PFPremiumFinance.SchemeVersion = PFScheme.SchemeVersion
WHERE   pfInstalments.pfInstalments_ID = @InstalmentsID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

