SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_AgentInformation'
GO
 
CREATE Procedure spu_Get_AgentInformation   
    @Agent_cnt BIGINT  
AS  
    SELECT pat.Code,is_float_balance_account,is_overdraft_account,     
    float_balance_limit , overDraft_limit ,Overdraft_expiry   
    FROM party_agent pa  
    LEFT JOIN   Party_agent_type pat ON   
    pa.party_agent_type_id=pat.party_agent_type_id   
    WHERE party_cnt=@Agent_cnt  
  