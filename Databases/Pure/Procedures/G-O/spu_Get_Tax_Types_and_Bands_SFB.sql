SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Tax_Types_and_Bands_SFB'
GO

CREATE PROCEDURE spu_Get_Tax_Types_and_Bands_SFB  
    @tax_group_id int = null,  
    @effective_date datetime = null,  
    @transtype varchar(7) = null  
AS  
  
    SELECT  
        @effective_date = ISNULL(@effective_date, GETDATE())  
  
    IF @tax_group_id IS NULL  
  
        SELECT  tt.tax_type_id,  
             tt.description,  
             tb.tax_band_id,  
             tb.description,  
             tbr.is_value,  
             tbr.rate,  
             tbr.currency_id,  
             tt.code,  
             1 sequence, -- no group so no cumulative taxes  
      tbr.allow_tax_credit,  
      tbr.country_id,  
      tbr.state_id,  
      tbr.class_of_business_id,
      tbr.calc_basis
        FROM tax_type tt  
        JOIN    tax_band tb  
                ON tb.tax_type_id = tt.tax_type_id  
        JOIN    tax_band_rate tbr  
                ON tbr.tax_band_id = tb.tax_band_id  
        WHERE   tt.is_deleted = 0  
            AND tt.effective_date <= @effective_date  
            AND tb.is_deleted = 0  
            AND tb.effective_date <= @effective_date  
            AND tbr.tax_band_rate_id =         -- Ensure we only get one rate for the band!!!  
               (SELECT  TOP 1 tax_band_rate_id  
                FROM    tax_band_rate tbr2  
                WHERE   tbr2.tax_band_id = tb.tax_band_id  
                    AND tbr2.is_deleted = 0  
                    AND tbr2.effective_date <= @effective_date  
                    AND CASE @TransType  
			   WHEN 'TTRITP'  THEN tbr2.TTRI  
			   WHEN 'TTRIFP'  THEN tbr2.TTRI
                           WHEN 'TTRI'  THEN tbr2.TTRI  
                           WHEN 'TTRITC' THEN tbr2.TTRIC 
                           WHEN 'TTRIFC' THEN tbr2.TTRIC 
                           WHEN 'TTRIC' THEN tbr2.TTRIC 
                           WHEN 'TTAC' THEN tbr2.TTAC  
                           WHEN 'TTF' THEN tbr2.TTF 
                           WHEN 'TTCP' THEN tbr2.TTCP
                           WHEN 'TTCS' THEN tbr2.TTCS  
                           WHEN 'TTCR' THEN tbr2.TTCR  
                           ELSE 1  
                        END > 0
                ORDER BY  
                        tbr2.effective_date DESC)  
        ORDER BY  
                tt.description,  
                tb.description  
  
    ELSE  
  
        SELECT  tt.tax_type_id,  
             tt.description,  
             tb.tax_band_id,  
             tb.description,  
             tbr.is_value,  
             tbr.rate,  
             tbr.currency_id,  
             tt.code,  
             tgtb.sequence,  
      tbr.allow_tax_credit,  
      tbr.country_id,  
      tbr.state_id,  
      tbr.class_of_business_id,tbr.calc_basis
        FROM tax_type tt  
        JOIN    tax_band tb  
                ON tb.tax_type_id = tt.tax_type_id  
        JOIN    tax_band_rate tbr  
                ON tbr.tax_band_id = tb.tax_band_id  
        JOIN    tax_group_tax_band tgtb  
                ON tgtb.tax_band_id = tb.tax_band_id  
        WHERE   tt.is_deleted = 0  
            AND tt.effective_date <= @effective_date  
            AND tb.is_deleted = 0  
            AND tb.effective_date <= @effective_date  
            AND tbr.tax_band_rate_id =         -- Ensure we only get one rate for the band!!!  
               (SELECT  TOP 1 tax_band_rate_id  
                FROM    tax_band_rate tbr2  
                WHERE   tbr2.tax_band_id = tb.tax_band_id  
                    AND tbr2.is_deleted = 0  
                    AND tbr2.effective_date <= @effective_date  
                    AND CASE @TransType  
                           WHEN 'TTRITP'  THEN tbr2.TTRI  
			   WHEN 'TTRIFP'  THEN tbr2.TTRI  
                           WHEN 'TTRI'  THEN tbr2.TTRI  
                           WHEN 'TTRITC' THEN tbr2.TTRIC  
                           WHEN 'TTRIFC' THEN tbr2.TTRIC  
                           WHEN 'TTRIC' THEN tbr2.TTRIC  
                           WHEN 'TTAC' THEN tbr2.TTAC  
                           WHEN 'TTF' THEN tbr2.TTF  
                           WHEN 'TTCP' THEN tbr2.TTCP  
                           WHEN 'TTCS' THEN tbr2.TTCS  
                           WHEN 'TTCR' THEN tbr2.TTCR  
                           ELSE 1  
                        END > 0  
                ORDER BY  
                        tbr2.effective_date DESC)  
            AND tgtb.tax_group_id = @tax_group_id  
        ORDER BY  
                tgtb.sequence,  
                tt.description,  
                tb.description  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
