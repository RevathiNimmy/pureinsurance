SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_control_status_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_control_status_sel
    @Ren_Status_Code varchar(10)
AS


BEGIN

    SELECT rc.insurance_folder_cnt,
    gdm.code as data_model_code,
    insfile.insured_cnt,
    gs.is_insurer_lead, /* AK 161001 - need to know this now */
    B.description BusinessType,
    P.resolved_name Client,
    insfile.insurance_ref Policy,
    insfile.renewal_date
    FROM renewal_control rc
    INNER JOIN GIS_Data_Model gdm ON rc.gis_data_model_id = gdm.gis_data_model_id
    INNER JOIN GIS_Scheme gs ON rc.gis_scheme_id = gs.gis_scheme_id
    INNER JOIN renewal_status_type rst ON rc.renewal_status_type_id = rst.renewal_status_type_id
    INNER JOIN Party P ON P.party_cnt = rc.party_cnt
    INNER JOIN Insurance_Folder insfold ON rc.insurance_folder_cnt = insfold.insurance_folder_cnt
    INNER JOIN Insurance_File insfile ON insfile.insurance_folder_cnt = insfold.insurance_folder_cnt
    INNER JOIN GIS_Business_Type B ON B.gis_business_type_id = insfile.gemini_business_type
    WHERE insfile.insurance_file_cnt = (SELECT MIN(insurance_file_cnt)
                                        FROM Insurance_File
                                        WHERE Insurance_File.insurance_folder_cnt = rc.insurance_folder_cnt)
    AND rst.code = @Ren_Status_Code
    AND ISNULL(rc.suspension_level, 0) = 0

END
GO


