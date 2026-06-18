SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_complaintaction'
GO


CREATE PROCEDURE spu_wp_complaintaction
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @complaint_action_date DATETIME,
    @complaint_action_comment VARCHAR(8000),
    @complaint_action_type VARCHAR(100),
    @complaint_action_user VARCHAR(255)

DECLARE c_cursor CURSOR SCROLL KEYSET READ_ONLY FOR
    SELECT
        fcf.date_modified,
        fcf.comment,
        fat.description,
        pmu.username
    FROM fsa_complaint_folder com
    JOIN fsa_complaint_file fcf 
        ON com.fsa_complaint_folder_cnt = fcf.fsa_complaint_folder_cnt
    JOIN fsa_complaint_actiontype fat 
        ON fcf.fsa_complaint_actiontype_id = fat.fsa_complaint_actiontype_id
    JOIN pmuser pmu 
        ON fcf.user_id = pmu.user_id
    WHERE com.reference = @DocumentRef
    ORDER BY fcf.complaint_version ASC

OPEN c_cursor

FETCH ABSOLUTE @Instance1 FROM c_cursor INTO
    @complaint_action_date,
    @complaint_action_comment,
    @complaint_action_type,
    @complaint_action_user

CLOSE c_cursor
DEALLOCATE c_cursor

SELECT
    @complaint_action_date 'complaint_action_date',
    @complaint_action_comment 'complaint_action_comment',
    @complaint_action_type 'complaint_action_type',
    @complaint_action_user 'complaint_action_user'

GO


