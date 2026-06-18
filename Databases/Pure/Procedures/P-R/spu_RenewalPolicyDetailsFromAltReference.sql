SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RenewalPolicyDetailsFromAltReference'
GO

CREATE PROCEDURE spu_RenewalPolicyDetailsFromAltReference
    @alternate_reference VARCHAR(80)
AS


SELECT p.party_cnt,
       i.insurance_folder_cnt,
       i.renewal_date,
       i.risk_code_id,
       rc.renewal_gis_scheme_id,
       i.product_id,
       i.insurance_file_cnt,
       d.code,
       d.gis_data_model_id,
       i.risk_code_id,
       isnull(f.last_edi_message_count_received,-1) as last_edi_message_count_received,
       rf.risk_folder_cnt,
       i.this_premium,
       i.net_premium,
       i.tax_amount
FROM insurance_file i
INNER JOIN renewal_control rc ON renewal_insurance_file_cnt = i.insurance_file_cnt
INNER JOIN insurance_folder f ON f.insurance_folder_cnt = rc.insurance_folder_cnt
INNER JOIN risk_folder rf ON rf.insurance_folder_cnt = f.insurance_folder_cnt 
INNER JOIN Risk_Code rcd on rcd.Risk_Code_Id = i.risk_code_id 
INNER JOIN Party p ON p.party_cnt = i.insured_cnt
INNER JOIN gis_policy_link gpl ON gpl.insurance_file_cnt = i.insurance_file_cnt 
INNER JOIN gis_data_model d on d.gis_data_model_id = gpl.gis_data_model_id
WHERE i.alternate_reference = @alternate_reference
GROUP BY p.party_cnt,
         i.insurance_folder_cnt,
         i.renewal_date,
         i.risk_code_id,
         rc.renewal_gis_scheme_id,
         i.product_id,
         i.insurance_file_cnt,
         d.code,
         d.gis_data_model_id,
         f.last_edi_message_count_received,
         rf.risk_folder_cnt,
         i.this_premium,
	 i.net_premium,
         i.tax_amount
GO