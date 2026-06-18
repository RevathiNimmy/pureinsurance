SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_PolicyTax'
GO

CREATE PROCEDURE spu_wp_PolicyTax
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT 	tb.code as 'Tax_Band_Code', 
	tb.description as 'Tax_Band_Desc', 
	ift.premium 'Tax_Premium',
	ift.percentage 'Tax_Percentage',
	ift.value 'Tax_Value',
	ift.is_value 'Tax_Is_Value',
	ift.is_manually_changed 'Tax_Is_Manually_Changed'
FROM	Tax_Calculation ift JOIN Tax_Band tb ON ift.tax_band_id = tb.tax_band_id
WHERE	ift.insurance_file_cnt = @InsuranceFileCnt
AND	ift.Tax_Calculation_cnt = @Instance2
AND	ift.risk_cnt IS NULL
AND ift.transtype='TTIF'

GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO

