SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Quote_Insurer_Sel'
GO


CREATE PROCEDURE spu_SirRen_Quote_Insurer_Sel
    @effective_date datetime,
    @insurance_folder_cnt int = NULL,
    @source_id int = NULL,
    @insurer_mode int = NULL
AS

DECLARE @rensel_id int

/* Get the renewal selection id */
SELECT @rensel_id = renewal_status_type_id FROM renewal_status_type WHERE code = 'RENSEL'

/* Update the renewal_edi_audit_id column */
EXECUTE spu_SIRRen_UpdateRenControlAuditID

IF (@insurance_folder_cnt IS NULL)
BEGIN
	/* No insurance_folder so we want them all. All I say! */
	DECLARE @daynum int

	SELECT
		'is_replacement' =
			CASE
			WHEN rc.renewal_status_type_id = @rensel_id THEN 0
			WHEN rc.renewal_status_type_id > @rensel_id THEN 1
			ELSE NULL -- This shouldnt happen because of the WHERE clause
			END,
		rea.renewal_edi_audit_id,
		rea.insurance_folder_cnt,
		rc.gis_scheme_id,
		rc.renewal_gis_scheme_id,
		rc.renewal_insurance_file_cnt,
		rc.product_id,
		rc.renewal_date,
		rc.party_cnt,
		rc.risk_code_id,
		rc.gis_data_model_id,
		g.code
	FROM renewal_edi_audit rea
	INNER JOIN renewal_control rc
	ON rea.renewal_edi_audit_id = rc.renewal_edi_audit_id
	INNER JOIN Gis_Data_Model g
	ON g.gis_data_model_id = rc.gis_data_model_id
	INNER JOIN Risk_Code rcd
	ON rcd.Risk_Code_Id = rc.risk_code_id /* AK 281101 - we need to fetch timings for the group */
    INNER JOIN Renewal_Settings rsr
    ON rsr.product_id = rcd.risk_group_id
	INNER JOIN insurance_folder i ON rc.insurance_folder_cnt = i.insurance_folder_cnt
	INNER JOIN source sr ON sr.source_id = i.source_id
	INNER JOIN insurance_file inf ON rc.renewal_insurance_file_cnt = inf.insurance_file_cnt
        INNER JOIN insurance_file inf2 ON rc.old_insurance_file_cnt = inf2.insurance_file_cnt 
	LEFT JOIN GIS_Scheme s
	ON rc.gis_scheme_id = s.gis_scheme_id
	AND isnull(s.is_insurer_lead, 0) = 1
	WHERE rea.renewal_edi_status = 0
	AND rc.renewal_status_type_id >= @rensel_id
        AND inf2.insurance_file_status_id IS NULL --ignore cancelled policies etc  PN20149
	AND @effective_date >=
		DATEADD(d,
			CASE
			WHEN (SELECT ISNULL(quote_day_num, 0) FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id) <> 0 THEN
				(SELECT -quote_day_num FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id)
			WHEN (ISNULL(rsr.quote_day_num, 0)) <> 0 THEN
				-rsr.quote_day_num
			ELSE
				(SELECT -ISNULL(quote_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
			END
		, rc.renewal_date)
	AND ISNULL(rc.suspension_level, 0) = 0
	AND NOT EXISTS
	(
		SELECT NULL
		FROM insurance_file
		WHERE insurance_folder_cnt = rc.insurance_folder_cnt
		AND insurance_file_type_id = (SELECT insurance_file_type_id FROM Insurance_File_Type WHERE code = 'MTAPERMCAN')
	) 

	AND 
	(
	@source_id = 0
	    OR
	    (
		@source_id <> 0
		AND
		ISNULL(i.source_id, 0) = @source_id
	    )
	)   

	AND
	(
	    (
		@insurer_mode = 0
		AND
		ISNULL(sr.underwriting_branch_ind, 0) = 0
	    )
	      OR
	    (
		@insurer_mode = 0
		AND
		ISNULL(sr.underwriting_branch_ind, 0) = 1
		AND
		ISNULL(inf.alternate_reference, '') = ''
	    )
	      OR
	    (
		@insurer_mode = 1 
		AND
		ISNULL(sr.underwriting_branch_ind, 0) = 1
		AND
		ISNULL(inf.alternate_reference, '') <> ''
	    )
	)
END
ELSE
BEGIN
	/* We have the insurance_folder_cnt so select that record */
	SELECT
		'is_replacement' =
			CASE
			WHEN rc.renewal_status_type_id = @rensel_id THEN 0
			WHEN rc.renewal_status_type_id > @rensel_id THEN 1
			ELSE NULL
			END,
		rea.renewal_edi_audit_id,
		rea.insurance_folder_cnt,
		rc.gis_scheme_id,
		rc.renewal_gis_scheme_id,
		rc.renewal_insurance_file_cnt,
		rc.product_id,
		rc.renewal_date,
		rc.party_cnt,
		rc.risk_code_id,
		rc.gis_data_model_id,
		g.code
	FROM renewal_edi_audit rea
	INNER JOIN renewal_control rc
	ON rea.renewal_edi_audit_id = rc.renewal_edi_audit_id
	INNER JOIN Gis_Data_Model g
	ON g.gis_data_model_id = rc.gis_data_model_id
        INNER JOIN insurance_file inf2 ON rc.old_insurance_file_cnt = inf2.insurance_file_cnt 
	LEFT JOIN GIS_Scheme s
	ON rc.gis_scheme_id = s.gis_scheme_id
	AND isnull(s.is_insurer_lead, 0) = 1
	WHERE rc.insurance_folder_cnt = @insurance_folder_cnt
	AND ISNULL(rc.suspension_level, 0) = 0
        AND inf2.insurance_file_status_id IS NULL --ignore cancelled policies etc  PN20149
END

GO


