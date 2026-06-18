SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_party_agent_selall'
GO


CREATE PROCEDURE spu_party_agent_selall  

AS

SELECT
    *
FROM party_agent

GO