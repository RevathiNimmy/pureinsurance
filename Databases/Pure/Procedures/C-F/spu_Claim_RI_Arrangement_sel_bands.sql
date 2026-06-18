SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_sel_bands'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_sel_bands    
    @claim_id int    
AS    
    
    Select Distinct    
            rb.ri_band_id,    
            rb.description    
    From    ri_band rb    
    Join    claim_ri_arrangement ra    
            On ra.ri_band_id = rb.ri_band_id    
    Where   ra.claim_id = @claim_id    
    Order By    
            rb.description    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
