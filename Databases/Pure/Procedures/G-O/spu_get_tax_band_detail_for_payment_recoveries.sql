EXECUTE DDLDropProcedure 'spu_get_tax_band_detail_for_payment_recoveries'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_get_tax_band_detail_for_payment_recoveries
AS
BEGIN
	SELECT tb.tax_band_id,tb.code,tb.description  
	FROM tax_band tb  
	JOIN tax_band_rate tbr  
			ON tbr.tax_band_id = tb.tax_band_id  
			WHERE   tb.is_deleted = 0  
				AND tb.effective_date <= getdate()  
				AND tbr.tax_band_rate_id =          
				   (SELECT  TOP 1 tax_band_rate_id  
					FROM    tax_band_rate tbr2  
					WHERE   tbr2.tax_band_id = tb.tax_band_id  
						AND tbr2.is_deleted = 0
						AND tbr2.TTRIPR=1  
						AND tbr2.effective_date <= getdate()  
			ORDER BY  
						tbr2.effective_date DESC)
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO