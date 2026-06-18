SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Quote_Broker_Sel'
GO


CREATE PROCEDURE spu_SirRen_Quote_Broker_Sel
    @effective_date DATETIME,
    @insurance_folder_cnt INT = NULL,
    @source_id INT = NULL,
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
    /* Select all the records */
    DECLARE @daynum INT

    DECLARE @iRiskGroupId INT
    SELECT @default_product = ISNULL(@default_product,'')

    IF @default_product <> ''
    BEGIN
        SELECT @iRiskGroupId = (SELECT risk_group_id FROM risk_group WHERE code = @default_product)
    END

    SELECT
        rc.insurance_folder_cnt,
        rc.party_cnt,
        rc.renewal_date,
        rc.risk_code_id,
        rc.gis_data_model_id,
        g.code,
        rc.gis_scheme_id,
        rc.product_id,
        rc.renewal_insurance_file_cnt,
        rc.renewal_gis_scheme_id,
        gb.code
    FROM Renewal_Control rc
    INNER JOIN Risk_Code rcd on rcd.Risk_Code_Id = rc.risk_code_id /* AK 281101 - we need to fetch timings for the group */
    INNER JOIN Risk_Group rg on rcd.risk_group_id = rg.risk_group_id /*DC 260902 we may be checking for specific risk group (default product) */
    INNER JOIN Renewal_Settings rsr ON rsr.product_id = rcd.risk_group_id
    INNER JOIN Gis_Data_Model g ON g.gis_data_model_id = rc.gis_data_model_id
    INNER JOIN insurance_folder i ON rc.insurance_folder_cnt = i.insurance_folder_cnt
    INNER JOIN source sr ON sr.source_id = i.source_id
    INNER JOIN insurance_file inf ON rc.renewal_insurance_file_cnt = inf.insurance_file_cnt
    INNER JOIN insurance_file inf2 ON rc.old_insurance_file_cnt = inf2.insurance_file_cnt 
    LEFT JOIN GIS_Scheme s ON rc.gis_scheme_id = s.gis_scheme_id
    LEFT JOIN Gis_Business_Type gb ON gb.gis_business_type_id = s.gis_business_type_id
    JOIN policy_type pt
        ON pt.policy_type_id = inf.policy_type_id
        AND pt.code = 'SCHEMES'
    WHERE @effective_date >=
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
    AND rc.renewal_status_type_id = 
    (
        SELECT renewal_status_type_id
        FROM renewal_status_type
        WHERE code = 'RENSEL'
    )
    AND rc.renewal_edi_audit_id IS NULL
    AND ISNULL(rc.suspension_level, 0) = 0
    AND isnull(s.is_insurer_lead, 0) <> 1
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
    /* Just select the one record. */
    SELECT
        rc.insurance_folder_cnt,
        rc.party_cnt,
        rc.renewal_date,
        rc.risk_code_id,
        rc.gis_data_model_id,
        g.code,
        rc.gis_scheme_id,
        rc.product_id,
        rc.renewal_insurance_file_cnt,
        rc.renewal_gis_scheme_id,
        gb.code
    FROM Renewal_Control rc
    INNER JOIN Gis_Data_Model g
    ON g.gis_data_model_id = rc.gis_data_model_id
    INNER JOIN insurance_file inf2 ON rc.old_insurance_file_cnt = inf2.insurance_file_cnt
    LEFT JOIN GIS_Scheme s
    ON rc.gis_scheme_id = s.gis_scheme_id
    AND isnull(s.is_insurer_lead, 0) <> 1
    LEFT JOIN Gis_Business_Type gb
    ON gb.gis_business_type_id = s.gis_business_type_id
    WHERE rc.insurance_folder_cnt = @insurance_folder_cnt
    AND rc.renewal_edi_audit_id IS NULL
    AND ISNULL(rc.suspension_level, 0) = 0
    AND inf2.insurance_file_status_id IS NULL --ignore cancelled policies etc  PN20149
END

GO


