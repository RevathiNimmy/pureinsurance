SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Base_Claim'
GO

CREATE PROCEDURE spu_CLM_Get_Base_Claim

@claim_id int

AS 

BEGIN


	SELECT base_Claim_id 
	FROM claim
	WHERE claim_id = @claim_id

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
