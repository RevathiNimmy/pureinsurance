SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_WhatIf_Quote_Sel'
GO


CREATE PROCEDURE spu_SirRen_WhatIf_Quote_Sel
    @effective_date DATETIME,
    @insurance_folder_cnt INT = NULL
AS

IF (@insurance_folder_cnt IS NULL)
BEGIN
    DECLARE @daynum INT

    SELECT
        rc.renewal_insurance_file_cnt,
        gdm.code as data_model_code,
        rc.renewal_gis_scheme_id,
        gisbt.code,
        i.this_premium,
        i.net_premium,
        i.tax_amount
    FROM renewal_control rc
    JOIN renewal_status_type rst 
        ON rc.renewal_status_type_id = rst.renewal_status_type_id
    JOIN Risk_Code rcd 
        ON rcd.Risk_Code_Id = rc.risk_code_id /* AK 281101 - we need to fetch timings for the group */
    JOIN Renewal_Settings rsr 
        ON rsr.product_id = rcd.risk_group_id
    JOIN Insurance_File i 
        ON i.Insurance_File_Cnt = rc.Renewal_Insurance_File_Cnt
    JOIN GIS_Business_Type gisbt 
        ON i.gemini_business_type = gisbt.gis_business_type_id
    JOIN GIS_Scheme gs 
        ON gs.gis_scheme_id = rc.renewal_gis_scheme_id  --PN23194
    LEFT JOIN GIS_Data_Model gdm 
        ON rc.gis_data_model_id = gdm.gis_data_model_id
    WHERE @effective_date >=
        DATEADD(d,
            CASE
            WHEN (SELECT ISNULL(selection_day_num, 0) FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id) <> 0 THEN
                (SELECT -selection_day_num FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id)
            WHEN (ISNULL(rsr.selection_day_num, 0)) <> 0 THEN
                -rsr.selection_day_num
            ELSE
                (SELECT -ISNULL(selection_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
            END
        , rc.renewal_date)
    AND rc.renewal_status_type_id IN (SELECT renewal_status_type_id FROM renewal_status_type WHERE code IN ('RENSEL', 'RENQUOTED', 'INVITED'))
    AND ISNULL(rc.suspension_level, 0) = 0
    AND gs.expiry_date >= GETDATE()
    AND NOT EXISTS
    (
        SELECT 
            NULL
        FROM insurance_file
        WHERE insurance_folder_cnt = rc.insurance_folder_cnt
        AND insurance_file_type_id = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'MTAPERMCAN')
    )    
END
ELSE
BEGIN
    SELECT
        rc.renewal_insurance_file_cnt,
        gdm.code as data_model_code,
        rc.renewal_gis_scheme_id,
        gisbt.code,
        i.this_premium,
        i.net_premium,
        i.tax_amount
    FROM renewal_control rc
    JOIN renewal_status_type rst 
        ON rc.renewal_status_type_id = rst.renewal_status_type_id
    JOIN Insurance_File i 
        ON i.Insurance_File_Cnt = rc.Renewal_Insurance_File_Cnt
    JOIN GIS_Business_Type gisbt 
        ON i.gemini_business_type = gisbt.gis_business_type_id
    JOIN GIS_Scheme gs 
        ON gs.gis_scheme_id = rc.renewal_gis_scheme_id  --PN23194
    LEFT JOIN GIS_Data_Model gdm 
        ON rc.gis_data_model_id = gdm.gis_data_model_id
    WHERE rc.insurance_folder_cnt = @insurance_folder_cnt
    AND rc.renewal_status_type_id IN (SELECT renewal_status_type_id FROM renewal_status_type WHERE code IN ('RENSEL', 'RENQUOTED', 'INVITED'))
    AND ISNULL(rc.suspension_level, 0) = 0
    AND gs.expiry_date >= GETDATE()
END

GO


