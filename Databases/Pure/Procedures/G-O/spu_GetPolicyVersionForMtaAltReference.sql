SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetPolicyVersionForMtaAltReference'
GO

CREATE PROCEDURE spu_GetPolicyVersionForMtaAltReference
    @alternate_reference VARCHAR(80)
AS


DECLARE @iPolicy_Insurance_File_Type_ID INT
DECLARE @iMTAPerm_Insurance_File_Type_ID INT


/* Only POLICY or MTA PERM rows will be selected from the Insurance_File table.
Find out the ID's for these */

SELECT @iPolicy_Insurance_File_Type_ID = insurance_file_type_id
                           FROM Insurance_File_Type
                           WHERE code = 'POLICY'

SELECT @iMTAPerm_Insurance_File_Type_ID = insurance_file_type_id
                        FROM Insurance_File_Type
                        WHERE code = 'MTA PERM'

SELECT p.party_cnt,
       i.insurance_folder_cnt,
       i.renewal_date,
       i.risk_code_id,
       gpl.gis_scheme_id,
       i.product_id,
       i.insurance_file_cnt,
       d.code,
       d.gis_data_model_id,
       i.risk_code_id,
       isnull(f.last_edi_message_count_received,-1) as last_edi_message_count_received,
       rf.risk_folder_cnt
FROM insurance_file i
INNER JOIN insurance_folder f ON f.insurance_folder_cnt = i.insurance_folder_cnt
INNER JOIN risk_folder rf ON rf.insurance_folder_cnt = f.insurance_folder_cnt 
INNER JOIN Risk_Code rcd on rcd.Risk_Code_Id = i.risk_code_id 
INNER JOIN Party p ON p.party_cnt = i.insured_cnt
INNER JOIN gis_policy_link gpl ON gpl.insurance_file_cnt = i.insurance_file_cnt AND gpl.gis_scheme_id IS NOT NULL
INNER JOIN gis_data_model d on d.gis_data_model_id = gpl.gis_data_model_id
WHERE (i.insurance_file_type_id = @iPolicy_Insurance_File_Type_ID OR
        i.insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID)
AND i.insurance_file_status_id IS NULL
AND i.insurance_file_cnt = 
			( SELECT MAX(insurance_file_cnt) 
				FROM insurance_file x 
				WHERE x.insurance_folder_cnt = i.insurance_folder_cnt
				AND (x.insurance_file_type_id = @iPolicy_Insurance_File_Type_ID 
				OR x.insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID)
				AND x.alternate_reference = @alternate_reference
				)	
GROUP BY p.party_cnt,
                i.insurance_folder_cnt,
                i.renewal_date,
                i.risk_code_id,
                gpl.gis_scheme_id,
                i.product_id,
        i.insurance_file_cnt,
        d.code,
        d.gis_data_model_id,
        f.last_edi_message_count_received,
        rf.risk_folder_cnt

GO