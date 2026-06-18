SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Del_Branches_Linked_With_BatchRenewal'
GO

CREATE PROCEDURE spu_SIR_Del_Branches_Linked_With_BatchRenewal
	@BatchRenewalJobId	INT
As

	DELETE Batch_Renewal_Job_Branches 
	WHERE batch_renewal_job_id = @BatchRenewalJobId 

GO
