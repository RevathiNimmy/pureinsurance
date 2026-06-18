EXECUTE DDLDropProcedure 'spu_DOC_select_document'

GO
CREATE PROCEDURE spu_DOC_select_document
    @folder_num int ,  
    @filter varchar(52) ,  
    @access_level tinyint  
AS  
BEGIN  
 SELECT doc_num, doc_name, password, doc_type, create_date, ISNULL(dms.code, '')  
 FROM DOC_document  
	LEFT JOIN dme_migration_status dms ON dms.dme_migration_status_id = DOC_document.dme_migration_status_id
 WHERE folder_num = @folder_num  
 AND doc_name >= @filter  
 AND access_level >= @access_level  
 ORDER BY create_date DESC  
END
GO