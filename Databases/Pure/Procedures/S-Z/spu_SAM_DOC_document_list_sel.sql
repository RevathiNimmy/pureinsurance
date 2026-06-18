SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_DOC_document_list_sel'
GO

CREATE PROCEDURE spu_SAM_DOC_document_list_sel
(
@insurance_folder_cnt int
)
AS

DECLARE @SQL varchar(2000)

IF exists(select * FROM syscolumns sc  WHERE object_id('doc_document') = sc.id AND sc.name = 'document_template_id')
BEGIN 
	--Start-(Arul Stephen)-(Bug Fixing -PN55772) 
	SELECT @SQL = 'SELECT doc_num, doc_name,doc_document.create_date ' 
	--End-(Arul Stephen)-(Bug Fixing -PN55772)  
	SELECT @SQL = @SQL + 'FROM doc_document '
	SELECT @SQL = @SQL + 'JOIN doc_folder ON doc_folder.folder_num=doc_document.folder_num '
	SELECT @SQL = @SQL + 'LEFT OUTER JOIN	document_template ON document_template.document_template_id=doc_document.document_template_id '
	SELECT @SQL = @SQL + 'WHERE doc_folder.ex_code=''' + CONVERT(varchar(20), @insurance_folder_cnt) + ''' '
	SELECT @SQL = @SQL + 'AND doc_folder.folder_level=2 '
	SELECT @SQL = @SQL + 'AND (ISNULL(document_template.is_visible_from_web,0)=1 '
	SELECT @SQL = @SQL + '	   OR ISNULL(doc_document.visible_from_web,0)=1) ' 

END
ELSE
BEGIN 
	--Start-(Arul Stephen)-(Bug Fixing -PN55772)  
	SELECT @SQL = 'SELECT doc_num, doc_name,doc_document.create_date,visible_from_web '
	--End-(Arul Stephen)-(Bug Fixing -PN55772) 
	SELECT @SQL = @SQL + 'FROM doc_document '
	SELECT @SQL = @SQL + 'JOIN doc_folder ON doc_folder.folder_num=doc_document.folder_num '
	SELECT @SQL = @SQL + 'WHERE doc_folder.ex_code=''' + CONVERT(varchar(20), @insurance_folder_cnt) + ''' '
	SELECT @SQL = @SQL + 'AND doc_folder.folder_level=2 '

END
	EXECUTE (@SQL)

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


