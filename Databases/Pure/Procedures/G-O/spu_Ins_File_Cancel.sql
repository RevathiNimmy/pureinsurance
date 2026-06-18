SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Ins_File_Cancel'
GO


CREATE PROCEDURE spu_Ins_File_Cancel
    @insurance_file_cnt int
AS


DECLARE @CancelledID int
DECLARE @InsuranceFolderCnt int

DECLARE @AgentUnderwriter varchar(1)

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = ""
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = "A"
BEGIN
    SELECT @CancelledID = (SELECT insurance_file_type_id
                 FROM insurance_file_type
                WHERE insurance_file_type.code = 'MTAPERMCAN')

    /* update file type and version number */

    UPDATE insurance_file
    SET insurance_file_type_id = @CancelledID,
    policy_version = policy_version + 1
    WHERE insurance_file_cnt = @insurance_file_cnt
END
ELSE
BEGIN
    /* Get the ID for Cancelled insurance_file_status */
    SELECT @CancelledID = (SELECT insurance_file_status_id
                 FROM insurance_file_status
                WHERE insurance_file_status.code = 'CAN')

    SELECT @InsuranceFolderCnt =  (SELECT insurance_folder_cnt
                    FROM insurance_file
                    WHERE insurance_file_cnt = @insurance_file_cnt)

    UPDATE insurance_file
    SET insurance_file_status_id = @CancelledID
    WHERE insurance_folder_cnt = @InsuranceFolderCnt
END
GO


