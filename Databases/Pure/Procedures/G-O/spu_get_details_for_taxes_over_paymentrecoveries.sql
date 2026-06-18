EXECUTE DDLDropProcedure 'spu_get_details_for_taxes_over_paymentrecoveries'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_get_details_for_taxes_over_paymentrecoveries
    @nAccountID int    
AS
BEGIN
DECLARE @PaymentsRecoveriesEnabled int
SELECT @PaymentsRecoveriesEnabled = ISNULL(value,0) From hidden_options Where option_number = 109


	SELECT pin.is_reinsurer, @PaymentsRecoveriesEnabled
	FROM account a
	INNER JOIN party p ON a.short_code = p.shortname
	LEFT JOIN party_insurer pin ON pin.party_cnt=p.party_cnt
	WHERE a.account_id = @nAccountID


END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO



