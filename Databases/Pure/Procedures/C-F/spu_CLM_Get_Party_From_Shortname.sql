SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Party_From_Shortname'
GO

CREATE PROCEDURE spu_CLM_Get_Party_From_Shortname

@shortname varchar(20)

AS

BEGIN
	SELECT party_cnt 
	FROM party 
        WHERE shortname =@shortname
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
