SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_CLM_Update_Suppression'
GO

CREATE PROCEDURE spu_CLM_Update_Suppression
	@claim_id INT,
	@suppress_reserves INT,
	@suppress_payments INT,
	@suppress_recoveries INT,
	@original_suppress_reserves INT OUTPUT,
	@original_suppress_payments INT OUTPUT,
	@original_suppress_recoveries INT OUTPUT
AS

SELECT
	@original_suppress_reserves=Suppress_Reserves,
	@original_suppress_payments=Suppress_Payments,
	@original_suppress_recoveries=Suppress_Recoveries
FROM
	Claim
WHERE
	Claim_id = @claim_id

UPDATE Claim
	SET Suppress_Reserves = ISNULL(@suppress_reserves, Suppress_Reserves),
		Suppress_Payments = ISNULL(@suppress_payments, Suppress_Payments),
		Suppress_Recoveries = ISNULL(@suppress_recoveries, Suppress_Recoveries)
WHERE
	Claim_id = @claim_id

GO