SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_SelAll_CurrencybyISO'
GO


CREATE PROCEDURE spu_ACT_Do_SelAll_CurrencybyISO
AS


SELECT
    currency_id,
    caption_id,
    iso_code,
    description,
    minor_part,
    code,
    symbol,
    alignment,
    decimal_places,
    is_deleted,
    effective_date,
    format_string,
    round_to_places
FROM Currency
ORDER BY iso_code
GO


