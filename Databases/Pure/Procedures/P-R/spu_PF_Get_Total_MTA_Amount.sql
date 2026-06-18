SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PF_Get_Total_MTA_Amount'
GO

CREATE PROCEDURE spu_PF_Get_Total_MTA_Amount  
(
@pfprem_finance_cnt int,  
@pfprem_finance_version int  
) 
AS  
   
Declare @BaseCurrency INT  
Declare @AmountOrig_version Numeric(19,2)  
Declare @AmountPrev_version Numeric(19,2)  
  
SELECT @BaseCurrency = Min(Amount_Currency_Id)  
FROM transdetail  
WHERE transdetail_id in (
                         SELECT pftransaction_id  
			             FROM   pftransaction_id  
			             WHERE  pfprem_finance_cnt     = @pfprem_finance_cnt  
			             AND  pfprem_finance_version = @pfprem_finance_version
			            )  
 
 SELECT @AmountOrig_version = TotalCost + deposit - tax_cost - financefee  
 FROM   PFPremiumFinance 
 WHERE  pfprem_finance_cnt     = @pfprem_finance_cnt  
   AND  pfprem_finance_version = @pfprem_finance_version
  
SELECT @AmountPrev_version = ISNULL(SUM(outstanding_currency_amount),0)  
FROM   transdetail  
WHERE  transdetail_id in (
                          SELECT PlanTransaction_id  
                          FROM   PFPremiumFinance  
			              WHERE  pfprem_finance_cnt     = @pfprem_finance_cnt  
                            AND  pfprem_finance_version = @pfprem_finance_version - 1
                         )  
  
Set @AmountOrig_version = @AmountOrig_version - @AmountPrev_version  
  
Select @AmountOrig_version as base_amount , @BaseCurrency as base_currency_id  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
