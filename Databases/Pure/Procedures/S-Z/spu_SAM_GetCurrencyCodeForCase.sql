SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_GetCurrencyCodeForCase'
GO
 
CREATE PROCEDURE spu_SAM_GetCurrencyCodeForCase  
    @base_case_id INT ,  
    @currencycode VARCHAR(30) OUTPUT
AS  
  
SELECT @currencycode=(SELECT TOP 1 C.CODE FROM Currency C
INNER JOIN Claim ON Claim.Currency_id =C.currency_id 
WHERE Claim.base_case_id =@base_case_id)
  
GO