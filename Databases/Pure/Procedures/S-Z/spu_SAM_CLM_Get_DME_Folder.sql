SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_DME_Folder'
GO



CREATE PROCEDURE spu_SAM_CLM_Get_DME_Folder

@ex_code varchar(20), 
@folder_level tinyint, 
@parent_num integer = 0


AS

BEGIN
	SELECT folder_num, folder_name 
	FROM DOC_folder
	WHERE ex_code = @ex_Code
	AND folder_level = @folder_level
	AND (@parent_num = 0) or (parent_num = @parent_num)
END





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
