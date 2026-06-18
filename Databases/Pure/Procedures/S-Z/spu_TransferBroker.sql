SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_TransferBroker'
GO

CREATE PROCEDURE spu_TransferBroker
						@RenewalInsuranceFileCnt int,
        				@TransferToPartyCnt int
AS

BEGIN
	Begin Transaction

    DECLARE @PMUserID AS SMALLINT
    DECLARE @PartyCnt AS INT
    DECLARE @InsuranceFolderCnt AS INT
    DECLARE @OldBrokerCode AS VARCHAR(20)
    DECLARE @NewBrokerCode AS VARCHAR(20)

    -- Get the old Broker Code
    SELECT  @OldBrokerCode=RTRIM(P.shortname),
            @PartyCnt=I.insured_cnt,
            @InsuranceFolderCnt=I.insurance_folder_cnt,
            @PMUserID=ISNULL(IFS.modified_by_id,IFS.created_by_id)
    FROM    Party P
    JOIN    Insurance_File I ON I.lead_agent_cnt=P.party_cnt
    JOIN    Insurance_File_System IFS ON IFS.insurance_file_cnt=I.insurance_file_cnt
    WHERE   I.insurance_file_cnt = @RenewalInsuranceFileCnt

	IF @@ERROR <> 0
		Goto Catch

		-- Deleting Old Sub Agent from new Broker
		DELETE from insurance_file_agent from insurance_file_agent IFA 
		INNER JOIN Party_Relationship PR on IFA.party_cnt=PR.relation_cnt 
		INNER JOIN Insurance_File I on PR.party_cnt=I.lead_agent_cnt  
		WHERE IFA.insurance_file_cnt=@RenewalInsuranceFileCnt and I.insurance_file_cnt=@RenewalInsuranceFileCnt


	-- transfer renewal version to new broker
	UPDATE Insurance_file 
    SET lead_agent_cnt = @TransferToPartyCnt,
        business_type_id=CASE WHEN @TransferToPartyCnt IS NULL THEN 1 ELSE business_type_id END
    WHERE insurance_file_cnt = @RenewalInsuranceFileCnt

	IF @@ERROR <> 0
		Goto Catch

	-- transfer renewal version to new broker and reset renewal status type id to original 
	-- (broker_xfer_status_type_id = original value set by renewal selection)
	UPDATE  Renewal_Status 
	SET     renewal_status_type_id = broker_xfer_status_type_id,
			lead_agent_cnt = @TransferToPartyCnt
	WHERE   renewal_insurance_file_cnt = @RenewalInsuranceFileCnt

	IF @@ERROR <> 0
		Goto Catch

    -- Recalculate Agent Commission
    EXEC spu_sir_agent_commission_calc @RenewalInsuranceFileCnt, 'REN'

    -- Get the new Broker Code
    IF @TransferToPartyCnt IS NULL
        SELECT  @NewBrokerCode='Direct Business'
    ELSE
        SELECT  @NewBrokerCode=RTRIM(P.shortname)
        FROM    Party P
        WHERE   P.party_cnt = @TransferToPartyCnt

	IF @@ERROR <> 0
		Goto Catch

    -- Insert an Event Log entry
    INSERT INTO event_log(party_cnt, insurance_folder_cnt, insurance_file_cnt, claim_cnt, 
        document_cnt, new_address_cnt, old_address_cnt, campaign_id, 
        document_type_id, report_type_id, event_type_id, user_id, event_date, 
        description, old_party_type_id, transaction_export_folder_cnt, 
        account_key, event_log_subject_id, short_description, 
        fsa_complaint_folder_cnt, Priority_Code, Is_Completed, Sticky_top, Sticky_left)
    VALUES(@PartyCnt, @InsuranceFolderCnt, @RenewalInsuranceFileCnt, NULL, NULL, NULL, 
        NULL, NULL, NULL, NULL, 5, @PMUserID, GETDATE(), 
        'Broker Transfer from '+@OldBrokerCode+' to '+@NewBrokerCode, 0, NULL, NULL, 
        NULL, NULL, NULL, NULL, 1, NULL, NULL)

   	IF @@ERROR <> 0
		Goto Catch   
	
	Goto Finally

	Catch:
		Rollback Transaction
		Return 0
		
	Finally:
		Commit Transaction	
		Return 1

END
Go