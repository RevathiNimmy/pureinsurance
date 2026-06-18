SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_agent_risk_groups'
GO

CREATE PROCEDURE spu_get_agent_risk_groups
    @party_cnt INT

AS

    SELECT 	rg.risk_group_id,
    		rg.description,
    		ISNULL(parg.competency_id, 0),
		'' --DC011203 PN7910 was ""
    FROM 	party_agent_risk_group parg 
    RIGHT OUTER JOIN 	risk_group rg
    ON 		parg.risk_group_id = rg.risk_group_id
    AND 	((parg.party_cnt is null) or (parg.party_cnt = @party_cnt))

GO
