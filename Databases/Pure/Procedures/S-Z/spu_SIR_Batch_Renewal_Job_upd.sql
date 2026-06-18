SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Batch_Renewal_Job_upd'
GO

CREATE PROCEDURE spu_SIR_Batch_Renewal_Job_upd

	@batch_renewal_job_id	 		INT,
	@code 							VARCHAR(20),
	@description 					VARCHAR(255),  
	@sam_server 					VARCHAR(500),  
	@days_before_renewal_date		SMALLINT = 0,
	@is_active 						TINYINT,  
	@batch_renewal_job_type_id 		INT,  
	@renewal_docs_destination		TINYINT,
	@report_sort_order 				TINYINT,  
	@all_agents 					TINYINT,  
	@pmuser_id 						INT,
	@include_direct_policies  	 	TINYINT,
	@run_extended_rule				TINYINT
	        
AS  
BEGIN  

	UPDATE Batch_Renewal_Job
	SET 
	code = @code ,
	description = @description,
	sam_server = @sam_server,
	days_before_renewal_date = @days_before_renewal_date,
	is_active = @is_active,
	batch_renewal_job_type_id = @batch_renewal_job_type_id,
	renewal_docs_destination = @renewal_docs_destination,
	report_sort_order = @report_sort_order,
	all_agents = @all_agents,
	pmuser_id = @pmuser_id,
	date_updated = GETDATE(),
	include_direct_policies = @include_direct_policies,
	run_extended_rule = @run_extended_rule
	WHERE batch_renewal_job_id = @batch_renewal_job_id
END

GO
