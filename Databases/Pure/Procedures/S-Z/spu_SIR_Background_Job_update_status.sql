set quoted_identifier on set ansi_nulls on
go
execute DDLDropProcedure 'spu_SIR_Background_Job_update_status'
go
CREATE PROCEDURE spu_SIR_Background_Job_update_status (
	@background_job_id INT,
	@job_status NVARCHAR(1)
) AS BEGIN

	UPDATE	Background_Job
	SET	job_status = @job_status
	WHERE 	background_job_id = @background_job_id

END
GO
