SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropFUNCTION  'dbo.GetDMETopLevelFolder'
GO

CREATE FUNCTION dbo.GetDMETopLevelFolder
(@folder_num as int)
RETURNS int
BEGIN
DECLARE @parent_num int
DECLARE @return_folder_num int

SELECT @parent_num = parent_num FROM doc_folder WITH(NOLOCK) WHERE folder_num = @folder_num

IF @parent_num != 0
	SELECT @return_folder_num = dbo.GetDMETopLevelFolder(@parent_num)
ELSE	
	SELECT @return_folder_num = @folder_num

RETURN @return_folder_num
END


GO