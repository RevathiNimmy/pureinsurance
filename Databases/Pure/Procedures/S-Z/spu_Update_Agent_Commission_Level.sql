SET QUOTED_IDENTIFIER  OFF   
 SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_Update_Agent_Commission_Level'
GO


CREATE PROCEDURE spu_Update_Agent_Commission_Level
@Agent_Commission_Level_Id int, 
@Party_Agent_Cnt int, 
@Commission_Level_Id int, 
@Efffective_Date Date,
@Is_deleted tinyint,
@user_id int,
@unique_id varchar(50),
@screen_hierarchy varchar(500)

AS

If @Agent_Commission_Level_Id > 0 
	BEGIN 
		UPDATE	Agent_Commission_Level 
		SET		Commission_level_id =  @Commission_Level_Id,
				Effective_date  = @Efffective_Date,
				Is_deleted = @is_deleted,
				UserId = @user_id,
				UniqueId = @unique_id,
				ScreenHierarchy = @screen_hierarchy
		WHERE	Agent_Commission_Level_Id = @Agent_Commission_Level_Id
		AND		Party_agent_cnt = @Party_Agent_Cnt
	END 

ELSE
	BEGIN
		INSERT INTO Agent_Commission_Level (Party_agent_cnt,Commission_level_id,Effective_date,Is_deleted, UserId, UniqueId, ScreenHierarchy)
		VALUES (@Party_Agent_Cnt,@Commission_Level_Id,@Efffective_Date,@is_deleted,@user_id,@unique_id,@screen_hierarchy)
	END

GO
SET QUOTED_IDENTIFIER  ON   
 SET ANSI_NULLS  OFF 
GO





