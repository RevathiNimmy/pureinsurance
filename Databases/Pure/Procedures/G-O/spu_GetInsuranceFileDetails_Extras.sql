EXECUTE DDLDropProcedure 'spu_GetInsuranceFileDetails_Extras'
GO

CREATE Procedure spu_GetInsuranceFileDetails_Extras
	@InsuranceFileCnt  INT
AS
DECLARE @SubAgentCode VARCHAR(20), @SubAgentName VARCHAR(255)

SELECT     @SubAgentCode = p.shortname, @SubAgentName = p.name   
   FROM       policy_agents    polag              --Policy agents holds all agents and sub-agents for a policy  
   INNER JOIN party_agent      partag    ON polag.agent_cnt            = partag.party_cnt           --Party_agent holds a link to the party_agent_type table  
   INNER JOIN party_agent_type partagty  ON partag.party_agent_type_id = partagty.party_agent_type_id --Join to party_agent_type table to get sub-agent only  
   INNER JOIN party p                    ON polag.agent_cnt            = p.party_cnt                  --Join to party table to get subagent name and short name  
   WHERE      polag.insurance_file_cnt = @InsuranceFileCnt  
   AND        partagty.description     = 'SUB AGENT' 
   
   
   
SELECT  RTRIM(P.code)Product, ISNULL(PAG.resolved_name,'') LeadAgent, 
shortName AS LeadAgentCode ,    
lead_insurer_cnt,     insured_cnt, @SubAgentName SubAgentName,  @SubAgentCode SubAgentCode,
cover_start_date ,CONVERT(VARCHAR,anniversary_date,106)anniversary_date,
IFL.risk_code_id,   RC.risk_group_id,    country_id,       
PD.description  policy_deductibles,
PL.description Policy_limits, 
manual_discount_percentage

FROM Insurance_File IFL
JOIN Product P ON IFL.product_id = P.product_id
LEFT JOIN Party PAG ON IFL.lead_agent_cnt = PAG.party_cnt
LEFT JOIN Policy_Deductibles PD ON IFL.Policy_Deductibles_id = PD.policy_Deductibles_id
LEFT JOIN  Policy_Limits PL ON IFL.policy_limits_id = PL.policy_Limits_id
LEFT JOIN Risk_code RC ON IFL.risk_code_id = RC.risk_code_id
WHERE IFL.insurance_file_cnt = @InsuranceFileCnt

GO
