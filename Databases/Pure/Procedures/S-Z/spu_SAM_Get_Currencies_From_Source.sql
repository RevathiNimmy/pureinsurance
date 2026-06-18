SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_Get_Currencies_From_Source'
GO


CREATE PROCEDURE spu_SAM_Get_Currencies_From_Source
    @source_code char(10)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date       Who */
/* -------- --------------------------- ----       --- */
/* 1.0      New procedure               08-02-2006 RDT */
/********************************************************************************************************/
SELECT cy.code 'CurrencyCode', cy.description 'Description' FROM CompanyCurrency cc 
INNER JOIN currency cy ON cc.currency_id=cy.currency_id 
INNER JOIN Source src ON cc.company_id=src.source_id
WHERE src.code = @source_code and (cy.is_deleted is null or cy.is_deleted=0 )  order by cy.code

GO

