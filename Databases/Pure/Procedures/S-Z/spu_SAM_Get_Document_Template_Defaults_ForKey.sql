SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_SAM_Get_Document_Template_Defaults_ForKey'
GO
CREATE PROCEDURE spu_SAM_Get_Document_Template_Defaults_ForKey
	@document_template_id VARCHAR(10)
AS
SELECT	T.document_template_id,
	T.code [document_template_code],
	T.description [document_template_description],
	G.document_template_group_id,
	G.code [document_template_group_code],
	G.description [document_template_group_description],
	S.document_template_sub_group_id,
	S.code [document_template_sub_group_code],
	S.description [document_template_sub_group_description],
	T.is_portal_internal_only,
	T.is_portal_selected_by_default,
	T.email_sub_template_code,  
	T.email_attachment_template_code 
FROM	Document_Template T
LEFT JOIN Document_Template_Group G ON G.document_template_group_id=T.document_template_group_id
LEFT JOIN Document_Template_Sub_Group S ON S.document_template_sub_group_id=T.document_template_sub_group_id
WHERE	T.document_template_id=@document_template_id
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO