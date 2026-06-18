SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_delete_pmuser_group_info'
GO

CREATE PROCEDURE spu_delete_pmuser_group_info
	@user_id as integer,
	@pmuser_group_id as integer,
	@unique_id as varchar(50),
	@screen_hierarchy as varchar(500)
        
AS

BEGIN

    UPDATE pmuser_group_user SET
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy 
	where 	pmuser_group_id = @pmuser_group_id 
	and	user_id = @user_id

	delete from pmuser_group_user
	where 	pmuser_group_id = @pmuser_group_id 
	and	user_id = @user_id

END
GO
