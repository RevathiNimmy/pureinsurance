SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spe_Agent_PLSSource'
GO

CREATE PROCEDURE spe_Agent_PLSSource  --Add  
    @Party_Cnt INT,    
    @Branchid INT,
	@user_id INT=NULL,
	@unique_id VARCHAR(50)=NULL,
	@screen_hierarchy VARCHAR(500)=NULL
    
AS    
    
INSERT INTO    
    Party_Agent_Branch (Party_cnt,Source_id,UserId, UniqueId, ScreenHierarchy)    
VALUES    
    (@Party_Cnt,@Branchid, @user_id, @unique_id, @screen_hierarchy)  
