SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_CLM_Get_Document_Generated_Status'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Document_Generated_Status
     @Claimkey int
AS
SELECT Document_Generated_Status FROM Claim 

WHERE Claim_id = @Claimkey

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
