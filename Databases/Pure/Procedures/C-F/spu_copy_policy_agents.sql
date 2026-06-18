EXECUTE DDLDropProcedure 'spu_copy_policy_agents'
GO

CREATE PROCEDURE spu_copy_policy_agents
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS


INSERT INTO policy_agents (
        insurance_file_cnt,
        agent_cnt,
        agent_count,
        agent_commission_percentage,
        agent_commission_amount,
        agent_commission_value,
        is_minimum_brokerage,
        override_rate_table,
        apply_perc_to_prem_or_comm)
    SELECT  @NewInsuranceFileCnt,
        agent_cnt,
        agent_count,
        0,
        0,
        0,
        is_minimum_brokerage,
        override_rate_table,
        apply_perc_to_prem_or_comm
    FROM    policy_agents
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt
	

EXEC spu_copy_sub_agent @OldInsuranceFileCnt=@OldInsuranceFileCnt,@NewInsuranceFileCnt=@NewInsuranceFileCnt

GO