SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_tax_policy_agent_add'
GO

CREATE PROCEDURE spu_TXN_tax_policy_agent_add
(
@from_event bit,
@insurance_file_cnt int,
@policy_agents_id int,
@tax_group_id int,
@tax_rate numeric(19,4),
@tax_amount numeric(19,4)
)
AS

DECLARE @tax_band_id int
SELECT TOP 1 @tax_band_id=tax_band_id FROM tax_group_tax_band WHERE tax_group_id=@tax_group_id

DECLARE @calc_basis int
SELECT TOP 1 @calc_basis=calc_basis FROM tax_band_rate WHERE tax_band_id=@tax_band_id

if @from_event=0
	INSERT INTO tax_calculation
	(insurance_file_cnt, tax_group_id, tax_band_id, percentage, value, is_value, is_manually_changed, policy_agents_id, transtype, allow_tax_credit,calc_basis)
	VALUES
	(@insurance_file_cnt, @tax_group_id, @tax_band_id, @tax_rate, @tax_amount, 1, 0, @policy_agents_id, 'TTAC', 0,@calc_basis)
else
	INSERT INTO event_tax_calculation
	(insurance_file_cnt, tax_group_id, tax_band_id, percentage, value, is_value, is_manually_changed, policy_agents_id, transtype, allow_tax_credit,calc_basis)
	VALUES
	(@insurance_file_cnt, @tax_group_id, @tax_band_id, @tax_rate, @tax_amount, 1, 0, @policy_agents_id, 'TTAC', 0,@calc_basis)


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

