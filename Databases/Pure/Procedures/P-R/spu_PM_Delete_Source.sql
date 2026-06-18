SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PM_Delete_Source'
GO


CREATE PROCEDURE spu_PM_Delete_Source
    @source_id integer,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS


UPDATE Source
    SET is_deleted = 1,
		UserId = @user_id,
		UniqueId = @unique_id,
		ScreenHierarchy = @screen_hierarchy
    WHERE source_id = @source_id
GO

