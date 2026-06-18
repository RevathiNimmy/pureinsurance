SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PFInstalment_Details_Update'
GO

Create Procedure spu_ACT_PFInstalment_Details_Update  
@pfprem_Financialcnt int,  
@pfFinancialVersion int,  
@nInstalmentno int,  
@DueDate DateTime  
As  
BEGIN  
DECLARE @inst_count INTEGER
Update PFInstalments set DueDate=@DueDate where pfprem_finance_cnt=@pfprem_Financialcnt  
AND pfprem_finance_version=@pfFinancialVersion And InstalmentNumber=@nInstalmentno  
select @inst_count=count(InstalmentNumber) from PFInstalments where pfprem_finance_cnt=@pfprem_Financialcnt and InstalmentNumber > 0 and pfprem_finance_version=@pfFinancialVersion 
IF @nInstalmentno = 1
BEGIN
Update PFPremiumFinance set first_instalment_date=@DueDate where pfprem_finance_cnt=@pfprem_Financialcnt  
AND pfprem_finance_version=@pfFinancialVersion  
END
ELSE IF @nInstalmentno = @inst_count
BEGIN
Update PFPremiumFinance set last_instalment_date=@DueDate where pfprem_finance_cnt=@pfprem_Financialcnt  
AND pfprem_finance_version=@pfFinancialVersion  
END
Else
Begin
Update PFPremiumFinance set next_instalment_date=@DueDate where pfprem_finance_cnt=@pfprem_Financialcnt  
AND pfprem_finance_version=@pfFinancialVersion  
End
END 
Go


