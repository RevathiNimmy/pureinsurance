SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_document_template_upd'
GO

/************************************************************************/
/* 1.0  06/08/1997 RFC Original (Based on SP Original)                  */
/* 1.1  26/07/2001 RWH Added printer.                                   */
/* 1.2  09/04/2002 AK  Added chaser                                     */
/* 1.3  02/05/2005 RKS Added document_filter                            */
/************************************************************************/
CREATE PROCEDURE spe_document_template_upd
    @document_template_id int,  
    @code char(10),  
    @description varchar(255),  
    @source_id int,  
    @document_type_id int,  
    @created_by_id smallint,  
    @date_created datetime,  
    @modified_by_id smallint,  
    @last_modified datetime,  
    @is_deleted tinyint,  
    @slot_number int,  
    @risk_code_id int,  
    @risk_group_id int,  
    @is_editable_after_merging tinyint,  
    @printer varchar(255),  
    @chaser varchar(255),  
    @document_filter varchar(50),  
    @copy_of_original tinyint,  
    @original_document_template_id int,  
    @effective_date datetime = NULL,  
    @is_visible_from_web tinyint = 0,  
    @is_visible_from_client_manager tinyint = 0,
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
  
IF @original_document_template_id = 0  
    SELECT @original_document_template_id = NULL  
  
SET @effective_date = ISNULL(@effective_date, GETDATE())  
  
DECLARE @CCMOldDocumentName AS VARCHAR(255)
DECLARE @CCMNewDocumentName AS VARCHAR(255)

SELECT @CCMOldDocumentName = ISNULL(CCMDocumentTemplate,'') FROM Document_Template WHERE document_template_id = @document_template_id
SELECT @CCMNewDocumentName = ISNULL(@CCMDocumentName,'')

IF @CCMNewDocumentName <> @CCMOldDocumentName  
BEGIN
	UPDATE Document_Template
	SET DocumentFieldsetFieldList = NULL,
	CCMRefreshDate = NULL
	WHERE document_template_id = @document_template_id
END
  
UPDATE document_template  
    SET  
    code=@code,  
    description=@description,  
    source_id=@source_id,  
    document_type_id=@document_type_id,  
    created_by_id=@created_by_id,  
    date_created=@date_created,  
    modified_by_id=@modified_by_id,  
    last_modified=@last_modified,  
    is_deleted=@is_deleted,  
    slot_number=@slot_number,  
    risk_code_id=@risk_code_id,  
    risk_group_id=@risk_group_id,  
    is_editable_after_merging=@is_editable_after_merging,  
    printer = @printer,  
    chaser_description = @chaser,  
    document_filter = @document_filter,  
    copy_of_original = @copy_of_original,  
    original_document_template_id = @original_document_template_id,  
    effective_date = @effective_date,  
    is_visible_from_web = @is_visible_from_web,  
    is_visible_from_client_manager=@is_visible_from_client_manager,
    archive_with_no_print=@archive_with_no_print ,
    email_as_body=@email_as_body ,
    spool_document=@spool_document, 
	archive_as_text=@archive_as_text,
    document_template_group_id=@document_template_group_id,
	document_template_sub_group_id=@document_template_sub_group_id,
	is_portal_internal_only=@is_portal_internal_only,
	is_portal_selected_by_default=@is_portal_selected_by_default,  
	archive_as_xml=@archive_as_xml,
	CCMDocumentTemplate = @CCMDocumentName,
	email_sub_template_code = @email_sub_template_code,
	email_attachment_template_code  = @email_attachment_template_code,
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
WHERE document_template_id = @document_template_id  
END  
GO
