SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropPROCEDURE 'spu_SAM_GetDMESubFolders'
GO

CREATE PROCEDURE spu_SAM_GetDMESubFolders
 @folder_num int  
AS  
SELECT  
 folder_num,  
 parent_num,  
 folder_name,  
 ex_code,  
 folder_level,
 create_date  
FROM  
 DOC_folder WITH(NOLOCK)  
WHERE  
 parent_num = @folder_num  
order by folder_name

GO