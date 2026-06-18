set quoted_identifier on set ansi_nulls on
go
execute DDLDropProcedure 'spu_SIR_Background_Job_add'
go
CREATE PROCEDURE spu_SIR_Background_Job_add (
	@background_job_id INT OUTPUT,
	@description NVARCHAR(255),
	@job_xml NVARCHAR(max),
	@job_when_to_start DATETIME, 
	@job_user_id SMALLINT
) AS BEGIN

	DECLARE @dtNow datetime
	
	SET @dtNow = getdate()
	
	IF @job_when_to_start IS NULL BEGIN
		SET @job_when_to_start = @dtNow
	END
	
	-- Calculate the Party, Policy and Claim References now
	-- to ensure they are consistent between job time submit
	-- and processing (Policy, could be made Live in-between)
	DECLARE @Folderpath  Varchar(250)
	DECLARE	@party_cnt int
	DECLARE @party_code varchar(255)
	DECLARE @insurance_file_cnt int
	DECLARE @insurance_ref varchar(255)
	DECLARE @claim_id int
	DECLARE @claim_number varchar(255)
	DECLARE @xml XML
 	
	SET @Folderpath='GenerateDefaultPath'
 	SET @xml = CAST(@job_xml AS XML)
	
	SELECT	@party_cnt=ps.p.value('@value', 'int')
	FROM	@xml.nodes('/BACKGROUND_JOB/JOB/PARAMETERS/PARAMETER') AS ps(p)
	WHERE	ps.p.value('@name', 'varchar(max)')='PartyCnt'

	SELECT	@insurance_file_cnt=ps.p.value('@value', 'int')
	FROM	@xml.nodes('/BACKGROUND_JOB/JOB/PARAMETERS/PARAMETER') AS ps(p)
	WHERE	ps.p.value('@name', 'varchar(max)')='InsuranceFileCnt'

	SELECT	@claim_id=ps.p.value('@value', 'int')
	FROM	@xml.nodes('/BACKGROUND_JOB/JOB/PARAMETERS/PARAMETER') AS ps(p)
	WHERE	ps.p.value('@name', 'varchar(max)')='ClaimID'
	
	IF @party_cnt<>0 BEGIN
		SELECT	@party_code=shortname
		FROM	Party  (NOLOCK)
		WHERE	party_cnt=@party_cnt

		If @description ='GenerateDefaultPath' AND @claim_id =0 AND  @insurance_file_cnt = 0
		BEGIN
			IF EXISTS(Select * FROM Background_Job WHERE party_code = @party_code)
			Return 1
		END
	END
	
	IF @insurance_file_cnt<>0 BEGIN
		SELECT	@insurance_ref=insurance_ref
		FROM	Insurance_File (NOLOCK)
		WHERE	insurance_file_cnt=@insurance_file_cnt
		
		IF @party_cnt=0 BEGIN
			SELECT @party_code = shortname
			FROM Party (NOLOCK)
			INNER JOIN Insurance_File ON Insurance_File.insured_cnt = Party.party_cnt
			WHERE Insurance_File.insurance_file_cnt = @insurance_file_cnt
		END
		If @description ='GenerateDefaultPath'
		BEGIN
			IF EXISTS(Select * FROM Background_Job WHERE party_code = @party_code AND insurance_ref =@insurance_ref AND @claim_id =0)
			Return 1
	END
	END
	
	IF @claim_id<>0 BEGIN
		SELECT	@claim_number=Claim_Number
		FROM	Claim (NOLOCK)
		WHERE	Claim_id=@claim_id
		
		IF @insurance_file_cnt=0 BEGIN
			SELECT	@insurance_ref=Policy_Number
			FROM	Claim (NOLOCK)
			WHERE	Claim_id=@claim_id
		END
		If @description ='GenerateDefaultPath'
		BEGIN
			IF EXISTS(Select * FROM Background_Job WHERE party_code = @party_code AND claim_number=@Claim_Number )
			Return 1
		END
	END
		
	--<JOB jobtype="EXWRKITEM">
	If ISNULL(@claim_id,0) = 0
	BEGIN
			SELECT	@claim_number = ps.p.value('@VALUE', 'varchar(max)')
			FROM	@xml.nodes('/BACKGROUND_JOB/JOB/KEYDATA/KEY') AS ps(p)
			WHERE	ps.p.value('@NAME', 'varchar(255)')='ClaimNo'
	END		
		
	INSERT INTO Background_Job(
		description,
		job_xml,
		job_status,
		job_created,
		job_when_to_start,
		job_started,
		job_completed,
		job_expiry,
		failure_description,
		job_user_id, 
		party_code,
		insurance_ref,
		claim_number
		)
	VALUES (
		@description,
		@job_xml,
		'W',
		@dtNow,
		@job_when_to_start,
		NULL,
		NULL,
		DATEADD(month,1,@dtNow),	-- expires 1 month after creation
		NULL,
		@job_user_id,
		@party_code,
		@insurance_ref,
		@claim_number)

	SET @background_job_id = @@IDENTITY

END
GO
