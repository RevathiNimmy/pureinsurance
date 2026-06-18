SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_select_matched_folders'
GO

CREATE PROCEDURE spu_DOC_select_matched_folders
    @parent_num int ,
    @folder_name varchar(20) ,
    @access_level tinyint
AS
BEGIN
	SELECT folder_num, folder_name, password, create_date
	FROM DOC_folder
	WHERE parent_num = @parent_num
	AND folder_name LIKE @folder_name
	AND access_level >= @access_level
	ORDER BY folder_name
END

GO