SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_add_pmuser_source_info'
GO

CREATE PROCEDURE spu_add_pmuser_source_info
	@user_id as integer,
	@source_id as integer,
	@unique_id as varchar(50),
	@screen_hierarchy as varchar(500)
        
AS

BEGIN

	insert pmuser_source
	(user_id, source_id, UniqueId, ScreenHierarchy)
	values (@user_id, @source_id, @unique_id, @screen_hierarchy)

END
GO
