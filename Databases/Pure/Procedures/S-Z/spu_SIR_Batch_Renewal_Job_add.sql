SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_add'
GO

CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_add
	@code 						VARCHAR(20),
	@description 				VARCHAR(255),  
	@sam_server 				VARCHAR(500),  
	@days_before_renewal_date	SMALLINT = 0,
	@is_active 					TINYINT,  
	@batch_renewal_job_type_id 	INT,  
	@renewal_docs_destination	TINYINT,
	@report_sort_order 			TINYINT,  
	@all_agents 				TINYINT,  
	@pmuser_id 					INT,
	@include_direct_policies	TINYINT,
	@run_extended_rule			TINYINT
AS  
BEGIN  


INSERT INTO Batch_Renewal_Job(  
	code,
	description,
	sam_server,
	days_before_renewal_date,
	is_active,
	batch_renewal_job_type_id,
	renewal_docs_destination,
	report_sort_order,
	all_agents,
	pmuser_id,
	date_created,
	date_updated,
	include_direct_policies,
	run_extended_rule
	)
VALUES (@code,
	@description,
	@sam_server,
	@days_before_renewal_date,
	@is_active,
	@batch_renewal_job_type_id,
	@renewal_docs_destination,
	@report_sort_order,
	@all_agents,
	@pmuser_id,
	GETDATE(),
	NULL,
	@include_direct_policies,
	@run_extended_rule		
	)
SELECT SCOPE_IDENTITY()
END

GO
