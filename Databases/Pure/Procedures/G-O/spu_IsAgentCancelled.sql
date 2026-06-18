SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_IsAgentCancelled'
GO

-- return party_cnt if agent is cancelled
CREATE PROCEDURE spu_IsAgentCancelled
    @PartyCnt INT
AS

SELECT  party_cnt
FROM	Party_Agent
WHERE	party_cnt = @PartyCnt
AND	IsNull(date_cancelled,'1899-12-29') <> '1899-12-29'


GO


