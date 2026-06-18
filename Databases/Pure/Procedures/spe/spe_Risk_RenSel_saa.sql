

-- ==========================================================================    
-- result set should match spe_risk_saa and spe_risk_sel    
-- The result set must also match spu_Get_Uncopied_Risks    
-- ==========================================================================    
 
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Risk_RenSel_saa'
GO
   
CREATE PROCEDURE spe_Risk_RenSel_saa    
    @insurance_file_cnt int    
AS    
    
SELECT    
    r.risk_cnt,    
    risk_status_id,    
    risk_folder_cnt,    
    accumulation_id,    
    r.risk_type_id,    
    r.description,    
    sequence_number,    
    sum_insured_requested,    
    r.inception_date,    
    r.expiry_date,    
    is_not_index_linked,    
    is_accumulated,    
    r.lapsed_reason_id,    
    r.lapsed_date,    
    r.lapsed_description,    
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
    ifrl.status_flag,    
    pro_rata_rate,                                            -- RAW 23/04/2004 : CQ753 : added    
    premium_this_year,  
    rcnl.Risk_Cover_Note_Link_Id,     
    rcnl.Cover_Note_Ref,      
    rcnl.Cover_Note_From,      
    rcnl.Cover_Note_To,
    ISNULL(r.is_mandatory_risk,0) is_mandatory_risk ,
    ifrl.is_risk_edited,
    CASE WHEN EXISTS (SELECT NULL FROM risk_type_rule_set rtrs with (nolock) INNER JOIN risk with (nolock) ON r.risk_type_id = rtrs.risk_type_id AND rtrs.type = 'RN') THEN 1 ELSE 0 END [RENRule], 
    Case WHEN Exists (Select Null From PMUser_Authority_Rule_Set_Link UARSL with (nolock)
                                    INNER JOIN Rule_Set RS with (nolock) ON RS.rule_set_id = UARSL.rule_set_id
                                    INNER JOIN Transaction_Type TT with (nolock) ON tt.transaction_type_id = UARSL.transaction_type_id
                                    Where UARSL.product_id = ifi.product_id AND TT.code = 'REN')
            THEN 1 ELSE 0 END [UALRule]
            
FROM Risk r with (nolock) 
INNER JOIN insurance_file_risk_link IFRL with (nolock) on IFRL.risk_cnt = r.risk_cnt and IFRL.insurance_file_cnt = @insurance_file_cnt AND ifrl.status_flag IN ('R','U','C')
INNER JOIN insurance_file IFI with (nolock) ON IFRL.insurance_file_cnt = IFI.insurance_file_cnt
LEFT JOIN      
     Risk_Cover_Note_Link rcnl                   ON rcnl.Risk_Id = r.risk_cnt     
ORDER BY ISNULL(r.is_mandatory_risk, 0), r.risk_number ASC    
   
  
