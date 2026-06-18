SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropPROCEDURE 'spu_SAM_GetDMEFolderBranch'
GO

CREATE PROCEDURE spu_SAM_GetDMEFolderBranch
	@folder_num int
AS
	SELECT ex_code FROM DOC_folder WHERE folder_num = dbo.GetDMETopLevelFolder(@folder_num)

GO