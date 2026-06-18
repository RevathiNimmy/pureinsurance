SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Task_Log_Add'
GO


CREATE PROCEDURE spu_SirRen_Task_Log_Add
    @insurance_folder_cnt int,
    @process_id int,
    @process_code varchar(30),
    @status int,
    @message varchar(255)
AS


BEGIN
    INSERT INTO Renewal_Task_Log
    (
        process_id,
        process_code,
        insurance_folder_cnt,
        insurance_file_cnt,
        policy_no,
        party_cnt,
        resolved_name,
        date,
        status,
        error_log
    )
    SELECT
        @process_id,
        @process_code,
        @insurance_folder_cnt,
        RC.old_insurance_file_cnt,
        I.insurance_ref,
        RC.party_cnt,
        P.resolved_name,
        GetDate(),
        @status,
        @message
      FROM Renewal_Control RC,
        Insurance_File I,
        Party P
      WHERE RC.insurance_folder_cnt = @insurance_folder_cnt
    AND I.insurance_file_cnt = RC.old_insurance_file_cnt
    AND P.party_cnt = RC.party_cnt
END
GO


