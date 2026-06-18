SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropFUNCTION  'dbo.GetDMEPath'
GO

CREATE FUNCTION dbo.GetDMEPath
(@folder_num as int)
RETURNS varchar(256)
BEGIN
DECLARE @name varchar(256)
DECLARE @parent_num int

SELECT @parent_num = parent_num, @name = LTRIM(RTRIM(folder_name)) FROM doc_folder WITH(NOLOCK) WHERE folder_num = @folder_num

IF @parent_num != 0
	SELECT @name = dbo.GetDMEPath(@parent_num) + '|' + LTRIM(RTRIM(@name))
	
RETURN @name

END


GO