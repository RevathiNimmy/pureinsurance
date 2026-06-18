
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_wp_get_agent_address'
GO

CREATE PROCEDURE spu_wp_get_agent_address
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @address_usage_code VARCHAR(10),
    @address1 VARCHAR(60) OUTPUT,
    @address2 VARCHAR(60) OUTPUT,
    @address3 VARCHAR(60) OUTPUT,
    @address4 VARCHAR(60) OUTPUT,
    @postal_code VARCHAR(20) OUTPUT,
    @country VARCHAR(255) OUTPUT
AS
BEGIN
	DECLARE @Agent_cnt int
	DECLARE @PartyCode varchar(10)
	
	--AR20050419 - PN17879 Get party type, if agent then use PartyCnt otherwise get agent from policy
	SELECT @PartyCode=PT.code
	FROM Party P INNER JOIN Party_Type PT ON P.party_type_id=PT.party_type_id
	WHERE P.party_cnt=@PartyCnt
	
	IF @PartyCode='AG'
		SELECT @Agent_cnt=@PartyCnt
	ELSE
		SELECT top 1 @agent_cnt=agent_cnt
		FROM policy_agents
		WHERE insurance_file_cnt = @InsuranceFileCnt
		
	SELECT	@address1 = a.address1,
			@address2 = a.address2,
			@address3 = a.address3,
			@address4 = a.address4,
			@postal_code =
			(
				CASE
				WHEN a.postal_code = CONVERT(varchar(20), a.address_id) THEN ''
				ELSE a.postal_code
				END
			),
			@country = c.description
	FROM Party_Address_Usage AS pau
	JOIN Address AS a
	ON pau.address_cnt = a.address_cnt
	JOIN Address_Usage_Type AS aut
	ON pau.address_usage_type_id = aut.address_usage_type_id
	LEFT OUTER JOIN Country AS c
	ON a.country_id = c.country_id
	WHERE pau.party_cnt = @Agent_cnt
	AND aut.code = @address_usage_code
END

GO


