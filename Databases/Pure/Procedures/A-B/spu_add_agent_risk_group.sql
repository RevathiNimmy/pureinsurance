SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_add_agent_risk_group'
GO

CREATE PROCEDURE spu_add_agent_risk_group
    @party_cnt INT,
    @risk_group_id INT

AS
DECLARE @competency_id integer

	SELECT @competency_id = ( SELECT fsa_agent_competency_id FROM fsa_agent_competency WHERE code = "CMP" )

	IF EXISTS 	( 
			SELECT 	* 
			FROM 	party_agent_risk_group 
			WHERE 	party_cnt = @party_cnt
			AND	risk_group_id = @risk_group_id 
			)
	BEGIN
		UPDATE 	party_agent_risk_group
		SET 	competency_id = @competency_id, date_last_amended = GetDate()
		WHERE 	party_cnt = @party_cnt
		AND	risk_group_id = @risk_group_id 
	END
	ELSE
	BEGIN
		insert into party_agent_risk_group (party_cnt, risk_group_id, competency_id, date_last_amended, effective_date)
		values (@party_cnt, @risk_group_id, @competency_id, GetDate(), GetDate())
	END
GO
