SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_auto_ren_db_sel'
GO


CREATE PROCEDURE spu_SIRRen_auto_ren_db_sel
    @effective_date DATETIME,
    @default_product VARCHAR(20) = NULL,
    @insurance_folder_cnt INT = NULL,
    @source_id INT = NULL,
    @insurer_mode INT = NULL

AS

DECLARE @daynum INT

IF @source_id IS NULL
BEGIN
    SELECT @source_id = 0
END

IF @insurer_mode IS NULL
BEGIN
    SELECT @insurer_mode = 0
END

IF @insurance_folder_cnt IS NOT NULL
BEGIN
    SELECT  RC.insurance_folder_cnt,
        RC.gis_scheme_id,
        RC.renewal_gis_scheme_id,
        RC.renewal_insurance_file_cnt,
        RC.product_id,
        RC.renewal_date,
        RC.party_cnt,
        RC.risk_code_id,
        RC.gis_data_model_id,
        ISNULL(RC.renewal_edi_audit_id, 0),
        G.code,
        ISNULL(RC.offer_alt,0),
        S.scheme_type_flags,
        GB.code,
        p.party_cnt,
        ISNULL(pa.agent_cnt, 0) agent_cnt,
        i.insurance_file_cnt old_insurance_file_cnt,
        i.payment_method
    FROM        Renewal_Control RC
    INNER JOIN  Risk_Code RCD ON RCD.risk_code_id = RC.risk_code_id
    INNER JOIN  Risk_Group RG ON RCD.risk_group_id = RG.risk_group_id
    INNER JOIN  Renewal_Settings RSR ON RSR.product_id = RCD.risk_group_id
    INNER JOIN  Gis_Data_Model G ON G.gis_data_model_id = RC.gis_data_model_id
    INNER JOIN  Gis_Scheme S ON S.gis_scheme_id = RC.renewal_gis_scheme_id
        INNER JOIN  Gis_Insurer gi ON s.gis_insurer_id = gi.gis_insurer_id
        INNER JOIN  Party P ON gi.party_cnt = p.party_cnt
    INNER JOIN  Insurance_File i ON rc.old_insurance_file_cnt = i.insurance_file_cnt
    LEFT JOIN   Gis_Business_Type GB ON GB.gis_business_type_id = S.gis_business_type_id
    LEFT JOIN   Policy_Agents PA on RC.renewal_insurance_file_cnt = PA.insurance_file_cnt
    WHERE rc.renewal_status_type_id = 4 -- RENINVITED
    AND rc.insurance_folder_cnt = @insurance_folder_cnt
    AND ISNULL(RC.suspension_level, 0) = 0

