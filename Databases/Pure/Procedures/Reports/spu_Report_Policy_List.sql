/*This stored procedure is used by the following reports:*/
/*Policy_List*/
/*Export_Policy_List*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Policy_List'
GO

CREATE PROCEDURE spu_Report_Policy_List
    @branch_id INT,
    @report_type VARCHAR(20),
    @agent VARCHAR(20)

AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF ISNULL(@report_type,'') = ''
BEGIN
    SELECT @report_type = 'Standard'
END

IF ISNULL(@agent,'') = ''
BEGIN
    SELECT @agent = 'ALL'
END

CREATE TABLE #Policy
(
    insurance_file_cnt INT,
    policy_status VARCHAR(255)
)
CREATE INDEX temp__policy__insurance_file_cnt ON #Policy (insurance_file_cnt)

INSERT INTO #Policy
SELECT
    I.insurance_file_cnt,
    CASE IFT.insurance_file_type_id
        WHEN 1 THEN IFT.description
        ELSE ISNULL(IFS.Description, 'Live')
    END
FROM Insurance_Folder F
JOIN Insurance_File I
    ON I.insurance_folder_cnt = F.insurance_folder_cnt
JOIN Party PC
    ON PC.party_cnt = F.insurance_holder_cnt
JOIN Source S
    ON S.source_id = I.source_id
JOIN Insurance_File_Type IFT
    ON IFT.insurance_file_type_id = I.insurance_file_type_id
LEFT JOIN Insurance_File_Status IFS
    ON IFS.insurance_file_status_id = I.insurance_file_status_id
WHERE I.policy_version =
    (
        SELECT 
            MAX(I2.policy_version)
        FROM insurance_file I2
        JOIN insurance_file_type IFT2
            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE')
    )
AND PC.is_deleted = 0
AND ISNULL(I.policy_ignore, 0) = 0
AND S.source_id = ISNULL(@branch_id,S.source_id)

IF @report_type = 'Standard' AND @agent='ALL'
BEGIN
    SELECT
        /*Company_Details*/
        S.code 'branch_code',
        S.description 'branch_desc',
    
        /*Client Details*/
        PC.shortname 'client_code',
        PC.resolved_name 'client_name',
        ISNULL(A.description, '') 'area_desc',
        ISNULL(PAE.shortname, '') 'acc_exec_code',
        ISNULL(PAE.resolved_name, '') 'acc_exec_name',
        ISNULL(PCC.party_business_id, '') 'client_business',
    
        /*Insurer Details*/
        PI.shortname 'insurer_code',
        PI.resolved_name 'insurer_name',
    
        /*Account Handler Details*/
        ISNULL(PA.shortname, '') 'acc_hand_code',
        ISNULL(PA.resolved_name, '') 'acc_hand_name',
    
        /*Insurance Details*/
        I.insurance_ref,
        I.cover_start_date,
        I.expiry_date,
        I.renewal_date,
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    ISNULL(I.this_premium, 0)
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT SUM(ISNULL(I2.this_premium, 0))
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'this_premium',
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    ISNULL(I.commission_amount, 0)
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT SUM(ISNULL(I2.commission_amount, 0))
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'commission_amount', 
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    (
                        SELECT ISNULL(SUM(coinsurer_commission_amount),0)
                        FROM policy_coinsurers
                        WHERE insurance_file_cnt = I.insurance_file_cnt
                    )
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT ISNULL(SUM(ISNULL(PC2.coinsurer_commission_amount, 0)),0)
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        JOIN policy_coinsurers PC2
                            ON PC2.insurance_file_cnt = I2.insurance_file_cnt
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'coinsurer_commission_amount',
        ISNULL(AC.description, '') 'analysis_code_desc',
        X.policy_status,
        ISNULL(commission_percentage,0) 'commission_percentage',
        ISNULL(annual_premium,0) 'annual_premium',
        ISNULL(BT.description,'') 'business_type_desc',
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    ISNULL(I.net_premium, 0)
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT SUM(ISNULL(I2.net_premium, 0))
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'net_premium', 
    
        /*Risk Details*/
        RC.description 'risk_code_desc',
        RG.description 'risk_group_desc',
    
        /*Currency Details*/
        C.currency_id,
        C.code 'currency_code',
        C.description 'currency_desc',
    
        /*Agent Details*/
        '' AS 'agent_code',
        '' AS 'agent_name',
        0 'agent_commission',
        0 'agent_percentage',
        0 'agent_payment',
        0 'agent_rate_override',
        '' AS 'agent_apply_to',
        ''AS 'agent_type',
    
        /*Group By Details*/
        (
            CASE @report_type
                WHEN 'Standard' THEN X.policy_status
                ELSE S.code /*branch_code*/
            END
        ) 'group_by_1',
        (
                CASE @report_type
                    WHEN 'Standard' THEN ''
                    ELSE X.policy_status
                END
        ) 'group_by_2',
       '' AS 'group_by_3'
    
    FROM #Policy X
    JOIN Insurance_File I
        ON I.insurance_file_cnt = X.insurance_file_cnt
    JOIN Insurance_Folder F
        ON F.insurance_folder_cnt = I.insurance_folder_cnt
    JOIN Source S
        ON S.source_id = I.source_id
    JOIN Policy_Type PT
        ON PT.policy_type_id = I.policy_type_id
    LEFT JOIN Party PI
        ON PI.party_cnt = I.lead_insurer_cnt
    JOIN Party PC
        ON PC.party_cnt = F.insurance_holder_cnt
    LEFT JOIN Party_Corporate_Client PCC
        ON PCC.party_cnt = PC.party_cnt
    LEFT JOIN Area A
        ON A.area_id = PC.area_id
    LEFT JOIN Party PAE
        ON PAE.party_cnt = PC.consultant_cnt
    LEFT JOIN Party PA
        ON PA.party_cnt = I.account_handler_cnt
    JOIN Risk_Code RC
        ON RC.risk_code_id = I.risk_code_id
    JOIN Risk_Group RG
        ON RG.risk_group_id = RC.risk_group_id
    JOIN currency C
        ON C.currency_id = I.currency_id
    LEFT JOIN Analysis_Code AC
        ON AC.analysis_code_id = I.analysis_code_id
    LEFT JOIN Business_Type BT
        ON BT.business_type_id = I.business_type_id
    ORDER BY
        C.currency_id,
        policy_status,
        PC.shortname,
        I.insurance_file_cnt

