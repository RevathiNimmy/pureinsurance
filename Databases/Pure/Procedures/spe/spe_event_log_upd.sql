SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_event_log_upd'
GO

CREATE PROCEDURE spe_event_log_upd
    @event_cnt int ,
    @party_cnt int ,
    @insurance_folder_cnt int ,
    @insurance_file_cnt int ,
    @claim_cnt int ,
    @document_cnt int ,
    @new_address_cnt int ,
    @old_address_cnt int ,
    @campaign_id int ,
    @document_type_id int ,
    @event_type_id int ,
    @user_id smallint ,
    @event_date datetime ,
    @description varchar(8000),
    @old_party_type_id int,
    @event_log_subject_id int, 
    @event_type_code varchar(10),
    @account_key int,
    @priority_code varchar(10),
    @is_completed smallint,
    @peril_id int,
    @Batch_Id int = NULL

AS

BEGIN

IF (@event_type_id) is NULL
BEGIN
    IF @event_type_code is NOT NULL
    BEGIN
        SELECT @event_type_id = event_type_id FROM event_type WHERE code = @event_type_code
    END
END

UPDATE event_log
SET party_cnt = @party_cnt,
    insurance_folder_cnt = @insurance_folder_cnt,
    insurance_file_cnt = @insurance_file_cnt,
    claim_cnt = @claim_cnt,
    document_cnt = @document_cnt,
    new_address_cnt = @new_address_cnt,
    old_address_cnt = @old_address_cnt,
    campaign_id = @campaign_id,
    document_type_id = @document_type_id,
    event_type_id = @event_type_id,
    user_id = @user_id,
    event_date = @event_date,
    description = @description,
    old_party_type_id = @old_party_type_id,
    event_log_subject_id = @event_log_subject_id,
    account_key = @account_key,
    priority_code = @priority_code,
    is_completed = @is_completed,
    peril_id = @peril_id,
    Batch_Id = @Batch_Id
WHERE event_cnt = @event_cnt

END

GO

