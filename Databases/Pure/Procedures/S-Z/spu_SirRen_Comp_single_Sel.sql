SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRREN_Comp_Single_Sel'
GO


CREATE PROCEDURE spu_SIRREN_Comp_Single_Sel
    @effective_date datetime,
    @insurance_folder_cnt integer = NULL
AS

IF (@insurance_folder_cnt IS NULL) BEGIN
	--No Insurance_Folder
	DECLARE @daynum integer
    
	--Lapse any records that have not been renewed after 15 days
	EXECUTE spu_SIRREN_CompLapse_Sel @effective_date
    
	SELECT
		rc.insurance_folder_cnt,
		rc.gis_scheme_id,
		rs.code,
		rc.renewal_gis_scheme_id,
		rc.renewal_insurance_file_cnt,
		rc.product_id,
		rc.renewal_date,
		rc.party_cnt,
		rc.risk_code_id,
		rc.gis_data_model_id,
		IsNull(rc.renewal_edi_audit_id, 0),
		g.code,
		ift.code,
		gb.gis_business_type_id,
		rc.old_insurance_file_cnt,
		gbt.code
	FROM Renewal_Control rc
	INNER JOIN Renewal_Status_Type rs ON rs.renewal_status_type_id = rc.renewal_status_type_id
	INNER JOIN Risk_Code rcd on rcd.Risk_Code_Id = rc.risk_code_id	--Need to fetch timings for the group
	INNER JOIN Renewal_Settings rsr ON rsr.product_id = rcd.risk_group_id
	INNER JOIN GIS_Data_Model g ON g.gis_data_model_id = rc.gis_data_model_id
	INNER JOIN GIS_QEM_Usage gb ON rc.gis_scheme_id = gb.gis_scheme_id
	INNER JOIN GIS_Business_Type gbt ON gb.gis_business_type_id = gbt.gis_business_type_id
	INNER JOIN Insurance_File i ON rc.renewal_insurance_file_cnt = i.insurance_file_cnt
	INNER JOIN Insurance_File_type ift ON i.insurance_file_type_id = ift.insurance_file_type_id
	WHERE rc.renewal_status_type_id IN (SELECT renewal_status_type_id FROM Renewal_Status_Type WHERE code = 'LAPSECONF' OR code = 'RENEWCONF')
	AND @effective_date >=
        DateAdd(d,
            CASE
                WHEN (SELECT IsNumeric(confirm_day_num) FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id) = 1 THEN
                    (SELECT -confirm_day_num FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id)
                WHEN (IsNumeric(rsr.confirm_day_num)) = 1 THEN
                    -rsr.confirm_day_num
                ELSE
                    (SELECT IsNull(-confirm_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
                END
        , rc.renewal_date)
	AND IsNull(rc.suspension_level, 0) = 0
	AND NOT EXISTS
	(
		SELECT NULL
		FROM insurance_file
		WHERE insurance_folder_cnt = rc.insurance_folder_cnt
		AND insurance_file_type_id = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'MTAPERMCAN')
	)
END ELSE BEGIN
	--Insurance folder passed in
	SELECT 
		rc.insurance_folder_cnt,
		rc.gis_scheme_id,
		rs.code,
		rc.renewal_gis_scheme_id,
		rc.renewal_insurance_file_cnt,
		rc.product_id,
		rc.renewal_date,
		rc.party_cnt,
		rc.risk_code_id,
		rc.gis_data_model_id,
		IsNull(rc.renewal_edi_audit_id, 0),
		g.code,
		ift.code,
		gb.gis_business_type_id,
		rc.old_insurance_file_cnt,
		gbt.code
    FROM Renewal_Control rc
    INNER JOIN Renewal_Status_Type rs ON rs.renewal_status_type_id = rc.renewal_status_type_id
    INNER JOIN GIS_Data_Model g ON g.gis_data_model_id = rc.gis_data_model_id
    INNER JOIN GIS_Data_Model_Business gb ON g.gis_data_model_id = gb.gis_data_model_id
    INNER JOIN GIS_Business_Type gbt ON gb.gis_business_type_id = gbt.gis_business_type_id
    INNER JOIN Insurance_File i ON rc.renewal_insurance_file_cnt = i.insurance_file_cnt
    INNER JOIN Insurance_File_Type ift ON i.insurance_file_type_id = ift.insurance_file_type_id
    WHERE rc.insurance_folder_cnt = @insurance_folder_cnt
    AND IsNull(rc.suspension_level, 0) = 0
END

GO


