SET quoted_identifier ON 
GO 

SET ansi_nulls ON 
GO 

EXECUTE Ddldropprocedure 'spu_RI_ModelCurrencyRate_upd' 
GO 

CREATE PROCEDURE spu_RI_ModelCurrencyRate_upd
    @currency_id INT,
    @conversion_rate DECIMAL(18,4),
    @ri_model_id INT,
	@UserId INT = null,
	@UniqueId VARCHAR(50) = null,
	@ScreenHierarchy VARCHAR(500) = null

AS
BEGIN

   IF EXISTS (
    SELECT 1 FROM RIModelCurrencyRates
    WHERE currency_id = @currency_id AND ri_model_id = @ri_model_id
)
BEGIN
    IF @conversion_rate > 0
    BEGIN
        UPDATE RIModelCurrencyRates
        SET conversion_rate = @conversion_rate,
            UserId = ISNULL(@UserId, UserId),
            UniqueId = ISNULL(@UniqueId, UniqueId),
            ScreenHierarchy = ISNULL(@ScreenHierarchy, ScreenHierarchy)
        WHERE currency_id = @currency_id AND ri_model_id = @ri_model_id;
    END
    ELSE
    BEGIN
		UPDATE RIModelCurrencyRates
        SET UserId = ISNULL(@UserId, UserId),
            UniqueId = ISNULL(@UniqueId, UniqueId),
            ScreenHierarchy = ISNULL(@ScreenHierarchy, ScreenHierarchy)
        WHERE currency_id = @currency_id AND ri_model_id = @ri_model_id;

        DELETE FROM RIModelCurrencyRates
        WHERE currency_id = @currency_id AND ri_model_id = @ri_model_id;
    END
END
ELSE
BEGIN
    IF @conversion_rate > 0
    BEGIN
        INSERT INTO RIModelCurrencyRates (currency_id, conversion_rate, ri_model_id, UserId, UniqueId, ScreenHierarchy)
        VALUES (@currency_id, @conversion_rate, @ri_model_id, @UserId, @UniqueId, @ScreenHierarchy);
    END
END

END
Go