END
ELSE
BEGIN
    SELECT
        /*Company_Details*/
        S.code 'branch_code',
        S.description 'branch_desc',
    
        /*Client Details*/
        PC.shortname 'client_code',
        PC.resolved_name 'client_name',
        ISNULL(A.description, '') 'area_desc',
        ISNULL(PAE.shortname, '') 'acc_exec_code',
        ISNULL(PAE.resolved_name, '') 'acc_exec_name',
        ISNULL(PCC.party_business_id, '') 'client_business',
    
        /*Insurer Details*/
        PI.shortname 'insurer_code',
        PI.resolved_name 'insurer_name',
    
        /*Account Handler Details*/
        ISNULL(PA.shortname, '') 'acc_hand_code',
        ISNULL(PA.resolved_name, '') 'acc_hand_name',
    
        /*Insurance Details*/
        I.insurance_ref,
        I.cover_start_date,
        I.expiry_date,
        I.renewal_date,
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    ISNULL(I.this_premium, 0)
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT SUM(ISNULL(I2.this_premium, 0))
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'this_premium',
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    ISNULL(I.commission_amount, 0)
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT SUM(ISNULL(I2.commission_amount, 0))
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'commission_amount', 
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    (
                        SELECT ISNULL(SUM(coinsurer_commission_amount),0)
                        FROM policy_coinsurers
                        WHERE insurance_file_cnt = I.insurance_file_cnt
                    )
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT ISNULL(SUM(ISNULL(PC2.coinsurer_commission_amount, 0)),0)
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        JOIN policy_coinsurers PC2
                            ON PC2.insurance_file_cnt = I2.insurance_file_cnt
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'coinsurer_commission_amount',
        ISNULL(AC.description, '') 'analysis_code_desc',
        X.policy_status,
        ISNULL(commission_percentage,0) 'commission_percentage',
        ISNULL(annual_premium,0) 'annual_premium',
        ISNULL(BT.description,'') 'business_type_desc',
        (
            CASE
                WHEN PT.code = 'GENERAL' THEN 
                    ISNULL(I.net_premium, 0)
                ELSE /*SCHEMES & GII*/
                    (
                        SELECT SUM(ISNULL(I2.net_premium, 0))
                        FROM insurance_file I2
                        JOIN insurance_file_type IFT2
                            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
                        WHERE I2.insurance_folder_cnt = I.insurance_folder_cnt
                        AND IFT2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN', 'QUOTE', 'MTA TEMP')
                        AND ISNULL(I2.insurance_file_status_id, 0) IN (0, 2, 4) /*Live, Lapsed or Replaced*/
                        AND I2.policy_version >=
                            (
                                SELECT 
                                    ISNULL(MAX(I3.policy_version), 1)
                                FROM insurance_file I3
                                JOIN insurance_file_type IFT3
                                    ON IFT3.insurance_file_type_id = I3.insurance_file_type_id
                                WHERE I3.insurance_folder_cnt = I2.insurance_folder_cnt
                                AND IFT3.code IN ('POLICY', 'RENEWAL', 'QUOTE')
                            )
                    )
            END 
        ) 'net_premium', 
    
        /*Risk Details*/
        RC.description 'risk_code_desc',
        RG.description 'risk_group_desc',
    
        /*Currency Details*/
        C.currency_id,
        C.code 'currency_code',
        C.description 'currency_desc',
    
        /*Agent Details*/
        ISNULL(PAg.shortname, '') 'agent_code',
        ISNULL(PAg.resolved_name, '') 'agent_name',
        ISNULL(PolAg.agent_commission_value,0) 'agent_commission',
        ISNULL(PolAg.agent_commission_percentage,0) 'agent_percentage',
        ISNULL(PolAg.agent_commission_amount,0) 'agent_payment',
        ISNULL(PolAg.override_rate_table,0) 'agent_rate_override',
        (
            CASE ISNULL(PolAg.apply_perc_to_prem_or_comm,0)
                WHEN 1 THEN 'C'
                WHEN 0 THEN 'P'
            END
        ) 'agent_apply_to',
        ISNULL(PAT.description,'') 'agent_type',
    
        /*Group By Details*/
        (
            CASE @report_type
                WHEN 'Standard' THEN X.policy_status
                ELSE S.code /*branch_code*/
            END
        ) 'group_by_1',
        (
                CASE @report_type
                    WHEN 'Standard' THEN ''
                    ELSE X.policy_status
                END
        ) 'group_by_2',
        (
                CASE @report_type
                    WHEN 'Standard' THEN ''
                    ELSE ISNULL(PAg.shortname, '') /*agent_code*/
                END
        ) 'group_by_3'
    
    FROM #Policy X
    JOIN Insurance_File I
        ON I.insurance_file_cnt = X.insurance_file_cnt
    JOIN Insurance_Folder F
        ON F.insurance_folder_cnt = I.insurance_folder_cnt
    JOIN Source S
        ON S.source_id = I.source_id
    JOIN Policy_Type PT
        ON PT.policy_type_id = I.policy_type_id
    LEFT JOIN Party PI
        ON PI.party_cnt = I.lead_insurer_cnt
    JOIN Party PC
        ON PC.party_cnt = F.insurance_holder_cnt
    LEFT JOIN Party_Corporate_Client PCC
        ON PCC.party_cnt = PC.party_cnt
    LEFT JOIN Area A
        ON A.area_id = PC.area_id
    LEFT JOIN Party PAE
        ON PAE.party_cnt = PC.consultant_cnt
    LEFT JOIN Party PA
        ON PA.party_cnt = I.account_handler_cnt
    JOIN Risk_Code RC
        ON RC.risk_code_id = I.risk_code_id
    JOIN Risk_Group RG
        ON RG.risk_group_id = RC.risk_group_id
    JOIN currency C
        ON C.currency_id = I.currency_id
    LEFT JOIN Analysis_Code AC
        ON AC.analysis_code_id = I.analysis_code_id
    LEFT JOIN Business_Type BT
        ON BT.business_type_id = I.business_type_id
    LEFT JOIN Policy_Agents PolAg
        JOIN Party PAg
            ON PAg.party_cnt = PolAg.agent_cnt
        JOIN Party_Agent PAAg
            ON PAAg.party_cnt = PAg.party_cnt
        JOIN Party_Agent_type PAT
            ON PAT.party_agent_type_id = PAAg.party_agent_type_id
        ON PolAg.insurance_file_cnt = I.insurance_file_cnt
    ORDER BY
        C.currency_id,
        policy_status,
        PC.shortname,
        I.insurance_file_cnt
END

DROP TABLE #Policy

GO

