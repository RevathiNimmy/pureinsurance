EXECUTE DDLDropProcedure 'spu_Upd_Doc_In_Batch'

GO
CREATE PROCEDURE spu_Upd_Doc_In_Batch
    @batch_id int,
    @doc_Num int
AS  
BEGIN  
Declare @dme_migration_status_id INT 
Declare @migrated_status_id INT 

Select @dme_migration_status_id = dme_migration_status_id From dme_migration_status Where Code = 'WIP';
Select @migrated_status_id = dme_migration_status_id From dme_migration_status Where Code = 'COMPLETE';

-- update doc with batch_id if not already a part of batch
Update DOC_document
Set migration_id = @batch_id,
	dme_migration_status_id = @dme_migration_status_id -- 1 WIP, 2 Failed, 3 Completed, 0 or Null No Action; lookup dme_migration_status
	From DOC_document 
Where doc_num = @doc_Num AND ISNull(dme_migration_status_id, 0) NOT IN (@dme_migration_status_id, @migrated_status_id)
	AND datepart(YY, create_date) >= (datepart(YY, GETDATE()) - 2) -- up to 2 years in past from now

END;
GO 
