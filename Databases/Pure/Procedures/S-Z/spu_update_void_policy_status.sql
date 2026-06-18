SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_update_void_policy_status'
GO

CREATE PROCEDURE spu_update_void_policy_status  
    @insurance_file_cnt INT,  
    @user_id INT, 
    @trans_type VARCHAR(20)
AS
BEGIN
    DECLARE @insurance_file_type_id INT
    DECLARE @insurance_file_status_id INT
    DECLARE @insurance_folder_cnt INT

    IF ISNULL(@trans_type, '') = ''
    BEGIN
        SELECT @trans_type = ttype.code
        FROM insurance_file_system AS ifilesys
        INNER JOIN transaction_type AS ttype 
            ON ttype.transaction_type_id = ifilesys.last_trans_type_id
        WHERE ifilesys.insurance_file_cnt = @insurance_file_cnt
    END

    SELECT @insurance_folder_cnt = insurance_folder_cnt
    FROM insurance_file
    WHERE insurance_file_cnt = @insurance_file_cnt

    IF @trans_type = 'REN'
    BEGIN
        SELECT @insurance_file_type_id = insurance_file_type_id
        FROM insurance_file_type WITH (NOLOCK)
        WHERE code = 'VOIDRENREP'

        UPDATE insurance_file
        SET insurance_file_status_id = NULL
        WHERE insurance_file_cnt = (
            SELECT TOP 1 IFI.insurance_file_cnt
            FROM insurance_file AS IFI
            INNER JOIN insurance_file_status AS IFS 
                ON IFS.insurance_file_status_id = IFI.insurance_file_status_id
            WHERE IFS.code = 'REP'
              AND IFI.insurance_folder_cnt = @insurance_folder_cnt
              AND IFI.insurance_file_cnt < @insurance_file_cnt
            ORDER BY IFI.insurance_file_cnt DESC
        )
    END
    ELSE IF @trans_type = 'MTR'
    BEGIN
      
        SELECT @insurance_file_type_id = insurance_file_type_id
        FROM insurance_file_type WITH (NOLOCK)
        WHERE code = 'VOIDREP'

        EXEC spu_cancel_all_versions @insurance_file_cnt
    END
    ELSE
    BEGIN
        SELECT @insurance_file_type_id = insurance_file_type_id
        FROM insurance_file_type WITH (NOLOCK)
        WHERE code = 'VOIDREP'
    END

    SELECT @insurance_file_status_id = insurance_file_status_id
    FROM insurance_file_status WITH (NOLOCK)
    WHERE code = 'VOID'

    UPDATE insurance_file
    SET original_insurance_file_type_id = insurance_file_type_id,
        insurance_file_status_id = @insurance_file_status_id,
        insurance_file_type_id = @insurance_file_type_id
    WHERE insurance_file_cnt = @insurance_file_cnt

    INSERT INTO event_log (
        party_cnt, insurance_folder_cnt, insurance_file_cnt, claim_cnt, document_cnt,
        old_address_cnt, new_address_cnt, campaign_id, document_type_id, report_type_id,
        event_type_id, user_id, event_date, description, old_party_type_id,
        event_log_subject_id, account_key, fsa_complaint_folder_cnt, short_description,
        priority_code, is_completed, peril_id, case_id, rtf_text, is_manual_description,
        batch_id, bg_id
    )
    SELECT TOP 1
        party_cnt, insurance_folder_cnt, @insurance_file_cnt, claim_cnt, document_cnt,
        old_address_cnt, new_address_cnt, campaign_id, document_type_id, report_type_id,
        5, @user_id, GETDATE(), (ISNULL(description, '') + ' and voided'),
        old_party_type_id, event_log_subject_id, account_key, fsa_complaint_folder_cnt,
        short_description, priority_code, is_completed, peril_id, case_id, rtf_text,
        is_manual_description, batch_id, bg_id
    FROM event_log
    WHERE insurance_file_cnt = @insurance_file_cnt
      AND event_type_id = 5
    ORDER BY event_cnt DESC
END
GO
