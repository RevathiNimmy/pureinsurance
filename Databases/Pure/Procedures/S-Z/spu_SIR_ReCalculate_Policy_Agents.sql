ddldropprocedure 'spu_SIR_ReCalculate_Policy_Agents'
go

CREATE PROCEDURE spu_SIR_ReCalculate_Policy_Agents
 	@insurance_file_cnt int
AS

BEGIN
declare @policy_agents_id integer
declare @premium numeric(19,4)
declare @agent_commission_percentage numeric(19,4)
declare @agent_commission_amount numeric(19,4)
declare @agent_commission_value numeric(19,4)
declare @tax_percentage numeric(19,4)
declare @tax_amount numeric(19,4)
declare @old_agent_commission_value numeric(19,4)
declare @old_tax_amount numeric(19,4)

--Get Premium Value (exc tax)
select @Premium = sum(premium_excluding_tax) from insurance_cob_section where insurance_file_cnt = @insurance_file_cnt

DECLARE CURSOR_Policy_Agents CURSOR STATIC FOR
	select policy_agents_id, agent_commission_percentage, agent_commission_amount, agent_commission_value, tax_amount from policy_agents where insurance_file_cnt=@insurance_file_cnt and agent_commission_percentage <> 0

OPEN CURSOR_Policy_Agents

FETCH NEXT FROM CURSOR_Policy_Agents INTO
  	@policy_Agents_id, @agent_commission_percentage, @agent_commission_amount, @old_agent_commission_value, @old_tax_amount

WHILE @@FETCH_STATUS = 0
BEGIN
	select @tax_percentage=0
	if @old_agent_commission_value <> 0 
	Begin
		select @tax_percentage = @old_tax_amount / @old_agent_commission_value
	End

	select @agent_commission_value = @agent_commission_amount + ((@agent_commission_percentage/100) * @premium)
	select @tax_amount = @agent_commission_value * @tax_percentage

	update 
		policy_agents
	set 
		agent_commission_value = round(@agent_commission_value,2),
		tax_amount = round(@tax_amount,2)
	where 
		policy_agents_id = @policy_agents_id

	FETCH NEXT FROM CURSOR_Policy_Agents INTO
		@policy_Agents_id, @agent_commission_percentage, @agent_commission_amount, @old_agent_commission_value, @old_tax_amount
END

CLOSE CURSOR_Policy_Agents
DEALLOCATE CURSOR_Policy_Agents

END