SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_update_pmuser_group_info'
GO

CREATE PROCEDURE spu_update_pmuser_group_info
	@user_id as integer,
	@pmuser_group_id as integer,
	@is_supervisor as smallint,
	@unique_id as varchar(50),
	@screen_hierarchy as varchar(500)
        
AS

BEGIN

	If exists ( select * from pmuser_group_user 
			where 	pmuser_group_id = @pmuser_group_id 
			and	user_id = @user_id )
	begin
		update pmuser_group_user
		set is_supervisor = @is_supervisor,
		UniqueId = @unique_id,
		ScreenHierarchy = @screen_hierarchy
		where 	pmuser_group_id = @pmuser_group_id 
		and	user_id = @user_id
	end
	else
	begin
		insert pmuser_group_user
		(pmuser_group_id, user_id, display_sequence_num, is_supervisor, UniqueId, ScreenHierarchy)
		values (@pmuser_group_id, @user_id, 0, @is_supervisor, @unique_id, @screen_hierarchy)
	end

END
GO
