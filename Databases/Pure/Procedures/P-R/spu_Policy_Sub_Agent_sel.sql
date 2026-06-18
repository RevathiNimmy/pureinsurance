SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Policy_Sub_Agent_sel'
GO

CREATE PROCEDURE spu_Policy_Sub_Agent_sel  
 	@insurance_file_cnt int
AS  
BEGIN  

   -- For a given ins file cnt, Get the sub-agent for a policy if one has been assigned
   -- Note that there can be many agents but only one sub-agent
   SELECT     p.shortname, p.name
   FROM       policy_agents    polag								      --Policy agents holds all agents and sub-agents for a policy
   INNER JOIN party_agent      partag    ON polag.agent_cnt            = partag.party_cnt    	      --Party_agent holds a link to the party_agent_type table
   INNER JOIN party_agent_type partagty  ON partag.party_agent_type_id = partagty.party_agent_type_id --Join to party_agent_type table to get sub-agent only
   INNER JOIN party p                    ON polag.agent_cnt            = p.party_cnt                  --Join to party table to get subagent name and short name
   WHERE      polag.insurance_file_cnt = @insurance_file_cnt
   AND        partagty.description     = 'SUB AGENT'

END  
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO