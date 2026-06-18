
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Claim_RI_Arrangement_saa'
GO
CREATE PROCEDURE spu_Claim_RI_Arrangement_saa  
    @claim_id int,  
--Arul Stephen  
    @mode int=1,  
    @Recovery int=2  
--End Arul Stephen  
AS  
  
    Select  ra.ri_arrangement_id,  
            ra.ri_band_id,  
            ra.ri_model_id,  
            ra.sum_insured,  
            Case @mode  
                When 0 then ra.reserve-ra.this_reserve  
                    Else    ra.reserve  
                End reserve,  
            Case @mode  
  
                When 0 Then ra.Payment-ra.this_payment     
                Else    ra.Payment     
                End payment,  
  
            ra.this_reserve,  
--Arul Stephen  
            Case  
                 When @Recovery!=2 then ra.this_salvage+ra.this_recovery  
                 Else  
                  ra.this_payment  
                  End this_payment,  
--End Arul Stephen  
            ra.is_modified,  
            c.catastrophe_code_id,  
            rm.xol_clm_ri_model_id,  
            rm.xol_clm_limit,  
            rm.xol_cat_ri_model_id,  
            rm.xol_cat_limit,  
            rm.xol_cat_reinstatements,  
     ra.Ri_arrangement_version,  
--Start-(Arul Stephen)-(Bug Fixing-PN56548)    
            Case @mode      
      
                When 0 Then (ra.salvage + ra.recovery ) - (ra.this_salvage+ra.this_recovery)     
                Else    (ra.salvage + ra.recovery )     
                End  RecoveryToDate,    
--End-(Arul Stephen)-(Bug Fixing-PN56548)
	ISNULL(ra.xol_ri_model_id,0),
	ra.incurred_to_date 
    From    claim_ri_arrangement ra  
    Left Join    ri_model rm  
            On rm.ri_model_id = ra.ri_model_id  
    Join    claim c  
            On c.claim_id = ra.claim_id  
    Where   ra.claim_id = @claim_id  
    Order By  
            ra.ri_band_id  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
