
EXECUTE DDLDropProcedure 'spu_SIR_Background_Job_GetNext'
GO
CREATE PROCEDURE spu_SIR_Background_Job_GetNext (
	@background_job_id INT OUTPUT,
	@job_xml NVARCHAR(max) OUTPUT,
	@job_service NVARCHAR(255),
	@party_code VARCHAR(255) OUTPUT,
	@insurance_ref VARCHAR(255) OUTPUT,
	@claim_number VARCHAR(255) OUTPUT,
	@ljob_user_id INT OUTPUT,
	@wait_minutes_before_retry INT = 0,
	@loggedin_userid int OUTPUT,
	@loggedin_username VARCHAR(255)  OUTPUT,
	@loggedin_userpassword VARCHAR(255) OUTPUT

) AS BEGIN

	set nocount on
	declare @lID integer
	declare @RowCount as integer
	
	DECLARE @dtNow datetime
	set @dtNow = getdate()
	
	BEGIN TRY
	INSERT INTO Background_Job_InProcess
		SELECT TOP 1 background_job_id  , job_status
		FROM Background_Job WITH (NOLOCK)  
		WHERE job_status = 'W'  
		AND job_when_to_start < @dtNow  
		AND DateAdd(mi,@wait_minutes_before_retry,ISNULL(last_job_retry_time,0) ) <= GetDate()  
	ORDER BY job_when_to_start, background_job_id  
	END TRY
	BEGIN CATCH
		 RETURN
	END CATCH

	BEGIN TRANSACTION  

 -- Return the first ID from 'Background_Job_InProcess' with status 'W' found and update the Status to X.  
 
	 UPDATE Background_Job_InProcess    
		 SET @lID = Background_Job_InProcess.background_job_id,  
			 Background_Job_InProcess.job_status = 'X' 
		 FROM (  
			SELECT  TOP 1 background_job_id  
				FROM Background_Job_InProcess    
					WHERE job_status = 'W'  
						ORDER BY background_job_id  
				) AS Top1ID  
		 WHERE Background_Job_InProcess.background_job_id = Top1ID.background_job_id
    
	-- Update the Status to X in Background_Job.  
	 UPDATE Background_Job    
		SET Background_Job.job_started = @dtNow,  
		  Background_Job.job_status = 'X',  
		  Background_Job.job_service = @job_service  
		  WHERE Background_Job.background_job_id = @lID
  
	 SET @RowCount = @@rowcount  
  
	 COMMIT TRANSACTION 
	
	if @RowCount > 0 begin	
		-- Retun the Job ID & Job XML
		SELECT 	@background_job_id = b.background_job_id, 
				@job_xml = b.job_xml,
				@party_code = b.party_code,
				@insurance_ref = b.insurance_ref,
				@claim_number = b.claim_number,
				@ljob_user_id = b.job_user_id,
				@loggedin_userid = b.job_user_id,
				@loggedin_username = pmu.username,
				@loggedin_userpassword = pmu.password 
		FROM	Background_Job b with (nolock) 
		left join pmuser  pmu with (nolock) on pmu.user_id = b.job_user_id	 
		WHERE	background_job_id = @lID

		DELETE FROM Background_Job_InProcess WHERE background_job_id = @lID
	END
END
GO
