SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXEC DDLDropProcedure 'spe_Agent_PLDSource'
GO

CREATE PROCEDURE spe_Agent_PLDSource  --Delete    
    @Party_Cnt INT,    
    @Branchid INT,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)    
AS    

UPDATE  Party_Agent_Branch
SET UserId = @user_id, UniqueId = @unique_id, ScreenHierarchy = @screen_hierarchy
WHERE
party_cnt = @Party_Cnt 

DELETE FROM    
    Party_Agent_Branch    
WHERE    
    party_cnt = @Party_Cnt    
  