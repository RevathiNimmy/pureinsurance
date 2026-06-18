SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_CurrencyInCompany'
GO
CREATE PROCEDURE spu_ACT_Do_CurrencyInCompany    
    @company_id smallint,    
    @currency_id smallint = NULL  
AS    
    
SELECT    
    cur.currency_id,    
    cur.iso_code,    
    cur.description    
FROM Currency cur, CompanyCurrency cc    
WHERE cur.currency_id = cc.currency_id    
AND cc.company_id = @company_id    
AND cur.is_deleted<>1  
AND   
   (  
(@currency_id IS NULL)  
OR  
(@currency_id IS NOT NULL AND cur.currency_id = @currency_id)  
   )  
ORDER BY    
    cur.iso_code    
GO

