SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PFScheme_GetCurrency'
GO

CREATE Procedure spu_PFScheme_GetCurrency
	@pfprem_finance_cnt INT,
	@pfprem_finance_version INT
	
AS

SELECT  
 S.currency_id, currency_base_xrate,PF.use_trans_currency,Currency.code,cb.code,s.base_currency_id   
FROM      
 insurance_file S      
 INNER JOIN PFPremiumFinance PF ON      
 PF.insurance_file_cnt=S.insurance_file_cnt    
 INNER JOIN Currency ON  
  s.currency_id= Currency.currency_id  
  INNER JOIN Currency CB   
  ON s.base_currency_id = CB.currency_id   
WHERE      
 PF.pfprem_finance_cnt=@pfprem_finance_cnt      
 AND PF.pfprem_finance_version=@pfprem_finance_version  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO