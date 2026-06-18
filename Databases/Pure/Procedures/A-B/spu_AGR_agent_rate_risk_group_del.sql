SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_risk_group_del'
GO


CREATE PROCEDURE spu_AGR_agent_rate_risk_group_del
(
@party_id int,
@risk_group_id int
)
AS

DELETE FROM agent_group_rate
WHERE 
party_cnt = @party_id
AND
risk_group_id = @risk_group_id
  
GO