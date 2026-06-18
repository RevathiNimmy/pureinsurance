SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_event_policy_agent_del'
GO

CREATE PROCEDURE spu_event_policy_agent_del
(@insurance_file_cnt int)
AS

DELETE FROM event_policy_agents WHERE insurance_file_cnt=@insurance_file_cnt

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO