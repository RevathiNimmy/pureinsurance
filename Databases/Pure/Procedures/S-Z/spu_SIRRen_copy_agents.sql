EXECUTE DDLDropProcedure 'spu_SIRRen_copy_agents'
GO

/******************************************************************************************************************
*
*  Name: 	spu_SIRRen_copy_agents
*
*  Description: 	Copy agents from old insurance_file_cnt to new insurance_file_cnt
*
*  History: 	12/11/2002 - DC Created
*  02/02/2004   Tracy Richards. As part of renewals, this was duplicating the actions of spu_copy_policy_agents, 
*               called from bSIRInsuranceFile. Didn't want to remove it in case it was used elsewhere, 
*               but now this checks that the Policy's agent has not already been copied before trying to do so.
*******************************************************************************************************************/

CREATE PROCEDURE spu_SIRRen_copy_agents
    @old_insurance_file_cnt int,
    @new_insurance_file_cnt int
AS

IF EXISTS (SELECT * FROM policy_agents WHERE insurance_file_cnt = @old_insurance_file_cnt)
BEGIN

	INSERT INTO 	policy_agents (
			insurance_file_cnt,
			agent_cnt,
			agent_count,
			agent_commission_percentage,
			agent_commission_amount,
			agent_commission_value,
			is_minimum_brokerage,
			override_rate_table,
			apply_perc_to_prem_or_comm)		
			SELECT 	@new_insurance_file_cnt, 
					pa.agent_cnt,  
					pa.agent_count,
					pa.agent_commission_percentage,
					pa.agent_commission_amount,
					pa.agent_commission_value,
					pa.is_minimum_brokerage,
					pa.override_rate_table,
					pa.apply_perc_to_prem_or_comm
			FROM		policy_agents pa
			WHERE 	insurance_file_cnt = @old_insurance_file_cnt
            --Check this record doesn't already exist
            AND     NOT EXISTS (SELECT  * 
                            FROM    policy_agents pa2
                            WHERE   pa2.insurance_file_cnt = @new_insurance_file_cnt
                            AND     pa2.agent_cnt = pa.agent_cnt
                            AND     pa2.agent_count = pa.agent_count)

	INSERT INTO 	event_policy_agents (
			insurance_file_cnt,
			agent_cnt,
			agent_count,
			agent_commission_percentage,
			agent_commission_amount,
			agent_commission_value,
			is_minimum_brokerage,
			override_rate_table,
			apply_perc_to_prem_or_comm)		
			SELECT 	@new_insurance_file_cnt, 
					pa.agent_cnt,  
					pa.agent_count,
					pa.agent_commission_percentage,
					pa.agent_commission_amount,
					pa.agent_commission_value,
					pa.is_minimum_brokerage,
					pa.override_rate_table,
					pa.apply_perc_to_prem_or_comm
			FROM		policy_agents pa
			WHERE 	insurance_file_cnt = @old_insurance_file_cnt
            --Check this record doesn't already exist
            AND     NOT EXISTS (SELECT  * 
                            FROM    event_policy_agents pa2
                            WHERE   pa2.insurance_file_cnt = @new_insurance_file_cnt
                            AND     pa2.agent_cnt = pa.agent_cnt
                            AND     pa2.agent_count = pa.agent_count)
END


