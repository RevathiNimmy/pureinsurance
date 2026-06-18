EXECUTE DDLDropProcedure 'spu_Sel_DME_Batch'

GO
CREATE PROCEDURE spu_Sel_DME_Batch 
    @batch_id int
AS  
BEGIN  
Select doc_num From DOC_document
	Inner Join dme_migration_status dms ON dms.dme_migration_status_id = DOC_document.dme_migration_status_id
	Where migration_id = @batch_id AND dms.code IN ('WIP', 'FAIL')
END;
GO  
