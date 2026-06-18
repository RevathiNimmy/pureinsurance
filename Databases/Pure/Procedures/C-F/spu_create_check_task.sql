DDLDropProcedure 'spu_create_check_task'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_create_check_task
	@user_id int,
	@party_cnt int,
	@insurance_file_cnt int,
	@os_pmwrk_task_instance_cnt int
AS
DECLARE @PmwrkTaskGroupId int,
	@PmwrkTaskId int,
	@PartyType varchar(2),
	@Shortname varchar(20),
	@ResolvedName varchar(255),
	@Competency varchar(10),
	@CompetencyDesc varchar(255),
	@PolicyNumber varchar(30),
	@PmwrkTaskInstanceCnt int,
	@PartyCntKey int,
	@InsuranceFileCntKey int,
	@PartyTypeKey int,
	@ShortnameKey int,
	@ResolvedNameKey int,
	@Description varchar(255),
	@LazyUserName varchar(20),
	@LazyUserId int,
	@RiskGroup varchar(255),
	@InsuranceFileCnt int,
	@Customer varchar(255),
	@AlertedPmuserGroupId int,
	@AlertedUserId int,
	@TaskDescription varchar(255),
	@CreatedByUserId int

SELECT @InsuranceFileCnt = ISNULL(@insurance_file_cnt, 0)

--is this broking?
if EXISTS (SELECT value from hidden_options where branch_id = 1 and option_number = 1 and value = 'A')
BEGIN
	--is fsa compliance enabled?
	IF EXISTS (SELECT value from hidden_options where option_number = 61 and value = '1' )
	BEGIN


		IF @InsuranceFileCnt <> 0
		BEGIN
		
			SELECT @LazyUsername = (SELECT username FROM pmuser where [user_id] = @user_id)	

			SELECT @CreatedByUserId = @user_id 

			--get the competency for the user for the risk of the policy being processed
			--and policy number
			SELECT 	@Competency = fus.code,
				@CompetencyDesc = fus.[description],
				@PolicyNumber = ifi.insurance_ref,
				@RiskGroup = rg.[description]
			FROM 	insurance_file ifi
			JOIN	risk_code rc
			ON	ifi.risk_code_id = rc.risk_code_id
			JOIN	fsa_user_competency fuc
			ON	rc.risk_group_id = fuc.risk_group_id
			JOIN	fsa_user_status fus
			ON	fuc.fsa_user_status_id = fus.fsa_user_status_id
			AND	fuc.[user_id] = @user_id
			JOIN	risk_group rg 
			ON	rg.risk_group_id = rc.risk_group_id
			WHERE	ifi.insurance_file_cnt = @insurance_file_cnt
	
			--create task for supervisor to check details that user has done for a particular policy/risk
			--becuase that user has a competency level that is 'non technical' or 'under training'
			IF  (RTrim(@Competency) = 'NTEC' OR RTrim(@Competency) = 'TRN' OR @Competency = NULL)
			BEGIN
				--get details of party
				SELECT	@PartyType = pt.code,
				@Shortname = p.shortname,
				@ResolvedName = p.[name]
				FROM	party p
				JOIN	party_type pt
				ON	p.party_type_id = pt.party_type_id
				WHERE 	p.party_cnt = @party_cnt	
		
				SELECT @Customer = @ResolvedName + " - " + @PolicyNumber
				SELECT @PmwrkTaskGroupId = (SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'CLNTMGT')
				SELECT @PmwrkTaskId = (SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'PMRCLIENT')	
				SELECT @Description = 'ALERT! Policy requires checking, that user ''' + Rtrim(@LazyUsername) + ''''
				SELECT @Description = @Description + ' has created/updated, with competency level of ''' + RTrim(@CompetencyDesc) + ''''
				SELECT @Description = @Description + ' for risk group ''' + RTrim(@RiskGroup) + '''.'
				SELECT @AlertedPmuserGroupId = (SELECT pmuser_group_id FROM pmwrk_task_check WHERE pmwrk_task_id = 0)
				SELECT @AlertedUserId = (SELECT pmuser_id FROM pmwrk_task_check WHERE pmwrk_task_id = 0)

			END
		
		END
		ELSE
		BEGIN

			SELECT @CreatedByUserId = (SELECT [user_id] FROM pmuser WHERE username = 'siriuscomm')
			
			SELECT @TaskDescription = 	(	
							SELECT 	pwt.[description] 
							FROM 	pmwrk_task pwt 
							JOIN 	pmwrk_task_instance pwti
							ON	pwt.pmwrk_task_id = pwti.pmwrk_task_id
							WHERE 	pwti.pmwrk_task_instance_cnt = @os_pmwrk_task_instance_cnt
							)
			SELECT @LazyUserId = 	(	
							SELECT 	pwti.[user_id]
							FROM 	pmwrk_task pwt 
							JOIN 	pmwrk_task_instance pwti
							ON	pwt.pmwrk_task_id = pwti.pmwrk_task_id
							WHERE 	pwti.pmwrk_task_instance_cnt = @os_pmwrk_task_instance_cnt
							)
			SELECT @LazyUsername = (SELECT username FROM pmuser where [user_id] = @LazyUserid)	
			--DC140704 PN13369 add customer name from overdue task
			SELECT @Customer = (select customer from pmwrk_task_instance where pmwrk_task_instance_cnt =  @os_pmwrk_task_instance_cnt)
			SELECT @PmwrkTaskGroupId = (SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'COMMON')
			SELECT @PmwrkTaskId = (SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'MEMO')
			SELECT @Description = 'ALERT! User ''' + RTrim(@LazyUsername) + ''''
			SELECT @Description = @Description + ' has outstanding task for '''
			SELECT @Description = @Description + RTrim(@TaskDescription) + '''.'
			SELECT @AlertedPmuserGroupId = (SELECT pmuser_group_id FROM pmwrk_task_check WHERE pmwrk_task_id = (SELECT pmwrk_task_id FROM pmwrk_task_instance WHERE pmwrk_task_instance_cnt = @os_pmwrk_task_instance_cnt))
			SELECT @AlertedUserId = (SELECT pmuser_id FROM pmwrk_task_check WHERE pmwrk_task_id = (SELECT pmwrk_task_id FROM pmwrk_task_instance WHERE pmwrk_task_instance_cnt = @os_pmwrk_task_instance_cnt))

		END
	
	IF @Competency <> NULL OR @InsuranceFileCnt =0
	BEGIN
		--create diary entry task
		INSERT INTO pmwrk_task_instance
		(customer, 
		pmwrk_task_group_id, 
		pmwrk_task_id, 
		[description], 
		task_due_date, 
		pmuser_group_id, 
		[user_id], 
		task_status, 
		is_urgent, 
		date_created, 
		created_by_id, 
		last_modified, 
		modified_by_id, 
		is_visible, 
		workflow_information)
		VALUES
		(@Customer,  
		@PmwrkTaskGroupId, 
		@PmwrkTaskId, 
		@Description, 
		getdate(), 
		@AlertedPmuserGroupId, 
		@AlertedUserId, 
		0, 
		0, 
		getdate(), 
		@CreatedByUserId, 
		NULL, 
		NULL, 
		1, 
		NULL)
			
		SELECT @PmwrkTaskInstanceCnt = 	@@IDENTITY

		--create keys for diary entry
		IF @InsuranceFileCnt <> 0
		BEGIN

			--get key ids
			SELECT 	@PartyCntKey = pmnav_key_id
			FROM	pmnav_key
			WHERE 	name = 'party_cnt'
			SELECT 	@PartyTypeKey = pmnav_key_id
			FROM	pmnav_key
			WHERE 	name = 'party_type'
			SELECT 	@ShortnameKey = pmnav_key_id
			FROM	pmnav_key
			WHERE 	name = 'short_name'
			SELECT 	@ResolvedNameKey = pmnav_key_id
			FROM	pmnav_key
			WHERE 	name = 'resolved_name'
			SELECT 	@InsuranceFileCntKey = pmnav_key_id
			FROM	pmnav_key
			WHERE 	name = 'insurance_file_cnt'

			--key for party_cnt
			INSERT INTO pmwrk_task_inst_key
			(pmwrk_task_instance_cnt, pmnav_key_id, key_value)
			VALUES
			(@PmwrkTaskInstanceCnt,@PartyCntKey, @party_cnt)
			--key for insurance_file_cnt
			INSERT INTO pmwrk_task_inst_key
			(pmwrk_task_instance_cnt, pmnav_key_id, key_value)
			VALUES
			(@PmwrkTaskInstanceCnt, @InsuranceFileCntKey, @insurance_file_cnt)	
			--key for party_type
			INSERT INTO pmwrk_task_inst_key
			(pmwrk_task_instance_cnt, pmnav_key_id, key_value)
			VALUES
			(@PmwrkTaskInstanceCnt, @PartyTypeKey, @PartyType)	
			--key for shortname
			INSERT INTO pmwrk_task_inst_key
			(pmwrk_task_instance_cnt, pmnav_key_id, key_value)
			VALUES
			(@PmwrkTaskInstanceCnt, @ShortnameKey, @Shortname)
			--key for resolved_name
			INSERT INTO pmwrk_task_inst_key
			(pmwrk_task_instance_cnt, pmnav_key_id, key_value)
			VALUES
			(@PmwrkTaskInstanceCnt, @ResolvedNameKey, @ResolvedName)

		END
	END
	END
END
GO