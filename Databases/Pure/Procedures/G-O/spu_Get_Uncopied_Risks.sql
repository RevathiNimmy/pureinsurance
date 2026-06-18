

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_Get_Uncopied_Risks' 
GO


-- ==========================================================================  
-- result set should match spe_risk_saa and spe_risk_sel  
-- The result set must also match spu_Get_Uncopied_Risks  
-- ==========================================================================  
  
CREATE PROCEDURE spu_Get_Uncopied_Risks 
    @insurance_file_cnt int  
AS  
  
SELECT  
    r.risk_cnt,  
    risk_status_id,  
    risk_folder_cnt,  
    accumulation_id,  
    risk_type_id,  
    description,  
    sequence_number,  
    sum_insured_requested,  
    inception_date,  
    expiry_date,  
    is_not_index_linked,  
    is_accumulated,  
    lapsed_reason_id,  
    lapsed_date,  
    lapsed_description,  
    var_data_ref,  
    total_sum_insured,  
    total_annual_premium,  
    total_this_premium,  
    is_ri_at_risk_level,  
    is_auto_reinsured,  
    gis_screen_id,  
    eml_percentage,  
    risk_number,  
    variation_number,  
    is_risk_selected,  
    coverage,  
    insured_item,  
    extensions,  
    NULL,  
    NULL,  
    premium_this_year, 
    rcnl.Risk_Cover_Note_Link_Id,   
    rcnl.Cover_Note_Ref,    
    rcnl.Cover_Note_From,    
    rcnl.Cover_Note_To,
    r.Is_Mandatory_Risk	
FROM    insurance_file_risk_link ifr,
Risk r 
    
left JOIN    
     Risk_Cover_Note_Link rcnl                   ON rcnl.Risk_Id = r.risk_cnt   
  
WHERE   ifr.insurance_file_cnt = @insurance_file_cnt  
AND ifr.status_flag <> 'D' AND ifr.status_flag <> 'C'  
AND ifr.risk_cnt = r.risk_cnt  
ORDER BY r.risk_folder_cnt ASC  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
