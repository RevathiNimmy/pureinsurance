SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Sources_List_For_PickList'
GO

CREATE PROCEDURE spu_SIR_Get_Sources_List_For_PickList

	@BatchRenewalJobId	INT = NULL

As
BEGIN

	SELECT source_id,description,0 
	FROM Source 
	WHERE is_deleted = 0 AND
	source_id NOT IN (SELECT source_id FROM Batch_Renewal_Job_Branches WHERE batch_renewal_job_id = @BatchRenewalJobId)  
	UNION ALL SELECT BRJB.source_id,  
	S.description,1  
	FROM Batch_Renewal_Job_Branches BRJB
	LEFT JOIN Source S ON BRJB.source_id = S.source_id  
	WHERE BRJB.batch_renewal_job_id = @BatchRenewalJobId 

END

GO
