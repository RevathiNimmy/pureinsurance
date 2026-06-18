
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_associated_subagents_add'
GO

CREATE PROCEDURE spu_SAM_associated_subagents_add
	@insurance_file_cnt int,
	@agent_cnt int
AS

DECLARE @ssp_sub_agent int
DECLARE @AddSSPAgentEnabled int

SELECT @ssp_sub_agent = ISNULL(party_cnt,0) FROM party_agent WHERE is_ssp_subagent=1

SELECT @AddSSPAgentEnabled = ISNULL(value, 0) FROM Hidden_Options WHERE option_number = 96

IF ISNULL(@agent_cnt,0) > 0
BEGIN	
	INSERT INTO insurance_file_agent
	(insurance_file_cnt, party_cnt, percentage, amount)
	SELECT
	@insurance_file_cnt, PR.relation_cnt, 0, 0
	FROM
	party_relationship PR
	JOIN
	relationship_type RT ON RT.relationship_type_id=PR.relationship_type_id
	JOIN
	party_relationship_group PRG ON PRG.party_relationship_group_id=RT.party_relationship_group_id
	JOIN
	party_agent PA ON PA.party_cnt=PR.relation_cnt
	JOIN
	party_agent_type PAT ON PAT.party_agent_type_id=PA.party_agent_type_id
	WHERE
	PR.party_cnt=@agent_cnt
	AND
	PRG.code='002'
	AND
	PAT.code='Sub-Agent'
	AND
	PR.relation_cnt <> @ssp_sub_agent
END	

IF (@ssp_sub_agent > 0) AND (@AddSSPAgentEnabled > 0)
BEGIN
	IF NOT EXISTS(SELECT NULL FROM insurance_file_agent WHERE insurance_file_cnt=@insurance_file_cnt AND party_cnt=@ssp_sub_agent)
	BEGIN
		INSERT INTO insurance_file_agent
		(insurance_file_cnt, party_cnt, percentage, amount)
		VALUES
		(@insurance_file_cnt, @ssp_sub_agent, 0, 0)
	END
END

GO


