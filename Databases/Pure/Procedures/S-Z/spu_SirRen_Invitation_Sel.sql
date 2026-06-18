SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Invitation_Sel'
GO


CREATE PROCEDURE spu_SirRen_Invitation_Sel
    @effective_date DATETIME,
    @insurance_folder_cnt INT = NULL,
    @Source_id INT = NULL,
    @insurer_mode INT = NULL,
    @default_product VARCHAR(20) = NULL
AS

IF @source_id IS NULL
BEGIN
    SELECT @source_id = 0
END

IF @insurer_mode IS NULL
BEGIN
    SELECT @insurer_mode = 0
END

IF (@insurance_folder_cnt IS NULL)
BEGIN
    /* No Insurance_Folder */
    DECLARE @daynum INT

    DECLARE @iRiskGroupId INT
    SELECT @default_product = ISNULL(@default_product,'')

    IF @default_product <> ''
    BEGIN
        SELECT @iRiskGroupId = (SELECT risk_group_id FROM risk_group WHERE code = @default_product)
    END

    SELECT
        rc.insurance_folder_cnt,
        rc.gis_scheme_id,
            rc.renewal_gis_scheme_id,
            rc.renewal_insurance_file_cnt,
            rc.product_id,
        rc.renewal_date,
        rc.party_cnt,
        rc.risk_code_id,
        rc.gis_data_model_id,
        ISNULL(rc.renewal_edi_audit_id, 0),
        g.code,
        isnull(rc.offer_alt, 0),
        s.scheme_type_flags,
        gb.code,
        PA.agent_cnt
    FROM renewal_control rc
    INNER JOIN Risk_Code rcd on rcd.Risk_Code_Id = rc.risk_code_id /* AK 281101 - we need to fetch timings for the group */
    INNER JOIN Risk_Group rg on rcd.risk_group_id = rg.risk_group_id /*DC 260902 we may be checking for specific risk group (default product) */
    INNER JOIN Renewal_Settings rsr ON rsr.product_id = rcd.risk_group_id
    INNER JOIN gis_data_model g
        ON g.gis_data_model_id = rc.gis_data_model_id
    INNER JOIN gis_scheme s
        ON s.gis_scheme_id = rc.renewal_gis_scheme_id
    INNER JOIN insurance_folder i ON rc.insurance_folder_cnt = i.insurance_folder_cnt
    INNER JOIN source sr ON sr.source_id = i.source_id
    INNER JOIN insurance_file inf ON rc.renewal_insurance_file_cnt = inf.insurance_file_cnt
    INNER JOIN insurance_file inf2 ON rc.old_insurance_file_cnt = inf2.insurance_file_cnt
    LEFT JOIN Gis_Business_Type gb ON gb.gis_business_type_id = s.gis_business_type_id
    LEFT JOIN   Policy_Agents PA on RC.renewal_insurance_file_cnt = PA.insurance_file_cnt
    JOIN policy_type pt
        ON pt.policy_type_id = inf.policy_type_id
        AND pt.code = 'SCHEMES'
    WHERE rc.renewal_status_type_id = (SELECT renewal_status_type_id FROM renewal_status_type WHERE code = 'RENQUOTED')
    AND @effective_date >= 
        DATEADD(d,
            CASE
            WHEN (SELECT ISNULL(invite_day_num, 0) FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id) <> 0 THEN
                (SELECT -invite_day_num FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id)
            WHEN (ISNULL(rsr.invite_day_num, 0)) <> 0 THEN
                -rsr.invite_day_num
            ELSE
                (SELECT -ISNULL(invite_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
            END
        , rc.renewal_date)
    AND ISNULL(rc.suspension_level, 0) = 0
    AND inf2.insurance_file_status_id IS NULL --ignore cancelled policies etc  PN20149
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
    AND
    (
        @default_product = ''
        OR
        (
            @default_product <> ''
            AND 
            rg.risk_group_id = @iRiskGroupId
        )
    )
END
ELSE
BEGIN
    /* Insurance folder passed in */
    SELECT
        rc.insurance_folder_cnt,
        rc.gis_scheme_id,
        rc.renewal_gis_scheme_id,
        rc.renewal_insurance_file_cnt,
        rc.product_id,
        rc.renewal_date,
        rc.party_cnt,
        rc.risk_code_id,
        rc.gis_data_model_id,
        ISNULL(rc.renewal_edi_audit_id, 0),
        g.code,
        isnull(rc.offer_alt, 0),
        s.scheme_type_flags,
        gb.code,
        PA.agent_cnt
    FROM renewal_control rc
    INNER JOIN gis_data_model g
    ON g.gis_data_model_id = rc.gis_data_model_id
    INNER JOIN gis_scheme s
    ON s.gis_scheme_id = rc.renewal_gis_scheme_id
    INNER JOIN insurance_file inf2 ON rc.old_insurance_file_cnt = inf2.insurance_file_cnt
    LEFT JOIN Gis_Business_Type gb ON gb.gis_business_type_id = s.gis_business_type_id
    LEFT JOIN   Policy_Agents PA on RC.renewal_insurance_file_cnt = PA.insurance_file_cnt
    WHERE rc.insurance_folder_cnt = @insurance_folder_cnt
    AND ISNULL(rc.suspension_level, 0) = 0
    AND inf2.insurance_file_status_id IS NULL --ignore cancelled policies etc  PN20149
END

GO


