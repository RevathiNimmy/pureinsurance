SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Agent'
GO

CREATE PROCEDURE spu_Get_Agent  
  @insurance_file_cnt Int  
AS  
SELECT pa.party_cnt,  
       pa.is_single_instalment_plan  
FROM party_agent pa  
LEFT JOIN insurance_file ifl ON ifl.lead_agent_cnt=pa.party_cnt  
WHERE ifl.insurance_file_cnt=@insurance_file_cnt  


GO