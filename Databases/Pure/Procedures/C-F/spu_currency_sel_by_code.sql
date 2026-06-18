SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_currency_sel_by_code'
GO


CREATE PROCEDURE spu_currency_sel_by_code
    @iso_code char(4)
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
WHERE iso_code = @iso_code


GO


