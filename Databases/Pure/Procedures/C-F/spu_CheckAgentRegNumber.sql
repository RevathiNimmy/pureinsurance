SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_CheckAgentRegNumber'
GO

CREATE PROCEDURE spu_CheckAgentRegNumber

    @RegNumber VARCHAR(255)
    
AS

/*If Registration number exists in the party_agent table then set parameter to show it as existing*/
SELECT fsa_registration_number
    FROM party_agent
    WHERE fsa_registration_number = @RegNumber

GO

