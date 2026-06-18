SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Batch_Renewal_Job_Code'
GO

CREATE PROCEDURE spu_SIR_Get_Batch_Renewal_Job_Code

	@batch_renewal_job_type	VARCHAR(20)

As
BEGIN

DECLARE @ReturnJobCode VARCHAR(20)
SET @ReturnJobCode ='0'

	IF UPPER(@batch_renewal_job_type) ='ACCEPTANCE'
	BEGIN
		SELECT TOP 1 @ReturnJobCode = RIGHT(BRJ.code,LEN(BRJ.code)-2) 
		FROM Batch_Renewal_Job BRJ
		WHERE BRJ.code LIKE 'RA%'
		ORDER BY BRJ.batch_renewal_job_id DESC
	END

	IF UPPER(@batch_renewal_job_type) ='INVITATION'
	BEGIN
		SELECT TOP 1 @ReturnJobCode = RIGHT(BRJ.code,LEN(BRJ.code)-2) 
		FROM Batch_Renewal_Job BRJ
		WHERE BRJ.code LIKE 'RI%'
		ORDER BY BRJ.batch_renewal_job_id DESC
	END

	IF UPPER(@batch_renewal_job_type) ='SELECTION'
	BEGIN
		SELECT TOP 1 @ReturnJobCode = RIGHT(BRJ.code,LEN(BRJ.code)-2) 
		FROM Batch_Renewal_Job BRJ
		WHERE BRJ.code LIKE 'RS%'
		ORDER BY BRJ.batch_renewal_job_id DESC
	END

	--Return JobCode
	SELECT @ReturnJobCode AS JOBCODE

END

GO
