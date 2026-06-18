SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Get_InstalmentsAndDeposit_Remaining'
GO

 
CREATE PROCEDURE spu_Get_InstalmentsAndDeposit_Remaining  
    @pfprem_finance_cnt INT,  
    @pfprem_finance_version INT  
AS  
    DECLARE @InstalmentCount INT 
    DECLARE @InstalmentsProcessed INT  
    DECLARE @InstalmentsRemaining INT  
  
    SELECT @InstalmentCount = COUNT(*)  
    FROM PFInstalments  
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
    AND TransactionCode > 2  
  
 AND Amount <> 0  
  
    SELECT @InstalmentsProcessed = COUNT(*)  
    FROM PFInstalments  
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
    AND Status NOT IN (1,2,4,6,7)  
    AND TransactionCode > 2  
   
 AND Amount <> 0  
  
    SELECT @InstalmentsRemaining = @InstalmentCount - @InstalmentsProcessed  
    SELECT @InstalmentsRemaining  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO