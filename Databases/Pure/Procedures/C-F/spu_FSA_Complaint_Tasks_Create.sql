SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_Tasks_Create'
GO
CREATE PROCEDURE spu_FSA_Complaint_Tasks_Create 
    @FSA_complaint_folder_cnt int,
    @UserId int

AS
BEGIN

    DECLARE @PartyCnt INTEGER
    DECLARE @ClaimId INTEGER
    DECLARE @InsuranceFileCnt INTEGER
    DECLARE @Shortname VARCHAR(30)
    DECLARE @Client VARCHAR(255)
    DECLARE @PartyType VARCHAR(255)
    DECLARE @PartyTypeCode VARCHAR(30)
    DECLARE @RestartStep INTEGER
    DECLARE @Customer VARCHAR(255)
    DECLARE @PmwrkTaskGroupId INTEGER
    DECLARE @PmwrkTaskId INTEGER
    DECLARE @PmwrkUserGroupId INTEGER
    DECLARE @PartyCntKeyId INTEGER
    DECLARE @BranchKeyId INTEGER
    DECLARE @InsuranceFileCntKeyId INTEGER
    DECLARE @PartyTypeKeyId INTEGER
    DECLARE @ShortnameKeyId INTEGER
    DECLARE @ClientKeyId INTEGER
    DECLARE @ClientCodeKeyId INTEGER
    DECLARE @ClientNameKeyId INTEGER
    DECLARE @ClaimCntKeyId INTEGER
    DECLARE @RestartStepKeyId INTEGER
    DECLARE @LongNameKeyId INTEGER
    DECLARE @AgentCntKeyId INTEGER
    DECLARE @ClientCntKeyId INTEGER
    DECLARE @AgentOnlyKeyId INTEGER
    DECLARE @TaskCustomerKeyId INTEGER
    DECLARE @DateOfBirthKeyId INTEGER
    DECLARE @SwiftPartyKeyId INTEGER
    DECLARE @PartyStatusKeyId INTEGER
    DECLARE @AddressLine1KeyId INTEGER
    DECLARE @AddressLine2KeyId INTEGER
    DECLARE @PostCodeKeyId INTEGER
    DECLARE @AddressCntKeyId INTEGER
    DECLARE @HandlerTypeCntKeyId INTEGER
    DECLARE @AddressLine3KeyId INTEGER
    DECLARE @FSAComplaintFolderCntKeyId INTEGER
    DECLARE @IncludeComplaintsKeyId INTEGER
    DECLARE @IsIncludeClosedBranchCheckKeyId INTEGER
    DECLARE @FSAComplaintTypeKeyId INTEGER
    DECLARE @InsuranceHolderKeyId INTEGER
    DECLARE @FSAComplaintFolderReferenceKeyId INTEGER
    DECLARE @ComplaintMode VARCHAR(255)
    DECLARE @MaxId INTEGER
    DECLARE @DateOFBirth DATETIME
    DECLARE @Reference VARCHAR(50)
    DECLARE @SwiftPartyId INTEGER
    
    --SET UP KEY VALUES

    SELECT @PMWrkTaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'MTNCMPLT' )
    SELECT @PartyCnt = ( select party_cnt from fsa_complaint_folder where fsa_complaint_folder_cnt = @FSA_complaint_folder_cnt )
    SELECT @ClaimId = ( select claim_id from fsa_complaint_folder where fsa_complaint_folder_cnt = @FSA_complaint_folder_cnt )
    SELECT @InsuranceFileCnt = ( select insurance_file_cnt from fsa_complaint_folder where fsa_complaint_folder_cnt = @FSA_complaint_folder_cnt )
    SELECT @Shortname = ( SELECT shortname FROM party WHERE party_cnt = @PartyCnt )
    SELECT @Client = ( SELECT resolved_name FROM party WHERE party_cnt = @PartyCnt )
    SELECT @PartyType = ( SELECT pt.description FROM party_type pt JOIN party p ON p.party_type_id = pt.party_type_id WHERE p.party_cnt = @PartyCnt )
    SELECT @PartyTypeCode = ( SELECT pt.code FROM party_type pt JOIN party p ON p.party_type_id = pt.party_type_id WHERE p.party_cnt = @PartyCnt )  
    SELECT @RestartStep = 2
    SELECT @Reference = ( SELECT reference FROM fsa_complaint_folder WHERE fsa_complaint_folder_cnt = @FSA_complaint_folder_cnt )
    SELECT @Customer = rtrim(@Client) + ' - ' + Rtrim(@Reference)
    SELECT @PmwrkTaskGroupId = (SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SYSADMIN')
    SELECT @PmwrkUserGroupId = (SELECT pmuser_group_id FROM pmuser_group WHERE code = 'SYSADMIN')
    SELECT @DateOfBirth = ( SELECT date_of_birth FROM party_lifestyle WHERE party_cnt = @PartyCnt AND category = 1 )
    SELECT @SwiftPartyId = ( SELECT swift_party_id FROM party WHERE party_cnt = @PartyCnt )

    --GET KEY IDs AND SET KEY VALUES

    SELECT @ComplaintMode = 'Maintain Complaint - Compliance Requirement - Follow Up Within 5 days'

    INSERT INTO pmwrk_task_instance
    (customer, pmwrk_task_group_id, pmwrk_task_id, description, task_due_date, pmuser_group_id, [user_id], task_status, is_urgent, date_created, created_by_id, last_modified, modified_by_id, is_visible, workflow_information, source_id, pmwrk_task_action_type_id, task_outcome_date, task_outcome_id )
    VALUES
    (@Customer, @PmwrkTaskGroupId, @PmwrkTaskId, @ComplaintMode, GetDate() + 4, @PmwrkUserGroupId, @UserId, 2, 0, GetDate(), @UserId , NULL , NULL, 1, NULL, NULL, NULL, NULL, NULL)
    
    SELECT @MaxId = ( SELECT MAX(pmwrk_task_instance_cnt) FROM pmwrk_task_instance )
    
    SELECT @PartyCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'party_cnt')  
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @PartyCntKeyId, @PartyCnt)

    SELECT @InsuranceFileCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'insurance_file_cnt')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @InsuranceFileCntKeyId, ISNULL(@InsuranceFileCnt,0))

    SELECT @PartyTypeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'party_type')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @PartyTypeKeyId, @PartyType)

    SELECT @ShortnameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'shortname')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ShortnameKeyId, @Shortname)

    SELECT @ClientKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_key')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientKeyId, @PartyCnt)

    SELECT @ClientCodeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_code')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientCodeKeyId, @Shortname)

    SELECT @ClientNameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_name')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientNameKeyId, @Client)

    SELECT @ClaimCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'claim_cnt')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClaimCntKeyId, ISNULL(@ClaimId, 0))

    SELECT @RestartStepKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'restart_step')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @RestartStepKeyId, @RestartStep)

    SELECT @LongNameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'long_name')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @LongNameKeyId, @Client)

    SELECT @AgentCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'agent_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @AgentCntKeyId, @PartyCnt) 

    SELECT @ClientCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientCntKeyId, @PartyCnt)    

    SELECT @AgentOnlyKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'agent_only')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @AgentOnlyKeyId, 1)    

    SELECT @TaskCustomerKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'TaskCustomer')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @TaskCustomerKeyId, @Customer) 

    IF @DateOfBirth IS NOT NULL
    BEGIN

        SELECT @DateOfBirthKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'date_of_birth')
        INSERT INTO pmwrk_task_inst_key 
        (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
        VALUES
        (@MAXId, @DateOfBirthKeyId, @DateOfBirth)

    END

    SELECT @HandlerTypeCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'handler_type')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @HandlerTypeCntKeyId, @PartyTypeCode)

    SELECT @AddressLine3KeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'address_line_3')

    SELECT @FSAComplaintFolderCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_folder_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintFolderCntKeyId, @FSA_complaint_folder_cnt)
    
    SELECT @IncludeComplaintsKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'include_complaints')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @IncludeComplaintsKeyId, 1)

    SELECT @IsIncludeClosedBranchCheckKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'is_include_closed_branch_Chkd')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @IsIncludeClosedBranchCheckKeyId, 'False')
    
        SELECT @FSAComplaintTypeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_type')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintTypeKeyId, 2)

    SELECT @InsuranceHolderKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'insurance_holder')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @InsuranceHolderKeyId, @Shortname)

    SELECT @FSAComplaintFolderReferenceKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_folder_reference')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintFolderReferenceKeyId, @Reference)
    
    --GET KEY IDs AND SET KEY VALUES

    SELECT @ComplaintMode = 'Maintain Complaint - Compliance Requirement - Follow Up Within 28 days'

    INSERT INTO pmwrk_task_instance
    (customer, pmwrk_task_group_id, pmwrk_task_id, description, task_due_date, pmuser_group_id, user_id, task_status, is_urgent, date_created, created_by_id, last_modified, modified_by_id, is_visible, workflow_information, source_id, pmwrk_task_action_type_id, task_outcome_date, task_outcome_id )
    VALUES
    (@Customer, @PmwrkTaskGroupId, @PmwrkTaskId, @ComplaintMode, GetDate() + 27, @PmwrkUserGroupId, @UserId, 2, 0, GetDate(), @UserId , NULL ,NULL, 1, NULL, NULL, NULL, NULL, NULL)
    
    SELECT @MaxId = ( SELECT MAX(pmwrk_task_instance_cnt) FROM pmwrk_task_instance )
    
    SELECT @PartyCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'party_cnt')  
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @PartyCntKeyId, @PartyCnt)

    SELECT @InsuranceFileCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'insurance_file_cnt')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @InsuranceFileCntKeyId, ISNULL(@InsuranceFileCnt,0))

    SELECT @PartyTypeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'party_type')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @PartyTypeKeyId, @PartyType)

    SELECT @ShortnameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'shortname')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ShortnameKeyId, @Shortname)

    SELECT @ClientKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_key')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientKeyId, @PartyCnt)

    SELECT @ClientCodeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_code')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientCodeKeyId, @Shortname)

    SELECT @ClientNameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_name')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientNameKeyId, @Client)

    SELECT @ClaimCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'claim_cnt')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClaimCntKeyId, ISNULL(@ClaimId, 0))

    SELECT @RestartStepKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'restart_step')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @RestartStepKeyId, @RestartStep)

    SELECT @LongNameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'long_name')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @LongNameKeyId, @Client)

    SELECT @AgentCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'agent_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @AgentCntKeyId, @PartyCnt) 

    SELECT @ClientCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientCntKeyId, @PartyCnt)    

    SELECT @AgentOnlyKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'agent_only')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @AgentOnlyKeyId, 1)    

    SELECT @TaskCustomerKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'TaskCustomer')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @TaskCustomerKeyId, @Customer) 

    IF @DateOfBirth IS NOT NULL
    BEGIN

        SELECT @DateOfBirthKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'date_of_birth')
        INSERT INTO pmwrk_task_inst_key 
        (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
        VALUES
        (@MAXId, @DateOfBirthKeyId, @DateOfBirth)

    END

    SELECT @HandlerTypeCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'handler_type')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @HandlerTypeCntKeyId, @PartyTypeCode)

    SELECT @AddressLine3KeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'address_line_3')

    SELECT @FSAComplaintFolderCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_folder_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintFolderCntKeyId, @FSA_complaint_folder_cnt)
    
    SELECT @IncludeComplaintsKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'include_complaints')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @IncludeComplaintsKeyId, 1)

    SELECT @IsIncludeClosedBranchCheckKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'is_include_closed_branch_Chkd')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @IsIncludeClosedBranchCheckKeyId, 'False')
    
        SELECT @FSAComplaintTypeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_type')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintTypeKeyId, 2)

    SELECT @InsuranceHolderKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'insurance_holder')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @InsuranceHolderKeyId, @Shortname)

    SELECT @FSAComplaintFolderReferenceKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_folder_reference')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintFolderReferenceKeyId, @Reference)

    --GET KEY IDs AND SET KEY VALUES

    SELECT @ComplaintMode = 'Maintain Complaint - Compliance Requirement - Follow Up Within 56 days'

    INSERT INTO pmwrk_task_instance
    (customer, pmwrk_task_group_id, pmwrk_task_id, description, task_due_date, pmuser_group_id, user_id, task_status, is_urgent, date_created, created_by_id, last_modified, modified_by_id, is_visible, workflow_information, source_id, pmwrk_task_action_type_id, task_outcome_date, task_outcome_id )
    VALUES
    (@Customer, @PmwrkTaskGroupId, @PmwrkTaskId, @ComplaintMode, GetDate() + 55, @PmwrkUserGroupId, @UserId, 2, 0, GetDate(), @UserId , NULL ,NULL, 1, NULL, NULL, NULL, NULL, NULL)
    
    SELECT @MaxId = ( SELECT MAX(pmwrk_task_instance_cnt) FROM pmwrk_task_instance )
    
    SELECT @PartyCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'party_cnt')  
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @PartyCntKeyId, @PartyCnt)

    SELECT @InsuranceFileCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'insurance_file_cnt')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @InsuranceFileCntKeyId, ISNULL(@InsuranceFileCnt,0))

    SELECT @PartyTypeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'party_type')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @PartyTypeKeyId, @PartyType)

    SELECT @ShortnameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'shortname')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ShortnameKeyId, @Shortname)

    SELECT @ClientKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_key')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientKeyId, @PartyCnt)

    SELECT @ClientCodeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_code')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientCodeKeyId, @Shortname)

    SELECT @ClientNameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_name')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientNameKeyId, @Client)

    SELECT @ClaimCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'claim_cnt')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClaimCntKeyId, ISNULL(@ClaimId, 0))

    SELECT @RestartStepKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'restart_step')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @RestartStepKeyId, @RestartStep)

    SELECT @LongNameKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'long_name')
        INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @LongNameKeyId, @Client)

    SELECT @AgentCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'agent_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @AgentCntKeyId, @PartyCnt) 

    SELECT @ClientCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'client_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @ClientCntKeyId, @PartyCnt)    

    SELECT @AgentOnlyKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'agent_only')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @AgentOnlyKeyId, 1)    

    SELECT @TaskCustomerKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'TaskCustomer')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @TaskCustomerKeyId, @Customer) 

    IF @DateOfBirth IS NOT NULL 
    BEGIN

        SELECT @DateOfBirthKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'date_of_birth')
        INSERT INTO pmwrk_task_inst_key 
        (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
        VALUES
        (@MAXId, @DateOfBirthKeyId, @DateOfBirth)

    END

    SELECT @HandlerTypeCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'handler_type')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @HandlerTypeCntKeyId, @PartyTypeCode)

    SELECT @AddressLine3KeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'address_line_3')

    SELECT @FSAComplaintFolderCntKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_folder_cnt')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintFolderCntKeyId, @FSA_complaint_folder_cnt)
    
    SELECT @IncludeComplaintsKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'include_complaints')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @IncludeComplaintsKeyId, 1)

    SELECT @IsIncludeClosedBranchCheckKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'is_include_closed_branch_Chkd')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @IsIncludeClosedBranchCheckKeyId, 'False')
    
        SELECT @FSAComplaintTypeKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_type')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintTypeKeyId, 2)

    SELECT @InsuranceHolderKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'insurance_holder')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @InsuranceHolderKeyId, @Shortname)

    SELECT @FSAComplaintFolderReferenceKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE name = 'fsa_complaint_folder_reference')
    INSERT INTO pmwrk_task_inst_key 
    (pmwrk_task_instance_cnt, pmnav_key_id, key_value)
    VALUES
    (@MAXId, @FSAComplaintFolderReferenceKeyId, @Reference)

END
GO