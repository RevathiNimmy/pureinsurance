SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_CurrencyNotInCompany'
GO


CREATE PROCEDURE spu_ACT_Do_CurrencyNotInCompany
    @company_id smallint
AS


SELECT
    cur.currency_id,
    cur.iso_code,
    cur.description
FROM Currency cur
LEFT OUTER JOIN CompanyCurrency cc
	ON cc.currency_id = cur.currency_id
	AND cc.company_id = @company_id
WHERE cc.company_id = NULL AND cur.is_deleted<>1
ORDER BY
    cur.iso_code
GO


