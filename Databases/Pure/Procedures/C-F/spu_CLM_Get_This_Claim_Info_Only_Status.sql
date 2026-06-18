SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_This_Claim_Info_Only_Status'
GO

CREATE PROCEDURE spu_CLM_Get_This_Claim_Info_Only_Status

@claim_id int

AS

BEGIN
	SELECT Info_only 
	FROM claim 
	WHERE claim_id = @claim_id
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
