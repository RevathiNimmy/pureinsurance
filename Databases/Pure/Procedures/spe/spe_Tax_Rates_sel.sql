SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Tax_Rates_sel'
GO

CREATE PROCEDURE spe_Tax_Rates_sel
    @tax_rates_id smallint
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
WHERE tax_rates_id = @tax_rates_id

GO

