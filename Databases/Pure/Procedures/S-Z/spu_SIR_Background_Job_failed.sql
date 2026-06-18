set quoted_identifier on set ansi_nulls on
go
execute DDLDropProcedure 'spu_SIR_Background_Job_failed'
go
CREATE PROCEDURE spu_SIR_Background_Job_failed (
	@background_job_id INT,
	@failure_description nvarchar(1000)
)AS BEGIN

	UPDATE	Background_Job
	SET	job_status = 'F',
		job_completed = GETDATE(),
		failure_description = @failure_description
	WHERE 	background_job_id = @background_job_id

END
GO
