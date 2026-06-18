SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_count_children'
GO

CREATE PROCEDURE spu_DOC_count_children
    @parent_num int = -1
AS
	BEGIN
		SELECT COUNT (folder_num)
		FROM DOC_folder
		WHERE parent_num = @parent_num
	END

GO