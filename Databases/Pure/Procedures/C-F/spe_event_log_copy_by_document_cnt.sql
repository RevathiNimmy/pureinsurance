SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spe_event_log_copy_by_document_cnt'
GO


CREATE PROCEDURE spe_event_log_copy_by_document_cnt
    @document_cnt int,
    @event_date datetime,
    @description_prefix varchar(255),
    @userid int = null,
    @event_cnt int = null output
AS

    -- Get the original document archived event
    Select  @event_cnt = Null
    Select  @event_cnt = Min(event_cnt)
    From    event_log
    Where   document_cnt = @document_cnt

    -- Check we have an event to clone
    If IsNull(@event_cnt, 0) <> 0 Begin
        -- Clone the original event with new description and date
        Insert Into event_log (
                party_cnt,
                insurance_folder_cnt,
                insurance_file_cnt,
                claim_cnt,
                document_cnt,
                new_address_cnt,
                old_address_cnt,
                campaign_id,
                document_type_id,
                report_type_id,
                event_type_id,
                user_id,
                event_date,
                description,
                old_party_type_id,
                transaction_export_folder_cnt,
                account_key,
                event_log_subject_id,
                short_description,
                fsa_complaint_folder_cnt,
                priority_code)
        Select  party_cnt,
                insurance_folder_cnt,
                insurance_file_cnt,
                claim_cnt,
                document_cnt,
                new_address_cnt,
                old_address_cnt,
                campaign_id,
                document_type_id,
                report_type_id,
                event_type_id,
                isnull(@userid, user_id),
                @event_date,                        -- new event date
                @description_prefix + ' ' + description,   -- prefixed description
                old_party_type_id,
                transaction_export_folder_cnt,
                account_key,
                event_log_subject_id,
                short_description,
                fsa_complaint_folder_cnt,
                priority_code
        From    event_log
        Where   event_cnt = @event_cnt
    
        -- Get identity from new event
        Select  @event_cnt = @@Identity
    End

GO


