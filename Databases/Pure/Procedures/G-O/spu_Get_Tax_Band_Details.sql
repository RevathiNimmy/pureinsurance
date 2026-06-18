SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


Execute DDLDropProcedure 'spu_Get_Tax_Band_Details'
GO

create procedure spu_Get_Tax_Band_Details

     @TaxBandID int,
     @TaxType int

AS

SELECT     Tax_Band.tax_band_id, tax_band_rate.is_value, tax_band_rate.rate, tax_band_rate.Calc_Basis, tax_band_rate.Basis_Value, tax_band_rate.NB, 
                      tax_band_rate.AMTA, tax_band_rate.RMTA, tax_band_rate.CANC, tax_band_rate.REN, 0 AS Selected, 0 AS Applicable, 0 AS TaxAmount, 
                      tax_band_rate.is_deleted, Tax_Type.is_not_applied_to_client, Tax_Band.code, 0 AS Premium, 0 AS SumInsured, 0 As ManuallyChanged
FROM         Tax_Type INNER JOIN
                      Tax_Band ON Tax_Type.tax_type_id = Tax_Band.tax_type_id INNER JOIN
                      tax_band_rate ON Tax_Band.tax_band_id = tax_band_rate.tax_band_id
WHERE     (Tax_Type.tax_basis = @TaxType)
GROUP BY Tax_Band.tax_band_id, tax_band_rate.is_deleted, tax_band_rate.is_value, tax_band_rate.rate, tax_band_rate.Calc_Basis, 
                      tax_band_rate.Basis_Value, tax_band_rate.NB, tax_band_rate.AMTA, tax_band_rate.RMTA, tax_band_rate.CANC, tax_band_rate.REN, 
                      tax_band_rate.effective_date, tax_band_rate.is_deleted, Tax_Type.is_not_applied_to_client, Tax_Band.code
HAVING      (tax_band_rate.is_deleted = 0) AND (tax_band_rate.effective_date <= GETDATE()) AND (Tax_Band.tax_band_id = @TaxBandID)

GO


