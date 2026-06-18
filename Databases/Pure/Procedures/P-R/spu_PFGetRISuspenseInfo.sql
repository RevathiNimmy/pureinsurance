SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_PFGetRISuspenseInfo'
GO

-- Object:  Stored Procedure dbo.spu_PFGetRISuspenseInfo  
-- Script Date: 10/21/2003 8:20:54 AM ******/

CREATE PROCEDURE spu_PFGetRISuspenseInfo
    @PremiumFinanceCnt int,
    @PremiumFinanceVersion int
AS

SELECT      Account.account_id, PFScheme.spread_ri
FROM        PFScheme INNER JOIN
                 PFPremiumFinance ON PFScheme.CompanyNo = PFPremiumFinance.CompanyNo AND
                 PFScheme.SchemeNo = PFPremiumFinance.SchemeNo AND 
                 PFScheme.SchemeVersion = PFPremiumFinance.SchemeVersion INNER JOIN
                 Account ON PFScheme.ri_suspense_account_id = Account.account_id
WHERE     (PFPremiumFinance.pfprem_finance_cnt =@PremiumFinanceCnt ) AND
                (PFPremiumFinance.pfprem_finance_version = @PremiumFinanceVersion )


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

