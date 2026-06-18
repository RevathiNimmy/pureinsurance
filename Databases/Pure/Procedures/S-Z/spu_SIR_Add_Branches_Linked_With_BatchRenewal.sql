SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Add_Branches_Linked_With_BatchRenewal'
GO

CREATE PROCEDURE spu_SIR_Add_Branches_Linked_With_BatchRenewal

	@BatchRenewalJobId	INT,
	@source_id 			INT

As
	INSERT INTO Batch_Renewal_Job_Branches ([batch_renewal_job_id],[source_id]) VALUES (@BatchRenewalJobId,@source_id)

GO
