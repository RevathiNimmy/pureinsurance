EXEC DDLDropProcedure 'spu_ACT_GetSystemCurrency'
GO
                 
CREATE PROCEDURE spu_ACT_GetSystemCurrency
	@currency_id SMALLINT OUTPUT
AS
	SELECT
		@currency_id = currency_id
	FROM PMSystem
	WHERE system_id = 1

GO