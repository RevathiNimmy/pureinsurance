SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_GetPartyByShortName'
GO

CREATE PROCEDURE spu_GetPartyByShortName
	@ShortName varchar(20)
 AS

SELECT party_cnt FROM party WITH(NOLOCK) WHERE shortname = @ShortName

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

