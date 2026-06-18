SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetTransactionCurrency'
GO

CREATE PROCEDURE spu_GetTransactionCurrency
	@InsFileCnt INT
AS
BEGIN

SELECT 
		ifile.currency_id,
		ifile.base_currency_id AS BaseCurrencyID,
		Currency.iso_code AS TransISOCode,
		c.iso_code AS BaseISOCode,
		ifile.currency_base_xrate
FROM insurance_file ifile
LEFT JOIN currency ON  
   ifile.currency_id = currency.currency_id  
LEFT JOIN currency c ON
   ifile.base_currency_id = c.currency_id
WHERE ifile.insurance_file_cnt = @InsFileCnt

END
GO
