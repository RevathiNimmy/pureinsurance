SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropPROCEDURE 'spu_SAM_ResolveDMEPath'
GO

CREATE PROCEDURE spu_SAM_ResolveDMEPath
 @folder_name varchar(285),
 @sParentFolder VARCHAR(255) = NULL
AS  

DECLARE @lFolderNum INT
DECLARE @sFolderPath VARCHAR(256)

IF @sParentFolder IS NOT NULL
BEGIN
        -- Find the folder where its actual parent's name matches
        SELECT @lFolderNum = f.Folder_num
        FROM DOC_folder f
        INNER JOIN DOC_folder p ON f.parent_num = p.Folder_num
        WHERE f.folder_name = @folder_name
          AND p.folder_name = @sParentFolder
    END
ELSE
	SELECT @lFolderNum = Folder_num FROM DOC_folder WHERE folder_name = @folder_name


SET @sFolderPath = dbo.GetDMEPath(@lFolderNum)

SELECT @lFolderNum as Folder_num,@sFolderPath as FolderPath  


