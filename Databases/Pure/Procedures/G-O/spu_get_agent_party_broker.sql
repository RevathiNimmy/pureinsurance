SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_agent_party_broker'
GO

CREATE PROCEDURE spu_get_agent_party_broker
        @party_cnt NUMERIC        
AS

BEGIN
	SELECT PA.party_cnt 
	FROM party_agent PA 
	INNER JOIN party_agent_type PAT
	ON PA.party_agent_type_id = PAT.party_agent_type_id
	WHERE RTRIM(UPPER(PAT.code)) ='BROKER'
	AND PA.party_cnt=@party_cnt
	--AND pa.is_single_instalment_plan = 1   Defect 5092
END
GO