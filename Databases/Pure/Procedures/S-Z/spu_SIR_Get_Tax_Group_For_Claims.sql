SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SIR_Get_Tax_Group_For_Claims'
GO

CREATE PROCEDURE spu_SIR_Get_Tax_Group_For_Claims
AS    
    
BEGIN    
     
	SELECT DISTINCT    
		tg.tax_group_id,    
		tg.description,    
		tg.code,    
		tg.is_withholding_tax,    
		tg.advanced_tax_script 
		
	FROM Tax_Group tg    
    	INNER JOIN Tax_group_tax_band tgtb ON tg.tax_group_id = tgtb.tax_group_id    
    	INNER JOIN Tax_Band tb ON tb.tax_band_id = tgtb.tax_band_id    
    	INNER JOIN tax_band_rate tbr ON tb.tax_band_id = tbr.tax_band_id    
    
	WHERE tg.is_deleted = 0    
		AND tg.effective_date <= GetDate()    
		--AND tg.is_withholding_tax = 1
    
END    

GO