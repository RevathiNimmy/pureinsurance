SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Tax_Group_Tax_Band_Select'
GO



CREATE PROCEDURE spu_CLM_Tax_Group_Tax_Band_Select
AS
	BEGIN
		SELECT 
			tgtb.tax_group_id, 
			tgtb.tax_band_id, 
			tg.is_withholding_tax 

		FROM tax_group_tax_band tgtb

		INNER JOIN tax_group tg ON 
			tgtb.tax_group_id = tg.tax_group_id 
			
		INNER JOIN tax_band tb ON
			tgtb.tax_band_id = tb.tax_band_id
	
		WHERE tg.effective_date < GetDate()
		AND tb.effective_date < GetDate()
	END






GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
