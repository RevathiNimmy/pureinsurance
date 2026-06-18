SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_STS_Check_Agent_Insurance_File_Relationship'
GO

CREATE PROCEDURE spu_STS_Check_Agent_Insurance_File_Relationship
	@Username VARCHAR(255),
	@PrimaryKey INT,
	@Permission INT output

AS

SELECT @Permission = -1

SELECT 
	@Permission = PMUser.Party_Cnt
FROM 
	pmuser INNER JOIN policy_agents ON 
		pmuser.Party_Cnt = policy_agents.agent_cnt
WHERE 
	PMUser.username = @Username 
    AND policy_agents.Insurance_File_Cnt = @PrimaryKey

GO
