SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_log_add'
GO


CREATE PROCEDURE spu_Event_log_add
    @event_cnt int OUTPUT,
    @party_cnt int,
    @insurance_folder_cnt int,
    @insurance_file_cnt int,
    @claim_cnt int,
    @document_cnt int,
    @old_address_cnt int,
    @new_address_cnt int,
    @campaign_id int,
    @document_type_id int,
    @report_type_id int,
    @event_type_id int,
    @user_id int,
    @event_date datetime,
    @description varchar(255)
AS


BEGIN
INSERT INTO event_log (
    party_cnt,
    insurance_folder_cnt,
    insurance_file_cnt,
    claim_cnt,
    document_cnt,
    old_address_cnt,
    new_address_cnt,
    campaign_id,
    document_type_id,
    report_type_id,
    event_type_id,
    user_id,
    event_date,
    description)
VALUES (
    @party_cnt,
    @insurance_folder_cnt,
    @insurance_file_cnt,
    @claim_cnt,
    @document_cnt,
    @old_address_cnt,
    @new_address_cnt,
    @campaign_id,
    @document_type_id,
    @report_type_id,
    @event_type_id,
    @user_id,
    @event_date,

    @description)
END
BEGIN
SELECT @event_cnt = @@IDENTITY
END
GO


