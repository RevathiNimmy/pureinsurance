SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Party_Account_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Party_Account_Details  
 @party_cnt int  
  
AS  
  
BEGIN  
  
 SELECT  
  account.currency_id,  
  party.domiciled_for_tax,  
  party.tax_number,  
  party.tax_percentage, 
  account.account_id  
 FROM account  
  
  INNER JOIN Party ON  
   party.party_cnt = account.account_key  
  
 WHERE account_key = @party_cnt  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
