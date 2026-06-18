SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Add_Agents_Linked_With_BatchRenewal'
GO

CREATE PROCEDURE spu_SIR_Add_Agents_Linked_With_BatchRenewal

	@BatchRenewalJobId	INT,
	@Party_Cnt 			BIGINT

As

	INSERT INTO Batch_Renewal_Job_Agents ([batch_renewal_job_id],[party_cnt]) VALUES (@BatchRenewalJobId,@Party_Cnt)

GO
