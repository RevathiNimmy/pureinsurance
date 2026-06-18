SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Event_Policy_Agent_add'
GO


CREATE PROCEDURE spu_Event_Policy_Agent_add
 	@insurance_file_cnt int, 
	@agent_cnt int,
 	@agent_commission_percentage numeric(19,4),
 	@agent_commission_amount numeric(19,4),
	@agent_commission_value numeric(19,4),
	@is_minimum_brokerage tinyint,
	@override_rate_table tinyint,
	@apply_perc_to_prem_or_comm tinyint,
	@base_currency_id smallint,
	@tax_amount numeric(19,4)
AS
BEGIN
DECLARE @agent_count as int
/* Insert the values */

SELECT @agent_count = 1
SELECT @agent_count =  agent_count + 1  FROM  event_policy_agents WHERE EXISTS  ( select agent_count from event_policy_agents
								where insurance_file_cnt = @insurance_file_cnt
								AND agent_cnt = @agent_cnt )
INSERT INTO Event_Policy_Agents
( insurance_file_cnt,agent_cnt,agent_count,agent_commission_percentage,agent_commission_amount,agent_commission_value,is_minimum_brokerage ,override_rate_table, apply_perc_to_prem_or_comm,base_currency_id,tax_amount)
VALUES
(  @insurance_file_cnt,@agent_cnt,@agent_count,@agent_commission_percentage,@agent_commission_amount,@agent_commission_value,@is_minimum_brokerage ,@override_rate_table, @apply_perc_to_prem_or_comm,@base_currency_id,@tax_amount) 
END

SELECT @@IDENTITY AS id

GO
 

