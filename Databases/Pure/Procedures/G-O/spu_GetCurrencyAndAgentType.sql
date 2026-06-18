

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetCurrencyAndAgentType'
GO

Create Procedure spu_GetCurrencyAndAgentType
    @insurance_file_cnt INT
AS

SELECT Currency_id, PAT.Code,lead_agent_cnt 
FROM insurance_file ifi LEFT JOIN Party_agent PA ON ifi.lead_agent_cnt=PA.party_cnt 
LEFT JOIN Party_agent_type PAT ON Pa.Party_agent_type_id=PAT.Party_agent_type_id
Where insurance_file_cnt = @insurance_file_cnt

GO