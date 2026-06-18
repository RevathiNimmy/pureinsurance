
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Get_Instalments_Remaining'
GO

--Object:  Stored Procedure dbo.spu_Get_Instalments_Remaining   
-- Script Date: 10/9/2003 2:31:25 PM ******/
CREATE PROCEDURE spu_Get_Instalments_Remaining  
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int  
AS  
    DECLARE @InstalmentCount int  
    DECLARE @InstalmentsProcessed int  
    DECLARE @InstalmentsRemaining int  
  
IF EXISTS(SELECT NULL FROM PFPremiumFinance pf INNER JOIN PFScheme pfs ON pf.CompanyNo=pfs.CompanyNo AND pf.SchemeNo=pfs.SchemeNo AND pf.SchemeVersion=pfs.SchemeVersion 
			WHERE  pf.pfprem_finance_cnt=@pfprem_finance_cnt AND pf.pfprem_finance_version=@pfprem_finance_version AND pfs.pfscheme_type_id=1)
 BEGIN
 SELECT NoOfInstallments from PFPremiumFinance WHERE PFPremiumFinance.pfprem_finance_cnt=@pfprem_finance_cnt  AND PFPremiumFinance.pfprem_finance_version=@pfprem_finance_version
 RETURN
 END
    SELECT @InstalmentCount = Count(*)  
    FROM PFInstalments  
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
    AND TransactionCode > 2  
	AND InstalmentNumber != 0
	AND Amount <> 0
  
    SELECT @InstalmentsProcessed = Count(*)  
    FROM PFInstalments  
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
    AND Status NOT In(1,2,4,5,6,7)
    AND TransactionCode > 2  
	AND InstalmentNumber != 0
	AND Amount <> 0
  
    SELECT @InstalmentsRemaining = @InstalmentCount - @InstalmentsProcessed  
    SELECT @InstalmentsRemaining  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


