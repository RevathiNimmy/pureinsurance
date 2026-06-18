SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Contract_Certainty_Update'
GO

CREATE PROCEDURE spu_Contract_Certainty_Update
    @insurance_file_cnt int,
    @from_event bit,
    @terms_agreed bit,
    @terms_agreed_date datetime,
    @inception_Date datetime,
    @policy_documents_issued_date datetime = null,
    @policy_documents_correct bit = null,
    @error_notification_date datetime = null
AS

BEGIN

    IF @from_event = 1
    BEGIN
        UPDATE Event_Insurance_File
        SET
            terms_agreed = @terms_agreed,
            terms_agreed_date = @terms_agreed_date,
            inception_Date = @inception_Date, 
            policy_documents_issued_date = @policy_documents_issued_date,
            policy_documents_correct = @policy_documents_correct,
            error_notification_date = @error_notification_date
        WHERE insurance_file_cnt = @insurance_file_cnt
    END
    ELSE
    BEGIN
        UPDATE Insurance_File
        SET
            terms_agreed = @terms_agreed,
            terms_agreed_date = @terms_agreed_date,
            inception_Date = @inception_Date, 
            policy_documents_issued_date = @policy_documents_issued_date,
            policy_documents_correct = @policy_documents_correct,
            error_notification_date = @error_notification_date
        WHERE insurance_file_cnt = @insurance_file_cnt
    END

END
GO