EXEC DDLDropProcedure 'spu_ACT_GetTypeOfRates'
GO

CREATE PROCEDURE spu_ACT_GetTypeOfRates
	@TypeOfRates TINYINT OUTPUT
AS

/*
	Type of Rates
	1 = Single Ledger, One Rate For All Branches.
	2 = Single Ledger, One Rate For Each Branch.
	3 = Multi Ledger, One Rate For Each Branch.
*/

/*Set defaults*/
SELECT @TypeOfRates = 2

/*If a multi-company system*/
IF	(
		SELECT value
		FROM hidden_options
		WHERE option_number = 16
		AND branch_id = 1
	) = 1
BEGIN
	SELECT @TypeOfRates = 3
END
ELSE
BEGIN
	/*else if all branches have the same currency*/
	IF	(
			SELECT SUM(1)
			FROM
			(
				SELECT base_currency_id
				FROM source
				GROUP BY base_currency_id
			) source_currency
		) = 1
	BEGIN
		/*And if option for different rates set up by different branches is off.*/
		IF ISNULL(
			(
				SELECT value
				FROM system_options
				WHERE option_number = 154
				AND branch_id = 1
			),0) = 0
		BEGIN
			/*Then set the return value to type 1*/
			SELECT @TypeOfRates = 1
		END

	END

END


GO