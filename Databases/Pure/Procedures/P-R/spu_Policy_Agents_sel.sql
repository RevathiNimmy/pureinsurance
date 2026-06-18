SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Policy_Agents_sel'
GO

CREATE PROCEDURE spu_Policy_Agents_sel  
 	@insurance_file_cnt int
AS  
BEGIN  
	SELECT insurance_file_cnt, 
		   agent_cnt,
		   agent_count,
		   agent_commission_percentage,
		   agent_commission_amount,
		   agent_commission_value,
		   is_minimum_brokerage,
		   override_rate_table, 
		   apply_perc_to_prem_or_comm
	FROM	policy_agents
	WHERE insurance_file_cnt = @insurance_file_cnt
END  

GO