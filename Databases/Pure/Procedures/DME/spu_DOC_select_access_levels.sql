SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_select_access_levels'
GO
create procedure spu_DOC_select_access_levels
as
select File_Copy_level, 
	   File_Delete_level, 
	   File_Move_level,
	   Folder_Copy_level, 
	   Folder_Delete_level, 
	   Folder_Move_level
from DOC_system

GO
   