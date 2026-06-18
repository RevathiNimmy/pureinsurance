set quoted_identifier on set ansi_nulls on
go
execute DDLDropProcedure 'spu_SIR_Background_Job_complete'
go
CREATE PROCEDURE spu_SIR_Background_Job_complete (
	@background_job_id INT
)AS BEGIN
	UPDATE	Background_Job
	SET	job_status = 'C',
		job_completed = GETDATE()
	WHERE 	background_job_id = @background_job_id
END
GO
