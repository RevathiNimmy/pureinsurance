SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_AccountId_Form_TaxBandId'
GO

CREATE PROCEDURE spu_Get_AccountId_Form_TaxBandId
    @nTaxBandID INT
AS

	SELECT a.account_id FROM tax_band tb
	INNER JOIN Account a ON a.short_code='NOTA' + RTRIM(tb.code)
	WHERE tb.tax_band_id = @nTaxBandID	

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

