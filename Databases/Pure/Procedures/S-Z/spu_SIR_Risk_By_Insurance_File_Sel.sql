SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Risk_By_Insurance_File_Sel'
GO


-- RAW 23/04/2004 : CQ753 : added missing columns
-- RAW 15/11/2004 : Pricing Changes : added rating rule columns


CREATE PROCEDURE spu_SIR_Risk_By_Insurance_File_Sel
    @insurance_file_cnt int
AS

-- ==========================================================================
-- result set should match spe_risk_saa and spe_risk_sel
-- The result set must also match spu_Get_Uncopied_Risks
-- ==========================================================================

SELECT
    r.risk_cnt,
    r.risk_status_id,
    r.risk_folder_cnt,
    r.accumulation_id,
    r.risk_type_id,
    r.description,
    r.sequence_number,
    r.sum_insured_requested,
    r.inception_date,
    r.expiry_date,
    r.is_not_index_linked,
    r.is_accumulated,
    r.lapsed_reason_id,
    r.lapsed_date,
    r.lapsed_description,
    r.var_data_ref,
    r.total_sum_insured,
    r.total_annual_premium,
    r.total_this_premium,
    r.is_ri_at_risk_level,
    r.is_auto_reinsured,
    r.gis_screen_id,
    r.eml_percentage,
    r.risk_number,
    r.variation_number,
    r.is_risk_selected,
    r.coverage,
    r.insured_item,
    r.extensions,
    ifr.status_flag,                                          
    pro_rata_rate,                                            -- RAW 23/04/2004 : CQ753 : added
    NULL,
	NULL,  
    NULL,  
    NULL,  
    NULL,  
    r.is_mandatory_risk
FROM    insurance_file_risk_link ifr,
    Risk r

WHERE   ifr.insurance_file_cnt = @insurance_file_cnt
AND ifr.risk_cnt = r.risk_cnt
ORDER BY r.risk_cnt ASC



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
