SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_risk_code_fetch'
GO


CREATE PROCEDURE spu_AGR_agent_rate_risk_code_fetch
(
@party_id int,
@risk_code_id int
)
AS

SELECT
ar.effective_date,
ar.agent_rate1,
ar.agent_value1,
ar.minimum_total1,
ar.agent_rate2,
ar.agent_value2,
ar.minimum_total2,
ar.agent_rate3,
ar.agent_value3,
ar.minimum_total3,
ar.rate_type_ind,
ISNULL(ar.tax_group_id,0),
0 as apply_to_all
FROM agent_rate ar
WHERE 
ar.party_cnt = @party_id
AND
ar.risk_code_id = @risk_code_id
ORDER BY ar.effective_date DESC

  
GO