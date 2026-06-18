set quoted_identifier on set ansi_nulls on
go
execute DDLDropProcedure 'spu_SIR_Background_Job_update'
go
CREATE PROCEDURE spu_SIR_Background_Job_update (
	@background_job_id INT,
	@description NVARCHAR(255),
	@job_xml NVARCHAR(max),
	@job_when_to_start DATETIME, 
	@job_status NVARCHAR(1),
	@job_user_id SMALLINT
) AS BEGIN

	IF @job_when_to_start IS NULL BEGIN
		SELECT @job_when_to_start = job_when_to_start
		FROM Backgroup_Job
		WHERE background_job_id = @background_job_id
	END

	UPDATE 	Background_Job 
	SET	Description = @description,
		job_xml = @job_xml,
		job_status = @job_status,
		job_when_to_start = @job_when_to_start,
		job_user_id = @job_user_id
	WHERE background_job_id = @background_job_id

END
GO
