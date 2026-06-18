SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_select'
GO


CREATE PROCEDURE spu_AGR_agent_rate_select
(
@party_id int,
@risk_code_id int,
@risk_group_id int,
@effective_date datetime
)
AS

IF EXISTS(SELECT NULL FROM agent_rate WHERE party_cnt = @party_id AND risk_code_id = @risk_code_id AND effective_date<=@effective_date)

SELECT TOP 1
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
ar.tax_group_id
FROM agent_rate ar
WHERE 
ar.party_cnt = @party_id
AND
ar.risk_code_id = @risk_code_id
AND
ar.effective_date<=@effective_date
ORDER BY ar.effective_date DESC

ELSE

SELECT TOP 1
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
agr.tax_group_id
FROM agent_group_rate agr
WHERE 
agr.party_cnt = @party_id
AND
agr.risk_group_id = @risk_group_id
AND
agr.effective_date<=@effective_date
ORDER BY agr.effective_date DESC

GO