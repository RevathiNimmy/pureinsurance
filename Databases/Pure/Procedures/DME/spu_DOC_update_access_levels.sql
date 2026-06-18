SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_update_access_levels'
GO

create procedure spu_DOC_update_access_levels
@v1 tinyint,
@v2 tinyint,
@v3 tinyint,
@v4 tinyint,
@v5 tinyint,
@v6 tinyint
as
update DOC_system
set  File_Copy_level = @v1,
     File_Delete_level = @v2,
     File_Move_level = @v3,
     Folder_Copy_level = @v4,
     Folder_Delete_level = @v5,
     Folder_Move_level = @v6

GO  