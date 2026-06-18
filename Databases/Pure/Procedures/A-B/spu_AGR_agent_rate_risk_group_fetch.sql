SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_risk_group_fetch'
GO


CREATE PROCEDURE spu_AGR_agent_rate_risk_group_fetch
(
@party_id int,
@risk_group_id int
)
AS

SELECT
agr.effective_date,
agr.agent_rate1,
agr.agent_value1,
agr.minimum_total1,
agr.agent_rate2,
agr.agent_value2,
agr.minimum_total2,
agr.agent_rate3,
agr.agent_value3,
agr.minimum_total3,
agr.rate_type_ind,
ISNULL(agr.tax_group_id,0),
0 as apply_to_all
FROM agent_group_rate agr
WHERE 
agr.party_cnt = @party_id
AND
agr.risk_group_id = @risk_group_id
ORDER BY agr.effective_date DESC

GO