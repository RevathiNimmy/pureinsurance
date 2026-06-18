SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Del_Agents_Linked_With_BatchRenewal'
GO

CREATE PROCEDURE spu_SIR_Del_Agents_Linked_With_BatchRenewal

	@BatchRenewalJobId	INT

As

	DELETE Batch_Renewal_Job_Agents 
	WHERE batch_renewal_job_id = @BatchRenewalJobId 

GO
