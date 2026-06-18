SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Rename_DME_Folder'
GO


CREATE PROCEDURE spu_SAM_Rename_DME_Folder

@folder_name varchar(70), 
@folder_num integer

AS

BEGIN

	UPDATE DOC_folder 
	SET folder_name = @folder_name
	WHERE folder_num = @folder_num

END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
