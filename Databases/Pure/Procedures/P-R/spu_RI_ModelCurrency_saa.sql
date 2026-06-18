SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_RI_ModelCurrency_saa'
GO

CREATE PROCEDURE spu_RI_ModelCurrency_saa
	@ri_model_id int
AS
BEGIN
   SELECT 
        c.currency_id,
        c.Description AS CurrencyDescription,
        r.conversion_rate
    FROM Currency c
    LEFT JOIN RIModelCurrencyRates r 
        ON c.currency_id = r.currency_id 
        AND (r.ri_model_id = @ri_model_id OR r.ri_model_id IS NULL)
END
GO

