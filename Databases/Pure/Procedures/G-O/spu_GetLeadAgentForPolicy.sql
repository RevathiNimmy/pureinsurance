SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetLeadAgentForPolicy'
GO


CREATE PROCEDURE spu_GetLeadAgentForPolicy
    @InsuranceRef varchar(30)

AS
Declare @bIsTransactOnAgent BIT
-- we only only want the latest version not all of them
-- also return NULL if its a Direct Business


IF EXISTS (SELECT NULL from insurance_file INS 
INNER JOIN document DOC on DOC.insurance_file_cnt=INS.insurance_file_cnt
INNER JOIN transdetail TD on TD.document_id=DOC.document_id
INNER JOIN Account AC on AC.account_id=TD.account_id
INNER JOIN party PAR on PAR.party_cnt=AC.account_key
WHERE INS.insurance_ref=@InsuranceRef and TD.spare='GROSS' AND party_type_id<>3) --client
	SELECT @bIsTransactOnAgent=0 --if posting of gross amount on client
ELSE
	SELECT @bIsTransactOnAgent=1 --if posting of gross amount on agent

SELECT  TOP 1 IFL.lead_agent_cnt, LTRIM(RTRIM(PAT.code)) code,@bIsTransactOnAgent IsTransactOnAgent

FROM    insurance_file IFL
LEFT JOIN	Party_Agent PA ON PA.party_cnt = IFL.lead_agent_cnt
LEFT JOIN 	Party_Agent_Type PAT ON PAT.party_agent_type_id=PA.party_agent_type_id
WHERE IFL.insurance_ref LIKE RTRIM(@InsuranceRef) 
ORDER BY IFL.insurance_file_cnt DESC
GO


