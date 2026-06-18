SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_document_spooler_upd'
GO

CREATE PROCEDURE spe_document_spooler_upd  
    @document_spooler_id int,  
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
    @source_id int,  
    @document_template_id int,
    @is_client tinyint,
    @is_agent tinyint,
    @is_office tinyint,  
    @production_order int
AS  
BEGIN  
UPDATE document_spooler  
    SET  
    document_type_id=@document_type_id,  
    party_cnt=@party_cnt,  
    insurance_folder_cnt=@insurance_folder_cnt,  
    insurance_file_cnt=@insurance_file_cnt,  
    claim_cnt=@claim_cnt,  
    description=@description,  
    is_deletable=@is_deletable,  
    is_editable=@is_editable,  
    created_by_id=@created_by_id,  
    date_created=@date_created,  
    modified_by_id=@modified_by_id,  
    date_modified=@date_modified,  
    times_printed=@times_printed,  
    times_archived=@times_archived,  
    source_id = @source_id,  
    document_template_id = @document_template_id,  
    is_client = @is_client,
    is_agent = @is_agent,
    is_office = @is_office,  
    production_order = @production_order
  
WHERE document_spooler_id = @document_spooler_id  
END  

GO

