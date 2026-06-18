SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Policy_List_By_SubAgent'
GO
--ECK 010802 Modified to show premium excluding IPT
/*PN4273 Added code to return latest live version of a policy */
CREATE PROCEDURE spu_Report_Policy_List_By_SubAgent
    @branch_id int,
    @agent_type     varchar(9)

AS

DECLARE
    @insurance_file_status_id int,
    @policy_status varchar(20),
    @account_handler_cnt int,
    @account_handler varchar(20),
    @ibranchID int

SELECT @ibranchID = ISNULL(@branch_id, 0)

SELECT
    S.description branch,
    PCli.shortname,
    PCli.resolved_name,
    I.insurance_ref,
    PIns.name insurer,
    PIns.shortname ins_code,
    PSub.name sub_agent,
    I.this_premium Gross_Premium,
    I.commission_amount 			Commission,			        --1.6.9
    PolAg.agent_commission_value 		agent_Commission,			--1.6.9 
    (SELECT I.commission_amount - PolAg.agent_commission_value) net_commission, 	--1.6.9
    PolAg.agent_Commission_percentage	agent_percentage,			--1.6.9
    PolAg.agent_Commission_amount		agent_payment,				--1.6.9
    PolAg.override_rate_table		agent_rate_override,			--1.6.9
    (SELECT  (CASE									--1.6.9
        	WHEN apply_perc_to_prem_or_comm = 1 THEN
            	'C'
        	WHEN apply_perc_to_prem_or_comm = 0 THEN
            	'P'
 		END)				
     ) apply_to,
     I.renewal_date				renewal_date,
    ISNULL(
    ( SELECT ISNULL(CRG.caption, '')
        FROM PMCaption CRG,
            Risk_Group RG,
            Risk_Code RC
        WHERE RC.risk_code_id = I.risk_code_id
        AND RG.risk_group_id = RC.risk_group_id
        AND CRG.caption_id = RG.caption_id
    )
, '') risk_group,
    ISNULL(
    ( SELECT ISNULL(CAC.caption, '')
        FROM PMCaption CAC,
            Analysis_Code AC
        WHERE AC.analysis_code_id = I.analysis_code_id
        AND CAC.caption_id = AC.caption_id
    )
, '') analysis,
    ISNULL(
    ( SELECT ISNULL(CST.caption, 'Live')
        FROM PMCaption CST,
            Insurance_File_Status ST
        WHERE ST.insurance_file_status_id = I.insurance_file_status_id
        AND CST.caption_id = ST.caption_id
    )
, 'Live') policy_status,
    ISNULL(
    ( SELECT RC.description
        FROM Risk_Code RC
        WHERE RC.risk_code_id = I.risk_code_id
    ), '') risk_code,
    C.currency_id,
    C.code currency_code
FROM Insurance_Folder F
JOIN Insurance_File I
ON I.insurance_folder_cnt = F.insurance_folder_cnt
JOIN Source S
ON S.source_id = I.source_id
JOIN Insurance_File_Type IFT
ON IFT.insurance_file_type_id = I.insurance_file_type_id
JOIN Party PIns
ON PIns.party_cnt = I.lead_insurer_cnt
JOIN Party PCli
ON PCli.party_cnt = F.insurance_holder_cnt
JOIN currency C
ON C.currency_id = I.currency_id
JOIN Policy_agents PolAg
ON PolAg.insurance_file_cnt = I.insurance_file_cnt
JOIN Party PSub
ON PSub.party_cnt = PolAg.agent_cnt
JOIN Party_Agent PAgent
ON PAgent.party_cnt = PSub.party_cnt
JOIN Party_Agent_type PAgType
ON PAgType.party_agent_type_id = PAgent.party_agent_type_id 
WHERE I.policy_version=(select MAX(i2.policy_version) from insurance_file i2,insurance_file_type ift where i2.insurance_ref=i.insurance_ref AND ift.code in('POLICY','RENEWAL','MTA PERM','MTA TEMP','MTAPERMCAN'))
AND PCli.is_deleted = 0
AND PAgType.description = @agent_type			
AND ISNULL(I.policy_ignore, 0) = 0
AND
    (
        @branch_id = 0
        OR
        (
        @branch_id <> 0
        AND
        S.source_id = @ibranchID
        )
    )
ORDER BY C.currency_id, branch, policy_status, PCli.shortname

GO

