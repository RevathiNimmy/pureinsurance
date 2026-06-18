SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_document_spooler_add'
GO
CREATE PROCEDURE spe_document_spooler_add  
    @document_spooler_id int OUTPUT,  
    @document_type_id int,  
    @party_cnt int,  
    @insurance_folder_cnt int,  
    @insurance_file_cnt int,  
    @claim_cnt int,  
    @description varchar(255),  
    @is_deletable tinyint,  
    @is_editable tinyint,  
    @created_by_id smallint,  
    @date_created datetime,  
    @modified_by_id smallint,  
    @date_modified datetime,  
    @times_printed int,  
    @times_archived int,  
    @printer varchar(255),  
    @spool_level_ind tinyint,  
    @source_id int,  
    @document_template_id int,  
    @is_client tinyint,
    @is_agent tinyint,
    @is_office tinyint,  
    @production_order int
  
AS  
BEGIN  
INSERT INTO document_spooler (  
    document_type_id,  
    party_cnt,  
    insurance_folder_cnt,  
    insurance_file_cnt,  
    claim_cnt,  
    description,  
    is_deletable,  
    is_editable,  
    created_by_id,  
    date_created,  
    modified_by_id,  
    date_modified,  
    times_printed,  
    times_archived,  
    printer,  
    spool_level_ind,  
    source_id,  
    document_template_id,
    is_client,
    is_agent,
    is_office,  
    production_order)  
VALUES (  
    @document_type_id,  
    @party_cnt,  
    @insurance_folder_cnt,  
    @insurance_file_cnt,  
    @claim_cnt,  
    @description,  
    @is_deletable,  
    @is_editable,  
    @created_by_id,  
    @date_created,  
    @modified_by_id,  
    @date_modified,  
    @times_printed,  
    @times_archived,  
    @printer,  
    @spool_level_ind,  
    @source_id,  
    @document_template_id,
    @is_client,
    @is_agent,
    @is_office,  
    @production_order)  
END  
BEGIN
SELECT @document_spooler_id = @@IDENTITY
END

GO

