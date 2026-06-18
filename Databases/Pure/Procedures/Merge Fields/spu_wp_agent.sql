SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_agent'
GO


CREATE PROCEDURE spu_wp_agent
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
    @fsa_registration_number VARCHAR(255),
    @terms_of_payment VARCHAR(255)

    SELECT @address_usage_code = '3131 XCO'

    If @InsuranceFileCnt <> 0
    BEGIN
	    SELECT	@PartyCnt=insf.lead_agent_cnt
        FROM	insurance_file insf
        WHERE	insf.insurance_File_cnt = @InsuranceFileCnt
    END

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

    SELECT	@agent_name=p.name,
			@terms_of_payment=pf.description
    FROM	Party p
    LEFT JOIN PFFrequency pf on pf.pffrequency_id = p.payment_term_code
    WHERE	p.party_cnt = @PartyCnt

	SELECT @fsa_registration_number = pa.fsa_registration_number
	FROM party_agent pa
	WHERE pa.party_cnt = @PartyCnt
	
	SELECT @fsa_agent_status = fas.description
	FROM party_agent pa
	INNER JOIN fsa_agent_status fas ON pa.agent_status_id = fas.fsa_agent_status_id
	WHERE pa.party_cnt = @PartyCnt
	
    SELECT  'agent_address1' = @agent_address1,
            'agent_address2' = @agent_address2,
            'agent_address3' = @agent_address3,
            'agent_address4' = @agent_address4,
            'agent_postal_code' = @agent_postal_code,
            'agent_country' = @agent_country,
            'agent_name' = @agent_name,
            'fsa_registration_number' = @fsa_registration_number,
            'fsa_agent_status' = @fsa_agent_status,
            'terms_of_payment' = @terms_of_payment
GO


