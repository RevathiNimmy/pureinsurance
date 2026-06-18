SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Add_Products_Linked_With_BatchRenewal'
GO

CREATE PROCEDURE spu_SIR_Add_Products_Linked_With_BatchRenewal

	@BatchRenewalJobId	INT,
	@Product_id 		INT

As

	INSERT INTO Batch_Renewal_Job_Products ([batch_renewal_job_id],[product_id]) VALUES (@BatchRenewalJobId,@Product_id)

GO