SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_event_log_add'
GO

CREATE PROCEDURE spe_event_log_add
    @event_cnt int OUTPUT,
    @party_cnt int,
    @insurance_folder_cnt int = null,
    @insurance_file_cnt int = null,
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
    @description varchar(8000),
    @old_party_type_id int,
    @event_log_subject_id int,
    @event_type_code varchar(10),
    @account_key int,
    @fsa_complaint_folder_cnt int,
    @short_description varchar(70),
    @priority_code varchar(10),
    @is_completed smallint,
    @peril_id int,
    @case_id int = NULL,
    @rtf_Text ntext = "",
    @is_manual_description int = NULL,
    @bg_id int  = Null,
    @batch_ID Int = NULL,
    @Document_Path Varchar(255) = NULL

AS

IF (@insurance_folder_cnt IS NULL OR @insurance_folder_cnt =0 ) AND @insurance_file_cnt > 0 
BEGIN
    SELECT @insurance_folder_cnt = insurance_folder_cnt FROM Insurance_file 
    WHERE insurance_file_cnt = @insurance_file_cnt
END
IF (@old_address_cnt =0 )
BEGIN
   set @old_address_cnt =NULL
END

IF (@new_address_cnt =0 )
BEGIN
   set @new_address_cnt =NULL
END

--PN75282(Paralled changes done for 75045)- Start
IF @party_cnt = 0 
BEGIN
       Select @party_cnt = insured_cnt From Insurance_file Where insurance_file_cnt = @insurance_file_cnt 
END
--PN75282(Paralled changes done for 75045)- End

IF (@event_type_id) is NULL
BEGIN
    IF @event_type_code is NOT NULL
    BEGIN
        SELECT @event_type_id = event_type_id FROM event_type WHERE code = @event_type_code
    END
END

IF @batch_ID = 0  
    SET @batch_ID = NULL

INSERT INTO event_log
(
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
    description,
    old_party_type_id,
    event_log_subject_id,
    account_key,
    fsa_complaint_folder_cnt,
    short_description,
    priority_code,
    is_completed,
    peril_id,
    case_id,
    rtf_text,
    is_manual_description,
    batch_id,
    bg_id,
    Document_Path
)
VALUES
(
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
    @description,
    @old_party_type_id,
    @event_log_subject_id,
    @account_key,
    @fsa_complaint_folder_cnt,
    @short_description,
    @priority_code,
    @is_completed,
    @peril_id,
    @case_id,
    @rtf_Text,
    @is_manual_description,
    @batch_Id,
    @bg_id,
    @Document_Path
)

SELECT @event_cnt = @@IDENTITY



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

