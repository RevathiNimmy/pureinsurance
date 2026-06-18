SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_STS_Check_Agent_Party_Relationship'
GO

CREATE PROCEDURE spu_STS_Check_Agent_Party_Relationship
	@Username VARCHAR(255),
	@PrimaryKey INT,
	@Permission INT output

AS

SELECT @Permission = -1

SELECT 
	@Permission = PMUser.Party_Cnt 
FROM 
	pmuser INNER JOIN Party ON 
		pmuser.Party_Cnt = party.agent_cnt 
WHERE 
	PMUser.username = @Username 
    AND party.Party_Cnt = @PrimaryKey

GO
