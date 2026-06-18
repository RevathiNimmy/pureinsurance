SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Claim_Check_BaseClaimPerilkey'
GO

CREATE PROCEDURE spu_SAM_Claim_Check_BaseClaimPerilkey
     @BaseClaimPerilKey int,
@count int OUTPUT
AS
SELECT @count=count(*) FROM Claim_peril 

WHERE base_claim_Peril_id = @BaseClaimPerilKey

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 

GO