SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_wp_party_leadagent'
GO

CREATE PROCEDURE spu_wp_party_leadagent
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @lead_agent VARCHAR(100) OUTPUT,
    @lead_agent_address_1 VARCHAR(60) OUTPUT,
    @lead_agent_address_2 VARCHAR(60) OUTPUT,
    @lead_agent_address_3 VARCHAR(60) OUTPUT,
    @lead_agent_address_4 VARCHAR(60) OUTPUT,
    @lead_agent_address_5 VARCHAR(20) OUTPUT,
    @lead_agent_country VARCHAR(20) OUTPUT,
    @lead_agent_account VARCHAR(255) OUTPUT
AS

    DECLARE @AgentCnt INT
    
    -- Get agent from insurance_file
    SELECT  @AgentCnt = lead_Agent_cnt 
    FROM    insurance_file
    WHERE   insurance_file_cnt = @InsuranceFileCnt
    
    IF @AgentCnt IS NULL
    BEGIN
        -- No lead agent so clear fields
        SELECT  @lead_agent = NULL,
                @lead_agent_address_1 = NULL,
                @lead_agent_address_2 = NULL,
                @lead_agent_address_3 = NULL,
                @lead_agent_address_4 = NULL,
                @lead_agent_address_5 = NULL,
                @lead_agent_country = NULL,
                @lead_agent_account = NULL
    END
    ELSE
    BEGIN
        -- Get agent name
        SELECT  @lead_agent = name 
        FROM    party 
        WHERE   party_cnt = @AgentCnt
        
        -- Extract agent account number
        SELECT  @lead_agent_account = agency_account_number 
        FROM    party_agent
        WHERE   party_cnt = @AgentCnt
        
        -- Get agent address
        EXEC spu_wp_get_address  
            @AgentCnt,
            @InsuranceFileCnt,
            @ClaimCnt,
            '3131 XCO',
            @lead_agent_address_1 OUTPUT,
            @lead_agent_address_2 OUTPUT,
            @lead_agent_address_3 OUTPUT,
            @lead_agent_address_4 OUTPUT,
            @lead_agent_address_5 OUTPUT,
            @lead_agent_country OUTPUT
    END


GO


