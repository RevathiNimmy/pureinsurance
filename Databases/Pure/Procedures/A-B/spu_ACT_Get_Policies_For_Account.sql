SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Policies_For_Account'
GO


CREATE PROCEDURE spu_ACT_Get_Policies_For_Account 
	@account_id INT
AS

DECLARE @PartyCnt INT
DECLARE @PartyAgentTypeId INT

SELECT @PartyCnt = account_key,@PartyAgentTypeId=pa.party_agent_type_id FROM party p 
	INNER JOIN Account a ON a.account_key=p.party_cnt 
	LEFT JOIN party_agent pa ON pa.party_cnt=p.party_cnt
	WHERE account_id  = @account_id 

IF ISNULL(@PartyAgentTypeId,0)=1 
SELECT DISTINCT insurance_ref from insurance_file WHERE lead_agent_cnt = @PartyCnt ORDER BY insurance_ref
ELSE 
SELECT DISTINCT insurance_ref from insurance_file WHERE insured_cnt = @PartyCnt ORDER BY insurance_ref

