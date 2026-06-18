EXECUTE DDLDropProcedure spu_gis_get_quote_details_sbo
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_gis_get_quote_details_sbo
    @Insurance_File_Cnt INT
AS
SELECT  DISTINCT ifi.insurance_file_cnt,
	ifi.policy_version,
	ifs.last_trans_date,
	ift.code,
	ifi.cover_start_date,
	ifi.expiry_date,
	ifi.renewal_date,
	ifi.insurance_ref,
	isnull(ifs.last_trans_description,  ' ') as last_trans_description,
	ifs.date_created,
	Party.resolved_name as Name,
	ifi.insured_name as InsuredName,
	rc.risk_group_id,
	Insurance_Folder.insurance_folder_cnt,
	ifi.risk_code_id,
	m.description,
	ifi.lead_insurer_cnt,
	party_agent.party_cnt as lead_agent_cnt,
        isnull(ifst.code ,'LIVE') as status,
        party_agent_type.code,
        party_agent_type.description
	FROM Insurance_File ifi INNER JOIN Insurance_File_System ifs ON ifi.insurance_file_cnt = ifs.insurance_file_cnt
--	INNER JOIN Insurance_File ifi2 ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
	INNER JOIN Insurance_File_Type ift ON ifi.insurance_file_type_id = ift.insurance_file_type_id
	INNER JOIN Insurance_Folder ON ifi.insurance_folder_cnt = Insurance_Folder.insurance_folder_cnt
	INNER JOIN Party ON Insurance_Folder.insurance_holder_cnt = Party.party_cnt
        INNER JOIN Risk_Code rc ON rc.risk_code_id = ifi.risk_code_id
        LEFT OUTER JOIN mta_reason m ON m.mta_reason_id = ifi.user_defined_data_id
        LEFT OUTER JOIN insurance_file_status ifst on ifi.insurance_file_status_id=ifst.insurance_file_status_id
        LEFT OUTER JOIN policy_agents on policy_agents.insurance_file_cnt =ifi.insurance_file_cnt
        LEFT OUTER JOIN party_agent on policy_agents.agent_cnt = party_agent.party_cnt
	LEFT OUTER JOIN party_agent_type on party_agent_type.party_agent_type_id=party_agent.party_agent_type_id 
        WHERE (ifi.policy_ignore Is Null)
        And (ifi.insurance_file_cnt = @Insurance_File_Cnt)
--        And (ifi2.insurance_file_cnt = @Insurance_File_Cnt)
--        And (ifi.insurance_file_status_id Is Null)
        ORDER BY ifi.cover_start_date, ifi.expiry_date,lead_agent_cnt

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
