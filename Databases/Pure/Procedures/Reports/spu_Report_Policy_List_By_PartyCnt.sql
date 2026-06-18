SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
/*MKR 27/09/04 PN13345 Split out quote policies from Live based on Type ... took help from changes made 
in spu_report_policy_list for same issue */

EXECUTE DDLDropProcedure 'spu_Report_Policy_List_By_PartyCnt'
GO
CREATE PROCEDURE spu_Report_Policy_List_By_PartyCnt
    @party_cnt int
AS

DECLARE
    @insurance_file_status_id int,
    @policy_status varchar(20),
    @account_handler_cnt int,
    @account_handler varchar(20)

SELECT
    S.description branch,
    PCli.shortname,
    PCli.resolved_name,
    --I.insurance_ref,
    (
        CASE ISNULL(
                (
                    SELECT source.underwriting_branch_ind 
                    FROM source 
                    WHERE i.source_id = source.source_id 
                    AND i.alternate_reference IS NOT NULL
                ),'') 
            WHEN '' THEN i.insurance_ref
            ELSE i.alternate_reference 
        END
    ) 'insurance_ref',
    PIns.name insurer,
    ISNULL(
    ( SELECT ISNULL(PAcc.resolved_name, '')
        FROM Party PAcc
        WHERE PAcc.party_cnt = I.account_handler_cnt
    )
    , '') account_handler,
    I.cover_start_date,
    I.expiry_date,
    I.renewal_date,
    ISNULL(I.this_premium, 0) this_premium,
    (
        SELECT ISNULL(SUM(coinsurer_commission_amount), I.commission_amount)
        FROM policy_coinsurers 
        WHERE insurance_file_cnt = I.insurance_file_cnt
    ) commission_amount,
    ISNULL(
    ( SELECT ISNULL(CRG.caption, '')
        FROM PMCaption CRG,
            Risk_Code RC
        WHERE RC.risk_code_id = I.risk_code_id
        AND CRG.caption_id = RC.caption_id
    )
    , '') risk,
    ISNULL(
    ( SELECT ISNULL(CAC.caption, '')
        FROM PMCaption CAC,
            Analysis_Code AC
        WHERE AC.analysis_code_id = I.analysis_code_id
        AND CAC.caption_id = AC.caption_id
    )
    , '') analysis,
    (
        CASE IFT.insurance_file_type_id
            WHEN 1 THEN IFT.description
            ELSE ISNULL(IFS.Description, 'Live')
        END 
    ) 'policy_status',
    C.currency_id,
    C.code currency_code
FROM Insurance_Folder F
JOIN Insurance_File I
    ON I.insurance_folder_cnt = F.insurance_folder_cnt
JOIN Source S
    ON S.source_id = I.source_id
JOIN Insurance_File_Type IFT
    ON IFT.insurance_file_type_id = I.insurance_file_type_id
LEFT JOIN Insurance_File_Status IFS
    ON IFS.insurance_file_status_id = I.insurance_file_status_id    
LEFT JOIN Party PIns
    ON PIns.party_cnt = I.lead_insurer_cnt
JOIN Party PCli
    ON PCli.party_cnt = F.insurance_holder_cnt
JOIN currency C
    ON C.currency_id = I.currency_id
WHERE PCli.party_cnt = @party_cnt
AND PCli.is_deleted = 0
AND ISNULL(I.policy_ignore, 0) = 0
AND I.policy_version = 
    (
        SELECT MAX(I2.policy_version)
        FROM insurance_file I2
        JOIN insurance_file_type IFT2
            ON IFT2.insurance_file_type_id = I2.insurance_file_type_id
        WHERE I2.insurance_ref = I.insurance_ref 
        AND IFT2.code IN ('POLICY','RENEWAL','MTA PERM','MTA TEMP','MTAPERMCAN','QUOTE')
    )

ORDER BY C.currency_id, policy_status

GO

