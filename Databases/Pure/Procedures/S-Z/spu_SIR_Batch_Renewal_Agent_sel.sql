SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Agent_sel'
GO

CREATE PROCEDURE spu_SIR_Batch_Renewal_Agent_sel

	@batch_renewal_job_id	INT = NULL

As
BEGIN

	IF ISNULL(@batch_renewal_job_id,0) >= 0
	BEGIN
	SELECT DISTINCT Party.party_cnt,
		shortname,
		name,
		address.address1,
		Address.address2,
		postal_code =
		CASE address.postal_code
			WHEN convert(varchar(20),address.address_id) THEN
				''
   ELSE address.postal_code  
			END
		FROM Party,

		Party_Agent_Type,
		Party_Agent,
		Party_Agent_Branch,
		Party_Type
		,Source
		, Address,
		Party_Address_Usage,
		Address_Usage_Type,
		Batch_Renewal_Job_Agents
	WHERE Party_Type.party_type_id = Party.party_type_id
		AND Party.is_deleted = 0
		AND Source.source_id = Party.source_id
		AND Party_Address_Usage.party_cnt = Party.party_cnt
		AND Party_Address_Usage.address_cnt = Address.address_cnt
		AND Address_Usage_Type.address_usage_type_id = Party_Address_Usage.address_usage_type_id
		AND Address_Usage_Type.code = '3131 XCO'
		AND Party_type.code = 'AG'
		AND Party_Agent.party_cnt = Party.party_cnt
		AND Party_agent.party_cnt= Party_agent_branch.party_cnt
		AND Party_Agent_Type.party_agent_type_id = Party_Agent.party_agent_type_id
		AND Party_Agent_Type.is_visible = 1
		AND Party_Agent.party_cnt = Party.party_cnt
		AND Party_Agent_Type.party_agent_type_id = Party_Agent.party_agent_type_id
		AND Party_Agent_Type.Party_Agent_Type_id <> '2'
		AND Batch_Renewal_Job_Agents.party_cnt = Party.party_cnt
		AND Batch_Renewal_Job_Agents.batch_renewal_job_id=@batch_renewal_job_id
	ORDER BY Shortname
  
	END
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO