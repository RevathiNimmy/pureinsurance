--Start (Saurabh Agrawal) Tech spec Automatic Exchange Rates


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_CurrencyRate'    
GO


CREATE PROCEDURE spu_ACT_Get_CurrencyRate    
 @company_id SMALLINT,    
 @effective_date DATETIME,    
 @currency_id INT = NULL     
AS    
    
SELECT    
 R.effective_from,    
 R.rate_against_base,    
 R.currency_id,    
 R.company_id    
FROM CurrencyRate R    
WHERE (R.company_id = @company_id OR @company_id IS NULL)    
AND R.effective_from =  @effective_date  
  
And  
   (  
 (@currency_id IS NULL)  
 OR  
 (@currency_id IS NOT NULL AND R.currency_id = @currency_id)  
   )  
ORDER BY R.currency_id, R.company_id DESC   

--End (Saurabh Agrawal) Tech spec Automatic Exchange Rates