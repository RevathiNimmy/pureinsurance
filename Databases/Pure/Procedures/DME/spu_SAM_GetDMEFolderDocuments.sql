SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropPROCEDURE 'spu_SAM_GetDMEFolderDocuments'
GO

CREATE PROCEDURE spu_SAM_GetDMEFolderDocuments
	@folder_num int,
	@category_id int = NULL,
	@sub_category_id int = NULL,
	@doc_name_search nvarchar(255) = NULL
AS
SELECT
	d.doc_num,
	d.doc_name,
	d.doc_type,
	d.create_date,
	dbo.GetDMEPath(d.folder_num) as FolderPath,
	di.scan_user AS uploaded_by,
	dtg.description AS category,
	dtsg.description AS sub_category
FROM
	DOC_document d WITH(NOLOCK)
LEFT JOIN DOC_doc_info di WITH(NOLOCK)
	ON d.doc_num = di.doc_num
LEFT JOIN Document_Template_Group dtg WITH(NOLOCK)
	ON d.document_template_group_id = dtg.document_template_group_id
LEFT JOIN Document_Template_Sub_Group dtsg WITH(NOLOCK)
	ON d.document_template_sub_group_id = dtsg.document_template_sub_group_id
WHERE
	d.folder_num = @folder_num
	AND (@category_id IS NULL OR d.document_template_group_id = @category_id)
	AND (@sub_category_id IS NULL OR d.document_template_sub_group_id = @sub_category_id)
	AND (NULLIF(@doc_name_search, '') IS NULL OR d.doc_name LIKE '%' + @doc_name_search + '%')
ORDER BY
	d.create_date DESC


GO
