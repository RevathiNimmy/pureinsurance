Set QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_document_template_sel'
GO

/************************************************************************/
/* 1.0  07/07/97  RFC  Original (Based on Original by SP)               */
/* 1.1  26/07/01  RWH  Added printer.                                   */
/* 1.2  09/04/2002 AK  Added chaser                                     */
/* 1.3  02/05/2005 RKS Added document_filter                            */
/************************************************************************/
CREATE PROCEDURE spe_document_template_sel
    @document_template_id int  
AS  
SELECT  
    document_template_id,  
    code,  
    description,  
    source_id,  
    document_type_id,  
    created_by_id,  
    date_created,  
    modified_by_id,  
    last_modified,  
    is_deleted,  
    slot_number,  
    risk_code_id,  
    risk_group_id,  
    is_editable_after_merging,  
    printer,  
    chaser_description,  
    document_filter,  
    copy_of_original,  
    original_document_template_id,  
    effective_date,  
    is_visible_from_web,  
	is_visible_from_client_manager,
	archive_with_no_print,
	email_as_body,
	spool_document,
	archive_as_text,
    document_template_group_id,
	document_template_sub_group_id,
	is_portal_internal_only,
	is_portal_selected_by_default,  
	archive_as_xml,
	CCMDocumentTemplate,
	email_sub_template_code, 
	email_attachment_template_code
FROM document_template  
WHERE document_template_id = @document_template_id  
GO

