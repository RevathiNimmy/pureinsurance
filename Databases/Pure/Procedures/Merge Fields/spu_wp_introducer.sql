ddldropprocedure 'spu_wp_introducer'
go

CREATE PROCEDURE spu_wp_introducer
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE @agent_address1 VARCHAR(60),
    @agent_address2 VARCHAR(60),
    @agent_address3 VARCHAR(60),
    @agent_address4 VARCHAR(60),
    @agent_postal_code VARCHAR(20),
    @agent_country varchar(255),
    @address_usage_code VARCHAR(10),
    @agent_name VARCHAR(60),
    @fsa_agent_status VARCHAR(255),
    @fsa_registration_number VARCHAR(255)

    SELECT @address_usage_code = '3131 XCO'

    If @InsuranceFileCnt <> 0
    Begin
    SELECT @PartyCnt = (Select Top 1 agent_cnt
                from  policy_agents pa,
                      party_agent_type at,
                      party_agent ap
                where pa.insurance_file_cnt=@InsuranceFileCnt
                      and pa.agent_cnt=ap.party_cnt
                      and ap.party_agent_type_id=at.party_agent_type_id
                      and at.description='INTRODUCER')
    End

    EXEC spu_wp_get_address  @PartyCnt,
                @InsuranceFileCnt,
                @ClaimCnt,
                @address_usage_code,
                @agent_address1 OUTPUT,
                @agent_address2 OUTPUT,
                @agent_address3 OUTPUT,
                @agent_address4 OUTPUT,
                @agent_postal_code OUTPUT,
                @agent_country OUTPUT

    SELECT @agent_name = (SELECT p.name
                    FROM party p
                    WHERE p.party_cnt = @PartyCnt)

	SELECT @fsa_registration_number = pa.fsa_registration_number
	FROM party_agent pa
	WHERE pa.party_cnt = @PartyCnt
	
	SELECT @fsa_agent_status = fas.description
	FROM party_agent pa
	INNER JOIN fsa_agent_status fas ON pa.agent_status_id = fas.fsa_agent_status_id
	WHERE pa.party_cnt = @PartyCnt
	
    SELECT  'introducer_address1' = @agent_address1,
            'introducer_address2' = @agent_address2,
            'introducer_address3' = @agent_address3,
            'introducer_address4' = @agent_address4,
            'introducer_postal_code' = @agent_postal_code,
            'introducer_country' = @agent_country,
            'introducer_name' = @agent_name,
            'introducer_fsa_registration_number' = @fsa_registration_number,
            'introducer_fsa_introducer_status' = @fsa_agent_status

