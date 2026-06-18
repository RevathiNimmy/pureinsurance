SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_document_template_add'
GO
/************************************************************************/  
/* 1.0  06/08/1997 RFC Original (Based on SP Original)                  */  
/* 1.1  26/07/2001 RWH Added printer field to enable a specific printer */  
/*                      to be stored against a template.                */  
/* 1.2  09/04/2002 AK  Added chaser                                     */  
/* 1.3  02/05/2005 RKS Added document_filter                            */  
  
/************************************************************************/  
CREATE PROCEDURE spe_document_template_add  
    @document_template_id int OUTPUT ,  
    @code char(20) ,  
    @description varchar(255) ,  
    @source_id int ,  
    @document_type_id int ,  
    @created_by_id smallint ,  
    @date_created datetime ,  
    @modified_by_id smallint ,  
    @last_modified datetime ,  
    @is_deleted tinyint ,  
    @slot_number int ,  
    @risk_code_id int ,  
    @risk_group_id int ,  
    @is_editable_after_merging tinyint,  
    @printer varchar(255),  
    @chaser varchar(255),  
    @document_filter varchar(50),  
    @copy_of_original tinyint,  
    @original_document_template_id int,  
    @effective_date datetime  = NULL,  
    @is_visible_from_web tinyint = 0,  
    @is_visible_from_client_manager tinyint =0,
    @archive_with_no_print tinyint = 0,
    @email_as_body tinyint = 0,
    @spool_document tinyint =0,
    @archive_as_text tinyint =0,
    @document_template_group_id int,
	@document_template_sub_group_id int,
	@is_portal_internal_only tinyint=0,
	@is_portal_selected_by_default tinyint=0,  
	@archive_as_xml tinyint =0,
	@CCMDocumentName varchar(255),
	@email_sub_template_code varchar(50),
	@email_attachment_template_code varchar(250),
	@UserId int = NULL,
	@UniqueId Varchar(50) = NULL,
	@ScreenHierarchy Varchar(500) = NULL
AS  
BEGIN  

IF @copy_of_original = 1
	SELECT @document_template_id = MIN(document_template_id)-1 FROM document_template
ELSE BEGIN
    SELECT @document_template_id = MAX(document_template_id)+1 FROM document_template
IF @document_template_id IS NULL  
		SELECT @document_template_id = 1
END
  
IF @original_document_template_id = 0  
    SELECT @original_document_template_id = NULL  
  
IF @document_template_group_id = 0
  SELECT @document_template_group_id = NULL

IF @document_template_sub_group_id = 0	
	SELECT @document_template_sub_group_id = NULL
  
SET @effective_date = ISNULL(@effective_date, GETDATE())  
  
IF @document_template_id = 0
	SELECT  @document_template_id = -1  

INSERT INTO document_template (  
    document_template_id ,  
    code ,  
    description ,  
    source_id ,  
    document_type_id ,  
    created_by_id ,  
    date_created ,  
    modified_by_id ,  
    last_modified ,  
    is_deleted ,  
    slot_number ,  
    risk_code_id ,  
    risk_group_id ,  
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
	email_attachment_template_code,
	UserId,
	UniqueId,
	ScreenHierarchy
	)
 
VALUES (  
    @document_template_id,  
    @code,  
    CASE WHEN @original_document_template_id IS NOT NULL THEN @description + '_' + CAST(@document_template_id as VARCHAR) ELSE @description END,
    @source_id,  
    @document_type_id,  
    @created_by_id,  
    @date_created,  
    @modified_by_id,  
    @last_modified,  
    @is_deleted,  
    @slot_number,  
    @risk_code_id,  
    @risk_group_id,  
    @is_editable_after_merging,  
    @printer,  
    @chaser,  
    @document_filter,  
    @copy_of_original,  
    @original_document_template_id,  
    @effective_date,  
    @is_visible_from_web,  
    @is_visible_from_client_manager,
    @archive_with_no_print,
    @email_as_body ,
    @spool_document,
	@archive_as_text,
    @document_template_group_id,
	@document_template_sub_group_id,
	@is_portal_internal_only,
	@is_portal_selected_by_default,
	@archive_as_xml,
	@CCMDocumentName,
	@email_sub_template_code,
	@email_attachment_template_code,
	@UserId,
	@UniqueId,
	@ScreenHierarchy
)  
END  
GO

