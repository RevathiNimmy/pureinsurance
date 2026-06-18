SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'qryPolicyAgents'
GO

CREATE VIEW qryPolicyAgents AS 

SELECT 
    policy_agents.insurance_file_cnt	    	'PolicyID',
    party.shortname			    	'Code',
    party.resolved_name                     	'Name',
    policy_agents.agent_commission_percentage	'CommissionPercentage',
    policy_agents.agent_commission_amount	'CommissionCharge',
    policy_agents.agent_commission_value	'TotalCommission'
FROM policy_agents
JOIN party 
    ON policy_agents.agent_cnt = party.party_cnt
    
GO 