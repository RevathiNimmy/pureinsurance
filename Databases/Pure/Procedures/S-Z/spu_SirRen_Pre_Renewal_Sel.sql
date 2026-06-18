SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Pre_Renewal_Sel'
GO


CREATE PROCEDURE spu_SirRen_Pre_Renewal_Sel
    @effective_date DATETIME,
    @source_id INT,
    @insurer_mode INT,
    @insurance_folder_cnt INT,
    @default_product VARCHAR(20) = NULL
AS

DECLARE @daynum INT
DECLARE @iRiskGroupId INT


IF @source_id = 0
BEGIN
    SELECT @source_id = NULL
END

IF @insurance_folder_cnt = 0
BEGIN
    SELECT @insurance_folder_cnt = NULL
END
        
SELECT @default_product = ISNULL(@default_product,'')

IF @default_product <> ''
BEGIN
    SELECT 
        @iRiskGroupId = risk_group_id 
    FROM risk_group 
    WHERE code = @default_product
END

SELECT 
    p.party_cnt,
    i.insurance_folder_cnt,
    i.renewal_date,
    i.risk_code_id,
    gpl.gis_scheme_id,
    i.product_id,
    i.insurance_file_cnt,
    gdm.code,
    gdm.gis_data_model_id,
    ISNULL(pa.agent_cnt, 0) 'pa_agent_cnt'
FROM insurance_file i
JOIN insurance_file_type ift
    ON ift.insurance_file_type_id = i.insurance_file_type_id
JOIN risk_code rc 
    ON rc.risk_code_id = i.risk_code_id
JOIN risk_group rg 
    ON rg.risk_group_id = rc.risk_group_id
JOIN party p 
    ON p.party_cnt = i.insured_cnt
JOIN gis_policy_link gpl 
    ON gpl.insurance_file_cnt = i.insurance_file_cnt 
    AND gpl.gis_scheme_id IS NOT NULL
JOIN gis_scheme gs 
    ON gs.gis_scheme_id = gpl.gis_scheme_id
JOIN gis_data_model gdm 
    ON gdm.gis_data_model_id = gpl.gis_data_model_id
    AND (
            gdm.code <> 'GIITruck' 
            OR 
            (
                gdm.code = 'GIITruck' 
                AND 
                gs.is_insurer_lead = 1
            )
        )
JOIN source s 
    ON s.source_id = i.source_id
LEFT JOIN policy_agents pa 
    ON pa.insurance_file_cnt = i.insurance_file_cnt 
WHERE ift.code IN ('POLICY', 'MTA PERM')
AND i.insurance_file_status_id IS NULL
AND NOT EXISTS
    (
        SELECT 
            NULL
        FROM renewal_control
        WHERE insurance_folder_cnt = i.insurance_folder_cnt
    )

AND @effective_date >= 
    DATEADD(d,
        CASE
            WHEN (SELECT ISNULL(pre_selection_day_num, 0) FROM gis_scheme WHERE gis_scheme_id = gpl.gis_scheme_id) <> 0 THEN
                (SELECT -pre_selection_day_num FROM gis_scheme WHERE gis_scheme_id = gpl.gis_scheme_id)
            WHEN (SELECT ISNULL(pre_selection_day_num, 0) FROM renewal_settings WHERE product_id = rc.Risk_Group_Id) <> 0 THEN
                (SELECT -pre_selection_day_num FROM renewal_settings WHERE product_id = rc.Risk_Group_Id)
            ELSE
                (SELECT -ISNULL(pre_selection_day_num, 0) FROM renewal_settings WHERE product_id = -1)
        END
    , i.renewal_date)
AND i.insurance_file_cnt =
    ( 
        SELECT 
            MAX(i2.insurance_file_cnt)
        FROM insurance_file i2
        JOIN insurance_file_type ift2
            ON ift2.insurance_file_type_id = i2.insurance_file_type_id
        WHERE i2.insurance_folder_cnt = i.insurance_folder_cnt
        AND ift2.code IN ('POLICY', 'MTA PERM')
        AND i2.insurance_file_status_id IS NULL
    )
AND NOT EXISTS
    (
        SELECT 
            NULL
        FROM insurance_file i2
        JOIN insurance_file_type ift2
            ON ift2.insurance_file_type_id = i2.insurance_file_type_id
        WHERE i2.insurance_folder_cnt = i.insurance_folder_cnt
        AND i2.policy_version >= i.policy_version
        AND (
                ift2.code = 'MTAPERMCAN' /*Permanent MTA Cancellation*/
                OR
                (
                    ift2.code = 'LAP' /*Lapsed policy with valid reason*/
                    AND 
                    ISNULL(i2.lapsed_reason_id, 0) <> 0
                )
            )
    )
AND i.source_id = ISNULL(@source_id, i.source_id)
AND (
        (
            @insurer_mode = 0
            AND
            ISNULL(s.underwriting_branch_ind, 0) = 0
        )
        OR
        (
            @insurer_mode = 0
            AND
            ISNULL(s.underwriting_branch_ind, 0) = 1
            AND
            ISNULL(i.alternate_reference, '') = ''
        )
        OR
        (
            @insurer_mode = 1
            AND
            ISNULL(s.underwriting_branch_ind, 0) = 1
            AND
            ISNULL(i.alternate_reference, '') <> ''
        )
    )
AND i.Insurance_folder_cnt = ISNULL(@insurance_folder_cnt, i.Insurance_folder_cnt)
AND (
        @default_product = ''
        OR
        (
            @default_product <> ''
            AND 
            rg.risk_group_id = @iRiskGroupId
        )
    )
GROUP BY 
    p.party_cnt,
    i.insurance_folder_cnt,
    i.renewal_date,
    i.risk_code_id,
    gpl.gis_scheme_id,
    i.product_id,
    i.insurance_file_cnt,
    gdm.code,
    gdm.gis_data_model_id,
    pa.agent_cnt


GO


