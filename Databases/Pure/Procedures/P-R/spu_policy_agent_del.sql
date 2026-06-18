SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_policy_agent_del'
GO

CREATE PROCEDURE spu_policy_agent_del
(@insurance_file_cnt int)
AS

DELETE FROM policy_agents WHERE insurance_file_cnt=@insurance_file_cnt

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

