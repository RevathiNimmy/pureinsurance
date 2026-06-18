SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_delete_pmuser_source_info'
GO

CREATE PROCEDURE spu_delete_pmuser_source_info
	@user_id as integer,
	@source_id as integer,
	@unique_id as varchar(50),
	@screen_hierarchy as varchar(500)
        
AS

BEGIN
    UPDATE PMUser_Source SET
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy 
	where 	source_id = @source_id 
	and	user_id = @user_id

	delete from pmuser_source
	where 	source_id = @source_id 
	and	user_id = @user_id

END
GO
