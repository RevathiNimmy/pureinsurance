SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_agent_type_sel'
GO

CREATE PROCEDURE spu_TXN_agent_type_sel
(@party_cnt int)
AS

SELECT
PAT.code,
PAT.description
FROM
party_agent PA
INNER JOIN party_agent_type PAT ON PA.party_agent_type_id=PAT.party_agent_type_id
WHERE
PA.party_cnt=@party_cnt

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

