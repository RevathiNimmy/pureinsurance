SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_GetPolRen'
GO


CREATE PROCEDURE spu_SIRRen_GetPolRen
    @insurance_folder_cnt INT
AS

SELECT 
    insf.insurance_file_cnt,
    insf.insurance_file_status_id,
    insf.insurance_file_type_id,
    insf.insurance_ref,
    insf.product_id,
    insf.lead_insurer_cnt,
    insf.insured_cnt,
    insf.date_issued,
    insf.cover_start_date,
    insf.renewal_date,
    insf.risk_code_id,
    insf.this_premium,
    insf.net_premium,
    insf.commission_amount,
    insf.iptable_amount,
    insf.ipt_percentage,
    insf.tax_amount,
    insf.vatable_amount,
    insf.vat_percentage,
    insf.vat_amount,
    insf.insured_name,
    ift.code as insurance_file_type_code,
    insfol.code as insurance_folder_code,
    insfol.description as insurancer_folder_desc,
    gdm.code as gis_data_model_code,
    insf.business_type_id,
    insf.source_id,
    ins.last_trans_description,
    ins.last_modified,
    insf.edi_message_sent,
    insf.policy_type_id,
    insf.expiry_date,
    insf.country_id
FROM insurance_file insf
JOIN insurance_file_type ift
    ON ift.insurance_file_type_id = insf.insurance_file_type_id
    AND ift.code IN ('RENEWAL', 'RENEWALWIF', 'POLICY')
JOIN insurance_file_status ifst
    ON ifst.insurance_file_status_id = insf.insurance_file_status_id
    AND ifst.code = 'REN'
JOIN insurance_folder insfol
    ON insfol.insurance_folder_cnt = insf.insurance_folder_cnt
JOIN gis_policy_link gpl
    ON gpl.insurance_file_cnt = insf.insurance_file_cnt
JOIN gis_data_model gdm
    ON gdm.gis_data_model_id = gpl.gis_data_model_id
JOIN insurance_file_system ins
    ON ins.insurance_file_cnt = insf.insurance_file_cnt
WHERE insf.insurance_folder_cnt = @insurance_folder_cnt
ORDER BY insf.this_premium


GO


