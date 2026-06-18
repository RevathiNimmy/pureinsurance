SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Policy_List2'
GO


CREATE PROCEDURE spu_Report_Policy_List2
    @language_id int,
    @branch_id int
AS


DECLARE @insurance_file_status_id   int,
    @policy_status          varchar(20),
    @account_handler_cnt        int,
    @account_handler        varchar(20),
    @ilanguageID            int,
    @ibranchID          int

SELECT  @ilanguageID = ISNULL(@language_id, 1)
SELECT  @ibranchID = ISNULL(@branch_id, 0)

IF  @ibranchID = 0
BEGIN
    SELECT  S.description branch,
        PCli.shortname,
        PCli.resolved_name,
        I.insurance_ref,
        PIns.name insurer,
        PAcc.name account_handler,
        I.cover_start_date,
        I.expiry_date,
        I.renewal_date,
        ISNULL(I.this_premium, 0),
        ISNULL(I.commission_amount, 0),
        ISNULL(
        (   SELECT  ISNULL(CRG.caption, '')
            FROM    PMCaption       CRG,
                Risk_Group      RG,
                Risk_Code       RC
            WHERE   RC.risk_code_id = I.risk_code_id
            AND RG.risk_group_id = RC.risk_group_id
            AND CRG.caption_id = RG.caption_id
            AND CRG.language_id = @ilanguageID
        )
        , '') risk,
        ISNULL(
        (   SELECT  ISNULL(CAC.caption, '')
            FROM    PMCaption       CAC,
                Analysis_Code       AC
            WHERE   AC.analysis_code_id = I.analysis_code_id
            AND CAC.caption_id = AC.caption_id
            AND CAC.language_id = @ilanguageID
        )
        , '') analysis,
         ISNULL(
        (   SELECT  ISNULL(CST.caption, 'Live')
            FROM    PMCaption       CST,
                Insurance_File_Status   ST
            WHERE   ST.insurance_file_status_id = I.insurance_file_status_id
            AND CST.caption_id = ST.caption_id
            AND CST.language_id = @ilanguageID
        )
        , '') policy_status

    FROM    Source          S,
        Party           PCli,
        Insurance_File      I,
        Insurance_Folder    F,
        Party           PIns,
        Party           PAcc
    WHERE   PCli.party_cnt = F.insurance_holder_cnt
    AND F.insurance_folder_cnt = I.insurance_folder_cnt
    AND     S.source_id = I.source_id
    AND PIns.party_cnt = I.lead_insurer_cnt
    AND PAcc.party_cnt = I.account_handler_cnt
    AND PCli.is_deleted = 0
    AND ISNULL(I.policy_ignore, 0) = 0
END
ELSE
BEGIN
    SELECT  S.description branch,
        PCli.shortname,
        PCli.resolved_name,
        I.insurance_ref,
        PIns.name insurer,
        PAcc.name account_handler,
        I.cover_start_date,
        I.expiry_date,
        I.renewal_date,
        ISNULL(I.this_premium, 0),
        ISNULL(I.commission_amount, 0),
        ISNULL(
        (   SELECT  ISNULL(CRG.caption, '')
            FROM    PMCaption       CRG,
                Risk_Group      RG,
                Risk_Code       RC
            WHERE   RC.risk_code_id = I.risk_code_id
            AND RG.risk_group_id = RC.risk_group_id
            AND CRG.caption_id = RG.caption_id
            AND CRG.language_id = @ilanguageID
        )
        , '') risk,
        ISNULL(
        (   SELECT  ISNULL(CAC.caption, '')
            FROM    PMCaption       CAC,
                Analysis_Code       AC
            WHERE   AC.analysis_code_id = I.analysis_code_id
            AND CAC.caption_id = AC.caption_id
            AND CAC.language_id = @ilanguageID
        )
        , '') analysis,
        ISNULL(
        (   SELECT  ISNULL(CST.caption, 'Live')
            FROM    PMCaption       CST,
                Insurance_File_Status   ST
            WHERE   ST.insurance_file_status_id = I.insurance_file_status_id
            AND CST.caption_id = ST.caption_id
            AND CST.language_id = @ilanguageID
        )
        , '') policy_status

    FROM    Source          S,
        Party           PCli,
        Insurance_File      I,
        Insurance_Folder    F,
        Party           PIns,
        Party           PAcc
    WHERE   PCli.party_cnt = F.insurance_holder_cnt
    AND F.insurance_folder_cnt = I.insurance_folder_cnt
    AND     S.source_id = I.source_id
    AND PIns.party_cnt = I.lead_insurer_cnt
    AND PAcc.party_cnt = I.account_handler_cnt
    AND PCli.is_deleted = 0
    AND ISNULL(I.policy_ignore, 0) = 0
    AND S.source_id = @ibranchID
END
GO


