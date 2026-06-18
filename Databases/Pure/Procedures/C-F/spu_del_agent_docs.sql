SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_del_agent_docs'
GO

CREATE PROCEDURE spu_del_agent_docs
    @party_cnt INT,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)

AS

-- PW160702 Created
   UPDATE agent_docs SET 
   UserId = @user_id,
   UniqueId = @unique_id,
   ScreenHierarchy = @screen_hierarchy
   WHERE party_cnt = @party_cnt

    DELETE agent_docs
     WHERE party_cnt = @party_cnt

GO
