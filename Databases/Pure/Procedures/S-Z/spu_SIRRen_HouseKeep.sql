SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_HouseKeep'
GO

CREATE PROCEDURE spu_SIRRen_HouseKeep
    @Insurance_Folder_Cnt INT,
    @user_id INT = 1
AS

/*This stored procedure will remove unwanted records for a given insurance folder in following order:
    - Renewal Control
    - Events (just removes link to renewal policy versions)
    - Policy Versions (all renewal and what if policy versions including the risk data for those)
    - Task Log entries 
*/

DECLARE 
    @RenewalEventType INT,
    @RenewalDate DATETIME,
    @StartDate DATETIME,
    @PartyCnt INT,
    @sText VARCHAR(255),
    @Insurance_File_Cnt INT,
    @GIS_Policy_Link_Id INT
    
/*Get the event_type_id for Renewal, we do not delete RENPOLCHG*/
SELECT 
    @RenewalEventType = event_type_id 
FROM event_type 
WHERE code = 'RENEWAL'

/*Get the renewal dates from the policy*/
SELECT
    @PartyCnt = insurance_holder_cnt 
FROM insurance_folder
WHERE insurance_folder_cnt = @Insurance_Folder_Cnt

SELECT 
    @StartDate = cover_start_date, 
    @RenewalDate = renewal_date     
FROM insurance_file
WHERE insurance_file_cnt = 
    (
        SELECT 
            MAX(i.insurance_file_cnt)
        FROM insurance_file i
        JOIN insurance_file_type t
            ON t.insurance_file_type_id = i.insurance_file_type_id
            AND t.code IN ('RENEWAL', 'RENEWALWIF')
        WHERE i.insurance_folder_cnt = @Insurance_Folder_Cnt
    )

IF @StartDate IS NULL
BEGIN
    SELECT @sText = 'Renewal selected for housekeeping' 
END
ELSE
BEGIN
    SELECT @sText = 'Renewal (' 
        + (SELECT CONVERT(VARCHAR(12), @StartDate, 103)) + ' to '
        + (SELECT CONVERT(VARCHAR(12), @RenewalDate, 103)) 
        + ') selected for housekeeping'
END

/*Add an entry to the event_log*/
INSERT event_log 
(
    party_cnt,
    insurance_folder_cnt, 
    event_type_id, 
    user_id, 
    event_date, 
    description, 
    old_party_type_id
)
VALUES 
(
    @PartyCnt, 
    @Insurance_Folder_Cnt, 
    @RenewalEventType, 
    @user_id, 
    GETDATE(), 
    @sText, 
    0
)

/*Now we will have to delete Renewal Control, as otherwise foreign-key constraint will not let us delete insurance file*/
DELETE 
FROM Renewal_Control 
WHERE Insurance_Folder_Cnt = @Insurance_Folder_Cnt

/* Get a list of all the insurance files, which are supposed to be deleted */
DECLARE IFL_cursor CURSOR FORWARD_ONLY STATIC FOR
    SELECT 
        i.insurance_file_cnt
    FROM insurance_file i
    JOIN insurance_file_type t
        ON t.insurance_file_type_id = i.insurance_file_type_id
        AND t.code IN ('RENEWAL', 'RENEWALWIF')
    WHERE i.insurance_folder_cnt = @Insurance_Folder_Cnt

OPEN IFL_cursor

FETCH NEXT FROM IFL_cursor INTO @Insurance_File_Cnt

WHILE @@FETCH_STATUS = 0 
BEGIN
    
    /*Get the GIS_Policy_Link_Id for this policy version*/
    SELECT 
        @GIS_Policy_Link_Id = GIS_Policy_Link_Id
    FROM GIS_Policy_Link
    WHERE Insurance_File_Cnt = @Insurance_File_Cnt

    /*Remove all of the risk data for this GIS_Policy_Link_Id*/
    EXEC spu_SIRRen_DeleteGISObject @GIS_Policy_Link_Id

    /*Remove the GIS_Policy_Link_Id record*/
    DELETE 
    FROM GIS_Policy_Link 
    WHERE GIS_Policy_Link_Id = @GIS_Policy_Link_Id

    /*Keep the event notes on this policy version but remove the link to this policy version so that the note is just linked to insurance folder*/
    UPDATE Event_Log 
    SET insurance_file_cnt = NULL 
    WHERE Insurance_File_Cnt = @Insurance_File_cnt

    /*Remove records for dependencies of this policy version*/
    DELETE 
    FROM insurance_file_system 
    WHERE insurance_file_cnt = @Insurance_File_cnt
    
    DELETE 
    FROM policy_agents 
    WHERE insurance_file_cnt = @Insurance_File_cnt
    
    DELETE 
    FROM policy_fee 
    WHERE insurance_file_cnt = @insurance_file_cnt
    
    DELETE 
    FROM insurance_file_risk_link 
    WHERE insurance_file_cnt = @insurance_file_cnt
    
    /*Remove this policy version*/
    DELETE 
    FROM insurance_file 
    WHERE insurance_file_cnt = @Insurance_File_cnt

    FETCH NEXT FROM IFL_cursor INTO @Insurance_File_Cnt
END

CLOSE IFL_cursor
DEALLOCATE IFL_cursor

/*Remove all renewal task logs for this policy*/
DELETE 
FROM renewal_task_log 
WHERE insurance_folder_cnt = @Insurance_Folder_Cnt

GO

