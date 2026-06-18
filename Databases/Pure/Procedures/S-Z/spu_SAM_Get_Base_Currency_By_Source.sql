SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_Get_Base_Currency_By_Source'  
GO

CREATE PROCEDURE spu_SAM_Get_Base_Currency_By_Source  
    @source_code char(10)  
AS  

SELECT Distinct cy.code 'BaseCurrencyCode' , cy.description 'BaseCurrencyDescription' FROM Source src  
INNER JOIN currency cy ON src.base_currency_id=cy.currency_id  
INNER JOIN CompanyCurrency cc ON cc.company_id=src.source_id  
WHERE src.code = @source_code   

Go
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
