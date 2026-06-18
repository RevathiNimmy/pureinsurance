SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_tax_policy_agent_del'
GO

CREATE PROCEDURE spu_TXN_tax_policy_agent_del
(
@from_event bit,
@insurance_file_cnt int
)
AS

if @from_event=0
	DELETE FROM tax_calculation WHERE insurance_file_cnt=@insurance_file_cnt AND transtype='TTAC' AND policy_agents_id IS NOT NULL
else
	DELETE FROM event_tax_calculation WHERE insurance_file_cnt=@insurance_file_cnt AND transtype='TTAC' AND policy_agents_id IS NOT NULL
	
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

