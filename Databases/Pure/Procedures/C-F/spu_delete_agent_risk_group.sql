SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_delete_agent_risk_group'
GO

CREATE PROCEDURE spu_delete_agent_risk_group
    @party_cnt INT,
    @risk_group_id INT

AS

	update party_agent_risk_group
	set competency_id = 	( 	
				SELECT fsa_agent_competency_id 
				FROM FSA_Agent_Competency
				WHERE code = "UDT" 
				),
		date_last_amended = GetDate()
	where party_cnt = @party_cnt and risk_group_id = @risk_group_id
	
GO
