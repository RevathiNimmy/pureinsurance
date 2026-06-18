SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropPROCEDURE 'spu_SAM_GetDMEParentFolder'
GO

CREATE PROCEDURE spu_SAM_GetDMEParentFolder
	@folder_num int
AS
	SELECT parent_num FROM DOC_folder WHERE folder_num = @folder_num

GO