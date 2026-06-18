SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Currency'
GO


CREATE PROCEDURE spu_ACT_Select_Currency
    @currency_id smallint
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
WHERE currency_id = @currency_id
GO


