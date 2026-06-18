SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_add_agent_docs'
GO

CREATE PROCEDURE spu_add_agent_docs
    @party_cnt INT,
    @process_type INT,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)

AS

-- PW160702 Created

    INSERT INTO agent_docs
                (party_cnt,
                process_type,
				UserId,
				UniqueId,
				ScreenHierarchy)
         VALUES (@party_cnt,
                @process_type,
				@user_id,
				@unique_id,
				@screen_hierarchy)
GO
