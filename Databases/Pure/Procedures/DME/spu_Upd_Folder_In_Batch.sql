EXECUTE DDLDropProcedure 'spu_Upd_Folder_In_Batch'

GO
CREATE PROCEDURE spu_Upd_Folder_In_Batch 
    @batch_id int,
    @folder_Num int
AS  
BEGIN  
Declare @dme_migration_status_id INT 
Declare @migrated_status_id INT 

Select @dme_migration_status_id = dme_migration_status_id From dme_migration_status Where Code = 'WIP';
Select @migrated_status_id = dme_migration_status_id From dme_migration_status Where Code = 'COMPLETE';

With FolderTree As 
( 
Select child.folder_num, child.parent_num, child.folder_name, 0 as Depth
From DOC_folder child
Where folder_num = @folder_Num
Union All 
Select parent.folder_num, parent.parent_num, parent.folder_name, Depth + 1
From DOC_folder parent
    Join FolderTree
        On FolderTree.folder_num = parent.parent_num
) 
Update DOC_document
Set migration_id = @batch_id,
	dme_migration_status_id = @dme_migration_status_id -- 1 WIP, 2 Failed, 3 Completed, 0 or Null No Action; lookup dme_migration_status
From FolderTree ft
	Inner Join DOC_document dd ON dd.folder_num = ft.folder_num
Where ISNull(dme_migration_status_id, 0) NOT IN (@dme_migration_status_id, @migrated_status_id)
	AND datepart(YY, create_date) >= (datepart(YY, GETDATE()) - 2) -- up to 2 years in past from now

Select doc_num from DOC_document
	Where migration_id = @batch_id
	
END;
GO
