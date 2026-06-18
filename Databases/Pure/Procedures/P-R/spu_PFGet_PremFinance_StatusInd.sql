SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Execute DDLDropProcedure 'spu_PFGet_PremFinance_StatusInd'
GO

CREATE  PROCEDURE spu_PFGet_PremFinance_StatusInd  
   @premiumfinanceplancnt INT, 
   @financeplanversion INT 
AS  
  
SELECT statusind  
	FROM pfpremiumfinance
	WHERE pfprem_finance_cnt  =  @premiumfinanceplancnt 
	AND pfprem_finance_version  =  @financeplanversion 
  
SET QUOTED_IDENTIFIER OFF  
GO
SET ANSI_NULLS OFF
GO
