SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_select_folder'
GO

CREATE PROCEDURE spu_DOC_select_folder
    @parent_num int ,
    @filter varchar(10) ,
    @access_level tinyint,
    @user_id int
AS
BEGIN
	IF @parent_num = 0
		BEGIN
			SELECT folder_num, folder_name, password, create_date
			FROM DOC_folder
			WHERE parent_num = @parent_num
			AND folder_name >= @filter
			AND access_level >= @access_level
			AND ex_code NOT IN (SELECT ISNULL( CAST(source_id  AS VARCHAR) ,'') FROM PMUser_source WHERE user_id = @user_id)
			ORDER BY folder_name
		END
	ELSE
		BEGIN
			SELECT folder_num, folder_name, password, create_date
			FROM DOC_folder
			WHERE parent_num = @parent_num
			AND folder_name >= @filter
			AND access_level >= @access_level
			ORDER BY folder_name
		END
END
GO
