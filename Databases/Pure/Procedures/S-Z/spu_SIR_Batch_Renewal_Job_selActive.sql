SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_selActive'
GO

CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_selActive

	@batchrenewaljobcode	VARCHAR(5)

As
BEGIN

	SELECT batch_renewal_job_id FROM BATCH_RENEWAL_JOB WHERE LEFT(code,2) = UPPER(@batchrenewaljobcode)
	AND is_active = 1

END

GO
