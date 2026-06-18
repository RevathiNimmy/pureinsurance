EXECUTE DDLDropProcedure 'spu_Upd_Doc_Migration_Status'

GO
CREATE PROCEDURE spu_Upd_Doc_Migration_Status 
    @doc_num int,
    @statusCode varchar(20)
AS  
BEGIN  
Declare @dme_migration_status_id INT 

Select @dme_migration_status_id = dme_migration_status_id From dme_migration_status Where Code = @statusCode

Update DOC_document Set dme_migration_status_id = @dme_migration_status_id Where doc_num = @doc_num 
END;

GO