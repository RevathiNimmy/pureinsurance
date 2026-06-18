SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Policy_Account_Details'
GO


CREATE PROCEDURE spu_SAM_Get_Policy_Account_Details

@insurance_file_cnt int

AS  

SELECT 
insured_cnt, 
lead_agent_cnt,
insured_accountstatus.code insured_accountstatus_code, 
agent_accountstatus.code agent_accountstatus_code 

FROM insurance_file

LEFT OUTER JOIN party insured ON 
	insured_cnt = insured.party_cnt

LEFT OUTER JOIN account insured_account ON 
	insured.party_cnt = insured_account.account_key

LEFT OUTER JOIN accountstatus insured_accountstatus ON 
	insured_account.accountstatus_id = insured_accountstatus.accountstatus_id

LEFT OUTER JOIN party agent ON 
	lead_agent_cnt = agent.party_cnt

LEFT OUTER JOIN account agent_account ON 
	agent.party_cnt = agent_account.account_key

LEFT OUTER JOIN accountstatus agent_accountstatus ON 
	agent_account.accountstatus_id = agent_accountstatus.accountstatus_id

WHERE insurance_file_cnt = @insurance_file_cnt




GO
