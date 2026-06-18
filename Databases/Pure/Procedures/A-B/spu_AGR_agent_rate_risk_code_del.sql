SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_risk_code_del'
GO


CREATE PROCEDURE spu_AGR_agent_rate_risk_code_del
(
@party_id int,
@risk_code_id int
)
AS

DELETE FROM agent_rate
WHERE 
party_cnt = @party_id
AND
risk_code_id = @risk_code_id
  
GO