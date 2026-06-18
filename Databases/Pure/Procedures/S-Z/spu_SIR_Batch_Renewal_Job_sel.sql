SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_sel'
GO

CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_sel
	@batch_renewal_job_id	INT = NULL
As

BEGIN

IF ISNULL(@batch_renewal_job_id,0) > 0 
BEGIN
	SELECT 
		BRJ.batch_renewal_job_id,
		BRJ.code,
		BRJ.description,
		BRJ.sam_server,
		BRJ.days_before_renewal_date,	
		BRJ.is_active,
		BRJ.batch_renewal_job_type_id,
		BRJ.renewal_docs_destination,
		BRJ.report_sort_order,
		BRJ.all_agents,
		BRJ.pmuser_id,
		BRJ.date_created,
		BRJ.date_updated,
		U.username,
		BRJT.description,
		BRJ.include_direct_policies,
		ISNULL(BRJ.run_extended_rule ,0) run_extended_rule
		
	FROM Batch_Renewal_Job BRJ
	INNER JOIN PMUser U
	ON BRJ.pmuser_id=U.user_id
	INNER JOIN Batch_Renewal_Job_Type BRJT
	ON BRJ.batch_renewal_job_type_id = BRJT.batch_renewal_job_type_id
	WHERE BRJ.batch_renewal_job_id = @batch_renewal_job_id
END
ELSE
BEGIN
	SELECT 
		BRJ.batch_renewal_job_id,
		BRJ.code,
		BRJ.description,
		BRJ.sam_server,
		BRJ.days_before_renewal_date,	
		BRJ.is_active,
		BRJ.batch_renewal_job_type_id,
		BRJ.renewal_docs_destination,
		BRJ.report_sort_order,
		BRJ.all_agents,
		BRJ.pmuser_id,
		BRJ.date_created,
		BRJ.date_updated,
		U.username,
		BRJT.description,
		BRJ.include_direct_policies,
		ISNULL(BRJ.run_extended_rule ,0) run_extended_rule
		
	FROM Batch_Renewal_Job BRJ
	INNER JOIN PMUser U
	ON BRJ.pmuser_id=U.user_id
	INNER JOIN Batch_Renewal_Job_Type BRJT
	ON BRJ.batch_renewal_job_type_id = BRJT.batch_renewal_job_type_id
	ORDER BY BRJ.batch_renewal_job_id
END

END

GO
