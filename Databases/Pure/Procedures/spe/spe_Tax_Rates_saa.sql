SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Tax_Rates_saa'
GO

CREATE PROCEDURE spe_Tax_Rates_saa

AS
SELECT
    tax_rates_id,
    code,
    description,
    effective_date,
    is_deleted,
    caption_id,
    country_id,
    rate
 FROM Tax_Rates
ORDER BY tax_rates_id ASC

GO

