SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Contract_Certainty_Select'
GO

CREATE PROCEDURE spu_Contract_Certainty_Select
    @insurance_file_cnt int,
    @from_event bit
AS

BEGIN

    IF @from_event = 1  
    BEGIN  
        SELECT  
            EIF.terms_agreed,  
            EIF.terms_agreed_date,  
            CASE 
                WHEN (SELECT code FROM insurance_file_status WHERE insurance_file_status_id=EIF.insurance_file_status_id )='REN' THEN EIF.cover_start_date  
                ELSE ISNULL(EIF.inception_Date,EIFO.inception_Date)  
            END,  
            EIF.policy_documents_issued_date,  
            EIF.policy_documents_correct,  
            EIF.error_notification_date  
        FROM Event_Insurance_File AS EIF  
        JOIN Event_Insurance_Folder AS EIFO
            ON EIF.insurance_folder_cnt = EIFO.insurance_folder_cnt
        WHERE EIF.insurance_file_cnt = @insurance_file_cnt  
    END  
    ELSE  
    BEGIN  
        SELECT  
            INF.terms_agreed,  
            INF.terms_agreed_date,  
            CASE  
                WHEN (SELECT code FROM insurance_file_status WHERE insurance_file_status_id=INF.insurance_file_status_id )='REN' THEN INF.cover_start_date  
                ELSE ISNULL(INF.inception_Date,INFO.inception_date)  
            END,  
            INF.policy_documents_issued_date,  
            INF.policy_documents_correct,  
            INF.error_notification_date  
        FROM Insurance_File AS INF
        JOIN Insurance_Folder AS INFO
            ON INF.insurance_folder_cnt = INFO.insurance_folder_cnt
        WHERE INF.insurance_file_cnt = @insurance_file_cnt  
    END  

END
GO