SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_policyagent2'
GO

CREATE PROCEDURE spu_wp_policyagent2    

    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT

AS

DECLARE 
    @agent_type VARCHAR(255),
    @agent_origin VARCHAR(255),
    @agency_agreement_date DATETIME,
    @agency_next_review_date DATETIME,
    @agency_account_number VARCHAR(255),
    @is_branch TINYINT,
    @is_head_office TINYINT,
    @default_commission_percent FLOAT,
    @trading_name VARCHAR(255),
    @witholding_tax MONEY,
    @binder_indicator INT,
    @report_indicator INT,
    @agent_address1 VARCHAR(60),
    @agent_address2 VARCHAR(60),
    @agent_address3 VARCHAR(60),
    @agent_address4 VARCHAR(60),
    @agent_postal_code VARCHAR(20),
    @agent_name VARCHAR(60),
    @AgentCnt INT,
    @AgentCntLoop INT,
    @address_usage_code VARCHAR(10),
    @agent_value MONEY,
    @shortname VARCHAR(20),
    @country VARCHAR(255),
    @fsa_registration_number VARCHAR(255),
    @fsa_agent_status VARCHAR(255),
    @commission_percentage FLOAT,
    @percentage_of VARCHAR(20),
    @overridden VARCHAR(20)

DECLARE c_cursor SCROLL CURSOR FOR
    SELECT 
        pola.agent_cnt 
    FROM policy_agents pola
    JOIN party_agent pa
        ON pa.party_cnt = pola.agent_cnt
    JOIN party_agent_type pat
        ON pat.party_agent_type_id = pa.party_agent_type_id
    WHERE pola.insurance_file_cnt = @InsuranceFileCnt
    AND pat.description IN ('AGENT','SUB AGENT')
    ORDER BY pola.policy_agents_id

OPEN c_cursor

FETCH ABSOLUTE 2 FROM c_cursor INTO @AgentCnt

CLOSE c_cursor
DEALLOCATE c_cursor

IF @AgentCnt IS NOT NULL
BEGIN

    EXEC spu_wp_party_agent 
        @AgentCnt,
        @InsuranceFileCnt,
        @ClaimCnt,
        @agent_type OUTPUT,
        @agent_origin OUTPUT,
        @agency_agreement_date OUTPUT,
        @agency_next_review_date OUTPUT,
        @agency_account_number OUTPUT,
        @is_branch OUTPUT,
        @is_head_office OUTPUT,
        @default_commission_percent OUTPUT,
        @trading_name OUTPUT,
        @binder_indicator OUTPUT,
        @report_indicator OUTPUT,
        @witholding_tax OUTPUT

    SELECT @address_usage_code = '3131 XCO'

    EXEC spu_wp_get_address 
        @AgentCnt,
        @InsuranceFileCnt,
        @ClaimCnt,
        @address_usage_code,
        @agent_address1 OUTPUT,
        @agent_address2 OUTPUT,
        @agent_address3 OUTPUT,
        @agent_address4 OUTPUT,
        @agent_postal_code OUTPUT,
        @country OUTPUT

    SELECT @agent_name = 
        (
            SELECT p.name 
            FROM party p
            WHERE p.party_cnt = @AgentCnt
        )

    SELECT @shortname = 
        (
            SELECT p.shortname 
            FROM party p
            WHERE p.party_cnt = @AgentCnt
        )

    SELECT 
        @fsa_registration_number = fsa_registration_number
    FROM party_agent pa
    WHERE pa.party_cnt=@AgentCnt
            
    SELECT 
        @fsa_agent_status = fas.description
    FROM party_agent pa
    JOIN fsa_agent_status fas 
        ON fas.fsa_agent_status_id = pa.agent_status_id
    WHERE pa.party_cnt = @AgentCnt
        
    SELECT 
        @agent_value = agent_commission_value,
        @commission_percentage = agent_commission_percentage,
        @percentage_of = 
            CASE apply_perc_to_prem_or_comm
                WHEN 1 THEN 'Commission'
                ELSE 'Premium'
            END,
        @overridden = 
            CASE override_rate_table
                WHEN 1 THEN 'Yes'
                ELSE 'No'
            END
    FROM policy_agents 
    WHERE insurance_file_cnt = @InsuranceFileCnt
    AND agent_cnt = @AgentCnt

END 

SELECT  
    'agent_address1' = @agent_address1,
    'agent_address2' = @agent_address2,
    'agent_address3' = @agent_address3,
    'agent_address4' = @agent_address4,
    'agent_postal_code' = @agent_postal_code,
    'agent_name' = @agent_name,
    'agent_origin' = @agent_origin,
    'agency_agreement_date' = @agency_agreement_date,
    'agency_next_review_date' = @agency_next_review_date,
    'agency_account_number' = @agency_account_number,
    'is_branch' = @is_branch,
    'is_head_office' = @is_head_office,
    'default_comm_percent' = @default_commission_percent ,
    'trading_name' = @trading_name,
    'witholding_tax' = @witholding_tax,
    'binder_indicator' = @binder_indicator,
    'report_indicator' = @report_indicator,
    'agent_value' = @agent_value,
    'shortname' = @shortname,
    'fsa_registration_number' = @fsa_registration_number,
    'fsa_agent_status' = @fsa_agent_status,
    'commission_percentage' = @commission_percentage,
    'percentage_of' = @percentage_of,
    'overridden' = @overridden
    
GO
