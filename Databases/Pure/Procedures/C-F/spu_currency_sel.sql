SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_currency_sel'
GO


CREATE PROCEDURE spu_currency_sel
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
    round_to_places,
    is_base
FROM Currency
WHERE currency_id = @currency_id


GO


