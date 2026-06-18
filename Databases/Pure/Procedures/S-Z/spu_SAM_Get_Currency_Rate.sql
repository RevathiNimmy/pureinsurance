  
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Currency_Rate'
GO

CREATE PROCEDURE spu_SAM_Get_Currency_Rate  
 @losscurrency_id SMALLINT,  
 @paymentcurrency_id SMALLINT,   
 @company_id INT,    
 @effective_date DATETIME,    
 @rate NUMERIC(19, 10) OUTPUT    
    
AS    
    
DECLARE @TypeOfRates TINYINT    
DECLARE @losscurrencyrate NUMERIC(19, 10), @paymentcurrencyrate NUMERIC(19, 10)  
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT    
    
IF @TypeOfRates = 1    
BEGIN    
 SELECT @company_id = 1    
END    
      IF @losscurrency_id<>0
	BEGIN 
SELECT @losscurrencyrate = rate_against_base    
FROM CurrencyRate CR    
WHERE CR.company_id = @company_id    
AND CR.currency_id = @losscurrency_id     
AND CR.effective_from =    
  (    
   SELECT MAX(effective_from)    
   FROM CurrencyRate    
   WHERE effective_from <= @effective_date    
   AND currency_id = CR.currency_id    
   AND company_id = CR.company_id    
  )    
   	END
ELSE
	BEGIN
		SELECT @losscurrencyrate=1
	END  
SELECT @paymentcurrencyrate = rate_against_base    
FROM CurrencyRate CR    
WHERE CR.company_id = @company_id    
AND CR.currency_id = @paymentcurrency_id      
AND CR.effective_from =    
  (    
   SELECT MAX(effective_from)    
   FROM CurrencyRate    
   WHERE effective_from <= @effective_date    
   AND currency_id = CR.currency_id    
   AND company_id = CR.company_id    
  )    
      
IF @losscurrencyrate = NULL    
BEGIN    
 SELECT @losscurrencyrate = 1    
END    
  
IF @paymentcurrencyrate = NULL    
BEGIN    
 SELECT @losscurrencyrate = 1    
END    
  
SELECT @rate=@paymentcurrencyrate/@losscurrencyrate