END
ELSE
BEGIN
    IF ISNULL(@default_product, '') <> ''
    BEGIN

        DECLARE @iRiskGroupId INT
        DECLARE @MonthStart DATETIME

        SELECT  @iRiskGroupId = (SELECT risk_group_id FROM risk_group WHERE code = @default_product)
        SELECT  @monthstart = LTRIM(STR(DATEPART(yyyy, GETDATE()))) + '-' + LTRIM(STR(DATEPART(mm, GETDATE()))) + '-01'

        SELECT  RC.insurance_folder_cnt,
            RC.gis_scheme_id,
            RC.renewal_gis_scheme_id,
            RC.renewal_insurance_file_cnt,
            RC.product_id,
            RC.renewal_date,
            RC.party_cnt,
            RC.risk_code_id,
            RC.gis_data_model_id,
            ISNULL(RC.renewal_edi_audit_id, 0),
            G.code,
            ISNULL(RC.offer_alt,0),
            S.scheme_type_flags,
            GB.code,
            p.party_cnt,
            ISNULL(pa.agent_cnt, 0) agent_cnt,
            i.insurance_file_cnt old_insurance_file_cnt,
            i.payment_method
        FROM        Renewal_Control RC
        INNER JOIN  Risk_Code RCD ON RCD.risk_code_id = RC.risk_code_id
        INNER JOIN  Risk_Group RG ON RCD.risk_group_id = RG.risk_group_id
        INNER JOIN  Renewal_Settings RSR ON RSR.product_id = RCD.risk_group_id
        INNER JOIN  Gis_Data_Model G ON G.gis_data_model_id = RC.gis_data_model_id
        INNER JOIN  Gis_Scheme S ON S.gis_scheme_id = RC.renewal_gis_scheme_id
        INNER JOIN  Gis_Insurer gi ON s.gis_insurer_id = gi.gis_insurer_id
        INNER JOIN  Insurance_File i ON rc.old_insurance_file_cnt = i.insurance_file_cnt
        INNER JOIN  Party P ON gi.party_cnt = p.party_cnt
        LEFT JOIN   Gis_Business_Type GB ON GB.gis_business_type_id = S.gis_business_type_id
        LEFT JOIN   Policy_Agents PA on RC.renewal_insurance_file_cnt = PA.insurance_file_cnt
        JOIN policy_type pt
            ON pt.policy_type_id = i.policy_type_id
            AND pt.code = 'SCHEMES'
        WHERE       RC.renewal_status_type_id = (SELECT renewal_status_type_id FROM renewal_status_type WHERE code = 'INVITED')
        AND         RC.renewal_date < @monthstart
        AND         RG.risk_group_id = @iRiskGroupId
        AND         ISNULL(RC.suspension_level, 0) = 0
    END
    ELSE
    BEGIN
        SELECT  RC.insurance_folder_cnt,
            RC.gis_scheme_id,
            RC.renewal_gis_scheme_id,
            RC.renewal_insurance_file_cnt,
            RC.product_id,
            RC.renewal_date,
            RC.party_cnt,
            RC.risk_code_id,
            RC.gis_data_model_id,
            ISNULL(RC.renewal_edi_audit_id, 0),
            G.code,
            ISNULL(RC.offer_alt,0),
            S.scheme_type_flags,
            GB.code,
            p.party_cnt,
            ISNULL(pa.agent_cnt, 0) agent_cnt,
            i.insurance_file_cnt old_insurance_file_cnt,
            i.payment_method
        FROM        Renewal_Control RC
        INNER JOIN  Risk_Code RCD ON RCD.risk_code_id = RC.risk_code_id
        INNER JOIN  Risk_Group RG ON RCD.risk_group_id = RG.risk_group_id
        INNER JOIN  Renewal_Settings RSR ON RSR.product_id = RCD.risk_group_id
        INNER JOIN  Gis_Data_Model G ON G.gis_data_model_id = RC.gis_data_model_id
        INNER JOIN  Gis_Scheme S ON S.gis_scheme_id = RC.renewal_gis_scheme_id
            INNER JOIN  Gis_Insurer gi ON s.gis_insurer_id = gi.gis_insurer_id
            INNER JOIN  Party P ON gi.party_cnt = p.party_cnt
        INNER JOIN  Insurance_File i ON rc.old_insurance_file_cnt = i.insurance_file_cnt
            INNER JOIN insurance_folder INSF ON RC.insurance_folder_cnt = INSF.insurance_folder_cnt
        INNER JOIN source sr ON sr.source_id = i.source_id
        INNER JOIN insurance_file inf ON rc.renewal_insurance_file_cnt = inf.insurance_file_cnt
        LEFT JOIN   Gis_Business_Type GB ON GB.gis_business_type_id = S.gis_business_type_id
        LEFT JOIN   Policy_Agents PA on RC.renewal_insurance_file_cnt = PA.insurance_file_cnt
        JOIN (
            -- get the auto debit system option for each branch
            SELECT branch_id, value
            FROM system_options
            WHERE option_number = 203
                ) AS so ON i.source_id = so.branch_id
        JOIN policy_type pt
            ON pt.policy_type_id = i.policy_type_id
            AND pt.code = 'SCHEMES'
                WHERE rc.renewal_status_type_id = ISNULL(so.value, 1) + 3
        AND DATEADD(DAY, -rsr.confirm_day_num, rc.renewal_date) <= @effective_date
        AND ISNULL(RC.suspension_level, 0) = 0
        AND i.policy_type_id = 10 -- schemes policies only...

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
END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

