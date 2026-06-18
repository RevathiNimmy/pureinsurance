SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Claim_Check_BaseClaimkey'
GO

CREATE PROCEDURE spu_SAM_Claim_Check_BaseClaimkey
     @BaseClaimKey int,
@count int OUTPUT
AS
SELECT @count=count(*) FROM Claim 

WHERE base_claim_id = @BaseClaimKey

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO