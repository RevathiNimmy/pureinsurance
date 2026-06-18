SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Party_Service_Level'
GO

CREATE PROCEDURE spu_SIR_Get_Party_Service_Level
	@batch_renewal_job_id INT,
	@insurance_file_cnt INT,
	@service_level_id INT OUTPUT
As

BEGIN

declare @all_agents TINYINT
Select @all_agents = ISNULL(all_agents, 0) from Batch_Renewal_Job where batch_renewal_job_id = @batch_renewal_job_id and is_active = 1

if (@all_agents = 1)
BEGIN
	select @service_level_id = p.service_level_id from Insurance_File ifi
	inner join Party p on p.party_cnt = ifi.insured_cnt
	inner join Batch_Renewal_Job_Products brjp on brjp.product_id = ifi.product_id and brjp.batch_renewal_job_id = @batch_renewal_job_id
	inner join Batch_Renewal_Job_Branches brjb on brjb.source_id = ifi.source_id and brjb.batch_renewal_job_id = @batch_renewal_job_id
	where ifi.insurance_file_cnt = @insurance_file_cnt 

END
ELSE IF (@all_agents = 0)
BEGIN
	select @service_level_id = p.service_level_id from Insurance_File ifi
	inner join Party p on p.party_cnt = ifi.insured_cnt
	inner join Batch_Renewal_Job_Products brjp on brjp.product_id = ifi.product_id and brjp.batch_renewal_job_id = @batch_renewal_job_id
	inner join Batch_Renewal_Job_Branches brjb on brjb.source_id = ifi.source_id and brjb.batch_renewal_job_id = @batch_renewal_job_id
	inner join Batch_Renewal_Job_Agents brja on brja.party_cnt = ISNULL(ifi.lead_agent_cnt, brja.party_cnt) and brja.batch_renewal_job_id = @batch_renewal_job_id
	where ifi.insurance_file_cnt = @insurance_file_cnt 

END

END

GO
