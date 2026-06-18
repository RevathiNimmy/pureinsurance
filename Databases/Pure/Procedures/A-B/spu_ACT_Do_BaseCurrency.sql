SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_BaseCurrency'
GO


CREATE PROCEDURE spu_ACT_Do_BaseCurrency
    @company_id smallint
AS


SELECT
    cur.currency_id,
    cur.iso_code,
    cur.description
FROM Currency cur
WHERE cur.is_base = 1 AND cur.is_deleted<>1
ORDER BY
    cur.iso_code
GO