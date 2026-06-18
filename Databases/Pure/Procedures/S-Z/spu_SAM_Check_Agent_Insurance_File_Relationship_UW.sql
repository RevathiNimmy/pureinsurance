SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Check_Agent_Insurance_File_Relationship_UW'
GO

CREATE PROCEDURE spu_SAM_Check_Agent_Insurance_File_Relationship_UW
    @Username VARCHAR(255),
    @PrimaryKey INT,
    @Permission INT output

AS

SELECT @Permission = -1

SELECT 
    @Permission = PMUser.Party_Cnt
FROM 
    pmuser INNER JOIN insurance_file ON 
        pmuser.Party_Cnt = insurance_file.lead_agent_cnt
WHERE 
    PMUser.username = @Username 
    AND insurance_file.Insurance_File_Cnt = @PrimaryKey

GO
