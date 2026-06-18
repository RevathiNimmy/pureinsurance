SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_TaxNotAppliedToClient'
GO

CREATE Procedure spu_SIR_Get_TaxNotAppliedToClient
	@insurance_file_cnt INT,
	@tax_amount MONEY OUTPUT
AS

SELECT	@tax_amount = SUM(TC.value)
FROM	Tax_Calculation TC
JOIN	Tax_Band TB ON TB.tax_band_id=TC.tax_band_id
JOIN	Tax_Type TT ON TT.tax_type_id=TB.tax_type_id
WHERE	TC.insurance_file_cnt = @insurance_file_cnt
AND		TC.transtype IN ('TTIF','TTR')
AND		TT.is_not_applied_to_client = 1 AND TT.is_not_applied_to_client IS NOT NULL

GO